using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace VacationsAPI.Models.Worker
{
    public class MongoWorkerRepository : IWorkerRepository
    {
        private readonly IMongoCollection<WorkerEntity> workersCollection;
        public MongoWorkerRepository(IWorkerDatabaseSettings setting)
        {
            var client = new MongoClient(setting.ConnectionString);
            var database = client.GetDatabase(setting.DatabaseName);
            workersCollection = database.GetCollection<WorkerEntity>(setting.WorkersCollectionName);
        }
        public async Task<WorkerEntity> DeleteWorker(Guid id)
        {
            return await workersCollection.FindOneAndDeleteAsync(x => x.WorkerId == id);
        }

        public async Task<WorkerEntity> Get(Guid id)
        {
            return await workersCollection.Find(x => x.WorkerId == id).SingleOrDefaultAsync();
        }

        public async Task<List<WorkerEntity>> GetAllWorkers()
        {
            return await workersCollection.Find(x => true).ToListAsync();
        }

        public async Task<List<WorkerEntity>> GetAllWorkersByDepartment(Guid departmentId)
        {
            return await workersCollection.Find(x => x.DepartmentId == departmentId).ToListAsync();
        }

        public async Task<WorkerEntity> Insert(WorkerEntity worker)
        {
            await workersCollection.InsertOneAsync(worker);
            return worker;
        }

        public async Task UpdateWorker(WorkerEntity newWorker)
        {
            await workersCollection.ReplaceOneAsync(x => x.WorkerId == newWorker.WorkerId, newWorker);
        }
    }
}
