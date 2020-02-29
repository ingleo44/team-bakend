using System.Threading.Tasks;
using Common.Security;

namespace Blogs.Business.Interfaces
{
    public interface IAuthentication
    {
        public Task<JwtToken> Login(string username, string password);
        public Task  Logout();

    }


}