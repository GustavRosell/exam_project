using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

// Required virker ikke for Telefonnummer og Fødselsdato
namespace VagtplanApp.Shared.Model
{
    public class Person
    {
        // Felter
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 24);
        public int PersonalId { get; set; }

        [Required(ErrorMessage = "Email er påkrævet.")]
        [EmailAddress(ErrorMessage = "Ugyldig email adresse.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Adgangskode er påkrævet.")]
        public string Password { get; set; } // Bemærk: Dette burde være hashet i en rigtig applikation

        [Required(ErrorMessage = "Telefonnummer er påkrævet.")]
        public int Telefonnummer { get; set; }

        [Required(ErrorMessage = "Fornavn er påkrævet.")]
        public string ForNavn { get; set; }

        [Required(ErrorMessage = "Efternavn er påkrævet.")]
        public string EfterNavn { get; set; }

        [Required(ErrorMessage = "Fødselsdato er påkrævet.")]
        public DateOnly FødselsDato { get; set; }

        [Required(ErrorMessage = "Vælg venligst et køn.")]
        public string Køn { get; set; }
        public bool isKoordinator { get; set; } = false;

    }
}
