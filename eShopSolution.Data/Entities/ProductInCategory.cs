using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class ProductInCategory
    {
        public int ProductId { get; set; }//Khoa ngoại, cũng là khóa chính

        public Product Product { get; set; } //1 ProductInCatergory có cha là 1 Product (trỏ đến 1 Product)

        public int CategoryId { get; set; }

        public Category Category { get; set; } //1 ProductInCategory có cha là 1 Catergory (trỏ đến 1 Catergory)
    }
}
