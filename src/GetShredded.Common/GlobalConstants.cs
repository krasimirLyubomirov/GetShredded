namespace GetShredded.Common
{
    public class GlobalConstants
    {
        public const string Admin = "admin";

        public const string Moderator = "moderator";

        public const int UserFirstNameMinLength = 3;

        public const int UserFirstNameMaxLength = 15;

        public const int UserLastNameMinLength = 3;

        public const int UserLastNameMaxLength = 15;

        public const string ValidationForName = "[A-Za-z]+";

        public const string ErrorMessageInRegisterModel = 
            "Your Username/FirstName/LastName should contain only alphabet symbols";

        public const string ConfirmPassword = "Confirm Password";

        public const string DefaultRole = "user";

        public const string ModelError = "LoginError";

        public const string UsernameUnique = "Choose new username, this {0} is already in use!";

        public const string LoginError = "Username or password don't match!";

        public const string UserProfileRoute = "/Users/Profile/{username}";

        public const string NoSummary = "No summary included";

        public const int Zero = 0;
    }
}
