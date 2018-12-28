using System;

namespace GetShredded.Models
{
    public class Message
    {
        public int Id { get; set; }

        public DateTime SendOn { get; set; }

        public string SenderId { get; set; }
        public GetShreddedUser Sender { get; set; }

        public string ReceiverId { get; set; }
        public GetShreddedUser Receiver { get; set; }

        public string Text { get; set; }

        public bool? IsReaded { get; set; }
    }
}
