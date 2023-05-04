﻿// <auto-generated />
using System;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Migrations
{
    [DbContext(typeof(BudgetDbContext))]
    partial class BudgetDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Intive.Patronage2023.Modules.Budget.Domain.BudgetAggregate", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreatedOn");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Icon")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)")
                        .HasColumnName("Name");

                    b.Property<string>("Status")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(10)")
                        .HasDefaultValue("Active")
                        .HasColumnName("Status");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("UserId");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "UserId", "Name" }, "IX_Budget_UserId_Name")
                        .IsUnique();

                    b.ToTable("Budget", "Budgets");
                });

            modelBuilder.Entity("Intive.Patronage2023.Modules.Budget.Domain.BudgetTransactionAggregate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id")
                        .HasDefaultValueSql("newsequentialid()");

                    b.Property<Guid>("BudgetId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("BudgetId");

                    b.Property<DateTime>("BudgetTransactionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CategoryType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("CategoryType");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreatedOn");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Name");

                    b.Property<string>("Status")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(10)")
                        .HasDefaultValue("Active")
                        .HasColumnName("Status");

                    b.Property<string>("TransactionType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("TransactionType");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(19,4)")
                        .HasColumnName("Value");

                    b.HasKey("Id");

                    b.HasIndex("BudgetId");

                    b.ToTable("BudgetTransaction", "Budgets");
                });

            modelBuilder.Entity("Intive.Patronage2023.Modules.Budget.Domain.DomainEventStore", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("CreatedAt");

                    b.Property<string>("Data")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Data");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Type");

                    b.HasKey("Id");

                    b.ToTable("DomainEventStore", "Budgets");
                });

            modelBuilder.Entity("Intive.Patronage2023.Modules.Budget.Domain.BudgetAggregate", b =>
                {
                    b.OwnsOne("Intive.Patronage2023.Shared.Infrastructure.Domain.ValueObjects.Money", "Limit", b1 =>
                        {
                            b1.Property<Guid>("BudgetAggregateId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Currency")
                                .HasColumnType("int")
                                .HasColumnName("Currency");

                            b1.Property<decimal>("Value")
                                .HasColumnType("decimal(18,2)")
                                .HasColumnName("Value");

                            b1.HasKey("BudgetAggregateId");

                            b1.ToTable("Budget", "Budgets");

                            b1.WithOwner()
                                .HasForeignKey("BudgetAggregateId");
                        });

                    b.OwnsOne("Intive.Patronage2023.Shared.Infrastructure.Domain.ValueObjects.Period", "Period", b1 =>
                        {
                            b1.Property<Guid>("BudgetAggregateId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<DateTime>("EndDate")
                                .HasColumnType("datetime2")
                                .HasColumnName("EndDate");

                            b1.Property<DateTime>("StartDate")
                                .HasColumnType("datetime2")
                                .HasColumnName("StartDate");

                            b1.HasKey("BudgetAggregateId");

                            b1.ToTable("Budget", "Budgets");

                            b1.WithOwner()
                                .HasForeignKey("BudgetAggregateId");
                        });

                    b.Navigation("Limit")
                        .IsRequired();

                    b.Navigation("Period")
                        .IsRequired();
                });

            modelBuilder.Entity("Intive.Patronage2023.Modules.Budget.Domain.BudgetTransactionAggregate", b =>
                {
                    b.HasOne("Intive.Patronage2023.Modules.Budget.Domain.BudgetAggregate", null)
                        .WithMany()
                        .HasForeignKey("BudgetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
