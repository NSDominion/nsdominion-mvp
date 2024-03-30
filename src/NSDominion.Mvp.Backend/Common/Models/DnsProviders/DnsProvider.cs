namespace NSDominion.Mvp.Backend.Common.Models.DnsProviders;

/// <summary>
/// The DNS provider.
/// </summary>
/// <remarks>
/// This is the operator of the authoritative nameserver, not the registrar.
/// </remarks>
public enum DnsProvider
{
    /// <summary>
    /// Cloudflare. See: https://www.cloudflare.com/
    /// </summary>
    Cloudflare,

    /// <summary>
    /// Namecheap. See: https://www.namecheap.com/
    /// </summary>

    Namecheap,

    /// <summary>
    /// An unsupported provider.
    /// </summary>
    Unsupported
}
