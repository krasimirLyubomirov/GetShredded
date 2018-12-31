using System.Collections.Generic;
using System.Threading.Tasks;
using GetShredded.ViewModel.Input;
using GetShredded.ViewModel.Output.Diary;

namespace GetShredded.Services.Contracts
{
    public interface IDiaryService
    {
        ICollection<DiaryOutputModel> CurrentDiaries(string type);

        ICollection<DiaryOutputModel> UserDiaries(string username);

        Task DeleteDiary(int id, string username);

        ICollection<DiaryOutputModel> Types();

        Task Follow(string username, int id);

        Task UnFollow(string username, int id);

        bool IsFollowed(string username, int id);

        Task<int> CreateDiary(DiaryInputModel inputModel);

        DiaryDetailsOutputModel GetDiaryById(int id);

        void AddRating(int diaryId, double rating, string username);

        bool AlreadyRated(int diaryId, string username);
    }
}
