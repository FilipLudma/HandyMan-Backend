using System.Collections.Generic;
using HandyManAPI.Models;

namespace HandyManAPI.Interfaces.Repositories
{
    public interface IUserRepository
    {
        UserRecord Authenticate(string username, string password);
        IEnumerable<UserRecord> GetAll();
        UserRecord GetById(int id);
        UserRecord Create(UserRecord user, string password);
        void Update(UserRecord user, string password = null);
        void Delete(int id);
    }
}