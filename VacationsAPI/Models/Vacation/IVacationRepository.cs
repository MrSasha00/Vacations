using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacationsAPI.Models.Vacation
{
    interface IVacationRepository
    {
        VacationEntity Insert(VacationEntity vacation);
        VacationEntity Get(Guid id);
        List<VacationEntity> GetAllVacationsByWorker(Guid workerId);
        List<VacationEntity> GetAllVacations();
        void UpdateVacation(VacationEntity newVacation);
        VacationEntity Delete(Guid id);
    }
}
