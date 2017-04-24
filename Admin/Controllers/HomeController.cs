using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Admin.Models;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Admin.Controllers
{
    public class HomeController : Controller
    {


        [HttpGet]
        async public Task<ActionResult> Index()
        {
            var _client = new MongoClient();
            var _db = _client.GetDatabase("ComputersStore");
            var coll = _db.GetCollection<AdminHardwareModel>("Computer");
            IEnumerable<AdminHardwareModel> list = await coll.Find(_ => true).ToListAsync();
            
            return View(list);
        }

        [HttpPost]
        public ActionResult Index(AdminHardwareModel AdminModel)
        {
            return View(AdminModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}