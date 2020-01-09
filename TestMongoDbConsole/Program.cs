using System;
using System.Threading.Tasks;
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

            var typeACollection = mongoDatabase.GetTypedCollection<TypeA>("test_mix");
            typeACollection.CreateTypedKeyIndex();
            var typeBCollection = mongoDatabase.GetTypedCollection<TypeB>("test_mix");
            typeBCollection.CreateTypedKeyIndex();
            var typeCCollection = mongoDatabase.GetTypedCollection<TypeC>("test_mix");
            typeCCollection.CreateTypedKeyIndex();
            
            var typeAFilter = MongoDbHelper.CreateTypedFilter<TypeA>();
            var typeBFilter = MongoDbHelper.CreateTypedFilter<TypeB>();
            var typeCFilter = MongoDbHelper.CreateTypedFilter<TypeC>();
            
            #region Read Demo

            Console.WriteLine("=== Read Test ===");
            var typeACount = typeACollection.CountDocuments(typeAFilter);
            var typeBCount = typeBCollection.CountDocuments(typeBFilter);
            var typeCCount = typeCCollection.CountDocuments(typeCFilter);
            Console.WriteLine($"TypeA count={typeACount}, TypeB count={typeBCount}, TypeC count={typeCCount}");
            await PrintCollection(typeACollection, typeAFilter);
            await PrintCollection(typeBCollection, typeBFilter);
            await PrintCollection(typeCCollection, typeCFilter);

            #endregion


            #region Write Demo

            Console.WriteLine("\r\n=== Write Test ===");
            Console.ReadLine();

            var newBObj = new TypeB
            {
                Type = "TypeB",
                Prop1 = Guid.NewGuid(),
                ValueB = "TestB 2"
            };

            await typeBCollection.InsertOneAsync(newBObj);
            Console.WriteLine("Inserted TypeB collection:");
            await PrintCollection(typeBCollection, typeBFilter);

            #endregion


            #region Update Demo

            Console.WriteLine("\r\n=== Update Test ===");
            Console.ReadLine();
            Console.WriteLine("Origin TypeC collection:");
            await PrintCollection(typeCCollection, typeCFilter);

            var typeC1st = typeCCollection.TypedFindFluent().First();

            var targetFilter = typeCFilter & typeCCollection.TypedFilterBuilder().Eq(nameof(TypeC.Prop1), typeC1st.Prop1);

            var update = typeCCollection.TypedUpdateBuilder().Set(nameof(TypeC.Prop1), typeC1st.Prop1 + 1);

            var updateResult = await typeCCollection.UpdateOneAsync(targetFilter, update);

            Console.WriteLine($"Update on \"TypeC\" collection, updateResult= {updateResult}");
            Console.WriteLine("Modified TypeC collection:");
            await PrintCollection(typeCCollection, typeCFilter);

            #endregion


            #region Delete Demo

            Console.WriteLine("\r\n=== Delete Test ===");
            Console.ReadLine();
            Console.WriteLine("Origin TypeB collection:");
            await PrintCollection(typeBCollection, typeBFilter);

            var newBObjFilter = typeBFilter & typeBCollection.TypedFilterBuilder().Eq(nameof(TypeB.ValueB), newBObj.ValueB);

            var deleteResult = typeBCollection.DeleteOne(newBObjFilter);
            Console.WriteLine($"Delete on \"TypeB\" collection, deleteResult= {deleteResult}");
            
            Console.WriteLine("Modified TypeB collection:");
            await PrintCollection(typeBCollection, typeBFilter);

            #endregion
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
