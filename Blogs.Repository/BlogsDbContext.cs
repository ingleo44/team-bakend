using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blogs.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blogs.Repository
{
    public class BlogsDbContext : DbContext
    {
        public BlogsDbContext(DbContextOptions<BlogsDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>().HasOne(q => q.User).WithMany(q => q.Blogs).HasForeignKey(q => q.UserId);
            modelBuilder.Entity<User>().HasData(new User
            {
                Id=-1,
                Active = true, CreationDate = DateTime.Now, DtLastUpdate = DateTime.Now, FirsName = "prueba1",
                LastName = "prueba1", Password = "password1", Username = "user1"
            });
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var addedEntities = ChangeTracker.Entries().Where(E => E.State == EntityState.Added).ToList();

            addedEntities.ForEach(E =>
            {
                E.Property("CreationDate").CurrentValue = DateTime.Now;
                E.Property("DtLastUpdate").CurrentValue = DateTime.Now;
            });

            var editedEntities = ChangeTracker.Entries().Where(E => E.State == EntityState.Modified).ToList();

            editedEntities.ForEach(E =>
            {
                E.Property("DtLastUpdate").CurrentValue = DateTime.Now;
            });


            return base.SaveChangesAsync(cancellationToken);
        }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<User> Users { get; set; }


        private void AddModifiedDate()
        {

        }


    }
}