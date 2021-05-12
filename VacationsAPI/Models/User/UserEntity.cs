using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VacationsAPI.Models.User
{
    public class UserEntity
    {
        [BsonId]
        public string Login { get; set; }
        [BsonElement]
        public string Password { get; set; }
        [BsonElement]
        public Guid IdWorker { get; set; }

        [BsonConstructor]
        public UserEntity(string login, string password)
        {
            Login = login;
            Password = password;
            IdWorker = Guid.NewGuid();
        }
    }
}