﻿// ReSharper disable InconsistentNaming
using System;

namespace StockCapture.Models
{
    public class StockQuote
    {
        public virtual int ID { get; protected set; }

        public virtual double? Val { get; set; }

        public virtual DateTime? Date { get; set; }

        public virtual DateTime? InsertDate { get; set; }
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
