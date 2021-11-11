using eShopSolution.ApiIntergration;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Controllers
{
    //[Authorize] ko cần nữa do kế thừa từ BaseController
    public class UserController : BaseController
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IRoleApiClient _roleApiClient;

        public UserController(IUserApiClient userApiClient,
            IRoleApiClient roleApiClient)
        {
            _userApiClient = userApiClient;
            _roleApiClient = roleApiClient;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 5)
        {
            //B2. Dựng request
            var request = new GetUserPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _userApiClient.GetUsersPagings(request);   //Controller sử dụng Service của Project AdminApp để gửi request, lấy về PagedResult<UserViewModel> tạo View
            ViewBag.Keyword = keyword; //Chuyển keyword ra view để tái hiện lại trên textbox Search
            if (TempData["result"] != null) //Nhận TempData["result"] được redirect từ các method Create, Update, Delete
            {
                ViewBag.Msg = TempData["result"];
            }
            if (data != null)
            {
                return View(data.ResultObj);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _userApiClient.GetById(id);
            return View(result.ResultObj);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterRequest request)
        {
            if (!ModelState.IsValid) return View();

            var result = await _userApiClient.RegisterUser(request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Thêm mới người dùng " + request.FirtName + " thành công";
                return RedirectToAction("Index"); //Neu regist thanh cong thì Redirect
            }
            ModelState.AddModelError("", result.Message);//show error msg tu API
            return View(request); //Neu không thành công thì trả về View với request để user sửa và Error Message (duoc gan tu result.Message)
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var result = await _userApiClient.GetById(id); //Tim user theo id de load len form
            if (result != null && result.IsSuccessed)
            {
                var user = result.ResultObj;
                var updateRequest = new UserUpdateRequest()
                {
                    Id = user.Id,
                    FirtName = user.FirtName,
                    LastName = user.LastName,
                    Dob = user.Dob,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                };
                return View(updateRequest); //Load dữ liệu ra
            }

            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateRequest request)
        {
            if (!ModelState.IsValid) return View();

            var result = await _userApiClient.UpdateUser(request.Id, request);
            if (result != null && result.IsSuccessed)
            {
                TempData["result"] = "Cập nhật người dùng " + request.FirtName + " thành công";
                return RedirectToAction("Index"); //Neu update thanh cong thì Redirect
            }
            ModelState.AddModelError("", result.Message);
            return View(request); //Neu không thành công thì trả về View với request để user sửa
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); //Logout những session cũ
            HttpContext.Session.Remove("Token");//Xóa token cũ đi
            return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userApiClient.GetById(id); //Tim user theo id de load len form
            if (result != null && result.IsSuccessed)
            {
                var user = result.ResultObj;
                var deleteRequest = new UserDeleteRequest()
                {
                    Id = user.Id,
                    UserName = user.UserName
                };
                return View(deleteRequest); //Load dữ liệu ra
            }

            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UserDeleteRequest request)
        {
            if (!ModelState.IsValid) return View();
            var result = await _userApiClient.DeleteUser(request.Id);
            if (result != null && result.IsSuccessed)
            {
                TempData["result"] = "Xóa người dùng thành công";
                return RedirectToAction("Index"); //Neu delete thanh cong thì Redirect
            }
            ModelState.AddModelError("", result.Message);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> RoleAssign(Guid id) //Nhan vao user id
        {
            var roleAssignRequest = await GetRoleAssignRequest(id);
            return View(roleAssignRequest);
        }

        [HttpPost]
        public async Task<IActionResult> RoleAssign(RoleAssignRequest request)
        {
            if (!ModelState.IsValid) return View();

            var result = await _userApiClient.RoleAssignForUser(request.Id, request);
            if (result != null && result.IsSuccessed)
            {
                TempData["result"] = "Gán quyền người dùng thành công";
                return RedirectToAction("Index"); //Neu update thanh cong thì Redirect
            }
            ModelState.AddModelError("", result.Message);
            var roleAssignRequest = await GetRoleAssignRequest(request.Id);
            return View(roleAssignRequest); //Neu không thành công thì trả về View với request để user sửa
        }

        private async Task<RoleAssignRequest> GetRoleAssignRequest(Guid id)
        {
            var userObj = await _userApiClient.GetById(id); //tim user
            var roleObj = await _roleApiClient.GetAll(); //tim danh sách quyền có trong hệ thống
            var roleAssignRequest = new RoleAssignRequest();
            foreach (var role in roleObj.ResultObj)
            {
                roleAssignRequest.Roles.Add(new SelectItem()        //Nhờ Roles được new khi khai báo thuộc tính nên khi Add vào không bị Add vào danh sách null
                {
                    Id = role.Id.ToString(),
                    Name = role.Name,
                    Selected = userObj.ResultObj.Roles.Contains(role.Name) //Neu user đang được assign role này thì Selected = true
                });
            }
            return roleAssignRequest;
        }
    }
}