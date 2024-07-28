using Microsoft.EntityFrameworkCore;
using PhoneNumberTopUp.Data.Entity;

namespace PhoneNumberTopUp.Data;
public class TopUpDb : DbContext
{
    public TopUpDb(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u => u.Id);

        modelBuilder.Entity<Beneficiary>().HasKey(b => b.Id);
        modelBuilder.Entity<Beneficiary>().HasIndex(b => b.UserId);
        modelBuilder.Entity<Beneficiary>().HasIndex(b => new { b.UserId, b.PhoneNumber, b.Nickname }).IsUnique();
        modelBuilder.Entity<Beneficiary>().Property(b => b.Nickname).HasMaxLength(20);

        modelBuilder.Entity<TopUpTransaction>().HasKey(t => t.TransactionId);
        modelBuilder.Entity<TopUpTransaction>().HasIndex(t => t.UserId);
        modelBuilder.Entity<TopUpTransaction>().HasIndex(t => t.TransactionDateTime);

        modelBuilder.Entity<TopUpOption>().HasKey(t => t.Value);        
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Beneficiary> Beneficiarys { get; set; }
    public DbSet<TopUpOption> TopUpOptions { get; set; }
    public DbSet<TopUpTransaction> TopUpTransactions { get; set; }
}
