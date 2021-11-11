using eShopSolution.Application.Sales;
using eShopSolution.ViewModels.Sales;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService; //inject producService vao de su dụng các dịch vụ
        }

        [HttpPost()]        
        public async Task<IActionResult> Create([FromBody] CheckoutRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _orderService.Create(request);
            if (result != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
