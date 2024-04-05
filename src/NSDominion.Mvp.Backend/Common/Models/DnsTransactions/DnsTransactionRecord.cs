using NSDominion.Mvp.Backend.Common.Models.DnsRecords;

namespace NSDominion.Mvp.Backend.Common.Models.DnsTransactions;

/// <summary>
/// A DNS transaction record.
/// </summary>
public sealed class DnsTransactionRecord(string dnsTransactionId, string dnsTransactionGroupId, DnsRecord record)
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
    /// The DNS record that this DNS transaction record requires.
    /// </summary>
    public DnsRecord Record => record;

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
