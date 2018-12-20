using System;
using System.Collections.Generic;

namespace Lab_1.Classes
{
    [Serializable]

    public class Person
    {
        private string name;
        private string surname;
        private DateTime birthday;

        public Person(string name, string surname, DateTime birthday)
        {
            this.name = name;
            this.surname = surname;
            this.birthday = birthday;
        }

        public Person()
        {
            this.name = "";
            this.surname = "";
            this.birthday = DateTime.Now;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }
        public string Surname
        {
            get => surname;
            set => surname = value;
        }
        public DateTime Birthday
        {
            get => birthday;
            set => birthday = value;
        }

        public int BirthYear
        {
            get => birthday.Year;
            set => birthday = new DateTime(value, birthday.Month, birthday.Day);
        }

        public override string ToString()
        {
            return "\n - Person name: " + name + "\n - Person surname: " + surname + "\n - DateOfbirthday: " + birthday;
        }

        public virtual string ToShortString()
        {
            return "\nPerson name: " + name + "\nPerson surname: " + surname;
        }

        public override bool Equals(object obj)
        {
            var person = obj as Person;
            if (ReferenceEquals(person, null))
            {
                return false;
            }
            return person.Birthday == Birthday && person.Name == Name && person.Surname == Surname;
        }

        public override int GetHashCode()
        {
            var hashCode = 1430107185;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Surname);
            hashCode = hashCode * -1521134295 + Birthday.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Person p1, Person p2)
        {
            return EqualityComparer<Person>.Default.Equals(p1, p2);
        }

        public static bool operator !=(Person p1, Person p2)
        {
            return !(p1 == p2);
        }

        public virtual object DeepCopy()
        {
            var copy = new Person(Name, Surname, Birthday);
            return copy;
        }
    }
}
