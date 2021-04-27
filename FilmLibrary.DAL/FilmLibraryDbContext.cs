using FilmLibrary.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmLibrary.DAL
{
    public class FilmLibraryDbContext : IdentityDbContext<AppUser>
    {
        protected FilmLibraryDbContext() { }
        public FilmLibraryDbContext(DbContextOptions<FilmLibraryDbContext> options) : base(options) { }
        public DbSet<Film> Films { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Genre> Genres { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Rating>().HasData(new Rating { ID = 1, Rate = "G - General Audiences" });
            modelBuilder.Entity<Rating>().HasData(new Rating { ID = 2, Rate = "PG - Parental Guidance Suggested" });
            modelBuilder.Entity<Rating>().HasData(new Rating { ID = 3, Rate = "PG-13 - Parents Strongly Cautioned" });
            modelBuilder.Entity<Rating>().HasData(new Rating { ID = 4, Rate = "R - Restricted" });
            modelBuilder.Entity<Rating>().HasData(new Rating { ID = 5, Rate = "NC-17 - Clearly Adult" });

            modelBuilder.Entity<Genre>().HasData(new Genre { ID = 1, Type = "Action" });
            modelBuilder.Entity<Genre>().HasData(new Genre { ID = 2, Type = "Comedy" });
            modelBuilder.Entity<Genre>().HasData(new Genre { ID = 3, Type = "Drama" });
            modelBuilder.Entity<Genre>().HasData(new Genre { ID = 4, Type = "Fantasy" });
            modelBuilder.Entity<Genre>().HasData(new Genre { ID = 5, Type = "Horror" });
        }
    }
}
