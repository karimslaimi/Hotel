using Hotel.Models;
using Hotel.Security;
using Services;
using Services.ServiceDepence;
using Services.ServiceReservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hotel.Controllers
{
    public class HomeController : Controller
    {
        IserviceReservation sr = new ServiceReservation();
        [CustomAuthorizeAttribute(Roles = "director")]
        public ActionResult Index(DateTime? dt1,DateTime? dt2)
        {
            IserviceUser su = new ServiceUser();
            if (su.Get(x=>x.mail==User.Identity.Name).type=="employee") { return RedirectToAction("Reservations", "Employee"); }

            List<Reservation> _res = sr.GetMany().ToList();
            IserviceDepenses sd = new ServiceDepenses();
            List<Depenses> _dep = sd.GetMany().Take(5).ToList();
            ViewData["lastdep"] = _dep;

            //if (dt1 == null )
            //{
            //    DateTime d1 = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Sunday);

            //}
            //else {DateTime d1 = (DateTime)dt1; }
            //if (dt2 == null)
            //{
            //    DateTime d2 = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Saturday);
            //}
            //else { DateTime d2 = (DateTime)dt2; }

            //ViewBag.reservsem = _res.Where(x => x.Arrivee >= dt1 && x.Arrivee <= dt2).Count();//nombre reservation
            //ViewBag.money = _res.Where(x => x.Arrivee >= dt1 && x.Arrivee <= dt2).Sum(w => w.montant);//total income
            //ViewBag.client = _res.Where(x => x.Arrivee >= dt1 && x.Arrivee <= dt2).Select(l=>l.Clients.Count()).Sum();


           
            if (dt1!=null && dt2 == null)//if start date not null and end date is null that means from dt1 until now
            {
                ViewBag.reservsem = _res.Where(x => x.Arrivee >= dt1 && x.Arrivee <= DateTime.Now).Count();//nombre reservation
                ViewBag.money = _res.Where(x => x.Arrivee >= dt1 && x.Arrivee <= DateTime.Now).Sum(w => w.montant);//total income
                ViewBag.client = _res.Where(x => x.Arrivee >= dt1 && x.Arrivee <= DateTime.Now).Select(l => l.Clients.Count()).Sum();

                ViewBag.credit = _res.Where(x => x.methpaie == "credit" && x.Arrivee >= dt1 && x.Arrivee <= DateTime.Now).Sum(l => l.montant);
                ViewBag.cheque = _res.Where(x => x.methpaie == "cheque" && x.Arrivee >= dt1 && x.Arrivee <= DateTime.Now).Sum(l => l.montant);
                ViewBag.espece = _res.Where(x => x.methpaie == "espece" && x.Arrivee >= dt1 && x.Arrivee <= DateTime.Now).Sum(l => l.montant);

            }
            if (dt1 == null && dt2 != null)//if the start date is null and the end date is not null that means since the begining until now until now 
            {
                ViewBag.reservsem = _res.Where(x => x.Arrivee >= DateTime.Now.AddYears(-10) && x.Arrivee <= dt2).Count();//nombre reservation
                ViewBag.money = _res.Where(x => x.Arrivee >= DateTime.Now.AddYears(-10) && x.Arrivee <= dt2).Sum(w => w.montant);//total income
                ViewBag.client = _res.Where(x => x.Arrivee >= DateTime.Now.AddYears(-10) && x.Arrivee <= dt2).Select(l => l.Clients.Count()).Sum();

                ViewBag.credit = _res.Where(x => x.methpaie == "credit" && x.Arrivee >= DateTime.Now.AddYears(-10) && x.Arrivee <= dt2).Sum(l => l.montant);
                ViewBag.cheque = _res.Where(x => x.methpaie == "cheque" && x.Arrivee >= DateTime.Now.AddYears(-10) && x.Arrivee <= dt2).Sum(l => l.montant);
                ViewBag.espece = _res.Where(x => x.methpaie == "espece" && x.Arrivee >= DateTime.Now.AddYears(-10) && x.Arrivee <= dt2).Sum(l => l.montant);

            }
            if (dt1 != null && dt2 != null)//if both dt1 and dt2 are not null that means between these dates
            {
                ViewBag.reservsem = _res.Where(x => x.Arrivee >= dt1 && x.Arrivee <= dt2).Count();//nombre reservation
                ViewBag.money = _res.Where(x => x.Arrivee >= dt1 && x.Arrivee <= dt2).Sum(w => w.montant);//total income
                ViewBag.client = _res.Where(x => x.Arrivee >= dt1 && x.Arrivee <= dt2).Select(l => l.Clients.Count()).Sum();

                ViewBag.credit = _res.Where(x => x.methpaie == "credit" && x.Arrivee >= dt1 && x.Arrivee <= dt2).Sum(l => l.montant);
                ViewBag.cheque = _res.Where(x => x.methpaie == "cheque" && x.Arrivee >= dt1 && x.Arrivee <= dt2).Sum(l => l.montant);
                ViewBag.espece = _res.Where(x => x.methpaie == "espece" && x.Arrivee >= dt1 && x.Arrivee <= dt2).Sum(l => l.montant);

            }
            if (dt1 == null && dt2 == null)//if both null give him the stats of this week
            {
                //DateTime d1 = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Sunday);
                //DateTime d2 = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Saturday);
                ViewBag.reservsem = _res.Where(x=>x.Arrivee.Month==DateTime.Now.Month).Count();//nombre reservation
                ViewBag.money = _res.Where(x=>x.Arrivee.Month == DateTime.Now.Month).Sum(w => w.montant);//total income
                ViewBag.client = _res.Where(x => x.Arrivee.Month == DateTime.Now.Month).Select(l => l.Clients.Count()).Sum();
                ViewBag.credit = _res.Where(x => x.methpaie == "credit").Sum(l => l.montant);
                ViewBag.cheque = _res.Where(x => x.methpaie == "cheque").Sum(l => l.montant);
                ViewBag.espece = _res.Where(x => x.methpaie == "espece").Sum(l => l.montant);
            }


            return View();
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



        public ActionResult Unauthorized()
        {
            return View();
        }

        public PartialViewResult etatChambres()
        {
            IserviceReservation sr = new ServiceReservation();
            List<Reservation> lsr = sr.GetMany(x => x.Arrivee <= DateTime.Today.Date && DateTime.Today.Date <= x.dft).ToList();
        
                List<int> lstcmbr = new List<int>();
          
            foreach (Reservation xr in lsr)
            {
                lstcmbr.Add(xr.chambre);
            }
            ViewBag.listcmbr = lstcmbr;
            return PartialView();

        }


    }


 


}