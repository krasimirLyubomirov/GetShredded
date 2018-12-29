using System.Collections.Generic;
using GetShredded.ViewModel.Output.Diary;

namespace GetShredded.ViewModel.Output.Home
{
    public class IndexLoggedInModel
    {
        public ICollection<DiaryIndexOutputModel> Diaries { get; set; }
    }
}
