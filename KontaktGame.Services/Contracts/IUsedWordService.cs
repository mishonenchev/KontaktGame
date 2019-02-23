using KontaktGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KontaktGame.Services.Contracts
{
    public interface IUsedWordService
    {
        void AddUsedWord(UsedWord usedWord);
        List<UsedWord> GetAll();
        IEnumerable<UsedWord> GetUsedWordByPlayer(Player player);
        void Remove(UsedWord usedWord);
        void Update();
    }
}
