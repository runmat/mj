namespace GeneralTools.Contracts
{
    public interface ISmtpSettings 
    {
        string Server { get; set; }

        string Port { get; set; }

        bool EnableSsl { get; set; }

        string UserName { get; set; }

        string From { get; set; }

        string Password { get; set; }
    }
   
}
