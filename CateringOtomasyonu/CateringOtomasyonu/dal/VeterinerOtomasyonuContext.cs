using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CateringOtomasyonu.Models;
using CateringOtomasyonu.ViewModels;

namespace CateringOtomasyonu.dal;

public partial class VeterinerOtomasyonuContext : DbContext
{
    public VeterinerOtomasyonuContext()
    {
    }

    public VeterinerOtomasyonuContext(DbContextOptions<VeterinerOtomasyonuContext> options)
        : base(options)
    {
    }
    public DbSet<IletisimMesaj> IletisimMesajlari { get; set; }

    public virtual DbSet<Aktivite> Aktivitelers { get; set; } = null!;


    public virtual DbSet<Asilar> Asilars { get; set; }

    public virtual DbSet<HayvanSahipleri> HayvanSahipleris { get; set; }

    public virtual DbSet<Hayvanlar> Hayvanlars { get; set; }

    public virtual DbSet<Randevular> Randevulars { get; set; }

    public virtual DbSet<Tedaviler> Tedavilers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Veterinerler> Veterinerlers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=TAHIR\\SQLEXPRESS;Database=VeterinerOtomasyonu;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aktivite>(entity =>
        {
            entity.ToTable("Aktiviteler");

            entity.HasKey(e => e.Id).HasName("PK_Aktiviteler");

            entity.Property(e => e.Islem).HasMaxLength(200);
            entity.Property(e => e.KullaniciAdi).HasMaxLength(100);
            entity.Property(e => e.Rol).HasMaxLength(50);
            entity.Property(e => e.Tarih).HasColumnType("datetime");

            entity.HasOne(e => e.Kullanici)
                .WithMany(u => u.Aktiviteler)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Aktiviteler_Users");
        });





        modelBuilder.Entity<Asilar>(entity =>
        {
            entity.HasKey(e => e.AsiId).HasName("PK__Asilar__42AA3E466A6D4DB0");

            entity.ToTable("Asilar");

            entity.Property(e => e.AsiAdi).HasMaxLength(100);

            entity.HasOne(d => d.Hayvan).WithMany(p => p.Asilars)
                .HasForeignKey(d => d.HayvanId)
                .HasConstraintName("FK__Asilar__HayvanId__440B1D61");
        });

        modelBuilder.Entity<HayvanSahipleri>(entity =>
        {
            entity.HasKey(e => e.SahipId).HasName("PK__HayvanSa__0C690E93D313ED38");

            entity.ToTable("HayvanSahipleri");

            entity.Property(e => e.Ad).HasMaxLength(50);
            entity.Property(e => e.Adres).HasMaxLength(250);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Soyad).HasMaxLength(50);
            entity.Property(e => e.Telefon).HasMaxLength(20);
        });

        modelBuilder.Entity<Hayvanlar>(entity =>
        {
            entity.HasKey(e => e.HayvanId).HasName("PK__Hayvanla__273611B8425895EF");

            entity.ToTable("Hayvanlar");

            entity.Property(e => e.Ad).HasMaxLength(50);
            entity.Property(e => e.Cinsiyet).HasMaxLength(10);
            entity.Property(e => e.Irk).HasMaxLength(50);
            entity.Property(e => e.Tur).HasMaxLength(50);

            entity.HasOne(d => d.Sahip).WithMany(p => p.Hayvanlars)
                .HasForeignKey(d => d.SahipId)
                .HasConstraintName("FK__Hayvanlar__Sahip__3D5E1FD2");
        });

        modelBuilder.Entity<Randevular>(entity =>
        {
            entity.HasKey(e => e.RandevuId).HasName("PK__Randevul__B795F34B1E5F5E1E");

            entity.ToTable("Randevular");

            entity.Property(e => e.Notlar).HasMaxLength(500);
            entity.Property(e => e.Tarih).HasColumnType("datetime");
            entity.Property(e => e.DoluMu).HasDefaultValue(false);

            entity.HasOne(d => d.Hayvan).WithMany(p => p.Randevulars)
                .HasForeignKey(d => d.HayvanId)
                .HasConstraintName("FK__Randevula__Hayva__403A8C7D");

            entity.HasOne(d => d.Veteriner).WithMany(p => p.Randevulars)
                .HasForeignKey(d => d.VeterinerId)
                .HasConstraintName("FK__Randevula__Veter__412EB0B6");
        });

        modelBuilder.Entity<Tedaviler>(entity =>
        {
            entity.HasKey(e => e.TedaviId).HasName("PK__Tedavile__95C30026E01A4075");

            entity.ToTable("Tedaviler");

            entity.Property(e => e.Aciklama).HasMaxLength(500);
            entity.Property(e => e.Tarih).HasColumnType("datetime");

            entity.HasOne(d => d.Hayvan).WithMany(p => p.Tedavilers)
                .HasForeignKey(d => d.HayvanId)
                .HasConstraintName("FK__Tedaviler__Hayva__46E78A0C");

            entity.HasOne(d => d.Veteriner).WithMany(p => p.Tedavilers)
                .HasForeignKey(d => d.VeterinerId)
                .HasConstraintName("FK__Tedaviler__Veter__47DBAE45");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC078328FB95");

            entity.Property(e => e.KullaniciAdi).HasMaxLength(50);
            entity.Property(e => e.Rol).HasMaxLength(20);
            entity.Property(e => e.Sifre).HasMaxLength(100);

            entity.HasOne(d => d.Sahip).WithMany(p => p.Users)
                .HasForeignKey(d => d.SahipId)
                .HasConstraintName("FK_Users_Sahip");

            entity.HasOne(d => d.Veteriner).WithMany(p => p.Users)
                .HasForeignKey(d => d.VeterinerId)
                .HasConstraintName("FK_Users_Veteriner");
        });

        modelBuilder.Entity<Veterinerler>(entity =>
        {
            entity.HasKey(e => e.VeterinerId).HasName("PK__Veterine__59CC5D5C8824A2B5");

            entity.ToTable("Veterinerler");

            entity.Property(e => e.Ad).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Soyad).HasMaxLength(50);
            entity.Property(e => e.Telefon).HasMaxLength(20);
            entity.Property(e => e.Uzmanlik).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
