using CertifyMe.Models;

namespace CertifyMe.Extensions
{
    public static class CourseCompletionRecordExtensions
    {
        public static CourseCompletionRecord SyncWithExcelRow(this CourseCompletionRecord target, ExcelRowRecord source)
        {
            if (!string.IsNullOrEmpty(source.Name))
                target.Name = source.Name;

            if (!string.IsNullOrEmpty(source.Surname))
                target.Surname = source.Surname;

            if (!string.IsNullOrEmpty(source.Email))
                target.Email = source.Email;

            if (!string.IsNullOrEmpty(source.CourseName))
                target.CourseName = source.CourseName;

            if (source.CompletionDate != null)
                target.CompletionDate = (DateTime)source.CompletionDate;

            return target;
        }
    }
}