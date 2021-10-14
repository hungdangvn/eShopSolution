using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntergration
{
    public class CategoryApiClient : BaseApiClient, ICategoryApiClient
    {
        //private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly IHttpClientFactory _httpClientFactory;
        //private readonly IConfiguration _configuration;

        public CategoryApiClient(IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
            //_httpContextAccessor = httpContextAccessor;
            //_httpClientFactory = httpClientFactory;
            //_configuration = configuration;
        }

        public async Task<List<CategoryViewModel>> GetAll(string languageId)
        {
            //api / Categories ? languageId = vi - VN
            var data = await GetListAsync<CategoryViewModel>("/api/categories?languageId=" + languageId);
            return data;
        }
    }
}