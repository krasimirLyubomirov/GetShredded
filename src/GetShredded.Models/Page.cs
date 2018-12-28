using System;

namespace GetShredded.Models
{
    public class Page
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public double Length => this.Content.Length;

        public string UserId { get; set; }
        public GetShreddedUser GetShreddedUser { get; set; }

        public int GetShreddedDiaryId { get; set; }
        public GetShreddedDiary GetShreddedDiary { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
