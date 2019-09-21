using Hotel.Models;
using Model;
using Services;
using Services.ServiceClient;
using Services.ServiceReservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Hotel.Controllers
{
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

            return View(_reserv);
        }

        [HttpGet]
        public ActionResult AddReservation()
        {
            return View();

        }
        //resume

        public ActionResult AddReservation(Reservation res, string name1, string name2, string name3)
        {

            IserviceClient scl = new ServiceClient();

            if (ModelState.IsValid)
            {
                sr.Add(res);
                sr.Commit();

            }
            if (name1 != null && name1 != "")
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

            return RedirectToAction("Reservations");

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

        //[CustomAuthorizeAttribute(Roles = "Admin")]
        [HttpPost]
        public ActionResult editProfile(User us, string confirmpassword)
        {

            //we will get the admin from the data base to attach it
            User _user = su.GetById(us.id);

            if (_user.mail != us.mail && !string.IsNullOrEmpty(us.mail) && !string.IsNullOrWhiteSpace(us.mail))
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
            else if (us.password != "" && us.password != confirmpassword)
            {
                //if the two password doesn't math return the same view with error msg
                ViewBag.error = "le mot de passe ne correspond pas";
                return View();
            }
            //now update and commit


            su.Update(_user);
            su.Commit();

            ViewBag.success = "Votre Profile a été mis à jour";
            return View();



        }

    }
}