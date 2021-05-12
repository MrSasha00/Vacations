using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using VacationsAPI.Models.Department;

namespace VacationsAPI.Models.Department
{
    interface IDepartmentRepository
    {
        Task<DepartmentEntity> Insert(DepartmentEntity department);
        Task<DepartmentEntity> Get(Guid id);
        Task<List<DepartmentEntity>> GetAllDepartment();
        Task UpdateDepartment(DepartmentEntity department);
        Task<DepartmentEntity> DeleteDepartment(Guid id);
    }
}
