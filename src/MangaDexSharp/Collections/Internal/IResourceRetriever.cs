using MangaDexSharp.Parameters;
using System.Threading;
using System.Threading.Tasks;

namespace MangaDexSharp.Collections.Internal
{
    internal interface IResourceRetriever<TResource>
        where TResource : MangaDexResource
    {
        Task<ResourceCollection<TResource>> GetAsync(
            string endpoint,
            IQueryParameters? parameters,
            bool requireAuth,
            CancellationToken cancelToken = default);
    }
}