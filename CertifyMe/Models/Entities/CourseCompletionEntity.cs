namespace CertifyMe.Models.Entities
{
    public class CourseCompletionEntity
    {
        public CourseCompletionEntity()
        {
            Name = string.Empty;
            Surname = string.Empty;
            Email = string.Empty;
            CourseName = string.Empty;
        }

        public CourseCompletionEntity(ExcelRowRecord excelRow)
        {
            Name = excelRow.Name;
            Surname = excelRow.Surname;
            Email = excelRow.Email;
            CourseName = excelRow.CourseName;
            CompletionDate = (DateTime)excelRow.CompletionDate;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string CourseName { get; set; }

        public DateTime CompletionDate { get; set; }

        public CertificateEntity? Certificate { get; set; }
    }
}