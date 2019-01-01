using GetShredded.ViewModel.Input;
using GetShredded.ViewModel.Output.Information;

namespace GetShredded.Services.Contracts
{
    public interface IMessageService
    {
        void SendMessage(MessageInputModel inputModel);

        int NewMessages(string userId);

        InformationViewModel Information(string username);

        void AllMessagesSeen(string username);

        void DeleteAllMessages(string userId);

        void MessageSeen(int id);

        void DeleteMessage(int id);
    }
}
