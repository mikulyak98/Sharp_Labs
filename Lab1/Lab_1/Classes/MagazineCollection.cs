using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab_1.Classes
{
    public delegate void MagazineListHandler(object source, MagazineListHandlerEventArgs args);

    public class MagazineCollection
    {
        public string name_of_collection { get; set; }
        public event MagazineListHandler MagazineAdded;
        public event MagazineListHandler MagazineReplaced;

        private readonly List<Magazine> _magazines = new List<Magazine>();

        public void AddDefaults()
        {
            var res = MagazineAdded;
            for(var i = 0; i < 10; i++)
            {
                _magazines.Add(new Magazine());
                res?.Invoke(this, new MagazineListHandlerEventArgs(name_of_collection, "Added", _magazines.Count));
            }
        }
        public void AddMagazines(params Magazine[] magazines)
        {
            var res = MagazineAdded;
            foreach(var magazine in magazines)
            {
                _magazines.Add(magazine);
                res?.Invoke(this, new MagazineListHandlerEventArgs(name_of_collection, "Added", _magazines.Count));
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach(var article in _magazines)
            {
                sb.Append(article.ToString());
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        public string ToShortString()
        {
            var sb = new StringBuilder();
            foreach (var article in _magazines)
            {
                sb.Append(article.ToShortString());
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        public void SortByName()
        {
            _magazines.Sort(new Edition());
        }
        public void SortByDatePublication()
        {
            _magazines.Sort(new Edition());
        }
        public void SortByPrinting()
        {
            _magazines.Sort(new EditionPrintingComparer());
        }

        public double MaxRating => _magazines.DefaultIfEmpty(new Magazine()).Max(m => m.MediumRating);
        public IEnumerable<Magazine> MonthlyMagazines => _magazines.DefaultIfEmpty(new Magazine { Periodicity = Data.Frequency.Montly }).Where(m => m.Periodicity == Data.Frequency.Montly);
        public List<Magazine> RatingGroup(double value)
        {
            if(_magazines.Count == 0)
            {
                return null;
            }
            return _magazines.GroupBy(m => m.MediumRating >= value).Where(g => g.Key == true).SelectMany(g => g).ToList();
        }

        public bool Replace(int j, Magazine magazine)
        {
            if(j < 0 || j >= _magazines.Count)
            {
                return false;
            }
            var res = MagazineReplaced;
            _magazines[j] = magazine;
            res?.Invoke(this, new MagazineListHandlerEventArgs(name_of_collection, "Replaced", j));
            return true;
        }

        public Magazine this[int index]
        {
            get
            {
                if(index < 0 || index >= _magazines.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                return _magazines[index];
            }
            set
            {
                if (index < 0 || index >= _magazines.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                var res = MagazineReplaced;
                _magazines[index] = value;
                res?.Invoke(this, new MagazineListHandlerEventArgs(name_of_collection, "Replaced", index));
            }
        }
    }
}
