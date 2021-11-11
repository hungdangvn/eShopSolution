using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _config;

        public UserService(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager,
            IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
        }

        public async Task<ApiResult<string>> Authenticate(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null) return new ApiResultError<string>("Tài khoản không tồn tại");

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true); //logoutOnFailure = true: khi login failed nhiều qua thì khóa tài khoản lại
            if (!result.Succeeded)
            {
                return new ApiResultError<string>("Tài khoản hoặc mật khẩu không đúng");
            }

            var roles = _userManager.GetRolesAsync(user);
            //Tao thong tin để đẩy thong tin vao Token
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirtName),
                new Claim(ClaimTypes.Role, string.Join(";",roles)), //noi danh sach cac roles
                new Claim(ClaimTypes.Name, request.UserName)
            };
            //Ma hoa claims
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"])); //Ma hoa Key
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds

                );

            //Neu thanh cong se tra ve chuỗi Token de client bat duoc
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return new ApiResultSuccess<string>(tokenString);
        }

        public async Task<ApiResult<bool>> Register(RegisterRequest request)
        {
            if (await _userManager.FindByNameAsync(request.UserName) != null)
            {
                return new ApiResultError<bool>("Tài khoản đã tồn tại");
            }
            var email = await _userManager.FindByEmailAsync(request.Email);
            if (email != null)
            {
                return new ApiResultError<bool>("Email đã tồn tại");
            }
            var user = new AppUser()
            {
                Dob = request.Dob,
                Email = request.Email,
                FirtName = request.FirtName,
                LastName = request.LastName,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                return new ApiResultSuccess<bool>();
            }
            return new ApiResultError<bool>("Đăng ký không thành công");
        }

        public async Task<ApiResult<PagedResult<UserViewModel>>> GetUsersPaging(GetUserPagingRequest request)
        {
            var query = _userManager.Users.OrderByDescending(x => x.UserName).AsQueryable();
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.UserName.Contains(request.Keyword) ||
                x.FirtName.Contains(request.Keyword) ||
                x.LastName.Contains(request.Keyword) ||
                x.PhoneNumber.Contains(request.Keyword));
            }
            // Paging
            int totalRow = await query.CountAsync();

            //VD muon lay trang 2 pageIndex = 2 thì Skip (2-1)*10 = 10 records và Take 10 record tiếp theo
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)  //ToListAsync thi phai sua lai await
                .Take(request.PageSize)
                .Select(x => new UserViewModel()
                {
                    Id = x.Id,
                    FirtName = x.FirtName,
                    LastName = x.LastName,
                    PhoneNumber = x.PhoneNumber,
                    UserName = x.UserName,
                    Email = x.Email
                }).ToListAsync();

            // Select and projection
            var pageResult = new PagedResult<UserViewModel>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };

            return new ApiResultSuccess<PagedResult<UserViewModel>>(pageResult);
        }

        public async Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest request)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == request.Email && x.Id != id)) //Nếu Có bất cứ thằng user nào có email nào trùng Email mà ngoại trừ thằng User hiện tại
            {
                return new ApiResultError<bool>("Email đã tồn tại");            //... thì trả về luôn
            }
            var user = await _userManager.FindByIdAsync(id.ToString());

            user.Dob = request.Dob;
            user.Email = request.Email;
            user.FirtName = request.FirtName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new ApiResultSuccess<bool>();
            }
            return new ApiResultError<bool>("Cập nhật không thành công");
        }

        public async Task<ApiResult<UserViewModel>> GetById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiResultError<UserViewModel>("User không tồn tại");
            }
            var roles = await _userManager.GetRolesAsync(user); //Lay danh sach quyen dang duoc gán cho user
            var userViewModel = new UserViewModel()
            {
                Id = user.Id,
                FirtName = user.FirtName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                Email = user.Email,
                Dob = user.Dob,
                Roles = roles //Danh danh sach cac roles cua user
            };
            return new ApiResultSuccess<UserViewModel>(userViewModel);
        }

        public async Task<ApiResult<bool>> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiResultError<bool>("User không tồn tại");            //... thì trả về luôn
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return new ApiResultSuccess<bool>();
            }
            return new ApiResultError<bool>("Xóa không thành công");
        }

        public async Task<ApiResult<bool>> RoleAssisgn(Guid id, RoleAssignRequest request)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiResultError<bool>("Tài khoản không tồn tại");
            }
            var removeRoles = request.Roles.Where(x => x.Selected == false).Select(x => x.Name).ToList(); //Tim danh sách role từ request truyền về, cái nào không được click thì bỏ gán cho user
            //await _userManager.RemoveFromRolesAsync(user, removeRoles);//Không thể remove quyền nếu nó không đang được gán

            foreach (var roleName in removeRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roleName) == true) //Kiem tra, neu dang duoc gan
                {
                    await _userManager.RemoveFromRoleAsync(user, roleName); // thi gỡ ra
                }
            }

            var addedRoles = request.Roles.Where(x => x.Selected == true).Select(x => x.Name).ToList();// tìm danh sách role từ request truyền về, cái nào được click thì gán vào user
            foreach (var roleName in addedRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roleName) == false) //Kiem tra nếu user đang chưa được gán roleName,
                {
                    await _userManager.AddToRoleAsync(user, roleName);  //thì gán vào
                }
            }

            return new ApiResultSuccess<bool>();
        }

    }
}