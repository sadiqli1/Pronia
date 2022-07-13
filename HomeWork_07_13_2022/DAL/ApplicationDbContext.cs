using System.Linq;
using HomeWork_07_13_2022.Models;
using Microsoft.EntityFrameworkCore;
using Pronia.Models;

namespace HomeWork_07_13_2022.DAL
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<PlantCategory> PlantCategories { get; set; }
        public DbSet<PlantImage> PlantImages { get; set; }
        public DbSet<PlantInformation> PlantInformations { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PlantTag> PlantTags { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var item in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties()
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
                )
            {
                item.SetColumnType("decimal(6,2)");
                //item.SetDefaultValue(20.5m);
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
