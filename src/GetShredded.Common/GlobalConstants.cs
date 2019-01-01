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

        public const string ErrorOnDeleteUser = "User was not deleted. Something went wrong!";

        public const string Success = "Success";

        public const string Failed = "Failed the task";

        public const string NotUser = "For this action need to be User of the GetShredded Diary";

        public const string UserHasNoRights = "User need role admin";

        public const string NotValidPageDiaryConnection 
            = "This Diary with id: {0} or Page with id: {1} don't have a connection";

        public const string NotificationMessage = "{0} Diary you are following just added new Page";

        public const string All = "All";

        public const string AlreadyFollow = "{0} already is following this diary";

        public const string UserDontFollow = "User has no following on this diary";

        public const string DefaultNoImage 
            = "https://res.cloudinary.com/dbwuk5rsq/image/upload/v1544052092/noimagedefault.jpg";

        public const string MissingDiary = "No GetShredded Diary with such Id";

        public const string AlreadyRated = "User already rated this diary";

        public const string UserRights = "User don't have rights to make this action";

        public const string WithoutRecord = "No such record!";

        public const string AccCloudinarySecret = "Sb7IEefRr6LqOghHeB9OzKhbouU";

        public const string AccCloudinaryApiKey = "923729842421433";

        public const string CloudinaryCloudName = "dvo7kkcme";
    }
}
