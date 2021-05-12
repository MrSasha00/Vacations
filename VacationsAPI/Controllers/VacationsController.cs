using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationsAPI.Models.Department;
using VacationsAPI.Models.Vacation;
using VacationsAPI.Models.Worker;

namespace VacationsAPI.Controllers
{
    public class CreateVacationDTO
    {
        public DateTime StratDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid WorkerId { get; set; }
    }
    public class UpdateVacationDTO
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class VacationsController : ControllerBase
    {
        private readonly MongoWorkerRepository _workerRepository;
        private readonly MongoVacationRepository _vacationRepository;
        private readonly MongoDepartmentRepository _departmentRepository;

        public VacationsController(MongoVacationRepository vacationRepository, MongoWorkerRepository workerRepository, MongoDepartmentRepository departmentRepository)
        {
            _workerRepository = workerRepository;
            _vacationRepository = vacationRepository;
            _departmentRepository = departmentRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVacationDTO createdVacation)
        {
            if(createdVacation == null)
            {
                return BadRequest();
            }

            var worker = await _workerRepository.Get(createdVacation.WorkerId);
            var departments = await _departmentRepository.GetAllDepartment();
            var availableCount = (int)Math.Ceiling(departments.Count * 0.2);
            var workers = await _workerRepository.GetAllWorkers();
            var vacations = await _vacationRepository.GetAllVacations();

            if (CheckCross(workers, vacations, availableCount, createdVacation))
            {
                return BadRequest("limit 20% per organization");
            }
            
            workers = await _workerRepository.GetAllWorkersByDepartment(worker.DepartmentId);
            availableCount = (int) Math.Ceiling(workers.Count * 0.2);
            var tempVacations = new List<VacationEntity>();
            foreach (var workerEntity in workers)
            {
                tempVacations.AddRange(vacations.Where(vacation => workerEntity.WorkerId == vacation.WorkerId));
            }

            if (CheckCross(workers, tempVacations, availableCount, createdVacation))
            {
                return BadRequest("limit 20% per department");
            }
            
            var vacation = new VacationEntity(createdVacation.WorkerId, createdVacation.StratDate, createdVacation.EndDate);
            await _vacationRepository.Insert(vacation);
            worker.Vacations.Add(vacation.VacationId);
            await _workerRepository.UpdateWorker(worker);
            return Created("api/Vacations/" + vacation.VacationId, vacation);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var vacation = await _vacationRepository.Get(id);
            if (vacation == null)
            {
                return NotFound();
            }
            return new JsonResult(vacation);
        }

        [HttpGet("getByDepartment")]
        public async Task<IActionResult> GetByDepartment([FromQuery(Name = "idDepartment")] Guid idDepartment)
        {
            var department = await _departmentRepository.Get(idDepartment);
            if (department == null)
            {
                return NotFound("Department");
            }
            var workers = await _workerRepository.GetAllWorkersByDepartment(idDepartment);
            var vacations  = await _vacationRepository.GetAllVacations();
            var result = new List<VacationEntity>();
            foreach (var worker in workers)
            {
                result.AddRange(vacations.Where(vacation => worker.WorkerId == vacation.WorkerId));
            }
            return new JsonResult(result);
        }

        [HttpGet("getByWorker")]
        public async Task<IActionResult> GetByWorker([FromQuery(Name = "idWorker")] Guid idWorker)
        {
            var worker = await _workerRepository.Get(idWorker);
            if (worker == null)
            {
                return NotFound("Worker");
            }
            return new JsonResult(await _vacationRepository.GetAllVacationsByWorker(idWorker));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return new JsonResult(await _vacationRepository.GetAllVacations());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var vacation = await _vacationRepository.Get(id);
            if (vacation == null)
            {
                return NotFound("Vacation");
            }

            var worker = await _workerRepository.Get(vacation.WorkerId);
            worker.Vacations.Remove(vacation.VacationId);
            await _workerRepository.UpdateWorker(worker);
            return new JsonResult(await _vacationRepository.Delete(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateVacationDTO updateVacation)
        {
            var vacation = await _vacationRepository.Get(id);
            if (vacation == null)
            {
                return BadRequest();
            }
            //TODO: Проверка ограничений
            vacation.StartDate = updateVacation.StartDate;
            vacation.EndDate = updateVacation.EndDate;
            await _vacationRepository.UpdateVacation(vacation);
            return Ok();
        }


        private bool CheckCross(List<WorkerEntity> workers, List<VacationEntity> vacations, int limit, CreateVacationDTO createdVacation)
        {
            var cross = 0;
            foreach (var workerEntity in workers)
            {
                foreach (var vacationEntity in vacations)
                {
                    if (vacationEntity.WorkerId == workerEntity.WorkerId)
                    {
                        if (vacationEntity.StartDate <= createdVacation.EndDate && vacationEntity.EndDate >= createdVacation.EndDate)
                        {
                            cross++;
                            break;
                        }
                    }
                }
            }
            
            return limit > cross;
        }
    }
}
