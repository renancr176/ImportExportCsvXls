using Bogus;
using CsvHelper;
using ImportExportCsvXls.CsvHelper;
using ImportExportCsvXls.Extensions;
using ImportExportCsvXls.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using ImportExportCsvXls.CsvHelper.Mappings;
using CsvHelper.Excel;

namespace ImportExportCsvXls.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        public Faker Faker { get; private set; }

        public ProductController()
        {
            Faker = new Faker();
        }

        [HttpGet("Csv")]
        public async Task<IActionResult> GetCsvAsync()
        {
            var file = new FileModel()
            {
                FileName = $"{Path.GetTempPath()}{($"Products_{DateTime.Now.ToString("g")}.csv").NormalizeFileName()}",
                ContentType = "text/csv"
            };

            try
            {
                var products = GenerateProducts(Faker.Random.Int(10, 1000));

                using (var writer = new StreamWriter(file.FileName))
                using (var csv = new CsvWriter(writer, CsvHelperDefaults.DefaultCsvConfiguration))
                {
                    csv.Context.RegisterClassMap<ProductMap>();

                    csv.WriteHeader<ProductModel>();
                    csv.NextRecord();

                    foreach (var product in products)
                    {
                        csv.WriteRecord(product);
                        csv.NextRecord();
                    }
                }

                return File(System.IO.File.ReadAllBytes(file.FileName), file.ContentType, Path.GetFileName(file.FileName));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally {
                if (System.IO.File.Exists(file.FileName))
                {
                    System.IO.File.Delete(file.FileName);
                }
            }
        }

        [HttpGet("Xls")]
        public async Task<IActionResult> GetXlsAsync()
        {
            var file = new FileModel()
            {
                FileName = $"{Path.GetTempPath()}{($"Products_{DateTime.Now.ToString("g")}.xlsx").NormalizeFileName()}",
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            };

            try
            {
                var products = GenerateProducts(Faker.Random.Int(10, 1000));

                using (var csv = new ExcelWriter(file.FileName, CsvHelperDefaults.DefaultCsvConfiguration.CultureInfo))
                {
                    csv.Context.RegisterClassMap<ProductMap>();

                    csv.WriteHeader<ProductModel>();
                    csv.NextRecord();

                    foreach (var product in products)
                    {
                        csv.WriteRecord(product);
                        csv.NextRecord();
                    }
                }

                return File(System.IO.File.ReadAllBytes(file.FileName), file.ContentType, Path.GetFileName(file.FileName));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally
            {
                if (System.IO.File.Exists($"{Path.GetTempPath()}{file.FileName}"))
                {
                    System.IO.File.Delete($"{Path.GetTempPath()}{file.FileName}");
                }
            }
        }

        [HttpPost("Csv")]
        public async Task<IActionResult> PostCsvAsync(IFormFile file)
        {
            try
            {
                if (!file.FileName.ToLower().EndsWith(".csv") || file.ContentType != "text/csv")
                    throw new FormatException("The file sent is not a CSV.");

                var filePath = $"{Path.GetTempPath()}{file.FileName.Substring(0, file.FileName.LastIndexOf("."))}_{Guid.NewGuid()}{file.FileName.Substring(file.FileName.LastIndexOf("."))}";
                await file.CreateAsync(filePath);

                var products = new List<ProductModel>();

                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, CsvHelperDefaults.DefaultCsvConfiguration))
                {
                    csv.Context.RegisterClassMap<ProductMap>();

                    csv.Read();
                    csv.ReadHeader();

                    while (csv.Read())
                    {
                        products.Add(csv.GetRecord<ProductModel>());
                    }
                }

                return Ok(products);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost("Xls")]
        public async Task<IActionResult> PostXlsAsync(IFormFile file)
        {
            try
            {
                if ((!file.FileName.ToLower().EndsWith(".xls") && !file.FileName.ToLower().EndsWith(".xlsx")) 
                    || file.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    throw new FormatException("The file sent is not a XLS or XLSX.");

                var filePath = $"{Path.GetTempPath()}{file.FileName.Substring(0, file.FileName.LastIndexOf("."))}_{Guid.NewGuid()}{file.FileName.Substring(file.FileName.LastIndexOf("."))}";
                await file.CreateAsync(filePath);

                var products = new List<ProductModel>();

                using (var csv = new CsvReader(new ExcelParser(filePath)))
                {
                    csv.Context.RegisterClassMap<ProductMap>();

                    csv.Read();
                    csv.ReadHeader();

                    while (csv.Read())
                    {
                        products.Add(csv.GetRecord<ProductModel>());
                    }
                }

                return Ok(products);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        private IEnumerable<ProductModel> GenerateProducts(int quantity)
        {
            var products = new List<ProductModel>();

            for (int i = 0; i < quantity; i++)
            {
                var faker = new Faker();
                products.Add(
                    new ProductModel(
                        faker.Commerce.ProductName(),
                        faker.Random.Decimal(0.25M, 99.99M),
                        faker.Random.Bool(),
                        faker.Random.Int(0, 999)));
            }

            return products;
        }
    }
}
