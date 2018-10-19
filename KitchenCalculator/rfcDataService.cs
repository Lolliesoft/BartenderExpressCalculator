namespace KitchenCalculator
{
    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    internal class rfcDataService
    {
        private static NumberFormatInfo ioEuropeanNumberFormat;
        private static NumberFormatInfo ioUSNumberFormat = new NumberFormatInfo();

        static rfcDataService()
        {
            ioUSNumberFormat.NumberDecimalSeparator = ".";
            ioEuropeanNumberFormat = new NumberFormatInfo();
            ioEuropeanNumberFormat.NumberDecimalSeparator = ",";
        }

        public static string FormatDecimal(decimal adcNumber, int aiDecimalPlaces)
        {
            return FormatDecimal(adcNumber, aiDecimalPlaces, false, true);
        }

        public static string FormatDecimal(string asNumber, int aiDecimalPlaces)
        {
            string str = "";
            try
            {
                str = FormatDecimal(NotNullDecimal(asNumber), aiDecimalPlaces, false, true);
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
            return str;
        }

        public static string FormatDecimal(decimal adcNumber, int aiDecimalPlaces, bool abNegativeIsNull)
        {
            return FormatDecimal(adcNumber, aiDecimalPlaces, abNegativeIsNull, true);
        }

        public static string FormatDecimal(decimal adcNumber, int aiDecimalPlaces, bool abNegativeIsNull, bool abRemoveTrailingZeroes)
        {
            string str = "";
            try
            {
                NumberFormatInfo provider = (NumberFormatInfo) NumberFormatInfo.CurrentInfo.Clone();
                provider.NumberDecimalDigits = aiDecimalPlaces;
                provider.NumberGroupSeparator = "";
                if (abRemoveTrailingZeroes)
                {
                    return RemoveTrailingZeroes(adcNumber.ToString("N", provider), true, abNegativeIsNull);
                }
                str = adcNumber.ToString("N", provider);
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
            return str;
        }

        private static void HandleSystemError(Exception aoException)
        {
            MessageBox.Show(aoException.Message);
        }

        public static bool NotNullBoolean(object aoValue)
        {
            if (Convert.IsDBNull(aoValue))
            {
                return false;
            }
            if (aoValue == null)
            {
                return false;
            }
            if ((!(aoValue.ToString().ToLower() == "y") && !(aoValue.ToString().ToLower() == "yes")) && !(aoValue.ToString().ToLower() == "true"))
            {
                return false;
            }
            return true;
        }

        public static decimal NotNullDecimal(object aoValue)
        {
            if (Convert.IsDBNull(aoValue))
            {
                return 0M;
            }
            if (aoValue == null)
            {
                return 0M;
            }
            if (aoValue.ToString() == "")
            {
                return 0M;
            }
            if (aoValue.ToString().IndexOf('.') >= 0)
            {
                return Convert.ToDecimal(aoValue, ioUSNumberFormat);
            }
            if (aoValue.ToString().IndexOf(',') >= 0)
            {
                return Convert.ToDecimal(aoValue, ioEuropeanNumberFormat);
            }
            return Convert.ToDecimal(aoValue);
        }

        public static decimal NotNullDecimal(object aoValue, bool abNullIsNegative)
        {
            if (Convert.IsDBNull(aoValue))
            {
                if (abNullIsNegative)
                {
                    return -1M;
                }
                return 0M;
            }
            if (aoValue == null)
            {
                if (abNullIsNegative)
                {
                    return -1M;
                }
                return 0M;
            }
            if (aoValue.ToString() == "")
            {
                if (abNullIsNegative)
                {
                    return -1M;
                }
                return 0M;
            }
            if (aoValue.ToString().IndexOf('.') >= 0)
            {
                return Convert.ToDecimal(aoValue, ioUSNumberFormat);
            }
            if (aoValue.ToString().IndexOf(',') >= 0)
            {
                return Convert.ToDecimal(aoValue, ioEuropeanNumberFormat);
            }
            return Convert.ToDecimal(aoValue);
        }

        public static string NotNullString(object aoValue)
        {
            if (Convert.IsDBNull(aoValue))
            {
                return "";
            }
            if (aoValue == null)
            {
                return "";
            }
            if (aoValue is DateTime)
            {
                DateTime time = (DateTime) aoValue;
                return time.ToString("yyyy-MM-dd");
            }
            return aoValue.ToString();
        }

        public static void ParseNumbersAndText(string asSource, out decimal adcNumber, out string asText)
        {
            decimal num = 0M;
            string str = "";
            string aoValue = "";
            try
            {
                string s = asSource.Trim();
                for (int i = 0; i != s.Length; i++)
                {
                    string str3 = s.Substring(i, 1);
                    if ((char.IsNumber(s, i) || (str3 == ".")) || (str3 == ","))
                    {
                        aoValue = aoValue + str3;
                    }
                    else
                    {
                        str = s.Substring(i).Trim();
                        break;
                    }
                }
                num = NotNullDecimal(aoValue);
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
            adcNumber = num;
            asText = str;
        }

        public static string RemoveTrailingZeroes(string asNumber, bool abRemoveDecimal)
        {
            string str = "";
            int num = 0;
            bool flag = false;
            try
            {
                for (int i = asNumber.Length - 1; i > 0; i--)
                {
                    if (asNumber.Substring(i, 1) != "0")
                    {
                        num = i;
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    return asNumber;
                }
                str = asNumber.Substring(0, num + 1);
                if (abRemoveDecimal && (str.Substring(str.Length - 1, 1) == NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))
                {
                    str = str.Substring(0, str.Length - 1);
                }
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
            return str;
        }

        public static string RemoveTrailingZeroes(string asNumber, bool abRemoveDecimal, bool abNegativeIsNull)
        {
            string str = "";
            int num = 0;
            bool flag = false;
            try
            {
                if (asNumber == "")
                {
                    return "";
                }
                if (abNegativeIsNull && (asNumber.Substring(0, 1) == "-"))
                {
                    return "";
                }
                for (int i = asNumber.Length - 1; i > 0; i--)
                {
                    if (asNumber.Substring(i, 1) != "0")
                    {
                        num = i;
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    return asNumber;
                }
                str = asNumber.Substring(0, num + 1);
                if (abRemoveDecimal && (str.Substring(str.Length - 1, 1) == NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))
                {
                    str = str.Substring(0, str.Length - 1);
                }
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
            return str;
        }

        public static string ToYesNo(bool abValue)
        {
            if (abValue)
            {
                return "Yes";
            }
            return "No";
        }
    }
}

