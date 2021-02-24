using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductsCategories.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ProductsCategories.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context { get; set; }

        public HomeController(MyContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            List<Product> AllProducts = _context.Products.ToList();
            ViewBag.allproducts = AllProducts;
            return View();
        }

        [HttpPost("/")]
        public IActionResult CreateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                _context.SaveChanges();
                return RedirectToAction("CreateProduct");
            }
            return View("Index");
        }

        [HttpGet("/Category")]
        public IActionResult Category()
        {
            List<Category> AllCategories = _context.Categories.ToList();
            ViewBag.allcategories = AllCategories;
            return View();
        }

        [HttpPost("/Category")]
        public IActionResult CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                _context.SaveChanges();
                return RedirectToAction("CreateCategory");
            }
            return View("Category");
        }

        [HttpGet("/Product/{productId}")]
        public IActionResult ShowProduct(int productId)
        {
            // to get the Product 
            ViewBag.this_product = _context.Products.FirstOrDefault(p => p.ProductId == productId);
            // To show all unadded categories in the dropdown list
            ViewBag.Unassociated = _context.Categories.Include(c => c.Products).Where(c => c.Products.All(i => i.ProductId != productId)).ToList();
            // to pass selected category to the product category 
            Product todropdown = _context.Products.Include(p => p.Categories).ThenInclude(c => c.CategoryofProduct).FirstOrDefault(p => p.ProductId == productId);
            return View(todropdown);
        }

        [HttpPost("AddProdAsso/{prodid}")]
        public IActionResult AddProdAssociation(int prodid, int catid)
        {
            Association Asso = new Association();
            Asso.ProductId = prodid;
            Asso.CategoryId = catid;
            _context.Associations.Add(Asso);
            _context.SaveChanges();
            return Redirect($"/Product/{prodid}");
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


    }
}