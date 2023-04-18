using System;
using System.Collections.Generic;
using Project.Utils;
using System.Linq;
using Project.Utils.ExtensionMethods;
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
        public bool Equals(Shape other) => other.type == this.type;
        public override string ToString()
        {
            return TextMap[type];
        }
    }
    public class ShapeQuestion : BaseQuestion<Shape>, IRandomizable<ShapeQuestion>
    {
        public override QuestionType QuestionType => QuestionType.Other;

        public override QuestionContentType QuestionContentType => throw new NotImplementedException();

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

        public override string GetQuestion()
        {
            return base.GetQuestion() + ' ' + Shape.TextMap[_answer.type];
        }
        public void Randomize(Random random = null)
        {
            var shapeType = FlagExtensionMethods.Randomize<Shape.ShapeType>(random);
            _answer = new Shape(shapeType);
        }
    }
}