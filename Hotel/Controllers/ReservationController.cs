using Hotel.Models;
using Hotel.Security;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.pipeline.html;
using Model;
using Newtonsoft.Json;

using Services.ServiceClient;
using Services.ServiceReservation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
            Reservation resa = sr.Get(x=>x.id==id);













            string html=

                    "< body>< h1 > &nbsp; Erriadh Hotel Djerba </ h1 >< br />< br /><center class='justify - content - center' ><h2 > Facture N°" + resa.id +
            "</ h2 ></ center >< br />< br /><p style = 'font-size:20px' >< b > &nbsp; &nbsp; &nbsp; &nbsp; Nom et Prénom: </ b >< span >" + resa.Clients.FirstOrDefault().nomC +
            " </ span ></ p >< br />< p style = 'font-size:20px' >< b > &nbsp; &nbsp; &nbsp; &nbsp; N° Chambre: </ b >" + resa.chambre +
            " </ p >< br /> < p style = 'font-size:20px' >< b > &nbsp; &nbsp; &nbsp; &nbsp; Type Chambre : </ b >" + resa.type +
            "</ p >< br />< p style = 'font-size:20px' >< b > &nbsp; &nbsp; &nbsp; &nbsp; Date du réservation: </ b > de " + resa.Arrivee.ToString("dd/MM/yyyy") + " à " + resa.dft.ToString("dd/MM/yyyy") +
            "</ p >< br />< p style = 'font-size:20px' >< b > &nbsp; &nbsp; &nbsp; &nbsp; Montant payé : </ b >" + resa.montant + " " + resa.devise +
            "</ p >< br />< p style = 'font-size:20px' >< b > &nbsp; &nbsp; &nbsp; &nbsp; Payé " + (resa.methpaie == "Espece" ? " en" : "par") + " :</b>" + resa.methpaie +
            "</ p >< br />< br /> <div class='pull-right'> <p style = 'font-size:20px' >< b > Djerba, le" + DateTime.Today.Date.ToString("dd/MM/yyyy") +
            "</b></p> </div></body>";



          //  var cssfiles =System.IO.File.ReadAllText(@"C:\Users\karim\source\repos\Hotel\Hotel\Content\code\bootstrap\css\bootstrap.min.css") +
          //  System.IO.File.ReadAllText(@"C:\Users\karim\source\repos\Hotel\Hotel\Content\code\bootstrap\css\bootstrap-responsive.min.css");



          //  Byte[] res = null;
          //  using (var cssMemoryStream = new MemoryStream(Encoding.UTF8.GetBytes(cssfiles)))
          //  {
                

          //  using (MemoryStream ms = new MemoryStream())
          //  {
               
          //      StringReader reader = new StringReader(html);
          //      Document PdfFile = new Document(PageSize.A4);
          //      PdfWriter writer = PdfWriter.GetInstance(PdfFile, ms);
                   
          //      PdfFile.Open();
          //      XMLWorkerHelper.GetInstance().ParseXHtml(writer, PdfFile, ms,cssMemoryStream);
               
          //      PdfFile.Close();
          //      res = ms.ToArray();

          //      return File(res, "application/pdf", "facture N°" + resa.id + ".pdf");
          //     // return View(resa);
          //  }

          //}


            List<string> cssFiles = new List<string>
            {

               "~/Content/code/bootstrap/css/bootstrap.min.css" ,
                "~/Content/code/bootstrap/css/bootstrap-responsive.min.css"
            };

            MemoryStream output = new MemoryStream();

            MemoryStream input = new MemoryStream(Encoding.UTF8.GetBytes(html));

            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document, output);
            writer.CloseStream = false;

            document.Open();
            HtmlPipelineContext htmlContext = new HtmlPipelineContext(null);
            htmlContext.SetTagFactory(iTextSharp.tool.xml.html.Tags.GetHtmlTagProcessorFactory());

            ICSSResolver cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(false);
            cssFiles.ForEach(i => cssResolver.AddCssFile(System.Web.HttpContext.Current.Server.MapPath(i), true));

            var pipeline = new CssResolverPipeline(cssResolver, new HtmlPipeline(htmlContext, new PdfWriterPipeline(document, writer)));
            var worker = new XMLWorker(pipeline, true);
            var p = new XMLParser(worker);
            p.Parse(input);
            document.Close();

             return File(output.ToArray(), "application/pdf", "facture N°" + resa.id + ".pdf");


            //return View(resa);
        }
    }

}