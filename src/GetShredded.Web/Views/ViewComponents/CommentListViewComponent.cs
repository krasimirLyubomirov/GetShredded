using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GetShredded.Data;
using GetShredded.ViewModels.Output.Comment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GetShredded.Web.Views.ViewComponents
{
    public class CommentsListViewComponent : ViewComponent
    {
        public CommentsListViewComponent(GetShreddedContext context, IMapper mapper)
        {
            this.Context = context;
            this.Mapper = mapper;
        }

        protected IMapper Mapper { get; set; }

        protected GetShreddedContext Context { get; }

        public async Task<IViewComponentResult> InvokeAsync(int diaryId)
        {
            var comments = await GetPagesAsync(diaryId);
            this.ViewData[Common.GlobalConstants.DiaryId] = diaryId;
            return View(comments);
        }

        private async Task<List<CommentOutputModel>> GetPagesAsync(int id)
        {
            var comments = await this.Context.Comments
                .Where(x => x.GetShreddedDiary.Id == id)
                .ProjectTo<CommentOutputModel>(Mapper.ConfigurationProvider)
                .ToListAsync();

            return comments;
        }
    }
}
