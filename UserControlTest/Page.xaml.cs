using System.Collections.Generic;
using System.Linq;

namespace UserControlTest
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Page
    {

        public Page()
        {
            InitializeComponent();
        }

        public string Text { set { _textBlock.Text = value; } }

    }

    public class PageFactory
    {
        #region [--Singleton--]

        private static volatile PageFactory _instance;
        private static readonly object LockHelper = new object();

        private PageFactory()
        {
            _resolvedPages = new List<Page>();
        }

        public static PageFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (LockHelper)
                    {
                        if (_instance == null)
                            _instance = new PageFactory();
                    }
                }
                return _instance;
            }
        }

        #endregion //[--Singleton--]

        
        private readonly List<Page> _resolvedPages;

        public Page GetPage()
        {
            var count = _resolvedPages.Count;
            if(count==0) return new Page();
            var page = _resolvedPages[count - 1];
            _resolvedPages.RemoveAt(count - 1);
            return page;
        }

        public void ReservePage(Page page)
        {
            _resolvedPages.Add(page);
        }
    }

}
