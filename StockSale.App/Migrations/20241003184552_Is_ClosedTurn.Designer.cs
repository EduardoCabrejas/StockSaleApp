﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StockSale.App.Data;

#nullable disable

namespace StockSale.App.Migrations
{
    [DbContext(typeof(StockSaleDbContext))]
    [Migration("20241003184552_Is_ClosedTurn")]
    partial class Is_ClosedTurn
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ClientTurn", b =>
                {
                    b.Property<Guid>("ClientsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TurnsId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ClientsId", "TurnsId");

                    b.HasIndex("TurnsId");

                    b.ToTable("ClientTurn");
                });

            modelBuilder.Entity("StockSale.App.Models.Domain.Client", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Company")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Condition_Iva")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Cuil_t")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Debt")
                        .HasColumnType("int");

                    b.Property<int?>("Discount")
                        .HasColumnType("int");

                    b.Property<string>("Dni")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("StockSale.App.Models.Domain.Fiado", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("Import")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Fiados");
                });

            modelBuilder.Entity("StockSale.App.Models.Domain.OrderBuy", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ClientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("Discount")
                        .HasColumnType("int");

                    b.Property<float>("Import")
                        .HasColumnType("real");

                    b.Property<Guid?>("TurnId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("TurnId");

                    b.ToTable("OrderBuys");
                });

            modelBuilder.Entity("StockSale.App.Models.Domain.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("List_Price")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Packaging")
                        .HasColumnType("int");

                    b.Property<Guid?>("ProviderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Sell_Price")
                        .HasColumnType("int");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.Property<int>("Stock_Limit")
                        .HasColumnType("int");

                    b.Property<Guid?>("UnitMId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ProviderId");

                    b.HasIndex("UnitMId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("StockSale.App.Models.Domain.Product_Price", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("OrderBuyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<float>("PriceList")
                        .HasColumnType("real");

                    b.Property<float>("PriceSell")
                        .HasColumnType("real");

                    b.Property<Guid?>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ProviderOrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrderBuyId");

                    b.HasIndex("ProductId");

                    b.HasIndex("ProviderOrderId");

                    b.ToTable("Products_Price");
                });

            modelBuilder.Entity("StockSale.App.Models.Domain.Provider", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Company")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Providers");
                });

            modelBuilder.Entity("StockSale.App.Models.Domain.ProviderOrder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<float>("Import")
                        .HasColumnType("real");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<Guid>("ProviderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("TurnId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ProviderId");

                    b.HasIndex("TurnId");

                    b.ToTable("ProviderOrders");
                });

            modelBuilder.Entity("StockSale.App.Models.Domain.Turn", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<float>("Closing_Cash")
                        .HasColumnType("real");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<float>("Initial_Cash")
                        .HasColumnType("real");

                    b.Property<bool>("Is_Closed")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Turns");
                });

            modelBuilder.Entity("StockSale.App.Models.Domain.UnitM", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UnitsM");
                });

            modelBuilder.Entity("ClientTurn", b =>
                {
                    b.HasOne("StockSale.App.Models.Domain.Client", null)
                        .WithMany()
                        .HasForeignKey("ClientsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StockSale.App.Models.Domain.Turn", null)
                        .WithMany()
                        .HasForeignKey("TurnsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("StockSale.App.Models.Domain.OrderBuy", b =>
                {
                    b.HasOne("StockSale.App.Models.Domain.Client", "Client")
                        .WithMany("OrderBuys")
                        .HasForeignKey("ClientId");

                    b.HasOne("StockSale.App.Models.Domain.Turn", "Turn")
                        .WithMany("OrderBuys")
                        .HasForeignKey("TurnId");

                    b.Navigation("Client");

                    b.Navigation("Turn");
                });

            modelBuilder.Entity("StockSale.App.Models.Domain.Product", b =>
                {
                    b.HasOne("StockSale.App.Models.Domain.Provider", "Provider")
                        .WithMany("Products")
                        .HasForeignKey("ProviderId");

                    b.HasOne("StockSale.App.Models.Domain.UnitM", "UnitM")
                        .WithMany("Products")
                        .HasForeignKey("UnitMId");

                    b.Navigation("Provider");

                    b.Navigation("UnitM");
                });

            modelBuilder.Entity("StockSale.App.Models.Domain.Product_Price", b =>
                {
                    b.HasOne("StockSale.App.Models.Domain.OrderBuy", null)
                        .WithMany("Products_Prices")
                        .HasForeignKey("OrderBuyId");

                    b.HasOne("StockSale.App.Models.Domain.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");

                    b.HasOne("StockSale.App.Models.Domain.ProviderOrder", null)
                        .WithMany("Products_Prices")
                        .HasForeignKey("ProviderOrderId");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("StockSale.App.Models.Domain.ProviderOrder", b =>
                {
                    b.HasOne("StockSale.App.Models.Domain.Provider", "Provider")
                        .WithMany("ProviderOrders")
                        .HasForeignKey("ProviderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StockSale.App.Models.Domain.Turn", "Turn")
                        .WithMany("ProviderOrders")
                        .HasForeignKey("TurnId");

                    b.Navigation("Provider");

                    b.Navigation("Turn");
                });

            modelBuilder.Entity("StockSale.App.Models.Domain.Client", b =>
                {
                    b.Navigation("OrderBuys");
                });

            modelBuilder.Entity("StockSale.App.Models.Domain.OrderBuy", b =>
                {
                    b.Navigation("Products_Prices");
                });

            modelBuilder.Entity("StockSale.App.Models.Domain.Provider", b =>
                {
                    b.Navigation("Products");

                    b.Navigation("ProviderOrders");
                });

            modelBuilder.Entity("StockSale.App.Models.Domain.ProviderOrder", b =>
                {
                    b.Navigation("Products_Prices");
                });

            modelBuilder.Entity("StockSale.App.Models.Domain.Turn", b =>
                {
                    b.Navigation("OrderBuys");

                    b.Navigation("ProviderOrders");
                });

            modelBuilder.Entity("StockSale.App.Models.Domain.UnitM", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
