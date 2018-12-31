namespace GetShredded.Common
{
    public class GlobalConstants
    {
        public const int CommentContentLength = 300;

        public const int TitleLength = 100;

        public const int GetShreddedDiarySummary = 200;

        public const int MessageContentLength = 400;

        public const int PageContentLength = 3000;

        public const int NotificationContentLength = 200;

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

        public const string PageInputContentError 
            = "Your chapter should not have less than 50 charaters or more than 1000 characters";

        public const int PageMaxLength = 1000;

        public const int PageMinLength = 50;

        public const int TitleMaxLength = 100;

        public const int TitleMinLength = 3;

        public const int CommentLength = 100;

        public const int MessageLength = 400;

        public const int DiarySummaryLength = 200;

        public const string DiaryImageDisplay = "Diary Image";

        public const string NoTitleAdded = "Page has no title";

        public const string DeletedUser = "Deleted User";
    }
}
