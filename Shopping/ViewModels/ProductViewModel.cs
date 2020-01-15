using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string ImageSrc { get; set; }
        public string Title { get; set; }
        public string Price { get; set; }
    }
}
