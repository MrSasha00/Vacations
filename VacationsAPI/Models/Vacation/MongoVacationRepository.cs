using System;
using System.Collections.Generic;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace VacationsAPI.Models.Vacation
{
    public class MongoVacationRepository //: IVacationRepository
    {
        private readonly IMongoCollection<VacationEntity> vacationCollection;
        public MongoVacationRepository(IVacationDatabaseSettings setting)
        {
            var client = new MongoClient(setting.ConnectionString);
            var database = client.GetDatabase(setting.DatabaseName);
            vacationCollection = database.GetCollection<VacationEntity>(setting.VacationsCollectionName);
        }
        public async Task<VacationEntity> Delete(Guid id)
        {
            return await vacationCollection.FindOneAndDeleteAsync(x => x.VacationId == id);
        }

        public async Task<VacationEntity> Get(Guid id)
        {
            return await vacationCollection.Find(x => x.VacationId == id).FirstOrDefaultAsync();
        }

        public async Task<List<VacationEntity>> GetAllVacations()
        {
            return await vacationCollection.Find(x => true).ToListAsync();
        }

        public async Task<List<VacationEntity>> GetAllVacationsByWorker(Guid workerId)
        {
            return await vacationCollection.Find(x => x.WorkerId == workerId).ToListAsync();
        }

        public async Task<VacationEntity> Insert(VacationEntity vacation)
        {
            await vacationCollection.InsertOneAsync(vacation);
            return vacation;
        }

        public async Task UpdateVacation(VacationEntity newVacation)
        {
            await vacationCollection.ReplaceOneAsync(x => x.VacationId == newVacation.VacationId, newVacation);
        }
    }
}
