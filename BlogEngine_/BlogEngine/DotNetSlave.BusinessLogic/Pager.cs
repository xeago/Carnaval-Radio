using System.Collections.Generic;

namespace BlogEngine.Core
{
    public class Pager
    {
        private static int _currentPage;
        private static int _itemCount;

        /// <summary>
        /// Pager
        /// </summary>
        /// <param name="currentPage">Current page</param>
        /// <param name="itemCount">Item count</param>
        public Pager(int currentPage, int itemCount)
        {
            _currentPage = currentPage;
            _itemCount = itemCount;
        }

        /// <summary>
        /// Current page
        /// </summary>
        public int CurrentPage
        {
            get { return _currentPage; }
        }

        /// <summary>
        /// Pager items
        /// </summary>
        public List<PagerItem> PageItems
        {
            get
            {
                var items = new List<PagerItem>();

                for (int i = 1; i <= (int)(_itemCount / BlogSettings.Instance.PostsPerPage) + 1; i++)
                {
                    items.Add(i == _currentPage ? new PagerItem(i, true) : new PagerItem(i, false));
                }
                return items;
            }
        }
    }

    /// <summary>
    /// Page Item
    /// </summary>
    public class PagerItem
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="current"></param>
        public PagerItem(int pageNumber, bool current)
        {
            PageNumber = pageNumber;
            Current = current;
        }
        /// <summary>
        /// Page number
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// Current selected page
        /// </summary>
        public bool Current { get; set; }
    }
}
