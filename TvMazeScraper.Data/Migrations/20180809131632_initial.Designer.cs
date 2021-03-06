﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TvMazeScraper.Data;

namespace TvMazeScraper.Data.Migrations
{
    [DbContext(typeof(TvMazeScraperDbContext))]
    [Migration("20180809131632_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TvMazeScraper.Data.Model.Actor", b =>
                {
                    b.Property<int>("ActorId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("BirthDay")
                        .HasColumnType("date");

                    b.Property<int>("Id");

                    b.Property<string>("Name")
                        .HasMaxLength(500);

                    b.HasKey("ActorId");

                    b.ToTable("Actors");
                });

            modelBuilder.Entity("TvMazeScraper.Data.Model.Show", b =>
                {
                    b.Property<int>("ShowId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Id");

                    b.Property<string>("Name")
                        .HasMaxLength(500);

                    b.HasKey("ShowId");

                    b.ToTable("Shows");
                });

            modelBuilder.Entity("TvMazeScraper.Data.Model.ShowActor", b =>
                {
                    b.Property<int>("ShowId");

                    b.Property<int>("ActorId");

                    b.HasKey("ShowId", "ActorId");

                    b.HasIndex("ActorId");

                    b.ToTable("ShowActors");
                });

            modelBuilder.Entity("TvMazeScraper.Data.Model.ShowActor", b =>
                {
                    b.HasOne("TvMazeScraper.Data.Model.Actor", "Actor")
                        .WithMany()
                        .HasForeignKey("ActorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TvMazeScraper.Data.Model.Show", "Show")
                        .WithMany("ShowActors")
                        .HasForeignKey("ShowId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
