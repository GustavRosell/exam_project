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

        // Opdaterer vagt med en ny person
        public async Task UpdateShift(string vagtId, string personId)
        {
            // Hent alle Vagter fra databasen
            var alleVagter = await VagterCollection.Find(new BsonDocument()).ToListAsync();

            // Find den specifikke Vagter-instans
            Vagter fundetVagt = null;
            foreach (var vagt in alleVagter)
            {
                if (vagt.id == vagtId)
                {
                    fundetVagt = vagt;
                    break;
                }
            }

            if (fundetVagt != null && !fundetVagt.assignedPersons.Contains(personId))
            {
                // Tilføj personId til assignedPersons-listen
                fundetVagt.assignedPersons.Add(personId);

                // Opdater Vagter-instansen i databasen
                var filter = new BsonDocument("_id", new ObjectId(vagtId));
                await VagterCollection.ReplaceOneAsync(filter, fundetVagt);
            }
        }
    }
}
