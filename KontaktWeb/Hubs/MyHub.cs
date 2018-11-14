using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using KontaktGame.Services.Contracts;
using Microsoft.AspNet.SignalR;

namespace KontaktWeb.Hubs
{

    public class MyHub : Microsoft.AspNet.SignalR.Hub
    {
        private readonly IPlayerService _playerService;
        public MyHub(IPlayerService playerService)
        {
            _playerService = playerService;
        }
        public void SendMessage(string message)
        {
            Clients.All.newMessage(message);
        }
        public void SendActiveUsers()
        {
            string cookie = Context.RequestCookies.Where(x => x.Key == "auth").First().Value.Value;
            var currentUser = _playerService.GetPlayerByCookie(cookie);
            currentUser.ConID = Context.ConnectionId;
            currentUser.LastActiveTime = DateTime.Now;
            currentUser.IsActive = true;
            _playerService.Update();
            _playerService.RemoveInactivePlayers();
            SendMessage(currentUser.Name + " влезе в играта.");
            var users = _playerService.GetAll().Where(x => x.IsActive);
            Clients.All.receiveUsers(Json.Encode(users.Select(x => new { name = x.Name, isAsked = x.IsAsked })));
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            var user = _playerService.GetByConId(Context.ConnectionId);
            if (user != null)
            {
                user.IsActive = false;
                user.LastActiveTime = DateTime.Now;
                _playerService.Update();
                SendMessage(user.Name + " излезе от играта.");
            }

            var users = _playerService.GetAll().Where(x => x.IsActive);
            Clients.All.receiveUsers(Json.Encode(users.Select(x => new { name = x.Name, isAsked = x.IsAsked })));
            return base.OnDisconnected(stopCalled);
        }
    }
}