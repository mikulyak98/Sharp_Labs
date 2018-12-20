using Lab_1.Interfaces;

namespace Lab_1.Classes
{
    public class Human: IHasName
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string GetName()
        {
            return Name;
        }

        public override string ToString() => $"{GetType()}: {Name} {Surname}";
    }
}
