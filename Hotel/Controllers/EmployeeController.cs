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
using Services;
using Services.ServiceClient;
using Services.ServiceReservation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Hotel.Controllers
{
    [CustomAuthorizeAttribute(Roles = "employee")]
    public class EmployeeController : Controller
    {
        IserviceReservation sr = new ServiceReservation();
        IserviceUser su = new ServiceUser();

        // GET: Reservation
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Reservations(DateTime? d1, DateTime? d2, string kw, int? num)
        {
            List<Reservation> _reserv = sr.GetMany().Reverse().ToList();

            if (d1 != null && d2 == null)
            {
                _reserv = _reserv.Where(x => x.Arrivee >= d1).ToList();
            }
            if (d1 == null && d2 != null)
            {
                _reserv = _reserv.Where(x => x.dft >= d2).ToList();


            }
            if (d2 != null & d1 != null)
            {
                _reserv = _reserv.Where(x => (x.Arrivee <= d1 && x.dft <= d2 && x.dft > d1) || (x.Arrivee >= d1 && x.dft <= d2) || (x.Arrivee >= d1 && x.dft >= d2 && x.Arrivee < d2) || (x.Arrivee <= d1 && x.dft >= d2)).ToList();
            }
            if (d1 != null && d2 != null)
            {

            }
            if (num != null)
            {
                _reserv = _reserv.Where(x => x.chambre == num).ToList();
            }
            if (kw != null && kw != "")
            {
                _reserv = _reserv.Where(x => x.nat.Equals(kw, StringComparison.InvariantCultureIgnoreCase) ||
                x.type.Equals(kw, StringComparison.InvariantCultureIgnoreCase) ||
                x.bons.Equals(kw, StringComparison.InvariantCultureIgnoreCase) || rech(x.Clients, kw) ||
                x.agence.Equals(kw, StringComparison.InvariantCultureIgnoreCase) || x.devise.Equals(kw, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            return View(_reserv);
        }




        public ActionResult AddReservation()
        {

            WebClient n = new WebClient();
            var json = n.DownloadString("https://openexchangerates.org/api/currencies.json");



            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            ViewBag.currency = values;
            return View();
        }






        public ActionResult AddReservation(Reservation res, string name1, string name2, string name3)
        {

            IserviceClient scl = new ServiceClient();

            IList<Client> cll = new List<Client>();

            DateTime d1 = res.Arrivee;
            DateTime d2 = res.dft;
            List<Reservation> rss = sr.GetMany(x => (x.chambre == res.chambre) && (!(x.Arrivee > d2) && !(x.dft <= d1))).ToList();
            if (rss.Count != 0)
            {
                ModelState.AddModelError("", "chambre non disponible");
            }


            if (name1 != null && name1 != "")
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

            return RedirectToAction("Reservations", "Reservation");

        }



        public ActionResult Details(int id)
        {
            Reservation rs = sr.GetById(id);
            return View(rs);
        }


        public ActionResult Delete(int id)
        {


            sr.Delete(x => x.id == id);
            sr.Commit();
            return RedirectToAction("Reservations");
        }

        [HttpGet]
        public ActionResult editProfile()
        {
            //this is the get method
            //it will put the current connected user in the admin object
            User us = su.Get(x => x.mail == User.Identity.Name);
            return View(us);
        }

       
        [HttpPost]
        public ActionResult editProfile(User us, string confirmpassword)
        {

            //we will get the admin from the data base to attach it
            User _user = su.GetById(us.id);

            if (_user.mail != us.mail && !string.IsNullOrEmpty(us.mail) && !string.IsNullOrWhiteSpace(us.mail) && ModelState.IsValid)
            {
                //check if the email is not null , empty or white space and the change it
                _user.mail = us.mail;
            }

            if (us.password != "" && !string.IsNullOrWhiteSpace(us.password) && us.password == confirmpassword)
            {
                //for the password it s more tricky 
                //check if it s empty then check it s not white space and check if the two passord match

                SHA256 hash = new SHA256CryptoServiceProvider();
                Byte[] originalBytes = ASCIIEncoding.Default.GetBytes(us.password);
                Byte[] encodedBytes = hash.ComputeHash(originalBytes);
                us.password = BitConverter.ToString(encodedBytes);
                _user.password = us.password;

            }
            else if (!string.IsNullOrWhiteSpace(us.password) && us.password != confirmpassword)
            {
                //if the two password doesn't math return the same view with error msg
                ViewBag.error = "les mots de passe ne correspondent pas";
                return View();
            }
            //now update and commit


            su.Update(_user);
            su.Commit();

            ViewBag.success = "Votre Profile est mis à jour";
            return View();



        }



        public ActionResult confirmer(int id)
        {

            Reservation rs = sr.GetById(id);
            rs.comfirmed = false;
            sr.Update(rs);
            sr.Commit();
            return RedirectToAction("reservations");
        }



        public ActionResult Facture(int id, string tva)
        {
            Reservation resa = sr.Get(x => x.id == id);
            float tv = 14;

            if (tva != null && !string.IsNullOrEmpty(tva))
            {

                if (!float.TryParse(tva, out tv)) { tv = 14; }
            }


            // return View(resa);







            string html = "<body><div class='row'> <div class='span6'><h3>&nbsp;HOTEL DJERBA ERRIADH</h3></div>" +
                         " <div class='span2'>  <h3 class='pull-right' style='opacity:0.4'>FACTURE</h3> <br /><br /></div></div>" +
                         "<br /><br />< div class='row'><div class='span6' style='margin-left:50px'>" +
                         "<span ><b>Rue Mohamed el ferjeni houmet essouk, djerba tunisie<br /> DJERBA 4180<br />" +
                         "Tel : 00216 75 65 07 50 Fax : 00216 75 65 16 91<br />Email : hotel.erriadh @Topnet.tn</b></span>        </div>" +
                         "<div class='span2'><span class='pull-right' style='font-size:14px'><b>FACTURE : </b>&nbsp;&nbsp;&nbsp;" + resa.id + "<br /><b>DATE :</b> &nbsp;&nbsp;&nbsp;" +
                         DateTime.Today.Date.ToString("dd/MM/yyyy") + "</span><br /><br />" +
                         "</div></div><br /><div  >< h3 > FACTURE </ h3 ></ div >< br />< br /> " +
                         "<table class='table table-striped' style='border:solid 2px'><tr  ><th style='color:grey'>Description</th><th style = 'width:500px;color:grey;' > Montant </ th ></ tr >" +
                         "< tr style='height:400px'><td style = 'border:solid 2px' > Chambre " + resa.type + " de " + resa.Arrivee.ToString("dd/MM/yyyy") + " au " + resa.dft.ToString("dd/MM/yyyy") + "</td>" +
                         "<td style = 'border:solid 2px' >" + resa.montantpn + " " + resa.devise + "</ td ></ tr >< tr >< td >TVA</td><td >" + tv + "%</ td ></ tr >" +
                         "< tr >< td class='pull-right' >TOTAL</td><td  >" + resa.montant * (tv / 100 + 1) + " " + resa.devise + "</ td ></ tr ></ table >< br />< center >< h3 > MERCI DE VOTRE CONFIANCE !</h3>" +
                         "</center></body>";





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
            // XMLWorkerHelper.GetInstance().ParseXHtml(writer,document, new StreamReader(html));
            document.Close();

            return File(output.ToArray(), "application/pdf", "facture N°" + resa.id + ".pdf");



        }





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
                devise = s.devise,
                comfirmed = s.comfirmed,
                montantpn = s.montantpn,
                nbnuit = s.nbnuit,
                Clients = s.Clients.Select(g => new { nomC = g.nomC })

            });




            return Json(dp, JsonRequestBehavior.AllowGet);

        }



        private bool rech(ICollection<Client> cl, string kw)
        {
            bool flag = false;

            foreach (Client x in cl)
            {
                if (x.nomC == kw) { flag = true; }
            }


            return flag;
        }






    }
}