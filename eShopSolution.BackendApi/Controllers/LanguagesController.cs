using eShopSolution.Application.System.Languages;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguagesController : Controller
    {
        private readonly ILanguageService _languageService;

        public LanguagesController(ILanguageService languageService)
        {
            _languageService = languageService; //inject languageService vao de su dụng các dịch vụ
        }

        [HttpGet()]     //Nếu các method trùng tên nên đặt các alias để phân biệt
        public async Task<IActionResult> GetAll() //Ham GET nen thuộc tính [FromQuery] chi dinh lay tham so tu URL
        {
            var languages = await _languageService.GetAll();
            return Ok(languages);
        }
    }
}