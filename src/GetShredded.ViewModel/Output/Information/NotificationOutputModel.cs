namespace GetShredded.ViewModel.Output.Information
{
    public class NotificationOutputModel
    {
        public int Id { get; set; }

        public bool Seen { get; set; }

        public int UpdatedDiaryId { get; set; }

        public string Message { get; set; }

        public string Username { get; set; }
    }
}
