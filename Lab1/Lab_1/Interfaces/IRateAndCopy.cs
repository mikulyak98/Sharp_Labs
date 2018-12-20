
namespace Lab_1.Interfaces
{
    public interface IRateAndCopy<T>
    {
        double rating_of_article { get; }
        T DeepCopy();
    }
}
