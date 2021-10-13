using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Common;
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
    public class BaseApiClient
    {
        public readonly IHttpClientFactory _httpClientFactory; //An IHttpClientFactory can be requested using dependency injection (DI)
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        protected BaseApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        protected async Task<TRespose> GetAsync<TRespose>(string url)
        {
            //B1. Tạo 1 client
            //B2. Gán các thông số cho client (BaseAddress, Authorization)

            var sessions = _httpContextAccessor.HttpContext.Session
                .GetString(SystemConstants.AppSettings.Token);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions); //Gán Token vào Thuộc tính Authorization của RequestHeader

            var response = await client.GetAsync(url);               //gửi request đến API với tham số là URI và các tham số kèm theo, nhận lại response từ Server

            var body = await response.Content.ReadAsStringAsync();                              //tách cái body của response ra
            if (response.IsSuccessStatusCode)
            {
                TRespose myDeserializationObjList = (TRespose)JsonConvert.DeserializeObject(body, typeof(TRespose));
                return myDeserializationObjList; //chuyển đồi cái body ra thành đối tượng thằng con ApiResultSuccess<PagedResult<UserViewModel>> kế thừa từ thằng cha ApiResult
            }

            return JsonConvert.DeserializeObject<TRespose>(body);
        }
    }
}