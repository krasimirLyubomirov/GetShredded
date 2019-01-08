namespace GetShredded.ViewModels.Output.Users
{
    public class UserAdminOutputModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Role { get; set; }

        public int Comments { get; set; }

        public int Diaries { get; set; }

        public int MessageCount { get; set; }
    }
}
