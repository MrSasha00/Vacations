using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacationsAPI.Models.Worker
{
    interface IWorkerRepository
    {
        Task<WorkerEntity> Insert(WorkerEntity worker);
        Task<WorkerEntity> Get(Guid id);
        Task<List<WorkerEntity>> GetAllWorkersByDepartment(Guid departmentId);
        Task<List<WorkerEntity>> GetAllWorkers();
        Task UpdateWorker(WorkerEntity newWorker);
        Task<WorkerEntity> DeleteWorker(Guid id);
    }
}
