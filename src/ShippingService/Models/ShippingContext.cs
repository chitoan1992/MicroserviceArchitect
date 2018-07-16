using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ShippingService.Models
{
    public partial class ShippingContext : DbContext
    {
        public ShippingContext()
        {
        }

        public ShippingContext(DbContextOptions<ShippingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Shipping> Shipping { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shipping>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(10);
            });
        }
    }
}
