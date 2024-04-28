using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CoinDeskAPI.Models;

public partial class CurrencyContext : DbContext
{
    public CurrencyContext()
    {
    }

    public CurrencyContext(DbContextOptions<CurrencyContext> options)
        : base(options)
    {
    }
    public DbSet<CurrencyItem> CurrencyItems { get; set; }
    public virtual DbSet<CurrencyTable> CurrencyTables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\ProjectModels;AttachDbFilename=C:\\Users\\billy\\AppData\\Local\\Microsoft\\VisualStudio\\SSDT\\Cathay_UnitedDB.mdf;Integrated Security=True;Connect Timeout=30");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CurrencyTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Currency__3214EC0775E40704");

            entity.ToTable("CurrencyTable");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Chinese)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Currency)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.English)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Japanese)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Rate)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Rate_float);
            entity.Property(e => e.Symbol)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.CreateDateTime)
                .IsFixedLength();
            entity.Property(e => e.UpdateTime)
                .IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
