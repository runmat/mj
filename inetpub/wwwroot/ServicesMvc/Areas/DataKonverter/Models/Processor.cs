using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesMvc.Areas.DataKonverter.Models
{
    public class Processor
    {
        private const int _defaultLeft = 700;
        private const int _defaultTop = 340;

        public string Guid { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
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

        public Processor(int number = 0, Operation operation = Operation.Fix, string para1 = "X", string para2 = "", int posLeft = 0, int posTop = 0)
        {
            var rnd = new Random();

            Guid = System.Guid.NewGuid().ToString();
            Number = number;
            Operation = operation;
            OperationPara1 = para1;
            OperationPara2 = para2;

            // Set first X/Y position of HTML div container
            if (posLeft == 0)
                posLeft = _defaultLeft + rnd.Next(-20, 20);

            if (posTop == 0)
                posTop = _defaultTop + rnd.Next(-20, 20);

            PosLeft = posLeft;
            PosTop = posTop;
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
                default:
                    return Input;
            }
            return null;
        }
    }
}