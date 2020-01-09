using MongoDB.Bson;
using MongoDB.Driver;

namespace TestMongoDbConsole
{
    public static class MongoDbHelper
    {
        public const string ClassTypeField = @"obj_type";

        /// <summary>
        /// Create a filter that can get MongoDB document
        /// </summary>
        /// <typeparam name="T">The mapping POCO class</typeparam>
        /// <param name="key">BSON document field key</param>
        /// <param name="value">BSON document field value</param>
        /// <returns></returns>
        public static FilterDefinition<T> CreateTypedFilter<T>(string key = ClassTypeField, string value = null) where T : class
        {
            if (string.IsNullOrEmpty(value))
            {
                value = typeof(T).Name;
            }

            return TypedFilterBuilder<T>().Eq(key, value);
        }

        /// <summary>
        /// Create FindFluent interface for specified POCO MongoDB collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="typedFilterDefinition">Optional, if non-provided, will use default</param>
        /// <returns></returns>
        public static IFindFluent<T, T> TypedFindFluent<T>(this IMongoCollection<T> collection, FilterDefinition<T> typedFilterDefinition = null) where T : class
        {
            if (typedFilterDefinition == null)
            {
                typedFilterDefinition = CreateTypedFilter<T>();
            }

            return collection.Find(typedFilterDefinition);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string CreateTypedKeyIndex<T>(this IMongoCollection<T> collection, string key = ClassTypeField) where T : class
        {
            foreach(var index in collection.Indexes.List().ToList())
            {
                if (index.GetElement("name").Value == $"{key}_hashed")
                {
                    return string.Empty;
                }
            }

            return collection.Indexes.CreateOne(new CreateIndexModel<T>(Builders<T>.IndexKeys.Hashed(key)));
        }


        /// <summary>
        /// Helper method of Filter Definition Builder for POCO MongoDB collection
        /// </summary>
        /// <typeparam name="T">The mapping POCO class</typeparam>
        /// <returns></returns>
        public static FilterDefinitionBuilder<T> TypedFilterBuilder<T>() where T : class
        {
            return Builders<T>.Filter;
        }

        /// <summary>
        /// Create Filter Definition Builder of specified POCO MongoDB collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_"></param>
        /// <returns></returns>
        public static FilterDefinitionBuilder<T> TypedFilterBuilder<T>(this IMongoCollection<T> _) where T : class
        {
            return TypedFilterBuilder<T>();
        }

        /// <summary>
        /// Create Update Definition Builder of specified POCO MongoDB collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_"></param>
        /// <returns></returns>
        public static UpdateDefinitionBuilder<T> TypedUpdateBuilder<T>(this IMongoCollection<T> _) where T : class
        {
            return Builders<T>.Update;
        }

        /// <summary>
        /// Get a POCO type mapping MongoDB collection
        /// </summary>
        /// <typeparam name="T">The mapping POCO class</typeparam>
        /// <param name="database"></param>
        /// <param name="collectionName">Actual MongoDB collection name</param>
        /// <returns></returns>
        public static IMongoCollection<T> GetTypedCollection<T>(this IMongoDatabase database, string collectionName) where T : class
        {
            var guidSettings = new MongoCollectionSettings
            {
                GuidRepresentation = GuidRepresentation.Standard
            };

            return database.GetCollection<T>(collectionName, guidSettings);
        }
    }
}
