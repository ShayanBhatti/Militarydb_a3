using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MyMvcApp.Models;

public partial class NeondbContext : DbContext
{
    public NeondbContext()
    {
    }

    public NeondbContext(DbContextOptions<NeondbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<JobHistory> JobHistories { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=ep-broad-band-a59s3iyq-pooler.us-east-2.aws.neon.tech;Database=neondb;Username=neondb_owner;Password=npg_zO3gcu7JKMpm;SSL Mode=Require;Trust Server Certificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("countries_pkey");

            entity.ToTable("countries");

            entity.Property(e => e.CountryId)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("country_id");
            entity.Property(e => e.CountryName)
                .HasMaxLength(40)
                .HasColumnName("country_name");
            entity.Property(e => e.RegionId).HasColumnName("region_id");

            entity.HasOne(d => d.Region).WithMany(p => p.Countries)
                .HasForeignKey(d => d.RegionId)
                .HasConstraintName("countries_region_id_fkey");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("departments_pkey");

            entity.ToTable("departments");

            entity.HasIndex(e => e.LocationId, "dept_location_ix");

            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(30)
                .HasColumnName("department_name");
            entity.Property(e => e.LocationId).HasColumnName("location_id");
            entity.Property(e => e.ManagerId).HasColumnName("manager_id");

            entity.HasOne(d => d.Location).WithMany(p => p.Departments)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("departments_location_id_fkey");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("employees_pkey");

            entity.ToTable("employees");

            entity.HasIndex(e => e.DepartmentId, "emp_department_ix");

            entity.HasIndex(e => e.Email, "emp_email_uk").IsUnique();

            entity.HasIndex(e => e.JobId, "emp_job_ix");

            entity.HasIndex(e => e.ManagerId, "emp_manager_ix");

            entity.HasIndex(e => new { e.LastName, e.FirstName }, "emp_name_ix");

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.CommissionPct)
                .HasPrecision(2, 2)
                .HasColumnName("commission_pct");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.Email)
                .HasMaxLength(25)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(20)
                .HasColumnName("first_name");
            entity.Property(e => e.HireDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("hire_date");
            entity.Property(e => e.JobId)
                .HasMaxLength(10)
                .HasColumnName("job_id");
            entity.Property(e => e.LastName)
                .HasMaxLength(25)
                .HasColumnName("last_name");
            entity.Property(e => e.ManagerId).HasColumnName("manager_id");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phone_number");
            entity.Property(e => e.Salary)
                .HasPrecision(8, 2)
                .HasColumnName("salary");

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("employees_department_id_fkey");

            entity.HasOne(d => d.Job).WithMany(p => p.Employees)
                .HasForeignKey(d => d.JobId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("employees_job_id_fkey");

            entity.HasOne(d => d.Manager).WithMany(p => p.InverseManager)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("employees_manager_id_fkey");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.JobId).HasName("jobs_pkey");

            entity.ToTable("jobs");

            entity.Property(e => e.JobId)
                .HasMaxLength(10)
                .HasColumnName("job_id");
            entity.Property(e => e.JobTitle)
                .HasMaxLength(35)
                .HasColumnName("job_title");
            entity.Property(e => e.MaxSalary)
                .HasPrecision(6)
                .HasColumnName("max_salary");
            entity.Property(e => e.MinSalary)
                .HasPrecision(6)
                .HasColumnName("min_salary");
        });

        modelBuilder.Entity<JobHistory>(entity =>
        {
            entity.HasKey(e => new { e.EmployeeId, e.StartDate }).HasName("job_history_pkey");

            entity.ToTable("job_history");

            entity.HasIndex(e => e.DepartmentId, "jhist_department_ix");

            entity.HasIndex(e => e.EmployeeId, "jhist_employee_ix");

            entity.HasIndex(e => e.JobId, "jhist_job_ix");

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.StartDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("start_date");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.EndDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("end_date");
            entity.Property(e => e.JobId)
                .HasMaxLength(10)
                .HasColumnName("job_id");

            entity.HasOne(d => d.Department).WithMany(p => p.JobHistories)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("job_history_department_id_fkey");

            entity.HasOne(d => d.Employee).WithMany(p => p.JobHistories)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("job_history_employee_id_fkey");

            entity.HasOne(d => d.Job).WithMany(p => p.JobHistories)
                .HasForeignKey(d => d.JobId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("job_history_job_id_fkey");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("locations_pkey");

            entity.ToTable("locations");

            entity.HasIndex(e => e.City, "loc_city_ix");

            entity.HasIndex(e => e.CountryId, "loc_country_ix");

            entity.HasIndex(e => e.StateProvince, "loc_state_province_ix");

            entity.Property(e => e.LocationId).HasColumnName("location_id");
            entity.Property(e => e.City)
                .HasMaxLength(30)
                .HasColumnName("city");
            entity.Property(e => e.CountryId)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("country_id");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(12)
                .HasColumnName("postal_code");
            entity.Property(e => e.StateProvince)
                .HasMaxLength(25)
                .HasColumnName("state_province");
            entity.Property(e => e.StreetAddress)
                .HasMaxLength(40)
                .HasColumnName("street_address");

            entity.HasOne(d => d.Country).WithMany(p => p.Locations)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("locations_country_id_fkey");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.RegionId).HasName("regions_pkey");

            entity.ToTable("regions");

            entity.Property(e => e.RegionId).HasColumnName("region_id");
            entity.Property(e => e.RegionName)
                .HasMaxLength(25)
                .HasColumnName("region_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
