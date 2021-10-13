using eShopSolution.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class Category
    {
        public int Id { set; get; }
        public int SortOrder { set; get; }
        public bool IsShowOnHome { set; get; }
        public int? ParentId { set; get; }
        public Status Status { set; get; }

        public List<ProductInCategory> ProductInCategories { get; set; } //1 Catergory co nhieu Product (có nhiều dòng trong ProductInCatergories). QH 1-n

        public List<CategoryTranslation> CategoryTranslations { get; set; }

    }
}
