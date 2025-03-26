namespace CertifyMe.Models.Entities
{
    public class CertificateEntity
    {
        public CertificateEntity()
        {
            FileName = string.Empty;
            FileData = [];
            CourseCompletion = new CourseCompletionEntity();
        }

        public int Id { get; set; }

        public string FileName { get; set; }

        public byte[] FileData { get; set; }
        
        public CertificateStatus CertificateSendStatus { get; set; }

        public int CourseCompletionEntityId { get; set; }

        public CourseCompletionEntity CourseCompletion { get; set; }
    }
}