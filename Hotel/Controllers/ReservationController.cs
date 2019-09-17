using Hotel.Models;
using Model;
using Services.ServiceClient;
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




        public ActionResult Reservations(DateTime? d1, DateTime? d2, string kw,int? num)
        {
            List<Reservation> _reserv = sr.GetMany().ToList();

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

        public ActionResult AddReservation(Reservation res,string name1,string name2,string name3)
        {

            IserviceClient scl = new ServiceClient();

            if (ModelState.IsValid)
            {
                sr.Add(res);
                sr.Commit();
            
            }
            if(name1 != null && name1!="")
            {
                Client cl = new Client();
                cl.nomC = name1;
                cl.idr = res.id;
                scl.Add(cl);
                scl.Commit();
            }
            if (name2 != null && name2 != "")
            {
                Client cl = new Client();
                cl.nomC = name2;
                cl.idr = res.id;
                scl.Add(cl);
                scl.Commit();

            }
            if (name3 != null && name3 != "")
            {
                Client cl = new Client();
                cl.nomC = name3;
                cl.idr = res.id;
                scl.Add(cl);
                scl.Commit();

            }
          
            return RedirectToAction("Reservations","Reservation");
            
        }

      public ActionResult Detail(int id)
        {
            Reservation rs = sr.GetById(id);
            return View(rs);
        }





        public ActionResult Employee(DateTime? d1, DateTime? d2, string kw, int? num)
        {

            List<Reservation> _reserv = sr.GetMany().ToList();

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
            if (kw != null && kw != "")
            {
                _reserv = _reserv.Where(x => x.nat.Contains(kw) || x.type == kw).ToList();
            }

            return View("Reserv",_reserv);
        }


    }
}