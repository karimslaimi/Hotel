using Hotel.Models;
using Services.ServiceReservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hotel.Controllers
{
    public class ReservationController : Controller
    {

        IserviceReservation sr = new ServiceReservation();


        // GET: Reservation
        public ActionResult Index()
        {
            return View();
        }




        public ActionResult Reservations(DateTime? d1, DateTime d2, string kw,int? num)
        {
            List<Reservation> _reserv = sr.GetAll().ToList();

            if (d1 != null)
            {
                _reserv = _reserv.Where(x => x.Arrivee >= d1).ToList();
            }
            if (d2 != null)
            {
                _reserv = _reserv.Where(x => x.dft <= d2).ToList();
            }
            if (num != null)
            {
                _reserv = _reserv.Where(x => x.chambre == num).ToList();
            }
            if(kw!=null && kw != "")
            {
                _reserv = _reserv.Where(x => x.nat.Contains(kw)||x.type==kw).ToList();
            }

            return View(_reserv);
        }

        [HttpGet]
        public ActionResult AddReservation()
        {
            return View();

        }
        //resume

        public ActionResult AddReservation(Reservation res)
        {
            if (ModelState.IsValid)
            {
                sr.Add(res);
                sr.Commit();
                return RedirectToAction("");
            }
            return View();
            
        }

        public ActionResult validater(Reservation res)
        {
            return View(res);
        }




    }
}