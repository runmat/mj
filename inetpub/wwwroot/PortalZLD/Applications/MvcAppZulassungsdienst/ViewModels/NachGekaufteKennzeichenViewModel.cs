using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using CKG.Base.Kernel.Security;
using MvcAppZulassungsdienst.Models;
using MvcAppZulassungsdienst.SAP;

namespace MvcAppZulassungsdienst.ViewModels
{
    public class NachGekaufteKennzeichenViewModel
    {
        public IEnumerable<KennzeichenKopf> KennzeichenKopfList { get; private set; }

        public IEnumerable<KennzeichenPosition> GetKennzeichenPosList(string bstnr)
        {
            var connString = ConfigurationManager.AppSettings["Connectionstring"];
            var user = new User(UserID, ConfigurationManager.AppSettings["Connectionstring"]);
            var mode = user.IsTestUser;
            var app = new App(user);
            var form = new System.Web.UI.Page();
            form.Session["AppId"] = "AppZulassungsdienst.NachGekaufteKennzeichen";

            var kopf = KennzeichenKopfList.FirstOrDefault(k => k.Bstnr == bstnr);
            if (kopf == null) 
                return new List<KennzeichenPosition>();
            
            return Ngk.TblErfassteKennzeichenPositionen.Select("bstnr = " + bstnr).AsEnumerable()
                .Select(r => new KennzeichenPosition
                    {
                        Bstnr = (string) r["Bstnr"],
                        MatNr = (string) r["MatNr"],
                        MakTx = (string) r["MakTx"],
                        Menge = (decimal?) r["Menge"],
                        Preis = (decimal?) r["Preis"],
                        EeInd = (DateTime?)r["EeInd"],
                        BeDat = kopf.BeDat,
                        LtextNr = (string)r["Ltext_Nr"],
                        Ltext = new LongStringToSap(user, app, form).ReadString((string)r["Ltext_Nr"]),
                    });
        }

        public int UserID { get; set; }

        public string LieferantenNr { get; set; }

        public string Lieferant
        {
            get
            {
                if (Ngk == null) return "";

                var liefTable = Ngk.GetLieferanten();
                if (liefTable == null) 
                    return "-";
                var liefRow = liefTable.AsEnumerable().FirstOrDefault(r => (string)r["LIFNR"] == LieferantenNr);
                if (liefRow != null)    
                    return (string) liefRow["NAME1"];
                return "-";
            }
        }

        public int KopfCount
        {
            get
            {
                if (Ngk == null) return 0;

                if (Ngk.TblErfassteKennzeichenKopf != null && Ngk.TblErfassteKennzeichenKopf.Rows != null)
                    return Ngk.TblErfassteKennzeichenKopf.Rows.Count;
                
                return 0;
            }
        }

        //public string Test
        //{
        //    get { return ConfigurationManager.AppSettings["ErpConnectLicense"]; }
        //}

        //public string SapMessage
        //{
        //    get { return Ngk.SapMessage; }
        //}

        private NacherfassungGekaufteKennzeichen Ngk { get; set; }

        public void Init(string id)
        {
            KennzeichenKopfList = null;

            if (string.IsNullOrEmpty(id) || !id.Contains("_"))
                return;

            var parameters = id.Split('_');
            int userId;
            if (parameters.Count() != 2 || !Int32.TryParse(parameters[0], out userId))
                return;

            UserID = userId; //3385;
            LieferantenNr = parameters[1]; // "0000900153";


            var user = new User(UserID, ConfigurationManager.AppSettings["Connectionstring"]);
            var app = new App(user);
            var form = new System.Web.UI.Page();
            form.Session["AppId"] = "AppZulassungsdienst.NachGekaufteKennzeichen";

            Ngk = new NacherfassungGekaufteKennzeichen(ref user, app, "", "", form, LieferantenNr);
            
            if (Ngk.TblErfassteKennzeichenKopf == null)
                return;
            KennzeichenKopfList = Ngk.TblErfassteKennzeichenKopf.AsEnumerable().Select(r => new KennzeichenKopf
            {
                Bstnr = (string)r["Bstnr"],
                BeDat = (DateTime?)r["BeDat"],
                EeInd = (DateTime?)r["EeInd"],
                LiefersNr = (string)r["LiefersNr"],
                Name1 = (string)r["Name1"],
            }).ToList();

        }
    }
}