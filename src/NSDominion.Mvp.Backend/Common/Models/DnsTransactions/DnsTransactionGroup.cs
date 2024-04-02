namespace NSDominion.Mvp.Backend.Common.Models.DnsTransactions;

/// <summary>
/// A DNS transaction group. DNS transaction groups form part of DNS transactions and are treated as a unit for which propagation statuses
/// are accessible and propagation status events are emitted.
/// </summary>
/// <param name="dnsTransactionId"><inheritdoc cref="DnsTransactionId" path="/summary" /></param>
/// <param name="dnsTransactionGroupId"><inheritdoc cref="DnsTransactionGroupId" path="/summary" /></param>
/// <param name="isPropagated"><inheritdoc cref="IsPropagated" path="/summary" /></param>
/// <param name="wasPreviouslyPropagated"><inheritdoc cref="WasPreviouslyPropagated" path="/summary" /></param>
public sealed class DnsTransactionGroup(
    string dnsTransactionId,
    string dnsTransactionGroupId,
    IEnumerable<DnsTransactionRecord> records,
    bool isPropagated,
    bool wasPreviouslyPropagated
)
{
    /// <summary>
    /// The identifier of this DNS transaction this DNS transaction group is part of.
    /// </summary>
    public string DnsTransactionId { get; init; } = dnsTransactionId;

    /// <summary>
    /// The identifier of this DNS transaction group, unique within the scope of the containing DNS transaction.
    /// </summary>
    public string DnsTransactionGroupId { get; private init; } = dnsTransactionGroupId;

    /// <summary>
    /// The DNS transaction records that form part of this DNS transaction group.
    /// </summary>
    public IReadOnlyCollection<DnsTransactionRecord> Records => _records.AsReadOnly();

    /// <summary>
    /// Backing field for <see cref="Records"/>. 
    /// </summary>
    private readonly List<DnsTransactionRecord> _records = records.ToList();

    /// <summary>
    /// Whether this DNS transaction group is currently propagated as of the last propagation check.
    /// </summary>
    public bool IsPropagated { get; private set; } = isPropagated;

    /// <summary>
    /// Whether this DNS transaction group was ever previously propagated during a propagation check.
    /// </summary>
    /// <remarks>
    /// This flag is only after the first propagation and can therefore be used in DNS propagation events to limit certain actions to the
    /// first propagation only.
    /// </remarks>
    public bool WasPreviouslyPropagated { get; private set; } = wasPreviouslyPropagated;

    /// <summary>
    /// Sets the propagation status of this DNS transaction group.
    /// </summary>
    /// <param name="isPropagated"><inheritdoc cref="IsPropagated" path="/summary" /></param>
    public void SetPropagationStatus(bool isPropagated)
    {
        if (!isPropagated && IsPropagated) WasPreviouslyPropagated = true;

        IsPropagated = isPropagated;
    }
}
