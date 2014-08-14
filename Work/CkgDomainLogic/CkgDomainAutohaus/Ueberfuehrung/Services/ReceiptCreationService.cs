using System;
using System.Data;
using System.IO;
using System.Linq;
using CkgDomainLogic.Ueberfuehrung.Models;
using CkgDomainLogic.Ueberfuehrung.ViewModels;
using DocumentTools.Services;
using GeneralTools.Contracts;

namespace CkgDomainLogic.Ueberfuehrung.Services
{
    public class ReceiptCreationService
    {
        private readonly UeberfuehrungViewModel _viewModel;
        private readonly IAppSettings _appSettings;
        private readonly ILogonContext _logonContext;

        public ReceiptCreationService(UeberfuehrungViewModel viewModel)
        {
            _viewModel = viewModel;
            _appSettings = viewModel.AppSettings;
            _logonContext = viewModel.LogonContext;
        }

        public string CreatePDF()
        {
            DataTable tbHead; DataSet dsPDF;

            PrepareDataSet(out tbHead, out dsPDF);

            return new WordDocumentFactory(null, null).CreateDocumentDataset("SummaryUeberfuehrung", 
                                                        Path.Combine(_appSettings.BinPath, @"Ueberfuehrung\Docs\Auftragsbestätigung.doc"), 
                                                        _appSettings.TempPath, tbHead, dsPDF);
        }

        void PrepareDataSet(out DataTable tbHead, out DataSet dsPDF)
        {
            dsPDF = new DataSet();

            tbHead = CreateTableHead();
            dsPDF.Tables.Add(CreateTableStammdaten());
            dsPDF.Tables.Add(CreateTableFahrten());
            dsPDF.Tables.Add(CreateTableDienstleistungen());
            dsPDF.Tables.Add(CreateTableProtokolle());
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
                CreateStringColumn(dt, "Typ" + i);
                CreateStringColumn(dt, "RefNr" + i);
                CreateStringColumn(dt, "FzgWert" + i);
                CreateStringColumn(dt, "FzgZulBereit" + i);
                CreateStringColumn(dt, "FzgZulDAD" + i);
                CreateStringColumn(dt, "Reifen" + i);
                CreateStringColumn(dt, "FzgKlasse" + i);

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
            var fahrzeugStep = _viewModel.Steps.First(s => s.GroupName == "FAHRZEUGE");
            var fahrzeug = fahrzeugStep.CurrentSubStepForms.OfType<Fahrzeug>().FirstOrDefault(m => m.FahrzeugIndex == index.ToString());
            if (fahrzeug == null || fahrzeug.IsEmpty)
            {
                row["FIN" + index] = "";
                row["Kennz" + index] = "";
                row["Typ" + index] = "";
                row["RefNr" + index] = "";
                row["FzgWert" + index] = "";
                row["FzgZulBereit" + index] = "";
                row["FzgZulDAD" + index] = "";
                row["Reifen" + index] = "";
                row["FzgKlasse" + index] = "";
                return;
            }

            row["FIN" + index] = fahrzeug.FIN;
            row["Kennz" + index] = fahrzeug.Kennzeichen;
            row["Typ" + index] = fahrzeug.Typ;
            row["RefNr" + index] = fahrzeug.Referenznummer;
            row["FzgWert" + index] = fahrzeug.Fahrzeugwert;
            row["FzgZulBereit" + index] = fahrzeug.FahrzeugZugelassen;
            row["FzgZulDAD" + index] = "";
            row["Reifen" + index] = fahrzeug.Bereifung;
            row["FzgKlasse" + index] = fahrzeug.Fahrzeugklasse;
        }

        void StammdatenTryFillAddress(DataRow row, int index)
        {
            var r = GetFzgAddressPostfix(index);

            var fahrzeugStep = _viewModel.Steps.First(s => s.GroupName == "FAHRZEUGE");
            var address = fahrzeugStep.CurrentSubStepForms.OfType<Adresse>().FirstOrDefault(m => m.SubGroupName == r);
            if (address == null || address.IsEmpty)
            {
                row["Name" + r] = "";
                row["Strasse" + r] = "";
                row["PLZ_Ort" + r] = "";
                row["APartner" + r] = "";
                row["Telefon" + r] = "";
                return;
            }

            row["Name" + r] = address.Firma;
            row["Strasse" + r] = address.Strasse;
            row["PLZ_Ort" + r] = address.PlzOrt;
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
            CreateStringColumn(dt, "Datum");
            CreateStringColumn(dt, "Uhrzeit");
            CreateStringColumn(dt, "Fahrzeug");
            CreateStringColumn(dt, "KM");

            var fahrtenStep = _viewModel.Steps.First(s => s.GroupName == "FAHRTEN");
            var allValidAddressModels = fahrtenStep.CurrentSubStepForms.OfType<Adresse>().Where(m => !m.IsEmpty).ToList();
            allValidAddressModels.ForEach(address =>
            {
                var row = dt.NewRow();

                row["Adresstyp"] = address.TransportTypName;
                row["Adresse"] = address.AdresseAsRouteInfo;
                row["APartner"] = address.Ansprechpartner;
                row["Telefon"] = address.Telefon;
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

            var dienstleistungsStep = _viewModel.Steps.First(s => s.GroupName == "DIENSTLEISTUNGEN");
            var dienstleistungsSubSteps = dienstleistungsStep.SubSteps;
            dienstleistungsSubSteps.ForEach(subStep =>
                {
                    var dienstleistungsAuswahlModel = subStep.StepForms.OfType<DienstleistungsAuswahl>().First();
                    var bemerkungModel = subStep.StepForms.OfType<Bemerkungen>().First();

                    var row = dt.NewRow();

                    row["Adresstyp"] = Adresse.GetTransportTypName(dienstleistungsAuswahlModel.FahrtTyp);
                    row["Dienstleistungen"] = string.Join(Environment.NewLine, dienstleistungsAuswahlModel.GewaehlteDienstleistungen.Select(dl => dl.Name));
                    row["Bemerkung"] = bemerkungModel.Bemerkung;

                    dt.Rows.Add(row);
                });

            return dt;
        }

        private DataTable CreateTableProtokolle()
        {
            var dt = new DataTable {TableName = "Protokolle"};

            CreateStringColumn(dt, "Adresstyp");
            CreateStringColumn(dt, "Protokolle");

            var dienstleistungsStep = _viewModel.Steps.First(s => s.GroupName == "DIENSTLEISTUNGEN");
            var dienstleistungsSubSteps = dienstleistungsStep.SubSteps;
            dienstleistungsSubSteps.ForEach(subStep =>
                {
                    var dienstleistungsAuswahlModel = subStep.StepForms.OfType<DienstleistungsAuswahl>().First();
                    var protokollModel = subStep.StepForms.OfType<UploadFiles>().First();

                    if (protokollModel.UploadProtokolle.Any())
                    {
                        var row = dt.NewRow();

                        row["Adresstyp"] = Adresse.GetTransportTypName(dienstleistungsAuswahlModel.FahrtTyp);
                        row["Protokolle"] = string.Join(", ",
                                                        protokollModel.UploadProtokolle.Select(p => p.Protokollart));

                        dt.Rows.Add(row);
                    }
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
