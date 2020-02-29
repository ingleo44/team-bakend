using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blogs.Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IAuthentication _authentication;
  
        public AuthenticationController(IAuthentication authentication)
        {
            _authentication = authentication;
        }
        [HttpGet]
        public async Task<IActionResult> Login(string username, string password)
        {
            var result = await  _authentication.Login(username, password);
            return new ObjectResult(result);
        }
    }
}