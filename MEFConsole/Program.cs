////////////////////////////////////////////

#pragma warning disable 1587
/// Ref: http://www.cnblogs.com/beniao/archive/2010/07/03/1770276.html
#pragma warning restore 1587
////////////////////////////////////////////
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

namespace MEFConsole
{
    public interface IBookService
    {
        void GetBookName();
    }

    /// <summary>
    ///     Export
    /// </summary>
    [Export(typeof (IBookService))]
    public class ComputerBookService : IBookService
    {
        public void GetBookName()
        {
            Console.WriteLine("<<Hello MEF>>");
        }
    }

    internal class Program
    {
        /// <summary>
        ///     import part
        /// </summary>
        [Import]
        public IBookService Service { get; set; }

        /// <summary>
        ///     Host MEF & Compose parts
        /// </summary>
        private void Compose()
        {
            var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            var container = new CompositionContainer(catalog);

            //将部件（part）和宿主程序添加到组合容器
            container.ComposeParts(this, new ComputerBookService());
        }

        private static void Main()
        {
            var p = new Program();
            p.Compose();

            p.Service.GetBookName();
        }
    }
}