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
        public string Id { get; set; } = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 24);

        public DateTime StartTid { get; set; }
        public DateTime SlutTid { get; set; }
        public int MinAlder { get; set; }
        public string AntalPersoner { get; set; }  // antager dette er et antal, kunne også være en liste af person-ID'er?
    }
}
