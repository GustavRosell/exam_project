using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VagtplanApp.Shared.Model
{
    public class Shift
    {       
        [BsonId] // Unikt ID for hver vagt, genereret og formateret til at passe MongoDB's ObjectId
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; } = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 24);
        public List<string> assignedPersons { get; set; } = new List<string>(); // Liste over ID'er for personer, der er tildelt til vagten 
        public DateTime date { get; set; } = DateTime.Today; // Dato for vagt
        public DateTime startTime { get; set; } // Starttidspunkt for vagt
        public DateTime endTime { get; set; } // Sluttidspunkt for vagt
        public int numberOfPersons { get; set; } // Antal personer for vagt
        public Priority priority { get; set; } = Priority.Normal; // Prioriteten for vagten, defineret som enum
        public bool IsLocked { get; set; } // Angiver, om en vagt er låst af koordinatoren
    }

    // Priority: Definerer prioritetsniveauer for vagter.
    public enum Priority
    {
        Lav,
        Normal,
        Høj
    }
}
