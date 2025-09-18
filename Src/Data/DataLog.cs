namespace serialog
{
    public enum DataEntryFormat
    {
        Line,
        DateLine,
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

        // Raw entry
        public DataEntry(string line)
        {
            Line = line;
            Format = DataEntryFormat.Line;
        }

        public DataEntry(string line, DateTime timestamp)
        {
            Line = line;
            Timestamp = timestamp;
            Format = DataEntryFormat.DateLine;
        }

        public DataEntry(string line, DateTime timestamp, DataEntrySource source)
        {
            Line = line;
            Timestamp = timestamp;
            Source = source;
            Format = DataEntryFormat.DateSourceLine;
        }

        public override string ToString()
        {
            if (Format == DataEntryFormat.DateSourceLine)
            {
                return $"[{Timestamp:dd/MM/yyyy HH:mm:ss} {Source.ToShortString()}] {Line}";
            }
            else if (Format == DataEntryFormat.Line)
            {
                return Line;
            }
            else // (Format == DataEntryFormat.DateLine)
            {
                return $"[{Timestamp:dd/MM/yyyy HH:mm:ss} {Line}";
            }
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

        public IReadOnlyList<DataEntry> GetSnapshot()
        {
            lock (_lock)
            {
                return _entries.ToList();
            }
        }
    }
}
