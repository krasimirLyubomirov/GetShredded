using System.Collections.Generic;

namespace GetShredded.Models
{
    public class DiaryRating
    {
        public DiaryRating()
        {
            this.GetShreddedRatings = new HashSet<GetShreddedRating>();
        }

        public int Id { get; set; }

        public double Rating { get; set; }

        public string GetShreddedUserId { get; set; }
        public GetShreddedUser GetShreddedUser { get; set; }

        public ICollection<GetShreddedRating> GetShreddedRatings { get; set; }
    }
}
