using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Text.RegularExpressions;

namespace ImportExportCsvXls.CsvHelper.Converters;

public class CurrencyConverter : DecimalConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (text.LastIndexOf(",") == (text.Length - 3))
        {
            var stringBuilder = new StringBuilder();
            var splitedString = Regex.Replace(text, @"\D", "").ToCharArray().ToList();

            var index = 0;
            foreach (var s in splitedString)
            {
                if (index == (text.Length - 3))
                    stringBuilder.Append(".");
                stringBuilder.Append(s);
                index++;
            }

            text = stringBuilder.ToString();
        }

        return base.ConvertFromString(text, row, memberMapData);
    }

    public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
    {
        value = Math.Round((decimal)value, 2);

        return base.ConvertToString(value, row, memberMapData);
    }
}