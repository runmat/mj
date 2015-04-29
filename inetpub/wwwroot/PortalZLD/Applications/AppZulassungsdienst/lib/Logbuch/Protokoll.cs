using System;
using System.Collections.Generic;
using System.Data;
using CKG.Base.Business;

namespace AppZulassungsdienst.lib.Logbuch
{
    public class Protokoll : SapOrmBusinessBase
    {
        public enum Side
        {
            Input = 0,
            Output = 1
        }

        private LogbuchEntry[,] protokoll;
        private int length;
        private List<VorgangsartDetails> lstVorgangsarten;

        public DataTable ProtokollTabelle { get; private set; }

        public Protokoll(ref List<VorgangsartDetails> lstVorgänge)
        {
            protokoll = new LogbuchEntry[1, 2];
            lstVorgangsarten = lstVorgänge;
        }

        internal void addEntry(Side Side, LogbuchEntry eintrag)
        {
            int index;

            if (eintrag.LaufendeNummer == "")
            {
                index = FindChildEntry(Side, eintrag.VorgangsID, eintrag.LaufendeNummer);         
            }
            else
            {
                index = FindParentEntry(Side, eintrag.VorgangsID, eintrag.AntwortAufLaufendenummer);
            }

            if (index != -1)
            {
                protokoll[index, (int)Side] = eintrag;
            }
            else
            {
                length++;
                ExtendArray();
                protokoll[length - 1, (int)Side] = eintrag;
            }
        }

        public string GetAntwortToVorgangsart(string vgart)
        {
            VorgangsartDetails Antwort = lstVorgangsarten.Find(v => v.Vorgangsart == vgart);
            return Antwort.Antwortart.ToString();
        }

        public void EintragAbschliessen(int rowindex, EntryStatus status, string userName)
        {
            Ausgang Aus = (Ausgang)protokoll[rowindex, (int)Side.Output];
            Aus.EintragAbschliessen(status, userName);
            
            if (Aus.ErrorOccured)
                RaiseError(Aus.ErrorCode, Aus.Message);
        }

        public void EintragBeantworten(int rowindex, EmpfängerStatus status, string userName)
        {
            Eingang Ein = (Eingang)protokoll[rowindex, (int)Side.Input];
            Ein.EintragBeantworten(status, userName);

            if (Ein.ErrorOccured)
                RaiseError(Ein.ErrorCode, Ein.Message);
        }

        public void EintragBeantworten(int rowindex, string Betreff, string Text, string userName)
        {
            Eingang Ein = (Eingang)protokoll[rowindex, (int)Side.Input];
            Ein.EintragBeantworten(Betreff, Text, userName);

            if (Ein.ErrorOccured)
                RaiseError(Ein.ErrorCode, Ein.Message);
        }

        public void Rückfrage(int rowindex, string Betreff, string Text, string userName, string kostenstelle)
        {
            Eingang Ein = (Eingang)protokoll[rowindex, (int)Side.Input];
            Ein.Rückfrage(Betreff, Text, userName, kostenstelle);

            if (Ein.ErrorOccured)
                RaiseError(Ein.ErrorCode, Ein.Message);
        }

        private void ExtendArray()
        {
            LogbuchEntry[,] oldArray = (LogbuchEntry[,])protokoll.Clone();
            protokoll = new LogbuchEntry[protokoll.GetLength(0) + 1, 2];

            for (int i = 0; i < oldArray.GetLength(0); i++)
            {
                protokoll[i, 0] = oldArray[i, 0];
                protokoll[i, 1] = oldArray[i, 1];
            }
        }

        private int FindParentEntry(Side side, string vorgangsid, string antwortnummer)
        {
            Side OtherSide;

            if (side == Side.Input)
            {
                OtherSide = Side.Output;
            }
            else
            {
                OtherSide = Side.Input;
            }

            for (int i = 0; i <= protokoll.GetUpperBound(0); i++)
            {
                if (protokoll[i, (int)OtherSide] != null)
                {
                    if ((protokoll[i, (int)OtherSide].VorgangsID == vorgangsid) && (protokoll[i, (int)OtherSide].LaufendeNummer == antwortnummer))
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        private int FindChildEntry(Side side, string vorgangsid, string Laufendenummer)
        {
            Side OtherSide;

            if (side == Side.Input)
            {
                OtherSide = Side.Output;
            }
            else
            {
                OtherSide = Side.Input;
            }

            for (int i = 0; i <= protokoll.GetUpperBound(0); i++)
            {
                if (protokoll[i, (int)OtherSide] != null)
                {
                    if ((protokoll[i, (int)OtherSide].VorgangsID == vorgangsid) && (protokoll[i, (int)OtherSide].AntwortAufLaufendenummer == Laufendenummer))
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// Erzeugt eine aufbereitete DataTable aus dem vorhandenen Protokoll
        /// </summary>
        /// <param name="FilterInput">Filterwert für den Status im Protokoll Input, NULL-Wert entspricht alles anzeigen</param>
        /// <param name="FilterOutput">Filterwert für den Status im Protokoll Output, NULL-Wert entspricht alles anzeigen</param>
        /// <param name="FilterInputE">Filterwert für den StatusEmpfänger im Protokoll Input, NULL-Wert entspricht alles anzeigen</param>
        /// <param name="FilterOutputE">Filterwert für den StatusEmpfänger im Protokoll Output, NULL-Wert entspricht alles anzeigen</param>
        /// <returns>Protokolltabelle</returns>
        /// <remarks></remarks>
        public DataTable CreateTable(EntryStatus? FilterInput = null, EntryStatus? FilterOutput = null, 
            EmpfängerStatus? FilterInputE = null, EmpfängerStatus? FilterOutputE = null)
        {
            ProtokollTabelle = new DataTable();

            ProtokollTabelle.Columns.Add("Rowindex");

            ProtokollTabelle.Columns.Add("I_VORGID");
            ProtokollTabelle.Columns.Add("I_LFDNR");
            ProtokollTabelle.Columns.Add("I_DATUM", typeof(DateTime));
            ProtokollTabelle.Columns.Add("I_VON");
            ProtokollTabelle.Columns.Add("I_VERTR");
            ProtokollTabelle.Columns.Add("I_BETREFF");
            ProtokollTabelle.Columns.Add("I_LTXNR");
            ProtokollTabelle.Columns.Add("I_ANTW_LFDNR");
            ProtokollTabelle.Columns.Add("I_STATUS");
            ProtokollTabelle.Columns.Add("I_STATUSE");
            ProtokollTabelle.Columns.Add("I_VGART");
            ProtokollTabelle.Columns.Add("I_ZERLDAT");

            ProtokollTabelle.Columns.Add("I_HASLANGTEXT", typeof(bool));
            ProtokollTabelle.Columns.Add("I_ERL", typeof(bool));
            ProtokollTabelle.Columns.Add("I_READ", typeof(bool));
            ProtokollTabelle.Columns.Add("I_ANTW", typeof(bool));
            ProtokollTabelle.Columns.Add("I_TRENN", typeof(bool));

            ProtokollTabelle.Columns.Add("O_VORGID");
            ProtokollTabelle.Columns.Add("O_LFDNR");
            ProtokollTabelle.Columns.Add("O_DATUM", typeof(DateTime));
            ProtokollTabelle.Columns.Add("O_AN");
            ProtokollTabelle.Columns.Add("O_VERTR");
            ProtokollTabelle.Columns.Add("O_BETREFF");
            ProtokollTabelle.Columns.Add("O_LTXNR");
            ProtokollTabelle.Columns.Add("O_ANTW_LFDNR");
            ProtokollTabelle.Columns.Add("O_STATUS");
            ProtokollTabelle.Columns.Add("O_STATUSE");
            ProtokollTabelle.Columns.Add("O_VGART");
            ProtokollTabelle.Columns.Add("O_ZERLDAT");

            ProtokollTabelle.Columns.Add("O_HASLANGTEXT", typeof(bool));
            ProtokollTabelle.Columns.Add("O_LOE", typeof(bool));
            ProtokollTabelle.Columns.Add("O_CLO", typeof(bool));
            ProtokollTabelle.Columns.Add("O_TRENN", typeof(bool));

            ProtokollTabelle.Columns.Add("DEBUG", typeof(bool));

            ProtokollTabelle.AcceptChanges();

            for (int i = 0; i < length; i++)
            {
                bool bExit = false;

                // Gelöschte Datensätze überspringen
                if ((protokoll[i, (int)Side.Input] != null) && (protokoll[i, (int)Side.Input].Status == EntryStatus.Gelöscht))
                {
                    bExit = true;
                }
                if ((protokoll[i, (int)Side.Output] != null) && (protokoll[i, (int)Side.Output].Status == EntryStatus.Gelöscht))
                {
                    bExit = true;
                }

                if (!bExit)
                {
                    bool bIsInput = true;
                    bool bIsOutput = true;
                    bool bIsInputE = true;
                    bool bIsOutputE = true;

                    if (FilterInput != null)
                    {
                        if ((protokoll[i, (int)Side.Input] == null) || (protokoll[i, (int)Side.Input].Status != FilterInput))
                        {
                            bIsInput = false;
                        }
                    }

                    if (FilterOutput != null)
                    {
                        if ((protokoll[i, (int)Side.Output] == null) || (protokoll[i, (int)Side.Output].Status != FilterOutput))
                        {
                            bIsOutput = false;
                        }
                    }

                    if (FilterInputE != null)
                    {
                        if ((protokoll[i, (int)Side.Input] == null) || (protokoll[i, (int)Side.Input].StatusEmpfänger != FilterInputE))
                        {
                            bIsInputE = false;
                        }
                    }

                    if (FilterOutputE != null)
                    {
                        if ((protokoll[i, (int)Side.Output] == null) || (protokoll[i, (int)Side.Output].StatusEmpfänger != FilterOutputE))
                        {
                            bIsOutputE = false;
                        }
                    }

                    // Wenn weder Input noch Output den gesuchten Wert enthalten, dann Exit
                    bExit = (!bIsInput && !bIsOutput && !bIsInputE && !bIsOutputE);
                }

                if (!bExit)
                {
                    DataRow Row = ProtokollTabelle.NewRow();
                    Row["Rowindex"] = i;

                    Row["I_HASLANGTEXT"] = false;
                    Row["I_ERL"] = false;
                    Row["I_READ"] = false;
                    Row["I_ANTW"] = false;
                    Row["I_TRENN"] = false;

                    Row["O_HASLANGTEXT"] = false;
                    Row["O_LOE"] = false;
                    Row["O_CLO"] = false;
                    Row["O_TRENN"] = false;

                    if (protokoll[i, (int)Side.Input] != null)
                    {
                        Eingang Ein = (Eingang)protokoll[i, (int)Side.Input];

                        Row["I_VORGID"] = Ein.VorgangsID;
                        Row["I_LFDNR"] = Ein.LaufendeNummer;
                        Row["I_DATUM"] = Ein.Erfasst;
                        Row["I_VON"] = Ein.Verfasser;
                        Row["I_VERTR"] = Ein.Vertreter;
                        Row["I_BETREFF"] = Ein.Betreff;
                        Row["I_LTXNR"] = Ein.Langtextnummer;
                        Row["I_ANTW_LFDNR"] = Ein.AntwortAufLaufendenummer;
                        Row["I_STATUS"] = Ein.Status;
                        Row["I_STATUSE"] = Ein.StatusEmpfänger;
                        Row["I_VGART"] = Ein.Vorgangsart;
                        Row["I_ZERLDAT"] = Ein.ZuErledigenBis;
                        Row["I_HASLANGTEXT"] = (Ein.Langtextnummer.Length > 0);

                        // Steuert die Anzeige von Erledigt-, Gelesen- und Antwortbuttons
                        if ((protokoll[i, (int)Side.Input].Status != EntryStatus.Geschlossen)
                            && (protokoll[i, (int)Side.Input].Status != EntryStatus.Gelöscht)
                            && (protokoll[i, (int)Side.Input].StatusEmpfänger == EmpfängerStatus.Neu))
                        {
                            switch (GetAntwortToVorgangsart(Ein.Vorgangsart))
                            {
                                case "E":
                                    Row["I_ERL"] = true;
                                    break;
                                case "G":
                                    Row["I_READ"] = true;
                                    break;
                                case "A":
                                    Row["I_ANTW"] = true;
                                    break;
                            }
                        }

                        Row["I_TRENN"] = true;
                    }

                    if (protokoll[i, (int)Side.Output] != null)
                    {
                        Ausgang Aus = (Ausgang)protokoll[i, (int)Side.Output];

                        Row["O_VORGID"] = Aus.VorgangsID;
                        Row["O_LFDNR"] = Aus.LaufendeNummer;
                        Row["O_DATUM"] = Aus.Erfasst;
                        Row["O_AN"] = Aus.Empfänger;
                        Row["O_VERTR"] = Aus.Vertreter;
                        Row["O_BETREFF"] = Aus.Betreff;
                        Row["O_LTXNR"] = Aus.Langtextnummer;
                        Row["O_ANTW_LFDNR"] = Aus.AntwortAufLaufendenummer;
                        Row["O_STATUS"] = Aus.Status;
                        Row["O_STATUSE"] = Aus.StatusEmpfänger;
                        Row["O_VGART"] = Aus.Vorgangsart;
                        Row["O_ZERLDAT"] = Aus.ZuErledigenBis;
                        Row["O_HASLANGTEXT"] = (Aus.Langtextnummer.Length > 0);

                        // Steuert die Anzeige von Lösch- und Schließbuttons
                        if ((protokoll[i, (int)Side.Output].Status != EntryStatus.Geschlossen)
                            && (protokoll[i, (int)Side.Output].Status != EntryStatus.Gelöscht))
                        {
                            Row["O_LOE"] = true;
                            Row["O_CLO"] = true;
                        }

                        Row["O_TRENN"] = true;
                    }

                    Row["DEBUG"] = false;
                    
                    #if DEBUG
                        Row["DEBUG"] = true; 
                    #endif

                        ProtokollTabelle.Rows.Add(Row);
                }
            }

            ProtokollTabelle.AcceptChanges();

            return ProtokollTabelle;
        }
    }
}

