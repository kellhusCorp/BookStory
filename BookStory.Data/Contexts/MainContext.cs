using BookStory.Domain;
using Microsoft.EntityFrameworkCore;

namespace BookStory.Data.Contexts
{
    public class MainContext : DbContext
    {
        public MainContext()
        {
        }

        public MainContext(DbContextOptions<MainContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<AuthorShip> AuthorShips { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Book>()
                .Property(x => x.PublishYear)
                .IsRequired();

            modelBuilder.Entity<Book>()
                .Property(x => x.EditorialOfficeName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Book>()
                .HasOne<Genre>(x => x.Genre)
                .WithMany(y => y.Books)
                .HasForeignKey(x => x.GenreId);

            modelBuilder.Entity<Author>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(40);

            modelBuilder.Entity<Author>()
                .Property(x => x.BirthDate)
                .IsRequired();

            modelBuilder.Entity<AuthorShip>()
                .HasKey(x => new {x.BookId, x.AuthorId});

            modelBuilder.Entity<AuthorShip>()
                .HasOne<Book>(x => x.Book)
                .WithMany(x => x.AuthorShips)
                .HasForeignKey(x => x.BookId);

            modelBuilder.Entity<AuthorShip>()
                .HasOne<Author>(x => x.Author)
                .WithMany(x => x.AuthorShips)
                .HasForeignKey(x => x.AuthorId);

            modelBuilder.Entity<Genre>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(40);

            modelBuilder.Entity<Genre>()
                .HasMany(x => x.Books)
                .WithOne(y => y.Genre)
                .HasForeignKey(z => z.GenreId);
        }
    }
}