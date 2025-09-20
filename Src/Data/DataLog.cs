namespace serialog
{
    public enum DataEntryFormat
    {
        Line,
        DateLine,
        SourceLine,
        DateSourceLine
    }

    public enum DataEntrySource
    {
        SerialRX,
        SerialTX,
        File,
        User
    }

    public static class DataEntrySourceExtensions
    {
        public static string ToShortString(this DataEntrySource source)
        {
            return source switch
            {
                DataEntrySource.SerialRX => "RX",
                DataEntrySource.SerialTX => "TX",
                DataEntrySource.File => "F",
                DataEntrySource.User => "U",
                _ => "?"
            };
        }
    }

    public class DataEntry
    {
        public string Line { get; }
        public DateTime Timestamp { get; }
        public DataEntrySource Source { get; }
        public DataEntryFormat Format { get; }
        private string toString { get; }

        // Raw entry
        public DataEntry(string line)
        {
            Line = line;
            Format = DataEntryFormat.Line;
            toString = line;
        }

        public DataEntry(DateTime timestamp, string line)
        {
            Timestamp = timestamp;
            Line = line;
            Format = DataEntryFormat.DateLine;
            toString = $"[{Timestamp:dd/MM/yyyy HH:mm:ss}] {Line}";
        }

        public DataEntry(DataEntrySource source, string line)
        {
            Source = source;
            Line = line;
            Format = DataEntryFormat.SourceLine;
            toString = $"[{Source.ToShortString()}] {Line}";
        }

        public DataEntry(DateTime timestamp, DataEntrySource source, string line)
        {
            Timestamp = timestamp;
            Source = source;
            Line = line;
            Format = DataEntryFormat.DateSourceLine;
            toString = $"[{Timestamp:dd/MM/yyyy HH:mm:ss} {Source.ToShortString()}] {Line}";
        }

        public override string ToString()
        {
            return toString;
        }

        public string ToSearchString()
        {
            return Line;
        }
    }

    public class DataLog
    {
        private readonly List<DataEntry> _entries = new List<DataEntry>();
        private readonly object _lock = new object();

        public void Add(DataEntry entry)
        {
            lock (_lock)
            {
                _entries.Add(entry);
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _entries.Clear();
            }
        }


        public IReadOnlyList<DataEntry> GetSnapshot()
        {
            lock (_lock)
            {
                return _entries.ToList();
            }
        }

        public int Count
        {
            get
            {
                lock (_lock)
                {
                    return _entries.Count;
                }
            }
        }

        public DataEntry this[int index]
        {
            get
            {
                lock (_lock)
                {
                    return _entries[index];
                }
            }
        }

        public void RemoveAt(int index)
        {
            lock (_lock)
            {
                _entries.RemoveAt(index);
            }
        }
    }
}
