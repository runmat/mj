using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Insurance.Contracts;
using CkgDomainLogic.Insurance.Models;

namespace CkgDomainLogic.Insurance.Services
{
    //public class SchadenakteDataServiceSQL : CkgGeneralDataService, ISchadenakteDataService
    //{
    //    //public List<VersEvent> Events { get { return PropertyCacheGet(() => LoadEventsFromSql().ToList()); } }

    //    //public List<Versicherung> Versicherungen { get { return PropertyCacheGet(() => LoadVersicherungenFromSql().ToList()); } }

    //    public Schadenakte GetSchadenakte(string id)
    //    {
    //        var akte = new Schadenakte();

    //        using (var conn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]))
    //        {
    //            conn.Open();

    //            using (var cmd = conn.CreateCommand())
    //            {
    //                cmd.CommandText = "SELECT * FROM Schadenfall WHERE ID=@ID";
    //                cmd.Parameters.AddWithValue("@ID", Int32.Parse(id));

    //                using (var dr = cmd.ExecuteReader())
    //                {
    //                    if (dr.Read())
    //                    {
    //                        var eventId = Int32.Parse(dr["EventID"].ToString());
    //                        //var evt = Events.Find(e => e.ID == eventId);
    //                        var eventName = ""; // (evt != null ? evt.EventName : "");
    //                        var versId = dr["VersicherungID"].ToString();
    //                        //var vers = Versicherungen.Find(v => v.ID == versId);
    //                        var versName = ""; // (vers != null ? vers.Name : "");

    //                        akte.Schadenfall = new Schadenfall
    //                            {
    //                                ID = (int)dr["ID"],
    //                                EventID = eventId,
    //                                EventName = eventName,
    //                                VersicherungID = versId,
    //                                Versicherung = versName,
    //                                Kennzeichen = dr["Kennzeichen"].ToString(),
    //                                Vorname = dr["Vorname"].ToString(),
    //                                Nachname = dr["Nachname"].ToString(),
    //                                Email = dr["Email"].ToString(),
    //                                TelefonNr = dr["Telefon"].ToString(),
    //                                FzgHersteller = dr["FzgHersteller"].ToString(),
    //                                FzgModell = dr["FzgModell"].ToString(),
    //                                SelbstbeteiligungsHoehe = dr["SBHoehe"].ToString(),
    //                                Referenznummer = dr["Referenznummer"].ToString(),
    //                                Sammelbesichtigung = (dr["Sammelbesichtigung"].ToString() == "X")
    //                            };
    //                    }
    //                }
    //            }

    //            conn.Close();
    //        }

    //        return akte;
    //    }

    //    //private IEnumerable<VersEvent> LoadEventsFromSql()
    //    //{
    //    //    var liste = new List<VersEvent>();

    //    //    using (var conn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]))
    //    //    {
    //    //        conn.Open();

    //    //        using (var cmd = conn.CreateCommand())
    //    //        {
    //    //            cmd.CommandText = "SELECT ID, EventName FROM VersEvent WHERE KundenNr=@KundenNr ORDER BY ID DESC";
    //    //            cmd.Parameters.AddWithValue("@KundenNr", Int32.Parse(LogonContext.KundenNr));

    //    //            using (var dr = cmd.ExecuteReader())
    //    //            {
    //    //                while (dr.Read())
    //    //                {
    //    //                    liste.Add(new VersEvent
    //    //                    {
    //    //                        ID = Int32.Parse(dr["ID"].ToString()),
    //    //                        EventName = dr["EventName"].ToString()
    //    //                    });
    //    //                }
    //    //            }
    //    //        }

    //    //        conn.Close();
    //    //    }

    //    //    return liste;
    //    //}

    //    //private IEnumerable<Versicherung> LoadVersicherungenFromSql()
    //    //{
    //    //    var liste = new List<Versicherung>();

    //    //    using (var conn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]))
    //    //    {
    //    //        conn.Open();

    //    //        using (var cmd = conn.CreateCommand())
    //    //        {
    //    //            cmd.CommandText = "SELECT VersichererID, Name1 FROM Versicherer WHERE CustomerID=@CustomerID ORDER BY Name1 ASC";
    //    //            var logonContextDataService = LogonContext as ILogonContextDataService;
    //    //            if (logonContextDataService != null)
    //    //                cmd.Parameters.AddWithValue("@CustomerID", logonContextDataService.Customer.CustomerID);

    //    //            using (var dr = cmd.ExecuteReader())
    //    //            {
    //    //                while (dr.Read())
    //    //                {
    //    //                    liste.Add(new Versicherung
    //    //                    {
    //    //                        ID = Int32.Parse(dr["VersichererID"].ToString()),
    //    //                        Name = dr["Name1"].ToString()
    //    //                    });
    //    //                }
    //    //            }
    //    //        }

    //    //        conn.Close();
    //    //    }

    //    //    return liste;
    //    //}
    //}
}
