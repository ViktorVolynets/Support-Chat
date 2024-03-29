﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TechnicalSupport.Data;

namespace TechnicalSupport.Migrations
{
    [DbContext(typeof(ChatContext))]
    [Migration("20210311075337_five")]
    partial class five
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TechnicalSupport.Models.Client", b =>
                {
                    b.Property<int>("ClientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecondName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserIp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ClientId");

                    b.HasIndex("UserUserId");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("TechnicalSupport.Models.Dialog", b =>
                {
                    b.Property<Guid>("DialogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("ClientId")
                        .HasColumnType("int");

                    b.Property<Guid>("ClientUserUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<Guid>("EmployeeUserUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("DialogId");

                    b.HasIndex("ClientId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Dialogs");
                });

            modelBuilder.Entity("TechnicalSupport.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Age")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecondName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("WorkTimeId")
                        .HasColumnType("int");

                    b.HasKey("EmployeeId");

                    b.HasIndex("UserUserId");

                    b.HasIndex("WorkTimeId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("TechnicalSupport.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("TechnicalSupport.Models.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("LocalHash")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TechnicalSupport.Models.WorkTime", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<TimeSpan>("From")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("To")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.ToTable("WorkTimes");
                });

            modelBuilder.Entity("TechnicalSupport.Models.Client", b =>
                {
                    b.HasOne("TechnicalSupport.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TechnicalSupport.Models.Dialog", b =>
                {
                    b.HasOne("TechnicalSupport.Models.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId");

                    b.HasOne("TechnicalSupport.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId");

                    b.Navigation("Client");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("TechnicalSupport.Models.Employee", b =>
                {
                    b.HasOne("TechnicalSupport.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TechnicalSupport.Models.WorkTime", "WorkTime")
                        .WithMany("Employees")
                        .HasForeignKey("WorkTimeId");

                    b.Navigation("User");

                    b.Navigation("WorkTime");
                });

            modelBuilder.Entity("TechnicalSupport.Models.User", b =>
                {
                    b.HasOne("TechnicalSupport.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("TechnicalSupport.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("TechnicalSupport.Models.WorkTime", b =>
                {
                    b.Navigation("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}
