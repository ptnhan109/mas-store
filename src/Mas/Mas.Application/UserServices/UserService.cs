using Mas.Application.Options;
using Mas.Application.UserServices.Dtos;
using Mas.Common;
using Mas.Core;
using Mas.Core.Contants;
using Mas.Core.Entities;
using Mas.Core.Enums;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Application.UserServices
{
    public class UserService : IUserService
    {
        private readonly IAsyncRepository<User> _repository;
        private readonly AppOptions _options;

        public UserService(IAsyncRepository<User> repository, IOptions<AppOptions> options)
        {
            _repository = repository;
            _options = options.Value;
        }

        public async Task AddUser(AddUserRequest request)
        {
            string hashedPwd = request.Password.ToHashedString(_options.Secret);

            var entity = new User()
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Name = request.Name,
                PasswordHash = hashedPwd,
                Role = request.Role,
                Username = request.Username,
                Phone = request.Phone
            };

            if (request.Avatar != null)
            {
                string image = await request.Avatar.ToUpload(ContantsFolder.Avatar);
                entity.Image = image;
            }
            else
            {
                entity.Image = string.Empty;
            }


            await _repository.AddAsync(entity);
        }

        public async Task<UserItem> Authenticate(LoginModel loginModel)
        {
            string hashed = loginModel.Password.ToHashedString(_options.Secret);
            Expression<Func<User, bool>> where = c => c.Username.Equals(loginModel.UserName) && c.PasswordHash.Equals(hashed);
            var user = await _repository.FindAsync(where);
            if (user == null)
            {
                return default;
            }

            return new UserItem(user);
        }

        public async Task DeleteUser(Guid id)
        {
            var entity = await _repository.FindAsync(id);
            if(entity != null)
            {
                string filePath = entity.Image;
                await _repository.DeleteAsync(id);
                var path = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                await FileExtentions.Remove(path);
            }
        }

        public async Task<User> GetUser(Guid id)
        {
            return await _repository.FindAsync(id);
        }

        public async Task<List<UserItem>> GetUsers(EnumRole? role)
        {
            if (role != null)
            {
                Expression<Func<User, bool>> filters = c => c.Role == role.Value;
                var users = await _repository.FindAllAsync(filters, null);
                return users.Select(c => new UserItem(c)).ToList();
            }

            return (await _repository.FindAllAsync(null, null)).Select(c => new UserItem(c))
                .ToList();
        }

        public async Task UpdateUser(UpdateUserRequest request)
        {
            var entity = await _repository.FindAsync(request.Id);

            entity.Name = request.Name;
            entity.Role = request.Role;
            entity.Username = request.Username;
            entity.UpdatedAt = DateTime.Now;
            entity.Phone = request.Phone;

            if (!string.IsNullOrEmpty(request.Password))
            {
                entity.PasswordHash = request.Password.ToHashedString(_options.Secret);
            }

            if(request.Avatar != null)
            {
                string image = await request.Avatar.ToUpload(ContantsFolder.Avatar);
                entity.Image = image;
            }

            await _repository.UpdateAsync(entity);
        }
    }
}
