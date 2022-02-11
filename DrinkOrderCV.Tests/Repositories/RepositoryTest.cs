using JsonFlatFileDataStore;
using System.IO;

namespace DrinkOrderCV.Tests.Repositories
{
    public class RepositoryTest
    {
        public DataStore GetTestDatabase()
        {
            var databaseTest = "Data_Testing.json";
            if (File.Exists(databaseTest)) File.Delete(databaseTest);
            return new DataStore(databaseTest);
        }
    }
}