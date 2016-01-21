using System;
using System.Data;
using System.IO;
using System.Linq;
using CkgDomainLogic.Uebfuehrg.Models;
using CkgDomainLogic.Uebfuehrg.ViewModels;
using DocumentTools.Services;
using GeneralTools.Contracts;

namespace CkgDomainLogic.Uebfuehrg.Services
{
    public class ReceiptCreationService
    {
        private readonly UebfuehrgViewModel _viewModel;
        private readonly IAppSettings _appSettings;
        private readonly ILogonContext _logonContext;

        public ReceiptCreationService(UebfuehrgViewModel viewModel)
        {
            _viewModel = viewModel;
            _appSettings = viewModel.AppSettings;
            _logonContext = viewModel.LogonContext;
        }

        public string CreatePDF()
        {
            DataTable tbHead; DataSet dsPDF;

            PrepareDataSet(out tbHead, out dsPDF);

            return new WordDocumentFactory(null, null).CreateDocumentDataset("SummaryUebfuehrg", 
                                                        Path.Combine(_appSettings.BinPath, @"Uebfuehrg\Docs\Auftragsbestätigung.doc"), 
                                                        _appSettings.TempPath, tbHead, dsPDF);
        }

        void PrepareDataSet(out DataTable tbHead, out DataSet dsPDF)
        {
            dsPDF = new DataSet();

            tbHead = CreateTableHead();
            dsPDF.Tables.Add(CreateTableStammdaten());
            dsPDF.Tables.Add(CreateTableFahrten());
            dsPDF.Tables.Add(CreateTableDienstleistungen());
            dsPDF.Tables.Add(CreateTableAuftragsnummern());
        }

        DataTable CreateTableHead()
        {
            var dt = new DataTable {TableName = "Kopf"};

            CreateStringColumn(dt, "Datum");
            CreateStringColumn(dt, "User");

            var row = dt.NewRow();
            row["Datum"] = DateTime.Today.ToShortDateString();
            row["User"] = _logonContext.UserName;
            dt.Rows.Add(row);

            return dt;
        }

        DataTable CreateTableStammdaten()
        {
            var dt = new DataTable { TableName = "Stammdaten" };

            for (var i = 1; i <= 2; i++)
            {
                CreateStringColumn(dt, "FIN" + i);
                CreateStringColumn(dt, "Kennz" + i);
                CreateStringColumn(dt, "Hersteller" + i);
                CreateStringColumn(dt, "Modell" + i);
                CreateStringColumn(dt, "RefNr" + i);
                CreateStringColumn(dt, "FzgWert" + i);
                CreateStringColumn(dt, "FzgZulBereit" + i);
                CreateStringColumn(dt, "FzgZulDAD" + i);
                CreateStringColumn(dt, "FzgKlasse" + i);
                CreateStringColumn(dt, "Bereifung" + i);

                var r = GetFzgAddressPostfix(i);

                CreateStringColumn(dt, "Name" + r);
                CreateStringColumn(dt, "Strasse" + r);
                CreateStringColumn(dt, "PLZ_Ort" + r);
                CreateStringColumn(dt, "APartner" + r);
                CreateStringColumn(dt, "Telefon" + r);
            }

            
            var row = dt.NewRow();

            for (var i = 1; i <= 2; i++)
            {
                StammdatenTryFillFzg(row, i);
                StammdatenTryFillAddress(row, i);
            }

            dt.Rows.Add(row);

            
            return dt;
        }

        void StammdatenTryFillFzg(DataRow row, int index)
        {
            var fahrzeug = _viewModel.StepModels.OfType<Fahrzeug>().FirstOrDefault(m => m.FahrzeugIndex == index.ToString());
            if (fahrzeug == null)
            {
                row["FIN" + index] = "";
                row["Kennz" + index] = "";
                row["Hersteller" + index] = "";
                row["Modell" + index] = "";
                row["RefNr" + index] = "";
                row["FzgWert" + index] = "";
                row["FzgZulBereit" + index] = "";
                row["FzgZulDAD" + index] = "";
                row["FzgKlasse" + index] = "";
                row["Bereifung" + index] = "";
                return;
            }

            row["FIN" + index] = fahrzeug.FIN;
            row["Kennz" + index] = fahrzeug.Kennzeichen;
            row["Hersteller" + index] = fahrzeug.Hersteller;
            row["Modell" + index] = fahrzeug.Modell;
            row["RefNr" + index] = fahrzeug.Referenznummer;
            row["FzgWert" + index] = fahrzeug.FahrzeugwertText;
            row["FzgZulBereit" + index] = fahrzeug.FahrzeugZugelassen ? "Ja" : "Nein";
            row["FzgZulDAD" + index] = fahrzeug.ZulassungBeauftragt ? "Ja" : "Nein";
            row["FzgKlasse" + index] = fahrzeug.FahrzeugklasseConverted;
            row["Bereifung" + index] = fahrzeug.BereifungText;
        }

        void StammdatenTryFillAddress(DataRow row, int index)
        {
            var r = GetFzgAddressPostfix(index);

            var rgDaten = _viewModel.RgDatenFromStepModels;
            var address = (r == "RG" ? rgDaten.RgKunde : rgDaten.ReKunde);
            if (address == null)
            {
                row["Name" + r] = "";
                row["Strasse" + r] = "";
                row["PLZ_Ort" + r] = "";
                row["APartner" + r] = "";
                row["Telefon" + r] = "";
                return;
            }

            row["Name" + r] = address.Name1;
            row["Strasse" + r] = address.Strasse;
            row["PLZ_Ort" + r] = address.PLZ + " " + address.Ort;
            row["APartner" + r] = address.Ansprechpartner;
            row["Telefon" + r] = address.Telefon;
        }

        DataTable CreateTableFahrten()
        {
            var dt = new DataTable { TableName = "Fahrten" };

            CreateStringColumn(dt, "Adresstyp");
            CreateStringColumn(dt, "Adresse");
            CreateStringColumn(dt, "APartner");
            CreateStringColumn(dt, "Telefon");
            CreateStringColumn(dt, "Email");
            CreateStringColumn(dt, "Datum");
            CreateStringColumn(dt, "Uhrzeit");
            CreateStringColumn(dt, "Fahrzeug");
            CreateStringColumn(dt, "KM");

            var allValidAddressModels = _viewModel.StepModels.OfType<Adresse>().ToList();
            allValidAddressModels.ForEach(address =>
            {
                var row = dt.NewRow();

                row["Adresstyp"] = address.TransportTypName;
                row["Adresse"] = address.AdresseAsBlock;
                row["APartner"] = address.Ansprechpartner;
                row["Telefon"] = address.Telefon;
                row["Email"] = address.Email;
                row["Datum"] = address.Datum == null ? "" : address.Datum.GetValueOrDefault().ToShortDateString();
                row["Uhrzeit"] = address.Uhrzeitwunsch;
                row["Fahrzeug"] = address.FahrzeugBezeichnung;
                row["KM"] = address.EntfernungKm;

                dt.Rows.Add(row);
            });

            return dt;
        }

        DataTable CreateTableDienstleistungen()
        {
            var dt = new DataTable { TableName = "Dienstleistungen" };

            CreateStringColumn(dt, "Adresstyp");
            CreateStringColumn(dt, "Dienstleistungen");
            CreateStringColumn(dt, "Bemerkung");

            var dienstleistungsAuswahlen = _viewModel.StepModels.OfType<DienstleistungsAuswahl>().ToList();
            dienstleistungsAuswahlen.ForEach(dienstleistungsAuswahl =>
                {
                    var row = dt.NewRow();

                    var transportTypModel = _viewModel.TransportTypen.FirstOrDefault(tt => tt.ID == dienstleistungsAuswahl.FahrtTyp);
                    if (transportTypModel != null)
                        row["Adresstyp"] = transportTypModel.Name;
                    row["Dienstleistungen"] = string.Join(Environment.NewLine, dienstleistungsAuswahl.GewaehlteDienstleistungen.Select(dl => dl.Name));
                    row["Bemerkung"] = dienstleistungsAuswahl.Bemerkungen.Bemerkung;

                    dt.Rows.Add(row);
                });

            return dt;
        }

        DataTable CreateTableAuftragsnummern()
        {
            var dt = new DataTable { TableName = "Auftragsnummer" };

            CreateStringColumn(dt, "ID");
            CreateStringColumn(dt, "Auftragsnummer");

            var i = 0;
            _viewModel.AuftragsPositionen.ForEach(auftragsPosition =>
            {
                var row = dt.NewRow();

                row["ID"] = string.Format("{0}", ++i);
                row["Auftragsnummer"] = auftragsPosition.AuftragsNrText;

                dt.Rows.Add(row);
            });

            return dt;
        }

        static string GetFzgAddressPostfix(int index)
        {
            return (index == 1 ? "RG" : "RE");
        }

        static void CreateStringColumn(DataTable table, string columnName)
        {
            table.Columns.Add(columnName, typeof (string));
        }
    }
}
