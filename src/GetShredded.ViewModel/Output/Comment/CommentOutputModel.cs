using System;

namespace GetShredded.ViewModels.Output.Comment
{
    public class CommentOutputModel
    {
        public int Id { get; set; }

        public int DiaryId { get; set; }

        public string User { get; set; }

        public string Message { get; set; }

        public DateTime CommentedOn { get; set; }
    }
}
