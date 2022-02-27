using Mas.Application.UserServices.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Application.UserServices
{
    public interface IUserService
    {
        Task<UserItem> Authenticate(LoginModel loginModel);
    }
}
