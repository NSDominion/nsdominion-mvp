using System.Collections.Immutable;
using System.Net;
using System.Text;
using DnsClient;
using DnsClient.Protocol;
using NSDominion.Mvp.Backend.Common.Models.DnsRecords;

namespace NSDominion.Mvp.Backend.Services;

/// <inheritdoc />
public sealed class DnsRecordVerificationService(
    IAuthoritativeNameserversResolutionService authoritativeNameserversResolutionService,
    ILookupClient lookupClient
) : IDnsRecordVerificationService
{
    private readonly IAuthoritativeNameserversResolutionService _authoritativeNameserversResolutionService =
        authoritativeNameserversResolutionService;
    private readonly ILookupClient _lookupClient = lookupClient;

    /// <inheritdoc />
    public async Task<bool> VerifyDnsRecordAsync(string baseDomain, DnsRecord dnsRecord, CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<IPAddress> baseDomainAuthoritativeNameservers =
            await _authoritativeNameserversResolutionService.ResolveAuthoritativeNameserversAsync(baseDomain, cancellationToken);

        IDnsQueryResponse dnsQueryResponse = await _lookupClient.QueryServerAsync(
            servers: baseDomainAuthoritativeNameservers
                .Select(nameserverAddress => new NameServer(nameserverAddress))
                .ToImmutableList(),
            query: dnsRecord.ResolveAbsoluteName(baseDomain),
            queryType: dnsRecord.RecordType switch
            {
                DnsRecordType.A => QueryType.A,
                DnsRecordType.Ns => QueryType.NS,
                DnsRecordType.Cname => QueryType.CNAME,
                DnsRecordType.Mx => QueryType.MX,
                DnsRecordType.Txt => QueryType.TXT,
                DnsRecordType.Aaaa => QueryType.AAAA,
                DnsRecordType.Srv => QueryType.SRV,
                _ => throw new NotSupportedException()
            },
            queryClass: dnsRecord.RecordClass switch
            {
                DnsRecordClass.Internet => QueryClass.IN,
                _ => throw new NotSupportedException()
            },
            cancellationToken: cancellationToken
        );

        bool isRecordPresent = dnsRecord.RecordType switch
        {
            DnsRecordType.A => VerifyDnsARecord(dnsRecord, dnsQueryResponse.Answers.ARecords()),
            DnsRecordType.Cname => VerifyDnsCnameRecord(dnsRecord, dnsQueryResponse.Answers.CnameRecords()),
            DnsRecordType.Mx => VerifyDnsMxRecord(dnsRecord, dnsQueryResponse.Answers.MxRecords()),
            DnsRecordType.Txt => VerifyDnsTxtRecord(dnsRecord, dnsQueryResponse.Answers.TxtRecords()),
            DnsRecordType.Aaaa => VerifyDnsAaaaRecord(dnsRecord, dnsQueryResponse.Answers.AaaaRecords()),
            _ => throw new NotSupportedException()
        };

        return isRecordPresent;
    }

    private static bool VerifyDnsARecord(DnsRecord dnsRecord, IEnumerable<ARecord> aRecords)
    {
        IPAddress ipAddress = ParseIpAddressFromRecordData(dnsRecord.RecordData);

        return aRecords.Any(aRecord => aRecord.Address.Equals(ipAddress));
    }

    private static bool VerifyDnsAaaaRecord(DnsRecord dnsRecord, IEnumerable<AaaaRecord> aaaaRecords)
    {
        IPAddress ipAddress = ParseIpAddressFromRecordData(dnsRecord.RecordData);

        return aaaaRecords.Any(aRecord => aRecord.Address.Equals(ipAddress));
    }

    private static IPAddress ParseIpAddressFromRecordData(byte[] recordData) =>
        IPAddress.Parse(Encoding.ASCII.GetString(recordData));

    private static bool VerifyDnsTxtRecord(DnsRecord dnsRecord, IEnumerable<TxtRecord> txtRecords)
    {
        string recordData = Encoding.UTF8.GetString(dnsRecord.RecordData);

        return txtRecords.Any(txtRecord => txtRecord.Text.Count == 1 && txtRecord.Text.First() == recordData);
    }

    private static bool VerifyDnsMxRecord(DnsRecord dnsRecord, IEnumerable<MxRecord> txtRecords)
    {
        string recordData = Encoding.UTF8.GetString(dnsRecord.RecordData);

        return txtRecords.Any(mxRecord => mxRecord.Exchange.Value.Equals(recordData, StringComparison.OrdinalIgnoreCase));
    }

    private static bool VerifyDnsCnameRecord(DnsRecord dnsRecord, IEnumerable<CNameRecord> cnameRecords)
    {
        string recordData = Encoding.UTF8.GetString(dnsRecord.RecordData);

        return cnameRecords.Any(cnameRecord => cnameRecord.CanonicalName.Value.Equals(recordData, StringComparison.OrdinalIgnoreCase));
    }
}
