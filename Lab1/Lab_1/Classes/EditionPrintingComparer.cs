using System.Collections.Generic;

namespace Lab_1.Classes
{
    public class EditionPrintingComparer : IComparer<Edition>
    {
        public int Compare(Edition x, Edition y)
        {
            if (x == null)
            {
                return -1;
            }
            if (y == null)
            {
                return 1;
            }
            return x.Printing.CompareTo(y.Printing);
        }
    }
}
