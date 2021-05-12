using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacationsAPI.Models.Department
{
    public class DepartmentDatabaseSettings : IDepartmentDatabaseSettings
    {
        public string DepartmentsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
    public interface IDepartmentDatabaseSettings
    {
        string DepartmentsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
