using Hotel.Models;
using Services.ServiceDepence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hotel.Controllers
{
    public class DepController : Controller
    {
        // GET: Dep
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult addDep()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult addDep(Depenses dep)
        {
            ServiceDepenses sd = new ServiceDepenses();
            sd.Add(dep);          

            return View();
        }
        [HttpGet]
        public ActionResult allDep()
        {
            return View();
        }
        [HttpPost]
        public ActionResult allDep(Depenses dep)
        {
            ServiceDepenses sd = new ServiceDepenses();
            List<Depenses> ld=sd.GetAll().ToList();
            return View(ld);
        }
    }
}