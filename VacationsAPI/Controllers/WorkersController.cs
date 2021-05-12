using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using VacationsAPI.Models.Department;
using VacationsAPI.Models.Vacation;
using VacationsAPI.Models.Worker;

namespace VacationsAPI.Controllers
{
    public class CreateWorkerDTO
    {
        [Required]
        public Guid WorkerId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string MidName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Position { get; set; }
        public Guid DepartmentId { get; set; }
    }
    public class UpdateWorkerDTO
    {
        public string FirstName { get; set; }
        public string MidName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
    }
    public class UpdateFlagWorkerDTO
    {
        public bool Flag { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class WorkersController : ControllerBase
    {
        private readonly MongoWorkerRepository _workerRepository;
        private readonly MongoDepartmentRepository _departmentRepository;
        private readonly MongoVacationRepository _vacationRepository;
        public WorkersController(MongoDepartmentRepository departmentRepository, MongoWorkerRepository workerRepository, MongoVacationRepository vacationRepository)
        {
            _workerRepository = workerRepository;
            _departmentRepository = departmentRepository;
            _vacationRepository = vacationRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWorkerDTO createdWorker)
        {
            if(createdWorker == null)
            {
                return BadRequest();
            }
            var worker = new WorkerEntity(createdWorker.WorkerId, createdWorker.FirstName, createdWorker.MidName ,createdWorker.LastName, createdWorker.Position, createdWorker.DepartmentId);
            await _workerRepository.Insert(worker);
            var department = await _departmentRepository.Get(createdWorker.DepartmentId);
            if(department == null)
            {
                return BadRequest();
            }
            department.Workers.Add(worker.WorkerId);
            await _departmentRepository.UpdateDepartment(department);
            return Created("api/Workers/" + worker.WorkerId, worker);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var worker = await _workerRepository.Get(id);
            if(worker == null)
            {
                return NotFound();
            }
            return new JsonResult(worker);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllWorkers()
        {
            var workers = await _workerRepository.GetAllWorkers();
            return new JsonResult(workers);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletedWorker = await _workerRepository.DeleteWorker(id);
            if (deletedWorker == null)
            {
                return NotFound();
            }
            
            var departments = await _departmentRepository.GetAllDepartment();
            foreach (var department in departments)
            {
                department.Workers.Remove(id);
                await _departmentRepository.UpdateDepartment(department);
            }

            var vacations = await _vacationRepository.GetAllVacationsByWorker(id);
            foreach (var vacation in vacations)
            {
                await _vacationRepository.Delete(vacation.VacationId);
            }
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateWorkerDTO updateWorker)
        {
            var worker = await _workerRepository.Get(id);
            if (worker == null)
            {
                return NotFound();
            }
            worker.FirstName = updateWorker.FirstName;
            worker.MidName = updateWorker.MidName;
            worker.LastName = updateWorker.LastName;
            worker.Position = updateWorker.Position;
            await _workerRepository.UpdateWorker(worker);
            return Ok();
        }

        [HttpPatch("{id}/Responsible")]
        public async Task<IActionResult> SetResponsible(Guid id, [FromBody] UpdateFlagWorkerDTO updateFlagWorkerDto)
        {
            if (updateFlagWorkerDto == null)
            {
                return BadRequest();
            }
            var worker = await _workerRepository.Get(id);
            if (worker == null)
            {
                return NotFound("worker");
            }

            if (worker.DepartmentId == Guid.Empty)
            {
                return BadRequest("Worker do not have department");
            }
            worker.IsResponsible = updateFlagWorkerDto.Flag;
            await _workerRepository.UpdateWorker(worker);
            return Ok();
        }
       
        [HttpPatch("{id}/Admin")]
        public async Task<IActionResult> SetAdmin(Guid id, [FromBody] UpdateFlagWorkerDTO updateFlagWorkerDto)
        {
            if (updateFlagWorkerDto == null)
            {
                return BadRequest();
            }
            var worker = await _workerRepository.Get(id);
            if (worker == null)
            {
                return NotFound();
            }
            
            worker.IsAdmin = updateFlagWorkerDto.Flag;
            await _workerRepository.UpdateWorker(worker);
            return Ok();
        }

        [HttpPut("{id}/Department")]
        public async Task<IActionResult> SetDepartment(Guid id, [FromQuery(Name = "idDepartment")] Guid idDepartment)
        {
            var worker = await _workerRepository.Get(id);
            if (worker == null)
            {
                return BadRequest("Worker");
            }

            if (idDepartment == Guid.Empty)
            {
                return BadRequest("Department");
            }

            worker.DepartmentId = idDepartment;
            await _workerRepository.UpdateWorker(worker);
            return Ok();
        }

        [HttpGet("/workersByDepartment")]
        public async Task<IActionResult> GetWorkersByDepartment([FromQuery(Name = "idDepartment")] Guid idDepartment)
        {
            return new JsonResult(await _workerRepository.GetAllWorkersByDepartment(idDepartment));
        }

    }
}
