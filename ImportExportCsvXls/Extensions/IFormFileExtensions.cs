namespace ImportExportCsvXls.Extensions;

public static class IFormFileExtensions
{
    public static async Task CreateAsync(this IFormFile file, string filePath)
    {
        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }
    }
}