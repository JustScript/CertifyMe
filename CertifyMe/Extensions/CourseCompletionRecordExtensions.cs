using System.Net;
using System.Net.Mail;
using CertifyMe.Models;
using CertifyMe.Models.Entities;

namespace CertifyMe.Extensions
{
    public static class CourseCompletionRecordExtensions
    {
        public static CourseCompletionEntity SyncWithExcelRow(this CourseCompletionEntity target, ExcelRowRecord excelRow)
        {
            if (!string.IsNullOrEmpty(excelRow.Name))
                target.Name = excelRow.Name;

            if (!string.IsNullOrEmpty(excelRow.Surname))
                target.Surname = excelRow.Surname;

            if (!string.IsNullOrEmpty(excelRow.Email))
                target.Email = excelRow.Email;

            if (!string.IsNullOrEmpty(excelRow.CourseName))
                target.CourseName = excelRow.CourseName;

            if (excelRow.CompletionDate != null)
                target.CompletionDate = (DateTime)excelRow.CompletionDate;

            return target;
        }

        public static CourseCompletionEntity AttachCertificate(this CourseCompletionEntity target, string filename, byte[] certificate)
        {
            target.Certificate = new CertificateEntity()
            {
                FileName = filename,
                FileData = certificate
            };

            return target;
        }

        public static bool SendCertificateByEmail(this CourseCompletionEntity target)
        {
            if (target.Certificate == null)
            {
                return false;
            }

            var smtpClient = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("6604789d0e2e10", "196a38959c4b62"),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("CertifyMe@example.com"),
                Subject = "Your certificate",
                Body = $"Hi {target.Name} {target.Surname} - here is your certificate in attachment",
                IsBodyHtml = false,
            };

            mailMessage.To.Add(target.Email);

            var stream = new MemoryStream(target.Certificate.FileData);
            var attachment = new Attachment(stream, target.Certificate.FileName);
            mailMessage.Attachments.Add(attachment);

            smtpClient.Send(mailMessage);

            return true;
        }
    }
}