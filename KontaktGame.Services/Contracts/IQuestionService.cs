using KontaktGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KontaktGame.Services.Contracts
{
    public interface IQuestionService
    {
        void AddQuestion(Question question);
        List<Question> GetAll();
        IEnumerable<Question> GetQuestionByPlayer(Player player);
        void Remove(Question question);
        void Update();
    }
}
