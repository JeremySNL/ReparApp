using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReparApp.Models;

namespace ReparApp.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Technician> Technicians { get; set; }
    public DbSet<PhoneNumber> PhoneNumbers { get; set; }
    public DbSet<PhoneNumberType> PhoneNumberTypes { get; set; }
    public DbSet<Repair> Repairs { get; set; }
    public DbSet<RepairStatus> RepairStatuses { get; set; }
    public DbSet<RepairStatusHistory> RepairStatusHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 🔧 Repair → Technician
        modelBuilder.Entity<Repair>()
            .HasOne(r => r.Technician)
            .WithMany(t => t.Repairs)
            .HasForeignKey(r => r.TechnicianId)
            .OnDelete(DeleteBehavior.Restrict);

        // 🔧 Repair → Customer
        modelBuilder.Entity<Repair>()
            .HasOne(r => r.Customer)
            .WithMany(c => c.Repairs)
            .HasForeignKey(r => r.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        // 🔧 RepairStatusHistory → Repair
        modelBuilder.Entity<RepairStatusHistory>()
            .HasOne(h => h.Repair)
            .WithMany(r => r.StatusHistory)
            .HasForeignKey(h => h.RepairId)
            .OnDelete(DeleteBehavior.Restrict);

        // 🔧 RepairStatusHistory → Technician
        modelBuilder.Entity<RepairStatusHistory>()
            .HasOne(h => h.Technician)
            .WithMany()
            .HasForeignKey(h => h.ChangedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
