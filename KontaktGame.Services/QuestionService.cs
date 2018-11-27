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
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        public QuestionService(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }
        public void AddQuestion(Question question)
        {
            _questionRepository.Add(question);
            _questionRepository.SaveChanges();
        }
        public List<Question> GetAll()
        {
            return _questionRepository.GetAll().ToList();
        }
        public void Update()
        {
            _questionRepository.SaveChanges();
        }
    }
}
