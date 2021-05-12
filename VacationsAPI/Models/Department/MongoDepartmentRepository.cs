using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace VacationsAPI.Models.Department
{
    public class MongoDepartmentRepository : IDepartmentRepository
    {
        private readonly IMongoCollection<DepartmentEntity> departmentCollection;
        public MongoDepartmentRepository(IDepartmentDatabaseSettings setting)
        {
            var client = new MongoClient(setting.ConnectionString);
            var database = client.GetDatabase(setting.DatabaseName);
            departmentCollection = database.GetCollection<DepartmentEntity>(setting.DepartmentsCollectionName);
        }
        public async Task<DepartmentEntity> DeleteDepartment(Guid id)
        {
           return await departmentCollection.FindOneAndDeleteAsync(x => x.DepartmentId == id);
        }

        public async Task<DepartmentEntity> Get(Guid id)
        {
            return await departmentCollection.Find(x => x.DepartmentId == id).FirstOrDefaultAsync();
        }

        public async Task<List<DepartmentEntity>> GetAllDepartment()
        {
            return await departmentCollection.Find(x => true).ToListAsync();
        }

        public async Task<DepartmentEntity> Insert(DepartmentEntity department)
        {
            await departmentCollection.InsertOneAsync(department);
            return department;
        }

        public async Task UpdateDepartment(DepartmentEntity department)
        {
            await departmentCollection.ReplaceOneAsync(x => x.DepartmentId == department.DepartmentId, department);
        }
    }
}
