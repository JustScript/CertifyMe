using System.Globalization;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace CertifyMe.Models
{
    public class ExcelRowRecord
    {
        public ExcelRowRecord(string name, string surname, string email, string course, string completed)
        {
            Name = !string.IsNullOrEmpty(name)
                ? name
                : null;

            Surname = !string.IsNullOrEmpty(surname)
                ? surname
                : null;

            Email = !string.IsNullOrEmpty(email)
                ? email
                : null;

            CourseName = !string.IsNullOrEmpty(course)
                ? course
                : null;

            CompletionDate = DateTime.TryParse(completed, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime completionDate)
                ? completionDate 
                : null;
        }

        [IgnoreDataMember]
        public bool IsValid => Validate();

        private bool Validate()
        {
            return 
                !string.IsNullOrEmpty(Name) &&
                !string.IsNullOrEmpty(Surname) &&
                !string.IsNullOrEmpty(Email) && Regex.Match(Email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$").Success &&
                !string.IsNullOrEmpty(CourseName) &&
                CompletionDate != null;
        }

        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? CourseName { get; set; }
        public DateTime? CompletionDate { get; set; }
    }
}