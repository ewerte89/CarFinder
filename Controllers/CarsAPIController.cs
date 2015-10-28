using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cars.Models;
using System.Threading.Tasks;

namespace Cars.Controllers
{
    [RoutePrefix("api/cars")]
    public class CarsAPIController : ApiController
    {
        ApplicationDbContext Db = new ApplicationDbContext();

        [Route("GetCars")]
        public async Task<List<Cars.Models.Cars>> GetCars()
        {
            return await Db.GetCars("", "", "", "", "", null, null, null);
        }
    }
}
