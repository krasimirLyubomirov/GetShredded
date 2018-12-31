using System;
using System.Collections.Generic;
using System.Linq;
using GetShredded.ViewModel.Output.Comment;
using GetShredded.ViewModel.Output.Page;
using GetShredded.ViewModel.Output.Users;

namespace GetShredded.ViewModel.Output.Diary
{
    public class DiaryOutputModel
    {
        public DiaryOutputModel()
        {
            this.Ratings = new List<double>();
            this.Comments = new List<CommentOutputModel>();
            this.Followers = new List<UserOutputModel>();
            this.Pages = new List<PageOutputModel>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string Summary { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime LastEditedOn { get; set; }

        public double Rating => this.Ratings.Any() ? this.Ratings.Average() : 0;

        public ICollection<PageOutputModel> Pages { get; set; }

        public ICollection<UserOutputModel> Followers { get; set; }

        public ICollection<CommentOutputModel> Comments { get; set; }

        public ICollection<double> Ratings { get; set; }

        public DiaryTypeOutputModel Type { get; set; }

        public UserOutputDiaryModel User { get; set; }
    }
}
