using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace eShopSolution.ViewModels.Catalog.Products
{
    public class ProductCreateRequest
    {
        [Display(Name = "Giá bán")]
        public decimal Price { get; set; }

        [Display(Name = "Giá nhập")]
        public decimal OriginalPrice { get; set; }

        public int Stock { get; set; }

        [Display(Name = "Tên sản phẩm")]
        public string Name { set; get; }

        [Display(Name = "Mô tả sản phẩm")]
        public string Description { set; get; }

        [Display(Name = "Mô tả chi tiết")]
        public string Details { set; get; }

        public string SeoDescription { set; get; }
        public string SeoTitle { set; get; }

        public string SeoAlias { get; set; }
        public string LanguageId { set; get; }

        public IFormFile ThumbnailImage { get; set; }
    }
}