using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace VagtplanApp.Shared.Model
{
    public class Person
    {
        [BsonId] // Unikt ID for hver vagt, genereret og formateret til at passe MongoDB's ObjectId
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; } = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 24);

        // Nogle felter nullable, da vores log-in ellers ikke virker

        [EmailAddress(ErrorMessage = "Ugyldig email adresse.")]
        public string email { get; set; }  // Email for en person, required @
        public string password { get; set; } // Adgangskode for en person
        public int? phonenumber { get; set; } // Telefonnummer for en person
        public string? firstName { get; set; } // Fornavn for en person
        public string? lastName { get; set; } // Efternavn for en person
        public DateOnly? birthdate { get; set; } // Fødselsdato for en person
        public string? gender { get; set; } // Køn for en person
        public bool isKoordinator { get; set; } = false; // Definer om en person er koordinator
    }
}
