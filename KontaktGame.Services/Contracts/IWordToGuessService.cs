using KontaktGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KontaktGame.Services.Contracts
{
    public interface IWordToGuessService
    {
        void AddWordToGuess(WordToGuess wordToGuess);
        List<WordToGuess> GetAll();
        void Update();
    }
}
