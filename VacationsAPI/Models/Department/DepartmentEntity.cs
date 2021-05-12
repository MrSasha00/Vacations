using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace VacationsAPI.Models.Department
{
    public class DepartmentEntity
    {
        [BsonId]
        public Guid DepartmentId { get; set; }
        [BsonElement]
        public string Name { get; set; }
        [BsonElement]
        public Status PlanningStatus { get; set; }
        [BsonElement]
        public List<Guid> Workers { get; set; }
        [BsonConstructor]
        public DepartmentEntity(string name)
        {
            DepartmentId = Guid.NewGuid();
            Name = name;
            PlanningStatus = Status.Ended;
            Workers = new List<Guid>();
        }
        public DepartmentEntity(Guid id, string name, Status status, List<Guid> workers)
        {
            DepartmentId = id;
            Name = name;
            PlanningStatus = status;
            Workers = workers;
        }
    }
}
