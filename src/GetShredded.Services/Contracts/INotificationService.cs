namespace GetShredded.Services.Contracts
{
    public interface INotificationService
    {
        void AddNotification(int diaryId, string username, string diaryTitle);

        int NewNotifications(string username);

        void DeleteNotification(int id);

        void DeleteAllNotifications(string username);

        void MarkNotificationAsSeen(int id);

        bool DiaryExists(int id);
    }
}
