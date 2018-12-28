using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace GetShredded.Models
{
    public class GetShreddedUser : IdentityUser
    {
        public GetShreddedUser()
        {
            this.Notifications = new HashSet<Notification>();
            this.Comments = new HashSet<Comment>();
            this.SendMessages = new HashSet<Message>();
            this.FollowedDiaries = new HashSet<GetShreddedUserDiary>();
            this.GetShreddedDiaries = new HashSet<GetShreddedDiary>();
            this.Pages = new HashSet<Page>();
            this.DiaryRatings = new HashSet<DiaryRating>();
            this.ReceivedMessages = new HashSet<Message>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<DiaryRating> DiaryRatings { get; set; }

        public ICollection<GetShreddedDiary> GetShreddedDiaries { get; set; }

        public ICollection<Page> Pages { get; set; }

        public ICollection<Message> ReceivedMessages { get; set; }

        public ICollection<Message> SendMessages { get; set; }

        public ICollection<GetShreddedUserDiary> FollowedDiaries { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<Notification> Notifications { get; set; }
    }
}
