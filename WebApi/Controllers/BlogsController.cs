using System.Linq;
using System.Threading.Tasks;
using Blogs.Business.Classes;
using Blogs.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BlogsController : ControllerBase
    {
        private IBlogSupervisor _blogSupervisor;
        
        

        public BlogsController(IBlogSupervisor blogSupervisor)
        {
           
            _blogSupervisor = blogSupervisor;
        }

        // GET: api/Blogs
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await  _blogSupervisor.GetAll();
            return new ObjectResult(result);
        }

        // GET: api/Blogs/5
        [HttpGet("{id}", Name = "GetBlog")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _blogSupervisor.GetAll();
            return new ObjectResult(result.Where(q=>q.Id ==id));
        }

        // POST: api/Blogs
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateBlogViewModel value)
        {
            
            await _blogSupervisor.CreateBlog(value);
            return new ObjectResult("Ok");
        }

        // PUT: api/Blogs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateBlogViewModel value)
        {
            await _blogSupervisor.Update(id,value);
            return new ObjectResult("Ok");
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _blogSupervisor.DisableBlog(id);
            return new ObjectResult("Ok");
        }
    }
}
