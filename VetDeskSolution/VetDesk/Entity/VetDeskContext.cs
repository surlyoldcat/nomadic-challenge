using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace VetDesk.Entity
{
    public partial class VetDeskContext : IdentityDbContext<VetDeskUser>
    {
        public VetDeskContext()
        {
        }

        public VetDeskContext(DbContextOptions<VetDeskContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Critter> Critters { get; set; }
        public virtual DbSet<CritterType> CritterTypes { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new ApplicationException("DB Context not configured!");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Critter>(entity =>
            {
                entity.ToTable("Critter");
                 entity.Property(e => e.Color)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastWeight).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PhotoId)
                .IsRequired();

                entity.HasOne(d => d.CritterType)
                    .WithMany(p => p.Critter)
                    .HasForeignKey(d => d.CritterTypeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_CritterType_Critter");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Critters)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Customer_Critter");

                
            });

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.ToTable("Photo");
                entity.Property(p => p.FileName)
                .IsRequired()
                .HasMaxLength(128);

                entity.Property(p => p.PhotoFile)
                .IsRequired()
                .HasColumnType("image");

                entity.Property(p => p.ContentType)
                .IsRequired()
                .HasMaxLength(50);

                
            });

            modelBuilder.Entity<CritterType>(entity =>
            {
                entity.ToTable("CritterType");
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasData(

                    new { Id = 1, Description = "Cat" },
                    new {Id = 2, Description = "Dog" },
                    new {Id = 3, Description = "Llama"},
                    new {Id = 4, Description = "Herp"}

                    );
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
