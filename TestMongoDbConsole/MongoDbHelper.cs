using MongoDB.Driver;

namespace TestMongoDbConsole
{
    public static class MongoDbHelper
    {
        /// <summary>
        /// Create a filter that can get MongoDB document
        /// </summary>
        /// <typeparam name="T">The mapping POCO class</typeparam>
        /// <param name="key">BSON document field key</param>
        /// <param name="value">BSON document field value</param>
        /// <returns></returns>
        public static FilterDefinition<T> CreateTypedFilter<T>(string key = @"type", string value = null) where T : class
        {
            if (string.IsNullOrEmpty(value))
            {
                value = typeof(T).Name;
            }

            return Builders<T>.Filter.Eq(key, value);
        }
    }
}
