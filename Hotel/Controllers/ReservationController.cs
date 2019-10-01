using Hotel.Models;
using Hotel.Security;
using Microsoft.Ajax.Utilities;
using Model;
using Newtonsoft.Json;
using PdfSharp;
using PdfSharp.Pdf;
using Services.ServiceClient;
using Services.ServiceReservation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TheArtOfDev.HtmlRenderer.PdfSharp;

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
            List<Reservation> _reserv = sr.GetMany().Reverse().ToList();

            if (d1 != null && d2==null)
            {
                _reserv = _reserv.Where(x => x.Arrivee >= d1).ToList();
            }
            if(d1==null && d2 != null)
            {
                _reserv = _reserv.Where(x => x.dft >= d2).ToList();


            }
            if (d2 != null & d1!=null)
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
                _reserv = _reserv.Where(x => x.nat.Equals(kw, StringComparison.InvariantCultureIgnoreCase) ||
                x.type.Equals(kw, StringComparison.InvariantCultureIgnoreCase) || 
                x.bons.Equals(kw, StringComparison.InvariantCultureIgnoreCase) || rech(x.Clients,kw)||
                x.agence.Equals(kw, StringComparison.InvariantCultureIgnoreCase) || x.devise.Equals(kw, StringComparison.InvariantCultureIgnoreCase)).ToList();
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

            WebClient n = new WebClient();
            var json = n.DownloadString("https://openexchangerates.org/api/currencies.json");
            
          

            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            ViewBag.currency = values;
            return View();
        }
            





        [CustomAuthorizeAttribute(Roles = "director")]
        public ActionResult AddReservation(Reservation res,string name1,string name2,string name3)
        {

            IserviceClient scl = new ServiceClient();

            IList<Client> cll = new List<Client>();

            DateTime d1 = res.Arrivee;
            DateTime d2 = res.dft;
            List<Reservation> rss = sr.GetMany(x => (x.chambre == res.chambre) && (! (x.Arrivee > d2) && !(x.dft <= d1))).ToList();
            if ( rss.Count!=0)
            {
                ModelState.AddModelError("", "chambre non disponible");
            }


            if (name1 != null && name1!="")
            {
                Client cl = new Client();
                cl.nomC = name1;
                cl.idr = res.id;
                cll.Add(cl);
                //scl.Commit();
            }
            if (name2 != null && name2 != "")
            {
                Client cl = new Client();
                cl.nomC = name2;
                cl.idr = res.id;
                cll.Add(cl);
                //scl.Commit();

            }
            if (name3 != null && name3 != "")
            {
                Client cl = new Client();
                cl.nomC = name3;
                cl.idr = res.id;
                cll.Add(cl);
                //scl.Commit();

            }
            if (ModelState.IsValid)
            {

                Revenu rv = new Revenu();
                rv.devise = res.devise;
                rv.montant = res.montant;
                rv.type = res.methpaie;
                rv.daterev = DateTime.Now;

                res.revenu = rv;
                res.comfirmed = true;
                res.Clients = cll;
                sr.Add(res);
                sr.Commit();

            }
            else
            {
                WebClient n = new WebClient();
                var json = n.DownloadString("https://openexchangerates.org/api/currencies.json");



                var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                ViewBag.currency = values;
                return View();
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





        public ActionResult confirmer(int id)
        {

            Reservation rs = sr.GetById(id);
            rs.comfirmed = false;
            sr.Update(rs);
            sr.Commit();
            return RedirectToAction("reservations");
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

        public ActionResult Facture(int id)
        {
            Reservation resa = sr.GetById(id);


            Byte[] res = null;
            using (MemoryStream ms = new MemoryStream())
            {
                var pdf = PdfGenerator.GeneratePdf("some text", .PageSize.A4);
                pdf.Save(ms);
                res = ms.ToArray();
            }

            //StringWriter sw = new StringWriter();
            //HtmlTextWriter hw = new HtmlTextWriter(sw);
            //pnlPerson.RenderControl(hw);
            //StringReader sr = new StringReader(sw.ToString());
            //Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
            //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            //PdfWriter.GetInstance(pdfDoc, new FileStream(Server.MapPath("~/PDFs/") + "mypdf.pdf", FileMode.Create));
            //pdfDoc.Open();
            //htmlparser.Parse(sr);
            //pdfDoc.Close();
            return File(res, "application/pdf", "facture N°" + resa.id + ".pdf");

            
        }
    }

}