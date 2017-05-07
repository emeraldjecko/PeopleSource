
namespace PeoplesSource.Common
{
   

    /// <summary>
    /// A contract for mapping requests to responses on domain objects
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    public interface IDomainMapper<TRequest, TResponse> : IMapper<TRequest, TResponse>
    {

        /// <summary>
        /// Maps to existing.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        void MapToExisting(TRequest request, TResponse response);
    }
}
