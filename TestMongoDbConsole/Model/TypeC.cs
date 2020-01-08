using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;

namespace TestMongoDbConsole.Model
{
    class TypeC
    {
        public ObjectId Id { get; set; }
        public string Type { get; set; }
        public Info Info { get; set; }
    }
    class Info
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
