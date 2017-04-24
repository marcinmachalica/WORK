using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Admin.Models;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using Admin.ViewModels;

namespace Admin.Controllers
{
    public class HomeController : Controller
    {


        
        async public Task<ActionResult> Index()
        {
            var _client = new MongoClient();
            var _db = _client.GetDatabase("ComputersStore");
            var coll = _db.GetCollection<AdminHardwareModel>("Computer");
            IEnumerable<AdminHardwareModel> list = await coll.Find(_ => true).ToListAsync();
            List<IndexViewModel> ViewList = new List<IndexViewModel>();
            foreach (var item in list)
            {
                IndexViewModel VM = new IndexViewModel()
                {
                    ComputerName = item.ComputerName,
                    OsVersion = item.OsVersion,
                    UserName = item.UserName,


                };
                ViewList.Add(VM);

            }

            
            
            return View(list);
        }

        
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