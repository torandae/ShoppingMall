using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string ImageSrc { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
    }
}
