using System.Collections.Generic;
using GetShredded.ViewModels.Output.Comment;

namespace GetShredded.ViewModel.Output.Information
{
    public class InformationViewModel
    {
        public InformationViewModel()
        {
            this.NewMessages = new HashSet<MessageOutputModel>();
            this.OldMessages = new HashSet<MessageOutputModel>();
            this.Notifications = new HashSet<NotificationOutputModel>();
            this.UserComments = new HashSet<CommentOutputModel>();
        }

        public ICollection<MessageOutputModel> NewMessages { get; set; }

        public ICollection<MessageOutputModel> OldMessages { get; set; }

        public ICollection<NotificationOutputModel> Notifications { get; set; }

        public ICollection<CommentOutputModel> UserComments { get; set; }
    }
}
