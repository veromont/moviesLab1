using Microsoft.EntityFrameworkCore;
using moviesAPI.Models.EntityModels;

namespace moviesAPI.Models.CinemaContext;

public partial class CinemaContext : DbContext
{
    public CinemaContext()
    {
        Database.EnsureCreated();
    }

    public CinemaContext(DbContextOptions<CinemaContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public virtual DbSet<Hall> Halls { get; set; }
    public virtual DbSet<Movie> Movies { get; set; }
    public virtual DbSet<Session> Sessions { get; set; }
    public virtual DbSet<Ticket> Tickets { get; set; }
    public virtual DbSet<Genre> Genres { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>(entity =>
        {
            entity.ToTable("genres");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });
        modelBuilder.Entity<Hall>(entity =>
        {
            entity.ToTable("halls");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.IsAvailable).HasColumnName("isAvailable");
        });
        modelBuilder.Entity<Movie>(entity =>
        {
            entity.ToTable("movies");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Director).HasColumnName("director");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.ReleaseDate).HasColumnName("releaseDate");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.GenreId).HasColumnName("genreId");

            entity.HasOne(m => m.Genre)
                  .WithMany(g => g.Movies)
                  .HasForeignKey(m => m.GenreId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.ToTable("tickets");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.SessionId).HasColumnName("sessionId");
            entity.Property(e => e.SeatNumber).HasColumnName("seatNumber");

            entity.HasOne(t => t.Session)
                  .WithMany(s => s.SessionTickets)
                  .HasForeignKey(s => s.SessionId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<Session>(entity =>
        {
            entity.ToTable("sessions");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MovieId).HasColumnName("movieId");
            entity.Property(e => e.StartTime).HasColumnName("startTime");
            entity.Property(e => e.EndTime).HasColumnName("endTime");
            entity.Property(e => e.HallId).HasColumnName("hallId");
            entity.Property(e => e.Price).HasColumnName("price");

            entity.HasOne(s => s.Movie)
                  .WithMany(m => m.Sessions)
                  .HasForeignKey(s => s.MovieId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(s => s.Hall)
                  .WithMany(h => h.Sessions)
                  .HasForeignKey(s => s.HallId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<ClientLikesGenre>(entity =>
        {
            entity.ToTable("client_genre");
            entity.Property(e => e.Username).HasColumnName("username");
            entity.Property(e => e.GenreId).HasColumnName("genreId");

            entity.HasOne(c => c.Genre)
                  .WithMany(g => g.Clients)
                  .HasForeignKey(c => c.GenreId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
