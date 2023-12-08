using System;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VagtplanApp.Shared.Model
{
    public class Shift
    {
        // Felter

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; } = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 24);
        public DateTime date { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public int numberOfPersons { get; set; } 
        public List<string> assignedPersons { get; set; } = new List<string>(); 
        public Priority priority { get; set; }
    }

    public enum Priority // Denne enumerable bruges til at rangere shifts efter "Lav", "Normal", "Høj"
    {
        Lav,
        Normal,
        Høj
    }
}
