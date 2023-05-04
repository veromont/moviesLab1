﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using moviesAPI.Models.dbContext;

#nullable disable

namespace moviesAPI.Migrations
{
    [DbContext(typeof(MovieCinemaLabContext))]
    partial class MovieCinemaLabContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-preview.3.23174.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("moviesAPI.Models.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id")
                        .HasName("PK__genre__3213E85FFAC82D60");

                    b.ToTable("genres", (string)null);
                });

            modelBuilder.Entity("moviesAPI.Models.Hall", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<int>("Capacity")
                        .HasColumnType("int")
                        .HasColumnName("capacity");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit")
                        .HasColumnName("is_available");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("PK__hall__3213E83F5221F16A");

                    b.ToTable("halls", (string)null);
                });

            modelBuilder.Entity("moviesAPI.Models.Movie", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .IsUnicode(false)
                        .HasColumnType("char(36)")
                        .HasColumnName("id")
                        .IsFixedLength();

                    b.Property<string>("Director")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("director");

                    b.Property<TimeOnly>("Duration")
                        .HasPrecision(0)
                        .HasColumnType("time(0)")
                        .HasColumnName("duration");

                    b.Property<int>("GenreId")
                        .HasColumnType("int");

                    b.Property<double>("Rating")
                        .HasColumnType("float")
                        .HasColumnName("rating");

                    b.Property<DateOnly>("ReleaseDate")
                        .HasColumnType("date")
                        .HasColumnName("release_date");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("PK__movie__3213E83F0C1538E4");

                    b.HasIndex("GenreId");

                    b.ToTable("movies", (string)null);
                });

            modelBuilder.Entity("moviesAPI.Models.Session", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .IsUnicode(false)
                        .HasColumnType("char(36)")
                        .HasColumnName("id")
                        .IsFixedLength();

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime")
                        .HasColumnName("end_time");

                    b.Property<int>("HallId")
                        .HasColumnType("int")
                        .HasColumnName("hall_id");

                    b.Property<string>("MovieId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .IsUnicode(false)
                        .HasColumnType("char(36)")
                        .HasColumnName("movie_id")
                        .IsFixedLength();

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime")
                        .HasColumnName("start_time");

                    b.HasKey("Id")
                        .HasName("PK__session__3213E83FF82A853D");

                    b.HasIndex("HallId");

                    b.HasIndex("MovieId");

                    b.ToTable("sessions", (string)null);
                });

            modelBuilder.Entity("moviesAPI.Models.Ticket", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .IsUnicode(false)
                        .HasColumnType("char(36)")
                        .HasColumnName("id")
                        .IsFixedLength();

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("price");

                    b.Property<string>("SeatNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("seat_number");

                    b.Property<string>("SessionId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .IsUnicode(false)
                        .HasColumnType("char(36)")
                        .HasColumnName("session_id")
                        .IsFixedLength();

                    b.HasKey("Id")
                        .HasName("PK__ticket__3213E83FFAC82D60");

                    b.HasIndex("SessionId");

                    b.ToTable("tickets", (string)null);
                });

            modelBuilder.Entity("moviesAPI.Models.Movie", b =>
                {
                    b.HasOne("moviesAPI.Models.Genre", "Genre")
                        .WithMany("Movies")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__movie__genre___412EB0B6");

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("moviesAPI.Models.Session", b =>
                {
                    b.HasOne("moviesAPI.Models.Hall", "Hall")
                        .WithMany("Sessions")
                        .HasForeignKey("HallId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__session__hall_id__3E52440B");

                    b.HasOne("moviesAPI.Models.Movie", "Movie")
                        .WithMany("Sessions")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__session__movie_i__3D5E1FD2");

                    b.Navigation("Hall");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("moviesAPI.Models.Ticket", b =>
                {
                    b.HasOne("moviesAPI.Models.Session", "Session")
                        .WithMany("SessionTickets")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__ticket__session___412EB0B6");

                    b.Navigation("Session");
                });

            modelBuilder.Entity("moviesAPI.Models.Genre", b =>
                {
                    b.Navigation("Movies");
                });

            modelBuilder.Entity("moviesAPI.Models.Hall", b =>
                {
                    b.Navigation("Sessions");
                });

            modelBuilder.Entity("moviesAPI.Models.Movie", b =>
                {
                    b.Navigation("Sessions");
                });

            modelBuilder.Entity("moviesAPI.Models.Session", b =>
                {
                    b.Navigation("SessionTickets");
                });
#pragma warning restore 612, 618
        }
    }
}
