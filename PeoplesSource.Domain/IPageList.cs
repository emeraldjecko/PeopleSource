namespace PeoplesSource.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPageList
    {
        /// <summary>
        /// 
        /// </summary>
        int PageCount { get; }
        /// <summary>
        /// 
        /// </summary>
        int TotalItemCount { get; }
        /// <summary>
        /// 
        /// </summary>
        int PageIndex { get; }
        /// <summary>
        /// 
        /// </summary>
        int PageNumber { get; }
        /// <summary>
        /// 
        /// </summary>
        int PageSize { get; }
        /// <summary>
        /// 
        /// </summary>
        bool HasPreviousPage { get; }
        /// <summary>
        /// 
        /// </summary>
        bool HasNextPage { get; }
        /// <summary>
        /// 
        /// </summary>
        bool IsFirstPage { get; }
        /// <summary>
        /// 
        /// </summary>
        bool IsLastPage { get; }
    }
}
