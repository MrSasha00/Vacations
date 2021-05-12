using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacationsAPI.Models.Vacation
{
    public class VacationDatabaseSettings : IVacationDatabaseSettings
    {
        public string VacationsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
    public interface IVacationDatabaseSettings
    {
        string VacationsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
