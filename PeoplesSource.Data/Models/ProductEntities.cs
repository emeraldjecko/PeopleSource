namespace PeoplesSource.Data.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ProductEntities : DbContext
    {
        public ProductEntities()
            : base("name=ProductEntities")
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<SellerInfo> SellerInfoes { get; set; }
        public virtual DbSet<NetoProduct> NetoProducts { get; set; }
        public virtual DbSet<BestMatchPosition> BestMatchPositions { get; set; }
        public virtual DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(e => e.SKU)
                .IsUnicode(false);

            modelBuilder.Entity<SellerInfo>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<SellerInfo>()
                .Property(e => e.Increment)
                .HasPrecision(18, 5);

            modelBuilder.Entity<SellerInfo>()
                .Property(e => e.KZ)
                .HasPrecision(18, 5);

            modelBuilder.Entity<SellerInfo>()
                .Property(e => e.OHT)
                .HasPrecision(18, 5);
        }
    }
}
