using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Concurrent;
using VagtplanApp.Shared.Model;

namespace VagtplanApp.Server.Repositories
{
    public class PersonRepository : IPersonRepository
    {

        private readonly IMongoCollection<Person> PersonCollection;

        public PersonRepository()
        {
            MongoClient client = new MongoClient(@"mongodb+srv://Adgang:ViSkalHaveAdgang123@cluster0.2szl4mg.mongodb.net/");
            IMongoDatabase database = client.GetDatabase("festival");
            PersonCollection = database.GetCollection<Person>("persons");
        }

        // Henter alle personer
        public List<Person> GetAll()
        {
            // Finder alle personer i MongoDB-samlingen og gemmer dem i en liste
            var PersonList = PersonCollection.Find(new BsonDocument()).ToList();

            // Returnere listen af personer.
            return PersonList;
        }

        // Opretter person
        public async Task AddPerson(Person person)
        {
            await PersonCollection.InsertOneAsync(person);
        }

        //Metode der bliver benyttet i controller, til at matche input email med email i MongoDB
        public async Task<Person> GetPersonByEmail(string email)
        {
            // .FirstOrDefaultAsync skal benyttes her, da Find returnere et IFindFluent interface, men ikke udfører forespørgslen
            // .FirstOrDefaultAsync vælger det første element som matcher i collectionen, ellers Null. 
            return await PersonCollection.Find(person => person.email == email).FirstOrDefaultAsync();
        }

        //public async Task<Person> GetLatestPerson()
        //{
        //    return await PersonCollection.Find(new BsonDocument())
        //                                .Sort("{personalId: -1}")
        //                                .Limit(1)
        //                                .FirstOrDefaultAsync();
        //}

    }
}
