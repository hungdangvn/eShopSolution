﻿using eShopSolution.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Sales
{
    public class CheckoutRequest
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string  Email { get; set; }

        public string PhoneNumber { get; set; }

        public Guid UserId { get; set; }

        public List<OrderDetailViewModel> OrderDetailViewModel { get; set; } = new List<OrderDetailViewModel>();

    }
}
