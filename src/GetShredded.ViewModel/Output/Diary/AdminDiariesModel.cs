using System.Collections.Generic;
using GetShredded.ViewModels.Output.Diary;

namespace GetShredded.ViewModel.Output.Diary
{
    public class AdminDiariesModel
    {
        public AdminDiariesModel()
        {
            this.Diaries = new List<DiaryOutputModel>();
            this.DiaryType = new List<DiaryTypeOutputModel>();
        }

        public ICollection<DiaryOutputModel> Diaries { get; set; }

        public ICollection<DiaryTypeOutputModel> DiaryType { get; set; }
    }
}
