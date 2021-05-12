using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace VacationsAPI.Models.Vacation
{
    public class VacationEntity
    {
        [BsonId]
        public Guid VacationId { get; set; }
        [BsonElement]
        public Guid WorkerId { get; set; }
        [BsonElement]
        public DateTime StartDate { get; set; }
        [BsonElement]
        public DateTime EndDate { get; set; }
        [BsonConstructor]
        public VacationEntity(Guid workerId, DateTime startDate, DateTime endDate)
        {
            WorkerId = workerId;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
