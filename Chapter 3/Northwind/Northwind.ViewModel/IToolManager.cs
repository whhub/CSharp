using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace Northwind.ViewModel
{
    public interface IToolManager
    {
        ObservableCollection<ToolViewModel> Tools { get; set; }
        void OpenTool<T>(Func<T, bool> predicate, Func<T> toolFactory) where T : ToolViewModel;
        void CloseTool(ToolViewModel tool);
    }

    public class ToolManager : IToolManager
    {
        private readonly ICollectionView _toolCollectionView;

        public ToolManager()
        {
            Tools = new ObservableCollection<ToolViewModel>();
            _toolCollectionView = CollectionViewSource.GetDefaultView(Tools);
        }

        private void SetCurrentTool(ToolViewModel tool)
        {
            if (_toolCollectionView.MoveCurrentTo(tool) != true)
                throw new InvalidOperationException("Could not find the current tool.");
        }

        #region Implementation of IToolManager

        public ObservableCollection<ToolViewModel> Tools { get; set; }

        public void OpenTool<T>(Func<T, bool> predicate, Func<T> toolFactory) where T : ToolViewModel
        {
            var tool = Tools.Where(t => t.GetType() == typeof (T)).FirstOrDefault(t => predicate.Invoke((T) t));
            if (tool == null)
            {
                tool = toolFactory.Invoke();
                Tools.Add(tool);
                SetCurrentTool(tool);
            }
        }

        public void CloseTool(ToolViewModel tool)
        {
            Tools.Remove(tool);
        }

        #endregion
    }
}