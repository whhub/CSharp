using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UserControlTest
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            SetGrid(1, 1);
        }

        private void SetGrid(int row, int col)
        {
            #region Add Or Remove Row and Col

            var rows = _grid.RowDefinitions;
            var curRow = rows.Count;
            var cols = _grid.ColumnDefinitions;
            var curCol = cols.Count;

            var rowDelta = row - curRow;
            var colDelta = col - curCol;

            for (var i = 0; i < rowDelta; i++)
            {
                rows.Add(new RowDefinition());
            }
            for (var i = 0; i < colDelta; i++)
            {
                cols.Add(new ColumnDefinition());
            }
            if (rowDelta < 0) rows.RemoveRange(row, -rowDelta);
            if (colDelta < 0) cols.RemoveRange(col, -colDelta);

            #endregion Add Or Remove Row and Col

            #region Add Or Remove Elements

            var elementCount = row*col;
            var curElementCount = curRow*curCol;
            var elementDelta = elementCount - curElementCount;
            for (var i = 0; i < elementDelta; i++)
            {
                _grid.Children.Add(PageFactory.Instance.GetPage());
            }
            for (var i = curElementCount - 1; i >= elementCount; i--)
            {
                PageFactory.Instance.ReservePage(_grid.Children[i] as Page);
            }
            if (elementDelta < 0) _grid.Children.RemoveRange(elementCount, -elementDelta);

            #endregion Add Or Remove Elements

            #region Refresh Element Location

            for (int i = 0, elementIndex = 0; i < row; i++)
            {
                for (var j = 0; j < col; j++)
                {
                    var uiElement = _grid.Children[elementIndex];
                    Grid.SetRow(uiElement, i);
                    Grid.SetColumn(uiElement, j);
                    var page = uiElement as Page;
                    elementIndex++;
                    page.Text = elementIndex.ToString();
                }
            }

            #endregion Refresh Element Location
        }

        private void Button1Click(object sender, RoutedEventArgs e)
        {
            SetGrid(1, 1);
        }

        private void Button2Click(object sender, RoutedEventArgs e)
        {
            SetGrid(1, 2);
        }

        private void Button3Click(object sender, RoutedEventArgs e)
        {
            SetGrid(1, 3);
        }

        private void Button4Click(object sender, RoutedEventArgs e)
        {
            SetGrid(2, 2);
        }

        private void Button6Click(object sender, RoutedEventArgs e)
        {
            SetGrid(2, 3);
        }

        private void Button8Click(object sender, RoutedEventArgs e)
        {
            SetGrid(2, 4);
        }

        private Grid GetGrid(DependencyObject dependencyObject, string name)
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
            {
                var child = VisualTreeHelper.GetChild(dependencyObject, i);
                var grid = child as Grid;
                if (grid != null && grid.Name == name) return grid;

                var grandChild = GetGrid(child, name);
                if (grandChild != null) return grandChild;
            }
            return null;
        }
    }
}