namespace TimeRegistration
{
    public struct PDFPrintObj
    {
        string _Kartennummer;
        string _VDate;
        string _BDate;

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

        public PDFPrintObj(string kartennummer, string vdate, string bdate)
        {
            _Kartennummer = kartennummer;
            _VDate = vdate;
            _BDate = bdate;
        }
    }
}
