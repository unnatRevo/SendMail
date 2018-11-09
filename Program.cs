using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using CsvHelper;

namespace SendMail
{
    class CsvModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            TextReader reader = new StreamReader("/path/to/your/file.ext");
            var csvObj = new CsvReader(reader);
            var records = csvObj.GetRecords<CsvModel>();
            var temp = records.ToList();

            foreach (var item in temp)
            {
                EmailSend(item.EmailId, item.Name);
            }
        }

        public static void EmailSend(string recipientId, string recipientName) {
            try
            {
                Console.WriteLine($"Sending E-Mail to {recipientName}...");
                SmtpClient mailClient = new SmtpClient("mysmtpserver",587);
                mailClient.UseDefaultCredentials = false;
                mailClient.EnableSsl = true;
                mailClient.Credentials = new NetworkCredential("username","password");

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("mail_from@me.com");
                mailMessage.To.Add(recipientId);
                mailMessage.Body = $"Hello {recipientName}, this is for testing purpose only.\n\n-Regards\nSender Name";
                mailMessage.Attachments.Add(new Attachment("/path/to/your/attatchment/file.jpg"));
                mailMessage.Subject = "This is testing.";
                
                mailClient.Send(mailMessage);

                Console.WriteLine($"Email Sent.\n");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"\nMail sending process faced some error, please go through it.\n{ex.Message}");
            }
            
        }
    }
}
