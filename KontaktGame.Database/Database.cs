using KontaktGame.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KontaktGame.Database
{
    public class Database : DbContext, IDatabase
    {
        public Database() : base("DefaultConnection")
        {
        }
        public DbSet<Player> Players { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<UsedWord> UsedWords { get; set; }
        public DbSet<WordToGuess> WordsToGuess { get; set; }
        public void SaveChanges()
        {
            base.SaveChanges();
        }
    }
 }
