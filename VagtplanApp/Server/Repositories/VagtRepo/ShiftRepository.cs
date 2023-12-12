using MongoDB.Bson;
using MongoDB.Driver;
using VagtplanApp.Shared.Model;

namespace VagtplanApp.Server.Repositories
{
    // ShiftRepository: Håndterer interaktionen med MongoDB for vagt-relateret data.
    public class ShiftRepository : IShiftRepository
    {

        private readonly IMongoCollection<Shift> shiftCollection;

        public ShiftRepository()
        {
            // Opretter forbindelse til MongoDB og initialiserer shiftCollection
            MongoClient client = new MongoClient(@"mongodb+srv://Adgang:ViSkalHaveAdgang123@cluster0.2szl4mg.mongodb.net/");
            IMongoDatabase database = client.GetDatabase("festival");
            shiftCollection = database.GetCollection<Shift>("vagter");
        }

        // Henter alle vagter fra databasen
        public List<Shift> GetAllShifts()
        {
            // Finder alle personer i MongoDB-samlingen og gemmer dem i en liste
            var shiftList = shiftCollection.Find(new BsonDocument()).ToList();

            // Returnere listen af personer.
            return shiftList;
        }

        // Opretter en ny vagt i databasen
        public async Task CreateShift(Shift shift)
        {
            await shiftCollection.InsertOneAsync(shift);
        }

        // Tilføjer en person  til en specifik vagt
        public async Task AddPersonToShift(string shiftId, string personId)
        {
            // Filter der matcher documenter i mongoDB ("_id") med vores parameter vagtId, som skal konverters til et ObjectId pga MongoDB
            var filter = new BsonDocument("_id", new ObjectId(shiftId));

            // Tilføjer personId, til assignedPersons i DB
            var update = new BsonDocument("$addToSet", new BsonDocument("assignedPersons", personId));
            await shiftCollection.UpdateOneAsync(filter, update);
        }


        // Henter en liste af alle de vagter som en frivillig er tilknyttet
        public async Task<List<Shift>> GetShiftsByPersonId(string personId)
        {
            var filter = new BsonDocument("assignedPersons", new BsonDocument("$eq", personId));
            var shiftList = await shiftCollection.Find(filter).ToListAsync();
            return shiftList;
        }

        // Fjerner en person fra en vagt
        public async Task RemovePersonFromShift(string shiftId, string personId)
        {
            // Filter der matcher dokumenter i MongoDB ("_id") med vores parameter shiftId
            var filter = new BsonDocument("_id", new ObjectId(shiftId));

            // Opdateringskommandoen fjerner personId fra listen 'assignedPersons'
            var update = new BsonDocument("$pull", new BsonDocument("assignedPersons", personId));

            // Udfører opdateringsoperationen
            await shiftCollection.UpdateOneAsync(filter, update);
        }

        // Opdaterer en vagts oplysninger
        public async Task UpdateShift(Shift updatedShift)
        {
            var filter = new BsonDocument("_id", new ObjectId(updatedShift.id));

            var update = new BsonDocument("$set", new BsonDocument
            {
                { "date", updatedShift.date.ToLocalTime() }, // MongoDB forstår DateTime, men ikke DateOnly
                { "startTime", updatedShift.startTime }, // Antager at dette allerede er et DateTime objekt
                { "endTime", updatedShift.endTime }, // Antager at dette allerede er et DateTime objekt
                { "numberOfPersons", updatedShift.numberOfPersons },
                { "priority", updatedShift.priority },
                { "IsLocked", updatedShift.IsLocked }
            });

            await shiftCollection.UpdateOneAsync(filter, update);
        }

        // Sletter en vagt fra databasen
        public async Task DeleteShift(string shiftId)
        {
            // Filter to match the document with the given shiftId
            var filter = new BsonDocument("_id", new ObjectId(shiftId));

            // Delete the document that matches the filter
            await shiftCollection.DeleteOneAsync(filter);
        }

    }
}
