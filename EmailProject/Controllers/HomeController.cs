
using ActiveUp.Net.Mail;
using AE.Net.Mail;
using EmailProject.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Text;
using System.Xml;
using HtmlAgilityPack;

namespace EmailProject.Controllers
{
    public class HomeController : Controller
    {
        static List<AE.Net.Mail.MailMessage> messages = new List<AE.Net.Mail.MailMessage>();
        static ImapClient IC;

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(EmailModel model)
        {
            EmailModel email = model;
            send(email);
         
            return View(email);
        }
        public IActionResult MailBox()
        {   
            
            var model = GetEmails("Inbox");
            return View(model);
        }

        public IActionResult SelectEmail(string emailId)
        {
            AE.Net.Mail.MailMessage message = new AE.Net.Mail.MailMessage();

            message = messages.FirstOrDefault(x => x.Uid == emailId);

            var convertedMessage = ConvertEmailBodyToHtml(message);

            TestClass test = new TestClass();
            test.wiadomość = convertedMessage;
            return View(test);
        }
        public bool send(EmailModel model)
        {

            if(IsValidEmailAddress(model.From, model.To)) { }
            else {
                ViewData["data"] = "Check email addresses";
                return false;
            }


            

            Dictionary<string, string> listOfMails = new Dictionary<string, string>()
            {
                {"gmail","smtp.gmail.com" },
                {"wp","smtp.wp.pl" },
                {"outlook","smtp-mail.outlook.com." },
                {"o2","smtp.o2.pl" }
            };
            Dictionary<string,int> listOfPorts = new Dictionary<string,int>()
            {
                {"gmail" ,587 },
                {"wp" ,465 },
                {"outlook" ,587 },
                {"o2" ,587 }
            };
            //warmachine3001wm@gmail.com
            string password = "aslfraaxhvbggcrs";


            string server = listOfMails[model.Mail];
            int port = listOfPorts[model.Mail];
            

            string to = model.To;
            string from = model.From;

            string subject = model.Title;
            string body = model.Text;

            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(from, to, subject, body);

            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(server)
            {
                Port= port,
                Credentials = new NetworkCredential(from,password),
                EnableSsl= true
            };
            try
            {
                client.Send(message);
                ViewData["data"] = "email sending success";



            }
            catch (Exception ex)
            {
                
                Console.WriteLine(ex.ToString());

                ViewData["data"]= "email sending failed";
                return false;
            }
            return true;
        }

        public bool IsValidEmailAddress(string from,string to)
        {
            if (!string.IsNullOrEmpty(from) && new EmailAddressAttribute().IsValid(from)&& !string.IsNullOrEmpty(to) && new EmailAddressAttribute().IsValid(to))
                return true;
            else
                return false;
        }


        public IndexModel GetEmails(string filter)
        {
            string email = "warmachine3001wm@gmail.com";
            string password = "aslfraaxhvbggcrs";
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            IC = new ImapClient("imap.gmail.com",email,password,AuthMethods.Login,993,true);
            IC.SelectMailbox(filter);
            int all = IC.GetMessageCount()-1;

            var mailboxes = IC.ListMailboxes(string.Empty, "*");
            
            for (int i=all;i>all-20; i--) {
                var m=IC.GetMessage(i);
               
                    messages.Add(m);

                
            }
            IndexModel model = new IndexModel();
            model.ListMailBoxes = mailboxes;
            model.ListEmails = messages;
            return model;
        }

        public string ConvertEmailBodyToHtml(AE.Net.Mail.MailMessage message)
        {
            // Use HtmlAgilityPack to parse and clean up the message body
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(message.Body);

            // Remove potentially harmful tags, such as script and iframe
            var unwantedTags = new[] { "script", "iframe" };
            foreach (var tag in unwantedTags)
            {
                var nodes = htmlDoc.DocumentNode.Descendants(tag).ToArray();
                foreach (var node in nodes)
                {
                    node.Remove();
                }
            }

            // Remove any HTML comments
            var comments = htmlDoc.DocumentNode.DescendantsAndSelf().Where(n => n.NodeType == HtmlNodeType.Comment).ToArray();
            foreach (var comment in comments)
            {
                comment.Remove();
            }

            // Remove any inline styles, such as background-color
            var inlineStyleRegex = new Regex("style=(\"|\')[^\"\'\\>]*?(\"|\')");
            var elementsWithInlineStyle = htmlDoc.DocumentNode.Descendants().Where(d => d.Attributes.Any(a => a.Name == "style"));
            foreach (var element in elementsWithInlineStyle)
            {
                var styleAttribute = element.Attributes["style"];
                styleAttribute.Value = inlineStyleRegex.Replace(styleAttribute.Value, string.Empty);
            }

            var bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");
            var bodyContent = bodyNode.InnerHtml;
            return bodyContent;
        }

    }

}

