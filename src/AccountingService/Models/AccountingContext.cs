using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AccountingService.Models
{
    public partial class AccountingContext : DbContext
    {
        public AccountingContext()
        {
        }

        public AccountingContext(DbContextOptions<AccountingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Accounting> Accounting { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accounting>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(10);
            });
        }
    }
}
