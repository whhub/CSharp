using System;
using System.Linq;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace EIMPrint
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }

        #region [--PrintCommand--]

        private ICommand _printCommand;

        public ICommand PrintCommand
        {
            get { return _printCommand = _printCommand ?? new RelayCommand<WrapPanel>(Print); }
        }

        private void Print(WrapPanel obj)
        {
            GetPrintDialog();

            foreach (var image in obj.Children.OfType<Image>())
            {
                //var rtb = RenderTargetBitmap(image);

                //var label = new Label
                //{
                //    Background = new SolidColorBrush(Color.FromRgb(28, 41, 48)),
                //    Content = new Image() { Source = rtb, Stretch = Stretch.UniformToFill }
                //};

                //var visualBrush = new VisualBrush(label);
                var visualBrush = new ImageBrush(image.Source);
                double margin = 5;
                var printRect = GetPrintRect(image.RenderSize, margin);

                var drawingVisual = new DrawingVisual();
                using (DrawingContext context = drawingVisual.RenderOpen())
                {
                    context.DrawRectangle(visualBrush, null, printRect);
                }
                //_printDialog.PrintVisual(image as Visual, image.ToString());
                _printDialog.PrintVisual(drawingVisual, image.Name);
            }
        }



        #endregion [--PrintCommand--]

        private Rect GetPrintRect(Size printObjectSize, double margin)
        {
            var printDestinationSize = new Size(_printDialog.PrintableAreaWidth, _printDialog.PrintableAreaHeight);
            var printSize = CalculatePrintSize(printObjectSize, printDestinationSize);

            var printPoint = new Point(margin, margin);
            return new Rect(printPoint, printSize);
        }

        private Size CalculatePrintSize(Size printObjectSize, Size printDestinationSize)
        {
            return printObjectSize.IsThinThan(printDestinationSize)
                ? new Size(printObjectSize.Width * printDestinationSize.Height / printObjectSize.Height, printDestinationSize.Height)
                : new Size(printDestinationSize.Width, printObjectSize.Height * printDestinationSize.Width / printObjectSize.Width);
        }

        private PrintDialog _printDialog;

        private void GetPrintDialog()
        {
            var localPrintServer = new LocalPrintServer();
            var localPrinterQueueCollection = localPrintServer.GetPrintQueues();
            var printerQueue = localPrinterQueueCollection.FirstOrDefault();
            PrintTicket printTicket = null;
            if (null != printerQueue) printTicket = printerQueue.DefaultPrintTicket;

            _printDialog = new PrintDialog
            {
                PrintTicket = printTicket ?? new PrintTicket
                {
                    PageOrientation = PageOrientation.Portrait, //TODO-Print-Configuration : PageOrientation 
                    CopyCount = 1,
                    PageMediaSize = new PageMediaSize(PageMediaSizeName.ISOA4) //TODO-Print-Configuration : PageMediaSize
                }
            };
        }

        private RenderTargetBitmap RenderTargetBitmap(FrameworkElement visual)
        {
            var rtb = new RenderTargetBitmap((int)Math.Round(visual.ActualWidth), (int)Math.Round(visual.ActualHeight),
                96, 96, PixelFormats.Pbgra32);
            var dv = new DrawingVisual();
            using (DrawingContext context = dv.RenderOpen())
            {
                var vb = new VisualBrush(visual);
                context.DrawRectangle(vb, null, new Rect(new Point(0, 0), new Size(visual.ActualWidth, visual.ActualHeight)));
            }
            rtb.Render(dv);
            return rtb;
        }
    }

    public static class SizeExtension
    {
        public static bool IsThinThan(this Size sizeA, Size sizeB)
        {
            return sizeA.Width / sizeA.Height < sizeB.Width / sizeB.Height;
        }
    }
}