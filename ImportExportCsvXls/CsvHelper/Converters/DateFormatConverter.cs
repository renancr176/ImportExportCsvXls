using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace ImportExportCsvXls.CsvHelper.Converters;

public class DateFormatConverter : DateTimeConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (DateTime.TryParse(text, out var date))
            text = date.ToString("O");

        return base.ConvertFromString(text, row, memberMapData);
    }
}