using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.ViewModels.System.Users
{
    public class UserDeleteRequest
    {
        public Guid Id { get; set; }

        [Display(Name = "Tài khoản")]
        public string UserName { get; set; }
    }
}