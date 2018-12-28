namespace GetShredded.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public int UpdatedDiaryId { get; set; }

        public bool Seen { get; set; }

        public string Message { get; set; }

        public string GetShreddedUserId { get; set; }
        public GetShreddedUser GetShreddedUser { get; set; }
    }
}
