namespace GetShredded.ViewModels.Output.Diary
{
    public class DiaryDetailsOutputModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public string User { get; set; }

        public string DiaryType { get; set; }

        public string CreatedOn { get; set; }

        public string LastEditedOn { get; set; }

        public double Rating { get; set; }
    }
}
