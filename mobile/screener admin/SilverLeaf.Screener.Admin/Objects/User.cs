using System;

namespace SilverLeaf.Screener.Admin.Objects
{
    public class User
    {
        public static event EventHandler UserChanged = delegate { };

        public string UserName { get; set; } = string.Empty;

        public string Token { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public void RefreshProfileInfo(User profile)
        {
            UserName = profile.UserName;
        }
    }
}
