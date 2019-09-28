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



        [CustomAuthorizeAttribute(Roles = "director")]
        public ActionResult Reservations(DateTime? d1, DateTime? d2, string kw,int? num)
        {
            List<Reservation> _reserv = sr.GetMany().ToList();

            if (d1 != null && d2==null)
            {
                _reserv = _reserv.Where(x => x.Arrivee >= d1).ToList();
            }
            if (d2 != null & d1==null)
            {
                _reserv = _reserv.Where(x =>(x.Arrivee<=d1 && x.dft<=d2 && x.dft>d1)||(x.Arrivee>=d1 && x.dft<=d2)||(x.Arrivee>=d1 && x.dft>=d2 && x.Arrivee<d2)|| (x.Arrivee<=d1 && x.dft>=d2) ).ToList();
            }
            if(d1!=null && d2 != null)
            {

            }
            if (num != null)
            {
                _reserv = _reserv.Where(x => x.chambre == num).ToList();
            }
            if(kw!=null && kw != "")
            {
                _reserv = _reserv.Where(x => x.nat==kw||x.type==kw || x.bons==kw || rech(x.Clients,kw)|| x.agence==kw || x.devise==kw).ToList();
            }

            return View(_reserv);
        }
        /*("select e from Logement e , Contrat c where (e.idLog=c.logement and (:d1 not between c.dateDeb and c.dateFin) and (:d2 not between c.dateDeb and c.dateFin) and "
			+ "(c.dateDeb not between :d1 and :d2) and (c.dateFin not between :d1 and :d2)) "
			+ "or (e.idLog not in (select a.logement from Contrat a))")*/




        [CustomAuthorizeAttribute(Roles = "director")]
        [HttpGet]
        public ActionResult AddReservation()
        {
            return View();

        }
        //resume




        [CustomAuthorizeAttribute(Roles = "director")]
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




        [CustomAuthorizeAttribute(Roles = "director")]
        public ActionResult Delete(int id)
        {
            sr.Delete(x => x.id == id);
            sr.Commit();
            return RedirectToAction("Reservations");
        }




        [CustomAuthorizeAttribute(Roles = "director")]
        public JsonResult Detailres(int id)
        {
            IserviceClient sc = new ServiceClient();
            IserviceReservation sd = new ServiceReservation();
            dynamic dp = sd.GetMany(x => x.id == id).Select(s => new {
                id = s.id,
                chambre = s.chambre,
                agence = s.agence,
                type = s.type,
                Arrivee = s.Arrivee,
                nat = s.nat,
                nombre = s.nombre,
                montant = s.montant,
                bons = s.bons,
                dft = s.dft,
                methpaie = s.methpaie,
                devise=s.devise,
                comfirmed=s.comfirmed,
                Clients = s.Clients.Select(g => new { nomC=g.nomC})

            });




            return   Json(dp, JsonRequestBehavior.AllowGet);

        }




        private bool rech(ICollection<Client> cl,string kw)
        {
            bool flag = false;

            foreach(Client x in cl)
            {
                if (x.nomC == kw) { flag = true; }
            }


            return flag; 
        }

    }
}