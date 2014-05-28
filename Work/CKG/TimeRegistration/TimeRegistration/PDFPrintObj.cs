namespace TimeRegistration
{
    public struct PDFPrintObj
    {
        string _SAPConnectionString;
        string _Kartennummer;
        string _VDate;
        string _BDate;

        public string SAPConnectionString
        {
            get{return _SAPConnectionString;}
        }
        
        public string Kartennummer 
        {
            get { return _Kartennummer; }
        }

        public string VDate
        {
            get { return _VDate; }
        }

        public string BDate
        {
            get { return _BDate; }
        }

        public PDFPrintObj(string sapconnectionstring,string kartennummer,string vdate, string bdate )
        {
            _SAPConnectionString = sapconnectionstring;
            _Kartennummer = kartennummer;
            _VDate = vdate;
            _BDate = bdate;
        }
    }
}
