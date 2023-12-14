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
            // Opretter et filter, der matcher dokumenter i MongoDB ved at sammenligne "_id" feltet med shiftId. 
            // Konverterer shiftId til et ObjectId, da MongoDB kræver denne datatype for "_id".
            var filter = new BsonDocument("_id", new ObjectId(shiftId));

            // Tilføjer personId til assignedPersons i MongoDB
            var update = new BsonDocument("$addToSet", new BsonDocument("assignedPersons", personId));
            await shiftCollection.UpdateOneAsync(filter, update);
        }

        // Henter en liste af alle de vagter som en frivillig er tilknyttet
        public async Task<List<Shift>> GetShiftsByPersonId(string personId)
        {
            // Opretter et filter, der søger efter vagter, hvor 'personId' er til stede i 'assignedPersons' arrayet.
            var filter = new BsonDocument("assignedPersons", new BsonDocument("$eq", personId));

            // Anvender filteret til at finde og returnere en liste af 'Shift' objekter, hvor den angivne person er tildelt.
            var shiftList = await shiftCollection.Find(filter).ToListAsync();

            return shiftList;
        }

        // Fjerner en person fra en vagt
        public async Task RemovePersonFromShift(string shiftId, string personId)
        {
            // Opretter et filter for at identificere det specifikke vagtdokument i databasen ved hjælp af shiftId.
            // Konverterer shiftId til et ObjectId, som er det krævede format for MongoDB's _id felt.
            var filter = new BsonDocument("_id", new ObjectId(shiftId));

            // Definerer en opdateringsoperation, der fjerner personId fra 'assignedPersons' arrayet i det matchende dokument.
            var update = new BsonDocument("$pull", new BsonDocument("assignedPersons", personId));

            // Udfører opdateringen på det identificerede dokument i databasen.
            await shiftCollection.UpdateOneAsync(filter, update);
        }

        // Opdaterer en vagts oplysninger
        public async Task UpdateShift(Shift updatedShift)
        {
            // Opretter et filter for at identificere det specifikke vagtdokument i databasen ved hjælp af dens id.
            // Konverterer updatedShift.id til et ObjectId, som er det krævede format for MongoDB's _id felt.
            var filter = new BsonDocument("_id", new ObjectId(updatedShift.id));

            // Definerer en opdateringsoperation, der sætter de nye værdier for vagtens forskellige felter.
            var update = new BsonDocument("$set", new BsonDocument
            {
                { "date", updatedShift.date.ToLocalTime() }, 
                { "startTime", updatedShift.startTime }, 
                { "endTime", updatedShift.endTime }, 
                { "numberOfPersons", updatedShift.numberOfPersons },
                { "priority", updatedShift.priority },
                { "IsLocked", updatedShift.IsLocked }
            });

            await shiftCollection.UpdateOneAsync(filter, update);
        }

        // Sletter en vagt fra databasen
        public async Task DeleteShift(string shiftId)
        {
            // Opretter et filter til at identificere det specifikke vagtdokument i databasen ved hjælp af shiftId.
            // Konverterer shiftId til et ObjectId, som er det krævede format for MongoDB's _id felt.
            var filter = new BsonDocument("_id", new ObjectId(shiftId));

            // Udfører sletning af det dokument, der matcher filteret, fra shiftCollection.
            await shiftCollection.DeleteOneAsync(filter);
        }

    }
}
