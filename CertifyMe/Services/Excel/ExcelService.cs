using System.Globalization;
using CertifyMe.Models.Configuration;
using CertifyMe.Models.Entities;
using OfficeOpenXml;

namespace CertifyMe.Services
{
    public class ExcelService : IExcelService
    {
        private readonly ImportExcelFileSettings _excelColumns;

        public ExcelService(IConfiguration configuration)
        {
            _excelColumns = configuration.GetSection("ImportExcelFileColumnNumberSettings").Get<ImportExcelFileSettings>();

            // EPPlus NonCommercial license is free to use in non-commercial applications            
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public async Task<List<ExcelRowRecord>> GetRecordsFromExcelFileAsync(IFormFile file)
        {
            var records = new List<ExcelRowRecord>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);

                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    int endRowNum = worksheet.Dimension.End.Row;

                    for (int row = _excelColumns.StartRowNum; row <= endRowNum; row++) // Skip headers
                    {
                        DateTime.TryParse(worksheet.Cells[row, _excelColumns.CompletedColNum].Text, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime completionDate);
                        records.Add(new ExcelRowRecord
                        {
                            Name = worksheet.Cells[row, _excelColumns.NameColNum].Text,
                            Surname = worksheet.Cells[row, _excelColumns.SurnameColNum].Text,
                            Email = worksheet.Cells[row, _excelColumns.EmailColNum].Text,
                            CourseName = worksheet.Cells[row, _excelColumns.CourseColNum].Text,
                            CompletionDate = completionDate
                        });
                    }
                }
            }

            return records;
        }
    }
}