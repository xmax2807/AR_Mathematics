using System;
using System.Linq;
using System.Threading.Tasks;
using Project.Utils.ExtensionMethods;

namespace Project.QuizSystem{
    public class ShapeSCQ : RandomizableSCQ<Shape>, IVisitableImageQuestion<Shape.ShapeType>
    {
        public override QuestionContentType QuestionContentType => QuestionContentType.Image;

        protected override string constQuestion => "Hình dưới đây là hình gì ?";

        public ShapeSCQ(int optionsLength) : base(optionsLength)
        {
            int enumLength = FlagExtensionMethods.GetLength<Shape.ShapeType>();
            if(enumLength < options.Length){
                this.options = new Shape[enumLength];
            }
            Randomize(random);
        }


        public override void Randomize(Random rand = null)
        {
            rand ??= random;
            base.Randomize(rand);
            
            var listEnum = FlagExtensionMethods.ToEnumerable<Shape.ShapeType>();
            var arrayEnum = listEnum.Shuffle().ToArray();

            for(int i = 0; i < options.Length; i++){                
                options[i] = new Shape(arrayEnum[i]);
            }
            
        }

        public Shape.ShapeType GetImageType()
        {
            return options[AnswerIndex].type;
        }

        public Task<UnityEngine.Sprite> AcceptVisitor(IQuestionVisitor visitor)
        {
            return visitor.AskForSprite(this);
        }
    }
}