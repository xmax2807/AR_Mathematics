using System.Threading.Tasks;
using UnityEngine;

namespace Project.QuizSystem{
    
    public interface IVisitableTextQuestion{
        public string GetQuestion();
    }
    public interface IVisitableImageQuestion{
        public Task<Sprite> AcceptVisitor(IQuestionVisitor visitor);
    }
    public interface IVisitableImageQuestion<T> : IVisitableImageQuestion{
        public T GetImageType();
    }
}