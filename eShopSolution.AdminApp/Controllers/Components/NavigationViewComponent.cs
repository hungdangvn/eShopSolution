using eShopSolution.AdminApp.Models;
using eShopSolution.AdminApp.Sevices;
using eShopSolution.Utilities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Controllers.Components
{
    public class NavigationViewComponent : ViewComponent //Đặt tên Component theo cú pháp XxxViewComponent kế thừa từ ViewComponent y hệt Controller
    {
        private readonly ILanguageApiClient _languageApiClient;

        public NavigationViewComponent(ILanguageApiClient languageApiClient)
        {
            _languageApiClient = languageApiClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var languages = await _languageApiClient.GetAll();
            var navigationViewModel = new NavigationViewModel()
            {
                CurrentLanguageId = HttpContext.Session
                .GetString(SystemConstants.AppSettings.DefaultLanguageId), //Lấy DefaultLanguageId trong Session ra
                Languages = languages.ResultObj
            };
            return View("Default", navigationViewModel); //tra ve view partial "Default" trong Shared/Components/Navigation
        }
    }
}