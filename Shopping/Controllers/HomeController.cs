using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shopping.Models;
using Shopping.Data;
using Shopping.Entities;
using Shopping.ViewModels;

namespace Shopping.Controllers
{
    public class HomeController : Controller
    {
        private ShopDbContext _db;
        public HomeController(ShopDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var model = _db.Products.Select(product =>
            new ProductViewModel
            {
                Id = product.Id,
                ImageSrc = product.ImageSrc,
                Title = product.Title,
                Price = product.Price.ToString("N0") + "원"
            });
            return View(model);
        }

        public IActionResult AddToCart(int id)
        {
            var selectedProduct = _db.Products.Where(product => product.Id == id).SingleOrDefault();
            var cartItem = _db.CartItems.Where(i => i.ProductId == id).SingleOrDefault();
            if (cartItem == null)
            {
                _db.CartItems.Add(new CartItem
                {
                    Quantity = 1,
                    SubTotal = selectedProduct.Price,
                    ProductId = id
                });
            }
            else
            {
                cartItem.Quantity++;
                cartItem.SubTotal += selectedProduct.Price;
            }
            _db.SaveChanges();
            return RedirectToAction("Cart");
        }

        public IActionResult Cart()
        {
            var model = _db.CartItems.Select(cItem =>
            new CartViewModel
            {
                Id = cItem.Id,
                ImageSrc = _db.Products.Where(product => product.Id == cItem.ProductId).SingleOrDefault().ImageSrc,
                ProductId = cItem.Id,
                Title = _db.Products.Where(product => product.Id == cItem.ProductId).SingleOrDefault().Title,
                Price = _db.Products.Where(product => product.Id == cItem.ProductId).SingleOrDefault().Price.ToString("N0") + "원",
                Quantity = cItem.Quantity.ToString() + "개",
                SubTotal = (_db.Products.Where(product => product.Id == cItem.ProductId).SingleOrDefault().Price * cItem.Quantity).ToString("N0") + "원"
            });
            return View(model);
        }

        public IActionResult DeleteCartItem(int id)
        {
            var cartItem = _db.CartItems.Where(cItem => cItem.Id == id).SingleOrDefault();
            _db.CartItems.Remove(cartItem);
            _db.SaveChanges();
            return RedirectToAction("Cart");
        }

        public IActionResult Details(int id)
        {
            Product selectedProduct = _db.Products.Where(product => product.Id == id).SingleOrDefault();
            var model = new ProductViewModel
            {
                Id = selectedProduct.Id,
                ImageSrc = selectedProduct.ImageSrc,
                Title = selectedProduct.Title,
                Price = selectedProduct.Price.ToString("N0") + "원"
            };
            return View(model);
        }

        public IActionResult BuyNow(int id)
        {
            _db.OrdersItems.Add(new OrdersItem
            {
                Date = DateTime.Now,
                ProductId = id
            });
            _db.SaveChanges();
            var model = _db.OrdersItems.ToList();
            return View("Orders", model);
        }

        public IActionResult ProceedToCheckout()
        {
            var cartItems = _db.CartItems.ToList();
            foreach (var item in cartItems)
            {
                _db.OrdersItems.Add(new OrdersItem
                {
                    Date = DateTime.Now,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    SubTotal = item.SubTotal
                });
            }
            _db.SaveChanges();
            return RedirectToAction("Orders");
        }

        public IActionResult Orders()
        {
            var model = _db.OrdersItems
                .OrderByDescending(oItem => oItem.Date)
                .Select(oItem =>
            new OrdersViewModel
            {
                ProductId = oItem.ProductId,

                Date = oItem.Date,

                ImageSrc = _db.Products.Where(product => product.Id == oItem.ProductId)
                .SingleOrDefault().ImageSrc,

                Title = _db.Products.Where(product => product.Id == oItem.ProductId)
                .SingleOrDefault().Title,

                Price = _db.Products.Where(product => product.Id == oItem.ProductId)
                .SingleOrDefault().Price.ToString("N0") + "원",

                Quantity = oItem.Quantity.ToString() + "개",

                SubTotal = oItem.SubTotal.ToString("N0") + "원"
            });
            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
