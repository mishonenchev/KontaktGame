using KontaktGame.Database;
using KontaktGame.Database.Contracts;
using KontaktGame.Models;
using KontaktGame.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KontaktGame.Services
{
    public class PlayerService:IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        public PlayerService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }
        public void AddPlayer(Player player)
        {
            _playerRepository.Add(player);
            _playerRepository.SaveChanges();
        }
        public string GenerateCookie()
        {
            string alfabet = "abcdefghijklmnopqrstuvwxyz0123456789";
            string result="";
            Random rn = new Random();
            for (int i = 0; i < 83; i++)
            {
                result += alfabet[rn.Next(0, alfabet.Length - 1)];
            }
            if (_playerRepository.Where(x => x.CookieId == result).Count() == 0)
            {
                return result;
            }
            else return GenerateCookie();
        }
        public Player GetPlayerByCookie(string cookie)
        {
            return _playerRepository.Where(x => x.CookieId == cookie).FirstOrDefault();
        }
        public List<Player> GetAll()
        {
            return _playerRepository.GetAll().ToList();
        }
    }
}
