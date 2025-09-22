namespace serialog
{
    public class DataParser
    {
        private readonly List<byte> _lineBuffer = new List<byte>();

        public event Action<byte[]> LineParsed;

        public void Feed(ReadOnlySpan<byte> data)
        {
            foreach (var b in data)
            {
                if (b == (byte)'\n')
                {
                    if (_lineBuffer.Count > 0 && _lineBuffer[^1] == (byte)'\r')
                        _lineBuffer.RemoveAt(_lineBuffer.Count - 1);

                    var line = _lineBuffer.ToArray();
                    LineParsed?.Invoke(line);
                    _lineBuffer.Clear();
                }
                else
                {
                    _lineBuffer.Add(b);
                }
            }
        }
    }
}
