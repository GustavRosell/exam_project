﻿using System;
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

        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public int minAge { get; set; }
        public int numberOfPersons { get; set; }  // antager dette er et antal, kunne også være en liste af person-ID'er?
        public List<string> assignedPersons { get; set; } = new List<string>(); // String eller int? kommer an på id

    }
}