using Microsoft.EntityFrameworkCore;
using PhoneNumberTopUp.Data.Entity;

namespace PhoneNumberTopUp.Data;
public class TopUpDb : DbContext
{
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

        modelBuilder.Entity<TopUpOption>().HasData(
            new TopUpOption { DisplayName = "AED 5", Value = 5 },
            new TopUpOption { DisplayName = "AED 10", Value = 10 },
            new TopUpOption { DisplayName = "AED 20", Value = 20 },
            new TopUpOption { DisplayName = "AED 30", Value = 30 },
            new TopUpOption { DisplayName = "AED 50", Value = 50 },
            new TopUpOption { DisplayName = "AED 75", Value = 75 },
            new TopUpOption { DisplayName = "AED 100", Value = 100 });
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Beneficiary> Beneficiarys { get; set; }
    public DbSet<TopUpOption> TopUpOptions { get; set; }
    public DbSet<TopUpTransaction> TopUpTransactions { get; set; }
}
