using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace _9_MyAcademy_MVC_CodeFirst.Services
{
    public class EmailService
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPassword;
        private readonly string _senderEmail;
        private readonly string _senderName;
        private readonly string _adminEmail;

        public EmailService()
        {
            _smtpHost = ConfigurationManager.AppSettings["SmtpHost"] ?? "smtp.gmail.com";
            _smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"] ?? "587");
            _smtpUser = ConfigurationManager.AppSettings["SmtpUser"] ?? "";
            _smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"] ?? "";
            _senderEmail = ConfigurationManager.AppSettings["SenderEmail"] ?? "";
            _senderName = ConfigurationManager.AppSettings["SenderName"] ?? "LifeSure Insurance";
            _adminEmail = ConfigurationManager.AppSettings["AdminEmail"] ?? "emreokanbaskaya@gmail.com";
        }

        /// <summary>
        /// Sends notification email to admin when a new contact message is received
        /// </summary>
        public async Task SendContactNotificationToAdmin(string customerName, string customerEmail, string customerPhone, string subject, string message, string insuranceType, string aiCategory, double aiConfidence, bool aiIsUrgent)
        {
            try
            {
                string urgentBanner = "";
                if (aiIsUrgent)
                {
                    urgentBanner = @"
                        <div style='background-color: #dc3545; color: white; padding: 15px; text-align: center; font-size: 16px; font-weight: bold;'>
                            &#9888; URGENT MESSAGE - Requires Immediate Attention
                        </div>";
                }

                string categoryColor;
                string categoryIcon;
                switch (aiCategory)
                {
                    case "Complaint":
                        categoryColor = "#dc3545";
                        categoryIcon = "&#9888;";
                        break;
                    case "Thank You":
                        categoryColor = "#198754";
                        categoryIcon = "&#10084;";
                        break;
                    case "Inquiry":
                        categoryColor = "#0d6efd";
                        categoryIcon = "&#10067;";
                        break;
                    case "Request":
                        categoryColor = "#fd7e14";
                        categoryIcon = "&#128221;";
                        break;
                    case "Feedback":
                        categoryColor = "#6f42c1";
                        categoryIcon = "&#128172;";
                        break;
                    case "Urgent":
                        categoryColor = "#dc3545";
                        categoryIcon = "&#128680;";
                        break;
                    default:
                        categoryColor = "#6c757d";
                        categoryIcon = "&#128196;";
                        break;
                }

                var htmlBody = $@"
                    <html>
                    <head>
                        <style>
                            body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; margin: 0; padding: 0; }}
                            .container {{ max-width: 600px; margin: 0 auto; }}
                            .header {{ background-color: #0d6efd; color: white; padding: 20px; text-align: center; }}
                            .ai-section {{ padding: 15px 20px; border-bottom: 2px solid #e9ecef; }}
                            .ai-badge {{ display: inline-block; padding: 8px 20px; border-radius: 25px; color: white; font-weight: bold; font-size: 14px; }}
                            .confidence {{ display: inline-block; margin-left: 10px; padding: 5px 12px; background-color: #e9ecef; border-radius: 15px; font-size: 12px; color: #555; }}
                            .content {{ padding: 20px; background-color: #f8f9fa; }}
                            .field {{ margin-bottom: 12px; }}
                            .label {{ font-weight: bold; color: #0d6efd; }}
                            .message-box {{ background-color: white; padding: 15px; border-left: 4px solid #0d6efd; margin-top: 15px; }}
                            .insurance-badge {{ background-color: #0d6efd; color: white; padding: 6px 15px; border-radius: 20px; display: inline-block; font-size: 13px; }}
                            .footer {{ padding: 15px 20px; background-color: #e9ecef; text-align: center; font-size: 11px; color: #888; }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            {urgentBanner}
                            <div class='header'>
                                <h1 style='margin:0;'>New Contact Message</h1>
                            </div>
                            <div class='ai-section' style='background-color: #fff;'>
                                <p style='margin: 0 0 8px 0; font-size: 11px; text-transform: uppercase; letter-spacing: 1px; color: #888; font-weight: bold;'>AI Message Classification (Hugging Face)</p>
                                <span class='ai-badge' style='background-color: {categoryColor};'>{categoryIcon} {aiCategory}</span>
                                <span class='confidence'>{aiConfidence}% confidence</span>
                            </div>
                            <div class='content'>
                                <div class='field'>
                                    <span class='label'>Name:</span> {customerName}
                                </div>
                                <div class='field'>
                                    <span class='label'>Email:</span> {customerEmail}
                                </div>
                                <div class='field'>
                                    <span class='label'>Phone:</span> {customerPhone}
                                </div>
                                <div class='field'>
                                    <span class='label'>Insurance Type:</span> <span class='insurance-badge'>{insuranceType}</span>
                                </div>
                                <div class='field'>
                                    <span class='label'>Subject:</span> {subject}
                                </div>
                                <div class='message-box'>
                                    <span class='label'>Message:</span><br/>
                                    {message}
                                </div>
                            </div>
                            <div class='footer'>
                                Received at: {DateTime.Now:dd MMM yyyy HH:mm} | Classified by Hugging Face AI
                            </div>
                        </div>
                    </body>
                    </html>";

                var emailSubject = aiIsUrgent
                    ? $"[URGENT] New Contact Message: {subject}"
                    : $"[{aiCategory}] New Contact Message: {subject}";

                await SendEmailAsync(_adminEmail, emailSubject, htmlBody);
                Debug.WriteLine("Admin notification email sent successfully.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error sending admin notification email: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Sends auto-reply email to customer with AI-generated response
        /// </summary>
        public async Task SendAutoReplyToCustomer(string customerEmail, string customerName, string originalSubject, string aiResponse, string aiCategory)
        {
            try
            {
                string categoryMessage;
                switch (aiCategory)
                {
                    case "Complaint":
                        categoryMessage = "We understand your concern and take complaints very seriously. Our team will prioritize your case.";
                        break;
                    case "Thank You":
                        categoryMessage = "We truly appreciate your kind words! It means a lot to our team.";
                        break;
                    case "Inquiry":
                        categoryMessage = "Great question! We've prepared some information that should help answer your inquiry.";
                        break;
                    case "Request":
                        categoryMessage = "We've received your request and our team will process it as soon as possible.";
                        break;
                    case "Feedback":
                        categoryMessage = "Thank you for your valuable feedback! We continuously strive to improve our services.";
                        break;
                    case "Urgent":
                        categoryMessage = "We've flagged your message as urgent and our team will respond to you as quickly as possible.";
                        break;
                    default:
                        categoryMessage = "We've received your message and our team will review it shortly.";
                        break;
                }

                var htmlBody = $@"
                    <html>
                    <head>
                        <style>
                            body {{ font-family: Arial, sans-serif; line-height: 1.8; color: #333; margin: 0; padding: 0; }}
                            .container {{ max-width: 600px; margin: 0 auto; }}
                            .header {{ background-color: #0d6efd; color: white; padding: 30px; text-align: center; }}
                            .header h1 {{ margin: 0; }}
                            .category-bar {{ padding: 12px 30px; background-color: #e8f0fe; border-left: 4px solid #0d6efd; }}
                            .content {{ padding: 30px; background-color: #f8f9fa; }}
                            .greeting {{ font-size: 18px; margin-bottom: 20px; }}
                            .ai-response {{ background-color: white; padding: 20px; border-radius: 8px; margin: 20px 0; border-left: 4px solid #0d6efd; }}
                            .footer {{ background-color: #333; color: white; padding: 20px; text-align: center; font-size: 12px; }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                <h1>LifeSure Insurance</h1>
                                <p style='margin: 5px 0 0 0;'>Your Trusted Insurance Partner</p>
                            </div>
                            <div class='category-bar'>
                                <strong>Your message has been categorized as:</strong> {aiCategory}<br/>
                                <span style='font-size: 13px; color: #555;'>{categoryMessage}</span>
                            </div>
                            <div class='content'>
                                <p class='greeting'>Dear {customerName},</p>
                                <p>Thank you for reaching out to LifeSure Insurance. We have received your message and our team will get back to you shortly.</p>
                                <p>In the meantime, here is some helpful information based on your inquiry:</p>
                                <div class='ai-response'>
                                    {aiResponse.Replace("\n", "<br/>")}
                                </div>
                                <p>If you have any urgent questions, please don't hesitate to call us directly.</p>
                                <p>Best regards,<br/><strong>LifeSure Insurance Team</strong></p>
                            </div>
                            <div class='footer'>
                                <p>&copy; {DateTime.Now.Year} LifeSure Insurance. All rights reserved.</p>
                            </div>
                        </div>
                    </body>
                    </html>";

                await SendEmailAsync(customerEmail, $"Re: {originalSubject} - Thank you for contacting LifeSure", htmlBody);
                Debug.WriteLine($"Auto-reply email sent successfully to {customerEmail}.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error sending auto-reply email: {ex.Message}");
                throw;
            }
        }

        private async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            using (var message = new MailMessage())
            {
                message.From = new MailAddress(_senderEmail, _senderName);
                message.To.Add(new MailAddress(toEmail));
                message.Subject = subject;
                message.Body = htmlBody;
                message.IsBodyHtml = true;

                using (var client = new SmtpClient(_smtpHost, _smtpPort))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(_smtpUser, _smtpPassword);

                    try
                    {
                        await client.SendMailAsync(message);
                        Debug.WriteLine("Email sent successfully!");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"SMTP Error: {ex.Message}");
                        throw;
                    }
                }
            }
        }
    }
}
