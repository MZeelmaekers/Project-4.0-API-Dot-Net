using Microsoft.EntityFrameworkCore;
using Project40_API_Dot_NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<CameraBox>().ToTable("CameraBox");
            modelBuilder.Entity<Result>().ToTable("Result");
            modelBuilder.Entity<Plant>().ToTable("Plant");
        }
    }
}
