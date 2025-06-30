namespace SilverLeaf.Screener.Admin.Objects.Requests
{
    public class LogInRequest
    {
        public LogInRequest(string username, string password)
        {
            UserName = username;
            Password = password;
        }

        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
