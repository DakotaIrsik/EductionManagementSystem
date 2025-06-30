using SilverLeaf.Common.Interfaces;
using SilverLeaf.Entities.DTOs;
using SilverLeaf.Screener.Admin.Objects;
using SilverLeaf.Screener.Admin.Objects.Requests;
using SilverLeaf.Screener.Admin.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace SilverLeaf.Screener.Admin.Services
{
    public class UserService : IUserService
    {
        private readonly IUserAPI _userApi;
        private readonly IProcessing _processing;

        private User _user;

        public UserService(IUserAPI userApi, IProcessing processing)
        {
            _userApi = userApi;
            _processing = processing;
        }

        public event EventHandler<User> UserChanged;


        public User User
        {
            get => _user ?? (_user = CacheService.User);
            set
            {
                if (_user == value)
                {
                    return;
                }

                _user = value;
                CacheUser();
            }
        }

        public async Task Authenticate(string username, string password)
        {
            var response = await _processing.Process(_userApi.Login<UserDTO>(new LogInRequest(username, password))).ConfigureAwait(false);
            CacheService.LoginUserName = response.Data.Content.UserName;
        }

        public void ClearUserCache()
        {
            _user = null;
            CacheService.RemoveUserSettings();
        }

        private void CacheUser()
        {
            CacheService.User = _user;
            UserChanged?.Invoke(this, _user);
        }
    }
}
