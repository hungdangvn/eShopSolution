using eShopSolution.Application.Catalog.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService; //inject producService vao de su dụng các dịch vụ
        }

        [HttpGet]     //Nếu các method trùng tên nên đặt các alias để phân biệt
        public async Task<IActionResult> GetAll([FromQuery] string languageId) //Ham GET nen thuộc tính [FromQuery] chi dinh lay tham so tu URL
        {
            var categories = await _categoryService.GetAll(languageId);
            return Ok(categories);
        }
    }
}