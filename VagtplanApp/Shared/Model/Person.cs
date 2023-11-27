
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VagtplanApp.Shared.Model
{
    public class Person
    {
        // Felter

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; private set; }

        public string Email { get; set; }
        public string ForNavn { get; set; }
        public string EfterNavn { get; set; }
        public DateOnly FødselsDato { get; set; }
        public string By { get; set; }
        public string Land { get; set; }
        public string Køn { get; set; }
        public bool isKoordinator { get; set; }
    }
}
