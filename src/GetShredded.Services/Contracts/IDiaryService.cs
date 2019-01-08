using System.Collections.Generic;
using System.Threading.Tasks;
using GetShredded.ViewModel.Input;
using GetShredded.ViewModels.Input.Diary;
using GetShredded.ViewModels.Output.Diary;

namespace GetShredded.Services.Contracts
{
    public interface IDiaryService
    {
        ICollection<DiaryOutputModel> CurrentDiaries(string type);

        ICollection<DiaryOutputModel> Motivation(string username);

        Task DeleteDiary(int id, string username);

        ICollection<DiaryTypeOutputModel> Types();

        Task Follow(string username, string userId, int id);

        Task UnFollow(string username, int id);

        bool IsFollowed(string username, int id);

        Task<int> CreateDiary(DiaryInputModel inputModel);

        DiaryDetailsOutputModel GetDiaryById(int id);

        void AddRating(int diaryId, double rating, string username);

        bool AlreadyRated(int diaryId, string username);

        ICollection<DiaryOutputModel> FollowedDiaries(string username);

        ICollection<DiaryOutputModel> FollowedDiariesByType(string username, string type);

        void DeleteAllDiaries(string username);
    }
}
