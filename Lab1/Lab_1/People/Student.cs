using Lab_1.Classes;

namespace Lab_1.People
{
    [Couple(Pair = "Girl", ChildType = "Girl", Probability = 0.7)]
    [Couple(Pair = "PrettyGirl", ChildType = "PrettyGirl", Probability = 1)]
    [Couple(Pair = "SmartGirl", ChildType = "Girl", Probability = 0.5)]

    public class Student : Human
    {
    }
}
