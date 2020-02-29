using System.Collections.Generic;
using System.Threading.Tasks;
using Blogs.Business.Classes;
using Blogs.Repository.Entities;

namespace Blogs.Business.Interfaces
{
    public interface IUserSupervisor
    {
        public Task<ICollection<User>> GetAll();
        public Task CreateUser(CreateUserViewModel newBlog);
        public Task Update(int id, UpdateUserViewModel blog);
        public Task DisableUser(int userId);
    }
}