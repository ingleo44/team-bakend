using System.Threading.Tasks;

namespace Blogs.Business.Interfaces
{
    public interface IUser
    {
        public Task CreateUser(CreateUserViewModel newUser);
        public Task UpdateUser(UpdateUserViewModel user);
    }

    public class UpdateUserViewModel
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string FirsName { get; set; }
        public string LastName { get; set; }
        public bool Active { get; set; }
    }

    public class CreateUserViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirsName { get; set; }
        public string LastName { get; set; }

    }
}