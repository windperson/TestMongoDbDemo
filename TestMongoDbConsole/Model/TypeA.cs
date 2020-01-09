using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TestMongoDbConsole.Model
{
    class TypeA
    {
        public ObjectId Id { get; set; }
        public string valueA { get; set; }
        public int Prop1 { get; set; }

        [BsonElement(MongoDbHelper.ClassTypeField)]
        public string type { get; set; }
    }
}
