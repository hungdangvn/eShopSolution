using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.Products
{
    public class ProductDeleteRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}