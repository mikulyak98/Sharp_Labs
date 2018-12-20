using System;

namespace Lab_1.Classes
{
    public class MagazineListHandlerEventArgs: EventArgs
    {
        public MagazineListHandlerEventArgs(string _name_of_collection, string _type_of_change, int _changed_element)
        {
            name_of_collection = _name_of_collection;
            type_of_change = _type_of_change;
            changed_element = _changed_element;
        }

        public string name_of_collection { get; set; }
        public string type_of_change { get; set; }
        public int changed_element { get; set; }

        public override string ToString()
        {
            return "Name collection: " + name_of_collection + "\nType change: " + type_of_change + "\nChanged element: " + changed_element;
        }
    }
}
