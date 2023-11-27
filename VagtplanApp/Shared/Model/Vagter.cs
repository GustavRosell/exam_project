using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VagtplanApp.Shared.Model
{
    public class Vagter
    {
        // Felter

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; private set; }

        public DateTime StartTid { get; private set; }
        public DateTime SlutTid { get; private set; }
        public int MinAlder { get; private set; }
        public string AntalPersoner { get; private set; }  // antager dette er et antal, kunne også være en liste af person-ID'er?

        // Konstruktør

        public Vagter(DateTime startTid, DateTime slutTid, int minAlder, string antalPersoner)
        {
            StartTid = startTid;
            SlutTid = slutTid;
            MinAlder = minAlder;
            AntalPersoner = antalPersoner;
        }
    }
}
