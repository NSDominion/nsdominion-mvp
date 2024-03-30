using NSDominion.Mvp.Backend.Common.Models.DnsProviders;

namespace NSDominion.Mvp.Backend.Services;

/// <summary>
/// A service used to detect the provider of a DNS name.
/// </summary>
public interface IDnsProviderDetectionService
{
    /// <summary>
    /// Detects the DNS provider of the given domain.
    /// </summary>
    /// <param name="domain">The domain.</param>
    /// <param name="cancellationToken">A cancellation token used to cancel the operation.</param>
    /// <returns>The DNS provider of the domain.</returns>
    public Task<DnsProvider> DetectDnsProviderAsync(string domain, CancellationToken cancellationToken = default);
}
