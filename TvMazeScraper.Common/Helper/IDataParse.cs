using System;

namespace TvMazeScraper.Common.Helper
{
    public interface IDataParse
    {
        DateTime? StringToDate(string stringDate);
        string DateToString(DateTime? dateTime);
    }
}