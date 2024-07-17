using HDigital.Models;
using Microsoft.EntityFrameworkCore;

namespace HDigital.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }

        public DbSet<Drawing> Drawings { get; set; }

        public DbSet<Angajati> Angajati { get; set; }
        public DbSet<Vehicule> Vehicule { get; set; }
        public DbSet<Transport> Transport { get; set; }
        public DbSet<Utilaje> Utilaje { get; set; }

        public DbSet<Resurse> Resurse { get; set; } 


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Drawing>().ToTable("drawings");
            modelBuilder.Entity<Angajati>().ToTable("angajati");
            modelBuilder.Entity<Vehicule>().ToTable("vehicule");
            modelBuilder.Entity<Utilaje>().ToTable("utilaje");
            modelBuilder.Entity<Transport>().ToTable("Transport");
            modelBuilder.Entity<Resurse>().ToTable("Resurse");
            modelBuilder.Entity<Drawing>()
        .HasOne(d => d.User) 
        .WithMany(u => u.Drawings) 
        .HasForeignKey(d => d.UserId);
            
        }
    }
}
