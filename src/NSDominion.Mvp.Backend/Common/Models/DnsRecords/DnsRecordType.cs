namespace NSDominion.Mvp.Backend.Common.Models.DnsRecords;

/// <summary>
/// The DNS record type.
/// </summary>
public enum DnsRecordType
{
    /// <summary>
    /// An A record pointing to an IPv4 address.
    /// </summary>
    A = 1,

    /// <summary>
    /// A nameserver (NS) record.
    /// </summary>
    Ns = 2,

    /// <summary>
    /// An alias (CNAME) record pointing to another DNS name.
    /// </summary>
    Cname = 5,

    /// <summary>
    /// An mail exchange (MX) record for routing incoming mail.
    /// </summary>
    Mx = 15,

    /// <summary>
    /// A textual (TXT) record.
    /// </summary>
    Txt = 16,

    /// <summary>
    /// An AAAA record pointing to an IPv6 address.
    /// </summary>
    Aaaa = 28,

    /// <summary>
    /// A service locator (SRV) record.
    /// </summary>
    Srv = 33
}
