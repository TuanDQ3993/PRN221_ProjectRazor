﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProjectPRN221_LIBManagement.Models
{
    public partial class PRN221_LibContext : DbContext
    {
   
        public static PRN221_LibContext Ins=new PRN221_LibContext();
        public PRN221_LibContext()
        {
            if (Ins == null) { 
            Ins=this;
            }
        }

        public PRN221_LibContext(DbContextOptions<PRN221_LibContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Author> Authors { get; set; } = null!;
        public virtual DbSet<Book> Books { get; set; } = null!;
        public virtual DbSet<BookCondition> BookConditions { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Publisher> Publishers { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserBookLog> UserBookLogs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            if (!optionsBuilder.IsConfigured) { optionsBuilder.UseSqlServer(config.GetConnectionString("value")); }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasIndex(e => e.AuthorName, "UQ__Authors__4A1A120BEF81CE95")
                    .IsUnique();

                entity.Property(e => e.AuthorId).HasColumnName("AuthorID");

                entity.Property(e => e.AuthorName).HasMaxLength(100);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasIndex(e => e.Isbn, "UQ__Books__447D36EA1C9C334F")
                    .IsUnique();

                entity.Property(e => e.BookId).HasColumnName("BookID");

                entity.Property(e => e.AuthorId).HasColumnName("AuthorID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.Image).IsUnicode(false);

                entity.Property(e => e.Isbn)
                    .HasMaxLength(50)
                    .HasColumnName("ISBN");

                entity.Property(e => e.PublisherId).HasColumnName("PublisherID");

                entity.Property(e => e.Quantity).HasDefaultValueSql("((0))");

                entity.Property(e => e.Title).HasMaxLength(255);

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("FK_Books_Authors");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Books_Categories");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.PublisherId)
                    .HasConstraintName("FK_Books_Publishers");
            });

            modelBuilder.Entity<BookCondition>(entity =>
            {
                entity.HasKey(e => e.ConditionId)
                    .HasName("PK__BookCond__37F5C0EF91681A0C");

                entity.HasIndex(e => e.ConditionName, "UQ__BookCond__CE7E60669BBA379A")
                    .IsUnique();

                entity.Property(e => e.ConditionId).HasColumnName("ConditionID");

                entity.Property(e => e.ConditionName).HasMaxLength(50);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(e => e.CategoryName, "UQ__Categori__8517B2E0ABFE159A")
                    .IsUnique();

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryName).HasMaxLength(100);
            });

            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.HasIndex(e => e.PublisherName, "UQ__Publishe__5F0E2249CB849051")
                    .IsUnique();

                entity.Property(e => e.PublisherId).HasColumnName("PublisherID");

                entity.Property(e => e.PublisherName).HasMaxLength(100);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleName).HasMaxLength(20);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");

                entity.Property(e => e.StatusName).HasMaxLength(255);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");

                entity.Property(e => e.BookConditionOnBorrow).HasDefaultValueSql("((1))");

                entity.Property(e => e.BookId).HasColumnName("BookID");

                entity.Property(e => e.BorrowDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DueDate).HasColumnType("date");

                entity.Property(e => e.ReturnDate).HasColumnType("date");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.BookConditionOnBorrowNavigation)
                    .WithMany(p => p.TransactionBookConditionOnBorrowNavigations)
                    .HasForeignKey(d => d.BookConditionOnBorrow)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transactions_BookConditionOnBorrow");

                entity.HasOne(d => d.BookConditionOnReturnNavigation)
                    .WithMany(p => p.TransactionBookConditionOnReturnNavigations)
                    .HasForeignKey(d => d.BookConditionOnReturn)
                    .HasConstraintName("FK_Transactions_BookConditionOnReturn");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("FK_Transactions_Books");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.Status)
                    .HasConstraintName("FK_Transactions_Status");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Transactions_Users");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "UQ__Users__A9D10534DC97CD5E")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.PhoneNumber).HasMaxLength(15);

                entity.HasOne(d => d.RoleNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Role)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Roles");
            });

            modelBuilder.Entity<UserBookLog>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__UserBook__1788CCAC4223E16A");

                entity.ToTable("UserBookLog");

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("UserID");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.Property(e => e.LastBorrowDate).HasColumnType("date");

                entity.Property(e => e.LastReturnDate).HasColumnType("date");

                entity.Property(e => e.LastTransactionId).HasColumnName("LastTransactionID");

                entity.Property(e => e.LastUpdated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TotalBorrows).HasDefaultValueSql("((0))");

                entity.Property(e => e.TotalReturns).HasDefaultValueSql("((0))");

                entity.Property(e => e.ViolationCount).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.LastBookConditionOnReturnNavigation)
                    .WithMany(p => p.UserBookLogs)
                    .HasForeignKey(d => d.LastBookConditionOnReturn)
                    .HasConstraintName("FK_UserBookLog_Condition");

                entity.HasOne(d => d.LastTransaction)
                    .WithMany(p => p.UserBookLogs)
                    .HasForeignKey(d => d.LastTransactionId)
                    .HasConstraintName("FK_UserBookLog_LastTransaction");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UserBookLog)
                    .HasForeignKey<UserBookLog>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserBookLog_Users");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
