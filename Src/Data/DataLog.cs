namespace serialog
{
    public class DataEntry
    {
        public DateTime Timestamp { get; }
        public bool IsSent { get; }
        public string DisplayString { get; }

        public DataEntry(DateTime ts, bool isSent, string display)
        {
            Timestamp = ts;
            IsSent = isSent;
            DisplayString = display;
        }

        public override string ToString() =>
            $"[{Timestamp:dd/MM/yyyy HH:mm:ss}] {(IsSent ? "TX" : "")} {DisplayString}";
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
