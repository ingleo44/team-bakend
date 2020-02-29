using System.Collections.Generic;
using System.Threading.Tasks;
using Blogs.Repository.Entities;

namespace Blogs.Repository.Interfaces
{
    public interface IBlogRepository
    {
        public Task<ICollection<Blog>> Get();
        public Task<Blog> GetById(int id);
        public Task<Blog> Create(Blog blog);
        public Task<Blog> Update(Blog blog);
        public Task Delete(int blogId);
    }
}