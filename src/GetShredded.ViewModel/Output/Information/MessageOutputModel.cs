using System;

namespace GetShredded.ViewModel.Output.Information
{
    public class MessageOutputModel
    {
        public int Id { get; set; }

        public DateTime SendOn { get; set; }

        public string Sender { get; set; }

        public string Receiver { get; set; }

        public string Text { get; set; }

        public bool? IsReaded { get; set; }
    }
}
