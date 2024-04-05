using Microsoft.EntityFrameworkCore;
using NSDominion.Mvp.Backend.Common.Models.DnsTransactions;
using NSDominion.Mvp.Backend.Data;
using NSDominion.Mvp.Backend.Services;

namespace NSDominion.Mvp.Backend.Workers;

public sealed class DnsRecordVerificationWorker(
    IDbContextFactory<AppDbContext> dbContextFactory,
    IDnsRecordVerificationService dnsRecordVerificationService,
    TimeProvider timeProvider
) : BackgroundService
{
    private static readonly TimeSpan PropagationCheckingInterval = TimeSpan.FromSeconds(5);
    private const int TransactionsPerBatch = 10;
   
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory = dbContextFactory;
    private readonly IDnsRecordVerificationService _dnsRecordVerificationService = dnsRecordVerificationService;
    private readonly TimeProvider _timeProvider = timeProvider;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Task delayTask = Task.Delay(PropagationCheckingInterval, stoppingToken);
            
            await VerifyAndUpdateAsync(stoppingToken);
            
            await delayTask;
        }
    }
    
    private async Task VerifyAndUpdateAsync(CancellationToken cancellationToken)
    {
        await using AppDbContext dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        List<DnsTransaction> pendingDnsTransactions = await dbContext.DnsTransactions
            .OrderBy(dnsTransaction => dnsTransaction.PropagationStatusLastChecked)
            .Take(TransactionsPerBatch)
            .ToListAsync(cancellationToken);
        
        await Task.WhenAll(
            pendingDnsTransactions.Select(dnsTransaction => VerifyAndUpdateDnsTransactionAsync(dnsTransaction, cancellationToken))
        );
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    
    private async Task VerifyAndUpdateDnsTransactionAsync(DnsTransaction dnsTransaction, CancellationToken cancellationToken)
    {
        await Task.WhenAll(
            dnsTransaction.Groups.Select(dnsTransactionGroup => VerifyAndUpdateDnsTransactionGroupAsync(
                dnsTransaction,
                dnsTransactionGroup,
                cancellationToken
            ))
        );
        
        dnsTransaction.SetPropagationStatusLastChecked(_timeProvider.GetUtcNow());
    }
    
    private async Task VerifyAndUpdateDnsTransactionGroupAsync(
        DnsTransaction dnsTransaction,
        DnsTransactionGroup dnsTransactionGroup,
        CancellationToken cancellationToken
    )
    {
        await Task.WhenAll(
            dnsTransactionGroup.Records.Select(dnsTransactionRecord => VerifyAndUpdateDnsTransactionRecordAsync(
                dnsTransaction,
                dnsTransactionRecord,
                cancellationToken
            ))
        );
        
        bool isPropagated = dnsTransactionGroup.Records.All(dnsTransactionRecord => dnsTransactionRecord.IsPropagated);
        
        if (dnsTransactionGroup.IsPropagated != isPropagated) dnsTransactionGroup.SetPropagationStatus(isPropagated);
    }
    
    private async Task VerifyAndUpdateDnsTransactionRecordAsync(
        DnsTransaction dnsTransaction,
        DnsTransactionRecord dnsTransactionRecord,
        CancellationToken cancellationToken
    )
    {
        bool isPropagated = await _dnsRecordVerificationService.VerifyDnsRecordAsync(
            dnsTransaction.BaseDomainName,
            dnsTransactionRecord.Record,
            cancellationToken
        );
        
        if (dnsTransactionRecord.IsPropagated != isPropagated) dnsTransactionRecord.SetPropagationStatus(isPropagated);
    }
}