using Hotel.Models;
using Hotel.Security;
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
            
            List<Reservation> _res = sr.GetMany().ToList();
            if (dt1 == null )
            {
                DateTime d1 = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Sunday);

            }
            else {DateTime d1 = (DateTime)dt1; }
            if (dt2 == null)
            {
                DateTime d2 = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Saturday);
            }
            else { DateTime d2 = (DateTime)dt2; }

            ViewBag.reservsem = _res.Where(x => x.Arrivee >= dt1 && x.Arrivee <= dt2).Count();//nombre reservation
            ViewBag.money = _res.Where(x => x.Arrivee >= dt1 && x.Arrivee <= dt2).Sum(w => w.montant);//total income
            ViewBag.client = _res.Where(x => x.Arrivee >= dt1 && x.Arrivee <= dt2).Select(l=>l.Clients.Count()).Sum();


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




    }


 


}