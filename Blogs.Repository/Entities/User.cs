using System;
using System.Collections;
using System.Collections.Generic;

namespace Blogs.Repository.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirsName { get; set; }
        public string LastName { get; set; }
        public bool Active { get; set; }
        public ICollection<Blog> Blogs { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? DtLastUpdate { get; set; }
    }
}