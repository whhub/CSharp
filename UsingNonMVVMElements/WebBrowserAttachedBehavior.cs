using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace UsingNonMVVMElements
{
    public class WebBrowserAttachedBehavior
    {
        public static DependencyProperty SourcePageProperty =
            DependencyProperty.RegisterAttached("SourcePage", typeof (string), typeof (WebBrowserAttachedBehavior),
                new PropertyMetadata("", OnSourcePagePropertyChanged));

        public static string GetSourcePage(DependencyObject obj)
        {
            return (string) obj.GetValue(SourcePageProperty);
        }

        public static void SetSourcePage(DependencyObject obj, string value)
        {
            obj.SetValue(SourcePageProperty, value);
        }
        private static void OnSourcePagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null && d is WebBrowser)
            {
                var webBrowser = (WebBrowser) d;
                webBrowser.Source = new Uri(e.NewValue.ToString());
            }
        }
    }
}
