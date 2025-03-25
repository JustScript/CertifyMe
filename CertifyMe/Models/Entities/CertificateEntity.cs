namespace CertifyMe.Models.Entities
{
    public class CertificateEntity
    {
        public int Id { get; set; }

        public string FileName { get; set; } = string.Empty;

        public byte[] FileData { get; set; } = Array.Empty<byte>();
        
        public CertificateStatus CertificateSendStatus { get; set; }

        public int CourseCompletionEntityId { get; set; }

        public CourseCompletionEntity CourseCompletion { get; set; } = null!;
    }
}