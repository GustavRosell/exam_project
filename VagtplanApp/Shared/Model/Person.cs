﻿
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VagtplanApp.Shared.Model
{
    public class Person
    {
        // Felter
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 24);
        public string Email { get; set; } 
        public int Telefonnummer { get; set; }
        public string ForNavn { get; set; }
        public string EfterNavn { get; set; }
        public DateOnly FødselsDato { get; set; }
        public string Køn { get; set; }
        public bool isKoordinator { get; set; }
    }
}
