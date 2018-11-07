using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using KontaktGame.Services.Contracts;
using Microsoft.AspNet.SignalR;

namespace KontaktWeb.Hubs
{
    public class Hub : Microsoft.AspNet.SignalR.Hub
    {
        private readonly IPlayerService _playerService;
        public Hub(IPlayerService playerService)
        {
            _playerService = playerService;
        }
        public void SendMessage(string message)
        {
            Clients.All.newMessage(message);
        }
        public override Task OnConnected()
        {
            SendActiveUsers();
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            SendActiveUsers();
            return base.OnDisconnected(stopCalled);
        }
        private void SendActiveUsers()
        {
            var users = _playerService.GetAll();
            GlobalHost.ConnectionManager.GetHubContext<Hub>().Clients.All.receiveUsers(Json.Encode(users.Select(x=>new {name = x.Name, isAsked = x.IsAsked })));
        }
    }
}