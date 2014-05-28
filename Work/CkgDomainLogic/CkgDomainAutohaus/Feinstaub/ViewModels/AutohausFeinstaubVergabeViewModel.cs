using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.Autohaus.Models;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.Feinstaub.Contracts;
using CkgDomainLogic.Feinstaub.Models;
using CkgDomainLogic.General.ViewModels;

namespace CkgDomainLogic.Feinstaub.ViewModels
{
    public class AutohausFeinstaubVergabeViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IAutohausFeinstaubVergabeDataService DataService { get { return CacheGet<IAutohausFeinstaubVergabeDataService>(); } }

        [XmlIgnore]
        public List<Kundenstammdaten> Kundenstamm { get { return DataService.Kundenstamm; } }

        [XmlIgnore]
        public List<Domaenenfestwert> Kraftstoffcodes { get { return DataService.Kraftstoffcodes; } }

        [XmlIgnore]
        public List<Domaenenfestwert> Plakettenarten { get { return DataService.Plakettenarten; } }

        public string Kennzeichen { get; private set; }

        public bool VergabeMoeglich { get; private set; }

        public string Meldung { get; private set; }

        public string Plakettenart { get; private set; }

        public void LoadStammdaten(ModelStateDictionary state)
        {
            DataService.MarkForRefreshKundenstamm();
            DataService.MarkForRefreshKraftstoffcodes();
            DataService.MarkForRefreshPlakettenarten();

            CheckStammdaten(state);
        }

        public void CheckPlakettenvergabe(FeinstaubCheckUI pruefKriterien, ModelStateDictionary state)
        {
            CheckStammdaten(state);

            if (state.IsValid)
            {
                Kennzeichen = pruefKriterien.KennzeichenTeil1 + "-" + pruefKriterien.KennzeichenTeil2;
                VergabeMoeglich = false;

                string tmpPlakettenArt;
                var erg = DataService.CheckFeinstaubVergabe(new FeinstaubCheck
                {
                    Fahrzeugklasse = pruefKriterien.Fahrzeugklasse,
                    CodeAufbau = pruefKriterien.CodeAufbau,
                    Kraftstoffcode = pruefKriterien.Kraftstoffcode,
                    Emissionsschluesselnummer = pruefKriterien.Emissionsschluesselnummer
                }, out tmpPlakettenArt);

                if (!String.IsNullOrEmpty(erg))
                {
                    state.AddModelError("", erg);
                }
                else
                {
                    Plakettenart = tmpPlakettenArt;

                    var bez = "";
                    if (!String.IsNullOrEmpty(Plakettenart))
                    {
                        bez = Plakettenarten.Find(p => p.Wert == Plakettenart).Beschreibung;
                    }

                    switch (Plakettenart)
                    {
                        case "1":
                        case "2":
                        case "3":
                            VergabeMoeglich = true;
                            Meldung = String.Format("Es kann eine Feinstaubplakette '{0}' ausgegeben werden für das Kennzeichen {1}.", bez, Kennzeichen);
                            break;
                        case "4":
                            Meldung = String.Format("Für das Kennzeichen {0} kann keine Feinstaubplakette ausgegeben werden.", Kennzeichen);
                            break;
                        default:
                            Meldung = "Es konnte keine Feinstaubplakettenart ermittelt werden. Bitte scannen Sie die ZB1 ein und senden Sie diese per Mail an support@kroschke.de, "
                                + "Betreff: Überprüfung FSP. Sie erhalten dann schnellstmöglich eine E-Mail über die auszugebende Plakettenart.";
                            break;
                    }
                }
            }
        }

        public void SavePlakettenvergabe(FeinstaubVergabeUI vergabeDaten, ModelStateDictionary state)
        {
            var erg = DataService.SaveFeinstaubVergabe(new FeinstaubVergabe{ Kennzeichen = vergabeDaten.Kennzeichen, Plakettenart = vergabeDaten.Plakettenart });

            if (!String.IsNullOrEmpty(erg))
            {
                state.AddModelError("", erg);
            }
        }

        public FeinstaubVergabeUI GetVergabeModel()
        {
            return new FeinstaubVergabeUI
                {
                    Kennzeichen = Kennzeichen,
                    VergabeMoeglich = VergabeMoeglich,
                    Plakettenart = Plakettenart,
                    Meldung = Meldung
                };
        }

        private void CheckStammdaten(ModelStateDictionary state)
        {
            if (Kundenstamm == null || Kundenstamm.Count == 0)
            {
                state.AddModelError("", "Fehler: Kundenstammdaten nicht gepflegt! Bitte wenden Sie sich an unsere Service-Rufnummer 04102/804-170.");
            }
            else if (Kundenstamm.Count > 1)
            {
                state.AddModelError("", "Fehler: Kundenstammdaten nicht eindeutig! Bitte wenden Sie sich an unsere Service-Rufnummer 04102/804-170.");
            }
        }
    }
}
