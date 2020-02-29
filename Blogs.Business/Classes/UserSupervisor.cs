using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blogs.Business.Interfaces;
using Blogs.Repository.Entities;
using Blogs.Repository.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Blogs.Business.Classes
{
    public class UserSupervisor : IUserSupervisor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        public UserSupervisor(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public async Task<ICollection<User>> GetAll()
        {
            return await _userRepository.Get();
        }

        public async Task CreateUser(CreateUserViewModel newUser)
        {
            var entity = new User
            {
                FirsName = newUser.FirsName,
                LastName = newUser.LastName,
                Active = true,
                Password = newUser.Password,
                Username = newUser.Username

            };
            await _userRepository.Create(entity);
        }

        public async Task Update(int id, Interfaces.UpdateUserViewModel newUser)
        {
            var entity = await _userRepository.GetById(newUser.Id);
            if (entity==null)
            {
                throw new Exception("User Not Found");
            }
            entity.FirsName = newUser.FirsName;
            entity.LastName = newUser.LastName;
            entity.Active = newUser.Active;
            entity.Password = newUser.Password;
            await _userRepository.Update(entity);
        }

        public async Task DisableUser(int userId)
        {
            var entity = await _userRepository.GetById(userId);
            if (entity == null)
            {
                throw new Exception("User Not Found");
            }

            entity.Active = false;
            await _userRepository.Update(entity);
        }
    }
}