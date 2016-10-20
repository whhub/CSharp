using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace UIH.Mcsf.Filming.DataModel
{
    public interface ISelect
    {
        bool IsSelected { get; set; }
        bool IsFocused { get; set; }
        event EventHandler<BoolEventArgs> SelectedChanged;
        event EventHandler<BoolEventArgs> FocusedChanged;
    }

    public class SelectableList<T> : List<T> where T : ISelect
    {
        private int _focus;
        private int Focus
        {
            get { return _focus; }
            set
            {
                if(_focus == value) return;
                var oldFocusElement = this.ElementAtOrDefault(_focus);
                if (oldFocusElement != null) oldFocusElement.IsFocused = false;
                var newFocusElement = this.ElementAtOrDefault(value);
                if (newFocusElement != null) newFocusElement.IsFocused = true;
                _focus = value;
            }
        }

        protected SelectableList()
        {
            _focus = -1;
        }

        protected void ClickOn(T operationElement, ClickStatus clickStatus)
        {
            Debug.Assert(operationElement != null);

            //0. 焦点
            var operationElementIndex = IndexOf(operationElement);
            var lastFocus = Focus;
            var lastFocusElement = this.ElementAtOrDefault(lastFocus);

            //1. 仅仅右键按下
            if (clickStatus.IsRightMouseButtonClicked && !clickStatus.IsLeftMouseButtonClicked)
            {
                if (clickStatus.IsCtrlPressed && !clickStatus.IsShiftPressed) return; //仅仅ctrl按下， 不改变选中状态
                if (operationElement.IsSelected) return; //作用在选中的cell上，不改变选中状态
                //operationCell未选中                               //作用在未选中的cell上，仅仅选中该cell
                SelectOnly(operationElement);
                return;
            }

            //2. 有左键按下           


            //2.1 ctrl+shift按下
            if (clickStatus.IsShiftPressed && clickStatus.IsCtrlPressed)
            {
                //todo: 扩展到Card范围
                SelectRange(lastFocus, operationElementIndex, lastFocusElement != null && lastFocusElement.IsSelected);

            }
                //2.2 仅仅 ctrl 按下
            else if (clickStatus.IsShiftPressed)
            {
                ForEach(cell => cell.IsSelected = false);
                SelectRange(lastFocus, operationElementIndex, true);
            }
                //2.3 仅仅 ctrl 按下
            else if (clickStatus.IsCtrlPressed)
            {

                operationElement.IsSelected = !operationElement.IsSelected;
                Focus = operationElementIndex;
            }
                //2.4 没有modifier key 按下
            else
            {
                SelectOnly(operationElement);
            }
        }

        //会引起Focus变更
        private void SelectOnly(T element)
        {
            //todo:取消所有选中状态
            ForEach(e => e.IsSelected = false);
            element.IsSelected = true;
            Focus = IndexOf(element);
        }

        private void SelectRange(int index1, int index2, bool isSelected)
        {
            var first = Math.Min(index1, index2);
            var last = Math.Max(index1, index2);

            if (first < 0 || last >= Count)
            {
                Logger.Instance.LogDevWarning(string.Format("Select Out of Range[{0}:{1}]", first, last));
                return;
                ;
            }

            for (int i = first; i <= last; i++)
            {
                this[i].IsSelected = isSelected;
            }

        }
    }

    public class ClickStatus
    {
        public ClickStatus(bool isLeftMouseButtonClicked, bool isRightMouseButtonClicked, bool isCtrlPressed,
                           bool isShiftPressed)
        {
            IsLeftMouseButtonClicked = isLeftMouseButtonClicked;
            IsRightMouseButtonClicked = isRightMouseButtonClicked;
            IsCtrlPressed = isCtrlPressed;
            IsShiftPressed = isShiftPressed;
        }

        public bool IsLeftMouseButtonClicked { get; private set; }
        public bool IsRightMouseButtonClicked { get; private set; }
        public bool IsCtrlPressed { get; private set; }
        public bool IsShiftPressed { get; private set; }
    }
}