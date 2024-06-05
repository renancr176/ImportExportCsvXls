using CsvHelper.Configuration;
using System.Globalization;

namespace ImportExportCsvXls.CsvHelper;

public class CsvHelperDefaults
{
    public static CsvConfiguration DefaultCsvConfiguration => new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        NewLine = Environment.NewLine,
        Delimiter = ";"
    };
}