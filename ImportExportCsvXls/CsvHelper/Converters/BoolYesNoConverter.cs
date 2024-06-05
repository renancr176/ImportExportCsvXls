using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace ImportExportCsvXls.CsvHelper.Converters;

public class BoolYesNoConverter : BooleanConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        switch (text.ToLower())
        {
            case "yes":
            case "1":
                text = "true";
                break;
            case "no":
            case "0":
                text = "false"; 
                break;
        }

        return base.ConvertFromString(text, row, memberMapData);
    }

    public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
    {
        switch ((bool) value)
        {
            case true:
                return "Yes";
            default:
                return "No";
        }
    }
}