using NSDominion.Mvp.Backend.Common.Models.DnsRecords;

namespace NSDominion.Mvp.Backend.Common.Models.DnsTransactions;

/// <summary>
/// A DNS transaction group. DNS transaction groups form part of DNS transactions and are treated as a unit for which propagation statuses
/// are accessible and propagation status events are emitted.
/// </summary>
public sealed class DnsTransactionRecord(string dnsTransactionId, string dnsTransactionGroupId, IEnumerable<DnsRecord> records)
{
    /// <summary>
    /// The identifier of this DNS transaction this DNS transaction record is part of.
    /// </summary>
    public string DnsTransactionId { get; private init; } = dnsTransactionId;

    /// <summary>
    /// The identifier of this DNS transaction group this DNS transaction record is part of.
    /// </summary>
    public string DnsTransactionGroupId { get; private init; } = dnsTransactionGroupId;

    /// <summary>
    /// The DNS records that form part of this DNS transaction record.
    /// </summary>
    public IReadOnlyCollection<DnsRecord> Records => _records.AsReadOnly();

    /// <summary>
    /// Backing field for <see cref="Records"/>. 
    /// </summary>
    private readonly List<DnsRecord> _records = records.ToList();

    /// <summary>
    /// Whether this DNS transaction record is currently propagated as of the last propagation check.
    /// </summary>
    public bool IsPropagated { get; private set; }

    /// <summary>
    /// Sets the propagation status of this DNS transaction record.
    /// </summary>
    /// <param name="isPropagated"><inheritdoc cref="IsPropagated" path="/summary" /></param>
    public void SetPropagationStatus(bool isPropagated)
    {
        IsPropagated = isPropagated;
    }
}
