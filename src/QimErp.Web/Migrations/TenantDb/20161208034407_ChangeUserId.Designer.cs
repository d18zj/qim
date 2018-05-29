using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using QimErp.Web;

namespace QimErp.Web.Migrations.TenantDb
{
    [DbContext(typeof(TenantDbContext))]
    [Migration("20161208034407_ChangeUserId")]
    partial class ChangeUserId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("QimErp.Domain.Entity.User", b =>
                {
                    b.Property<string>("PId")
                        .HasAnnotation("MaxLength", 36);

                    b.Property<string>("CellPhone")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("CreateBy")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 36);

                    b.Property<DateTime>("CreateOn");

                    b.Property<string>("Description");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastLoginTime");

                    b.Property<string>("LastModifyBy")
                        .HasAnnotation("MaxLength", 36);

                    b.Property<DateTime?>("LastModifyOn");

                    b.Property<int>("LoginCount");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<int>("TenantId");

                    b.Property<string>("UserAccount")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 20);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 20);

                    b.HasKey("PId");

                    b.HasIndex("CreateBy");

                    b.HasIndex("LastModifyBy");

                    b.HasIndex("TenantId");

                    b.HasIndex("UserAccount")
                        .IsUnique();

                    b.ToTable("Core_Users");
                });

            modelBuilder.Entity("QimErp.Domain.Entity.User", b =>
                {
                    b.HasOne("QimErp.Domain.Entity.User", "CreatorUser")
                        .WithMany()
                        .HasForeignKey("CreateBy")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("QimErp.Domain.Entity.User", "LastModifierUser")
                        .WithMany()
                        .HasForeignKey("LastModifyBy");
                });
        }
    }
}
