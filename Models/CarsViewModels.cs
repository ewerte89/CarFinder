using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cars.Models
{
    public class CarsViewModels
    {
        public Cars Car { get; set; }
        public dynamic Recalls { get; set; }
        public string Image { get; set; }
    }
}