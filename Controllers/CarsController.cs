using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cars.Models;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Bing;

namespace Cars.Controllers
{
    public class CarsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Cars
        public async Task<ActionResult> Index(string year="2000")
        {
            var cars = await db.GetCars(year, "", "", "", "", null, null, null);
            ViewBag.year = new SelectList(await db.GetYears(), year);
            return View(cars);
        }

        // GET: Cars/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarsViewModels carVM = new CarsViewModels();
            carVM.Car = db.Cars.Find(id);

            if (carVM.Car == null)
            {
                return HttpNotFound();
            }

            HttpResponseMessage response;
            string content = "";

            carVM.Recalls = "";
            carVM.Image = "";


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://nhtsa.gov/");

                try
                {
                    response = await client.GetAsync("webapi/api/Recalls/vehicle/modelyear/" + carVM.Car.model_year
                        + "/make/" + carVM.Car.make + "/model/" + carVM.Car.model_name + "?format=json");
                    content = await response.Content.ReadAsStringAsync();
                }
                catch (Exception e)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            carVM.Recalls = JsonConvert.DeserializeObject(content);

            var image = new BingSearchContainer(new Uri("https://api.datamarket.azure.com/Bing/search/"));

            image.Credentials = new NetworkCredential("accountKey", "5u/0CzVmYrTKDOjlxPePfPkh/G8llMIfVJ7QC/oNEvQ");

            var marketData = image.Composite("image", carVM.Car.model_year + " " + carVM.Car.make + " " + carVM.Car.model_name + " " + carVM.Car.model_trim,
                null, null, null, null, null, null, null, null, null, null, null, null, null).Execute();
            carVM.Image = marketData.First().Image.First().MediaUrl;

            return View(carVM);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
