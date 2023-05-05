using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace moviesAPI.Models.dbContext;

public partial class MovieCinemaLabContext : DbContext
{
    public MovieCinemaLabContext()
    {
    }

    public MovieCinemaLabContext(DbContextOptions<MovieCinemaLabContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Hall> Halls { get; set; }
    public virtual DbSet<Movie> Movies { get; set; }
    public virtual DbSet<Session> Sessions { get; set; }
    public virtual DbSet<Ticket> Tickets { get; set; }
    public virtual DbSet<Genre> Genres { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Hall>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__hall__3213E83F5221F16A");

            entity.ToTable("halls");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.IsAvailable).HasColumnName("is_available");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(true)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__movie__3213E83F0C1538E4");

            entity.ToTable("movies");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("id");
            entity.Property(e => e.Director)
                .HasMaxLength(100)
                .IsUnicode(true)
                .HasColumnName("director");
            entity.Property(e => e.Duration)
                .HasPrecision(0)
                .HasColumnName("duration");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.ReleaseDate).HasColumnName("release_date").HasColumnType("date");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(true)
                .HasColumnName("title");

            entity.HasOne(d => d.Genre).WithMany(p => p.Movies)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__movie__genre___412EB0B6");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__session__3213E83FF82A853D");

            entity.ToTable("sessions");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("id");
            entity.Property(e => e.EndTime)
                .HasColumnType("datetime")
                .HasColumnName("end_time");
            entity.Property(e => e.HallId).HasColumnName("hall_id");
            entity.Property(e => e.MovieId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("movie_id");
            entity.Property(e => e.StartTime)
                .HasColumnType("datetime")
                .HasColumnName("start_time");
            entity.HasOne(d => d.Hall).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.HallId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__session__hall_id__3E52440B");

            entity.HasOne(d => d.Movie).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.MovieId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__session__movie_i__3D5E1FD2");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ticket__3213E83FFAC82D60");

            entity.ToTable("tickets");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("id");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.SeatNumber).HasColumnName("seat_number");
            entity.Property(e => e.SessionId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("session_id");

            entity.HasOne(d => d.Session).WithMany(p => p.SessionTickets)
                .HasForeignKey(d => d.SessionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ticket__session___412EB0B6");
        });
        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__genre__3213E85FFAC82D60");

            entity.ToTable("genres");

            entity.Property(e => e.Id);
            entity.Property(e => e.Name);
        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
