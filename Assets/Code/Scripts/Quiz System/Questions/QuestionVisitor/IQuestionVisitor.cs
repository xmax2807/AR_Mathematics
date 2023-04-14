using System.Threading.Tasks;

namespace Project.QuizSystem{
    public interface IQuestionVisitor{
        public Task<UnityEngine.Sprite> AskForSprite(ShapeSCQ imageQuestion);
        public Task<UnityEngine.Sprite> AskForSprite(GeneralQuestion generalQuestion);
    }
}