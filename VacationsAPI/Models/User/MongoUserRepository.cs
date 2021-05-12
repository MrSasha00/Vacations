using System.Threading.Tasks;
using MongoDB.Driver;
using VacationsAPI.Models.Worker;

namespace VacationsAPI.Models.User
{
    public class MongoUserRepository : IUserRepository
    {
        private readonly IMongoCollection<UserEntity> userCollection;

        public MongoUserRepository(IUserDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            userCollection = database.GetCollection<UserEntity>(settings.UsersCollectionName);
        }
        
        
        
        public async Task<UserEntity> Insert(UserEntity user)
        {
            await userCollection.InsertOneAsync(user);
            return user;
        }

        public async Task<UserEntity> GetByLogin(string login)
        {
            return await userCollection.Find(x => x.Login == login).SingleOrDefaultAsync();
        }

        public async Task<UserEntity> Delete(string login)
        {
            return await userCollection.FindOneAndDeleteAsync(x => x.Login == login);
        }
    }
}