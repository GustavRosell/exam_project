using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Concurrent;
using VagtplanApp.Shared.Model;

namespace VagtplanApp.Server.Repositories
{
    public class VagterRepository : IVagterRepository
    {

        private readonly IMongoCollection<Vagter> VagterCollection;

        public VagterRepository()
        {
            MongoClient client = new MongoClient(@"mongodb+srv://Adgang:ViSkalHaveAdgang123@cluster0.2szl4mg.mongodb.net/");
            IMongoDatabase database = client.GetDatabase("festival");
            VagterCollection = database.GetCollection<Vagter>("vagter");
        }

        // Henter alle personer
        public List<Vagter> GetAll()
        {
            // Finder alle personer i MongoDB-samlingen og gemmer dem i en liste
            var VagterList = VagterCollection.Find(new BsonDocument()).ToList();

            // Returnere listen af personer.
            return VagterList;
        }

        // Opretter person
        public async Task AddVagter(Vagter vagter)
        {
            await VagterCollection.InsertOneAsync(vagter);
        }

        // Frivillige kan tage vagter
        public async Task TakeShift(string vagtId, string personId)
        {
            // Filter der matcher documenter i mongoDB ("_id") med vores parameter vagtId, som skal konverters til et ObjectId pga MongoDB
            var filter = new BsonDocument("_id", new ObjectId(vagtId));

            // Tilføjer personId, til assignedPersons i DB
            var update = new BsonDocument("$addToSet", new BsonDocument("assignedPersons", personId));
            await VagterCollection.UpdateOneAsync(filter, update);
        }


        // Henter en liste af alle de vagter som en frivillig er tilknyttet
        public async Task<List<Vagter>> GetShiftsByPersonId(string personId)
        {
            var filter = new BsonDocument("assignedPersons", new BsonDocument("$eq", personId));
            var vagterList = await VagterCollection.Find(filter).ToListAsync();
            return vagterList;
        }
    }
}
