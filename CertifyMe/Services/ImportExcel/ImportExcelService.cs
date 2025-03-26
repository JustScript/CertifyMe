using CertifyMe.Models;
using OfficeOpenXml;

namespace CertifyMe.Services
{
    public class ImportExcelService : IImportExcelService
    {
        private readonly ImportExcelFileSettings _excelColumns;

        public ImportExcelService(IConfiguration configuration)
        {
            _excelColumns = configuration.GetSection("ImportExcelFileSettings").Get<ImportExcelFileSettings>();

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
                        var name = worksheet.Cells[row, _excelColumns.NameColNum].Text;
                        var surname = worksheet.Cells[row, _excelColumns.SurnameColNum].Text;
                        var email = worksheet.Cells[row, _excelColumns.EmailColNum].Text;
                        var course = worksheet.Cells[row, _excelColumns.CourseColNum].Text;
                        var completed = worksheet.Cells[row, _excelColumns.CompletedColNum].Text;

                        var record = new ExcelRowRecord(name, surname, email, course, completed);
                        records.Add(record);
                    }
                }
            }

            return records;
        }
    }
}