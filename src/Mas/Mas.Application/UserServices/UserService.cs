using Mas.Application.Options;
using Mas.Application.UserServices.Dtos;
using Mas.Common;
using Mas.Core;
using Mas.Core.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
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
    }
}
