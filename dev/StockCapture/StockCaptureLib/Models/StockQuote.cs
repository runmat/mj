// ReSharper disable InconsistentNaming
using System;

namespace StockCapture.Models
{
    public class StockQuote
    {
        public virtual int ID { get; protected set; }

        public virtual double? Val { get; set; }

        public virtual DateTime? Date { get; set; }

        public virtual DateTime? InsertDate { get; set; }

        public virtual double? DiffToPrev { get; set; }
        public virtual double DiffToPrevTicks { get { return DiffToPrev.GetValueOrDefault() * 1000; } }

        public virtual double? DiffToPrevPrev { get; set; }
        public virtual double DiffToPrevPrevTicks { get { return DiffToPrevPrev.GetValueOrDefault() * 1000; } }

        public virtual double? QueryDuration { get; set; }

        public virtual bool? EmailAlertSent { get; set; }

        public virtual string EmailAlertSentAsString { get { return EmailAlertSent.GetValueOrDefault() ? "X" : ""; } }
    }
}



//CREATE TABLE [dbo].[StockQuote](
//    [ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
//    [Val] [money] NULL,
//    [Date] [datetime] NULL,
//    [InsertDate] [datetime] NULL,
//    CONSTRAINT [PK_Stocks] PRIMARY KEY CLUSTERED 
//    (   
//        [ID] ASC
//    )
//)
