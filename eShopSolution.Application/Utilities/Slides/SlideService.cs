using eShopSolution.Application.Utilities.Slides;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.System.Roles;
using eShopSolution.ViewModels.Utilities.Slides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Roles
{
    public class SlideService : ISlideService
    {
        private readonly eShopDbContext _context;

        public SlideService(eShopDbContext context)
        {
            _context = context; //đăng ký services.AddTransient<RoleManager<AppRole>, RoleManager<AppRole>>(); trong Startup của API
        }

        public async Task<List<SlideViewModel>> GetAll()
        {
            var slides = await _context.Slides.OrderBy(o => o.SortOrder)
                .Select(x => new SlideViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Url = x.Url,
                    Image = x.Image,
                    SortOrder = x.SortOrder
                }).ToListAsync();

            return slides;
        }
    }
}