using System.Collections.Generic;
using GetShredded.ViewModels.Output.Diary;

namespace GetShredded.ViewModels.Output.Home
{
    public class IndexLoggedInModel
    {
        public ICollection<DiaryIndexOutputModel> Diaries { get; set; }
    }
}
