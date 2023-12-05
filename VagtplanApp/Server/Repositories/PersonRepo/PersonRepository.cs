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

        //Metode der bliver benyttet i controller, til at matche input email med email i MongoDB
        public async Task<Person> GetPersonByEmail(string email)
        {
            // .FirstOrDefaultAsync skal benyttes her, da Find returnere et IFindFluent interface, men ikke udfører forespørgslen
            // .FirstOrDefaultAsync vælger det første element som matcher i collectionen, ellers Null. 
            return await personCollection.Find(person => person.email == email).FirstOrDefaultAsync();
        }

        public async Task UpdatePerson(Person updatePerson)
        {
            var filter = Builders<Person>.Filter.Eq(p => p.id, updatePerson.id); // Bruger email for filter, men det betyder at vi ikke kan ændre emailen
            var update = Builders<Person>.Update
               .Set(p => p.firstName, updatePerson.firstName)
               .Set(p => p.lastName, updatePerson.lastName)
               //.Set(p => p.email, updatePerson.email) 
               .Set(p => p.password, updatePerson.password);
            // Add other properties as needed

            await personCollection.UpdateOneAsync(filter, update);
        }
    }
}
