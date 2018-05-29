using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using QimErp.Web;

namespace QimErp.Web.Migrations
{
    [DbContext(typeof(HostDbContext))]
    partial class HostDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("QimErp.Domain.Entity.DatabaseInfo", b =>
                {
                    b.Property<int>("PId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreateBy")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 36);

                    b.Property<DateTime>("CreateOn");

                    b.Property<string>("DbName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 20);

                    b.Property<string>("DbNo")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 20);

                    b.Property<int>("DbSize");

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("LastModifyBy")
                        .HasAnnotation("MaxLength", 36);

                    b.Property<DateTime?>("LastModifyOn");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 20);

                    b.HasKey("PId");

                    b.HasIndex("DbNo")
                        .IsUnique();

                    b.ToTable("Core_DatabaseInfo");
                });

            modelBuilder.Entity("QimErp.Domain.Entity.GlobalAccount", b =>
                {
                    b.Property<int>("PId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CellPhone")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("CreateBy")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 36);

                    b.Property<DateTime>("CreateOn");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("LastModifyBy")
                        .HasAnnotation("MaxLength", 36);

                    b.Property<DateTime?>("LastModifyOn");

                    b.Property<int>("TenantId");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 36);

                    b.HasKey("PId");

                    b.HasIndex("CellPhone");

                    b.HasIndex("Email");

                    b.HasIndex("TenantId");

                    b.HasIndex("UserId");

                    b.ToTable("Core_TenantAccount");
                });

            modelBuilder.Entity("QimErp.Domain.Entity.Tenant", b =>
                {
                    b.Property<int>("PId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreateBy")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 36);

                    b.Property<DateTime>("CreateOn");

                    b.Property<int>("DatabaseId");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("date");

                    b.Property<bool>("IsActive");

                    b.Property<string>("LastModifyBy")
                        .HasAnnotation("MaxLength", 36);

                    b.Property<DateTime?>("LastModifyOn");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("date");

                    b.Property<string>("TenantName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.HasKey("PId");

                    b.HasIndex("DatabaseId");

                    b.ToTable("Core_Tenant");
                });

            modelBuilder.Entity("QimErp.Domain.Entity.GlobalAccount", b =>
                {
                    b.HasOne("QimErp.Domain.Entity.Tenant", "Tenant")
                        .WithMany()
                        .HasForeignKey("TenantId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("QimErp.Domain.Entity.Tenant", b =>
                {
                    b.HasOne("QimErp.Domain.Entity.DatabaseInfo", "DatabaseInfo")
                        .WithMany("Tenants")
                        .HasForeignKey("DatabaseId");
                });
        }
    }
}
