using Hotel.Security;
using Model;
using PagedList;
using Services.ServiceRevenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hotel.Controllers
{
    public class RevenuController : Controller
    {
        IserviceRevenu srev = new ServiceRevenu();

        // GET: Revenu
        [CustomAuthorizeAttribute(Roles = "director")]
        public ActionResult Index(string kw, DateTime? d1, DateTime? d2)
        {

          


            List<Revenu> rev = srev.GetMany().Reverse().ToList();

            if (d1 != null && d2 == null)
            {
                rev = rev.Where(x => x.daterev >= d1).ToList();
            }
            if (d1 == null && d2 != null)
            {
                rev = rev.Where(x => x.daterev <= d2).ToList();


            }
            if (d2 != null & d1 != null)
            {
                rev = rev.Where(x => x.daterev >= d1 && x.daterev<=d2  ).ToList();
            }
           
            if (kw != null && kw != "" && !string.IsNullOrEmpty(kw))
            {
                rev = rev.Where(x=>x.devise.Equals(kw, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }



            rev=rev.GroupBy(s=>s.devise).Select(h=> new Revenu { devise=h.Key,montant=h.Sum(l=>l.montant)}).ToList();



            return View(rev);
        }
        [CustomAuthorizeAttribute(Roles = "director")]
        // GET: Revenu/Details/5
        public ActionResult Details(string kw, DateTime? d1, DateTime? d2,int? page)
        {
            var currentPage = page != null || page == 0 ? (int)page : 1;
            List<Revenu> rev = srev.GetMany().Reverse().ToList();

            if (d1 != null && d2 == null)
            {
                rev = rev.Where(x => x.daterev >= d1).ToList();
            }
            if (d1 == null && d2 != null)
            {
                rev = rev.Where(x => x.daterev <= d2).ToList();


            }
            if (d2 != null & d1 != null)
            {
                rev = rev.Where(x => x.daterev >= d1 && x.daterev <= d2).ToList();
            }

            if (kw != null && kw != "" && !string.IsNullOrEmpty(kw))
            {
                rev = rev.Where(x => x.devise.Equals(kw, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            ViewBag.kw = kw;
            ViewBag.d1 = d1;
            ViewBag.d2 = d2;

            return View(rev.ToPagedList(currentPage,20));
        }

        // GET: Revenu/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Revenu/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Revenu/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Revenu/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Revenu/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Revenu/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
