using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blogs.Repository.Entities;

namespace Blogs.Repository.Interfaces
{
    public interface IUserRepository
    {
        public Task<ICollection<User>> Get();
        public Task<User> GetById(int id);
        public Task<User> Create(User user);
        public Task<User> Update(User user);
        public Task Delete(int userId);
    }
}