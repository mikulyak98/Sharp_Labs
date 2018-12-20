using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Lab_1.Classes;
using Lab_1.Interfaces;
using Lab_1.People;
using System.IO;
using System.Threading.Tasks;

namespace Lab_1
{
    class Program
    {
        private static readonly Random Random = new Random();

        static void Main(string[] args)
        {
            ConsoleLoop();
        }

        private static void ConsoleLoop()
        {
            Console.OutputEncoding = Encoding.UTF8;
            if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Консоль не працює по неділям!");
                Console.ReadKey();
                return;
            }
            PrintHelp();
            Human first;
            Human second;
            var input = Console.ReadKey();
            while (input.Key != ConsoleKey.F10 && input.Key != ConsoleKey.Q)
            {
                if (input.Key == ConsoleKey.Enter)
                {
                    (first, second) = GeneratePair();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Перша особа: {first}");
                    Console.WriteLine($"Друга особа: {second}");
                    try
                    {                  
                        var (log, result) = Couple(first, second);
                        if (result == null)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        }
                        foreach (var entry in log)
                        {
                            Console.WriteLine(entry);
                        }
                        //process result
                        if (result != null)
                        {
                            PrintResult(result);
                        }
                    }
                    catch (GenderException e)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(e.Message);
                    }
                    Console.ResetColor();
                    Console.WriteLine("\n\n");
                    PrintHelp();
                    input = Console.ReadKey();
                }
            }
        }
        private static void PrintHelp()
        {
            Console.WriteLine("Натисніть Q або F10 щоб закрити консоль.");
            Console.WriteLine("Натисніть Enter щоб вивести наступну пару.");
        }
        private static void PrintResult(IHasName result)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            var resultType = result.GetType();
            Console.WriteLine("Тип результату: " + resultType + "\nІм'я: " + result.Name);
        }

        private static bool CheckLikes(double probability) => Random.NextDouble() < probability;

        private static (IEnumerable<string>, IHasName) Couple(Human h1, Human h2)
        {
            var femCount = 0;
            var firstType = h1.GetType();
            var secondType = h2.GetType();
            if (firstType.Name.Contains("Girl"))
            {
                femCount++;
            }
            if (secondType.Name.Contains("Girl"))
            {
                femCount++;
            }
            if (femCount == 0)
            {
                throw new GenderException("Пара містить лише чоловіків.");
            }
            if (femCount == 2)
            {
                throw new GenderException("Пара містить лише жінок.");
            }
            var firstAttributes = (CoupleAttribute[])Attribute.GetCustomAttributes(firstType, typeof(CoupleAttribute));
            var firstMatch = firstAttributes.FirstOrDefault(attribute => attribute.Pair == secondType.Name);
            var messages = new List<string>();
            if (firstMatch == null)
            {
                messages.Add("Не знайдено підходящий тип нащадку");
                return (messages, null);
            }

            if (CheckLikes(firstMatch.Probability))
            {
                messages.Add("Першій особі подобається друга.");
            }
            else
            {
                messages.Add("Першій особі не подобається друга.");
            }
            var secondAttributes = (CoupleAttribute[])Attribute.GetCustomAttributes(secondType, typeof(CoupleAttribute));
            var secondMatch = secondAttributes.FirstOrDefault(attribute => attribute.Pair == firstType.Name);
            if (secondMatch == null)
            {
                messages.Add("Не знайдено підходящий тип нащадку");
                return (messages, null);
            }
            if (CheckLikes(secondMatch.Probability))
            {
                messages.Add("Другій особі подобається перша.");
            }
            else
            {
                messages.Add("Другій особі не подобається перша.");
            }
            string name;
            try
            {
                name = GetName(h2);
                if (name == null)
                {
                    messages.Add("Не знайдено підходящий метод для імені");
                    return (messages, null);
                }
            }
            catch (TargetParameterCountException)
            {
                messages.Add("Неправильна кількість аргументів.");
                return (messages, null);
            }
            var child = CreateChild(firstMatch.ChildType, name);
            if (child == null)
            {
                messages.Add("Не знайдено сеттеру для імені.");
                return (messages, null);
            }
            var parName = h1.Name;
            if (firstType.Name.Contains("Girl"))
            {
                parName = GenerateParName(name);
            }
            SetSurname(child, parName);
            return (messages, child);
        }

        private static Human GenerateMale()
        {
            var value = Random.NextDouble();
            Human result;
            if (value < 0.5)
            {
                result = new Botan();
            }
            else
            {
                result = new Student();
            }
            result.Name = RandomString(Random.Next(5, 15));
            return result;
        }

        private static Human GenerateFemale()
        {
            var value = Random.NextDouble();
            Human result;
            if (value < 0.33)
            {
                result = new Girl();
            }
            else if (value < 0.66)
            {
                result = new PrettyGirl();
            }
            else
            {
                result = new SmartGirl();
            }
            result.Name = RandomString(Random.Next(5, 15));
            return result;
        }

        private static (Human, Human) GeneratePair()
        {
            Human h1, h2;
            if (Random.NextDouble() < 0.75)
            {
                h1 = GenerateMale();
            }
            else
            {
                h1 = GenerateFemale();
            }
            if (Random.NextDouble() < 0.75)
            {
                h2 = GenerateFemale();
            }
            else
            {
                h2 = GenerateMale();
            }
            return (h1, h2);
        }

        private static IHasName CreateChild(string childTypeName, string name)
        {
            var childType = Type.GetType($"Lab_1.People.{childTypeName}");
            var child = (IHasName)Activator.CreateInstance(childType);
            var nameProp = childType.GetProperty("Name");
            if (nameProp.CanWrite)
            {
                nameProp.SetValue(child, name);
            }
            else
            {
                return null;
            }
                return child;
        }

        private static void SetSurname(IHasName child, string name)
        {
            var parName = child.GetType().GetProperty("Surname");
            if (parName != null && parName.CanWrite)
            {
                parName.SetValue(child, name);
            }
        }

        private static string GetName(Human human)
        {
            var method = human.GetType().GetMethods().FirstOrDefault(m => m.ReturnType == typeof(string));
            if (method == null)
            {
                return null;
            }
            return (method.Invoke(human, null) as string);
        }

        private static string RandomString(int length)
        {
            const string chars = "абвгдеєжзиіїйклмнопрстуфхцчшщюя";
            var a = char.ToUpper(chars[Random.Next(chars.Length)]);
            return a + new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
        private static string GenerateParName(string name) => name + "овна";
    }
}
