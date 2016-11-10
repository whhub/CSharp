namespace Northwind.ViewModel
{
    public class ToolViewModel
    {
        public string DisplayName { get; set; }
    }

    public class AToolViewModel : ToolViewModel
    {
        public AToolViewModel()
        {
            DisplayName = "A";
        }
    }

    public class BToolViewModel : ToolViewModel
    {
        public BToolViewModel()
        {
            DisplayName = "B";
        }
    }
}