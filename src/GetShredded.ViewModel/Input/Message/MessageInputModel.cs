using System;
using System.ComponentModel.DataAnnotations;
using GetShredded.Common;

namespace GetShredded.ViewModel.Input
{
    public class MessageInputModel
    {
        [Required]
        [StringLength(GlobalConstants.MessageLength)]
        public string Message { get; set; }

        public DateTime SendDate { get; set; }

        public string SenderName { get; set; }

        public string ReceiverName { get; set; }
    }
}
