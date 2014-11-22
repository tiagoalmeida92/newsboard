using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NewsBoard.Utils
{
    public static class StringUtils
    {
        public static string RemoveDiacritics(this String text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.Normalize(NormalizationForm.FormD);
            char[] chars =
                text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        ///     Util to remove punctuation
        /// </summary>
        public static string RemovePunctuation(this String s)
        {
            s = s.Replace("\n", String.Empty);
            s = s.Replace("\r", String.Empty);
            s = s.Replace("\t", String.Empty);
            var sb = new StringBuilder();

            foreach (char c in s)
            {
                if (!char.IsPunctuation(c))
                    sb.Append(c);
            }

            return sb.ToString();
        }
    }
}