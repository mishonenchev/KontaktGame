using KontaktGame.Models;
using KontaktGame.Services.Contracts;
using KontaktWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KontaktWeb.Hubs;

namespace KontaktWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPlayerService _playerService;
        public HomeController(IPlayerService playerService)
        {
            _playerService = playerService;
        }
        public ActionResult Index()
        {
            if (!Request.Cookies.AllKeys.Any(x => x == "auth"))
            {
                return View("Login");
            }
            else if (_playerService.GetPlayerByCookie(Request.Cookies.Get("auth").Value) == null)
            {
                return View("Login");
            }
            return View("Index");
        }
        public ActionResult Login(LoginViewModel vm)
        {
            if (!String.IsNullOrEmpty(vm.Name))
            {
                string cookie = _playerService.GenerateCookie();
                _playerService.AddPlayer(new Player() { Name = vm.Name, IsAsked = false, CookieId = cookie, IsActive = true, LastActiveTime = DateTime.Now });
                var cookieObj = new HttpCookie("auth", cookie);
                cookieObj.Expires = DateTime.Now.AddHours(2);
                Response.AppendCookie(cookieObj);
                return RedirectToAction("Index");
            }
            return View("Login");
        }
    }
}