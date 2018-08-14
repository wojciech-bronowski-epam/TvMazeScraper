using System;
using System.Globalization;

namespace TvMazeScraper.Common.Helper
{
    public class DataParse : IDataParse
    {
        private const string DateFormat = "yyyy-MM-dd";

        public DateTime? StringToDate(string stringDate)
        {
            var isDateParsed = DateTime.TryParseExact(stringDate, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime);
            return isDateParsed ? dateTime : (DateTime?)null;
        }

        public string DateToString(DateTime? dateTime)
        {
            return dateTime?.ToString(DateFormat);
        }
    }
}