using KontaktGame.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KontaktGame.Database
{
    public interface IDatabase
    {
        DbSet<Player> Players { get; set; }
        DbSet<Question> Questions { get; set; }
        DbSet<UsedWord> UsedWords { get; set; }
        DbSet<WordToGuess> WordsToGuess { get; set; }
        DbSet<T> Set<T>() where T : class;
        void SaveChanges();
    }
}
