using KontaktGame.Database.Contracts;
using KontaktGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KontaktGame.Database.Repositories
{
    public class UsedWordRepository : BaseRepository<UsedWord>, IUsedWordRepository
    {
        public UsedWordRepository(IDatabase database) : base(database)
        {
        }
    }
}
