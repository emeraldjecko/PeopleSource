
namespace PeoplesSource.Common
{
    /// <summary>
    /// A contract for mapping requests to responses.
    /// </summary>
    public interface IMapper
    {
        /// <summary>
        /// Maps the specified request.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="request">The request.</param>
        /// <returns>The response.</returns>
        TResponse Map<TRequest, TResponse>(TRequest request);

        /// <summary>
        /// Gets a mapper for the specified types.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request to be mapped.</typeparam>
        /// <typeparam name="TResponse">The type of the response to be mapped.</typeparam>
        /// <returns>A mapper.</returns>
        IMapper<TRequest, TResponse> Get<TRequest, TResponse>();
    }

    /// <summary>
    /// A contract for mapping requests to responses.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    public interface IMapper<TRequest, TResponse>
    {
        /// <summary>
        /// Maps the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The response.</returns>
        TResponse Map(TRequest request);
    }
}
