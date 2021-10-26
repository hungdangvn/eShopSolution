using eShopSolution.ApiIntergration;
using eShopSolution.WebApp.Models;
using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using static eShopSolution.Utilities.Constants.SystemConstants;

namespace eShopSolution.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISharedCultureLocalizer _loc;
        private readonly ISlideApiClient _slideApiClient;
        private readonly IProductApiClient _productApiClient;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";

        public HomeController(ILogger<HomeController> logger
            , ISharedCultureLocalizer loc
            , ISlideApiClient slideApiClient
            , IProductApiClient productApiClient)
        {
            _logger = logger;
            _loc = loc;
            _slideApiClient = slideApiClient;
            _productApiClient = productApiClient;
        }

        public async Task<IActionResult> Index()
        {
            //var msg = _loc.GetLocalizedString("Vietnamese");
            var culture = CultureInfo.CurrentCulture.Name;
            var slides = await _slideApiClient.GetAll();
            var featuredProducts = await _productApiClient.GetFeaturedProducts(culture, ProductSetting.NumberOfFeaturedProducts);
            var lastestProducts = await _productApiClient.GetLastestProducts(culture, ProductSetting.NumberOfLastestProducts);
            var viewModel = new HomeViewModel()
            {
                Slides = slides,
                FeaturedProducts = featuredProducts,
                LastestProducts = lastestProducts
            };
            //var _userContentFolder = Path.Combine(IWebHostEnvironment.WebRootPath, USER_CONTENT_FOLDER_NAME);
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult SetCultureCookie(string cltr, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cltr)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

            return LocalRedirect(returnUrl);
        }
    }
}