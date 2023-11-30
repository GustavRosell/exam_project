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


    }
}
