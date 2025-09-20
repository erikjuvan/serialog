using System.Text.RegularExpressions;

namespace serialog
{
    public enum DataEntrySource
    {
        SerialRX,
        SerialTX,
        User, 
    }

    public static class DataEntrySourceExtensions
    {
        public static string ToShortString(this DataEntrySource source)
        {
            return source switch
            {
                DataEntrySource.SerialRX => "RX",
                DataEntrySource.SerialTX => "TX",
                DataEntrySource.User => "US",
                _ => "?"
            };
        }

        public static bool TryParseShortString(string text, out DataEntrySource source)
        {
            switch (text?.Trim().ToUpperInvariant())
            {
                case "RX":
                    source = DataEntrySource.SerialRX;
                    return true;
                case "TX":
                    source = DataEntrySource.SerialTX;
                    return true;
                case "US":
                    source = DataEntrySource.User;
                    return true;
                default:
                    source = default;
                    return false;
            }
        }
    }

    public class DataEntry
    {
        public string Content { get; }
        public DateTime? Timestamp { get; }
        public DataEntrySource? Source { get; }

        private string _formattedLine;

        // Computed once on first access, then cached
        public string FormattedLine => _formattedLine ??= (Timestamp, Source) switch
        {
            (DateTime ts, DataEntrySource s) => $"[{ts:dd/MM/yyyy HH:mm:ss} {s.ToShortString()}] {Content}",
            (DateTime ts, null) => $"[{ts:dd/MM/yyyy HH:mm:ss}] {Content}",
            (null, DataEntrySource s) => $"[{s.ToShortString()}] {Content}",
            _ => Content
        };

        public DataEntry(string content)
        {
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }

        public DataEntry(DateTime timestamp, string content)
        {
            Timestamp = timestamp;
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }

        public DataEntry(DataEntrySource source, string content)
        {
            Source = source;
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }

        public DataEntry(DateTime timestamp, DataEntrySource source, string content)
        {
            Timestamp = timestamp;
            Source = source;
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }

        public override string ToString() => FormattedLine;

        public string GetContent() => Content;
    }

    public static class DataEntryParser
    {
        // Matches formats like:
        // [12/09/2025 14:32:10] message
        // [12/09/2025 14:32:10 SRC] message
        // [SRC] message
        private static readonly Regex DateSourceRegex =
            new Regex(@"^\[(?<date>\d{2}/\d{2}/\d{4}\s+\d{2}:\d{2}:\d{2})(?:\s+(?<source>[A-Za-z0-9_]+))?\]\s*(?<line>.*)$",
                      RegexOptions.Compiled);

        private static readonly Regex SourceRegex =
            new Regex(@"^\[(?<source>[^\]]+)\]\s*(?<line>.*)$", RegexOptions.Compiled);

        public static bool TryParse(string input, out DataEntry entry)
        {
            entry = null;

            // Try [date [source]] line
            var match = DateSourceRegex.Match(input);
            if (match.Success)
            {
                if (DateTime.TryParse(match.Groups["date"].Value, out var timestamp))
                {
                    var sourceText = match.Groups["source"].Value;
                    var line = match.Groups["line"].Value;


                    if (!string.IsNullOrEmpty(sourceText) &&
                        DataEntrySourceExtensions.TryParseShortString(sourceText, out var source))
                    {
                        entry = new DataEntry(timestamp, source, line);
                    }
                    else
                    {
                        entry = new DataEntry(timestamp, line);
                    }
                    return true;
                }
            }

            // Try [source] line
            match = SourceRegex.Match(input);
            if (match.Success)
            {
                var sourceText = match.Groups["source"].Value;
                var line = match.Groups["line"].Value;

                if (Enum.TryParse<DataEntrySource>(sourceText, true, out var source))
                {
                    entry = new DataEntry(source, line);
                    return true;
                }
            }

            // Fallback: just line
            entry = new DataEntry(input);
            return true;
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
