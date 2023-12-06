using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

// Required virker ikke for phonenumber og Fødselsdato ved opret, required er udkommenteret ellers kan vi ikke logge-ind
namespace VagtplanApp.Shared.Model
{
    public class Person
    {
        // Felter
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; } = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 24);

        [Required(ErrorMessage = "email er påkrævet.")]
        [EmailAddress(ErrorMessage = "Ugyldig email adresse.")]
        public string email { get; set; }

        [Required(ErrorMessage = "Adgangskode er påkrævet.")]
        public string password { get; set; } // Bemærk: Dette burde være hashet i en rigtig applikation

        //[Required(ErrorMessage = "phonenumber er påkrævet.")]
        public int? phonenumber { get; set; }

        //[Required(ErrorMessage = "Fornavn er påkrævet.")]
        public string? firstName { get; set; }

        //[Required(ErrorMessage = "Efternavn er påkrævet.")]
        public string? lastName { get; set; }

        //[Required(ErrorMessage = "Fødselsdato er påkrævet.")]
        public DateOnly? birthdate { get; set; }

        //[Required(ErrorMessage = "Vælg venligst et køn.")]
        public string? gender { get; set; }
        public bool isKoordinator { get; set; } = false;

    }
}
