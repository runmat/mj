using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using GeneralTools.UnitTests.ModelMappings;

namespace ServicesMvc.Areas.DataKonverter.Models
{
    public interface IDataProcessor
    {
        DataItem SplitString(string input1, char separator, int stringNo);
    }


    public class DataProcessor : IDataProcessor
    {
        public DataItem SplitString(string input1, char separator, int stringNo)
        {
            var output = input1.Split(separator);

            var dateItem = new DataItem
            {
                Input = input1,
                DestDataType = DataItem.DataType.String,
                OutputString = output[stringNo]
            };


            return dateItem;

        }
    }


}