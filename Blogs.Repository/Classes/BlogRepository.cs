using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blogs.Repository.Entities;
using Blogs.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blogs.Repository.Classes
{
    public class BlogRepository : GenericRepository<Blog>, IBlogRepository
    {

        public BlogRepository(BlogsDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<ICollection<Blog>> Get()
        {
            return await Query().ToListAsync();
        }

        public async Task<Blog> GetById(int id)
        {
            return await Query().Where(q => q.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Blog> Create(Blog entity)
        {
            await InsertAsync(entity);
            return await GetById(entity.Id);
        }

        public async Task<Blog> Update(Blog entity)
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