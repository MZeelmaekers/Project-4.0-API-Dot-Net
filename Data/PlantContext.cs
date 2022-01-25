using Microsoft.EntityFrameworkCore;
using Project40_API_Dot_NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Project40_API_Dot_NET.Data
{
    public class PlantContext: DbContext
    {
        public PlantContext(DbContextOptions<PlantContext> options): base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<CameraBox> CameraBoxes { get; set; }
        public DbSet<Plant> Plants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
           .HasMany<CameraBox>(p => p.CameraBoxes);

            modelBuilder.Entity<User>()
           .HasMany<Plant>(p => p.Plants);

            modelBuilder.Entity<Result>()
                .HasMany<Plant>(p => p.Plants);

            modelBuilder.Entity<CameraBox>()
                .HasOne(c => c.User);

            modelBuilder.Entity<Plant>()
                .HasOne(p => p.User);

            modelBuilder.Entity<Plant>()
                .HasOne(p => p.Result);

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<CameraBox>().ToTable("CameraBox");
            modelBuilder.Entity<Result>().ToTable("Result");
            modelBuilder.Entity<Plant>().ToTable("Plant");
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {

            var entries = ChangeTracker.Entries().Where(E => E.State == EntityState.Added).ToList();

            foreach (var entityEntry in entries)
            {
                entityEntry.Property("CreatedAt").CurrentValue = DateTime.Now;
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
