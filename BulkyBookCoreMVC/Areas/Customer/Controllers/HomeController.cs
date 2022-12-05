using BulkyBookCoreMVC.Data;
using BulkyBookCoreMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyBookCoreMVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;

            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = GetProductList();

            return View(productList);
        }

        public IActionResult Details(int Id)
        {
            ShoppingCart cartObj = new ShoppingCart()
            {
                ProductId = Id,
               Product = GetProductList().FirstOrDefault(u => u.Id == Id),
               Count = 1
             };

            return View(cartObj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claim.Value;
            shoppingCart.Id = 0;

            var shoppingCartFromDB = _db.ShoppingCarts.FirstOrDefault(u => u.ApplicationUserId == claim.Value
            && u.ProductId == shoppingCart.ProductId
            );

            if (shoppingCartFromDB != null)
            {
                shoppingCartFromDB.Count += shoppingCart.Count;
                _db.SaveChanges();
            }
            else
            {

                _db.ShoppingCarts.Add(shoppingCart);
                _db.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }





        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public List<Product> GetProductList()
        {
            List<Product> query = new List<Product>();
            try
            {
                IEnumerable<Product> objProductList = _db.Products.ToList();

                IEnumerable<Category> lstCategory = _db.Categories.ToList();

                IEnumerable<CoverType> lstCoverType = _db.CoverTypes.ToList();




                if (objProductList != null && lstCategory != null && lstCoverType != null)
                {


                    query = (from prod in objProductList
                             join catg in lstCategory
                             on prod.CategoryId equals catg.Id
                             join covr in lstCoverType
                             on prod.CoverTypeId equals covr.Id
                             select new Product
                             {
                                 Id = prod.Id,
                                 Title = prod.Title,
                                 Description = prod.Description,
                                 ISBN = prod.ISBN,
                                 Author = prod.Author,
                                 Price = prod.Price,
                                 ImageURL = prod.ImageURL,
                                 CategoryName = catg.Name,
                                 CoverTypeName = covr.Name

                             }).ToList();

                    return query;
                }

            }
            catch (Exception) { };



            return query;
        }
    }
}