using System;
using Project.Utils;
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
    public class ShapeQuestion : BaseQuestion<Shape>, IRandomizable
    {
        public ShapeQuestion(string question) : base(question){
            Randomize();
        }
        public ShapeQuestion(string question, Shape answer) : base(question, answer)
        {
        }
        public override string GetQuestion()
        {
            return base.GetQuestion() + ' ' + _answer.type;
        }
        public void Randomize(Random random = null)
        {
            var shapeType = FlagExtensionMethods.Randomize<Shape.ShapeType>(random);
            _answer = new Shape(shapeType);
        }
    }
}