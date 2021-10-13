using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Roles;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Sevices
{
    public class RoleApiClient : IRoleApiClient ////Dang ky services.AddTransient<IRoleApiClient, RoleApiClient>(); trong startup
    {
        public readonly IHttpClientFactory _httpClientFactory; //An IHttpClientFactory can be requested using dependency injection (DI)
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RoleApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResult<List<RoleViewModel>>> GetAll()
        {
            //B1. Tạo 1 client
            //B2. Gán các thông số cho client (BaseAddress, Authorization)

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions); //Gán Token vào Thuộc tính Authorization của RequestHeader

            var response = await client.GetAsync($"/api/roles");               //gửi request đến API với tham số là URI và các tham số kèm theo, nhận lại response từ Server

            var body = await response.Content.ReadAsStringAsync();                              //tách cái body của response ra
            if (response.IsSuccessStatusCode)
            {
                List<RoleViewModel> myDeserializationObjList = (List<RoleViewModel>)JsonConvert.DeserializeObject(body, typeof(List<RoleViewModel>));
                return new ApiResultSuccess<List<RoleViewModel>>(myDeserializationObjList); //chuyển đồi cái body ra thành đối tượng thằng con ApiResultSuccess<PagedResult<UserViewModel>> kế thừa từ thằng cha ApiResult
            }

            return JsonConvert.DeserializeObject<ApiResultError<List<RoleViewModel>>>(body);
        }
    }
}