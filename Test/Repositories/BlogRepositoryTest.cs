using System;
using System.Collections.Generic;
using System.Linq;
using Blogs.Repository;
using Blogs.Repository.Classes;
using Blogs.Repository.Entities;
using FizzWare.NBuilder;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Test.Repositories
{
    [TestFixture]
    public class BlogRepositoryTest
    {
        private DbContextOptions<BlogsDbContext> _options;
        private IList<Blog> _blogs;



        [SetUp]
        public void SetUpTests()
        {
            var rnd = new Random();
            _options = new DbContextOptionsBuilder<BlogsDbContext>()
                .UseInMemoryDatabase(databaseName: "openreferralsTests")
                .Options;
            _blogs = Builder<Blog>.CreateListOfSize(100)
                .All()
                .With(p => p.Title = Faker.Lorem.Sentence())
                .With(p => p.Content = Faker.Lorem.Paragraph())
                .With(p => p.Active = true)
                .With(p => p.UserId = 1)
                .Build();

            var user = Builder<User>.CreateNew().With(p => p.Active = true).With(p => p.FirsName = Faker.Name.First())
                    .With(p => p.Id = 1)
                    .With(p => p.LastName = Faker.Name.Last())
                    .With(p => p.Username = Faker.Internet.UserName())
                    .With(p => p.Password = "password1")
                    .Build()
                ;

            _blogs = Builder<Blog>.CreateListOfSize(100)
                .All()
                .With(p => p.Title = Faker.Lorem.Sentence())
                .With(p => p.Content = Faker.Lorem.Paragraph())
                .With(p => p.Active = true)
                .With(p => p.UserId = 1)
                .With(p => p.User = user)
                .Build();
            // Insert seed data into the database using one instance of the context
            using (var context = new BlogsDbContext(_options))
            {
                context.Database.EnsureDeleted();

                foreach (var blog in _blogs)
                {
                    try
                    {
                        context.Blogs.Add(blog);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
                context.SaveChanges();
            }


        }

        [Test]
        public void BlogRepository_Get()
        {
            using (var context = new BlogsDbContext(_options))
            {
                var blogRepository = new BlogRepository(context);
                var providers = blogRepository.Get().Result;

                Assert.True(providers.ToList().Count == 100);
            }

        }

        [Test]
        public void BlogRepository_GetbyId()
        {
            using (var context = new BlogsDbContext(_options))
            {
                var id = 1;
                var blogRepository = new BlogRepository(context);
                var providers = blogRepository.GetById(1).Result;
                Assert.IsInstanceOf(typeof(Blog), providers);
            }

        }

        [Test]
        public async System.Threading.Tasks.Task BlogRepository_CreateAsync()
        {
            using (var context = new BlogsDbContext(_options))
            {
                var id = 1;
                var blogRepository = new BlogRepository(context);
                var blog = CreateBlog();
                blog.Title = "new Blog";
                blog.Id = -1;
                await blogRepository.Create(blog);
                var newBlog = blogRepository.GetById(-1).Result;
                Assert.IsNotNull(newBlog);
            }
        }

        [Test]
        public void BlogRepository_Update()
        {
            using (var context = new BlogsDbContext(_options))
            {
                var id = 1;
                var blogRepository = new BlogRepository(context);
                var blog = blogRepository.GetById(1).Result;
                blog.Title = "Modified";
                blog.Active = false;
                var result =blogRepository.Update(blog).Result;
                var blogMod = blogRepository.GetById(1).Result;
                Assert.True(blogMod.Active==false && blogMod.Title== "Modified");
            }
        }


        [Test]
        public async System.Threading.Tasks.Task BlogRepository_DeleteAsync()
        {
            using (var context = new BlogsDbContext(_options))
            {
                
                var blogRepository = new BlogRepository(context);
                var blog = CreateBlog();
                blog.Title = "new Blog";
                blog.Id = -1;
                var x=blogRepository.Create(blog).Result;
                await blogRepository.Delete(-1);
                var newBlog = blogRepository.GetById(-1).Result;
                Assert.IsNull(newBlog);
            }
        }



        private Blog CreateBlog()
        {
            var Blog = Builder<Blog>.CreateNew()
                .With(p => p.Title = Faker.Lorem.Sentence())
                .With(p => p.Content = Faker.Lorem.Paragraph())
                .With(p => p.Active = true)
                .With(p => p.UserId = 1)
                .Build();
            return Blog;
        }

    }

}