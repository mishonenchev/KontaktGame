using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using KontaktGame.Models;
using KontaktGame.Services.Contracts;
using Microsoft.AspNet.SignalR;

namespace KontaktWeb.Hubs
{

    public class MyHub : Microsoft.AspNet.SignalR.Hub
    {
        private readonly IPlayerService _playerService;
        private readonly IQuestionService _questionService;
        private readonly IUsedWordService _usedWordService;
        private readonly IWordToGuessService _wordToGuessService;
        public MyHub(IPlayerService playerService, IQuestionService questionService, IUsedWordService usedWordService, IWordToGuessService wordToGuessService)
        {
            _playerService = playerService;
            _questionService = questionService;
            _usedWordService = usedWordService;
            _wordToGuessService = wordToGuessService;
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
        public void StartGame()
        {
            var users = _playerService.GetAll().Where(x => x.IsActive).ToList();
            if (HttpRuntime.Cache["game started"] == null)
            {
                HttpRuntime.Cache.Insert("game started", false);
            }
            var isStarted = (bool)HttpRuntime.Cache["game started"];
            if (users.Count() >= 3 && !isStarted)
            {
                SendMessage("Game started");
                _playerService.GetAll().Where(x => x.IsActive).FirstOrDefault().IsAsked = true;
                HttpRuntime.Cache["game started"] = true;
                ChangeButtons();
                
                

            
            }
        }
        public void ChangeButtons()
        {
            var users = _playerService.GetAll().Where(x => x.IsActive).ToList();
            for (int i = 0; i < users.Count(); i++)
            {
                if (users[i].IsAsked)
                {
                    Clients.Client(users[i].ConID).buttonState("ДА", "НЕ");
                }
                else
                {
                    Clients.Client(users[i].ConID).buttonState("КОНТАКТ", "ПОЗНАЙ");
                } 
            }
        }

        public void AskQuestion(string question)
        {
            var user = _playerService.GetByConId(Context.ConnectionId);
            if (user!=null)
            {
                _questionService.AddQuestion(new Question() { question = question, PlayerWhoAsked = user });
            }
        }
        public void AddWordToGuess(string word)
        {
            var user = _playerService.GetByConId(Context.ConnectionId);
            if (user != null)
            {
                _wordToGuessService.AddWordToGuess(new WordToGuess() { Word = word, PlayerWhoIsAsked = user });
            }
        }
        public void AddUsedWord(string word)
        {
            var user = _playerService.GetByConId(Context.ConnectionId);
            if (user != null)
            {
                _usedWordService.AddUsedWord(new UsedWord() { Word = word, PlayerWhoUsed = user });
            }
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            var user = _playerService.GetByConId(Context.ConnectionId);
            var users = _playerService.GetAll().Where(x => x.IsActive);
            if (user != null)
            {
                user.IsActive = false;
                user.IsAsked = false;
                user.LastActiveTime = DateTime.Now;
                _playerService.Update();
                SendMessage(user.Name + " излезе от играта.");
            }
            if (users.Count() < 3 && (bool)HttpRuntime.Cache["game started"])
            {
                SendMessage("Game ended");
                //EndGame();
                HttpRuntime.Cache["game started"] = false;
                users.Where(x => x.IsAsked).FirstOrDefault().IsAsked = false;
                ChangeButtons();
            }
            Clients.All.receiveUsers(Json.Encode(users.Select(x => new { name = x.Name, isAsked = x.IsAsked })));
            return base.OnDisconnected(stopCalled);
        }
    }
}