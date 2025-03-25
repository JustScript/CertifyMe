namespace CertifyMe.Models.Entities
{
    public class CourseCompletionEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Surname { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string CourseName { get; set; } = string.Empty;

        public DateTime CompletionDate { get; set; }

        public CertificateEntity? Certificate { get; set; }
    }
}