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
        [CustomAuthorizeAttribute(Roles = "director")]
        [HttpGet]
        public ActionResult addDep()
        {
            
            return View();
        }
        [CustomAuthorizeAttribute(Roles = "director")]
        [HttpPost]
        public ActionResult addDep(Depenses dep)
        {
            try
            {
                ServiceDepenses sd = new ServiceDepenses();
                dep.datedep = DateTime.Now;
                sd.Add(dep);
                return RedirectToAction("allDep");
            }
            catch
            {
                return View();
            }
            }
        [CustomAuthorizeAttribute(Roles = "director")]
        [HttpGet]
        public ActionResult allDep()
        {
            ServiceDepenses sd = new ServiceDepenses();
            List<Depenses> ld = sd.GetMany().Reverse().ToList();
            return View(ld);
        }
        [CustomAuthorizeAttribute(Roles = "director")]
        [HttpPost]
        public ActionResult allDep(Depenses dep)
        {
            return View();
        }
        [CustomAuthorizeAttribute(Roles = "director")]
        public PartialViewResult Details(int id)
        {
            ServiceDepenses sd = new ServiceDepenses();
            Depenses dp = sd.GetById(id);

            return PartialView(dp);
        }
        [CustomAuthorizeAttribute(Roles = "director")]
        public ActionResult Delete(int id)
        {
            IserviceDepenses sd = new ServiceDepenses();
            sd.Delete(x => x.id == id);
            sd.Commit();

            return RedirectToAction("allDep");
        }
        [CustomAuthorizeAttribute(Roles = "director")]
        public JsonResult Detaildep(int id)
        {
            ServiceDepenses sd = new ServiceDepenses();
            Depenses dp = sd.GetById(id);
            return new JsonResult { Data = dp, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
    }
}