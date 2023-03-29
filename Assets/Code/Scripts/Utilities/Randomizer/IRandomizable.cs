namespace Project.Utils{
    public interface IRandomizable<T> : System.IEquatable<T>{
        void Randomize(System.Random rand);
    }
}