using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GetShredded.Models;
using GetShredded.Services.Contracts;
using GetShredded.ViewModel.Input;
using GetShredded.ViewModel.Output.Information;
using Microsoft.AspNetCore.Identity;
using NUnit.Framework;

namespace GetShredded.Tests.GetShreddedServices.MessageService
{
    [TestFixture]
    public class MessageServiceTests : BaseServiceFake
    {
        private IMessageService messageService => (IMessageService)this.Provider.GetService(typeof(IMessageService));
        private UserManager<GetShreddedUser> userManager =>
            (UserManager<GetShreddedUser>)this.Provider.GetService(typeof(UserManager<GetShreddedUser>));

        [Test]
        public void InformationReturningCorrectMesssagesAndNotificationsForUser()
        {
            //arrange
            var user = new GetShreddedUser
            {
                Id = "UserId",
                UserName = "User"
            };

            var messages = new[]
            {
                new Message
                {
                    IsReaded = true,
                    ReceiverId = user.Id,
                    Receiver = user,
                    Id = 1,
                    Text = "It works?",
                },

                new Message
                {
                    IsReaded = false,
                    ReceiverId = user.Id,
                    Receiver = user,
                    Id = 2,
                    Text = "It works two?",
                }
            };

            var notifications = new[]
            {
                new Notification
                {
                    GetShreddedUser = user,
                    GetShreddedUserId = user.Id,
                    Message = "boom",
                    Seen = false,
                },

                new Notification
                {
                    GetShreddedUser = user,
                    GetShreddedUserId = user.Id,
                    Message = "boom",
                    Seen = false,
                }
            };

            this.Context.Messages.AddRange(messages);
            this.Context.Notifications.AddRange(notifications);
            this.userManager.CreateAsync(user).GetAwaiter().GetResult();

            this.Context.SaveChanges();

            //act
            string username = user.UserName;
            var result = this.messageService.Information(username);

            //assert

            result.Should().BeOfType<InformationViewModel>();

            result.Notifications.As<IEnumerable<NotificationOutputModel>>()
                .Should().NotBeEmpty().And.HaveCount(2)
                .And.ContainItemsAssignableTo<NotificationOutputModel>();

            result.OldMessages.As<IEnumerable<MessageOutputModel>>()
                .Should().NotBeEmpty().And.HaveCount(1)
                .And.ContainItemsAssignableTo<MessageOutputModel>();

            result.NewMessages.As<IEnumerable<MessageOutputModel>>()
                .Should().NotBeEmpty().And.HaveCount(1)
                .And.ContainItemsAssignableTo<MessageOutputModel>();
        }

        [Test]
        public void AllMessageSeenShouldChangeForAllMessagesIsReadenToTrue()
        {
            //arrange
            var user = new GetShreddedUser
            {
                Id = "UserId",
                UserName = "User"
            };

            var messages = new[]
            {
                new Message
                {
                    IsReaded = false,
                    ReceiverId = user.Id,
                    Receiver = user,
                    Id = 1,
                    Text = "It works?",
                },

                new Message
                {
                    IsReaded = false,
                    ReceiverId = user.Id,
                    Receiver = user,
                    Id = 2,
                    Text = "It works two?",
                }
            };

            this.Context.Messages.AddRange(messages);
            this.userManager.CreateAsync(user).GetAwaiter().GetResult();
            this.Context.SaveChanges();

            //act
            string username = user.UserName;
            this.messageService.AllMessagesSeen(username);

            //assert

            var messagesFromDatabase = this.Context.Messages.Where(x => x.ReceiverId == user.Id).ToArray();

            messagesFromDatabase
                .Select(x => x.IsReaded.Should()
                .NotBeFalse())
                .Should().HaveCount(2);
        }

        [Test]
        public void DeleteAllMessagesShouldDeleteAllMessagesForUser()
        {
            //arrange
            var user = new GetShreddedUser
            {
                Id = "UserId",
                UserName = "User"
            };

            var messages = new[]
            {
                new Message
                {
                    IsReaded = false,
                    ReceiverId = user.Id,
                    Receiver = user,
                    Id = 1,
                    Text = "It works?",
                },

                new Message
                {
                    IsReaded = false,
                    ReceiverId = user.Id,
                    Receiver = user,
                    Id = 2,
                    Text = "It works two?",
                }
            };

            this.Context.Messages.AddRange(messages);
            this.userManager.CreateAsync(user).GetAwaiter().GetResult();
            this.Context.SaveChanges();

            //act
            string userId = user.Id;
            this.messageService.DeleteAllMessages(userId);

            //assert
            var messagesFromDb = this.Context.Messages.Where(x => x.ReceiverId == user.Id).ToArray();

            messagesFromDb.Should().BeEmpty();
        }

        [Test]
        public void DeleteMessageShouldDeleteOnlyTheMessageWithGivenId()
        {
            var user = new GetShreddedUser
            {
                Id = "UserId",
                UserName = "User"
            };

            var messages = new[]
            {
                new Message
                {
                    IsReaded = false,
                    ReceiverId = user.Id,
                    Receiver = user,
                    Id = 1,
                    Text = "It works?",
                },

                new Message
                {
                    IsReaded = false,
                    ReceiverId = user.Id,
                    Receiver = user,
                    Id = 2,
                    Text = "It works?two",
                }
            };

            this.Context.Messages.AddRange(messages);
            this.userManager.CreateAsync(user).GetAwaiter().GetResult();
            this.Context.SaveChanges();

            //act
            int messageId = messages[0].Id;
            this.messageService.DeleteMessage(messageId);

            //assert
            var messagesFromDatabase = this.Context.Messages.Where(x => x.ReceiverId == user.Id).ToArray();
            int singleMessageIdShouldBe = 2;

            messagesFromDatabase.Should()
                .ContainSingle()
                .And.Subject.Should()
                .Contain(x => x.Id == singleMessageIdShouldBe);
        }

        [Test]
        public void NewMessagesShouldReturnTheCountOfMessagesWhereIsReadenIsFalse()
        {
            //arrange
            var user = new GetShreddedUser
            {
                Id = "UserId",
                UserName = "User"
            };

            var messages = new[]
            {
                new Message
                {
                    IsReaded = false,
                    ReceiverId = user.Id,
                    Receiver = user,
                    Id = 1,
                    Text = "It works?",
                },

                new Message
                {
                    IsReaded = false,
                    ReceiverId = user.Id,
                    Receiver = user,
                    Id = 2,
                    Text = "It works?two",
                }
            };

            this.Context.Messages.AddRange(messages);
            this.userManager.CreateAsync(user).GetAwaiter().GetResult();
            this.Context.SaveChanges();

            //act
            string userId = user.Id;
            int count = this.messageService.NewMessages(userId);
            int expectedCount = messages.Count();
            //assert
            count.Should().BePositive().And.Subject.Should().Be(expectedCount);
        }

        [Test]
        public void MessageSeenShouldChangeMessageIsReadenPropertyToTrue()
        {
            //arrange
            var user = new GetShreddedUser
            {
                Id = "UserId",
                UserName = "User"
            };

            var message = new Message
            {
                IsReaded = false,
                ReceiverId = user.Id,
                Receiver = user,
                Id = 1,
                Text = "It works?",
            };

            this.Context.Messages.Add(message);
            this.userManager.CreateAsync(user).GetAwaiter().GetResult();
            this.Context.SaveChanges();

            //act
            int messageId = message.Id;
            this.messageService.MessageSeen(messageId);

            //assert
            var result = this.Context.Messages.Find(messageId);

            result.Should()
                .NotBeNull()
                .And.Subject.As<Message>()
                .IsReaded
                .Should().BeTrue();
        }

        [Test]
        public void SenMessageShouldCreateNewMessage()
        {
            //arrange
            var sender = new GetShreddedUser
            {
                Id = "SenderId",
                UserName = "Sender"
            };

            var receiver = new GetShreddedUser
            {
                Id = "ReceiverId",
                UserName = "Receiver"
            };

            var message = new MessageInputModel
            {
                SenderName = sender.UserName,
                ReceiverName = receiver.UserName,
                SendDate = DateTime.UtcNow,
                Message = "It works!!",
            };

            this.userManager.CreateAsync(receiver).GetAwaiter().GetResult();
            this.userManager.CreateAsync(sender).GetAwaiter().GetResult();
            this.Context.SaveChanges();

            //act
            this.messageService.SendMessage(message);

            //assert
            var result = this.Context.Messages.FirstOrDefault();

            result.Should().NotBeNull();
            result?.SenderId.Should().Be(sender.Id);
            result?.ReceiverId.Should().Be(receiver.Id);
        }
    }
}
