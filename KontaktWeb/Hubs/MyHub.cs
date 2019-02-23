using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using Hangfire;
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
        private bool questionPhase = false;
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
            if (users.Count() > 2 && !isStarted)
            {
                SendMessage("Game started");
                users.FirstOrDefault().IsAsked = true;
                _playerService.Update();
                var askedUser = _playerService.GetAll().Where(x => x.IsActive && x.IsAsked).FirstOrDefault();
                HttpRuntime.Cache["game started"] = true;
                ChangeButtons();
                Clients.Client(askedUser.ConID).promptForWordToGuess();
                StartRound();
            }
        }

        public void PromptForWord(string usedWord)
        {
            AddWordToGuess(usedWord);
            Clients.All.newMessage(_playerService.GetByConId(Context.ConnectionId).Name + " въведе дума за познаване.");
        }

        public void StartRound()
        {
            var user = _playerService.GetByConId(Context.ConnectionId);
            var users = _playerService.GetAll().Where(x => x.IsActive).ToList();
            questionPhase = true;
            ChangeButtons();

            //When the round ends make next user IsAsked - NextPlayerModal()
        }
        public void NextPlayerModal()
        {
            var user = _playerService.GetByConId(Context.ConnectionId);
            var users = _playerService.GetAll().Where(x => x.IsActive).ToList();
            if (user!=null)
            {
                user.IsAsked = false;
                users.SkipWhile(x=> x.Name==user.Name).Skip(1).FirstOrDefault().IsAsked = true;
                _playerService.Update();
                ChangeButtons();
                var askedUser = _playerService.GetAll().Where(x => x.IsActive && x.IsAsked).FirstOrDefault();
                Clients.Client(askedUser.ConID).promptForWordToGuess();
            }
        }
        public void FirstButtonAction(string input)
        {
            var user = _playerService.GetByConId(Context.ConnectionId);
            if (user.IsAsked)
            {
                Clients.All.displayAnswer("Да: " + input);
                AddUsedWord(input);
                Clients.All.displayUsedWord(input);
            }
            else
            {
                AddQuestion(input);
                Clients.All.displayQuestion(input);
                //when a player asked a question don't allow others to ask
            }
            ChangeButtons();
        }
        public void SecondButtonAction(string input)
        {
            var user = _playerService.GetByConId(Context.ConnectionId);
            if (user.IsAsked)
            {
                Clients.All.displayAnswer("Нe: " + input);
                AddUsedWord(input);
                Clients.All.displayUsedWord(input);
            }
            else
            {
                //if input matches WordToGuess end round
                AddUsedWord(input);
                Clients.All.displayUsedWord(input);
            }
            ChangeButtons();
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
                else if(!users[i].IsAsked && questionPhase)
                {
                    Clients.Client(users[i].ConID).buttonState("ЗАДАЙ", "ПОЗНАЙ");
                }
                else
                {
                    Clients.Client(users[i].ConID).buttonState("КОНТАКТ", "ПОЗНАЙ");
                } 
            }
        }
        public void AddQuestion(string question)
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
            var allUsers = _playerService.GetAll();
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
                Clients.All.displayWordToGuess(); //Show the whole word before beginning next game
                //EndGame();
                HttpRuntime.Cache["game started"] = false;
                questionPhase = false;
                var askedUser = allUsers.Where(x => x.IsAsked).FirstOrDefault();
                if (askedUser != null)
                {
                    askedUser.IsAsked = false;
                }
                ChangeButtons();
            }
            Clients.All.receiveUsers(Json.Encode(users.Select(x => new { name = x.Name, isAsked = x.IsAsked })));
            return base.OnDisconnected(stopCalled);
        }
    }
}