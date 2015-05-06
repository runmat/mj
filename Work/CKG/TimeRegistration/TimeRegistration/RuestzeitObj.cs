
namespace TimeRegistration
{
    struct RuestzeitObj
    {
        string sRuestkey;
        string strName;
        bool bMulti;
        string sZeit;

        public string Ruestzeitschluessel
        {
            get {return sRuestkey; }
        }

        public string Name
        {
            get { return strName; }
        }

        public bool Multi
        {
            get { return bMulti; }
        }

        public string Zeit
        {
            get {return sZeit; }
        }

        public RuestzeitObj(string Ruestschluessel, string Name, bool Multi,string Zeit)
        {
            sRuestkey = Ruestschluessel;
            strName = Name;
            bMulti = Multi;
            sZeit = Zeit;
        }
    }
}
