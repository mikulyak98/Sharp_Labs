using System.Collections.Generic;
using System.Text;

namespace Lab_1.Classes
{
    public class Listener
    {
        public Listener()
        {
            changes = new List<ListEntry>();
        }
        public List<ListEntry> changes { get; }

        public void Handler(object sourse, MagazineListHandlerEventArgs args)
        {
            changes.Add(new ListEntry(args.name_of_collection, args.type_of_change, args.changed_element));
        }

        public override string ToString()
        {
            var sb = new StringBuilder("All changes: " + changes.Count + "\n");
            for(var i = 0; i < changes.Count; i++)
            {
                sb.Append(changes[i] + "\n");
            }
            return sb.ToString();
        }
    }
}
