using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TestMongoDbConsole.Model
{
    class TypeC
    {
        public ObjectId Id { get; set; }

        public double Prop1 { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }
        [BsonElement("info")]
        public Info Info { get; set; }
    }

    class Info
    {
        [BsonElement("x")]
        public int X { get; set; }

        [BsonElement("y")]
        public int Y { get; set; }
    }
}
