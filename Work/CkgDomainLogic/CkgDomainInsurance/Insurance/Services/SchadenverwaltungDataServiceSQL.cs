using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Insurance.Contracts;
using CkgDomainLogic.Insurance.Models;
using GeneralTools.Models;
using SapORM.Contracts;

namespace CkgDomainLogic.Insurance.Services
{
    public class SchadenverwaltungDataServiceSQL : CkgGeneralDataService, ISchadenverwaltungDataService
    {
        public List<Schadenfall> Schadenfaelle { get { return PropertyCacheGet(() => LoadSchadenfaelleFromSql().ToList()); } }

        public List<VersEvent> Events { get { return PropertyCacheGet(() => LoadEventsFromSql().ToList()); } }

        public List<Versicherung> Versicherungen { get { return PropertyCacheGet(() => LoadVersicherungenFromSql().ToList()); } }

        public void MarkForRefreshSchadenfaelle()
        {
            PropertyCacheClear(this, m => m.Schadenfaelle);
        }

        public void MarkForRefreshEvents()
        {
            PropertyCacheClear(this, m => m.Events);
        }

        public void MarkForRefreshVersicherungen()
        {
            PropertyCacheClear(this, m => m.Versicherungen);
        }

        private IEnumerable<Schadenfall> LoadSchadenfaelleFromSql()
        {
            var liste = new List<Schadenfall>();

            using (var conn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Schadenfall WHERE KundenNr=@KundenNr";
                    cmd.Parameters.AddWithValue("@KundenNr", LogonContext.KundenNr.ToSapKunnr());

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var eventId = Int32.Parse(dr["EventID"].ToString());
                            var evt = Events.Find(e => e.ID == eventId);
                            var eventName = (evt != null ? evt.EventName : "");
                            var versId = dr["VersicherungID"].ToString();
                            //var vers = Versicherungen.Find(v => v.ID == versId);
                            var versName = ""; // (vers != null ? vers.Name : "");

                            liste.Add(new Schadenfall
                                {
                                    ID = (int)dr["ID"], 
                                    EventID = eventId,
                                    EventName = eventName,
                                    VersicherungID = versId,
                                    Versicherung = versName,
                                    Kennzeichen = dr["Kennzeichen"].ToString(),
                                    Vorname = dr["Vorname"].ToString(),
                                    Nachname = dr["Nachname"].ToString(),
                                    Email = dr["Email"].ToString(),
                                    TelefonNr = dr["Telefon"].ToString(),
                                    FzgHersteller = dr["FzgHersteller"].ToString(),
                                    FzgModell = dr["FzgModell"].ToString(),
                                    SelbstbeteiligungsHoehe = dr["SBHoehe"].ToString(),
                                    Referenznummer = dr["Referenznummer"].ToString(),
                                    Sammelbesichtigung = (dr["Sammelbesichtigung"].ToString() == "X")
                                });
                        }
                    }
                }

                conn.Close();
            }

            return liste;
        }

        private IEnumerable<VersEvent> LoadEventsFromSql()
        {
            var liste = new List<VersEvent>();

            using (var conn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT ID, EventName, AnlageDatum FROM VersEvent WHERE LoeschDatum IS NULL AND KundenNr=@KundenNr ORDER BY AnlageDatum DESC";
                    cmd.Parameters.AddWithValue("@KundenNr", Int32.Parse(LogonContext.KundenNr));

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            liste.Add(new VersEvent
                            {
                                ID = Int32.Parse(dr["ID"].ToString()),
                                EventName = dr["EventName"].ToString(),
                                AnlageDatum = DateTime.Parse(dr["AnlageDatum"].ToString())
                            });
                        }
                    }
                }

                conn.Close();
            }

            return liste;
        }

        private IEnumerable<Versicherung> LoadVersicherungenFromSql()
        {
            var liste = new List<Versicherung>();

            using (var conn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT VersichererID, Name1 FROM Versicherer WHERE CustomerID=@CustomerID ORDER BY Name1 ASC";
                    var logonContextDataService = LogonContext as ILogonContextDataService;
                    if (logonContextDataService != null)
                        cmd.Parameters.AddWithValue("@CustomerID", logonContextDataService.Customer.CustomerID);

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            liste.Add(new Versicherung
                            {
                                ID = Int32.Parse(dr["VersichererID"].ToString()),
                                Name = dr["Name1"].ToString()
                            });
                        }
                    }
                }

                conn.Close();
            }

            return liste;
        }

        public string SaveSchadenfall(Schadenfall schadenfall)
        {
            var erg = "";

            // Neuer Datensatz
            var blnInsert = schadenfall.ID == 0; // String.IsNullOrEmpty(schadenfall.ID);

            using (var conn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    if (blnInsert)
                    {
                        // Neuer Datensatz
                        cmd.CommandText = "INSERT INTO Schadenfall (EventID,VersicherungID,Kennzeichen,Vorname,Nachname,Email,Telefon,FzgHersteller,FzgModell,SBHoehe," 
                            + "Referenznummer,Sammelbesichtigung,KundenNr) VALUES (@EventID,@VersicherungID,@Kennzeichen,@Vorname,@Nachname,@Email,@Telefon,@FzgHersteller,@FzgModell,@SBHoehe," 
                            + "@Referenznummer,@Sammelbesichtigung,@KundenNr)";         
                    }
                    else
                    {
                        // Bereits vorhandener Datensatz
                        cmd.CommandText = "UPDATE Schadenfall SET EventID=@EventID,VersicherungID=@VersicherungID,Kennzeichen=@Kennzeichen,Vorname=@Vorname,Nachname=@Nachname," 
                            + "Email=@Email,Telefon=@Telefon,FzgHersteller=@FzgHersteller,FzgModell=@FzgModell,SBHoehe=@SBHoehe,Referenznummer=@Referenznummer," 
                            + "Sammelbesichtigung=@Sammelbesichtigung WHERE KundenNr=@KundenNr AND ID=@ID";
                    }

                    cmd.Parameters.AddWithValue("@EventID", schadenfall.EventID);
                    cmd.Parameters.AddWithValue("@VersicherungID", schadenfall.VersicherungID);
                    cmd.Parameters.AddWithValue("@Kennzeichen", schadenfall.Kennzeichen.NotNullOrEmpty().ToUpper());
                    cmd.Parameters.AddWithValue("@Vorname", schadenfall.Vorname ?? "");
                    cmd.Parameters.AddWithValue("@Nachname", schadenfall.Nachname ?? "");
                    cmd.Parameters.AddWithValue("@Email", schadenfall.Email ?? "");
                    cmd.Parameters.AddWithValue("@Telefon", schadenfall.TelefonNr ?? "");
                    cmd.Parameters.AddWithValue("@FzgHersteller", schadenfall.FzgHersteller ?? "");
                    cmd.Parameters.AddWithValue("@FzgModell", schadenfall.FzgModell ?? "");
                    cmd.Parameters.AddWithValue("@SBHoehe", schadenfall.SelbstbeteiligungsHoehe ?? "");
                    cmd.Parameters.AddWithValue("@Referenznummer", schadenfall.Referenznummer ?? "");
                    cmd.Parameters.AddWithValue("@Sammelbesichtigung", (schadenfall.Sammelbesichtigung ? "X" : ""));
                    cmd.Parameters.AddWithValue("@KundenNr", LogonContext.KundenNr.ToSapKunnr());
                    if (!blnInsert)
                    {
                        cmd.Parameters.AddWithValue("@ID", schadenfall.ID);
                    }                  

                    var anzahl = cmd.ExecuteNonQuery();

                    if (anzahl == 0)
                    {
                        erg = Localize.SaveFailed;
                    }
                }

                conn.Close();
            }

            return erg;
        }

        public string DeleteSchadenfall(string id)
        {
            var erg = "";

            using (var conn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Schadenfall WHERE KundenNr = @KundenNr AND ID = @ID";
                    cmd.Parameters.AddWithValue("@KundenNr", LogonContext.KundenNr.ToSapKunnr());
                    cmd.Parameters.AddWithValue("@ID", Int32.Parse(id));

                    var anzahl = cmd.ExecuteNonQuery();

                    if (anzahl == 0)
                    {
                        erg = Localize.DeleteFailed;
                    }
                }

                conn.Close();
            }

            return erg;
        }
    }
}
