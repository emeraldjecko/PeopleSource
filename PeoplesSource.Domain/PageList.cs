using System;

namespace PeoplesSource.Domain
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageList<T> : IPageList
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagerParams"></param>
        public PageList(PagerParams pagerParams)
        {
            Initialize(pagerParams);
        }
 
        #region IPagedList Members
 
        /// <summary>
        /// 
        /// </summary>
        public int PageCount { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int TotalItemCount { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int PageIndex { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int PageNumber { get { return PageIndex + 1; } }
        /// <summary>
        /// 
        /// </summary>
        public int PageSize { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public bool HasPreviousPage { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public bool HasNextPage { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsFirstPage { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsLastPage { get; private set; }
 
        #endregion
 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagerParams"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected void Initialize(PagerParams pagerParams)
        {
            //### argument checking
            if (pagerParams.PageIndex < 0)
            {
                throw new ArgumentOutOfRangeException("PageIndex cannot be below 0.");
            }
            if (pagerParams.PageSize < 1)
            {
                throw new ArgumentOutOfRangeException("PageSize cannot be less than 1.");
            }
            
 
            //### set properties          
            TotalItemCount = pagerParams.TotalRecords;
            PageSize = pagerParams.PageSize;
            PageIndex = pagerParams.PageIndex;
            if (TotalItemCount > 0)
            {
                PageCount = (int)Math.Ceiling(TotalItemCount / (double)PageSize);
            }
            else
            {
                PageCount = 0;
            }
            HasPreviousPage = (PageIndex > 0);
            HasNextPage = (PageIndex < (PageCount - 1));
            IsFirstPage = (PageIndex <= 0);
            IsLastPage = (PageIndex >= (PageCount - 1));          
        }
    }
}
