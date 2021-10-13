using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class OrderDetail
    {

        public int OrderId { set; get; }
        public int ProductId { set; get; }
        public int Quantity { set; get; }
        public decimal Price { set; get; }

        public Order Order { get; set; }    //1 OrderDetail có cha là 1 Order (trỏ đến 1 Order)

        public Product Product { get; set; } //1 OrderDetail có cha là 1 Product (trỏ đến 1 Order)

    }
}
