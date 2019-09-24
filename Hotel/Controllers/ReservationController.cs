using Hotel.Models;
using Hotel.Security;
using Microsoft.Ajax.Utilities;
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
    [CustomAuthorizeAttribute(Roles = "director")]
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

      //public JsonResult Details(int id)
      //  {
      //      Reservation rs = sr.GetById(id);
      //      return JSON(new {montant=rs. }rs,JsonRequestBehavior.AllowGet);
      //  }


        public ActionResult Delete(int id)
        {
            sr.Delete(x => x.id == id);
            sr.Commit();
            return RedirectToAction("Reservations");
        }


        public JsonResult Detailres(int id)
        {
            IserviceClient sc = new ServiceClient();
            IserviceReservation sd = new ServiceReservation();
            dynamic dp = sd.GetMany(x=>x.id==id).Select(s=>new {
                id=s.id,
                chambre=s.chambre,
                agence=s.agence,
                type=s.type,
                Arrivee=s.Arrivee,
                nat=s.nat,
                nombre=s.nombre,
                montant=s.montant,
                bons=s.bons,
                dft=s.dft,
                Clients = s.Clients.Select(g => new { nomC=g.nomC})

            });




            return   Json(dp, JsonRequestBehavior.AllowGet);

        }


    }
}