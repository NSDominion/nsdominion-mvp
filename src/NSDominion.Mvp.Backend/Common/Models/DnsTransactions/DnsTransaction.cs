namespace NSDominion.Mvp.Backend.Common.Models.DnsTransactions;

/// <summary>
/// A DNS transaction, used for containing DNS record changes to be applied to a domain. Each DNS transaction maps to one set of
/// instructions and automations for applying the DNS record changes to the domain.
/// </summary>
/// <param name="dnsTransactionId"><inheritdoc cref="DnsTransactionId" path="/summary" /></param>
/// <param name="tenantId"><inheritdoc cref="TenantId" path="/summary" /></param>
/// <param name="baseDomainName"><inheritdoc cref="BaseDomainName" path="/summary" /></param>
/// <param name="propagationStatusLastChecked"><inheritdoc cref="PropagationStatusLastChecked" path="/summary" /></param>
/// <param name="groups"><inheritdoc cref="Groups" path="/summary" /></param>
public sealed class DnsTransaction(
    string dnsTransactionId,
    string tenantId,
    string baseDomainName,
    DateTimeOffset? propagationStatusLastChecked,
    IEnumerable<DnsTransactionGroup> groups
)
{
    /// <summary>
    /// The identifier of this DNS transaction.
    /// </summary>
    public string DnsTransactionId { get; private init; } = dnsTransactionId;

    /// <summary>
    /// The identifier of the tenant this DNS transaction is in.
    /// </summary>
    public string TenantId { get; private init; } = tenantId;

    /// <summary>
    /// The base domain name which DNS names in this DNS transaction are resolved relative to.
    /// </summary>
    public string BaseDomainName { get; private init; } = baseDomainName;

    /// <summary>
    /// The date-time offset at which the propagation status of this DNS transaction was last checked.
    /// </summary>
    public DateTimeOffset? PropagationStatusLastChecked { get; private set; } = propagationStatusLastChecked;

    /// <summary>
    /// The DNS transaction groups that are part of this DNS transaction.
    /// </summary>
    /// <remarks>
    /// Each DNS transaction group is treated as a unit for which propagation statuses are accessible and propagation status events are
    /// emitted.
    /// </remarks>
    public IReadOnlyCollection<DnsTransactionGroup> Groups => _groups.AsReadOnly();

    /// <summary>
    /// Backing field for <see cref="Groups"/>. 
    /// </summary>
    private readonly List<DnsTransactionGroup> _groups = groups.ToList();

    /// <summary>
    /// Sets the date-time offset marking when this DNS transaction's propagation status was last checked.
    /// </summary>
    /// <param name="propagationStatusLastChecked"><inheritdoc cref="PropagationStatusLastChecked" path="/summary" /></param>
    public void SetPropagationStatusLastChecked(DateTimeOffset propagationStatusLastChecked)
    {
        PropagationStatusLastChecked = propagationStatusLastChecked;
    }
}
