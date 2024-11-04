using DogHouseService.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace DogHouseService.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Dog> Dogs { get; set; }
        
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
          
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dog>(entity =>
            {
                entity.HasKey(e => e.name); 
                entity.Property(e => e.color).IsRequired(); 
                entity.Property(e => e.tail_length).IsRequired();
                entity.Property(e=> e.weight).IsRequired();
                
            });
        }
    }
}
