namespace ns_Extension_methods
{
    internal static class StringExtensionMethods
    {
        public static string Unique(this string str, IEnumerable<string> strings)
        {
            if (!strings.Contains(str))
            {
                return str;
            }
            int strNum;
            int strNumInd = str.Length - 1;
            bool flag = false;
            do
            {
                if (!int.TryParse(str[strNumInd..], out strNum))
                {
                    flag = true;
                    break;
                }
                strNumInd--;
            }
            while (strNumInd >= 0);
            if (!flag)
            {
                strNum = 2;
            }
            while(strings.Contains($"{str}{strNum}"))
            {
                strNum++;
            }
            return $"{str}{strNum}";
        }
    }
}