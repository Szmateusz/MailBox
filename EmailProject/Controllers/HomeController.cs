
using ActiveUp.Net.Mail;
using AE.Net.Mail;
using EmailProject.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;

namespace EmailProject.Controllers
{
    public class HomeController : Controller
    {
        static List<AE.Net.Mail.MailMessage> messages = new List<AE.Net.Mail.MailMessage>();

        [HttpGet]
        public IActionResult Index()
        {
            IndexModel model = new IndexModel();
            model.List = GetEmails();
            return View(model);
        }
        [HttpPost]
        public IActionResult Index(IndexModel model)
        {
            EmailModel email = model.Email;

            send(email);
            return View(model);
        }

        public IActionResult SelectEmail(string emailId)
        {
            AE.Net.Mail.MailMessage message = new AE.Net.Mail.MailMessage();

            message = messages.FirstOrDefault(x => x.Uid == emailId);
            return View(message);
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

        static ImapClient IC;

        public List<AE.Net.Mail.MailMessage> GetEmails()
        {
            string email = "warmachine3001wm@gmail.com";
            string password = "aslfraaxhvbggcrs";
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            IC = new ImapClient("imap.gmail.com",email,password,AuthMethods.Login,993,true);
            IC.SelectMailbox("INBOX");
            int all = IC.GetMessageCount()-1;
            
            for(int i=all;i>all-20; i--) {
                var m=IC.GetMessage(i);
               
                    messages.Add(m);

                
            }
          
          return messages;
        }

    }

}

