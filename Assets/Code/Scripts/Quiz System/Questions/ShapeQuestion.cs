using System;
using System.Collections.Generic;
using Project.Utils;
using System.Linq;
using Project.Utils.ExtensionMethods;

namespace Project.QuizSystem{
    public struct Shape : IEquatable<Shape>{
        public ShapeType type;
        public Shape(ShapeType type){
            this.type = type;
        }
        public enum ShapeType{
            Square, Rectangle, Circle, Triangle
        }
        public bool Equals(Shape other) => other.type == this.type;
    }
    public class ShapeQuestion : BaseQuestion<Shape>, IRandomizable<ShapeQuestion>
    {
        private static Dictionary<Shape.ShapeType, string> textMap = new(Enum.GetNames(typeof(Shape.ShapeType)).Length){
            {Shape.ShapeType.Circle, "Hình Tròn"},
            {Shape.ShapeType.Triangle, "Hình Tam Giác"},
            {Shape.ShapeType.Square, "Hình Vuông"},
            {Shape.ShapeType.Rectangle, "Hình Chữ Nhật"},
        };
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
            return base.GetQuestion() + ' ' + textMap[_answer.type];
        }
        public void Randomize(Random random = null)
        {
            var shapeType = FlagExtensionMethods.Randomize<Shape.ShapeType>(random);
            _answer = new Shape(shapeType);
        }
    }
}