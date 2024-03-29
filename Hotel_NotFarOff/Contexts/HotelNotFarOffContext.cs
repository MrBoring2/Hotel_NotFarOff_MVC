﻿using System;
using System.Collections.Generic;
using System.Configuration;
using Hotel_NotFarOff.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Hotel_NotFarOff.Contexts
{
    public partial class HotelNotFarOffContext : DbContext
    {
        public HotelNotFarOffContext()
        {
        }

        public HotelNotFarOffContext(DbContextOptions<HotelNotFarOffContext> options)
            : base(options)
        {

        }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<BookingStatus> BookingStatuses { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Guest> Guests { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomCategory> RoomCategories { get; set; }
        public virtual DbSet<RoomImage> RoomImages { get; set; }
        public virtual DbSet<Service> Services { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseLazyLoadingProxies();
            //            if (!optionsBuilder.IsConfigured)
            //            {
            //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            //                optionsBuilder.UseSqlServer("Server=MRBORINGPC\\SQLEXPRESS01;Database=HotelNotFarOff;Trusted_Connection=True;");
            //            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.EmployeeId)
                    .HasName("PK_AdministratorProfle");

                entity.ToTable("Account");

                entity.Property(e => e.EmployeeId).ValueGeneratedNever();

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Employee)
                    .WithOne(p => p.Account)
                    .HasForeignKey<Account>(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SiteProfle_Employee");
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("Booking");

                entity.HasIndex(e => e.PaymentMethodId, "IX_Booking_PaymentMethodId");

                entity.Property(e => e.CheckIn).HasColumnType("datetime");

                entity.Property(e => e.CheckOut).HasColumnType("datetime");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.TenantFio)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TenantFIO");

                entity.HasOne(d => d.BookingStatus)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.BookingStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Booking_BookingStatus");

                entity.HasOne(d => d.PaymentMethod)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.PaymentMethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Booking_PaymentMethod");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Booking_Room");
            });

            modelBuilder.Entity<BookingStatus>(entity =>
            {
                entity.ToTable("BookingStatus");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Passport)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employee_Post");
            });

            modelBuilder.Entity<Guest>(entity =>
            {
                entity.ToTable("Guest");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasMany(d => d.Bookings)
                    .WithMany(p => p.Guests)
                    .UsingEntity<Dictionary<string, object>>(
                        "BookingGuest",
                        l => l.HasOne<Booking>().WithMany().HasForeignKey("BookingId").HasConstraintName("FK_BookingGuest_Booking"),
                        r => r.HasOne<Guest>().WithMany().HasForeignKey("GuestId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_BookingGuest_Guest"),
                        j =>
                        {
                            j.HasKey("GuestId", "BookingId");

                            j.ToTable("BookingGuest");
                        });
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.ToTable("PaymentMethod");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Post");

                entity.HasIndex(e => e.Id, "IX_Post");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("Room");

                entity.HasIndex(e => e.RoomNumber, "IX_Room")
                    .IsUnique();

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Room_Employee");

                entity.HasOne(d => d.RoomCategory)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.RoomCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Room_RoomCategory");
            });

            modelBuilder.Entity<RoomCategory>(entity =>
            {
                entity.ToTable("RoomCategory");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.MainImage)
                    .IsRequired()
                    .HasColumnType("image");

                entity.Property(e => e.PricePerDay).HasColumnType("money");

                entity.Property(e => e.ShortDescription)
                    .IsRequired()
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.RoomCategories)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_RoomCategory_Employee");

                entity.HasMany(d => d.Services)
                    .WithMany(p => p.RoomCategories)
                    .UsingEntity<Dictionary<string, object>>(
                        "ServiceToRoomCategory",
                        l => l.HasOne<Service>().WithMany().HasForeignKey("ServiceId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ServiceToRoomCategory_Service"),
                        r => r.HasOne<RoomCategory>().WithMany().HasForeignKey("RoomCategoryId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ServiceToRoomCategory_RoomCategory"),
                        j =>
                        {
                            j.HasKey("RoomCategoryId", "ServiceId");

                            j.ToTable("ServiceToRoomCategory");
                        });
            });

            modelBuilder.Entity<RoomImage>(entity =>
            {
                entity.ToTable("RoomImage");

                entity.HasIndex(e => e.RoomCategoryId, "IX_Galery");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasColumnType("image");

                entity.HasOne(d => d.RoomCategory)
                    .WithMany(p => p.RoomImages)
                    .HasForeignKey(d => d.RoomCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Galery_RoomCategory");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Service");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
