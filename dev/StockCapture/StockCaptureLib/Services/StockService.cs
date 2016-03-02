using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Xml;
using StockCapture.Models;

namespace StockCapture
{
    public class StockService
    {
        public static double EmailWarningThreshold { get { return 0.005; } }

        public static int QueryCallsPerMinute { get { return 12; } }

        public static void CaptureStockQuote(string yahooSymbol, out double price, out double openPrice, out DateTime dateTime)
        {
            var testStockQuery = (GetAppSettingsVal("TestStockQuery") == "true");
            var xml = testStockQuery
                ? "<?xml version=\"1.0\" encoding=\"UTF-8\"?><query xmlns:yahoo=\"http://www.yahooapis.com/v1/base.rng\" yahoo:count=\"1\" yahoo:created=\"2015-04-03T17:49:24Z\" yahoo:lang=\"en-US\"><diagnostics><url execution-start-time=\"0\" execution-stop-time=\"2\" execution-time=\"2\"><![CDATA[http://www.datatables.org/yahoo/finance/yahoo.finance.quotes.xml]]></url><publiclyCallable>true</publiclyCallable><cache execution-start-time=\"5\" execution-stop-time=\"6\" execution-time=\"1\" method=\"GET\" type=\"MEMCACHED\"><![CDATA[5d1e1de680846a307c9874dc3d6878dc]]></cache><url execution-start-time=\"7\" execution-stop-time=\"15\" execution-time=\"8\"><![CDATA[http://download.finance.yahoo.com/d/quotes.csv?f=aa2bb2b3b4cc1c3c4c6c8dd1d2ee1e7e8e9ghjkg1g3g4g5g6ii5j1j3j4j5j6k1k2k4k5ll1l2l3mm2m3m4m5m6m7m8nn4opp1p2p5p6qrr1r2r5r6r7ss1s7t1t7t8vv1v7ww1w4xy&s=EURUSD%3DX]]></url><query execution-start-time=\"6\" execution-stop-time=\"15\" execution-time=\"9\" params=\"{url=[http://download.finance.yahoo.com/d/quotes.csv?f=aa2bb2b3b4cc1c3c4c6c8dd1d2ee1e7e8e9ghjkg1g3g4g5g6ii5j1j3j4j5j6k1k2k4k5ll1l2l3mm2m3m4m5m6m7m8nn4opp1p2p5p6qrr1r2r5r6r7ss1s7t1t7t8vv1v7ww1w4xy&amp;s=EURUSD%3DX]}\"><![CDATA[select * from csv where url=@url and columns='Ask,AverageDailyVolume,Bid,AskRealtime,BidRealtime,BookValue,Change&PercentChange,Change,Commission,Currency,ChangeRealtime,AfterHoursChangeRealtime,DividendShare,LastTradeDate,TradeDate,EarningsShare,ErrorIndicationreturnedforsymbolchangedinvalid,EPSEstimateCurrentYear,EPSEstimateNextYear,EPSEstimateNextQuarter,DaysLow,DaysHigh,YearLow,YearHigh,HoldingsGainPercent,AnnualizedGain,HoldingsGain,HoldingsGainPercentRealtime,HoldingsGainRealtime,MoreInfo,OrderBookRealtime,MarketCapitalization,MarketCapRealtime,EBITDA,ChangeFromYearLow,PercentChangeFromYearLow,LastTradeRealtimeWithTime,ChangePercentRealtime,ChangeFromYearHigh,PercebtChangeFromYearHigh,LastTradeWithTime,LastTradePriceOnly,HighLimit,LowLimit,DaysRange,DaysRangeRealtime,FiftydayMovingAverage,TwoHundreddayMovingAverage,ChangeFromTwoHundreddayMovingAverage,PercentChangeFromTwoHundreddayMovingAverage,ChangeFromFiftydayMovingAverage,PercentChangeFromFiftydayMovingAverage,Name,Notes,Open,PreviousClose,PricePaid,ChangeinPercent,PriceSales,PriceBook,ExDividendDate,PERatio,DividendPayDate,PERatioRealtime,PEGRatio,PriceEPSEstimateCurrentYear,PriceEPSEstimateNextYear,Symbol,SharesOwned,ShortRatio,LastTradeTime,TickerTrend,OneyrTargetPrice,Volume,HoldingsValue,HoldingsValueRealtime,YearRange,DaysValueChange,DaysValueChangeRealtime,StockExchange,DividendYield']]></query><javascript execution-start-time=\"4\" execution-stop-time=\"27\" execution-time=\"22\" instructions-used=\"59750\" table-name=\"yahoo.finance.quotes\"></javascript><user-time>28</user-time><service-time>11</service-time><build-version>0.2.75</build-version></diagnostics><results><quote symbol=\"EURUSD=X\"><Ask>1.0982</Ask><AverageDailyVolume /><Bid>1.0981</Bid><AskRealtime /><BidRealtime /><BookValue>0.0000</BookValue><Change_PercentChange>+0.0099 - +0.9139%</Change_PercentChange><Change>+0.0099</Change><Commission /><Currency>USD</Currency><ChangeRealtime /><AfterHoursChangeRealtime /><DividendShare /><LastTradeDate>4/3/2015</LastTradeDate><TradeDate /><EarningsShare /><ErrorIndicationreturnedforsymbolchangedinvalid /><EPSEstimateCurrentYear /><EPSEstimateNextYear /><EPSEstimateNextQuarter>0.0000</EPSEstimateNextQuarter><DaysLow>1.0864</DaysLow><DaysHigh>1.1027</DaysHigh><YearLow>1.0458</YearLow><YearHigh>1.3990</YearHigh><HoldingsGainPercent /><AnnualizedGain /><HoldingsGain /><HoldingsGainPercentRealtime /><HoldingsGainRealtime /><MoreInfo /><OrderBookRealtime /><MarketCapitalization /><MarketCapRealtime /><EBITDA /><ChangeFromYearLow>0.0523</ChangeFromYearLow><PercentChangeFromYearLow>+5.0053%</PercentChangeFromYearLow><LastTradeRealtimeWithTime /><ChangePercentRealtime /><ChangeFromYearHigh>-0.3009</ChangeFromYearHigh><PercebtChangeFromYearHigh>-21.5056%</PercebtChangeFromYearHigh><LastTradeWithTime>6:49pm - &lt;b&gt;1.0981&lt;/b&gt;</LastTradeWithTime><LastTradePriceOnly>1.0981</LastTradePriceOnly><HighLimit /><LowLimit /><DaysRange>1.0864 - 1.1027</DaysRange><DaysRangeRealtime /><FiftydayMovingAverage /><TwoHundreddayMovingAverage /><ChangeFromTwoHundreddayMovingAverage /><PercentChangeFromTwoHundreddayMovingAverage /><ChangeFromFiftydayMovingAverage /><PercentChangeFromFiftydayMovingAverage /><Name>EUR/USD</Name><Notes /><Open>1.0876</Open><PreviousClose>1.0882</PreviousClose><PricePaid /><ChangeinPercent>+0.9139%</ChangeinPercent><PriceSales /><PriceBook /><ExDividendDate /><PERatio /><DividendPayDate /><PERatioRealtime /><PEGRatio>0.0000</PEGRatio><PriceEPSEstimateCurrentYear /><PriceEPSEstimateNextYear /><Symbol>EURUSD=X</Symbol><SharesOwned /><ShortRatio /><LastTradeTime>6:49pm</LastTradeTime><TickerTrend /><OneyrTargetPrice /><Volume>0</Volume><HoldingsValue /><HoldingsValueRealtime /><YearRange>1.0458 - 1.3990</YearRange><DaysValueChange /><DaysValueChangeRealtime /><StockExchange>CCY</StockExchange><DividendYield /><PercentChange>+0.9139%</PercentChange></quote></results></query><!-- total: 28 --><!-- pprd1-node1004-lh1.manhattan.bf1.yahoo.com -->"
                : GetStockXml(yahooSymbol);

            var myXmlDocument = new XmlDocument();
            myXmlDocument.LoadXml(xml);

            var nsmgr = new XmlNamespaceManager(myXmlDocument.NameTable);
            nsmgr.AddNamespace("yahoo", "http://www.yahooapis.com/v1/base.rng");
            var priceText = myXmlDocument.SelectSingleNode("query/results/quote/LastTradePriceOnly", nsmgr).InnerText;
            var date = myXmlDocument.SelectSingleNode("query/results/quote/LastTradeDate", nsmgr).InnerText;
            var time = myXmlDocument.SelectSingleNode("query/results/quote/LastTradeTime", nsmgr).InnerText;
            var openText = myXmlDocument.SelectSingleNode("query/results/quote/Open", nsmgr).InnerText;

            dateTime = DateTime.Parse(string.Format("{0} {1}", date, time), new CultureInfo("en-US"));
            dateTime = dateTime.AddHours(1);
            price = double.Parse(priceText.Replace('.', ','));
            openPrice = double.Parse(openText.Replace('.', ','));
        }

        public static void CaptureAndSaveStockQuote()
        {
            var timeStart = DateTime.Now;

            double price;
            double openPrice;
            DateTime dateTime;

            CaptureStockQuote("EURUSD", out price, out openPrice, out dateTime);

            var timeEnd = DateTime.Now;
            var queryDuration = (timeEnd - timeStart).TotalSeconds;
            using (var repository = new Repository())
            {
                var sq = new StockQuote
                {
                    Val = price,
                    Date = dateTime,
                    InsertDate = DateTime.Now,
                    QueryDuration = queryDuration
                };
                repository.SaveStockQuote(sq);

                var emailAlreadyRecentlySent = false;
                for (var p = 1; p <= 2; p++)
                {
                    var prevSqs = repository.RetrieveBetween<StockQuote>("ID", sq.ID - (p * QueryCallsPerMinute), sq.ID - 1);
                    if (prevSqs == null || !prevSqs.Any())
                        continue;

                    var val = sq.Val.GetValueOrDefault();
                    var min = prevSqs.Min(s => s.Val.GetValueOrDefault());
                    var max = prevSqs.Max(s => s.Val.GetValueOrDefault());

                    double diffToPrev;
                    if (Math.Abs(val - min) > Math.Abs(val - max))
                        diffToPrev = val - min;
                    else
                        diffToPrev = val - max;

                    if (p == 1)
                        sq.DiffToPrev = diffToPrev;
                    else
                        sq.DiffToPrevPrev = diffToPrev;

                    if (!emailAlreadyRecentlySent)
                        emailAlreadyRecentlySent = prevSqs.Any(s => s.EmailAlertSent.GetValueOrDefault());
                }

                repository.SaveStockQuote(sq);

                if (!emailAlreadyRecentlySent && Math.Abs(sq.DiffToPrev.GetValueOrDefault()) >= Math.Abs(EmailWarningThreshold))
                {
                    SendMail(sq);
                    
                    sq.EmailAlertSent = true;
                    repository.SaveStockQuote(sq);
                }
            }
        }

        static string GetStockXml(string yahooSymbol)
        {
            // Error: "No definition found for Table quotes"
            // replace table "yahoo.finance.quotes" with "quotes"
            // Fix: use "http://github.com/spullara/yql-tables/raw/d60732fd4fbe72e5d5bd2994ff27cf58ba4d3f84/yahoo/finance/yahoo.finance.quotes.xml" as quotes;

            const string tableFix = "use \"http://github.com/spullara/yql-tables/raw/d60732fd4fbe72e5d5bd2994ff27cf58ba4d3f84/yahoo/finance/yahoo.finance.quotes.xml\" as quotes;";
            var tableFixUrlEncoded = HttpUtility.UrlEncode(tableFix);

            var url = string.Format("http://query.yahooapis.com/v1/public/yql?q={1}select%20*%20from%20quotes%20where%20symbol%3D%22{0}%22&diagnostics=true&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys", 
                yahooSymbol, tableFixUrlEncoded);

            try
            {
                //Create Request
                var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);     //Declare an HTTP-specific implementation of the WebRequest class.
                myHttpWebRequest.Method = "GET";
                myHttpWebRequest.ContentType = "text/xml";

                //Get Response
                var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();   //Declare an HTTP-specific implementation of the WebResponse class
                var stream = myHttpWebResponse.GetResponseStream();
                if (stream == null)
                    return "";

                //Now load the XML Document
                var myXmlDocument = new XmlDocument();           //Declare XMLResponse document

                //Load response stream into XMLReader
                var myXmlReader = new XmlTextReader(stream);           //Declare XMLReader
                myXmlDocument.Load(myXmlReader);

                var xml = myXmlDocument.InnerXml;

                return xml;
            }
            catch (Exception myException)
            {
                throw new Exception("Error Occurred in AuditAdapter.getXMLDocumentFromXMLTemplate()", myException);
            }
        }

        static string GetAppSettingsVal(string key)
        {
            return ConfigurationManager.AppSettings[key] ?? "";
        }

        static void SendMail(StockQuote sq)
        {
            if (Repository.IsLocalhost)
                return;

            SetSmtpSettings();

            WebMail.Send(
                 to: "jenzenm@gmail.com,matthias.jenzen@kroschke.de",
                 subject: string.Format("{0} - Alert", "StockCapture"),
                 body: string.Format(
                        "DateTime: {0:dd.MM.yy HH:mm}<br/>" +
                        "Diff To Prev: {1} ticks<br/>" +
                        "Diff To PrevPrev: {2} ticks<br/><br/>Thx ;-)",
                            sq.Date,
                            sq.DiffToPrev.GetValueOrDefault()*1000,
                            sq.DiffToPrevPrev.GetValueOrDefault()*1000)
            );
        }

        static void SetSmtpSettings()
        {
            WebMail.SmtpServer = GetAppSettingsVal("SmtpServer");
            WebMail.SmtpPort = Int32.Parse(GetAppSettingsVal("SmtpPort"));
            WebMail.EnableSsl = GetAppSettingsVal("SmtpEnableSsl") == "true";
            WebMail.UserName = GetAppSettingsVal("SmtpUserName");
            WebMail.From = GetAppSettingsVal("SmtpFrom");
            WebMail.Password = GetAppSettingsVal("SmtpPassword");
        }
    }
}
