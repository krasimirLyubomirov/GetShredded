using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using System.Linq;
using AutoMapper.QueryableExtensions;
using GetShredded.Common;
using GetShredded.Data;
using GetShredded.ViewModel.Output.Page;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GetShredded.Web.Views.ViewComponents
{
    [ViewComponent(Name = "PageList")]
    public class PageListViewComponent : ViewComponent
    {
        public PageListViewComponent(GetShreddedContext context, IMapper mapper)
        {
            this.Context = context;
            this.Mapper = mapper;
        }

        protected IMapper Mapper { get; set; }

        protected GetShreddedContext Context { get; }

        public async Task<IViewComponentResult> InvokeAsync(int diaryId)
        {
            var pages = await GetPagesAsync(diaryId);
            this.ViewData[GlobalConstants.DiaryId] = diaryId;
            return View(pages);
        }

        private async Task<List<PageOutputModel>> GetPagesAsync(int id)
        {
            var pages = await this.Context.Pages
                .Where(x => x.GetShreddedDiaryId == id)
                .ProjectTo<PageOutputModel>(Mapper.ConfigurationProvider)
                .ToListAsync();

            return pages;
        }
    }
}
