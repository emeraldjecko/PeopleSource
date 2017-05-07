using System;

namespace PeoplesSource.Domain
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class PagerParams
    {
        /// <summary>
        /// Page size
        /// </summary>
        private int _pageSize;
        /// <summary>
        /// Total record count
        /// </summary>
        private int _totalRecords;
        /// <summary>
        /// Page index
        /// </summary>
        private int _pageIndex;

        /// <summary>
        /// Gets the size of the page.
        /// </summary>
        /// <value>The size of the page.</value>
        public int PageSize
        {
            get { return _pageSize; }
        }

        /// <summary>
        /// Gets or sets the index of the page.
        /// </summary>
        /// <value>The index of the page.</value>
        public int PageIndex
        {
            get { return _pageIndex; }
            set
            {
                if (_pageSize == 0 && value > 0)
                {
                    throw new ArgumentOutOfRangeException("error");
                }
                if (_totalRecords > 0)
                {
                    if (value * _pageSize >= _totalRecords)
                    {
                        throw new ArgumentOutOfRangeException("error");
                    }
                }
                else if (_totalRecords == 0)
                {
                    if (value > 0)
                    {
                        throw new ArgumentOutOfRangeException("error");
                    }
                }
                _pageIndex = value;
            }
        }

        /// <summary>
        /// Gets or sets the total records.
        /// </summary>
        /// <value>The total records.</value>
        public int TotalRecords
        {
            get { return _totalRecords; }
            set
            {
                if (_totalRecords != value)
                {
                    _totalRecords = value;
                    if (_totalRecords > 0)
                    {
                        if (_pageIndex * _pageSize >= _totalRecords)
                        {
                            PageIndex = (_totalRecords - 1) / _pageSize;
                        }
                    }
                    else if (_totalRecords == 0)
                    {
                        PageIndex = 0;
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalRecords"></param>
        public PagerParams(int pageSize, int pageIndex, int totalRecords)
        {
            _pageSize = pageSize;
            _totalRecords = totalRecords;
            PageIndex = pageIndex;
        }

        /// <summary>
        /// Gets the default pager parameters.
        /// </summary>
        /// <value>The default pager parameters.</value>
        public static PagerParams Default
        {
            get
            {
                return new PagerParams(20, 0, -1);
            }
        }

        /// <summary>
        /// Gets the no paging parameters.
        /// </summary>
        /// <value>The no paging parameters.</value>
        public static PagerParams NoPaging
        {
            get
            {
                return new PagerParams(int.MaxValue, 0, -1);
            }
        }
    }
}
