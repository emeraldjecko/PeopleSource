using System.Collections.Generic;

namespace PeoplesSource.Domain
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IList<T> Items { get; set; } 
    }
}
