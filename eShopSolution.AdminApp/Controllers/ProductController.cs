using eShopSolution.AdminApp.Sevices;
using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eShopSolution.AdminApp.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductApiClient _productApiClient;
        private readonly ICategoryApiClient _categoryApiClient;

        public ProductController(IProductApiClient productApiClient, ICategoryApiClient categoryApiClient)
        {
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
        }

        public async Task<IActionResult> Index(string keyword, int? categoryId, int pageIndex = 1, int pageSize = 5)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            //B2. Dựng request
            var request = new GetManageProductPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                LanguageId = languageId,
                CategoryId = categoryId
            };
            var data = await _productApiClient.GetPagings(request);   //Controller sử dụng Service của Project AdminApp để gửi request, lấy về PagedResult<UserViewModel> tạo View
            ViewBag.Keyword = keyword; //Chuyển keyword ra view để tái hiện lại trên textbox Search
            var categories = await _categoryApiClient.GetAll(languageId);
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = categoryId.HasValue && categoryId.Value == x.Id //keep Selected mỗi khi submit
            });
            if (TempData["result"] != null) //Nhận TempData["result"] được redirect từ các method Create, Update, Delete
            {
                ViewBag.Msg = TempData["result"];
            }
            if (data != null)
            {
                return View(data);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")] //// Phuong thuc nhan parameter multipart trong đó có trường dữ liệu là kiểu file
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _productApiClient.CreateProduct(request);
            if (result)
            {
                TempData["result"] = "Thêm mới sản phẩm " + request.Name + " thành công";
                return RedirectToAction("Index"); //Neu regist thanh cong thì Redirect
            }
            ModelState.AddModelError("", "Thêm sản phẩm thất bại");//show error msg tu API
            return View(request);
        }
    }
}