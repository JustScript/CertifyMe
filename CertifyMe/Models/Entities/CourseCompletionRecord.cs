namespace CertifyMe.Models.Entities
{
    public class CourseCompletionRecord
    {
        public static CourseCompletionRecord FromDto(ExcelRowRecord dto)
        {
            return new CourseCompletionRecord
            {
                Name = dto.Name,
                Surname = dto.Surname,
                Email = dto.Email,
                CourseName = dto.CourseName,
                CompletionDate = dto.CompletionDate
            };
        }
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string CourseName { get; set; }
        public DateTime CompletionDate { get; set; }
    }
}