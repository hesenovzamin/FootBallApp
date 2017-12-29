using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FootballAppMVC.Models;
using FootballAppMVC.Controllers;
using System.Net.Mail;

namespace FootballAppMVC.Controllers
{
    public class Userss
    {
        static public string currentName = "";
        static public string currentusername = "";
        static public int currentAge = 0;
        static public string currentEmail = "";
        static public string currentSex = "";
        static public string currentPassword = "";
        static public string currentPasswordr = "";
        static public string currentCode = "";

    }
    public class RegisterController : Controller
    {
        
        FootballDbEntitiesTeam myTables = new FootballDbEntitiesTeam();
        // GET: Register
        public ActionResult SignUp()
        {
            ViewBag.curUser = CurrentUserHolder.currentUsername;
            return View(myTables.MainMenu.ToList());
        }
        [HttpPost]
        public ActionResult SignUp(string Name, string username, int Age, string Email,
            string Sex, string Password, string Passwordr)
        {
            int say;
            Random rmd = new Random();
            say = rmd.Next(10000, 90000);
            List<User> Register = myTables.User.ToList();
            Userss.currentName = Name;
            Userss.currentusername = username;
            Userss.currentAge = Age;
            Userss.currentEmail = Email;
            Userss.currentSex = Sex;
            Userss.currentPassword = Password;
            Userss.currentPasswordr = Passwordr;
            Userss.currentCode = say.ToString();
            MailMessage msj = new MailMessage();
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("hesenovzamin@gmail.com", "zamin5525");
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;

            msj.To.Add(Email);
            msj.From = new MailAddress("hesenovzamin@gmail.com", "zamin5525");
            msj.Subject = "Dogrulama kodu";
            msj.Body = say.ToString();

            client.Send(msj);
            return RedirectToAction("MailCeck", "Register");
        }
        public ActionResult MailCeck()
        {
            List<MainMenu> menuItems = myTables.MainMenu.ToList();
            return View(menuItems);
        }
        [HttpPost]
        public ActionResult MailCeck(string Mail)
        {
            if (Mail == Userss.currentCode)
            {
                List<User> Register = myTables.User.ToList();
                bool users = true, emails = true;
                User user = new User();
                user.Name = Userss.currentName;
                user.Username = Userss.currentusername;
                user.Email = Userss.currentEmail;
                user.Age = Userss.currentAge;
                user.Sex = Userss.currentSex;
                user.Password = Userss.currentPassword;
                foreach (var name in Register)
                {
                    if (name.Username == Userss.currentusername)
                    {
                        users = false;
                        break;
                    }
                }
                foreach (var emaill in Register)
                {
                    if (emaill.Email == Userss.currentEmail)
                    {
                        emails = false;
                        break;
                    }
                }
                if (!(Userss.currentPassword == Userss.currentPasswordr && users == true && emails == true))
                {
                    return RedirectToAction("IncorrectSignUp", "Error");
                }
                if (Userss.currentPassword == Userss.currentPasswordr && users == true && emails == true)
                {
                    myTables.User.Add(user);
                    myTables.SaveChanges();
                    return RedirectToAction("SuccessfulSignUp", "Register");
                }
            }
            return RedirectToAction("IncorrectSignUp", "Error");

        }

        public ActionResult SuccessfulSignUp()
        {
            ViewBag.curUser = CurrentUserHolder.currentUsername;
            return View(myTables.MainMenu.ToList());
        }
    }
}