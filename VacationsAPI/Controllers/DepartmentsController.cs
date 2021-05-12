using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using VacationsAPI.Models;
using VacationsAPI.Models.Department;
using VacationsAPI.Models.Worker;

namespace VacationsAPI.Controllers
{
    public class CreateDepartmentDTO
    {
        [Required]
        public string Name { get; set; }
    }
    public class UpdateDepartmentDTO
    {
        public string Name { get; set; }
        public Status PlanningStatus { get; set; }
        public List<Guid> Workers { get; set; }
    }
    public class IdDTO
    {
        public Guid Id { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly MongoDepartmentRepository _departmentRepository;
        private readonly MongoWorkerRepository _workerRepository;
        public DepartmentsController(MongoDepartmentRepository departmentRepository, MongoWorkerRepository workerRepository)
        {
            _departmentRepository = departmentRepository;
            _workerRepository = workerRepository;
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentDTO createDepartmentDTO)
        {
            if (createDepartmentDTO == null)
            {
                return BadRequest();
            }
            var departament = new DepartmentEntity(createDepartmentDTO.Name);
            await _departmentRepository.Insert(departament);
            return Created("/api/Departments/" + departament.DepartmentId, departament);
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var departmentList = await _departmentRepository.GetAllDepartment();
            return new JsonResult(departmentList);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var department = await _departmentRepository.Get(id);
            if (department == null)
            {
                return NotFound();
            }
            return new JsonResult(department);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletedDepartment = await _departmentRepository.DeleteDepartment(id);
            if (deletedDepartment == null)
            {
                return NotFound();
            }
            var workers = await _workerRepository.GetAllWorkersByDepartment(id);
            foreach (var worker in workers)
            {
                worker.DepartmentId = Guid.Empty;
                await _workerRepository.UpdateWorker(worker);
            }
            return Ok();
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id,[FromBody] UpdateDepartmentDTO updatedUpdateDepartmentDto)
        {
            if (updatedUpdateDepartmentDto == null)
            {
                return BadRequest();
            }

            var department = new DepartmentEntity(id, updatedUpdateDepartmentDto.Name, updatedUpdateDepartmentDto.PlanningStatus, updatedUpdateDepartmentDto.Workers);
            await _departmentRepository.UpdateDepartment(department);
            return Ok();
        }
        
        [HttpPatch("{id}/addWorker")]
        public async Task<IActionResult> AddWorker(Guid id, [FromBody] IdDTO idWorker)
        {
            if (idWorker == null)
            {
                return BadRequest();
            }
            var department = await _departmentRepository.Get(id);
            if(department == null)
            {
                return NotFound("department");
            }
            var worker = await _workerRepository.Get(idWorker.Id);
            if (worker == null)
            {
                return NotFound("worker");
            }
            worker.DepartmentId = id;
            await _workerRepository.UpdateWorker(worker);
            department.Workers.Add(worker.WorkerId);
            await _departmentRepository.UpdateDepartment(department);
            return Ok();
        }
        
        [HttpPatch("{id}/removeWorker")]
        public async Task<IActionResult> RemoveWorker( Guid id, [FromBody] IdDTO idWorker)
        {
            if (idWorker == null)
            {
                return BadRequest();
            }
            var department = await _departmentRepository.Get(id);
            if (department == null)
            {
                return NotFound("department");
            }
            var worker = await _workerRepository.Get(idWorker.Id);
            if (worker == null)
            {
                return NotFound("worker");
            }
            worker.DepartmentId = Guid.Empty;
            await _workerRepository.UpdateWorker(worker);
            department.Workers.Remove(worker.WorkerId);
            await _departmentRepository.UpdateDepartment(department);
            return Ok();
        }

        [HttpPatch("{id}/startPlanning")]
        public async Task<IActionResult> SetStatusPlanning(Guid id)
        {
            return await ChangeStatus(id, Status.Planning);
        }
        
        [HttpPatch("{id}/stopPlanning")]
        public async Task<IActionResult> SetStatusEnded(Guid id)
        {
            return await ChangeStatus(id, Status.Ended);
        }
        private async Task<IActionResult> ChangeStatus(Guid id, Status status)
        {
            var department = await _departmentRepository.Get(id);
            if (department == null)
            {
                return NotFound();
            }
            department.PlanningStatus = status;
            await _departmentRepository.UpdateDepartment(department);
            return Ok();
        }
    }
}
