using System;

namespace CkgDomainLogic.CoC.Models
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CocLayoutAttribute : Attribute 
    {
        public string Group { get; set; }

        public string GroupLabel { get; set; }

        public string Label { get; set; }

        public string Measure { get; set; }

        public int MaxLen { get; set; }

        public string SelectOptions { get; set; }

        public bool IsCocOrderEditable { get; set; }

        public bool MultiLine { get; set; }

        private int _multiLineRows = 3;
        public int MultiLineRows
        {
            get { return _multiLineRows; }
            set { _multiLineRows = value; }
        }
    }
}
