using Mas.Application.UserServices.Dtos;
using Mas.Core.Entities;
using Mas.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Application.UserServices
{
    public interface IUserService
    {
        Task<UserItem> Authenticate(LoginModel loginModel);

        Task AddUser(AddUserRequest request);

        Task<User> GetUser(Guid id);

        Task<List<UserItem>> GetUsers(EnumRole? role);

        Task UpdateUser(UpdateUserRequest request);

        Task DeleteUser(Guid id);
    }
}
