using Hotel.Models;
using Hotel.Security;
using Services.ServiceDepence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hotel.Controllers
{
    [CustomAuthorizeAttribute(Roles ="director")]
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
            ServiceDepenses sd = new ServiceDepenses();
            List<Depenses> ld = sd.GetMany().ToList();
            return View(ld);
        }
        [HttpPost]
        public ActionResult allDep(Depenses dep)
        {
            return View();
        }
        public PartialViewResult Details(int id)
        {
            ServiceDepenses sd = new ServiceDepenses();
            Depenses dp = sd.GetById(id);

            return PartialView(dp);
        }
        public ActionResult Delete(int id)
        {
            IserviceDepenses sd = new ServiceDepenses();
            sd.Delete(x => x.id == id);
            sd.Commit();

            return RedirectToAction("allDep");
        }
    }
}