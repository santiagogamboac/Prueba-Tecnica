using System;
using System.Collections.Generic;
using Backend.Application.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infraestructure.Persistence;

public partial class ShopDbContext : DbContext
{
    public ShopDbContext()
    {
    }

    public ShopDbContext(DbContextOptions<ShopDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Shipper> Shippers { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //  => optionsBuilder.UseNpgsql("Host=db;Port=5432;Database=shop;Username=postgres;Password=Password123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Categoryid).HasName("categories_pkey");

            entity.ToTable("categories");

            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Categoryname)
                .HasMaxLength(255)
                .HasColumnName("categoryname");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Picture).HasColumnName("picture");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Customerid).HasName("customers_pkey");

            entity.ToTable("customers");

            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.Companyname)
                .HasMaxLength(255)
                .HasColumnName("companyname");
            entity.Property(e => e.Contactname)
                .HasMaxLength(255)
                .HasColumnName("contactname");
            entity.Property(e => e.Contacttitle)
                .HasMaxLength(255)
                .HasColumnName("contacttitle");
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .HasColumnName("country");
            entity.Property(e => e.Fax)
                .HasMaxLength(50)
                .HasColumnName("fax");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.Postalcode)
                .HasMaxLength(20)
                .HasColumnName("postalcode");
            entity.Property(e => e.Region)
                .HasMaxLength(100)
                .HasColumnName("region");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Employeeid).HasName("employees_pkey");

            entity.ToTable("employees");

            entity.Property(e => e.Employeeid).HasColumnName("employeeid");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Birthdate).HasColumnName("birthdate");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .HasColumnName("country");
            entity.Property(e => e.Extension)
                .HasMaxLength(10)
                .HasColumnName("extension");
            entity.Property(e => e.Firstname)
                .HasMaxLength(255)
                .HasColumnName("firstname");
            entity.Property(e => e.Hiredate).HasColumnName("hiredate");
            entity.Property(e => e.Homephone)
                .HasMaxLength(50)
                .HasColumnName("homephone");
            entity.Property(e => e.Lastname)
                .HasMaxLength(255)
                .HasColumnName("lastname");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Photo).HasColumnName("photo");
            entity.Property(e => e.Postalcode)
                .HasMaxLength(20)
                .HasColumnName("postalcode");
            entity.Property(e => e.Region)
                .HasMaxLength(100)
                .HasColumnName("region");
            entity.Property(e => e.Reportsto).HasColumnName("reportsto");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.Titleofcourtesy)
                .HasMaxLength(64)
                .HasColumnName("titleofcourtesy");

            entity.HasOne(d => d.ReportstoNavigation).WithMany(p => p.InverseReportstoNavigation)
                .HasForeignKey(d => d.Reportsto)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_employees_reportsto");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Orderid).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.HasIndex(e => e.Customerid, "idx_orders_customer");

            entity.HasIndex(e => e.Employeeid, "idx_orders_employee");

            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Employeeid).HasColumnName("employeeid");
            entity.Property(e => e.Freight)
                .HasPrecision(10, 2)
                .HasColumnName("freight");
            entity.Property(e => e.Orderdate).HasColumnName("orderdate");
            entity.Property(e => e.Requireddate).HasColumnName("requireddate");
            entity.Property(e => e.Shipaddress).HasColumnName("shipaddress");
            entity.Property(e => e.Shipcity)
                .HasMaxLength(100)
                .HasColumnName("shipcity");
            entity.Property(e => e.Shipcountry)
                .HasMaxLength(100)
                .HasColumnName("shipcountry");
            entity.Property(e => e.Shipname)
                .HasMaxLength(255)
                .HasColumnName("shipname");
            entity.Property(e => e.Shippeddate).HasColumnName("shippeddate");
            entity.Property(e => e.Shippostalcode)
                .HasMaxLength(20)
                .HasColumnName("shippostalcode");
            entity.Property(e => e.Shipregion)
                .HasMaxLength(100)
                .HasColumnName("shipregion");
            entity.Property(e => e.Shipvia).HasColumnName("shipvia");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Customerid)
                .HasConstraintName("fk_orders_customers");

            entity.HasOne(d => d.Employee).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Employeeid)
                .HasConstraintName("fk_orders_employees");

            entity.HasOne(d => d.ShipviaNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Shipvia)
                .HasConstraintName("fk_orders_shippers");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.Orderid, e.Productid }).HasName("order_details_pkey");

            entity.ToTable("order_details");

            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Discount).HasColumnName("discount");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Unitprice)
                .HasPrecision(10, 2)
                .HasColumnName("unitprice");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.Orderid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_details_orders");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.Productid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_details_products");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Productid).HasName("products_pkey");

            entity.ToTable("products");

            entity.HasIndex(e => e.Categoryid, "idx_products_category");

            entity.HasIndex(e => e.Productname, "idx_products_name");

            entity.HasIndex(e => e.Supplierid, "idx_products_supplier");

            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Discontinued)               
                .HasColumnName("discontinued");
            entity.Property(e => e.Productname)
                .HasMaxLength(255)
                .HasColumnName("productname");
            entity.Property(e => e.Quantityperunit)
                .HasMaxLength(100)
                .HasColumnName("quantityperunit");
            entity.Property(e => e.Reorderlevel).HasColumnName("reorderlevel");
            entity.Property(e => e.Supplierid).HasColumnName("supplierid");
            entity.Property(e => e.Unitprice)
                .HasPrecision(10, 2)
                .HasColumnName("unitprice");
            entity.Property(e => e.Unitsinstock).HasColumnName("unitsinstock");
            entity.Property(e => e.Unitsonorder).HasColumnName("unitsonorder");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.Categoryid)
                .HasConstraintName("fk_products_categories");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Products)
                .HasForeignKey(d => d.Supplierid)
                .HasConstraintName("fk_products_suppliers");
        });

        modelBuilder.Entity<Shipper>(entity =>
        {
            entity.HasKey(e => e.Shipperid).HasName("shippers_pkey");

            entity.ToTable("shippers");

            entity.Property(e => e.Shipperid).HasColumnName("shipperid");
            entity.Property(e => e.Companyname)
                .HasMaxLength(255)
                .HasColumnName("companyname");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Supplierid).HasName("suppliers_pkey");

            entity.ToTable("suppliers");

            entity.Property(e => e.Supplierid).HasColumnName("supplierid");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.Companyname)
                .HasMaxLength(255)
                .HasColumnName("companyname");
            entity.Property(e => e.Contactname)
                .HasMaxLength(255)
                .HasColumnName("contactname");
            entity.Property(e => e.Contacttitle)
                .HasMaxLength(255)
                .HasColumnName("contacttitle");
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .HasColumnName("country");
            entity.Property(e => e.Fax)
                .HasMaxLength(50)
                .HasColumnName("fax");
            entity.Property(e => e.Homepage)
                .HasMaxLength(500)
                .HasColumnName("homepage");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.Postalcode)
                .HasMaxLength(20)
                .HasColumnName("postalcode");
            entity.Property(e => e.Region)
                .HasMaxLength(100)
                .HasColumnName("region");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
