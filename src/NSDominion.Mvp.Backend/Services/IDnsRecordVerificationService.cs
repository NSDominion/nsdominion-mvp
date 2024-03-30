using NSDominion.Mvp.Backend.Common.Models.DnsRecords;

namespace NSDominion.Mvp.Backend.Services;

/// <summary>
/// A service used to verify the presence of DNS records in domains.
/// </summary>
public interface IDnsRecordVerificationService
{
    /// <summary>
    /// Verifies that the provided DNS record exists in the base domain.
    /// </summary>
    /// <param name="baseDomain">The base domain. The DNS name will be resolved within this domain.</param>
    /// <param name="dnsRecord">The DNS record to verify.</param>
    /// <param name="cancellationToken">A cancellation token used to cancel the operation.</param>
    /// <returns><c>true</c> if the DNS record exists; <c>false</c> otherwise.</returns>
    public Task<bool> VerifyDnsRecordAsync(string baseDomain, DnsRecord dnsRecord, CancellationToken cancellationToken = default);
}
