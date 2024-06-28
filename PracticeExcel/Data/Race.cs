using Microsoft.EntityFrameworkCore;
using PracticeExcel.Models;

namespace PracticeExcel.Data
{
    public class Race : DbContext
    {
        public Race(DbContextOptions<Race> options) : base(options)
        {
        }

        public DbSet<Bike> Bikes { get; set; }
        public DbSet<Part> Parts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bike>().ToTable("BIKE").HasNoKey();
            modelBuilder.Entity<Part>().ToTable("PART").HasNoKey();
        }
    }
}
