using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Blogs.Business.Interfaces;
using Blogs.Repository.Entities;
using Blogs.Repository.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Blogs.Business.Classes
{
    public class BlogSupervisor : IBlogSupervisor
    {
        private readonly IBlogRepository _blogRepository;
        private IUserRepository _userRepository;
        private string _currentUserName;


        public BlogSupervisor(IHttpContextAccessor httpContextAccessor, IBlogRepository blogRepository, IUserRepository userRepository)
        {
            _blogRepository = blogRepository;
            _userRepository = userRepository;
            _currentUserName = httpContextAccessor?.HttpContext?.User?.Identity != null ? httpContextAccessor.HttpContext.User.Identity.Name : "";

        }

        public async Task<ICollection<Blog>> GetAll()
        {
            return await _blogRepository.Get();
        }

        public async Task CreateBlog(CreateBlogViewModel newBlog)
        {

            var userId = await GetUserId();
            var entity = new Blog
            {
                Title = newBlog.Title,
                Active = true,
                Content = newBlog.Content,
                UserId = userId
            };
            await _blogRepository.Create(entity);

        }

        

        public async Task Update(int id, UpdateBlogViewModel updatedBlog)
        {
            var userId = await GetUserId();
            var currentBlog = await _blogRepository.GetById(id);

            if (currentBlog.UserId != userId)
            {
                throw new Exception("Blog can not be updated because the authenticated user is not the owner of it");
            }

            if (currentBlog == null)
            {
                throw new Exception("Blog Not Found");
            }
            currentBlog.Title = updatedBlog.Title;
            currentBlog.Active = updatedBlog.Active;
            currentBlog.Content = updatedBlog.Content;
            currentBlog.UserId = userId;
            await _blogRepository.Update(currentBlog);
        }

        public async Task DisableBlog(int blogId)
        {
            var userId = await GetUserId();
            var currentBlog = await _blogRepository.GetById(blogId);
            if (currentBlog.UserId != userId)
            {
                throw new Exception("Blog can not be updated because the current user is not the owner of it");
            }
            if (currentBlog == null)
            {
                throw new Exception("Blog Not Found");
            }
            currentBlog.Active = false;
            await _blogRepository.Update(currentBlog);
        }


        private async Task<int> GetUserId()
        {
            var userList = await _userRepository.Get();
            var user = userList.FirstOrDefault(q => q.Username == _currentUserName && q.Active);
            if (user == null)
            {
                throw new Exception("user does not exists");
            }

            return user.Id;
        }
    }
}