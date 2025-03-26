using System.Globalization;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace CertifyMe.Models
{
    public class ExcelRowRecord
    {
        public ExcelRowRecord(string name, string surname, string email, string course, string completed)
        {
            Name = name;
            Surname = surname;
            Email = email;
            CourseName = course;

            DateTime.TryParse(completed, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime completionDate);
            CompletionDate = completionDate;
        }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string CourseName { get; set; }
        public DateTime CompletionDate { get; set; }
    }
}