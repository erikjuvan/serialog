using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace serialog
{
    public class LogView
    {
        private readonly List<DataEntry> _visible = new List<DataEntry>();
        public event Action<DataEntry> EntryAdded;

        public void Clear()
        {
            _visible.Clear();
        }

        public void Add(DataEntry entry)
        {
            _visible.Add(entry);
            EntryAdded?.Invoke(entry);
        }

        public IEnumerable<DataEntry> All => _visible;
    }

}
