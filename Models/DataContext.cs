﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Rentacar.Models;

public partial class DataContext : DbContext
{
    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Araclar> Araclars { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<Extralar> Extralars { get; set; }

    public virtual DbSet<Kaskolar> Kaskolars { get; set; }

    public virtual DbSet<Lokasyonlar> Lokasyonlars { get; set; }

    public virtual DbSet<Resim> Resims { get; set; }

    public virtual DbSet<DbLogs> DbLogs{ get; set; }

    public virtual DbSet<Rezervasyon> Rezervasyons { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Yorumlar> Yorumlars { get; set; }

    public DbSet<RezervasyonExtra> RezervasyonExtralar { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)=> optionsBuilder.UseSqlite("Data Source=.//Data//data.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Araclar>(entity =>
        {
            entity.ToTable("Araclar");

            entity.HasIndex(e => e.Id, "IX_Araclar_Id").IsUnique();
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.ToTable("Blog");

            entity.HasIndex(e => e.Id, "IX_Blog_Id").IsUnique();

            entity.HasOne(d => d.Admin).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Extralar>(entity =>
        {
            entity.ToTable("Extralar");

            entity.HasIndex(e => e.Id, "IX_Extralar_Id").IsUnique();
        });

        modelBuilder.Entity<Kaskolar>(entity =>
        {
            entity.ToTable("Kaskolar");

            entity.HasIndex(e => e.Id, "IX_Kaskolar_Id").IsUnique();
        });

        modelBuilder.Entity<Lokasyonlar>(entity =>
        {
            entity.ToTable("Lokasyonlar");

            entity.HasIndex(e => e.Id, "IX_Lokasyonlar_Id").IsUnique();
        });

        modelBuilder.Entity<Resim>(entity =>
        {
            entity.ToTable("Resim");

            entity.HasIndex(e => e.Id, "IX_Resim_Id").IsUnique();

            entity.HasOne(d => d.Arac).WithMany(p => p.Resims)
                .HasForeignKey(d => d.AracId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Rezervasyon>(entity =>
        {
            entity.ToTable("Rezervasyon");

            entity.HasIndex(e => e.Id, "IX_Rezervasyon_Id").IsUnique();

            entity.HasOne(d => d.AlisLokasyon).WithMany(p => p.RezervasyonAlisLokasyons).HasForeignKey(d => d.AlisLokasyonId);

            entity.HasOne(d => d.Arac).WithMany(p => p.Rezervasyons)
                .HasForeignKey(d => d.AracId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.TeslimLokasyon).WithMany(p => p.RezervasyonTeslimLokasyons).HasForeignKey(d => d.TeslimLokasyonId);

            entity.HasOne(d => d.User).WithMany(p => p.Rezervasyons)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
        modelBuilder.Entity<Rezervasyon>()
    .HasMany(r => r.Extralar)
    .WithMany(e => e.Rezervasyons)
    .UsingEntity<Dictionary<string, object>>(
        "RezervasyonExtralar",
        j => j.HasOne<Extralar>().WithMany().HasForeignKey("ExtraId"),
        j => j.HasOne<Rezervasyon>().WithMany().HasForeignKey("RezervasyonId"),
        j =>
        {
            j.HasKey("RezervasyonId", "ExtraId");
            j.ToTable("RezervasyonExtralar");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.HasIndex(e => e.Id, "IX_Role_Id").IsUnique();
        });
        modelBuilder.Entity<DbLogs>(entity =>
        {
            entity.ToTable("DbLogs");

            entity.HasIndex(e => e.Id, "IX_DbLogs_Id").IsUnique();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.HasIndex(e => e.Id, "IX_User_Id").IsUnique();

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Yorumlar>(entity =>
        {
            entity.ToTable("Yorumlar");

            entity.HasIndex(e => e.Id, "IX_Yorumlar_Id").IsUnique();

            entity.HasOne(d => d.Arac).WithMany(p => p.Yorumlars).HasForeignKey(d => d.AracId);

            entity.HasOne(d => d.User).WithMany(p => p.Yorumlars).HasForeignKey(d => d.UserId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
