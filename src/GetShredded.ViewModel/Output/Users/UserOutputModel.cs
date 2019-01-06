using System.Collections.Generic;
using GetShredded.ViewModel.Output.Information;
using GetShredded.ViewModels.Output.Diary;

namespace GetShredded.ViewModels.Output.Users
{
    public class UserOutputModel
    {
        public UserOutputModel()
        {
            this.UserDiaries = new List<DiaryOutputModel>();
            this.Notifications = new List<NotificationOutputModel>();
            this.Messages = new List<MessageOutputModel>();
            this.FollowedDiaries = new List<DiaryOutputModel>();
        }

        public ICollection<DiaryOutputModel> FollowedDiaries { get; set; }

        public ICollection<MessageOutputModel> Messages { get; set; }

        public ICollection<DiaryOutputModel> UserDiaries { get; set; }

        public ICollection<NotificationOutputModel> Notifications { get; set; }

        public string Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Role { get; set; }

        public int Diaries { get; set; }

        public int Comments { get; set; }

        public int MessagesCount { get; set; }

        public string Email { get; set; }
    }
}
