using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TestMongoDbConsole.Model
{
    class TypeB
    {
        public ObjectId Id { get; set; }
        [BsonElement("valueB")]
        public string ValueB { get; set; }
        public Guid Prop1 { get; set; }

        [BsonElement(MongoDbHelper.ClassTypeField)]
        public string Type { get; set; }
    }
}
