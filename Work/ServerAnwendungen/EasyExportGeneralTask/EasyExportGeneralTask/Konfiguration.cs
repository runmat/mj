using System.Configuration;

namespace EasyExportGeneralTask
{
    /// <summary>
    /// globale Konfiguration
    /// </summary>
    internal static class Konfiguration
    {
        public static string xmlConfigFilePath { get { return ConfigurationManager.AppSettings["ConfigFilePath"]; } }

        public static string pathPdfSplitAndMergeApplication { get { return ConfigurationManager.AppSettings["PDFSplitAndMergePath"]; } }
        public static string pathZipApplication { get { return ConfigurationManager.AppSettings["ZIPPath"]; } }

        public static string easyRemoteHosts { get { return ConfigurationManager.AppSettings["EasyRemoteHosts"]; } }
        public static string easyRequestTimeout { get { return ConfigurationManager.AppSettings["EasyRequestTimeout"]; } }
        public static string easySessionId { get { return ConfigurationManager.AppSettings["EasySessionId"]; } }
        public static string easyUser { get { return ConfigurationManager.AppSettings["EasyUser"]; } }
        public static string easyPwd { get { return ConfigurationManager.AppSettings["EasyPwdClear"]; } }
        public static string easyQueryIndexName { get { return ConfigurationManager.AppSettings["EasyQueryIndexName"]; } }
        public static string easyLogPath { get { return ConfigurationManager.AppSettings["EasyLogPath"]; } }
        public static string easyLogPathXml { get { return ConfigurationManager.AppSettings["EasyLogPathXml"]; } }
        public static string easyBlobPathRemote { get { return ConfigurationManager.AppSettings["EasyBlobPathRemote"]; } }

        public static string mailSmtpServer { get { return ConfigurationManager.AppSettings["SmtpServer"]; } }
        public static string mailAbsender { get { return ConfigurationManager.AppSettings["EMailAbsender"]; } }
        public static string mailAbsenderError { get { return ConfigurationManager.AppSettings["EMailAbsenderError"]; } }
        public static string mailEmpfaengerError { get { return ConfigurationManager.AppSettings["EMailEmpfaengerError"]; } }
        public static string mailEmpfaengerTest { get { return ConfigurationManager.AppSettings["EMailEmpfaengerTEST"]; } }

        public static bool isProdSap { get { return (ConfigurationManager.AppSettings["ProdSAP"].ToUpper() == "TRUE"); } }

        public static bool pauseAfterCompletion { get { return (ConfigurationManager.AppSettings["PauseAfterCompletion"].ToUpper() == "TRUE"); } }

        public static string WkdaDokumentAblagePfad { get { return ConfigurationManager.AppSettings["WkdaDokumentAblagePfad"]; } }
        public static string WkdaAtDokumentAblagePfad { get { return ConfigurationManager.AppSettings["WkdaAtDokumentAblagePfad"]; } }
    }
}
