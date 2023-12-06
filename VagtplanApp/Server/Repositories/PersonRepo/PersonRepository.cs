using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Concurrent;
using VagtplanApp.Shared.Model;

namespace VagtplanApp.Server.Repositories
{
    public class PersonRepository : IPersonRepository
    {

        private readonly IMongoCollection<Person> personCollection;

        public PersonRepository()
        {
            MongoClient client = new MongoClient(@"mongodb+srv://Adgang:ViSkalHaveAdgang123@cluster0.2szl4mg.mongodb.net/");
            IMongoDatabase database = client.GetDatabase("festival");
            personCollection = database.GetCollection<Person>("persons");
        }

        // Henter alle personer
        public List<Person> GetAllPersons()
        {
            // Finder alle personer i MongoDB-samlingen og gemmer dem i en liste
            var personList = personCollection.Find(new BsonDocument()).ToList();

            // Returnere listen af personer.
            return personList;
        }

        // Opretter person
        public async Task CreatePerson(Person person)
        {
            await personCollection.InsertOneAsync(person);
        }

        //Metode der bliver benyttet i controller, til at matche input Email med Email i MongoDB
        public async Task<Person> GetPersonByEmail(string email)
        {
            // .FirstOrDefaultAsync skal benyttes her, da Find returnere et IFindFluent interface, men ikke udfører forespørgslen
            // .FirstOrDefaultAsync vælger det første element som matcher i collectionen, ellers Null. 
            return await personCollection.Find(person => person.email == email).FirstOrDefaultAsync();
        }

        public async Task UpdatePerson(Person updatedPerson)
        {
            // Opretter et filter, der matcher dokumentet baseret på ObjectId
            var filter = new BsonDocument("_id", new ObjectId(updatedPerson.id));

            // Opretter et opdateringsdokument baseret på updatedPerson objektet
            // Bruger '$set' for at opdatere de specifikke felter i dokumentet
            var update = new BsonDocument("$set", new BsonDocument
            {
                { "email", updatedPerson.email },
                { "password", updatedPerson.password },
                { "phonenumber", updatedPerson.phonenumber }
            });
            
            // Udfører opdateringsoperationen
            await personCollection.UpdateOneAsync(filter, update);
        }
    }
}
