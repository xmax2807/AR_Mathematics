namespace Project.Utils{
    public interface IRandomizable{
        void Randomize(System.Random rand);
    }
    public interface IRandomizable<T> : IRandomizable where T : System.IEquatable<T>{}
}