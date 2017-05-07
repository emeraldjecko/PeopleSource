using System;
using System.Collections.Generic;
using PeoplesSource.Common;
using System.Linq;

namespace PeoplesSource.Domain.Services
{
    /// <summary>
    /// Class ReferenceService.
    /// </summary>
    public class ReferenceService : IReferenceService
    {
        /// <summary>
        /// The _country repository
        /// </summary>
        private readonly IRepository<Country> _countryRepository;
       

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceService"/> class.
        /// </summary>
        /// <param name="countryRepository">The country repository.</param>
        public ReferenceService
            (
            IRepository<Country> countryRepository
            )
        {
            _countryRepository = countryRepository.ThrowIfNull("countryRepository");
        }

        /// <summary>
        /// Gets the countries.
        /// </summary>
        /// <returns> country </returns>
        public IList<Country> GetCountry()
        {
            return _countryRepository.GetAll();
        }

        /// <summary>
        /// Gets the country.
        /// </summary>
        /// <param name="countryId">The Country identifier.</param>
        /// <returns>country.</returns>
        public Country GetCountry(int countryId)
        {
            return _countryRepository.GetById(countryId);
        }

    }
}
