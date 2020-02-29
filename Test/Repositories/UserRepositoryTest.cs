using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Blogs.Repository;
using Blogs.Repository.Classes;
using Blogs.Repository.Entities;
using FizzWare.NBuilder;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Test.Repositories
{
    [TestFixture]
    public class UserRepositoryTest
    {
        private DbContextOptions<BlogsDbContext> _options;
        private IList<User> _users;



        [SetUp]
        public void SetUpTests()
        {
            var rnd = new Random();
            _options = new DbContextOptionsBuilder<BlogsDbContext>()
                .UseInMemoryDatabase(databaseName: "openreferralsTests")
                .Options;
            _users = Builder<User>.CreateListOfSize(100)
                .All()
                .With(p => p.LastName = Faker.Name.Last())
                .With(p => p.Username = Faker.Internet.UserName())
                .With(p => p.Password = "password1")
                .Build();

            //foreach (var user in _users)
            //{
            //    user.Blogs = CreateBlogList(user);
            //}

            // Insert seed data into the database using one instance of the context
            using (var context = new BlogsDbContext(_options))
            {
                context.Database.EnsureDeleted();

                foreach (var user in _users)
                {
                    try
                    {
                        context.Users.Add(user);

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
        public void UserRepository_Get()
        {
            using (var context = new BlogsDbContext(_options))
            {
                var userRepository = new UserRepository(context);
                var providers = userRepository.Get().Result;

                Assert.True(providers.ToList().Count == 100);
            }

        }

        [Test]
        public void UserRepository_GetbyId()
        {
            using (var context = new BlogsDbContext(_options))
            {
                var id = 1;
                var userRepository = new UserRepository(context);
                var providers = userRepository.GetById(1).Result;
                Assert.IsInstanceOf(typeof(User), providers);
            }

        }

        [Test]
        public async System.Threading.Tasks.Task UserRepository_CreateAsync()
        {
            using (var context = new BlogsDbContext(_options))
            {
                var id = 1;
                var userRepository = new UserRepository(context);
                var newUser = CreateUser();
                newUser.Id = -1;
                await userRepository.Create(newUser);
                var newBlog = userRepository.GetById(-1).Result;
                Assert.IsNotNull(newBlog);
            }
        }

        [Test]
        public void UserRepository_Update()
        {
            using (var context = new BlogsDbContext(_options))
            {
                var id = 1;
                var userRepository = new UserRepository(context);
                var user = userRepository.GetById(1).Result;
                user.FirsName = "Modified";
                user.Active = false;
                var result = userRepository.Update(user).Result;
                var blogMod = userRepository.GetById(1).Result;
                Assert.True(blogMod.Active == false && blogMod.FirsName == "Modified");
            }
        }


        [Test]
        public async System.Threading.Tasks.Task BlogRepository_DeleteAsync()
        {
            using (var context = new BlogsDbContext(_options))
            {

                var userRepository = new UserRepository(context);
                var user = CreateUser();
                user.Id = -1;
                await userRepository.Create(user);
                await userRepository.Delete(-1);
                var newUser = userRepository.GetById(-1).Result;
                Assert.IsNull(newUser);
            }
        }





        private User CreateUser()
        {
            var user = Builder<User>.CreateNew()
                .With(p => p.LastName = Faker.Name.Last())
                .With(p => p.Username = Faker.Internet.UserName())
                .With(p => p.Password = "password1")
                .Build();
            return user;
        }

        private ICollection<Blog> CreateBlogList(User user)
        {
            var blogs = Builder<Blog>.CreateListOfSize(3)
                    .All()
                    .With(p => p.Title = Faker.Lorem.Sentence())
                    .With(p => p.Content = Faker.Lorem.Paragraph())
                    .With(p => p.Active = true)
                    .With(p => p.User = user)
                    .With(p => p.UserId = user.Id)
                    .Build();
            ;
            return blogs;
        }

    }
}