using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Languages;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Languages
{
    public class LanguageService : ILanguageService
    {
        private readonly IConfiguration _config;
        private readonly eShopDbContext _context;

        public LanguageService(eShopDbContext context,
            IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<ApiResult<List<LanguageViewModel>>> GetAll()
        {
            var languages = await _context.Languages.Select(x => new LanguageViewModel()
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();
            return new ApiResultSuccess<List<LanguageViewModel>>(languages);
        }
    }
}