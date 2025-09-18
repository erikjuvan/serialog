using System.IO.Ports;
using System.Collections.Concurrent;

namespace serialog
{
    public class SerialReader
    {
        private readonly SerialPort _serial;
        private readonly SerialDataBuffer _buffer;
        private readonly DataParser _parser;
        private readonly BlockingCollection<byte[]> _queue = new();
        private readonly Thread _worker;

        public SerialReader(SerialPort serial, SerialDataBuffer buffer, DataParser parser)
        {
            _serial = serial;
            _buffer = buffer;
            _parser = parser;

            _serial.DataReceived += OnDataReceived;

            _worker = new Thread(ProcessLoop) { IsBackground = true };
            _worker.Start();
        }

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int count = _serial.BytesToRead;
            var buf = new byte[count];
            _serial.Read(buf, 0, count);

            // Save raw bytes immediately
            _buffer.Append(buf, count);

            // Push to worker for parsing
            _queue.Add(buf);
        }

        private void ProcessLoop()
        {
            foreach (var buf in _queue.GetConsumingEnumerable())
            {
                _parser.Feed(buf);
            }
        }

        public void Stop() => _queue.CompleteAdding();
    }
}
