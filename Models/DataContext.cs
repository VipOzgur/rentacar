using System;
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

    public virtual DbSet<Resim> Resims { get; set; }

    public virtual DbSet<Rezervasyon> Rezervasyons { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=.//Data//data.db");

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

            entity.HasOne(d => d.Arac).WithMany(p => p.Rezervasyons)
                .HasForeignKey(d => d.AracId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User).WithMany(p => p.Rezervasyons)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.HasIndex(e => e.Id, "IX_Role_Id").IsUnique();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.HasIndex(e => e.Id, "IX_User_Id").IsUnique();

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
