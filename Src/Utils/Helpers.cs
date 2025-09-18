using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace serialog
{
    public static class Helpers
    {
        public static string[] GetSortedPorts()
        {
            var portNames = System.IO.Ports.SerialPort.GetPortNames();

            Array.Sort(portNames, (x, y) =>
            {
                int xNum = 0, yNum = 0;

                bool xIsCom = x.StartsWith("COM", StringComparison.OrdinalIgnoreCase) &&
                              int.TryParse(x.Substring(3), out xNum);

                bool yIsCom = y.StartsWith("COM", StringComparison.OrdinalIgnoreCase) &&
                              int.TryParse(y.Substring(3), out yNum);

                if (xIsCom && yIsCom)
                {
                    return xNum.CompareTo(yNum); // sort numerically
                }
                else if (xIsCom)
                {
                    return -1; // COM ports first
                }
                else if (yIsCom)
                {
                    return 1;
                }
                else
                {
                    return string.Compare(x, y, StringComparison.OrdinalIgnoreCase);
                }
            });

            return portNames;
        }
    }
}
