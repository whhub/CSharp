using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UIH.Mcsf.App.Common;
using UIH.Mcsf.Filming.Interface;
using UIH.Mcsf.Filming.Widgets;
using UIH.Mcsf.Filming.Wrapper;

namespace UIH.Mcsf.Filming.DataModel
{
    [CallTrace(true)]
    public class CellLink : List<CellBase>
    {
        private int _focus = -1;
        //private int _lastSelected = -1;
        private LayoutBase _layout;
        public LayoutBase Layout
        {
            set
            {
                if (_layout.Equals(value)) return;
                _layout = value;
                Polish();
            }
        }
        
        public event EventHandler Changed = delegate { };

        public CellLink(LayoutBase layout)
        {
            _layout = layout;
            AddRange(CellFactory.Instance.CreateCells(_layout.Capacity));
        }

        public int PageCount
        {
            get { return Count / _layout.Capacity; }
        }

        public int Focus
        {
            get { return _focus; }
            set
            {
                if(_focus == value) return;
                var oldFocusCell = this.ElementAtOrDefault(_focus);
                if (oldFocusCell != null) oldFocusCell.IsFocused = false;
                var newFocusCell = this.ElementAtOrDefault(value);
                if (newFocusCell != null) newFocusCell.IsFocused = true;
                _focus = value;
            }
        }

        public void AddSeries(string seriesUid, int index = 0)
        {

            var newCells =
                DBWrapperHelper.DBWrapper.GetImageListBySeriesInstanceUID(seriesUid).Select(
                    image => CellFactory.Instance.CreateCell(image)).ToList();
            TrimBegin(index, newCells.Count());

            InsertRange(index, newCells);

            Polish();

        }

        private void TrimBegin(int index, int count)
        {
            int nonEmptyIndex = FindIndex(index, cell => !cell.IsEmpty);
            if (nonEmptyIndex == -1) nonEmptyIndex = Count;
            int emptyCellCount = nonEmptyIndex - index;
            int toBeRemovedCellCount = Math.Min(emptyCellCount, count);
            RemoveRange(index, toBeRemovedCellCount);
        }

        private void Polish()
        {
            TrimEnd();
            Complement();
            Changed(this, new EventArgs());
        }

        private void Complement()
        {
            int pageCellCount = _layout.Capacity;
            var lastPageCellCount = Count % pageCellCount;
            if (lastPageCellCount != 0)
                AddRange(CellFactory.Instance.CreateCells(pageCellCount - lastPageCellCount));
        }

        private void TrimEnd()
        {
            var emptyRangeStart = FindLastIndex(cell => !cell.IsEmpty);
            if (emptyRangeStart == -1) emptyRangeStart += _layout.Capacity;  //we have to leave at lease an empty page
            if (emptyRangeStart > Count - 1) return;
            RemoveRange(emptyRangeStart + 1, Count - emptyRangeStart - 1);
        }

        public IList<CellBase> GetCells(int pageIndex)
        {
            var pageCellCount = _layout.Capacity;
            var cellIndex = pageIndex * pageCellCount;
            if (cellIndex >= Count) return null;
            return GetRange(cellIndex, pageCellCount);
        }

        public IEnumerable<Page> BuildPages()
        {
            var pageCellCount = _layout.Capacity;
            var pageCount = Count / pageCellCount;
            for (int cellIndex = 0, pageIndex = 0; cellIndex < Count; cellIndex += pageCellCount, pageIndex++)
            {
                yield return new Page(GetRange(cellIndex, pageCellCount), pageIndex, pageCount, _layout);
            }
        }

        #region [--Override--]

        private new void AddRange(IEnumerable<CellBase> cells)
        {
            var cellBases = cells as List<CellBase> ?? cells.ToList();
            cellBases.ForEach(cell=>
                                      {
                                          cell.Clicked -= CellOnClicked;
                                          cell.Clicked += CellOnClicked;
                                      });
            base.AddRange(cellBases);
        }

        private new void RemoveRange(int index, int count)
        {
            var cells = GetRange(index, count);
            cells.ForEach(cell=> cell.Clicked -= CellOnClicked);
            base.RemoveRange(index, count);
        }

        private void CellOnClicked(object sender, ClickStatusEventArgs clickStatusEventArgs)
        {
            var isLeftMouseButtonClicked = clickStatusEventArgs.IsLeftMouseButtonClicked;
            var isRightMouseButtonClicked = clickStatusEventArgs.IsRightMouseButtonClicked;
            var isCtrlPressed = clickStatusEventArgs.IsCtrlPressed;
            var isShiftPressed = clickStatusEventArgs.IsShiftPressed;

            var operationCell = sender as CellBase;
            Debug.Assert(operationCell != null);

            //0. 焦点
            var operationCellIndex = IndexOf(operationCell);
            var lastFocus = Focus;

            //1. 仅仅右键按下
            if (isRightMouseButtonClicked && !isLeftMouseButtonClicked)
            {
                if(isCtrlPressed && !isShiftPressed) return;    //仅仅ctrl按下， 不改变选中状态
                if(operationCell.IsSelected) return;                //作用在选中的cell上，不改变选中状态
                //operationCell未选中                               //作用在未选中的cell上，仅仅选中该cell
                SelectOnly(operationCell);
                return;
            }

            //2. 有左键按下           


            //2.1 ctrl+shift按下
            if (isShiftPressed && isCtrlPressed)  
            {
                //todo: 扩展到Card范围
                var lastFocusCell = this.ElementAtOrDefault(lastFocus);
                SelectRange(lastFocus, operationCellIndex, lastFocusCell != null && lastFocusCell.IsSelected);

            }
            //2.2 仅仅 ctrl 按下
            else if (isShiftPressed)
            {
                ForEach(cell=>cell.IsSelected = false);
                SelectRange(lastFocus, operationCellIndex, true);
            } 
            //2.3 仅仅 ctrl 按下
            else if (isCtrlPressed)
            {

                operationCell.IsSelected = !operationCell.IsSelected;
                Focus = operationCellIndex;
            }
            //2.4 没有modifier key 按下
            else 
            {
                SelectOnly(operationCell);
            }
        }

        //会引起Focus变更
        private void SelectOnly(CellBase operationCell)
        {
            //todo:取消所有选中状态
            ForEach(cell => cell.IsSelected = false);
            operationCell.IsSelected = true;
            Focus = IndexOf(operationCell);
        }

        private void SelectRange(int index1, int index2, bool isSelected)
        {
            var first = Math.Min(index1, index2);
            var last = Math.Max(index1, index2);

            if (first < 0 || last >= Count)
            {
                Logger.Instance.LogDevWarning(string.Format("Select Out of Range[{0}:{1}]", first, last));
                return;;
            }

            for (int i = first; i <= last; i++)
            {
                this[i].IsSelected = isSelected;
            }
        }

        #endregion [--Override--]
    }
}
