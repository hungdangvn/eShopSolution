using eShopSolution.Application.System.Users;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.BackendApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous] //Cho phep chưa đăng nhập vẫn có thể gọi được method này
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.Authenticate(request); //Authent và nhận về chuỗi token
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        //http://localhost:port/api/users/register
        [HttpPost("register")]
        [AllowAnonymous] //Cho phep chưa đăng nhập vẫn có thể gọi được method này
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.Register(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        //PUT http://localhost:port/api/users/id
        [HttpPut("{id}")]        //Lay id tren URL
        public async Task<IActionResult> Update(Guid id, [FromBody] UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.Update(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result); //Tra ve doi tuong de phia client xem xet
        }

        //http://localhost:port/api/users/paging?pageIndex=1&pageSize=10&keyword=
        [HttpGet("paging")]     //Các method trùng tên nên đặt các alias để phân biệt
        //[AllowAnonymous]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetUserPagingRequest request) //Ham GET nen [FromQuery] chi dinh lay tham so tu URL
        {
            var users = await _userService.GetUsersPaging(request);
            return Ok(users);
        }

        //http://localhost:port/api/users/id
        [HttpGet("{id}")]     //Các method trùng tên nên đặt các alias để phân biệt
        public async Task<IActionResult> GetById(Guid id) //Ham GET nen [FromQuery] chi dinh lay tham so tu URL
        {
            var user = await _userService.GetById(id);
            return Ok(user);
        }

        //PUT http://localhost:port/api/users/id
        [HttpDelete("{id}")]        //Lay id tren URL
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.Delete(id);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result); //Tra ve doi tuong de phia client xem xet
        }

        //PUT http://localhost:port/api/users/id/roles
        [HttpPut("{id}/roles")]        //Lay id tren URL
        public async Task<IActionResult> RoleAssign(Guid id, [FromBody] RoleAssignRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.RoleAssisgn(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result); //Tra ve doi tuong de phia client xem xet
        }
    }
}