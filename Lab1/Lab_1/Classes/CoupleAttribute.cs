using System;

namespace Lab_1.Classes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CoupleAttribute: Attribute
    {
        public string Pair { get; set; }
        public double Probability { get; set; }
        public string ChildType { get; set; }
    }
}
