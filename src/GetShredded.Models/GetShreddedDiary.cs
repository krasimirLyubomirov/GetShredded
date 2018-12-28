using System;
using System.Collections.Generic;
using System.Linq;

namespace GetShredded.Models
{
    public class GetShreddedDiary
    {
        public GetShreddedDiary()
        {
            this.Pages = new HashSet<Page>();
            this.Followers = new HashSet<GetShreddedUserDiary>();
            this.Comments = new HashSet<Comment>();
            this.Ratings = new HashSet<GetShreddedRating>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string Summary { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime LastEditedOn { get; set; }

        public virtual ICollection<Page> Pages { get; set; }

        public ICollection<GetShreddedUserDiary> Followers { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<GetShreddedRating> Ratings { get; set; }

        public int DiaryTypeId { get; set; }
        public DiaryType Type { get; set; }

        public string UserId { get; set; }
        public GetShreddedUser User { get; set; }

        public double Rating => this.Ratings.Any() ? this.Ratings.Average(x => x.DiaryRating.Rating) : 0;

        public double Length => this.Pages.Sum(x => x.Length);
    }
}
