using EdiMonthlyReportGenerator.Models;
using EdiMonthlyReportGenerator.Services.Interfaces;
using System.Net.Mail;
using System.Web.UI;

namespace EdiMonthlyReportGenerator.Services.Implements
{
    public class EmailService : IEmailService
    {
        private readonly IConfigurationService _configurationService;
        public EmailService(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public Result SendEmail(string emailSubject, IEnumerable<InfoRow> infoRows, string dateTime, bool isSuccessful = false)
        {
            var prod = _configurationService.AppSettings.IsProd ? string.Empty : " (STG)";
            var emailBody = CreateEmailBody(prod, emailSubject, infoRows, dateTime, isSuccessful);
            return SendEmail(emailSubject, emailBody);
        }

        private static string CreateEmailBody(string prod, string emailSubject, IEnumerable<InfoRow> infoRows, string dateTime, bool isSuccessful)
        {
            var sw = new StringWriter();

            using (var writer = new HtmlTextWriter(sw))
            {
                AddHtmlHeaderTags(writer);

                if (!string.IsNullOrWhiteSpace(prod))
                {
                    writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "maroon");
                    //writer.AddStyleAttribute("color", "fuchsia");
                    writer.RenderBeginTag(HtmlTextWriterTag.P);
                    writer.RenderBeginTag(HtmlTextWriterTag.B);
                    writer.Write($"This email is generated for staging environment");
                    writer.RenderEndTag(); //B                
                    writer.RenderEndTag(); //P  
                }

                writer.RenderBeginTag(HtmlTextWriterTag.P);
                writer.RenderBeginTag(HtmlTextWriterTag.B);
                writer.Write($"{emailSubject}\r\n");
                writer.RenderEndTag(); //B                
                writer.RenderEndTag(); //P               


                writer.RenderBeginTag(HtmlTextWriterTag.Hr);

                writer.RenderBeginTag(HtmlTextWriterTag.P);
                writer.Write($"File Generation Time: {dateTime}");
                writer.RenderEndTag(); //P

                writer.RenderBeginTag(HtmlTextWriterTag.Hr);

                if (infoRows != null && infoRows.Any())
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Table);
                    writer.RenderBeginTag(HtmlTextWriterTag.Tbody);
                    foreach (var infoRow in infoRows)
                    {
                        AddInfoRow(writer, $"{infoRow.Header}: ", infoRow.Info);
                       
                    }                    
                    writer.RenderEndTag(); //Tbody                    
                    writer.RenderEndTag(); //Table
                }
                AddHtmlEndTags(writer);
                
            }

            return sw.ToString();
        }

        #region Private Method(s)
        private Result SendEmail(string subject, string emailbody, System.Net.Mail.Attachment? attachment = null) 
        {
            try
            {
                var smtpServer = _configurationService.AppSettings.EmailSettings.SmtpServer;
                var smtpPort = _configurationService.AppSettings.EmailSettings.SmtpPort;
                var senderEmail = _configurationService.AppSettings.EmailSettings.FromEmail;
                var senderName = _configurationService.AppSettings.EmailSettings.FromEmailName;
                var recipients = _configurationService.AppSettings.EmailSettings.Recipients;
                var recipientsCc = string.Empty; // For future use if required                

                var mail = new MailMessage
                {
                    From = new MailAddress(senderEmail, senderName)
                };

                var recipientsList = recipients.Split(new char[] { ',', ';' });

                foreach (var recipient in recipientsList.Select(e => e.ToLower()).Distinct())
                {
                    if (!string.IsNullOrWhiteSpace(recipient))
                    {
                        mail.To.Add(new MailAddress(recipient));
                    }
                }

                mail.Subject = subject;
                mail.IsBodyHtml = true;

                if (!string.IsNullOrEmpty(recipientsCc))
                {
                    string[] recipientsCcList = recipientsCc.Split(new char[] { ',', ';' });

                    foreach (string? cc in recipientsCcList.Select(e => e.ToLower()).Distinct())
                    {
                        if (!string.IsNullOrWhiteSpace(cc))
                        {
                            mail.CC.Add(new MailAddress(cc));
                        }
                    }
                }

                mail.Body = emailbody;

                if (attachment != null)
                {
                    mail.Attachments.Add(attachment);
                }

                var smtp = new SmtpClient(smtpServer, smtpPort);

                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                return new Result(false, $"Exception in SendEmail (subject: {subject}) : {ex.Message}");
            }

            return new Result(true, $"Email with subject: {subject} sent successfully.");
        }

        private static void AddInfoRow(HtmlTextWriter writer, string header, string info)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.AddAttribute("class", "boldfont");
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.Write(header);
            writer.RenderEndTag(); //Span
            writer.RenderEndTag(); //Td
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write(info);            
            writer.RenderEndTag(); //Td
            writer.RenderEndTag(); //Tr           
        }

        private static void AddHtmlHeaderTags(HtmlTextWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Html);

            writer.RenderBeginTag(HtmlTextWriterTag.Head);

            writer.AddAttribute("type", "text/css");
            writer.RenderBeginTag(HtmlTextWriterTag.Style);
            writer.WriteLine("body{ width:100% !important; } /* Force Hotmail to display emails at full width */");
            writer.WriteLine("body{ -webkit-text-size-adjust:none; } /* Prevent Webkit platforms from changing default text sizes. */");
            writer.WriteLine("body{ margin:0; padding:0; font-family: Calibri, Archivo Narrow, Arial; font-size:16px; }");
            writer.WriteLine(".boldfont { font-weight: bold; }");
            writer.RenderEndTag(); //Style

            writer.RenderEndTag(); //Head

            writer.RenderBeginTag(HtmlTextWriterTag.Body);
            
        }

        private static void AddHtmlEndTags(HtmlTextWriter writer)
        {
            writer.RenderEndTag(); //Body

            writer.RenderEndTag(); //Html
        }
        #endregion
    }
}
