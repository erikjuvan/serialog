using System.Text;

namespace serialog
{
    public static class FormatHelpers
    {
        public static string BytesToDisplayString(IEnumerable<byte> bytes, bool hideNonPrintable)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                if (b >= 32 && b <= 126)
                    sb.Append((char)b);
                else if (!hideNonPrintable)
                    sb.Append($"{{0x{b:X2}}}");
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
