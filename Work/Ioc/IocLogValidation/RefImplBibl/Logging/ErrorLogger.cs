using System;
using System.Web;
using Elmah;
using NLog;

namespace RefImplBibl.Logging
{
    /// <summary>
    /// Logger dient auschließlich für das Schreiben von Fehlermeldungen wenn die Datenbank ausfällt
    /// Elmahs ErrorXml Klasse wird verwendet um die Daten des Exceptions und des http Contexts zu extrahieren 
    /// </summary>
    public class ErrorLogger
    {
        private readonly Logger _log = null;

        public ErrorLogger()
        {
            _log = LogManager.GetLogger("Exception");
        }

        public void Log(Exception exception)
        {
            LogEventInfo logEventInfo = new LogEventInfo();
            logEventInfo.Level = LogLevel.Error;
            logEventInfo.TimeStamp = DateTime.Now;

            var elmahError = new Error(exception, HttpContext.Current);

            //// Größere Kontrolle über das XML:
            //// Folgende Zeile auskommentieren und denn darauf folgenden Block aktivieren
            //// Kopiert aus der Routine EncodeString in ErrorXml mit Anpassungen an die XmlWriterSettings
            logEventInfo.Properties["DetailedMessage"] = ErrorXml.EncodeString(elmahError);

            //var sw = new StringWriter();
            //
            //using (var writer = XmlWriter.Create(sw, new XmlWriterSettings
            //{
            //    CloseOutput = true,
            //    Encoding = new UTF8Encoding(false),
            //    Indent = false,
            //    NewLineOnAttributes = false,
            //    CheckCharacters = false,
            //    OmitXmlDeclaration = true, // see issue #120: http://code.google.com/p/elmah/issues/detail?id=120
            //}))
            //{
            //    writer.WriteStartElement("error");
            //    ErrorXml.Encode(elmahError, writer);
            //    writer.WriteEndElement();
            //    writer.Flush();
            //}
            //
            //logEventInfo.Properties["DetailedMessage"] = sw.ToString();

            logEventInfo.Message = elmahError.Message;
            _log.Log(logEventInfo);
        }
    }
}
