using System;

namespace Blogs.Repository.Entities
{
    public class Blog
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Active { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime DtLastUpdate { get; set; }
    }
}