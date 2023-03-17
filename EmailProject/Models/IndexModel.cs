namespace EmailProject.Models
{
    public class IndexModel
    {
        public EmailModel Email { get; set; }
        public List<AE.Net.Mail.MailMessage> ListEmails { get; set; }
        public AE.Net.Mail.Imap.Mailbox[] ListMailBoxes { get; set; }

    }
}
