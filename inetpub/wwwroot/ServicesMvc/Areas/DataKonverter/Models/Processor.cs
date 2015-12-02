using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesMvc.Areas.DataKonverter.Models
{
    public class Processor
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public Operation Operation { get; set; }
        public string OperationPara1 { get; set; }
        public string OperationPara2 { get; set; }
        public string Input { get; set; }

        public string Output
        {
            get { return CalcOutput(); }
        }

        public int PosLeft { get; set; }
        public int PosTop { get; set; }

        public Processor()
        {
            Guid = System.Guid.NewGuid().ToString();
            Operation = Operation.Fix;    // Default
            OperationPara1 = "X";
        }

        private string CalcOutput()
        {
            switch (Operation)
            {
                    case Operation.Fix:
                    return OperationPara1;

                    case Operation.Merge:
                    try
                    {
                        return Input.Replace("|", OperationPara1);
                    }
                    catch (Exception e)
                    {
                        return Input;
                    }

                    case Operation.Split:
                    try
                    {
                        var arrString = Input.Split(Convert.ToChar(OperationPara1));
                        return arrString[Convert.ToInt32(OperationPara2)];
                    }
                    catch (Exception e)
                    {
                        return Input;
                    }
            }
            return null;
        }
    }
}