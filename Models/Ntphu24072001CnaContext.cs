using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Web_RealEstate.Models;

public partial class Ntphu24072001CnaContext : DbContext
{
    public Ntphu24072001CnaContext()
    {
    }

    public Ntphu24072001CnaContext(DbContextOptions<Ntphu24072001CnaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<ChuDauTu> ChuDauTus { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<LoginUser> LoginUsers { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Property> Properties { get; set; }

    public virtual DbSet<Seller> Sellers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=sql.bsite.net\\MSSQL2016; Database=ntphu24072001_CNA;Encrypt=True;TrustServerCertificate=true;uid=ntphu24072001_CNA;password=123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.ToTable("Admin");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.ParentId).HasColumnName("Parent_Id");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_Category_Category");
        });

        modelBuilder.Entity<ChuDauTu>(entity =>
        {
            entity.ToTable("ChuDauTu");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.ToTable("Location");

            entity.Property(e => e.Id).HasComment("Mã tỉnh thành");
            entity.Property(e => e.ParentId)
                .HasComment("Mã quận huyện")
                .HasColumnName("Parent_Id");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_Location_Location");
        });

        modelBuilder.Entity<LoginUser>(entity =>
        {
            entity.ToTable("LoginUser");

            entity.Property(e => e.Status).HasComment("0: inactive \r\n1: active");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.Property(e => e.Date).HasColumnType("datetime");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.ToTable("Post");

            entity.Property(e => e.AdminId).HasColumnName("Admin_Id");
            entity.Property(e => e.Date)
                .HasComment("Ngày viết")
                .HasColumnType("datetime");
            entity.Property(e => e.PublicDate)
                .HasComment("Ngày đăng")
                .HasColumnType("datetime");
            entity.Property(e => e.RealEstateId).HasColumnName("RealEstate_Id");
            entity.Property(e => e.SellerId).HasColumnName("Seller_Id");
            entity.Property(e => e.Status).HasComment("0: unPublic  1: Public");

            entity.HasOne(d => d.Admin).WithMany(p => p.Posts)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Post_Admin");

            entity.HasOne(d => d.RealEstate).WithMany(p => p.Posts)
                .HasForeignKey(d => d.RealEstateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Post_RealEstate");

            entity.HasOne(d => d.Seller).WithMany(p => p.Posts)
                .HasForeignKey(d => d.SellerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Post_Seller");
        });

        modelBuilder.Entity<Property>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_RealEstate");

            entity.ToTable("Property");

            entity.Property(e => e.CategoryId).HasColumnName("Category_Id");
            entity.Property(e => e.ChuDauTuId).HasColumnName("ChuDauTu_Id");
            entity.Property(e => e.LocationId).HasColumnName("Location_Id");

            entity.HasOne(d => d.Category).WithMany(p => p.Properties)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RealEstate_Category");

            entity.HasOne(d => d.ChuDauTu).WithMany(p => p.Properties)
                .HasForeignKey(d => d.ChuDauTuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Property_ChuDauTu");

            entity.HasOne(d => d.Location).WithMany(p => p.Properties)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RealEstate_Location");
        });

        modelBuilder.Entity<Seller>(entity =>
        {
            entity.ToTable("Seller");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
