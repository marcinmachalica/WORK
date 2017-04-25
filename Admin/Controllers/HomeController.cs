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
using mongotest;

namespace Admin.Controllers
{
    public class HomeController : Controller
    {


        
        async public Task<ActionResult> Index()
        {
            Config conf = new Config();
            var coll = conf.Connect();
            IEnumerable<AdminHardwareModel> list = await coll.Find(_ => true).ToListAsync();
            List<IndexViewModel> ViewList = new List<IndexViewModel>();
            foreach (var item in list)
            {
                IndexViewModel VM = new IndexViewModel()
                {
                    ViewModelId = item._id,
                    ComputerName = item.ComputerName,
                    OsVersion = item.OsVersion,
                    UserName = item.UserName,
                    Is64 = item.Is64,
                    TickCount = item.TickCount,
                    UserDomain = item.UserDomain,
                    CpuName = item._cpu.CPUName,
                    GpuName = item._gpu.GPUName,
                    RAMsize = (item._memory.UsedMemory[0] + item._memory.UsedMemory[1]).ToString(),
                    Mbname = item._mb.MBName,
                    HDDName = item._hdd.HDName[0],
                    LastUpdate=item.LastUpdate
    };
                ViewList.Add(VM);

            }

            
            
            return View(ViewList);
        }

        
        async public Task<ActionResult> Details(string id)
        {
            Config conf = new Config();
            var coll=conf.Connect();
            IEnumerable<AdminHardwareModel> list =await coll.Find(x => x._id == id).ToListAsync();

            return View(list);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}