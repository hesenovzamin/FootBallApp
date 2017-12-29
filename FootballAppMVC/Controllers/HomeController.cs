using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FootballAppMVC.Models;
using FootballAppMVC.Controllers;

namespace FootballAppMVC.Controllers
{
    public class HomeController : Controller
    {
        FootballDbEntitiesTeam myTables = new FootballDbEntitiesTeam();
        // GET: Home
        public ActionResult Index()
        {
            var id = 0;
            List<User> usersList = myTables.User.ToList();
            foreach (var user in usersList)
            {
                if (user.Username == CurrentUserHolder.currentUsername)
                {
                    id = user.Id;
                }
            }
            List<Games> MyGames = new List<Games>();
            foreach (var Usergame in myTables.UserAndGamesPivot.ToList())
            {
                if (Usergame.User_id == id)
                {
                    foreach (var game in myTables.Games.ToList())
                    {
                        if (Usergame.Game_id == game.Id)
                        {
                            MyGames.Add(game);
                        }
                    }
                }
            }
 
            var lastFive = (from item in MyGames  orderby item.Date select item).Take(5);

            ViewBag.LastFive = lastFive;
            List<UserAndGamesPivot> usersGamesList = myTables.UserAndGamesPivot.ToList();
            List<MainMenu> menuItems = myTables.MainMenu.ToList();
            List<Games> gamesList = myTables.Games.ToList();
            // Senfing to the view below

            ViewBag.curUser = CurrentUserHolder.currentUsername;
            ViewBag.gamesItems = gamesList;
            ViewBag.usersItems = usersList;
            return View(menuItems);
        }
    }
}