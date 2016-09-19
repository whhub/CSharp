using System.Windows;
using System.Windows.Controls;

namespace UserControlTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SetGrid(int row, int col)
        {
            var rows = _grid.RowDefinitions;
            int curRow = rows.Count;
            var cols = _grid.ColumnDefinitions;
            int curCol = cols.Count;

            int rowDelta = row - curRow;
            int colDelta = col - curCol;

            for (int i = 0; i < rowDelta; i++)
            {
                rows.Add(new RowDefinition());
            }
            for (int i = 0; i < colDelta; i++)
            {
                cols.Add(new ColumnDefinition());
            }
            if(rowDelta < 0) rows.RemoveRange(row, -rowDelta);
            if(colDelta < 0) cols.RemoveRange(col, -colDelta);
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
    }
}
