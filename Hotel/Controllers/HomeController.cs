using Hotel.Models;
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
        public ActionResult Index()
        {
            List<Reservation> _res = sr.GetMany().ToList();
            DateTime dt1= DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            DateTime dt2= DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Sunday);

            ViewBag.reservsem = _res.Where(x => x.Arrivee >= dt1 && x.Arrivee <= dt2).Count();
            ViewBag.money = _res.Where(x => x.Arrivee >= dt1 && x.Arrivee <= dt2).Sum(w => w.montant);
            ViewBag.chambre = _res.Where(x => x.Arrivee >= dt1 && x.Arrivee <= dt2).Select(l=>l.chambre).Distinct().Count();


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
    }
}