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
    public class WordToGuessService : IWordToGuessService
    {
        private readonly IWordToGuessRepository _wordToGuessRepository;
        public WordToGuessService(IWordToGuessRepository wordToGuessRepository)
        {
            _wordToGuessRepository = wordToGuessRepository;
        }
        public void AddWordToGuess(WordToGuess wordToGuess)
        {
            _wordToGuessRepository.Add(wordToGuess);
            _wordToGuessRepository.SaveChanges();
        }
        public List<WordToGuess> GetAll()
        {
            return _wordToGuessRepository.GetAll().ToList();
        }
        public void Update()
        {
            _wordToGuessRepository.SaveChanges();
        }
    }
}