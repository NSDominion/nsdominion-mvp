using System.Net;

namespace NSDominion.Mvp.Backend.Services;

/// <summary>
/// A service used to resolve the authoritative nameserver of a domain.
/// </summary>
public interface IAuthoritativeNameserversResolutionService
{
    /// <summary>
    /// Resolves the authoritative nameservers of the given domain.
    /// </summary>
    /// <remarks>
    /// If no results are found directly on the domain, the domain tree is traversed upwards, removing the leftmost subdomain part each
    /// time, until a match is found.
    /// </remarks>
    /// <param name="domain">The domain.</param>
    /// <param name="cancellationToken">A cancellation token used to cancel the operation.</param>
    /// <returns>The authoritative nameservers of the domain.</returns>
    public Task<IReadOnlyCollection<IPAddress>> ResolveAuthoritativeNameserversAsync(
        string domain,
        CancellationToken cancellationToken = default
    );
}
