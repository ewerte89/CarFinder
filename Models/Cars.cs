using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cars.Models
{
    public class Cars
    {
        public int id { get; set; }
        public string make { get; set; }
        public string model_name { get; set; }
        public string model_trim { get; set; }
        public string model_year { get; set; }
        public string engine_num_cyl { get; set; }
        public string transmission_type { get; set; }
        public string lkm_mixed { get; set; }
    }
}