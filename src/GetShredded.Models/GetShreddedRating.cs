namespace GetShredded.Models
{
    public class GetShreddedRating
    {
        public int DiaryRatingId { get; set; }
        public DiaryRating DiaryRating { get; set; }

        public int GetShreddedDiaryId { get; set; }
        public GetShreddedDiary GetShreddedDiary { get; set; }
    }
}
