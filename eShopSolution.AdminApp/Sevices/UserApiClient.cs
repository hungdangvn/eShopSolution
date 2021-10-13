using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
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

//using Microsoft.AspNetCore.Http;

namespace eShopSolution.AdminApp.Sevices
{
    public class UserApiClient : IUserApiClient
    {
        public readonly IHttpClientFactory _httpClientFactory; //An IHttpClientFactory can be requested using dependency injection (DI)
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserApiClient(IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResult<string>> Authenticate(LoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            //B1. Tạo 1 client
            //B2. Gán các thông số cho client (BaseAddress, Authorization)

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var response = await client.PostAsync("/api/users/authenticate", httpContent); //B3. Post request đến API với tham số là URI và các tham số kèm theo, nhận lại response từ Server
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(await response.Content.ReadAsStringAsync()); ; //Tách token dạng Json từ trong response để return, chuyển đổi nó thành thằng con ApiResultSuccess kế thừa từ thằng cha ApiResult
            }

            return JsonConvert.DeserializeObject<ApiResultError<string>>(await response.Content.ReadAsStringAsync()); ; ////Tách token dạng Json từ trong response để return, chuyển đổi nó thành thằng con ApiResultError kế thừa từ thằng cha ApiResult
        }

        public async Task<ApiResult<PagedResult<UserViewModel>>> GetUsersPagings(GetUserPagingRequest request)
        {
            //////B1. Tạo 1 client
            //////B2. Gán các thông số cho client (BaseAddress, Authorization)
            var client = _httpClientFactory.CreateClient();
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions); //Gán Token vào Thuộc tính Authorization của RequestHeader
            //api/Users/paging?PageIndex=1&PageSize=7
            var response = await client.GetAsync($"/api/users/paging?pageIndex=" +              //gửi request đến API với tham số là URI và các tham số kèm theo, nhận lại response từ Server
                $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}");
            var body = await response.Content.ReadAsStringAsync();                              //tách cái body của response ra

            var users = JsonConvert.DeserializeObject<ApiResultSuccess<PagedResult<UserViewModel>>>(body);        //chuyển đồi cái body ra thành đối tượng thằng con ApiResultSuccess<PagedResult<UserViewModel>> kế thừa từ thằng cha ApiResult

            return users;
        }

        public async Task<ApiResult<bool>> RegisterUser(RegisterRequest request)
        {
            //B1. Tạo 1 client
            //B2. Gán các thông số cho client (BaseAddress, Authorization)

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            //Tạo body cho câu request API PostAsync
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"api/users/register", httpContent);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiResultError<bool>>(result); //Tra ve ApiResultError với message có sẵn tùy theo tình huông
        }

        public async Task<ApiResult<bool>> UpdateUser(Guid id, UserUpdateRequest request)
        {
            //B1. Tạo 1 client
            //B2. Gán các thông số cho client (BaseAddress, Authorization)

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions); //Gán Token vào Thuộc tính Authorization của RequestHeader

            var json = JsonConvert.SerializeObject(request); //Chuyển request thành dạng json
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/users/{id}", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<bool>>(result);
            }

            return JsonConvert.DeserializeObject<ApiResultError<bool>>(result);
        }

        public async Task<ApiResult<UserViewModel>> GetById(Guid id)
        {
            //return await GetAsync<ApiResult<UserViewModel>>($"/api/users/{id}");
            ////B1. Tạo 1 client
            ////B2. Gán các thông số cho client (BaseAddress, Authorization)
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions); //Gán Token vào Thuộc tính Authorization của RequestHeader

            var response = await client.GetAsync($"/api/users/{id}");
            var body = await response.Content.ReadAsStringAsync();                              //tách cái body của response ra
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<UserViewModel>>(body);        //chuyển đồi cái body ra thành đối tượng thằng con ApiResultSuccess<PagedResult<UserViewModel>> kế thừa từ thằng cha ApiResult
            }

            return JsonConvert.DeserializeObject<ApiResultError<UserViewModel>>(body);        //chuyển đồi cái body ra thành đối tượng thằng con ApiResultSuccess<PagedResult<UserViewModel>> kế thừa từ thằng cha ApiResult
        }

        public async Task<ApiResult<bool>> DeleteUser(Guid id)
        {
            //B1. Tạo 1 client
            //B2. Gán các thông số cho client (BaseAddress, Authorization)
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions); //Gán Token vào Thuộc tính Authorization của RequestHeader
            //Goi API
            var response = await client.DeleteAsync($"/api/users/{id}");
            var body = await response.Content.ReadAsStringAsync();                              //tách cái body của response ra
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<bool>>(body);        //chuyển đồi cái body ra thành đối tượng thằng con ApiResultSuccess<PagedResult<UserViewModel>> kế thừa từ thằng cha ApiResult
            }

            return JsonConvert.DeserializeObject<ApiResultError<bool>>(body);        //chuyển đồi cái body ra thành đối tượng thằng con ApiResultSuccess<PagedResult<UserViewModel>> kế thừa từ thằng cha ApiResult
        }

        public async Task<ApiResult<bool>> RoleAssignForUser(Guid id, RoleAssignRequest request)
        {
            //B1. Tạo 1 client
            //B2. Gán các thông số cho client (BaseAddress, Authorization)

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions); //Gán Token vào Thuộc tính Authorization của RequestHeader

            var json = JsonConvert.SerializeObject(request); //Chuyển request thành dạng json
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/users/{id}/roles", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<bool>>(result);
            }

            return JsonConvert.DeserializeObject<ApiResultError<bool>>(result);
        }
    }
}