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

        public UserEntity(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}