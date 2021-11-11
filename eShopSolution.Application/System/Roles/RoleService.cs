using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.System.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Roles
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _rolesManager;
        public RoleService(RoleManager<AppRole> roleManager)
        {
            _rolesManager = roleManager; //đăng ký services.AddTransient<RoleManager<AppRole>, RoleManager<AppRole>>(); trong Startup của API
        }
        public async Task<List<RoleViewModel>> GetAll()
        {
            var roles = await _rolesManager.Roles
                .Select(x => new RoleViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToListAsync();

            return roles;
        }
    }
}
