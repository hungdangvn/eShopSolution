using eShopSolution.ViewModels.Utilities.Slides;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntergration
{
    public class SlideApiClient : BaseApiClient, ISlideApiClient
    {
        //private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly IHttpClientFactory _httpClientFactory;
        //private readonly IConfiguration _configuration;

        public SlideApiClient(IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
            //_httpContextAccessor = httpContextAccessor;
            //_httpClientFactory = httpClientFactory;
            //_configuration = configuration;
        }

        public async Task<List<SlideViewModel>> GetAll()
        {
            //api / Categories ? languageId = vi - VN
            var data = await GetListAsync<SlideViewModel>("/api/slides");
            return data;
        }
    }
}