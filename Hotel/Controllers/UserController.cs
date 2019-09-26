using Hotel.Models;
using Hotel.Security;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Hotel.Controllers
{
    public class UserController : Controller
    {

        IserviceUser su = new ServiceUser();

        // GET: User
        public ActionResult Index()
        {
            return View();



         }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User _uss)
        {

                User us = new User();
            SHA256 hash = new SHA256CryptoServiceProvider();
            Byte[] originalBytes = ASCIIEncoding.Default.GetBytes(_uss.password);
            Byte[] encodedBytes = hash.ComputeHash(originalBytes);
            _uss.password = BitConverter.ToString(encodedBytes);

            us = su.Get(x => x.mail == _uss.mail && x.password == _uss.password);
            if (us == null)
            {
                ViewBag.logerr = "email ou mot de passe invalide";
                return View();
            }
            else if(us.type=="employee")
            {
                FormsAuthentication.SetAuthCookie(us.mail, true);
                return RedirectToAction("Reservations", "Employee");

            }else if (us.type == "director")
            {
                FormsAuthentication.SetAuthCookie(us.mail, true);
                return RedirectToAction("Index", "Home");
            }

            return View();
        }


        [Authorize]
        public ActionResult logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();



            return RedirectToAction("login");
        }


        [CustomAuthorizeAttribute(Roles = "director")]
        [HttpGet]
        public ActionResult editProfile()
        {
            //this is the get method
            //it will put the current connected user in the admin object
            User us= su.GetMany(x => x.mail == User.Identity.Name).Single();
            return View(us);
        }

        [CustomAuthorizeAttribute(Roles = "director")]
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



        [CustomAuthorizeAttribute(Roles = "director")]
        [HttpGet]
        public ActionResult AddEmp()
        {
            return View();
        }

        [CustomAuthorizeAttribute(Roles = "director")]
        [HttpPost]
        public ActionResult AddEmp(User us,string pass)
        {
            if (pass != us.password)
            {
                ModelState.AddModelError("error", "mot de passe ne correspand pas");
                return View();
            }
            else
            {
                us.type = "employee";
                SHA256 hash = new SHA256CryptoServiceProvider();
                Byte[] originalBytes = ASCIIEncoding.Default.GetBytes(us.password);
                Byte[] encodedBytes = hash.ComputeHash(originalBytes);
                us.password = BitConverter.ToString(encodedBytes);
                su.Add(us);
                su.Commit();
                return RedirectToAction("listEmp");
            }


            
        }

        [CustomAuthorizeAttribute(Roles = "director")]
        public ActionResult DeleteEmp(int id)
        {

            su.Delete(x => x.id == id);
            su.Commit();
            return RedirectToAction("listEmp");
        }
        [CustomAuthorizeAttribute(Roles = "director")]
        [HttpGet]
        public ActionResult listEmp()
        {
            List<User> _users = su.GetMany(x => x.type != "director").ToList();
            return View(_users);
        }

       
       


    }
}