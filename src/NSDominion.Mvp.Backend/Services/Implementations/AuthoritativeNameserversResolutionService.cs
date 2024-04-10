using System.Collections.Immutable;
using System.Net;
using DnsClient;
using DnsClient.Protocol;

namespace NSDominion.Mvp.Backend.Services.Implementations;

/// <inheritdoc />
public sealed class AuthoritativeNameserversResolutionService(ILookupClient lookupClient) : IAuthoritativeNameserversResolutionService
{
    private readonly ILookupClient _lookupClient = lookupClient;
    
    /// <inheritdoc />
    public async Task<IReadOnlyCollection<IPAddress>> ResolveAuthoritativeNameserversAsync(
        string domain,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentException.ThrowIfNullOrEmpty(domain);
        
        IDnsQueryResponse nameServerQueryResponse =
            await _lookupClient.QueryAsync(domain, QueryType.NS, QueryClass.IN, cancellationToken);
        
        ImmutableList<NsRecord> nsRecords = nameServerQueryResponse.Answers
            .NsRecords()
            .ToImmutableList();
        
        if (nsRecords.IsEmpty)
        {
            string? parentDomain = domain.Split('.', 1).ElementAtOrDefault(1);
            
            return parentDomain is not null
                ? await ResolveAuthoritativeNameserversAsync(parentDomain, cancellationToken)
                : _lookupClient.NameServers.Select(nameServer => IPAddress.Parse(nameServer.Address)).ToImmutableList();
        }
        
        IPHostEntry[] nameServerHostEntries =
            await Task.WhenAll(nsRecords.Select(nsRecord => _lookupClient.GetHostEntryAsync(nsRecord.NSDName)));
        
        return nameServerHostEntries
            .SelectMany(nameServerHostEntry => nameServerHostEntry.AddressList)
            .ToImmutableList();
    }
}