namespace NSDominion.Mvp.Backend.Common.Models.DnsRecords;

/// <summary>
/// A DNS record.
/// </summary>
/// <param name="name"><inheritdoc cref="Name" path="/summary" /></param>
/// <param name="timeToLive"><inheritdoc cref="TimeToLive" path="/summary" /></param>
/// <param name="recordClass"><inheritdoc cref="RecordClass" path="/summary" /></param>
/// <param name="recordType"><inheritdoc cref="RecordType" path="/summary" /></param>
/// <param name="recordData"><inheritdoc cref="RecordData" path="/summary" /></param>
public sealed class DnsRecord(string name, TimeSpan timeToLive, DnsRecordClass recordClass, DnsRecordType recordType, byte[] recordData)
{
    /// <summary>
    /// The name at which this DNS record is located. This can either be absolute or relative; absolute names must end with a dot.
    /// </summary>
    public string Name { get; init; } = name;

    /// <summary>
    /// The maximum time this DNS record may be cached for.
    /// </summary>
    public TimeSpan TimeToLive { get; init; } = timeToLive;

    /// <summary>
    /// The record class of this DNS record.
    /// </summary>
    public DnsRecordClass RecordClass { get; init; } = recordClass;

    /// <summary>
    /// The record type of this DNS record.
    /// </summary>
    public DnsRecordType RecordType { get; init; } = recordType;

    /// <summary>
    /// The record data contained in this DNS record.
    /// </summary>
    public byte[] RecordData { get; init; } = recordData;

    /// <summary>
    /// Whether this DNS record is absolute or not.
    /// </summary>
    public bool IsAbsolute { get; init; } = name.EndsWith('.');

    /// <summary>
    /// Resolves the absolute name of this DNS record relative to a base domain.
    /// </summary>
    /// <param name="baseDomain">The base domain.</param>
    /// <returns>The absolute name of this DNS record.</returns>
    public string ResolveAbsoluteName(string baseDomain) => IsAbsolute ? Name : Name + baseDomain;
}
