﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Olimpo;

#nullable disable

namespace server.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20240602013822_14")]
    partial class _14
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.30")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Olimpo.Domain.Alert", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("critical")
                        .HasColumnType("integer");

                    b.Property<int>("success")
                        .HasColumnType("integer");

                    b.Property<int>("type")
                        .HasColumnType("integer");

                    b.Property<int>("warning")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.ToTable("alerts");
                });

            modelBuilder.Entity("Olimpo.Domain.Channel", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("Sensorid")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("alertsid")
                        .HasColumnType("uuid");

                    b.Property<int>("channel_id")
                        .HasColumnType("integer");

                    b.Property<Guid>("current_metricid")
                        .HasColumnType("uuid");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("unit")
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("Sensorid");

                    b.HasIndex("alertsid");

                    b.HasIndex("current_metricid");

                    b.ToTable("channels");
                });

            modelBuilder.Entity("Olimpo.Domain.Metric", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("Channelid")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("datetime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("error_code")
                        .HasColumnType("integer");

                    b.Property<long>("latency")
                        .HasColumnType("bigint");

                    b.Property<string>("message")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("value")
                        .HasColumnType("numeric");

                    b.HasKey("id");

                    b.HasIndex("Channelid");

                    b.ToTable("metrics");
                });

            modelBuilder.Entity("Olimpo.Domain.Service", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("Stackid")
                        .HasColumnType("uuid");

                    b.Property<string>("host")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("Stackid");

                    b.ToTable("services");
                });

            modelBuilder.Entity("Olimpo.Domain.Stack", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("order")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.ToTable("stacks");
                });

            modelBuilder.Entity("Olimpo.Sensors.Sensor", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool?>("SSL_Verification_Check")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("Serviceid")
                        .HasColumnType("uuid");

                    b.Property<int>("check_each")
                        .HasColumnType("integer");

                    b.Property<string>("host")
                        .HasColumnType("text");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("password")
                        .HasColumnType("text");

                    b.Property<int?>("port")
                        .HasColumnType("integer");

                    b.Property<int>("timeout")
                        .HasColumnType("integer");

                    b.Property<string>("type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("username")
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("Serviceid");

                    b.ToTable("sensors");
                });

            modelBuilder.Entity("Olimpo.Domain.Channel", b =>
                {
                    b.HasOne("Olimpo.Sensors.Sensor", null)
                        .WithMany("channels")
                        .HasForeignKey("Sensorid");

                    b.HasOne("Olimpo.Domain.Alert", "alerts")
                        .WithMany()
                        .HasForeignKey("alertsid");

                    b.HasOne("Olimpo.Domain.Metric", "current_metric")
                        .WithMany()
                        .HasForeignKey("current_metricid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("alerts");

                    b.Navigation("current_metric");
                });

            modelBuilder.Entity("Olimpo.Domain.Metric", b =>
                {
                    b.HasOne("Olimpo.Domain.Channel", null)
                        .WithMany("metrics")
                        .HasForeignKey("Channelid");
                });

            modelBuilder.Entity("Olimpo.Domain.Service", b =>
                {
                    b.HasOne("Olimpo.Domain.Stack", null)
                        .WithMany("services")
                        .HasForeignKey("Stackid");
                });

            modelBuilder.Entity("Olimpo.Sensors.Sensor", b =>
                {
                    b.HasOne("Olimpo.Domain.Service", null)
                        .WithMany("sensors")
                        .HasForeignKey("Serviceid");
                });

            modelBuilder.Entity("Olimpo.Domain.Channel", b =>
                {
                    b.Navigation("metrics");
                });

            modelBuilder.Entity("Olimpo.Domain.Service", b =>
                {
                    b.Navigation("sensors");
                });

            modelBuilder.Entity("Olimpo.Domain.Stack", b =>
                {
                    b.Navigation("services");
                });

            modelBuilder.Entity("Olimpo.Sensors.Sensor", b =>
                {
                    b.Navigation("channels");
                });
#pragma warning restore 612, 618
        }
    }
}
