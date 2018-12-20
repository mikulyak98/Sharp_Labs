using System;
using System.Collections.Generic;
using System.Text;
using Lab_1.Interfaces;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Globalization;

namespace Lab_1.Classes
{
    [Serializable]
    public class Magazine: Edition, IRateAndCopy<Magazine>
    { 
        private Data.Frequency periodicity;
        private List<Article> list_of_articles;
        public List<Person> Editors { get; set; }

        public Magazine(string name_of_edition, Data.Frequency periodicity, DateTime publication_date_edition, int printing, List<Article> Articles, List<Person> Editors): base(name_of_edition, publication_date_edition, printing)
        {
            this.list_of_articles = Articles;
            this.Editors = Editors;
        }

        public Magazine():base()
        {
            this.list_of_articles = new List<Article>();
            this.Editors = new List<Person>();
        }

        public Data.Frequency Periodicity
        {
            get => periodicity;
            set => periodicity = value;
        }
        public List<Article> Articles1
        {
            get => list_of_articles;
            set => list_of_articles = value;
        }
        
        public double MediumRating
        {
            get
            {
                if (list_of_articles.Count == 0)
                    return 0;
                var sum = .0;
                foreach(var article in list_of_articles)
                {
                    sum += ((Article)article).rating_of_article;
                }
                return sum / list_of_articles.Count;
            }
        }
        public double rating_of_article => MediumRating;
        public bool this[Data.Frequency frequency] => periodicity == frequency;

        public void AddArticles(params Article[] articles)
        {
            this.list_of_articles.AddRange(articles);
        }
        public void AddEditors(params Person[] editors)
        {
            Editors.AddRange(editors);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var article in list_of_articles)
            {
                sb.Append("Article: " + article + "\n\n");
            }
            return "Name of magazine: " + name_of_edition + "\nPeriodicity: " + periodicity + "\nDate of issue: " + publication_date_edition +
                "\nPrinting of magazine: " + printing + "\nList of article:\n" + sb;
        }

        public virtual string ToShortString()
        {
            return "Name of magazine: " + name_of_edition + "\nPeriodicity: " + periodicity + "\nDate of issue: " + publication_date_edition +
                "\nPrinting of magazine: " + printing + "\nMedium rating: " + MediumRating;
        }

        public override bool Equals(object obj)
        {
            var magazine = obj as Magazine;
            if (ReferenceEquals(magazine, null))
            {
                return false;
            }
            return name_of_edition == magazine.name_of_edition &&
                    periodicity == magazine.periodicity &&
                    printing == magazine.printing &&
                    publication_date_edition == magazine.publication_date_edition;
        }
        public static bool operator ==(Magazine m1, Magazine m2)
        {
            return EqualityComparer<Magazine>.Default.Equals(m1, m2);
        }
        public static bool operator !=(Magazine m1, Magazine m2)
        {
            return !(m1 == m2);
        }
        public override int GetHashCode()
        {
            var hashCode = -703031920;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Data.Frequency>.Default.GetHashCode(periodicity);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Article>>.Default.GetHashCode(list_of_articles);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Person>>.Default.GetHashCode(Editors);
            return hashCode;
        }

        public Edition Edition
        {
            get => this;
            set
            {
                Name_of_edition = value.Name_of_edition;
                Printing = value.Printing;
                Publication_date_edition = value.Publication_date_edition;
            }
        }

        public Magazine DeepCopy()
        {
            Magazine result;
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Position = 0;
                result = formatter.Deserialize(stream) as Magazine;
            }
            return result;
        }
        public IEnumerable<Article> GetRatings(double minRating)
        {
            foreach (var o in list_of_articles)
            {
                var article = o as Article;
                if (article == null || article.rating_of_article < minRating)
                {
                    continue;
                }
                yield return article;
            }
        }
        public IEnumerable<Article> GetArticlesByName(string name)
        {
            foreach (var o in list_of_articles)
            {
                var article = o as Article;
                if (article == null || !article.name_of_article.Contains(name))
                {
                    continue;
                }
                yield return article;
            }
        }

        public bool Save(string filename)
        {
            using (var stream = File.Open(filename, FileMode.Create))
            {
                try
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(stream, this);
                    return true;
                }
                catch(Exception ex)
                {
                    return false;
                }
            }
        }

        public bool Load(string filename)
        {
            using (var stream = File.Open(filename, FileMode.Open))
            {
                try
                {
                    var formatter = new BinaryFormatter();
                    var result = formatter.Deserialize(stream) as Magazine;
                    if (result == null)
                    {
                        return false;
                    }
                    Edition = result.Edition;
                    Editors = result.Editors;
                    Periodicity = result.Periodicity;
                    list_of_articles = result.list_of_articles;
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public bool AddFromConsole()
        {
            try
            {
                Console.WriteLine("String text:\n" + "<article_name>;<author_name> <author_surname> <dd.mm.yyyy date>;<rating.rating>");
                var input = Console.ReadLine().Split(';');
                if (input.Length != 3)
                {
                    return false;
                }
                var personData = input[1].Split(' ');
                if (personData.Length != 3)
                {
                    return false;
                }
                var date = DateTime.ParseExact(personData[2], "dd.mm.yyyy", CultureInfo.InvariantCulture);
                var rating = double.Parse(input[2], CultureInfo.InvariantCulture);
                var author = new Person(personData[0], personData[1], date);
                var article = new Article(author, input[0], rating);
                list_of_articles.Add(article);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Save(string filename, Magazine magazine)
        {
            return magazine.Save(filename);
        }
        public static bool Load(string filename, Magazine magazine)
        {
            return magazine.Load(filename);
        }
    }
}
