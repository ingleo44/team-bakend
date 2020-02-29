using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Blogs.Business.Interfaces;
using Blogs.Repository.Interfaces;
using Common.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

namespace Blogs.Business.Classes
{
    public class Authentication: IAuthentication
    {

        private IUserRepository _userRepository;
        private IConfiguration _configuration;

        public Authentication(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }


        // this is the authentication method I set the parameters via get method which is not the most secure but for demonstration purposes is fine
        public async Task<JwtToken> Login(string username, string password)
        {
            var userList = await _userRepository.Get();
            var currentUser = userList.FirstOrDefault(q => q.Username == username && q.Password==password && q.Active);
            if (currentUser == null)
            {
                throw new Exception("Invalid Credentials");
            }

            return await SetAuthToken(username);
        }



        public async Task<JwtToken> SetAuthToken(string username)
        {
            //var user = _users.SingleOrDefault(x => x.Username == username && x.Password == password);

            var secretKey =  _configuration.GetValue<string>("Jwt:secretKey");
            var userList = await _userRepository.Get();

            var currentUser = userList.FirstOrDefault(q => q.Username == username && q.Active);
            if (currentUser == null)
            {
                throw new Exception("The username for creating the authentication token does not exists");
            }

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim("user_id", $"'{currentUser.Id}'")
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var result = new JwtToken { Token = tokenHandler.WriteToken(token) };


            return result;
        }


   

        public Task Logout()
        {
            throw new System.NotImplementedException();
        }
    }
}