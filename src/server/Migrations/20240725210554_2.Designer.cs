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
    [Migration("20240725210554_2")]
    partial class _2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.30")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Olimpo.Domain.Channel", b =>
                {
                    b.Property<string>("id")
                        .HasColumnType("text");

                    b.Property<string>("Sensorid")
                        .HasColumnType("text");

                    b.Property<int>("channel_id")
                        .HasColumnType("integer");

                    b.Property<string>("current_metricid")
                        .HasColumnType("text");

                    b.Property<int>("danger_orientation")
                        .HasColumnType("integer");

                    b.Property<decimal>("danger_value")
                        .HasColumnType("numeric");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("success_orientation")
                        .HasColumnType("integer");

                    b.Property<decimal>("success_value")
                        .HasColumnType("numeric");

                    b.Property<string>("unit")
                        .HasColumnType("text");

                    b.Property<int>("warning_orientation")
                        .HasColumnType("integer");

                    b.Property<decimal>("warning_value")
                        .HasColumnType("numeric");

                    b.HasKey("id");

                    b.HasIndex("Sensorid");

                    b.HasIndex("current_metricid");

                    b.ToTable("channels");
                });

            modelBuilder.Entity("Olimpo.Domain.Device", b =>
                {
                    b.Property<string>("id")
                        .HasColumnType("text");

                    b.Property<string>("Stackid")
                        .HasColumnType("text");

                    b.Property<string>("host")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("Stackid");

                    b.ToTable("devices");
                });

            modelBuilder.Entity("Olimpo.Domain.Metric", b =>
                {
                    b.Property<string>("id")
                        .HasColumnType("text");

                    b.Property<DateTime>("datetime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("latency")
                        .HasColumnType("bigint");

                    b.Property<string>("message")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("status")
                        .HasColumnType("integer");

                    b.Property<decimal?>("value")
                        .HasColumnType("numeric");

                    b.HasKey("id");

                    b.ToTable("metrics");
                });

            modelBuilder.Entity("Olimpo.Domain.Metric_History", b =>
                {
                    b.Property<string>("id")
                        .HasColumnType("text");

                    b.Property<string>("Channelid")
                        .HasColumnType("text");

                    b.Property<DateTime>("datetime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("latency")
                        .HasColumnType("bigint");

                    b.Property<decimal?>("value")
                        .HasColumnType("numeric");

                    b.HasKey("id");

                    b.HasIndex("Channelid");

                    b.ToTable("metrics_history");
                });

            modelBuilder.Entity("Olimpo.Domain.Stack", b =>
                {
                    b.Property<string>("id")
                        .HasColumnType("text");

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
                    b.Property<string>("id")
                        .HasColumnType("text");

                    b.Property<string>("Deviceid")
                        .HasColumnType("text");

                    b.Property<bool?>("SSL_Verification_Check")
                        .HasColumnType("boolean");

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

                    b.HasIndex("Deviceid");

                    b.ToTable("sensors");
                });

            modelBuilder.Entity("Olimpo.Domain.Channel", b =>
                {
                    b.HasOne("Olimpo.Sensors.Sensor", null)
                        .WithMany("channels")
                        .HasForeignKey("Sensorid");

                    b.HasOne("Olimpo.Domain.Metric", "current_metric")
                        .WithMany()
                        .HasForeignKey("current_metricid");

                    b.Navigation("current_metric");
                });

            modelBuilder.Entity("Olimpo.Domain.Device", b =>
                {
                    b.HasOne("Olimpo.Domain.Stack", null)
                        .WithMany("devices")
                        .HasForeignKey("Stackid");
                });

            modelBuilder.Entity("Olimpo.Domain.Metric_History", b =>
                {
                    b.HasOne("Olimpo.Domain.Channel", null)
                        .WithMany("metrics")
                        .HasForeignKey("Channelid");
                });

            modelBuilder.Entity("Olimpo.Sensors.Sensor", b =>
                {
                    b.HasOne("Olimpo.Domain.Device", null)
                        .WithMany("sensors")
                        .HasForeignKey("Deviceid");
                });

            modelBuilder.Entity("Olimpo.Domain.Channel", b =>
                {
                    b.Navigation("metrics");
                });

            modelBuilder.Entity("Olimpo.Domain.Device", b =>
                {
                    b.Navigation("sensors");
                });

            modelBuilder.Entity("Olimpo.Domain.Stack", b =>
                {
                    b.Navigation("devices");
                });

            modelBuilder.Entity("Olimpo.Sensors.Sensor", b =>
                {
                    b.Navigation("channels");
                });
#pragma warning restore 612, 618
        }
    }
}
