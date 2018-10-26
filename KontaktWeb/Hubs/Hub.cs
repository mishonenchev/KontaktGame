using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace KontaktWeb.Hubs
{
    public class Hub : Microsoft.AspNet.SignalR.Hub
    {
        public void SendMessage(string msg)
        {
            Clients.All.newMessage(msg);
        }
    }
}