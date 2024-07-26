using Microsoft.EntityFrameworkCore;
using PhoneNumberTopUp.Data.Entity;

namespace PhoneNumberTopUp.Data;
public class TopUpDb : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(s => s.Id);
        modelBuilder.Entity<Beneficiary>().HasKey(s => s.Id);
        modelBuilder.Entity<Beneficiary>().HasIndex(b => b.UserId);

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
}
