﻿using System.Collections.Generic;
using GetShredded.ViewModel.Output.Diary;
using GetShredded.ViewModel.Output.Information;

namespace GetShredded.ViewModel.Output.Users
{
    public class UserOutputModel
    {
        public UserOutputModel()
        {
            this.UserDiaries = new List<DiaryIndexOutputModel>();
            this.Notifications = new List<NotificationOutputModel>();
            this.Messages = new List<MessageOutputModel>();
            this.FollowedDiaries = new List<DiaryIndexOutputModel>();
        }

        public ICollection<DiaryIndexOutputModel> FollowedDiaries { get; set; }

        public ICollection<MessageOutputModel> Messages { get; set; }

        public ICollection<DiaryIndexOutputModel> UserDiaries { get; set; }

        public ICollection<NotificationOutputModel> Notifications { get; set; }

        public string Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Role { get; set; }

        public int Diaries { get; set; }

        public int Comments { get; set; }

        public int MessagesCount { get; set; }

        public string Email { get; set; }
    }
}