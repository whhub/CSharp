////////////////////////////////

#pragma warning disable 1587
/// Ref：http://www.cnblogs.com/xiaokang088/archive/2012/02/21/2361631.html
#pragma warning restore 1587
////////////////////////////////

using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using ILog;

namespace MEFConsoleHost
{
    internal class Program
    {
        [Import(typeof (ILogService))]
        public ILogService CurrentLogService { get; set; }

        private void Compose()
        {
            var catalog = new DirectoryCatalog(AppDomain.CurrentDomain.BaseDirectory, "FileLog.dll");
            //var catalog = new DirectoryCatalog(AppDomain.CurrentDomain.BaseDirectory, "TextLog.dll");
            var container = new CompositionContainer(catalog);

            container.ComposeParts(this);
        }

        private static void Main()
        {
            var p = new Program();
            p.Compose();

            p.CurrentLogService.Log("MEF Log For Console");
        }
    }
}