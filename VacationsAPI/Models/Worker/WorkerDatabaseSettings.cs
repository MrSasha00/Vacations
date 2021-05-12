using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacationsAPI.Models.Worker
{
    public class WorkerDatabaseSettings : IWorkerDatabaseSettings
    {
        public string WorkersCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
    public interface IWorkerDatabaseSettings
    {
        string WorkersCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
