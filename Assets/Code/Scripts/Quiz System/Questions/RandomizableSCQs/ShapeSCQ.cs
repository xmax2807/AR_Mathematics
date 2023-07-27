using System;
using System.Linq;
using System.Threading.Tasks;
using Project.QuizSystem.SaveLoadQuestion;
using Project.Utils.ExtensionMethods;

namespace Project.QuizSystem{
    public class ShapeSCQ : RandomizableSCQ<Shape>, IVisitableImageQuestion<Shape.ShapeType>
    {
        public override QuestionContentType QuestionContentType => QuestionContentType.Image;
        public string spriteName;
        protected override string constQuestion => "Hình dưới đây là hình gì ?";

        public ShapeSCQ(int optionsLength) : base(optionsLength)
        {
            int enumLength = FlagExtensionMethods.GetLength<Shape.ShapeType>();
            if(enumLength < options.Length){
                this.options = new Shape[enumLength];
            }
        }


        public override void Randomize(Random rand = null)
        {
            rand ??= random;
            base.Randomize(rand);   
            
            spriteName = "";
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

        public async Task<UnityEngine.Sprite> AcceptVisitor(IQuestionVisitor visitor)
        {
            var sprite = await visitor.AskForSprite(this);
            spriteName = sprite.name;
            return sprite;
        }

        protected override RandomizableSCQ<Shape> DeepClone()
        {
            return new ShapeSCQ(this.options.Length);
        }

        protected override Shape ParseOptionFromString(string data)
        {
            if(Shape.TextMap.ContainsValue(data)){
                return new Shape(Shape.TextMap.FindMatch((keyVal)=> keyVal.Value == data).Key);
            }
            return new Shape();
        }

        protected override QuestionSaveData ConvertToData(RandomizableSCQSaveData<Shape> parent)
        {
            return new ImageSCQSaveData<Shape>(this.spriteName, parent);
        }
        public override void SetData(QuestionSaveData data)
        {
            if(data is ImageSCQSaveData<Shape> imageData){
                this.spriteName = imageData.ImageName;
            }
            base.SetData(data);
        }
    }
}