﻿using MyMusic.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusic.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User> Authenticate(string username, string password);
        Task<User> Create(User user, string password);
        void Update(User user, string password = null);
        void Delete(int id);
        Task<IEnumerable<User>> GetAllUserAsync();
        Task<User> GetWithUsersByIdAsync(int id);
    }
}
