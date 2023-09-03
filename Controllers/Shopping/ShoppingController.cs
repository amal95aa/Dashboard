using Dashboard.Data;
using Dashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using System.Security.Cryptography;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Dashboard.Controllers.Shopping
{
    public class ShoppingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShoppingController(ApplicationDbContext context)
        {
            _context = context;
        }



        [Authorize]
        public IActionResult Checkout(int id)
        {
            var user = HttpContext.User.Identity.Name;
            var productDetails = _context.ProductDetails.SingleOrDefault(p => p.Id == id);
            var cart = new Cart()
            {
                CustomerId = user,
                MyProductId = productDetails.ProductId,
                ProductName = productDetails.ProductName,
                Flavor = productDetails.Flavor,
                Image = productDetails.Image,
                Price = productDetails.Price,
                Qty = productDetails.Qty,
                Tax = 0.15,
                Discount = 15,
                ShippingCharge = 20,
                Total = productDetails.Price * (15 / 100) + productDetails.Price - 15 + 20,

            };

            _context.Carts.Add(cart);
            _context.SaveChanges();



            return View(cart);
        }



        public async Task<IActionResult> Invoice(int id)
        {
            var user = HttpContext.User.Identity.Name;
            var cart = _context.Carts.SingleOrDefault(x => x.Id == id);
            var invoice = new Invoice()
            {
                CustomerId = user,
                ProductId = cart.MyProductId,
                ProductName = cart.ProductName,
                Price = cart.Price,
                Tax = cart.Tax,
                Discount = cart.Discount,
                Qty = cart.Qty,
                Total = cart.Total,

            };

            _context.Invoices.Add(invoice);
            _context.SaveChanges();
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("SweetCake@example.com", "tuwiqam@gmail.com"));
            message.To.Add(MailboxAddress.Parse(user));
            message.Subject = "SweetCake ";
            message.Body = new TextPart("plain")
            {
                Text = "Purchased from a store Sweet Cake"
            };
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect("smtp.gmail.com", 587);
                    client.Authenticate("tuwiqam@gmail.com", "zywimnjblpwavtvu");
                    await client.SendAsync(message);
                    client.Disconnect(true);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message.ToString());
                }



            };

            return View(invoice);

        }


        public IActionResult ProductDetails(int id)
        {
            var ProductDetails = _context.ProductDetails.SingleOrDefault(p => p.Id == id);
            return View(ProductDetails);
        }

        public IActionResult Index()
        {
            var product = _context.Products.ToList();
            var ProductDetails = _context.ProductDetails.ToList();
            ViewBag.ProductDetails = ProductDetails;
            return View(product);
        }
    }
}