using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;

namespace TestMongoDbConsole.Model
{
    class TypeA
    {
        public ObjectId Id { get; set; }
        public string valueA { get; set; }
        public int Prop1 { get; set; }
        public string type { get; set; }
    }
}
