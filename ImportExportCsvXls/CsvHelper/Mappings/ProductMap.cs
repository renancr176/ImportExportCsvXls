using CsvHelper.Configuration;
using ImportExportCsvXls.CsvHelper.Converters;
using ImportExportCsvXls.Models;

namespace ImportExportCsvXls.CsvHelper.Mappings;

public class ProductMap : ClassMap<ProductModel>
{
    public ProductMap()
    {
        Map(p => p.Id).Name("Id");
        Map(p => p.Name).Name("Name");
        Map(p => p.Price).Name("Price")
            .TypeConverter<CurrencyConverter>();
        Map(p => p.Active).Name("Active")
            .TypeConverter<BoolYesNoConverter>();
        Map(p => p.Quantity).Name("Quantity");
        Map(p => p.CreatedAt).Name("Creation Date")
            .TypeConverter<DateFormatConverter>();
    }
}