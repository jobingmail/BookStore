using BulkyBookCoreMVC.Data;
using BulkyBookCoreMVC.Models;
using BulkyBookCoreMVC.Models.ViewModels;
using BulkyBookCoreMVC.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyBookCoreMVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public ShoppingCartVM _ShoppingCartVM { get; set; }
        public CartController(ApplicationDbContext db)
        {
                _db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {

             _ShoppingCartVM = GetShoppingCartVM();

            return View(_ShoppingCartVM);
        }

        

        public ShoppingCartVM GetShoppingCartVM()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (_ShoppingCartVM == null)
            {
                _ShoppingCartVM = new ShoppingCartVM();
            }

            if (_ShoppingCartVM.ListCart == null)
            {
                _ShoppingCartVM.ListCart = _db.ShoppingCarts.Where(u => u.ApplicationUserId == claim.Value).ToList();

            }
            foreach (var cart in _ShoppingCartVM.ListCart)
            {
                cart.Product = _db.Products.FirstOrDefault(u => u.Id == cart.ProductId);
                cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price);

                _ShoppingCartVM.CartTotal += (cart.Price * cart.Count);
            }

            return _ShoppingCartVM;
        }
        
        public IActionResult plus(int cartId)
        {
            var tmp = GetShoppingCartVM().ListCart;

            var cart = tmp.FirstOrDefault(u=>u.Id== cartId);
            cart.Count++;

            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult minus(int cartId)
        {
            var tmp = GetShoppingCartVM().ListCart;
            var cart = tmp.FirstOrDefault(u => u.Id == cartId);
            cart.Count--;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Remove(int cartId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var listCartByUser = GetShoppingCartVM().ListCart.Where(u => u.ApplicationUserId == claim.Value).ToList();

            ShoppingCart product = listCartByUser.FirstOrDefault(u => u.Id == cartId);

            _db.ShoppingCarts.Remove(product);
            _db.SaveChanges();

            return RedirectToAction("Index");


        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            _ShoppingCartVM = GetShoppingCartVM();


            //_ShoppingCartVM.ListCart = _db.ShoppingCarts.Where(u => u.ApplicationUserId == claim.Value).ToList();
            _ShoppingCartVM.OrderHeader = new();
            _ShoppingCartVM.OrderHeader.ApplicationUser = _db.ApplicationUsers.FirstOrDefault(u => u.Id == claim.Value);

            var applicationUser = _ShoppingCartVM.OrderHeader.ApplicationUser;

            _ShoppingCartVM.OrderHeader.Name = applicationUser.Name;
            _ShoppingCartVM.OrderHeader.City = applicationUser.City;
            _ShoppingCartVM.OrderHeader.PhoneNumber = applicationUser.PhoneNumber;
            _ShoppingCartVM.OrderHeader.StreetAddress = applicationUser.StreetAddress;
            _ShoppingCartVM.OrderHeader.State = applicationUser.State;
            _ShoppingCartVM.OrderHeader.PostalCode = applicationUser.PostalCode;

            foreach (var cart in _ShoppingCartVM.ListCart)
            {
                cart.Product = _db.Products.FirstOrDefault(u => u.Id == cart.ProductId);
                cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price);

                _ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            return View(_ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            
            _ShoppingCartVM.ListCart = _db.ShoppingCarts.Where(c => c.ApplicationUserId == claim.Value).ToList();

            _ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            _ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            _ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            _ShoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;


            foreach (var cart in _ShoppingCartVM.ListCart)
            {
                cart.Product = _db.Products.FirstOrDefault(u => u.Id == cart.ProductId);
                cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price);

                _ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            _db.OrderHeaders.Add(_ShoppingCartVM.OrderHeader);
            _db.SaveChanges();


            foreach (var cart in _ShoppingCartVM.ListCart)
            {

                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderId = _ShoppingCartVM.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count
                };

                _db.OrderDetails.Add(orderDetail);
                _db.SaveChanges();


            }

            _db.ShoppingCarts.RemoveRange(_ShoppingCartVM.ListCart);
            _db.SaveChanges();



            return RedirectToAction("Index","Home");
        }

        private double GetPriceBasedOnQuantity(double quantity, double price)//,double price50,double price100)
        {


            //if (quantity <= 50)
            //{

            //    return price;
            //}
            //else
            //{
            //    if (quantity <= 100)
            //    {
            //        return price50;
            //    }
            //    else
            //    {
            //        return price100;
            //    }
            //}

            return price;


        }
    }
}



