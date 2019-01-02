using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GetShredded.Data;
using GetShredded.Models;
using GetShredded.Services.Contracts;
using GetShredded.ViewModel.Input;
using GetShredded.ViewModel.Output.Comment;
using GetShredded.ViewModel.Output.Information;
using Microsoft.AspNetCore.Identity;

namespace GetShredded.Services
{
    public class MessageService : BaseService, IMessageService
    {
        public MessageService(
            UserManager<GetShreddedUser> userManager,
            GetShreddedContext context,
            IMapper mapper)
            : base(userManager, context, mapper)
        {
        }

        public void SendMessage(MessageInputModel inputModel)
        {
            var sender = this.UserManager.FindByNameAsync(inputModel.SenderName).GetAwaiter().GetResult();
            var receiver = this.UserManager.FindByNameAsync(inputModel.ReceiverName).GetAwaiter().GetResult();

            var message = new Message
            {
                IsReaded = false,
                SendOn = inputModel.SendDate,
                Text = inputModel.Message,
                ReceiverId = receiver.Id,
                SenderId = sender.Id
            };

            this.Context.Messages.Add(message);
            this.Context.SaveChanges();
        }

        public int NewMessages(string userId)
        {
            var newMessages = this.Context.Messages.Count(x => x.ReceiverId == userId && x.IsReaded == false);

            return newMessages;
        }

        public InformationViewModel Information(string username)
        {
            var allMessages = this.Context.Messages.Where(x => x.Receiver.UserName == username).ProjectTo<MessageOutputModel>(Mapper.ConfigurationProvider).ToList();

            var newMessages = allMessages.Where(x => x.IsReaded == false).ToArray();

            var oldMessages = allMessages.Where(x => newMessages.All(z => z.Id != x.Id)).ToArray();

            var comments = this.Context.Comments
                .Where(x => x.GetShreddedUser.UserName == username)
                .ProjectTo<CommentOutputModel>(Mapper.ConfigurationProvider).ToArray();

            var notifications = this.Context.Notifications
                .Where(x => x.GetShreddedUser.UserName == username)
                .ProjectTo<NotificationOutputModel>(Mapper.ConfigurationProvider).ToList();

            var model = new InformationViewModel
            {
                NewMessages = newMessages,
                OldMessages = oldMessages,
                Notifications = notifications,
                UserComments = comments
            };

            return model;
        }

        public void AllMessagesSeen(string username)
        {
            var messages = this.Context.Messages
                .Where(x => x.Receiver.UserName == username && x.IsReaded == false)
                .ToList();

            messages.ForEach(x => x.IsReaded = true);

            this.Context.UpdateRange(messages);
            this.Context.SaveChanges();
        }

        public void DeleteAllMessages(string userId)
        {
            var messages = this.Context.Messages.Where(x => x.Receiver.Id == userId).ToArray();

            this.Context.Messages.RemoveRange(messages);

            this.Context.SaveChanges();
        }

        public void MessageSeen(int id)
        {
            var message = this.Context.Messages.Find(id);
            message.IsReaded = true;

            this.Context.Messages.Update(message);

            this.Context.SaveChanges();
        }

        public void DeleteMessage(int id)
        {
            var message = this.Context.Messages.Find(id);

            this.Context.Messages.Remove(message);

            this.Context.SaveChanges();
        }
    }
}
