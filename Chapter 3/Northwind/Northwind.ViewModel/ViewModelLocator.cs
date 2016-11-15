using Northwind.Application;

namespace Northwind.ViewModel
{
    public class ViewModelLocator
    {
        public static MainWindowViewModel MainWindowViewModelStatic
            = new MainWindowViewModel(new UIDataProvider(), new ToolManager());
    }
}