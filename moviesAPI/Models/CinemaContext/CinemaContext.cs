using Microsoft.EntityFrameworkCore;
using moviesAPI.Models.db;

namespace moviesAPI.Models.CinemaContext;

public partial class CinemaContext : DbContext
{
    public CinemaContext()
    {
    }

    public CinemaContext(DbContextOptions<CinemaContext> options)
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
        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
