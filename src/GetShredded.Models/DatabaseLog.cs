using System.ComponentModel.DataAnnotations;

namespace GetShredded.Models
{
    public class DatabaseLog
    {
        public int Id { get; set; }

        [Required]
        public string LogType { get; set; }

        [Required]
        public string Content { get; set; }

        public bool Handled { get; set; }
    }
}
