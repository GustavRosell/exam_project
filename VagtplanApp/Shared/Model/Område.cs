using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VagtplanApp.Shared.Model
{
    public class Område
    {
        // Felter

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; private set; }

        public string OmrådeNavn { get; private set; }

        // Konstruktør
        public Område(string områdeNavn)
        {
            OmrådeNavn = områdeNavn;
        }
    }
}
