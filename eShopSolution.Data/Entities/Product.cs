using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace eShopSolution.Data.Entities
{
    //[Table("Products")] //Dùng Fluent API nên ko cần Data Annotation nữa
    public class Product
    {
        //Các thuộc tính của bảng Product
        public int Id { get; set; }

        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public int Stock { get; set; }
        public int ViewCount { get; set; }
        public DateTime DateCreated { get; set; }

        public bool? IsFeatured { get; set; }
        public string SeoAlias { get; set; }

        //Các mối quan hệ ràng buộc với bảng Product
        public List<ProductInCategory> ProductInCategories { get; set; } //1 Product thuoc nhieu Catergory (có nhiều dòng trong ProductInCatergories)

        public List<OrderDetail> OrderDetails { get; set; } //1 Product xuat hien nhieu lan trong bang OrderDetai

        public List<Cart> Carts { get; set; } //1 Product xuat hien nhieu lan trong bang Carts

        public List<ProductImage> ProductImages { get; set; } // Một Product => có nhiều ProductImage

        public List<ProductTranslation> ProductTranslations { get; set; } //1 Product co nhieu ProductTranslation ngon ngu vi, en, rus translate
    }
}