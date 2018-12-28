using System.Collections.Generic;

namespace GetShredded.Models
{
    public class DiaryType
    {
        public DiaryType()
        {
            this.Diaries = new HashSet<GetShreddedDiary>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<GetShreddedDiary> Diaries { get; set; }
    }
}
