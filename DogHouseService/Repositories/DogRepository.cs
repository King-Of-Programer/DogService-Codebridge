using DogHouseService.Data;
using DogHouseService.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace DogHouseService.Repositories
{
    public class DogRepository : IDogRepository
    {
        private readonly DataContext _context;

        public DogRepository(DataContext context)
        {
            _context = context;
        }

        public IQueryable<Dog> GetAllDogsQuery()
        {
            return _context.Dogs.AsQueryable();
        }
        public async Task AddDogAsync(Dog dog)
        {
            await _context.Dogs.AddAsync(dog);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Dogs.AnyAsync(d => d.name == name);
        }
    }
}
