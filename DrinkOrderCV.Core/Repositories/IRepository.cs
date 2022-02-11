namespace DrinkOrderCV.Core.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetAsync(string id);
        Task<IEnumerable<T>> GetAsync();
        Task<T> SaveAsync(T item);
        string CollectionName { get; set; }
    }
}