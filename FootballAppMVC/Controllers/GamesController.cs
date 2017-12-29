using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FootballAppMVC.Models;
using FootballAppMVC.Controllers;

namespace FootballAppMVC.Controllers
{
    public class EditGameIdHolder
    {
        static public int currentEditGameId = 0;
    }
    public class CurrentUserIdHolder
    {
        static public int curUserId = 0;
    }
    public class GamesController : Controller
    {
        FootballDbEntitiesTeam myTables =  new FootballDbEntitiesTeam();
        // GET: Games/AllGames
        public ActionResult AllGames()
        {
           
            ViewBag.curUser = CurrentUserHolder.currentUsername;
            List<MainMenu> menuItems = myTables.MainMenu.ToList();
            // the games list
            List<Games> gamesList = myTables.Games.ToList();
            var OrderGame = (from item in gamesList orderby item.Date descending select item);
            ViewBag.allGamesList = OrderGame;
            // the teams list
            List<Team> teamsList = myTables.Team.ToList();
            ViewBag.allTeamsList = teamsList;
            if(CurrentUserHolder.currentUsername == "")
            {
                return RedirectToAction("GoLogin","Error");
            }
            return View(menuItems);
        }
        public ActionResult NewGame()
        {
            return View();
        }
        [HttpPost]
        public ActionResult NewGame(string gameName, string teamFir, string teamSec, string gameDate)
        {
            // creating 2 teams
            Team team1 = new Team();
            Team team2 = new Team();
            //assigning values to teams
            team1.Name = teamFir;
            team2.Name = teamSec;
            team1.Goals = 0;
            team2.Goals = 0;
            // adding teams to the database
            myTables.Team.Add(team1);  
            myTables.Team.Add(team2);
            myTables.SaveChanges();
            // adding details to the game
            Games myGame = new Games();
            myGame.Name = gameName;
            myGame.Team1_id = team1.Id;
            myGame.Team2_id = team2.Id;
            myGame.Date = Convert.ToDateTime(gameDate);
            myTables.Games.Add(myGame);
            myTables.SaveChanges();
            return RedirectToAction("AllGames");
        }
        public ActionResult EditGame(int id)
        {
            ViewBag.curUser = CurrentUserHolder.currentUsername;
            // menu items
            List<MainMenu> menuItems = myTables.MainMenu.ToList();
            // the game id
            ViewBag.gameId = id;
            EditGameIdHolder.currentEditGameId = id;
            // all games list
            List<Games> allGames = myTables.Games.ToList();
            ViewBag.allGamesList = allGames;
            // all teams list
            List<Team> teamsList = myTables.Team.ToList();
            ViewBag.allTeamsList = teamsList;
            // all games and users pivot list
            List<UserAndGamesPivot> usersGamesList = myTables.UserAndGamesPivot.ToList();
            ViewBag.allUsersGamesList = usersGamesList;
            // all users list
            List<User> userList = myTables.User.ToList();
            ViewBag.allUserList = userList;
            ViewBag.Userid = CurrentUserIdHolder.curUserId;
             int a = 0, b = 0;
            foreach (var user in userList)
            {
                if (CurrentUserHolder.currentUsername == user.Username)
                {
                    a = user.Id;
                    break;
                }
            }
            foreach (var UserGame in usersGamesList)
             {
                if (a == UserGame.User_id && UserGame.IsAdmin == true && UserGame.Game_id == id)
                {
                   
                    b = UserGame.User_id;
                    break;
                }
            }
            ViewBag.aaa = a;
            ViewBag.bbb = b;
            return View(menuItems);
        }
        public ActionResult RemovePlayer(int RemoveID, int GameID)
        {
            List<UserAndGamesPivot> usersGamesList = myTables.UserAndGamesPivot.ToList();
            foreach (var RemovedPlayer in usersGamesList)
            {
                if (RemoveID == RemovedPlayer.User_id && GameID == RemovedPlayer.Game_id)
                {
                    myTables.UserAndGamesPivot.Remove(RemovedPlayer);
                    break;
                }
            }
            myTables.SaveChanges();
            return RedirectToAction("AllGames","Games");
        }
        [HttpPost]
        public ActionResult EditGame(int id, int score1, int score2)
        {
            List<User> userList = myTables.User.ToList();
            List<Games> allGames = myTables.Games.ToList();
            if (score1 >= 0 && score2 >= 0)
            {
                foreach (var game in allGames)
                {
                    if (game.Id == id)
                    {
                        string a = score1.ToString();
                        string b = score2.ToString();
                        string score = a + " - " + b;
                        game.Score = score; // has to be changed
                    }
                }
                myTables.SaveChanges();
            }
            return RedirectToAction("AllGames", "Games");
        }
        public ActionResult EnterStatic(int EditPlayerID)
        {
            ViewBag.PlayerId = EditPlayerID;
            ViewBag.curUser = CurrentUserHolder.currentUsername;
            // menu items
            List<MainMenu> menuItems = myTables.MainMenu.ToList();
            // all games list
            List<Games> allGames = myTables.Games.ToList();
            ViewBag.allGamesList = allGames;
            // all teams list
            List<Team> teamsList = myTables.Team.ToList();
            ViewBag.allTeamsList = teamsList;
            // all games and users pivot list
            List<UserAndGamesPivot> usersGamesList = myTables.UserAndGamesPivot.ToList();
            ViewBag.allUsersGamesList = usersGamesList;
            // all users list
            List<User> userList = myTables.User.ToList();
            ViewBag.allUserList = userList;
            ViewBag.UserId = CurrentUserIdHolder.curUserId;
            return View(menuItems);
        }

        [HttpPost]
        public ActionResult EnterStatic(int EditPlayerID,int goal, string Match)
        {
            int G = 0, D = 0, L = 0, W = 0;
            G = G + goal;
            List<User> userList = myTables.User.ToList();
            List<Games> GameList = myTables.Games.ToList();

            if (CurrentUserHolder.currentUsername == "")
            {
                return RedirectToAction("GoLogin", "Error");
            }

            foreach (var user in userList)
            {
                if (user.Id == EditPlayerID)
                {
                    int g = user.Goals;
                    int d = user.Draws;
                    int l = user.Loses;
                    int w = user.Wins;
                    user.Goals = g + G;
                    if (Match == "Draw")
                    {
                        D = D + 1;
                        d = d + D;
                        user.Draws = d;
                    }
                    if (Match == "Lose")
                    {
                        L = L + 1;
                        l = l + L;
                        user.Loses = l;
                    }
                    if (Match == "Win")
                    {
                        W = W + 1;
                        w = w + W;
                        user.Wins = w;
                    }
                    myTables.SaveChanges();

                }

            }

            return RedirectToAction("AllGames", "Games");
        }


        public ActionResult AddPlayer(int id)
                {
                    bool Player = true;
                    int playersCount = 0;
                    List<User> userList = myTables.User.ToList();
                    List<UserAndGamesPivot> Pivot = myTables.UserAndGamesPivot.ToList();
                    ViewBag.allUserList = userList;
                    if (CurrentUserHolder.currentUsername == "")
                    {
                        return RedirectToAction("GoLogin", "Error");
                    }
                    UserAndGamesPivot newPlayer = new UserAndGamesPivot();


                    foreach (var user in userList)
                    {

                        if (user.Username == CurrentUserHolder.currentUsername)
                        {
                            foreach (var centralId in Pivot)
                            {
                                if (user.Id == centralId.User_id && EditGameIdHolder.currentEditGameId == centralId.Game_id)
                                {
                                    Player = false;
                                    break;

                                }
                            }
                            if (Player == true)
                            {
                                newPlayer.User_id = user.Id;
                                newPlayer.Game_id = EditGameIdHolder.currentEditGameId;
                                foreach (var game in myTables.Games.ToList())
                                {
                                    if (game.Id == EditGameIdHolder.currentEditGameId)
                                    {
                                        if (id == 1)
                                        {
                                            newPlayer.Team_id = game.Team1_id;
                                        }
                                        if (id == 2)
                                        {
                                            newPlayer.Team_id = game.Team2_id;
                                        }
                                        foreach (var player in Pivot)
                                        {
                                            if(player.Game_id == game.Id)
                                            {
                                                playersCount++;
                                            }
                                        }
                                    }
                                }
                                if (playersCount == 0)
                                {
                                    newPlayer.IsAdmin = true;
                                }
                                else
                                {
                                    newPlayer.IsAdmin = false;
                                }
                                myTables.UserAndGamesPivot.Add(newPlayer);
                                break;
                            }

                        }
                    }
                    myTables.SaveChanges();
                    if (Player == false)
                    {
                        return Content("You are already in this game");
                    }
                    return RedirectToAction("EditGame", "Games", new { id = EditGameIdHolder.currentEditGameId });
                }
            }
        }
