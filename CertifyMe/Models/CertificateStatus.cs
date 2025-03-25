namespace CertifyMe.Models
{
    public enum CertificateStatus
    {
        NotSent = 0,
        Sent = 1,
        Failed = -1,
        Resend = 100
    }
}