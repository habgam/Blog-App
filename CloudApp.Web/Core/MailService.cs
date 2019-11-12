using CloudApp.Data;
using CloudApp.Data.Enum;
using CloudApp.Data.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace CloudApp.Web.Core
{
    public class MailService
    {
        public bool SendMail(string subject, string body, int orgaId, HttpPostedFileBase file, List<string> toNames=null)
        {
            try
            {
                DbDataContext db = new DbDataContext("CloudAppWebSiteView");
                COrganization org = db.Organizations.Where(p => p.ActiveStatus == EActiveStatus.Active && p.OrganizationId == orgaId).FirstOrDefault();
                if (org != null)
                {
                    var message = new MailMessage();
                    if (toNames != null && toNames.Count > 0)
                        foreach (var item in toNames)
                        {
                            message.To.Add(new MailAddress(item));  // replace with valid value 
                        }
                    else
                        message.To.Add(new MailAddress(org.SmtpWhoMail));  // replace with valid value 
                    message.From = new MailAddress(org.SmtpUserName);  // replace with valid value
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;
                    if (file != null)
                    {
                        Stream st = file.InputStream;
                        message.Attachments.Add(new Attachment(st, file.FileName));
                    }
                    using (var smtp = new SmtpClient())
                    {
                        var credential = new NetworkCredential
                        {
                            UserName = org.SmtpUserName,  // replace with valid value
                            Password = org.SmtpPassword  // replace with valid value
                        };
                        smtp.Credentials = credential;
                        smtp.Host = org.SmtpServer;
                        smtp.Port = Convert.ToInt32(org.SmtpPort);
                        smtp.EnableSsl = org.SmtpUseSSL;
                        smtp.Send(message);
                        return true;
                    }
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
    }
}