using Lab_1.Classes;

namespace Lab_1.People
{
    [Couple(Pair = "Girl", ChildType = "SmartGirl", Probability = 0.7)]
    [Couple(Pair = "PrettyGirl", ChildType = "PrettyGirl", Probability = 1)]
    [Couple(Pair = "SmartGirl", ChildType = "Book", Probability = 0.8)]

    public class Botan : Human
    {
    }
}
