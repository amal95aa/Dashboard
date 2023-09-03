using Dashboard.Data;
using Dashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Dashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        public IActionResult AddProduct(Product product)
        {

            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }





        public IActionResult AddProductDetails(ProductDetails productDetails)
        {

            _context.ProductDetails.Add(productDetails);
            _context.SaveChanges();
            return RedirectToAction("ProductDetails");
        }





        public IActionResult Edit(int id)
        {
            ViewBag.Name = Request.Cookies["Name"];
            var product = _context.Products.SingleOrDefault(p => p.Id == id);
            return View(product);
        }




        public IActionResult EditProductDetails(int id)
        {
            ViewBag.Name = Request.Cookies["Name"];
            var productDetails = _context.ProductDetails.SingleOrDefault(p => p.ProductId == id);
            return View(productDetails);
        }




        public IActionResult UpdateProducts(Product product)
        {

            _context.Products.Update(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }




        public IActionResult UpdateProductsDetails(ProductDetails productDetails)
        {

            _context.ProductDetails.Update(productDetails);
            _context.SaveChanges();
            return RedirectToAction("ProductDetails");
        }




        public IActionResult Delete(int id)
        {
            var product = _context.Products.SingleOrDefault(p => p.Id == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        public IActionResult DeleteProductsDetails(int id)
        {
            var productDetails = _context.ProductDetails.SingleOrDefault(p => p.Id == id);
            if (productDetails != null)
            {
                _context.ProductDetails.Remove(productDetails);
                _context.SaveChanges();
            }
            return RedirectToAction("ProductDetails");
        }

        [Authorize]
        public IActionResult Index()
        {
            var Name = HttpContext.User.Identity.Name;
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddMinutes(10);
            Response.Cookies.Append("Name", Name, options);
            //HttpContext.Session.SetString("Name", Name);
            ////TempData["Name"] = Name;
            ViewBag.Name = Name;
            var product = _context.Products.ToList();
            return View(product);
        }



        [HttpPost]
        public IActionResult Index(string ProductName)
        {

            var products = _context.Products.Where(x => x.ProductName.Contains(ProductName)).ToList();
            ViewBag.s = products;
            return View(products);
        }



        [HttpPost]
        public IActionResult ProductDetails(int id)
        {
            var ProductDetails = _context.ProductDetails.Where(p => p.ProductId == id).ToList();
            var product = _context.Products.ToList();
            ViewBag.ProductDetails = ProductDetails;
            return View(product);
        }



        public IActionResult ProductDetails()
        {
            ViewBag.Name = Request.Cookies["Name"];
            //ViewBag.Name = HttpContext.Session.GetString("Name");
            ////ViewBag.Name = TempData["Name"];

            var product = _context.Products.ToList();
            var ProductDetails = _context.ProductDetails.ToList();
            ViewBag.ProductDetails = ProductDetails;
            return View(product);
        }



        [HttpPost]
        public IActionResult InvoiceList(string ProductName)
        {

            var invoiceList = _context.Invoices.Where(x => x.ProductName.Contains(ProductName)).ToList();
            ViewBag.s = invoiceList;
            return View(invoiceList);
        }

        public IActionResult InvoiceList()
        {
            ViewBag.Name = Request.Cookies["Name"];
            //ViewBag.Name = HttpContext.Session.GetString("Name");
            ////ViewBag.Name = TempData["Name"];

            var invoiceList = _context.Invoices.ToList();
            ViewBag.invoiceList = invoiceList;
            return View(invoiceList);
        }

        public IActionResult PaymentAccept()
        {
            ViewBag.Name = Request.Cookies["Name"];
            return View();
        }


        [HttpPost]
        public IActionResult PaymentAccept(PaymentAccept paymentAccept)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }


            return View();
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