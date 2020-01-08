using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using TestMongoDbConsole.Model;

namespace TestMongoDbConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var mongoDatabase = client.GetDatabase("test_db");
            var typeACollection = mongoDatabase.GetCollection<TypeA>("test_mix");
            var typeBCollection = mongoDatabase.GetCollection<TypeB>("test_mix");

            var typeAFilter = MongoDbHelper.CreateTypedFilter<TypeA>();
            var typeBFilter = MongoDbHelper.CreateTypedFilter<TypeB>();

            Console.WriteLine("=== Read Test ===");
            var typeACount = typeACollection.CountDocuments(typeAFilter);
            var typeBCount = typeBCollection.CountDocuments(typeBFilter);
            Console.WriteLine($"TypeA count={typeACount}, TypeB count={typeBCount}");

            var typeACursor = typeACollection.Find(typeAFilter).ToCursor();

            await PrintCollection(typeACollection, typeAFilter);
            await PrintCollection(typeBCollection, typeBFilter);

            Console.WriteLine("\r\n=== Write Test ===");
            Console.ReadLine();

            var newB = new TypeB
            {
                Type = "TypeB", Prop1 = Guid.NewGuid(), ValueB = "TestB 2"
            };

            await typeBCollection.InsertOneAsync(newB);
            Console.WriteLine("B collection:");

            await PrintCollection(typeBCollection, typeBFilter);
        }

        private static async Task PrintCollection<T>(IMongoCollection<T> collection, FilterDefinition<T> filter) where T : class
        {
            await collection.Find(filter).ForEachAsync(x =>
            {
                var jsonStr = JsonConvert.SerializeObject(x, Formatting.Indented);
                Console.WriteLine(jsonStr);
            });
        }
    }
}
