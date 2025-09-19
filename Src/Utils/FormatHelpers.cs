using System.Text;

namespace serialog
{
    public static class FormatHelpers
    {
        public static string BytesToDisplayString(IEnumerable<byte> bytes, bool allAsHex = false, bool nonPrintableAsHex = false)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                if (allAsHex)
                    sb.Append($"\\x{(int)b:X2}");
                else if (nonPrintableAsHex)
                {
                    if (b >= 32 && b <= 126)
                        sb.Append((char)b);
                    else
                        sb.Append($"\\x{b:X2}");
                }
                else
                {
                    if (b >= 32 && b <= 126)
                        sb.Append((char)b);
                    else
                        sb.Append((char)(0x2400 + b));
                }                
            }
            return sb.ToString();
        }

        public static string NumberToBKBMB(double num, string suffix = "")
        {
            string str;

            if (num > 1024.0 * 1024.0)
            {
                num /= 1024.0 * 1024.0;
                str = num.ToString("0.00") + " MB" + suffix;
            }
            else if (num > 1024.0)
            {
                num /= 1024.0;
                str = num.ToString("0.00") + " KB" + suffix;
            }
            else
            {
                str = Convert.ToInt32(num).ToString() + " B" + suffix;
            }

            return str;
        }
    }
}
