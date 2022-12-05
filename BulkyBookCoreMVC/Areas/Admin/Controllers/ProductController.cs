using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Http;
using BulkyBookCoreMVC.Data;
using BulkyBookCoreMVC.Models;
using BulkyBookCoreMVC.Models.ViewModels;
using Microsoft.AspNetCore.Routing.Constraints;

namespace WebAppCore6.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;

        private IWebHostEnvironment _hostnvironment;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment hostenvironment)
        {
            _db = db;
            _hostnvironment = hostenvironment;
        }

        public IActionResult Index()
        {
            return View(GetProductList());
        }



        
        [HttpGet]
        public IActionResult Create()
        {

            
            ProductVM productVM = new ProductVM();
           
            productVM.CategoryList = _db.Categories?.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }).ToList();

            productVM.CoverTypeList = _db.CoverTypes?.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }).ToList();



            return View(productVM);



        }



        


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductVM objVM, IFormFile? file)
        {
            

            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, "Images", "Product");
                    var extention = Path.GetExtension(file.FileName);

                    var fullpath = Path.Combine(wwwRootPath, uploads, filename + extention);

                    using (var fileStreams = new FileStream(fullpath, FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }

                    objVM.ImageURL = @"/Images/Product/" + filename + extention;

                }

                var _product = (Product)objVM;


                try
                {
                    _db.Products?.Add(_product);
                    _db.SaveChanges();
                    TempData["success"] = "Product Created Successfully";
                    return RedirectToAction("Index");
                }
                catch { }
            }
            
            return View(objVM);
        }


        public IActionResult Edit(int? id)
        {

            if (id == null || id == 0)
            {
                return NotFound();
            }

            var productFromDB = _db.Products.FirstOrDefault(p=>p.Id==id);

            //var categoryFromDB = _db.Products.FirstOrDefault(c => c.Id==id);
            //
            if (productFromDB == null)
            {
                return NotFound();
            }

            //SetViewbagForCovertypeAndCategory();
            ProductVM productVM = new ProductVM();
            productVM.ISBN = productFromDB.ISBN;
            productVM.Author = productFromDB.Author;
            productVM.ImageURL = productFromDB.ImageURL;
            productVM.Id = productFromDB.Id;
            productVM.Title = productFromDB.Title;
            productVM.Description= productFromDB.Description;
            productVM.Price= productFromDB.Price;
            productVM.CategoryId = productFromDB.CategoryId;
            productVM.CoverTypeId = productFromDB.CoverTypeId;


            productVM.CategoryList = _db.Categories?.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }).ToList();

            productVM.CoverTypeList = _db.CoverTypes?.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value=i.Id.ToString()
            }).ToList() ;


            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product obj,IFormFile file)
        {
            



            Product? createObj = _db.Products?.FirstOrDefault(u => u.Id == obj.Id);

            

            string wwwRootPath = _hostnvironment.WebRootPath;
            if (file != null)
            {
                var prevFile = Path.Combine(wwwRootPath, "Images\\Product", Path.GetFileName(createObj.ImageURL));
                System.IO.File.Delete(prevFile);

                string filename = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, "Images", "Product");
                var extention = Path.GetExtension(file.FileName);

                var fullpath = Path.Combine(wwwRootPath, uploads, filename + extention);

                using (var fileStreams = new FileStream(fullpath, FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }

                createObj.ImageURL = @"/Images/Product/" + filename + extention;

            }



            createObj.Author = obj.Author;
            createObj.Description = obj.Description;
            createObj.Title = obj.Title;
            createObj.ISBN = obj.ISBN;
            createObj.CoverTypeId = obj.CoverTypeId;
            createObj.CategoryId = obj.CategoryId;
            createObj.Price = obj.Price;
            
            try
            {

                _db.Products?.Update(createObj);
                _db.SaveChanges();
                return RedirectToAction("Index");

            }
            catch(Exception)
            {
                
                return View(obj);

            }
            

            
        }


        public IActionResult Delete(int? id)
        {


            var categoryFromDB = _db.Products?.FirstOrDefault(p=>p.Id==id);

           
            if (categoryFromDB == null)
            {
                return NotFound();
            }

            return View(categoryFromDB);
        }

        public IActionResult DeletePOST(int? Id)
        {
            try
            {


                var obj = _db.Products?.FirstOrDefault(p => p.Id == Id);

              


                if (obj == null)
                {
                    return NotFound();
                }

                _db.Products?.Remove(obj);
                _db.SaveChanges();
                

                if (!string.IsNullOrEmpty(obj.ImageURL))
                {
                    string wwwRootPath = _hostnvironment.WebRootPath;
                    string uploadpath = Path.Combine(wwwRootPath, "Images\\Product", Path.GetFileName(obj.ImageURL));

                    System.IO.File.Delete(uploadpath);
                }

                TempData["success"] = "Product Deleted Successfully";

                return RedirectToAction("Index");
            }
            catch(Exception)
            {
                TempData["error"] = "An Error occured while deleting";

                return RedirectToAction("Delete", new { id = Id });
            }

            
        }

        //API Calls
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var productsList = GetProductList();
            return Json(new { data = productsList });
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

//string fileName = Path.GetFileName(file.FileName);

//string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\Product", fileName);

//var stream = new FileStream(uploadpath, FileMode.Create);

//file.CopyToAsync(stream);

//ViewBag.Message = "File uploaded successfully.";

//// ViewBag.ImageURL = Path.Combine("Product","Images", fileName);


//objVM.Product.ImageURL = @"/Images/Product/" + fileName;

//public void SetViewbagForCovertypeAndCategory()
//{
//    var CategoryList = (from obj in _db.Categories
//                        select new SelectListItem
//                        { Value = obj.Id.ToString(), Text = obj.Name })
//                        .ToList<SelectListItem>();
//   // CategoryList.Insert(0, new SelectListItem() { Value = "0", Text = "--Select--" });
//    ViewBag.CategoryList = CategoryList;



//    var CoverTypeList = (from obj in _db.CoverTypes
//                         select new SelectListItem
//                         { Value = obj.Id.ToString(), Text = obj.Name })
//                   .ToList<SelectListItem>();

//    //CoverTypeList.Insert(0, new SelectListItem() { Value = "0", Text = "--Select--" });

//    ViewBag.CoverTypeList = CoverTypeList;
//}