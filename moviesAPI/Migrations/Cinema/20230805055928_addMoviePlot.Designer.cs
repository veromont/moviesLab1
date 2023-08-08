﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using moviesAPI.DAL;

#nullable disable

namespace moviesAPI.Migrations.Cinema
{
    [DbContext(typeof(CinemaContext))]
    [Migration("20230805055928_addMoviePlot")]
    partial class addMoviePlot
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("moviesAPI.Models.EntityModels.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("genres", (string)null);
                });

            modelBuilder.Entity("moviesAPI.Models.EntityModels.Hall", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Capacity")
                        .HasColumnType("integer")
                        .HasColumnName("capacity");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("boolean")
                        .HasColumnName("isAvailable");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("halls", (string)null);
                });

            modelBuilder.Entity("moviesAPI.Models.EntityModels.Movie", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Director")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("director");

                    b.Property<TimeOnly>("Duration")
                        .HasColumnType("time without time zone")
                        .HasColumnName("duration");

                    b.Property<int?>("GenreId")
                        .HasColumnType("integer")
                        .HasColumnName("genreId");

                    b.Property<string>("Plot")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("plot");

                    b.Property<double>("Rating")
                        .HasColumnType("double precision")
                        .HasColumnName("rating");

                    b.Property<DateOnly>("ReleaseDate")
                        .HasColumnType("date")
                        .HasColumnName("releaseDate");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id");

                    b.HasIndex("GenreId");

                    b.ToTable("movies", (string)null);
                });

            modelBuilder.Entity("moviesAPI.Models.EntityModels.Session", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("endTime");

                    b.Property<int>("HallId")
                        .HasColumnType("integer")
                        .HasColumnName("hallId");

                    b.Property<Guid>("MovieId")
                        .HasColumnType("uuid")
                        .HasColumnName("movieId");

                    b.Property<double>("Price")
                        .HasColumnType("double precision")
                        .HasColumnName("price");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("startTime");

                    b.HasKey("Id");

                    b.HasIndex("HallId");

                    b.HasIndex("MovieId");

                    b.ToTable("sessions", (string)null);
                });

            modelBuilder.Entity("moviesAPI.Models.EntityModels.Ticket", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("SeatNumber")
                        .HasColumnType("integer")
                        .HasColumnName("seatNumber");

                    b.Property<Guid>("SessionId")
                        .HasColumnType("uuid")
                        .HasColumnName("sessionId");

                    b.Property<Guid>("USerId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SessionId");

                    b.ToTable("tickets", (string)null);
                });

            modelBuilder.Entity("moviesAPI.Models.EntityModels.UserGenreConnection", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("GenreId")
                        .HasColumnType("integer")
                        .HasColumnName("genre_id");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.HasIndex("GenreId");

                    b.ToTable("client_genre", (string)null);
                });

            modelBuilder.Entity("moviesAPI.Models.EntityModels.Movie", b =>
                {
                    b.HasOne("moviesAPI.Models.EntityModels.Genre", "Genre")
                        .WithMany("Movies")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("moviesAPI.Models.EntityModels.Session", b =>
                {
                    b.HasOne("moviesAPI.Models.EntityModels.Hall", "Hall")
                        .WithMany("Sessions")
                        .HasForeignKey("HallId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("moviesAPI.Models.EntityModels.Movie", "Movie")
                        .WithMany("Sessions")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hall");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("moviesAPI.Models.EntityModels.Ticket", b =>
                {
                    b.HasOne("moviesAPI.Models.EntityModels.Session", "Session")
                        .WithMany("SessionTickets")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Session");
                });

            modelBuilder.Entity("moviesAPI.Models.EntityModels.UserGenreConnection", b =>
                {
                    b.HasOne("moviesAPI.Models.EntityModels.Genre", "Genre")
                        .WithMany("Clients")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("moviesAPI.Models.EntityModels.Genre", b =>
                {
                    b.Navigation("Clients");

                    b.Navigation("Movies");
                });

            modelBuilder.Entity("moviesAPI.Models.EntityModels.Hall", b =>
                {
                    b.Navigation("Sessions");
                });

            modelBuilder.Entity("moviesAPI.Models.EntityModels.Movie", b =>
                {
                    b.Navigation("Sessions");
                });

            modelBuilder.Entity("moviesAPI.Models.EntityModels.Session", b =>
                {
                    b.Navigation("SessionTickets");
                });
#pragma warning restore 612, 618
        }
    }
}
