using StructureMap;

namespace PeoplesSource.Common
{
    /// <summary>
    /// A mapper for mapping miscellaneous requests to their associated responses.
    /// </summary>
    public class Mapper : IMapper
    {
        /// <summary>
        /// Maps the specified request.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <param name="request">The request.</param>
        /// <returns>The response.</returns>
        public TResponse Map<TRequest, TResponse>(TRequest request)
        {
            return ObjectFactory.GetInstance<IMapper<TRequest, TResponse>>().Map(request);
        }

        /// <summary>
        /// Gets a mapper for the specified types.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request to be mapped.</typeparam>
        /// <typeparam name="TResponse">The type of the response to be mapped.</typeparam>
        /// <returns>A mapper.</returns>
        public IMapper<TRequest, TResponse> Get<TRequest, TResponse>()
        {
            return ObjectFactory.GetInstance<IMapper<TRequest, TResponse>>();
        }
    }
}
