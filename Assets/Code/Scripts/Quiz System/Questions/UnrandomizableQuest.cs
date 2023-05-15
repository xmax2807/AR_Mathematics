using System.Collections.Generic;
using System.Threading.Tasks;
using Project.QuizSystem.SaveLoadQuestion;
using UnityEngine;

namespace Project.QuizSystem{
    // public struct Direction{
    //     public static Dictionary<DirectionType, string> textMap = new(){
    //         {DirectionType.Up, "Trên"},
    //         {DirectionType.Left, "Trái"},
    //         {DirectionType.Right, "Phải"},
    //         {DirectionType.Down, "Dưới"},
    //         {DirectionType.Middle, "Giữa"},
    //     };
        
    //     public enum DirectionType{
    //         Up,Left,Right,Down,Middle
    //     }
    // }
    public class GeneralQuestion : SingleChoice<string>, IVisitableImageQuestion<string>
    {
        private string imageUrl;
        public GeneralQuestion(string question, string[] options, int answer, string imageUrl) : base(question, options, answer)
        {
            this.imageUrl = imageUrl;
        }

        public override QuestionContentType QuestionContentType => QuestionContentType.Image;

        public Task<Sprite> AcceptVisitor(IQuestionVisitor visitor)
        {
            return visitor.AskForSprite(this);
        }

        public override IQuestion Clone()
        {
            return new GeneralQuestion(this._question, this.options, this._answer, this.imageUrl);
        }

        public override QuestionSaveData ConvertToData()
        {
            return new UnrandomizableSCQSaveData(){
                Question = _question,
                ImageURL = imageUrl,
                Options = options,
                UserAnswerIndex = _answer,
                CorrectAnswerIndex = _playerAnswered,
            };
        }
        public override void SetData(QuestionSaveData data)
        {
            base.SetData(data);
            if(data is UnrandomizableSCQSaveData saveData){
                imageUrl = saveData.ImageURL;
            }
        }

        public string GetImageType()
        {
            return imageUrl;
        }

        public override string[] GetOptions()
        {
            return options;
        }
    }
}