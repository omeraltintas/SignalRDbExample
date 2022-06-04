using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ChartServer.Models
{
    public partial class SatisDbContext : DbContext
    {
        public SatisDbContext()
        {
        }

        public SatisDbContext(DbContextOptions<SatisDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Personeller> Personellers { get; set; } = null!;
        public virtual DbSet<Satislar> Satislars { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.;Database=SatisDb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Personeller>(entity =>
            {
                entity.ToTable("Personeller");

                entity.Property(e => e.Adi).HasMaxLength(50);

                entity.Property(e => e.Soyadi).HasMaxLength(50);
            });

            modelBuilder.Entity<Satislar>(entity =>
            {
                entity.ToTable("Satislar");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
