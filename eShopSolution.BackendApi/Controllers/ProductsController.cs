using eShopSolution.Application.Catalog.Products;
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.BackendApi.Controllers
{
    //api/products
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService producService)
        {
            _productService = producService; //inject producService vao de su dụng các dịch vụ
        }

        [HttpGet("{paging}")]     //Nếu các method trùng tên nên đặt các alias để phân biệt
        public async Task<IActionResult> GetAllPaging([FromQuery] GetManageProductPagingRequest request) //Ham GET nen thuộc tính [FromQuery] chi dinh lay tham so tu URL
        {
            var products = await _productService.GetAllPaging(request);
            return Ok(products);
        }

        //https://localhost:5001/api/Products/1/vi
        [HttpGet("{productId}/{languageId}")] //Nếu các method trùng tên nên đặt các alias để phân biệt
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var product = await _productService.GetById(productId, languageId);
            if (product == null) return BadRequest("Cannot find product");
            return Ok(product);
        }

        [HttpGet("featured/{languageId}/{take}")] //Nếu các method trùng tên nên đặt các alias để phân biệt
        [AllowAnonymous]
        public async Task<IActionResult> GetFeaturedProducts(string languageId, int take)
        {
            var products = await _productService.GetFeaturedProducts(languageId, take);
            if (products == null) return BadRequest("Cannot find product");
            return Ok(products);
        }

        [HttpGet("lastest/{languageId}/{take}")] //Nếu các method trùng tên nên đặt các alias để phân biệt
        [AllowAnonymous]
        public async Task<IActionResult> GetLastestProducts(string languageId, int take)
        {
            var products = await _productService.GetLastestProducts(languageId, take);
            if (products == null) return BadRequest("Cannot find product");
            return Ok(products);
        }

        [HttpPost]
        [Consumes("multipart/form-data")] // Phuong thuc nhan parameter multipart trong đó có trường dữ liệu là kiểu file
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request) // Sử dụng thuộc tính [FromForm] để lấy được parameters từ trền form của Swagger
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var productId = await _productService.Create(request);
            if (productId == 0) return BadRequest();

            var product = await _productService.GetById(productId, request.LanguageId);

            return CreatedAtAction(nameof(GetById), new { id = productId }, product);
        }

        [HttpPut("{productId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromRoute] int productId, [FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            request.Id = productId;
            var affectedResult = await _productService.Update(request);
            if (affectedResult == 0) return BadRequest();

            return Ok();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var affectedResult = await _productService.Delete(productId);
            if (affectedResult == 0) return BadRequest();

            return Ok();
        }

        //https://localhost:5001/api/Products/1/300
        [HttpPatch("{productId}/{newPrice}")] //Update 1 phần của bản ghi dùng HttpPatch
        public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
        {
            var isSuccessful = await _productService.UpdatePrice(productId, newPrice);
            if (isSuccessful) return Ok();

            return BadRequest();
        }

        //Images
        [HttpPost("{productId}/images")]
        public async Task<IActionResult> CreateImage(int productId, [FromForm] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var imageId = await _productService.AddImage(productId, request);
            if (imageId == 0) return BadRequest();

            var image = await _productService.GetImageById(imageId);

            return CreatedAtAction(nameof(GetImageById), new { id = imageId }, image);
        }

        [HttpPut("{productId}/images/{imageId}")]
        public async Task<IActionResult> UpdateImage(int productId, int imageId, [FromForm] ProductImageUpdateRequest request) //_productService.UpdateImage ko cần productId nhưng vẫn để vào để theo chuẩn RestFull the hiện quan hệ cha - con
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var affectedResult = await _productService.UpdateImage(imageId, request);
            if (affectedResult == 0) return BadRequest();

            return Ok();
        }

        [HttpDelete("{productId}/images/{imageId}")]
        public async Task<IActionResult> RemoveImage(int productId, int imageId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var affectedResult = await _productService.RemoveImage(imageId);
            if (affectedResult == 0) return BadRequest();

            return Ok();
        }

        [HttpGet("{productId}/images/{imageId}")]
        public async Task<IActionResult> GetImageById(int productId, int imageId)
        {
            var image = await _productService.GetImageById(imageId);
            if (image == null) return BadRequest("Cannot find product");
            return Ok(image);
        }

        [HttpPut("{id}/categories")]        //Lay id tren URL
        public async Task<IActionResult> CategoryAssign(int id, [FromBody] CategoryAssignRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productService.CategoryAssisgn(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result); //Tra ve doi tuong de phia client xem xet
        }
    }
}