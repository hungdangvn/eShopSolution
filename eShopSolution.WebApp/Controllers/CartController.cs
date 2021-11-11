using eShopSolution.ApiIntergration;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Sales;
using eShopSolution.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace eShopSolution.WebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IOrderApiClient _orderApiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public CartController(IProductApiClient productApiClient            
            , IOrderApiClient orderApiClient
            , IHttpContextAccessor httpContextAccessor
            )
        {
            _productApiClient = productApiClient;           
            _orderApiClient = orderApiClient;
            _httpContextAccessor = httpContextAccessor;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Checkout()
        {
            //ViewBag["Success"] = null;
            return View(GetCheckOutViewModel());
        
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutViewModel request)
        {
            string userId = "";
            var model = GetCheckOutViewModel();
            request.CartItems = model.CartItems;

            var orderDetails = new List<OrderDetailViewModel>();            

            var checkoutRequest = new CheckoutRequest()
            {
                Name = request.CheckoutRequest.Name,
                Address = request.CheckoutRequest.Address,
                Email = request.CheckoutRequest.Email,
                PhoneNumber = request.CheckoutRequest.PhoneNumber,
                OrderDetailViewModel = orderDetails                
        };

            if (User.Identity.IsAuthenticated)
            {
                userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                checkoutRequest.UserId = Guid.Parse(userId);
            }
            else
            {
                TempData["LoginRequired"] = "Please login before check out!";
                
                return View(request);
            }

            foreach (var item in model.CartItems)
            {
                orderDetails.Add(new OrderDetailViewModel()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }
            
            
            //TODO: send request to API
            var result = await _orderApiClient.CreateOrder(checkoutRequest);
            if (result)
            {
                TempData["Success"] = "Order purchase successful!";
            }

            return View(request);

        }

        public async Task<IActionResult> AddToCart(int id, string languageId)
        {
            var product = await _productApiClient.GetById(id, languageId);
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            var currentCart = new List<CartItemViewModel>();
            if (session != null)
            {
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            }
            int quantity = 1;
            if (currentCart.Any(x => x.ProductId == id))
            {
                currentCart.First(x => x.ProductId == id).Quantity = currentCart.First(x => x.ProductId == id).Quantity + quantity;
            }
            else
            {
                var cartItem = new CartItemViewModel()
                {
                    ProductId = id,
                    Description = product.Description,
                    Image = product.ThumbnailImage,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = quantity
                };
                currentCart.Add(cartItem);
            }

            HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));
            return Ok(currentCart);
        }

        public IActionResult UpdateCart(int id, int quantity)
        {            
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            var currentCart = new List<CartItemViewModel>();
            if (session != null)
            {
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            }

            foreach (var item in currentCart)
            {
                if (item.ProductId == id)
                {
                    if (quantity == 0)
                    {
                        currentCart.Remove(item);
                        break;
                    }
                    else
                    {
                        item.Quantity = quantity;
                    }
                    
                }
            }            

            HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));
            return Ok(currentCart);
        }

        [HttpGet]
        public IActionResult GetListItems()
        {            
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            var currentCart = new List<CartItemViewModel>();
            if (session != null)
            {
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            }           

            return Ok(currentCart);
        }

        private CheckoutViewModel GetCheckOutViewModel() 
        {

            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            var checkoutViewModel = new CheckoutViewModel();

            if (session != null)
            {
                checkoutViewModel.CartItems = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
                checkoutViewModel.CheckoutRequest = new CheckoutRequest();
            }
            return checkoutViewModel;
        }     


    }
}
