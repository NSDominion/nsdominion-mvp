using Microsoft.EntityFrameworkCore;
using NSDominion.Mvp.Backend.Common.Models.DnsTransactions;

namespace NSDominion.Mvp.Backend.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<DnsTransaction> DnsTransactions { get; set; }
}
