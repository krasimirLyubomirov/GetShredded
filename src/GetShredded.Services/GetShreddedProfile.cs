using System;
using System.Linq;
using AutoMapper;
using GetShredded.Common;
using GetShredded.Models;
using GetShredded.ViewModel.Input;
using GetShredded.ViewModel.Input.Page;
using GetShredded.ViewModel.Input.Users;
using GetShredded.ViewModel.Output.Diary;
using GetShredded.ViewModel.Output.Information;
using GetShredded.ViewModel.Output.Page;
using GetShredded.ViewModel.Output.Users;
using GetShredded.ViewModels.Output.Comment;
using GetShredded.ViewModels.Output.Diary;
using GetShredded.ViewModels.Output.Users;

namespace GetShredded.Services
{
    public class GetShreddedProfile : Profile
    {
        public GetShreddedProfile()
        {
            CreateMap<RegisterInputModel, GetShreddedUser>();

            CreateMap<GetShreddedDiary, DiaryOutputModel>()
                .ForMember(opt => opt.Ratings, cfg => cfg.MapFrom(x => x.Ratings.Select(z => z.DiaryRating.Rating)))
                .ForMember(opt => opt.Followers, cfg => cfg.MapFrom(x => x.Followers.Select(xx => xx.GetShreddedUser)))
                .ForMember(x => x.LastEditedOn, opt => opt.MapFrom(x => x.LastEditedOn.Date))
                .ForMember(x => x.CreatedOn, opt => opt.MapFrom(x => x.CreatedOn.Date))
                .ForMember(o => o.Rating, opt => opt.Ignore())
                .ForMember(o => o.User, opt => opt.MapFrom(x => x.User))
                .ForMember(x => x.Summary, opt => opt.NullSubstitute(GlobalConstants.NoSummary))
                .ForMember(x => x.Comments, opt => opt.MapFrom(x => x.Comments))
                .ForMember(x => x.Pages, opt => opt.MapFrom(x => x.Pages))
                .ForMember(x => x.Title, o => o.MapFrom(x => x.Title))
                .ForMember(x => x.Type, o => o.MapFrom(x => x.Type))
                .ForMember(x => x.Id, o => o.MapFrom(x => x.Id))
                .ForMember(x => x.ImageUrl, o => o.AllowNull());

            CreateMap<GetShreddedDiary, DiaryIndexOutputModel>()
                .ForMember(opt => opt.User, cfg => cfg.MapFrom(x => x.User.UserName))
                .ForMember(opt => opt.Title, cfg => cfg.MapFrom(x => x.Title))
                .ForMember(opt => opt.DiaryType, cfg => cfg.MapFrom(x => x.Type.Name))
                .ForMember(opt => opt.Id, cfg => cfg.MapFrom(x => x.Id))
                .ForMember(opt => opt.CreatedOn, cfg => cfg.MapFrom(x => x.CreatedOn.ToShortDateString()))
                .ForMember(opt => opt.Summary, cfg => cfg.MapFrom(x => x.Summary ?? GlobalConstants.NoSummary))
                .ForMember(opt => opt.Rating, cfg => cfg.MapFrom(x => x.Ratings.Any() ? x.Ratings.Average(r => r.DiaryRating.Rating) : GlobalConstants.Zero));

            CreateMap<DiaryInputModel, GetShreddedDiary>()
                .ForMember(x => x.CreatedOn, opt => opt.MapFrom(x => x.CreatedOn))
                .ForMember(x => x.LastEditedOn, opt => opt.MapFrom(x => x.CreatedOn))
                .ForMember(x => x.Summary, o => o.MapFrom(x => x.Summary))
                .ForMember(x => x.Title, o => o.MapFrom(x => x.Title))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<GetShreddedDiary, DiaryDetailsOutputModel>()
                .ForMember(x => x.CreatedOn, opt => opt.MapFrom(x => x.CreatedOn.ToShortDateString()))
                .ForMember(x => x.LastEditedOn, opt => opt.MapFrom(x => x.LastEditedOn.ToShortDateString()))
                .ForMember(x => x.User, opt => opt.MapFrom(x => x.User.UserName))
                .ForMember(x => x.Title, opt => opt.MapFrom(x => x.Title))
                .ForMember(x => x.DiaryType, opt => opt.MapFrom(x => x.Type.Name))
                .ForMember(x => x.Rating, cfg => cfg.MapFrom(x => x.Rating))
                .ForMember(x => x.ImageUrl, o => o.MapFrom(x => x.ImageUrl))
                .ForMember(opt => opt.Summary, cfg => cfg.NullSubstitute(GlobalConstants.NoSummary));

            CreateMap<GetShreddedUser, UserOutputModel>()
                .ForMember(x => x.Id, cfg => cfg.MapFrom(x => x.Id))
                .ForMember(x => x.Username, cfg => cfg.MapFrom(x => x.UserName))
                .ForMember(x => x.FirstName, cfg => cfg.MapFrom(x => x.FirstName))
                .ForMember(x => x.LastName, cfg => cfg.MapFrom(x => x.LastName))
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

            CreateMap<GetShreddedUser, ChangeRoleModel>()
                .ForMember(x => x.Id, cfg => cfg.MapFrom(x => x.Id))
                .ForMember(x => x.Username, cfg => cfg.MapFrom(x => x.UserName))
                .ForMember(x => x.Role, cfg => cfg.Ignore())
                .ForMember(x => x.UpdatedRole, cfg => cfg.Ignore())
                .ForMember(x => x.ApplicationRoles, cfg => cfg.Ignore());

            CreateMap<GetShreddedUser, UserAdminOutputModel>()
                .ForMember(x => x.Id, cfg => cfg.MapFrom(x => x.Id))
                .ForMember(x => x.Username, cfg => cfg.MapFrom(x => x.UserName))
                .ForMember(x => x.FirstName, cfg => cfg.MapFrom(x => x.FirstName))
                .ForMember(x => x.LastName, cfg => cfg.MapFrom(x => x.LastName))
                .ForMember(x => x.Role, cfg => cfg.Ignore())
                .ForMember(x => x.Diaries, cfg => cfg.MapFrom(x => x.GetShreddedDiaries.Count))
                .ForMember(x => x.Comments, cfg => cfg.MapFrom(x => x.Comments.Count))
                .ForMember(x => x.MessageCount, cfg => cfg.MapFrom(x => x.SendMessages.Concat(x.ReceivedMessages).Count()));

            CreateMap<GetShreddedUser, UserOutputDiaryModel>()
                .ForMember(x => x.Username, o => o.MapFrom(z => z.UserName))
                .ForMember(x => x.FirstName, o => o.MapFrom(z => z.FirstName))
                .ForMember(x => x.LastName, o => o.MapFrom(z => z.LastName))
                .ForMember(x => x.Id, o => o.MapFrom(z => z.Id));

            CreateMap<Page, PageOutputModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Content, opt => opt.MapFrom(x => x.Content))
                .ForMember(x => x.Length, o => o.MapFrom(x => x.Content.Length))
                .ForMember(x => x.CreatedOn, opt => opt.MapFrom(x => x.CreatedOn))
                .ForMember(x => x.User, o => o.MapFrom(x => x.GetShreddedUser.UserName))
                .ForMember(x => x.Title, o => o.MapFrom(x => x.Title ?? GlobalConstants.NoTitleAdded));

            CreateMap<Page, PageEditModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Content, opt => opt.MapFrom(x => x.Content))
                .ForMember(x => x.Length, o => o.MapFrom(x => x.Content.Length))
                .ForMember(x => x.CreatedOn, opt => opt.MapFrom(x => x.CreatedOn))
                .ForMember(x => x.Author, o => o.MapFrom(x => x.GetShreddedUser.UserName))
                .ForMember(x => x.Title, o => o.MapFrom(x => x.Title ?? GlobalConstants.NoTitleAdded))
                .ForMember(x => x.DiaryId, o => o.MapFrom(x => x.GetShreddedDiaryId)).ReverseMap();

            CreateMap<PageInputModel, Page>()
                .ForMember(x => x.GetShreddedDiaryId, opt => opt.MapFrom(x => x.DiaryId))
                .ForMember(x => x.Content, opt => opt.MapFrom(x => x.Content))
                .ForMember(x => x.CreatedOn, opt => opt.MapFrom(x => x.CreatedOn))
                .ForMember(x => x.Title, opt => opt.MapFrom(x => x.Title))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<Comment, CommentOutputModel>()
                .ForMember(x => x.Id, o => o.MapFrom(x => x.Id))
                .ForMember(x => x.User, o => o.MapFrom(x => x.GetShreddedUser.UserName ?? GlobalConstants.DeletedUser))
                .ForMember(x => x.CommentedOn, o => o.MapFrom(x => x.CommentedOn))
                .ForMember(x => x.Message, o => o.MapFrom(x => x.Message))
                .ForMember(x => x.DiaryId, o => o.NullSubstitute(default(int)));

            CreateMap<CommentInputModel, Comment>()
                .ForMember(x => x.Message, o => o.MapFrom(x => x.Message))
                .ForMember(x => x.CommentedOn, o => o.MapFrom(x => x.CommentedOn))
                .ForMember(x => x.GetShreddedDiaryId, o => o.MapFrom(x => x.DiaryId))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DiaryType, DiaryTypeOutputModel>()
                .ForMember(x => x.Type, opt => opt.MapFrom(z => z.Name)).ReverseMap();

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
