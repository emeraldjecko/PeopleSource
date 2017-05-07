using System.Collections.Generic;

namespace PeoplesSource.Domain.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IReferenceService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IList<Country> GetCountry();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        Country GetCountry(int countryId);
    }
}
