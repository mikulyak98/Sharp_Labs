using System;
using System.Collections.Generic;
using Lab_1.Interfaces;

namespace Lab_1.Classes
{
    [Serializable]

    public class Article: IRateAndCopy<Article>
    {
        public Person author { get; set; }
        public string name_of_article { get; set; }
        public double rating_of_article { get; set; }

        public Article(Person author, string name_of_article, double rating_of_article)
        {
            this.author = author;
            this.name_of_article = name_of_article;
            this.rating_of_article = rating_of_article;
        }

        public Article()
        {
            this.author = new Person();
            this.name_of_article = "";
            this.rating_of_article = 0;
        }

        public override string ToString()
        {
            return "\nDate of author: " + author.ToString() + "\nArticle title: " + name_of_article + "\nRating of article: " + rating_of_article;
        }

        public override bool Equals(object obj)
        {
            var article = obj as Article;
            if (ReferenceEquals(article, null))
            {
                return false;
            }
            return author.Equals(article.author) && name_of_article == article.name_of_article && rating_of_article == article.rating_of_article;
        }
        public override int GetHashCode()
        {
            var hashCode = 1663829154;
            hashCode = hashCode * -1521134295 + EqualityComparer<Person>.Default.GetHashCode(author);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name_of_article);
            hashCode = hashCode * -1521134295 + rating_of_article.GetHashCode();
            return hashCode;
        }
        public static bool operator ==(Article a1, Article a2)
        {
            return EqualityComparer<Article>.Default.Equals(a1, a2);
        }
        public static bool operator !=(Article a1, Article a2)
        {
            return !(a1 == a2);
        }
        public Article DeepCopy()
        {
            var copy = new Article(author.DeepCopy() as Person, name_of_article, rating_of_article);
            return copy;
        }
    }
}
