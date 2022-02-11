using JsonFlatFileDataStore;
using Newtonsoft.Json;
using System.Dynamic;

namespace DrinkOrderCV.Core.Repositories.Impl
{
    public class Repository<T> where T : BaseModel
    {
        private DataStore _store; 
        private static Random random = new Random();

        public string CollectionName { get; set; }

        public Repository(DataStore store)
        {
            _store = store;
            CollectionName = typeof(T).Name;
        }

        protected async Task<bool> UpdateAsync(string key, T item) 
        {
            return await _store.GetCollection<T>().UpdateOneAsync(x=>x.Code == key, item);
            // var result = await _store.GetCollection(CollectionName)
            //     .UpdateOneAsync(key, item);
        }

        protected async Task<bool> Exist(Func<T, bool> query) 
        {
            return _store.GetCollection<T>().AsQueryable().Any(query);
            // return _store.GetCollection(CollectionName)
            //     .AsQueryable()
            //     .Any(query);
        }

        protected async Task<T> GetAsync(Func<T, bool> func)
        {
            return _store.GetCollection<T>().AsQueryable().FirstOrDefault(func);
            // return _store.GetCollection(CollectionName)
            //      .AsQueryable()
            //      .FirstOrDefault(func);
        }

        protected async Task InsertAsync(T item) 
        {
            await _store.GetCollection<T>().InsertOneAsync(item);
            // await _store.GetCollection(CollectionName)
            //     .InsertOneAsync(item);
        }

        protected async Task<T> GetIfExist(Func<T, bool> func)
        {
            if (await (Exist(func)))
            {
                return await GetAsync(func);
            }

            return default;
        }

        protected async Task<IEnumerable<T>> GetAllAsync()
        {
            IList<T> convertedItems = new List<T>();

            foreach(var item in _store.GetCollection<T>()
                .AsQueryable())
            {
                convertedItems.Add(item);
            }

            return convertedItems;
        }

        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}