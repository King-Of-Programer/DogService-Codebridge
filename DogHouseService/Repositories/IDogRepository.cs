using DogHouseService.Data.Entity;

namespace DogHouseService.Repositories
{
    public interface IDogRepository
    {
        IQueryable<Dog> GetAllDogsQuery();
        Task AddDogAsync(Dog dog);
        Task<bool> ExistsByNameAsync(string name);
    }
}
