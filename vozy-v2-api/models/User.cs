namespace vozy_v2_api.models
{
    public class User
    {
        public User(string user, string password)
        {
            this.user = user;
            this.password = password;
        }

        public string user { get; set; }
        public string password { get; set; }
    }
}
