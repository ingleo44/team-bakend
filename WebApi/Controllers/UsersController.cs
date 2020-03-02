using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blogs.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {

        private IUserSupervisor _userSupervisor;

        public UsersController(IUserSupervisor userSupervisor)
        {
            _userSupervisor = userSupervisor;
        }

        // GET: api/UsersController
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _userSupervisor.GetAll();
            return new ObjectResult(result);
        }

        // GET: api/UsersController/5
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _userSupervisor.GetAll();
            return new ObjectResult(result.Where(q => q.Id == id));
        }

        [AllowAnonymous]
        // POST: api/UsersController
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserViewModel value)
        {
            await _userSupervisor.CreateUser(value);
            return new ObjectResult("Ok");
        }

        // PUT: api/UsersController/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateUserViewModel value)
        {
            await _userSupervisor.Update(id, value);
            return new ObjectResult("Ok");
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userSupervisor.DisableUser(id);
            return new ObjectResult("Ok");
        }
    }
}
