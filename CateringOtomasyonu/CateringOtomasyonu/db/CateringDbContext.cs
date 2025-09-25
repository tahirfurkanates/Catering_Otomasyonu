using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
namespace CateringOtomasyonu.db;

public partial class CateringDbContext : DbContext
{
    public CateringDbContext()
    {
    }

    public CateringDbContext(DbContextOptions<CateringDbContext> options)
        : base(options)
    {
    }
    public DbSet<MenuOgeleri> MenuOgeleri { get; set; } = null!;
    public DbSet<IletisimMesajlari> IletisimMesajlari { get; set; } = null!;
    public virtual DbSet<Etkinlikler> Etkinliklers { get; set; }

    public virtual DbSet<CateringOtomasyonu.Models.OneriSikayet> OneriSikayetler { get; set; }

    public virtual DbSet<IletisimMesajlari> IletisimMesajlaris { get; set; }

    public virtual DbSet<MenuOgeleri> MenuOgeleris { get; set; }

    public virtual DbSet<Musteriler> Musterilers { get; set; }

    public virtual DbSet<Personeller> Personellers { get; set; }

    public virtual DbSet<SiparisKalemleri> SiparisKalemleris { get; set; }

    public virtual DbSet<Siparisler> Siparislers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=TAHIR\\SQLEXPRESS;Database=CateringOtomasyonu;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Etkinlikler>(entity =>
        {
            entity.HasKey(e => e.EtkinlikId).HasName("PK__Etkinlik__0299F28D97087F89");

            entity.ToTable("Etkinlikler");

            entity.Property(e => e.Durum)
                .HasMaxLength(30)
                .HasDefaultValue("Planlandı");
            entity.Property(e => e.Konum).HasMaxLength(200);
            entity.Property(e => e.Notlar).HasMaxLength(500);

            entity.HasOne(d => d.Musteri).WithMany(p => p.Etkinliklers)
                .HasForeignKey(d => d.MusteriId)
                .HasConstraintName("FK_Etkinlikler_Musteriler");
        });

        modelBuilder.Entity<IletisimMesajlari>(entity =>
        {
            entity.HasKey(e => e.IletisimId);

            entity.ToTable("IletisimMesajlari");

            entity.Property(e => e.AdSoyad).HasMaxLength(100);
            entity.Property(e => e.Tarih)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<MenuOgeleri>(entity =>
        {
            entity.HasKey(e => e.MenuOgeleriId).HasName("PK__MenuOgel__F510DFCCB0154F2A");

            entity.ToTable("MenuOgeleri");

            entity.Property(e => e.Aciklama).HasMaxLength(250);
            entity.Property(e => e.Ad).HasMaxLength(100);
            entity.Property(e => e.AktifMi).HasDefaultValue(true);
            entity.Property(e => e.BirimFiyat).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Kategori).HasMaxLength(50);
        });

        modelBuilder.Entity<Musteriler>(entity =>
        {
            entity.HasKey(e => e.MusteriId).HasName("PK__Musteril__7262479174E0AD5F");

            entity.ToTable("Musteriler");

            entity.Property(e => e.Ad).HasMaxLength(100);
            entity.Property(e => e.Adres).HasMaxLength(250);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Telefon).HasMaxLength(30);
            entity.Property(e => e.VergiNo).HasMaxLength(50);
            entity.Property(e => e.Yetkili).HasMaxLength(100);
        });

        modelBuilder.Entity<Personeller>(entity =>
        {
            entity.HasKey(e => e.PersonelId).HasName("PK__Personel__0F0C5731F41887BA");

            entity.ToTable("Personeller");

            entity.Property(e => e.Ad).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Gorev).HasMaxLength(50);
            entity.Property(e => e.Sifre).HasMaxLength(50);
            entity.Property(e => e.Soyad).HasMaxLength(50);
            entity.Property(e => e.Telefon).HasMaxLength(30);

            entity.HasOne(d => d.Musteri).WithMany(p => p.Personellers)
                .HasForeignKey(d => d.MusteriId)
                .HasConstraintName("FK_Personeller_Musteriler");
        });

        modelBuilder.Entity<SiparisKalemleri>(entity =>
        {
            entity.HasKey(e => e.SiparisKalemiId).HasName("PK__SiparisK__16EBA85D857C99A0");

            entity.ToTable("SiparisKalemleri");

            entity.Property(e => e.Adet)
                .HasDefaultValue(1m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BirimFiyat).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Tutar)
                .HasComputedColumnSql("([Adet]*[BirimFiyat])", true)
                .HasColumnType("decimal(37, 4)");

            entity.HasOne(d => d.MenuOgeleri).WithMany(p => p.SiparisKalemleris)
                .HasForeignKey(d => d.MenuOgeleriId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SiparisKalemleri_MenuOgeleri");

            entity.HasOne(d => d.Siparis).WithMany(p => p.SiparisKalemleris)
                .HasForeignKey(d => d.SiparisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SiparisKalemleri_Siparisler");
        });

        modelBuilder.Entity<Siparisler>(entity =>
        {
            entity.HasKey(e => e.SiparisId).HasName("PK__Siparisl__C3F03BFDF52FD879");

            entity.ToTable("Siparisler");

            entity.Property(e => e.Durum)
                .HasMaxLength(30)
                .HasDefaultValue("Taslak");
            entity.Property(e => e.OlusturmaTarihi).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.ToplamTutar).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Etkinlik).WithMany(p => p.Siparislers)
                .HasForeignKey(d => d.EtkinlikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Siparisler_Etkinlikler");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
