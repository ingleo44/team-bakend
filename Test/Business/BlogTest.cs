using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blogs.Business.Classes;
using Blogs.Repository.Entities;
using Blogs.Repository.Interfaces;
using FizzWare.NBuilder;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace Test.Business
{
    public class BlogTest
    {
        private Mock<IBlogRepository> _10Records_blogRepository;
        private Mock<IUserRepository> _1User_userRepository;
        private Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private ICollection<Blog> _blogs;

        [SetUp]
        public void SetUpTests()
        {

            var rnd = new Random();

            var testUser = new User()
            {
                Id = 1,
                Active = true,
                Username = "user1",
                FirsName = "user1",
                LastName = "user1",
                Password = "password1",
                CreationDate = DateTime.Now,
                DtLastUpdate = DateTime.Now

            };

            ICollection<User> users = new List<User>();
            users.Add(testUser);


            _blogs = Builder<Blog>.CreateListOfSize(10)
                .All()
                .With(p => p.Title = Faker.Lorem.Sentence())
                .With(p => p.Content = Faker.Lorem.Paragraph())
                .With(p => p.Active = true)
                .With(p => p.UserId = 1)
                .Build();
            _10Records_blogRepository = new Mock<IBlogRepository>();
            _10Records_blogRepository.Setup(x => x.Get()).Returns(Task.FromResult(_blogs));
            _10Records_blogRepository.Setup(x => x.Create(It.IsAny<Blog>())).Returns(Task.FromResult(new Blog() { Id = -1 }));
            _10Records_blogRepository.Setup(x => x.Update(It.IsAny<Blog>())).Returns(Task.FromResult(new Blog() { Id = -1 }));
            _10Records_blogRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(Task.FromResult(_blogs.FirstOrDefault()));


            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockHttpContextAccessor.Setup(_ => _.HttpContext.User.Identity.Name).Returns("user1");

            _1User_userRepository = new Mock<IUserRepository>();
            _1User_userRepository.Setup(x => x.Get()).Returns(Task.FromResult(users));

        }
        [Test]
        public async Task GetBlogs_100RecordsOnTable()
        {
            // Arrange            
            var blogService = new BlogSupervisor(_mockHttpContextAccessor.Object,_10Records_blogRepository.Object, _1User_userRepository.Object);
            // Assert
            var result = await  blogService.GetAll();
            Assert.True(result.Count ==10);
        }
        [Test]
        public async Task CreateBlogs_withUser()
        {
            var newRecord = new CreateBlogViewModel()
            {
                Title = "a title",
                Content = "a content",
                Active = true
            };

            // Arrange            
            var blogService = new BlogSupervisor(_mockHttpContextAccessor.Object, _10Records_blogRepository.Object, _1User_userRepository.Object);
            // Assert
            Assert.DoesNotThrowAsync(async ()=> await blogService.CreateBlog(newRecord));
        }

        [Test]
        public async Task CreateBlogs_whithoutUser_shouldthrowexception()
        {
            var newRecord = new CreateBlogViewModel()
            {
                Title = "a title",
                Content = "a content",
                Active = true
            };

            _mockHttpContextAccessor=new Mock<IHttpContextAccessor>();
            // Arrange            
            var blogService = new BlogSupervisor(_mockHttpContextAccessor.Object, _10Records_blogRepository.Object, _1User_userRepository.Object);
            // Assert

            Assert.ThrowsAsync<Exception>(async () => await blogService.CreateBlog(newRecord));
        }


        [Test]
        public async Task UpdateBlogs_withUser()
        {
            var newRecord = new UpdateBlogViewModel()
            {
                Title = "a title",
                Content = "a content",
                Active = true
                
            };
            // Arrange            
            var blogService = new BlogSupervisor(_mockHttpContextAccessor.Object, _10Records_blogRepository.Object, _1User_userRepository.Object);
            // Assert

            Assert.DoesNotThrowAsync(async () => await blogService.Update(1,newRecord));
        }


        [Test]
        public async Task UpdateBlogs_withoutUser()
        {
            var newRecord = new UpdateBlogViewModel()
            {
                Title = "a title",
                Content = "a content",
                Active = true

            };
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            // Arrange            
            var blogService = new BlogSupervisor(_mockHttpContextAccessor.Object, _10Records_blogRepository.Object, _1User_userRepository.Object);
            // Assert

            Assert.ThrowsAsync<Exception>(async () => await blogService.Update(-1, newRecord));
        }
    }
}