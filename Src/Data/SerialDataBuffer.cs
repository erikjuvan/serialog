namespace serialog
{
    public class SerialDataBuffer
    {
        private readonly List<byte> _rawBytes = new List<byte>();
        private readonly object _lock = new object();

        public int Count => _rawBytes.Count;

        public void Append(byte[] data, int count)
        {
            lock (_lock)
            {
                for (int i = 0; i < count; i++)
                    _rawBytes.Add(data[i]);
            }
        }

        public byte[] GetSnapshot()
        {
            lock (_lock)
            {
                return _rawBytes.ToArray();
            }
        }
    }

}
