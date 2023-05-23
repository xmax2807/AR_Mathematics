using System;
using System.Collections.Generic;
using Project.Utils;
using System.Linq;
using Project.Utils.ExtensionMethods;
using Project.QuizSystem.SaveLoadQuestion;
using UnityEngine.UI;

namespace Project.QuizSystem{
    public struct Shape : IEquatable<Shape>{
        public static Dictionary<ShapeType, string> TextMap = new(Enum.GetNames(typeof(ShapeType)).Length){
            {ShapeType.Circle, "Hình Tròn"},
            {ShapeType.Triangle, "Hình Tam Giác"},
            {ShapeType.Square, "Hình Vuông"},
            {ShapeType.Rectangle, "Hình Chữ Nhật"},
        };
        public ShapeType type;
        public Shape(ShapeType type){
            this.type = type;
        }
        public enum ShapeType{
            Square, Rectangle, Circle, Triangle
        }
        public readonly bool Equals(Shape other) {
            return other.type == this.type;
        }
        public override readonly string ToString()
        {
            return TextMap[type];
        }
    }
    public class ShapeQuestion : BaseQuestion<Shape>, IEquatable<ShapeQuestion>, IRandomizableQuestion
    {
        public override QuestionType QuestionType => QuestionType.Other;

        public override QuestionContentType QuestionContentType => QuestionContentType.None;

        public ShapeQuestion(string question) : base(question){
            Randomize();
        }
        public ShapeQuestion(string question, Shape answer) : base(question, answer)
        {
        }

        public bool Equals(ShapeQuestion other)
        {
            return other._answer.Equals(_answer);
        }
        protected override void TrySetAnswer(object value)
        {
            if(value is Shape.ShapeType shapeType){
                _playerAnswered = new Shape(shapeType);
            }
        }

        public override string GetQuestion()
        {
            return base.GetQuestion() + ' ' + Shape.TextMap[_answer.type];
        }
        public void Randomize(Random random = null)
        {
            var shapeType = FlagExtensionMethods.Randomize<Shape.ShapeType>(random);
            _answer = new Shape(shapeType);
        }

        public override IQuestion Clone()
        {
            return new ShapeQuestion(this._question);
        }

        public IQuestion Random(Random rand = null){
            var result = new ShapeQuestion(this._question);
            result.Randomize();
            return result;
        }

        public IQuestion GetClone()
        {
            return Clone();
        }
    }
}