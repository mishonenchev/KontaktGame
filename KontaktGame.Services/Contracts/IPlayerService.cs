using KontaktGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KontaktGame.Services.Contracts
{
    public interface IPlayerService
    {
        void AddPlayer(Player player);
        string GenerateCookie();
        Player GetPlayerByCookie(string cookie);
        List<Player> GetAll();
        void Update();
        Player GetByConId(string conId);
        void RemoveInactivePlayers();
    }
}
