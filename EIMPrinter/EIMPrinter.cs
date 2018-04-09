using System;
using System.Linq;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EIMPrinter
{
    public class EIMPrinter
    {

        public EIMPrinter()
        {
            GetPrintDialog();
        }

        public void Print(FrameworkElement userControl)
        {
            var visualBrush = new VisualBrush(userControl);
            var printRect = new Rect(new Point(0, 0), new Size(_printDialog.PrintableAreaWidth, _printDialog.PrintableAreaHeight));

            var drawingVisual = new DrawingVisual();
            using (DrawingContext context = drawingVisual.RenderOpen())
            {
                context.DrawRectangle(visualBrush, null, printRect);
            }
            _printDialog.PrintVisual(drawingVisual, DateTime.Now.ToString());
        }

        /// <summary>
        /// Print Images by url
        /// </summary>
        /// <param name="urls">image url such as @"e:\Male.png"</param>
        public void Print(params string[] urls)
        {
            //try
            //{
                foreach (var url in urls)
                {
                    var image = new BitmapImage(new Uri(url, UriKind.RelativeOrAbsolute));

                    var label = new Label
                    {
                        Content = new Image() {Source = image, Stretch = Stretch.UniformToFill}
                    };

                    var visualBrush = new VisualBrush(label);

                    var imageSize = new Size(image.Width, image.Height);
                    //var printRect = GetPrintRect(imageSize, _margin);
                    var printRect = new Rect(new Point(0,0), new Size(_printDialog.PrintableAreaWidth, _printDialog.PrintableAreaHeight));

                    var drawingVisual = new DrawingVisual();
                    using (DrawingContext context = drawingVisual.RenderOpen())
                    {
                        //context.DrawRectangle(visualBrush, null, printRect);
                        context.DrawImage(image, printRect);
                    }

                    //double scale = GetImmuteScale(printRect.Size);
                    //drawingVisual.Transform = new ScaleTransform(imageSize.Width/_printDialog.PrintableAreaWidth, imageSize.Height/_printDialog.PrintableAreaHeight);

                    _printDialog.PrintVisual(drawingVisual, DateTime.Now.ToString());
                }
            //}
            //catch(Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}
        }

        private double GetImmuteScale(Size printSize)
        {
            return Math.Min(_printDialog.PrintableAreaWidth / (printSize.Width + _margin * 2),
                _printDialog.PrintableAreaHeight / (printSize.Height + _margin * 2));
        }

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
                ? new Size(printObjectSize.Width*printDestinationSize.Height/printObjectSize.Height,
                    printDestinationSize.Height)
                : new Size(printDestinationSize.Width,
                    printObjectSize.Height*printDestinationSize.Width/printObjectSize.Width);
        }

        private PrintDialog _printDialog;
        private int _margin = 0;

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
                    PageOrientation = PageOrientation.Portrait, 
                    CopyCount = 1,
                    PageMediaSize = new PageMediaSize(PageMediaSizeName.ISOA4)
                }
            };
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
