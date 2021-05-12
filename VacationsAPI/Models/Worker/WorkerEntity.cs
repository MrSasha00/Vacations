using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VacationsAPI.Models.Worker
{
    public class WorkerEntity
    {
        [BsonId]
        public Guid WorkerId { get; set; }
        [BsonElement]
        public string FirstName { get; set; }
        [BsonElement]
        public string MidName { get; set; }
        [BsonElement]
        public string LastName { get; set; }
        [BsonElement]
        public string Position { get; set; }
        [BsonElement]
        public Guid DepartmentId { get; set; }
        [BsonElement]
        public int DaysLeft { get; set; }
        [BsonElement]
        public bool IsResponsible { get; set; }
        [BsonElement]
        public bool IsAdmin { get; set; }
        [BsonElement]
        public List<Guid> Vacations { get; set; }
        [BsonConstructor]
        public WorkerEntity(string firstName, string midName, string lastName, string position, Guid departmentId)
        {
            WorkerId = Guid.NewGuid();
            FirstName = firstName;
            MidName = midName;
            LastName = lastName;
            Position = position;
            DaysLeft = 28;
            IsResponsible = false;
            IsAdmin = false;
            DepartmentId = departmentId;
            Vacations = new List<Guid>();
        }
    }
}
