using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blogs.Business.Classes;
using Blogs.Repository.Entities;

namespace Blogs.Business.Interfaces
{
    public interface IBlogSupervisor
    {
        public Task<ICollection<Blog>> GetAll();
        public Task CreateBlog(CreateBlogViewModel newBlog);
        public Task Update(int id, UpdateBlogViewModel blog);
        public Task DisableBlog(int blogId);
    }
}