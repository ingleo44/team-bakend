namespace Blogs.Business.Classes
{
    public class UpdateUserViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirsName { get; set; }
        public string LastName { get; set; }
        public bool Active { get; set; }
    }
}