using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blogs.Repository.Entities;
using Blogs.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blogs.Repository.Classes
{
    public class UserRepository : GenericRepository<User> , IUserRepository
    {
        public UserRepository(BlogsDbContext dbContext) : base(dbContext)
        {
        }
     

        public async Task<ICollection<User>> Get()
        {
            return await Query().ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await Query().Where(q=>q.Id==id).FirstOrDefaultAsync();
        }

        public async Task<User> Create(User entity)
        {
            await InsertAsync(entity);
            return await GetById(entity.Id);
        }

        public async Task<User> Update(User entity)
        {
            await UpdateAsync(entity);
            return await GetById(entity.Id);
        }

        public async Task Delete(int id)
        {
            var entity = await GetById(id);
            await DeleteAsync(entity);
        }
    }
}