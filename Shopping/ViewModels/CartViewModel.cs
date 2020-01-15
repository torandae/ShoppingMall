using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.ViewModels
{
    public class CartViewModel
    {
        public int Id { get; set; }
        public string ImageSrc { get; set; }
        public int ProductId { get; set; }
        public string Title { get; set; }
        public string Price { get; set; }
        public string Quantity { get; set; }
        public string SubTotal { get; set; }
        public string Total { get; set; }
    }
}
