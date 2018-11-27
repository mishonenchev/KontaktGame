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
    public class UsedWordService : IUsedWordService
    {
        private readonly IUsedWordRepository _usedWordRepository;
        public UsedWordService(IUsedWordRepository usedWordRepository)
        {
            _usedWordRepository = usedWordRepository;
        }
        public void AddUsedWord(UsedWord usedWord)
        {
            _usedWordRepository.Add(usedWord);
            _usedWordRepository.SaveChanges();
        }
        public List<UsedWord> GetAll()
        {
            return _usedWordRepository.GetAll().ToList();
        }
        public void Update()
        {
            _usedWordRepository.SaveChanges();
        }
    }
}
