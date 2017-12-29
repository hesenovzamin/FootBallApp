using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FootballAppMVC.Models;

namespace FootballAppMVC.Controllers
{
    public class CurrentUserHolder
    {
        static public string currentUsername = "";
    }
    public class LoginController : Controller
    {
        FootballDbEntitiesTeam myTables = new FootballDbEntitiesTeam();
        // GET: Login
        public ActionResult EnterSystem()
        {
            ViewBag.curUser = CurrentUserHolder.currentUsername;
            List<MainMenu> menuItems = myTables.MainMenu.ToList();
            return View(menuItems);
        }
        [HttpPost]
        public ActionResult EnterSystem(string username, string pass)
        {
            var a = 0;
            var identityStatus = false;
            foreach (var users in myTables.User)
            {
                // SURNAME IS THE USERNAME
                if(username == users.Username && pass == users.Password)
                {
                    users.Id = a;
                    identityStatus = true;
                    break;
                }
            }
            if(identityStatus == true)
            {
                CurrentUserHolder.currentUsername = username;
                CurrentUserIdHolder.curUserId = a;
            }
            else
            {
                return RedirectToAction("IncorrectLogin","Error");
            }

            return RedirectToAction("Index","Home");
        }
    }
}