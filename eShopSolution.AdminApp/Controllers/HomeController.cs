using eShopSolution.AdminApp.Models;
using eShopSolution.Utilities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.AdminApp.Controllers
{
    //[Authorize] //Cu vao trang chu la yeu cau phai xac thuc dang nhap login, ko duoc phep vao khi chua dang nhap; ko cần nữa do kế thừa từ BaseController
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            var user = User.Identity.Name;
            return View();
        }

        /***
         * ViewComponent Navigator/Default post về server, kèm theo NavigationViewModel có giá trị CurrentLanguageId
         *
         *
         */

        [HttpPost]
        public IActionResult Language(NavigationViewModel navigationViewModel)
        {
            HttpContext.Session.SetString(SystemConstants.AppSettings.DefaultLanguageId, navigationViewModel.CurrentLanguageId); //Thay đổi DefaultLanguageId trong Session theo giá trị navigationViewModel.CurrentLanguageI
            return RedirectToAction("Index", "User");
        }
    }
}