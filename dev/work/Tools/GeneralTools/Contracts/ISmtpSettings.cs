namespace GeneralTools.Contracts
{
    public interface ISmtpSettings 
    {
        string SmtpServer { get; }

        string SmtpSender { get; }

        //string SmtpPort { get; }

        //bool SmtpEnableSsl { get; }

        //string SmtpUserName { get; }

        //string SmtpPassword { get; }
    }
}
