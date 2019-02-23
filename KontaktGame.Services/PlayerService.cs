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
        private readonly IQuestionService _questionService;
        private readonly IUsedWordService _usedWordService;
        private readonly IWordToGuessService _wordToGuessService;
        public PlayerService(IPlayerRepository playerRepository, IQuestionService questionService, IUsedWordService usedWordService, IWordToGuessService wordToGuessService)
        {
            _playerRepository = playerRepository;
            _questionService = questionService;
            _usedWordService = usedWordService;
            _wordToGuessService = wordToGuessService;
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
        public Player GetByConId(string conId)
        {
           return _playerRepository.Where(x => x.ConID == conId).FirstOrDefault();
        }
        public void RemoveInactivePlayers()
        {
            var dateTime = DateTime.Now.AddHours(-1);
            var inactivePlayers = _playerRepository.Where(x => x.IsActive == false && x.LastActiveTime < dateTime).ToList();
            foreach (var player in inactivePlayers)
            {
                foreach (var question in _questionService.GetQuestionByPlayer(player))
                {
                    _questionService.Remove(question);
                }
                foreach (var usedWord in _usedWordService.GetUsedWordByPlayer(player))
                {
                    _usedWordService.Remove(usedWord);
                }
                foreach (var wordToGuess in _wordToGuessService.GetWordToGuessByPlayer(player))
                {
                    _wordToGuessService.Remove(wordToGuess);
                }
                _playerRepository.SaveChanges();
                _playerRepository.Remove(player);
            }
        }
        public List<Player> GetAll()
        {
            return _playerRepository.GetAll().ToList();
        }
        public void Update()
        {
            _playerRepository.SaveChanges();
        }
    }
}
