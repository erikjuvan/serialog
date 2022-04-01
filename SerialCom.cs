using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace serialog
{
    internal class SerialCom
    {
        SerialPort serialPort;

        public SerialCom()
        {
            // Create a new SerialPort object with default settings.
            serialPort = new SerialPort();
        }

        public SerialCom(string port_name, int baud_rate)
        {
            // Create a new SerialPort object with default settings.
            serialPort = new SerialPort();

            // Allow the user to set the appropriate properties.
            serialPort.PortName = port_name;
            serialPort.BaudRate = baud_rate;
            serialPort.Parity = Parity.None;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;
            serialPort.Handshake = Handshake.None;

            // Set the read/write timeouts
            serialPort.ReadTimeout = 100;
            serialPort.WriteTimeout = 100;

            serialPort.Open();
            
        }

        public void Open(string port_name, int baud_rate)
        {
            // Allow the user to set the appropriate properties.
            serialPort.PortName = port_name;
            serialPort.BaudRate = baud_rate;
            serialPort.Parity = Parity.None;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;
            serialPort.Handshake = Handshake.None;

            // Set the read/write timeouts
            serialPort.ReadTimeout = 100;
            serialPort.WriteTimeout = 100;

            serialPort.Open();
        }

        public void Close()
        {
            serialPort.Close();
        }

        ~SerialCom()
        {
            serialPort.Close();
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            return serialPort.Read(buffer, offset, count);
        }

        public string ReadLine()
        {
            return serialPort.ReadLine();
        }
        public string ReadExisting()
        {
            return serialPort.ReadExisting();
        }

        public static string[] GetPortNames()
        {
            return SerialPort.GetPortNames();
        }

        public void SetPortName(string PortName)
        {
            serialPort.PortName = PortName;
        }
        // Display BaudRate values and prompt user to enter a value.
        public void SetPortBaudRate(int PortBaudRate)
        {
            serialPort.BaudRate = PortBaudRate;
        }

        // Display PortParity values and prompt user to enter a value.
        public void SetPortParity(Parity PortParity)
        {
            serialPort.Parity = PortParity;
        }

        // Display DataBits values and prompt user to enter a value.
        public void SetPortDataBits(int PortDataBits)
        {
            serialPort.DataBits = PortDataBits;
        }

        // Display StopBits values and prompt user to enter a value.
        public void SetPortStopBits(StopBits PortStopBits)
        {
            serialPort.StopBits = PortStopBits;
        }
        public void SetPortHandshake(Handshake PortHandshake)
        {
            serialPort.Handshake = PortHandshake;
        }
    }
}
