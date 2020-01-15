using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shopping.Data;

namespace Shopping.ViewComponents
{
    public class CartTotal : ViewComponent
    {
        private ShopDbContext _db;

        public CartTotal(ShopDbContext db)
        {
            _db = db;
        }

        public IViewComponentResult Invoke()
        {
            var model = "총 " + _db.CartItems.Sum(cItem => cItem.SubTotal).ToString("N0") + "원";
            return View("Default", model);
        }
    }
}
