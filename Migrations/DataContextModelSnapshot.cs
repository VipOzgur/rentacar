﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Rentacar.Models;

#nullable disable

namespace Rentacar.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("Rentacar.Models.Araclar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Aciklama")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreateDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("Depozito")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Durum")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Fiyat")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("GoruntulemeSayisi")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("IsActive")
                        .HasColumnType("BLOB");

                    b.Property<int?>("KiralanmaSayisi")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Km")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Marka")
                        .HasColumnType("TEXT");

                    b.Property<string>("Model")
                        .HasColumnType("TEXT");

                    b.Property<string>("MotorGucu")
                        .HasColumnType("TEXT");

                    b.Property<string>("Plaka")
                        .HasColumnType("TEXT");

                    b.Property<string>("Profil")
                        .HasColumnType("TEXT");

                    b.Property<string>("UpdateDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Vites")
                        .HasColumnType("TEXT");

                    b.Property<string>("Yakit")
                        .HasColumnType("TEXT");

                    b.Property<string>("YakitKm")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Id" }, "IX_Araclar_Id")
                        .IsUnique();

                    b.ToTable("Araclar", (string)null);
                });

            modelBuilder.Entity("Rentacar.Models.Blog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AdminId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreateDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Icerik")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("IsActive")
                        .HasColumnType("BLOB");

                    b.Property<int?>("Tip")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UpdateDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.HasIndex(new[] { "Id" }, "IX_Blog_Id")
                        .IsUnique();

                    b.ToTable("Blog", (string)null);
                });

            modelBuilder.Entity("Rentacar.Models.DbLogs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreateDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Konum")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Not")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UpdateDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Id" }, "IX_DbLogs_Id")
                        .IsUnique();

                    b.ToTable("DbLogs", (string)null);
                });

            modelBuilder.Entity("Rentacar.Models.Extralar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Ad")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CreateDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("Durum")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Fiyat")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UpdateDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Id" }, "IX_Extralar_Id")
                        .IsUnique();

                    b.ToTable("Extralar", (string)null);
                });

            modelBuilder.Entity("Rentacar.Models.Kaskolar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Ad")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CreateDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("GunlukFiyat")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SaatlikFiyat")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UpdateDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Id" }, "IX_Kaskolar_Id")
                        .IsUnique();

                    b.ToTable("Kaskolar", (string)null);
                });

            modelBuilder.Entity("Rentacar.Models.Lokasyonlar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Ad")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CreateDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("Durum")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Fiyat")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Kordinatlar")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UpdateDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Id" }, "IX_Lokasyonlar_Id")
                        .IsUnique();

                    b.ToTable("Lokasyonlar", (string)null);
                });

            modelBuilder.Entity("Rentacar.Models.Resim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AltG")
                        .HasColumnType("TEXT");

                    b.Property<int>("AracId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreateDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UpdateDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AracId");

                    b.HasIndex(new[] { "Id" }, "IX_Resim_Id")
                        .IsUnique();

                    b.ToTable("Resim", (string)null);
                });

            modelBuilder.Entity("Rentacar.Models.Rezervasyon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AlisLokasyonId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("AracId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreateDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("Durum")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FinishDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Fiyat")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("KaskoId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Not")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Onay")
                        .HasColumnType("INTEGER");

                    b.Property<string>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("TeslimLokasyonId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UpdateDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AlisLokasyonId");

                    b.HasIndex("AracId");

                    b.HasIndex("TeslimLokasyonId");

                    b.HasIndex("UserId");

                    b.HasIndex(new[] { "Id" }, "IX_Rezervasyon_Id")
                        .IsUnique();

                    b.ToTable("Rezervasyon", (string)null);
                });

            modelBuilder.Entity("Rentacar.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Ad")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CreateDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UpdateDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Id" }, "IX_Role_Id")
                        .IsUnique();

                    b.ToTable("Role", (string)null);
                });

            modelBuilder.Entity("Rentacar.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Ad")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Adres")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreateDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Eposta")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("IsActive")
                        .HasColumnType("BLOB");

                    b.Property<string>("Not")
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b.Property<int>("RoleId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Soyad")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Telefon")
                        .HasColumnType("TEXT");

                    b.Property<string>("UpdateDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex(new[] { "Id" }, "IX_User_Id")
                        .IsUnique();

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("Rentacar.Models.Yorumlar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AracId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreateDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("Durum")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UpdateDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Yorum")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AracId");

                    b.HasIndex("UserId");

                    b.HasIndex(new[] { "Id" }, "IX_Yorumlar_Id")
                        .IsUnique();

                    b.ToTable("Yorumlar", (string)null);
                });

            modelBuilder.Entity("Rentacar.Models.Blog", b =>
                {
                    b.HasOne("Rentacar.Models.User", "Admin")
                        .WithMany("Blogs")
                        .HasForeignKey("AdminId")
                        .IsRequired();

                    b.Navigation("Admin");
                });

            modelBuilder.Entity("Rentacar.Models.Resim", b =>
                {
                    b.HasOne("Rentacar.Models.Araclar", "Arac")
                        .WithMany("Resims")
                        .HasForeignKey("AracId")
                        .IsRequired();

                    b.Navigation("Arac");
                });

            modelBuilder.Entity("Rentacar.Models.Rezervasyon", b =>
                {
                    b.HasOne("Rentacar.Models.Lokasyonlar", "AlisLokasyon")
                        .WithMany("RezervasyonAlisLokasyons")
                        .HasForeignKey("AlisLokasyonId");

                    b.HasOne("Rentacar.Models.Araclar", "Arac")
                        .WithMany("Rezervasyons")
                        .HasForeignKey("AracId")
                        .IsRequired();

                    b.HasOne("Rentacar.Models.Lokasyonlar", "TeslimLokasyon")
                        .WithMany("RezervasyonTeslimLokasyons")
                        .HasForeignKey("TeslimLokasyonId");

                    b.HasOne("Rentacar.Models.User", "User")
                        .WithMany("Rezervasyons")
                        .HasForeignKey("UserId")
                        .IsRequired();

                    b.Navigation("AlisLokasyon");

                    b.Navigation("Arac");

                    b.Navigation("TeslimLokasyon");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Rentacar.Models.User", b =>
                {
                    b.HasOne("Rentacar.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Rentacar.Models.Yorumlar", b =>
                {
                    b.HasOne("Rentacar.Models.Araclar", "Arac")
                        .WithMany("Yorumlars")
                        .HasForeignKey("AracId");

                    b.HasOne("Rentacar.Models.User", "User")
                        .WithMany("Yorumlars")
                        .HasForeignKey("UserId");

                    b.Navigation("Arac");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Rentacar.Models.Araclar", b =>
                {
                    b.Navigation("Resims");

                    b.Navigation("Rezervasyons");

                    b.Navigation("Yorumlars");
                });

            modelBuilder.Entity("Rentacar.Models.Lokasyonlar", b =>
                {
                    b.Navigation("RezervasyonAlisLokasyons");

                    b.Navigation("RezervasyonTeslimLokasyons");
                });

            modelBuilder.Entity("Rentacar.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Rentacar.Models.User", b =>
                {
                    b.Navigation("Blogs");

                    b.Navigation("Rezervasyons");

                    b.Navigation("Yorumlars");
                });
#pragma warning restore 612, 618
        }
    }
}
