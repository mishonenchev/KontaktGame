using KontaktGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KontaktGame.Database.Contracts;

namespace KontaktGame.Database.Repositories
{
    public class WordToGuessRepository : BaseRepository<WordToGuess>, IWordToGuessRepository
    {
        public WordToGuessRepository(IDatabase database) : base(database)
        {
        }
    }
}
