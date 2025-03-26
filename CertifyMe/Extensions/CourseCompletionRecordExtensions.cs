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
            target.Name = excelRow.Name;
            target.Surname = excelRow.Surname;
            target.Email = excelRow.Email;
            target.CourseName = excelRow.CourseName;
            target.CompletionDate = excelRow.CompletionDate;

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

            var host = Environment.GetEnvironmentVariable("SMTP_HOST");
            int.TryParse(Environment.GetEnvironmentVariable("SMTP_PORT"), out int port);
            var user = Environment.GetEnvironmentVariable("SMTP_USER");
            var password = Environment.GetEnvironmentVariable("SMTP_PWD");

            var smtpClient = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(user, password),
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