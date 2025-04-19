using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Models;

namespace MyMvcApp.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Mission> Mission { get; set; }
    public DbSet<Personnel> Personnel { get; set; }
    public DbSet<Unit> Units { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Mission>(entity =>
        {
            entity.HasKey(e => e.Missioncode).HasName("mission_pkey");

            entity.ToTable("mission");

            entity.Property(e => e.Missioncode)
                .HasMaxLength(10)
                .HasColumnName("missioncode");
            entity.Property(e => e.Missiondate).HasColumnName("missiondate");
        });

        modelBuilder.Entity<Personnel>(entity =>
        {
            entity.HasKey(e => e.Personnelid).HasName("personnel_pkey");

            entity.ToTable("personnel");

            entity.Property(e => e.Personnelid)
                .ValueGeneratedNever()
                .HasColumnName("personnelid");
            entity.Property(e => e.Bloodgroup)
                .HasMaxLength(5)
                .HasColumnName("bloodgroup");
            entity.Property(e => e.Contactnumber)
                .HasMaxLength(50)
                .HasColumnName("contactnumber");
            entity.Property(e => e.Dutystatus)
                .HasMaxLength(50)
                .HasColumnName("dutystatus");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Emergencycontact)
                .HasMaxLength(50)
                .HasColumnName("emergencycontact");
            entity.Property(e => e.Joiningdate).HasColumnName("joiningdate");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Rank)
                .HasMaxLength(50)
                .HasColumnName("rank");
            entity.Property(e => e.Unitname)
                .HasMaxLength(100)
                .HasColumnName("unitname");
            entity.Property(e => e.Weaponassigned)
                .HasMaxLength(50)
                .HasColumnName("weaponassigned");

            entity.HasOne(d => d.UnitnameNavigation).WithMany(p => p.Personnel)
                .HasForeignKey(d => d.Unitname)
                .HasConstraintName("personnel_unitname_fkey");

            entity.HasMany(d => d.Missioncodes).WithMany(p => p.Personnel)
                .UsingEntity<Dictionary<string, object>>(
                    "Personnelmission",
                    r => r.HasOne<Mission>().WithMany()
                        .HasForeignKey("Missioncode")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("personnelmissions_missioncode_fkey"),
                    l => l.HasOne<Personnel>().WithMany()
                        .HasForeignKey("Personnelid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("personnelmissions_personnelid_fkey"),
                    j =>
                    {
                        j.HasKey("Personnelid", "Missioncode").HasName("personnelmissions_pkey");
                        j.ToTable("personnelmissions");
                        j.IndexerProperty<int>("Personnelid").HasColumnName("personnelid");
                        j.IndexerProperty<string>("Missioncode")
                            .HasMaxLength(10)
                            .HasColumnName("missioncode");
                    });
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.HasKey(e => e.Unitname).HasName("unit_pkey");

            entity.ToTable("unit");

            entity.Property(e => e.Unitname)
                .HasMaxLength(100)
                .HasColumnName("unitname");
            entity.Property(e => e.Unitlocation)
                .HasMaxLength(100)
                .HasColumnName("unitlocation");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
