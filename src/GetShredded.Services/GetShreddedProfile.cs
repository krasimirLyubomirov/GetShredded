using System;
using System.Linq;
using AutoMapper;
using GetShredded.Common;
using GetShredded.Models;
using GetShredded.ViewModel.Input.Users;
using GetShredded.ViewModel.Output.Diary;
using GetShredded.ViewModel.Output.Information;
using GetShredded.ViewModel.Output.Users;

namespace GetShredded.Services
{
    public class GetShreddedProfile : Profile
    {
        public GetShreddedProfile()
        {
            CreateMap<RegisterInputModel, GetShreddedUser>();

            CreateMap<GetShreddedDiary, DiaryIndexOutputModel>()
                .ForMember(opt => opt.User, cfg => cfg.MapFrom(x => x.User.UserName))
                .ForMember(opt => opt.Title, cfg => cfg.MapFrom(x => x.Title))
                .ForMember(opt => opt.DiaryType, cfg => cfg.MapFrom(x => x.Type.Name))
                .ForMember(opt => opt.Id, cfg => cfg.MapFrom(x => x.Id))
                .ForMember(opt => opt.CreatedOn, cfg => cfg.MapFrom(x => x.CreatedOn.ToShortDateString()))
                .ForMember(opt => opt.Summary, cfg => cfg.MapFrom(x => x.Summary ?? GlobalConstants.NoSummary))
                .ForMember(opt => opt.Rating, cfg => cfg.MapFrom(x => x.Ratings.Any() ? x.Ratings.Average(r => r.DiaryRating.Rating) : GlobalConstants.Zero));

            CreateMap<GetShreddedUser, UserOutputModel>()
                .ForMember(x => x.Id, cfg => cfg.MapFrom(x => x.Id))
                .ForMember(x => x.Username, cfg => cfg.MapFrom(x => x.UserName))
                .ForMember(x => x.Email, cfg => cfg.MapFrom(x => x.Email))
                .ForMember(x => x.Role, cfg => cfg.Ignore())
                .ForMember(x => x.Comments, opt => opt.MapFrom(x => x.Comments.Count))
                .ForMember(x => x.MessagesCount, opt => opt.MapFrom(x => x.SendMessages.Count + x.SendMessages.Count))
                .ForMember(x => x.Diaries, opt => opt.MapFrom(x => x.GetShreddedDiaries.Count))
                .ForMember(x => x.Messages, opt => opt.MapFrom(x => x.SendMessages.Concat(x.ReceivedMessages)))
                .ForMember(x => x.Notifications, o => o.MapFrom(x => x.Notifications))
                .ForMember(x => x.UserDiaries, o => o.MapFrom(x => x.GetShreddedDiaries))
                .ForMember(x => x.FollowedDiaries, o => o.MapFrom(x => x.FollowedDiaries
                    .Where(z => z.GetShreddedUserId == x.Id)
                    .Select(s => s.GetShreddedDiary)));

            CreateMap<Notification, NotificationOutputModel>()
                .ForMember(x => x.Id, o => o.MapFrom(x => x.Id))
                .ForMember(x => x.Message, o => o.MapFrom(x => x.Message))
                .ForMember(x => x.Seen, o => o.MapFrom(x => x.Seen))
                .ForMember(x => x.UpdatedDiaryId, o => o.MapFrom(x => x.UpdatedDiaryId))
                .ForMember(x => x.Username, o => o.MapFrom(x => x.GetShreddedUser.UserName));

            CreateMap<Message, MessageOutputModel>()
                .ForMember(x => x.Id, o => o.MapFrom(x => x.Id))
                .ForMember(x => x.IsReaded, o => o.MapFrom(x => x.IsReaded))
                .ForMember(x => x.Sender, o => o.MapFrom(x => x.Sender.UserName))
                .ForMember(x => x.Receiver, o => o.MapFrom(x => x.Receiver.UserName))
                .ForMember(x => x.SendOn, o => o.MapFrom(x => x.SendOn))
                .ForMember(x => x.Text, o => o.MapFrom(x => x.Text));
        }
    }
}
