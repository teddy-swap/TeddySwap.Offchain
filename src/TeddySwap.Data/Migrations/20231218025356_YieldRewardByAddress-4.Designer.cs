﻿// <auto-generated />
using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TeddySwap.Data;

#nullable disable

namespace TeddySwap.Data.Migrations
{
    [DbContext(typeof(TeddySwapDbContext))]
    [Migration("20231218025356_YieldRewardByAddress-4")]
    partial class YieldRewardByAddress4
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("teddyswap-mainnet-v2")
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TeddySwap.Data.Models.Block", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<decimal>("Slot")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("Number")
                        .HasColumnType("numeric(20,0)");

                    b.HasKey("Id", "Slot");

                    b.ToTable("Blocks", "teddyswap-mainnet-v2");
                });

            modelBuilder.Entity("TeddySwap.Data.Models.Reducers.LedgerStateByAddress", b =>
                {
                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<decimal>("BlockNumber")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("Slot")
                        .HasColumnType("numeric(20,0)");

                    b.Property<JsonElement>("OutputsJson")
                        .HasColumnType("jsonb");

                    b.HasKey("Address", "BlockNumber", "Slot");

                    b.ToTable("LedgerStateByAddress", "teddyswap-mainnet-v2");
                });

            modelBuilder.Entity("TeddySwap.Data.Models.Reducers.LiquidityByAddressItem", b =>
                {
                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<decimal>("BlockNumber")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("Slot")
                        .HasColumnType("numeric(20,0)");

                    b.Property<JsonElement>("AssetsJson")
                        .HasColumnType("jsonb");

                    b.Property<decimal>("Lovelace")
                        .HasColumnType("numeric(20,0)");

                    b.HasKey("Address", "BlockNumber", "Slot");

                    b.ToTable("LiquidityByAddress", "teddyswap-mainnet-v2");
                });

            modelBuilder.Entity("TeddySwap.Data.Models.Reducers.LovelaceByAddressItem", b =>
                {
                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<decimal>("BlockNumber")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("Slot")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric(20,0)");

                    b.HasKey("Address", "BlockNumber", "Slot");

                    b.ToTable("LovelaceByAddress", "teddyswap-mainnet-v2");
                });

            modelBuilder.Entity("TeddySwap.Data.Models.Reducers.YieldClaimRequest", b =>
                {
                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<decimal>("BlockNumber")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("Slot")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("TxHash")
                        .HasColumnType("text");

                    b.Property<decimal>("TxIndex")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("ProcessTxHash")
                        .HasColumnType("text");

                    b.Property<string[]>("TBCs")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.HasKey("Address", "BlockNumber", "Slot", "TxHash", "TxIndex");

                    b.ToTable("YieldClaimRequests", "teddyswap-mainnet-v2");
                });

            modelBuilder.Entity("TeddySwap.Data.Models.Reducers.YieldRewardByAddress", b =>
                {
                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<decimal>("BlockNumber")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("Slot")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("PoolId")
                        .HasColumnType("text");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("Bonus")
                        .HasColumnType("numeric(20,0)");

                    b.Property<bool>("IsClaimed")
                        .HasColumnType("boolean");

                    b.Property<decimal>("LPAmount")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("PoolShare")
                        .HasColumnType("numeric");

                    b.Property<string[]>("TBCs")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Address", "BlockNumber", "Slot", "PoolId");

                    b.ToTable("YieldRewardByAddress", "teddyswap-mainnet-v2");
                });

            modelBuilder.Entity("TeddySwap.Data.Models.TransactionOutput", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<long>("Index")
                        .HasColumnType("bigint");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Slot")
                        .HasColumnType("numeric(20,0)");

                    b.HasKey("Id", "Index");

                    b.ToTable("TransactionOutputs", "teddyswap-mainnet-v2");
                });

            modelBuilder.Entity("TeddySwap.Data.Models.TransactionOutput", b =>
                {
                    b.OwnsOne("TeddySwap.Data.Models.Value", "Amount", b1 =>
                        {
                            b1.Property<string>("TransactionOutputId")
                                .HasColumnType("text");

                            b1.Property<long>("TransactionOutputIndex")
                                .HasColumnType("bigint");

                            b1.Property<decimal>("Coin")
                                .HasColumnType("numeric(20,0)");

                            b1.Property<JsonElement>("MultiAssetJson")
                                .HasColumnType("jsonb");

                            b1.HasKey("TransactionOutputId", "TransactionOutputIndex");

                            b1.ToTable("TransactionOutputs", "teddyswap-mainnet-v2");

                            b1.WithOwner()
                                .HasForeignKey("TransactionOutputId", "TransactionOutputIndex");
                        });

                    b.Navigation("Amount")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
