// Java-Klasse ZLDMobileJS enthält anwendungsspezifische Variablen und Funktionen
function ZLDMobileJS() {
    this.datenstruktur = null;
    this.selUsername = "";
    this.selKreis = "";
    this.selZulDat = "";
    this.alteVorgaengeOffen = false;
    this.letzteGesendeteId = "";
    this.anzGesendetOk = 0;
    this.anzGesendetFehler = 0;
    this.anzGesendetDuplikat = 0;
}

ZLDMobileJS.prototype.RedirectToLoginPage = function() {
    $(location).attr('href', "/ZLDMobile/Login/Login");
};

// Lädt die aktuelle Datenstruktur aus dem LocalStorage
ZLDMobileJS.prototype.LoadDatenstrukturFromLocalStorage = function () {
    this.datenstruktur = LoadFromLocalStorage("ZLDMobile_" + this.selUsername.toUpperCase());
};

// Sichert die aktuelle Datenstruktur im LocalStorage
ZLDMobileJS.prototype.SaveDatenstrukturInLocalStorage = function () {
    SaveInLocalStorage("ZLDMobile_" + this.selUsername.toUpperCase(), this.datenstruktur);
};

// Vorgang mit der angegebenen Id aus der lokalen Liste entfernen
ZLDMobileJS.prototype.RemoveLocalVorgang = function(loeschId) {
    for (var i = 0; i < this.datenstruktur.Vorgaenge.length; i++) {
        var vorg = this.datenstruktur.Vorgaenge[i];
        if (vorg.Id == loeschId) {
            // Zähler für Amt dekrementieren und ggf. Amt aus Auswahlliste entfernen
            for (var j = 0; j < this.datenstruktur.AemterMitVorgaengen.length; j++) {
                var amv = this.datenstruktur.AemterMitVorgaengen[j];
                if (amv.KurzBez == vorg.Amt && amv.ZulDatText == vorg.ZulDatText) {
                    amv.AnzVorgaenge--;
                    if (amv.AnzVorgaenge == 0) {
                        this.datenstruktur.AemterMitVorgaengen.splice(j--, 1);
                    }
                    break;
                }
            }
            // Vorgang aus lokaler Collection entfernen
            this.datenstruktur.Vorgaenge.splice(i--, 1);
            break;
        }
    }
    this.SaveDatenstrukturInLocalStorage();
};

// Entfernt die aktuelle Datenstruktur komplett aus dem LocalStorage
ZLDMobileJS.prototype.RemoveDatenstrukturFromLocalStorage = function (showMessageOnSuccess) {
    RemoveFromLocalStorage("ZLDMobile_" + this.selUsername.toUpperCase());
    this.alteVorgaengeOffen = false;
    //SetRefreshButtonEnabled(true);
    if (showMessageOnSuccess == true) {
        ShowMessage("Daten aus lokalem Speicher gel\u00f6scht", false);
    }
};

// Entfernt alle Ämter/Vorgänge aus dem LocalStorage
ZLDMobileJS.prototype.RemoveAemterUndVorgaengeFromLocalStorage = function (showMessageOnSuccess) {
    if (this.datenstruktur != null) {
        this.datenstruktur.AemterMitVorgaengen = [];
        this.datenstruktur.Vorgaenge = [];
        this.SaveDatenstrukturInLocalStorage();
    }
    this.alteVorgaengeOffen = false;
    //SetRefreshButtonEnabled(true);
    if (showMessageOnSuccess == true) {
        ShowMessage("Vorg\u00e4nge aus lokalem Speicher gel\u00f6scht", false);
    }
};

// Setzt einen AJAX-Post mit den Vorgangs-Ids an den Server ab, um die BEB-Stati zurückzuerhalten 
// und dann ggf. erledigte/fehlgeschlagene alte Vorgänge lokal zu entfernen
ZLDMobileJS.prototype.CheckBEBStatusAndCleanUpVorgaenge = function (vorgIds) {
    $.ajax({
        type: "POST",
        url: "/ZLDMobile/ErfassungMobil/GetBEBStatusVorgaenge",
        data: { vorgangIds: JSON.stringify(vorgIds) },
        dataType: "json",
        context: this,
        beforeSend: function () { ShowBusyIndicator(); },
        complete: function () { HideBusyIndicator(); },
        success: function (result) {
            var liste = JSON.parse(result);
            if (liste == "unauthenticated") {
                this.RedirectToLoginPage();
            }
            if (liste.length > 0) {
                for (var i = 0; i < liste.length; i++) {
                    var vorgStatus = liste[i];
                    if (vorgStatus.Status == "2" || vorgStatus.Status == "F"
                        // 18.09.2013, M. Jenzen / N. Grzanna: Status "7" und "L" sollen mit gelöscht werden  (Ticket #2013091810000318)
                        || vorgStatus.Status == "7" || vorgStatus.Status == "L") {
                        // Vorgang aus lokaler Liste entfernen
                        this.RemoveLocalVorgang(vorgStatus.Id);
                    }
                }
                // Wenn jetzt alle alten Vorgänge weg sind -> Refresh-Funktion freigeben
                if (this.AlteVorgaengeVorhanden() == false) {
                    this.alteVorgaengeOffen = false;
                    //SetRefreshButtonEnabled(true);
                }
                ChangeAnzeigemodus("Kreis", false);
            }
            if (this.alteVorgaengeOffen == true) {
                ShowMessage("Es konnten nicht alle alten Vorg\u00e4nge gel\u00f6scht werden, da z.T. noch nicht bearbeitet", false);
            } else {
                ShowMessage("Alte Vorg\u00e4nge wurden gel\u00f6scht", false);
            }
        },
        error: function (req, status, error) {
            ShowMessage("Ermittlung des Bearbeitungsstatus der Vorg\u00e4nge fehlgeschlagen. Bitte \u00fcberpr\u00fcfen Sie Ihre Internetverbindung. (" + error + ")", true);
        }
    });
};

// Bereinigt die alten Ämter/Vorgänge im lokalen Speicher (erledigte/fehlgeschlagene fliegen raus)
ZLDMobileJS.prototype.CleanUpOldAemterUndVorgaengeInLocalStorage = function () {
    if (this.datenstruktur != null) {
        var alteVorgaenge = [];

        // Ids der alten Vorgänge ermitteln
        for (var i = 0; i < this.datenstruktur.Vorgaenge.length; i++) {
            var vorg = this.datenstruktur.Vorgaenge[i];
            var dateParts = vorg.ZulDatText.split(".");
            var datum = new Date(dateParts[2], (dateParts[1] - 1), dateParts[0]);

            if (IstHeute(datum) == false) {
                alteVorgaenge[alteVorgaenge.length] = vorg.Id;
            }
        }

        if (alteVorgaenge.length > 0) {
            // BEB-Status der Vorgänge abfragen und Vorgänge bereinigen
            this.CheckBEBStatusAndCleanUpVorgaenge(alteVorgaenge);
        } else {
            ShowMessage("Keine alten Vorg\u00e4nge gefunden", false);
        }
    }
};

// Prüft, ob das Amt im aktuellen Array vorhanden ist
ZLDMobileJS.prototype.AemterMitVorgaengenArrayContainsAmt = function (kuerzel, zuldat) {
    if (this.datenstruktur != null && this.datenstruktur.AemterMitVorgaengen != null) {
        for (var i = 0; i < this.datenstruktur.AemterMitVorgaengen.length; i++) {
            var amv = this.datenstruktur.AemterMitVorgaengen[i];
            if (amv.KurzBez == kuerzel && amv.ZulDatText == zuldat) {
                return true;
            }
        }
    }
    return false;
};

// Prüft, ob der Vorgang mit der angegebenen Id im aktuellen Array vorhanden ist
ZLDMobileJS.prototype.VorgangArrayContainsId = function (id) {
    if (this.datenstruktur != null && this.datenstruktur.Vorgaenge != null) {
        for (var i = 0; i < this.datenstruktur.Vorgaenge.length; i++) {
            if (this.datenstruktur.Vorgaenge[i].Id == id) {
                return true;
            }
        }
    }
    return false;
};

// Sortiert die Ämter mit Vorgängen absteigend nach dem Zulassungsdatum und aufsteigend nach dem Amt (relevant, wenn alte Vorgänge vorhanden)
ZLDMobileJS.prototype.AemterMitVorgaengenSortieren = function () {
    if (this.datenstruktur != null && this.datenstruktur.AemterMitVorgaengen != null) {
        this.datenstruktur.AemterMitVorgaengen.sort(function (a, b) {
            var aAmt = a.KurzBez;
            var bAmt = b.KurzBez;
            var dateParts = a.ZulDatText.split(".");
            var aDatum = new Date(dateParts[2], (dateParts[1] - 1), dateParts[0]);
            dateParts = b.ZulDatText.split(".");
            var bDatum = new Date(dateParts[2], (dateParts[1] - 1), dateParts[0]);
            if ((aDatum.getFullYear() == bDatum.getFullYear()) && (aDatum.getMonth() == bDatum.getMonth()) && (aDatum.getDate() == bDatum.getDate())) {
                // Datum gleich
                return (aAmt == bAmt) ? 0 : (aAmt > bAmt) ? 1 : -1;
            }
            else {
                return (aDatum < bDatum) ? 1 : -1;
            }
        });
    }
};

// Sortiert die Vorgangsliste nach Vorgangsart und Kundenname
ZLDMobileJS.prototype.VorgaengeSortieren = function () {
    if (this.datenstruktur != null && this.datenstruktur.Vorgaenge != null) {
        this.datenstruktur.Vorgaenge.sort(function (a, b) {
            var aBeleg = a.BlTyp;
            var bBeleg = b.BlTyp;
            var aKunnr = parseInt(a.Kunnr, 10);
            var bKunnr = parseInt(b.Kunnr, 10);
            var aId = a.Id;
            var bId = b.Id;
            if (aBeleg == bBeleg) {
                if (aKunnr == bKunnr) {
                    return (aId > bId) ? 1 : -1;
                } else {
                    return (aKunnr > bKunnr) ? 1 : -1;
                }
            } else {
                return (aBeleg > bBeleg) ? 1 : -1;
            }
        });
    }
};

// Prüft, ob für das aktuell gewählte Amt noch ältere Vorgänge lokal vorhanden sind
ZLDMobileJS.prototype.AlteVorgaengeVorhanden = function () {
    if (this.datenstruktur != null && this.datenstruktur.Vorgaenge != null) {
        for (var i = 0; i < this.datenstruktur.Vorgaenge.length; i++) {
            var dateParts = this.datenstruktur.Vorgaenge[i].ZulDatText.split(".");
            var datum = new Date(dateParts[2], (dateParts[1] - 1), dateParts[0]);

            if (IstHeute(datum) == false) {
                return true;
            }
        }
    }
    return false;
};

// Synchronisiert die lokale Ämterliste mit der vom Server
ZLDMobileJS.prototype.SynchronisiereAemterMitVorgaengen = function (serverAemter) {
    if (this.datenstruktur.AemterMitVorgaengen == null || this.datenstruktur.AemterMitVorgaengen.length == 0) {
        // alle Ämter übernehmen
        this.datenstruktur.AemterMitVorgaengen = serverAemter;
    } else {
        // noch nicht vorhandene Ämter ergänzen
        for (var i = 0; i < serverAemter.length; i++) {
            var sAmt = serverAemter[i];
            if (this.AemterMitVorgaengenArrayContainsAmt(sAmt.KurzBez, sAmt.ZulDatText) == false) {
                this.datenstruktur.AemterMitVorgaengen[this.datenstruktur.AemterMitVorgaengen.length] = sAmt;
            }
        }
        // Ämter in Liste aktualisieren bzw. ggf. entfernen
        for (var j = 0; j < this.datenstruktur.AemterMitVorgaengen.length; j++) {
            var amv = this.datenstruktur.AemterMitVorgaengen[j];
            var gefunden = false;
            var dateParts = amv.ZulDatText.split(".");
            var datum = new Date(dateParts[2], (dateParts[1] - 1), dateParts[0]);

            if (IstHeute(datum) == true) {
                for (var k = 0; k < serverAemter.length; k++) {
                    var sAmv = serverAemter[k];
                    if (sAmv.KurzBez == amv.KurzBez) {
                        gefunden = true;
                        amv.Bezeichnung = sAmv.Bezeichnung;
                        amv.AnzVorgaenge = sAmv.AnzVorgaenge;
                        break;
                    }
                }
                if (gefunden == false) {
                    //alert("Sync: Amt " + amv.KurzBez + " entfernt");
                    this.datenstruktur.AemterMitVorgaengen.splice(j--, 1);
                }
            }
        }
    }
    this.SaveDatenstrukturInLocalStorage();
};

// Synchronisiert die lokale Vorgangsliste mit den neuen Vorgängen vom Server
ZLDMobileJS.prototype.SynchronisiereVorgaenge = function (serverVorgaenge) {
    if (this.datenstruktur.Vorgaenge == null || this.datenstruktur.Vorgaenge.length == 0) {
        // alle Vorgänge übernehmen
        this.datenstruktur.Vorgaenge = serverVorgaenge;
    } else {
        // noch nicht vorhandene Vorgänge ergänzen
        for (var i = 0; i < serverVorgaenge.length; i++) {
            var sVorg = serverVorgaenge[i];
            if (this.VorgangArrayContainsId(sVorg.Id) == false) {
                this.datenstruktur.Vorgaenge[this.datenstruktur.Vorgaenge.length] = sVorg;
            }
        }
        // ggf. bereits erledigte Vorgänge aus lokaler Liste entfernen
        if (this.datenstruktur.Vorgaenge.length > serverVorgaenge.length) {
            for (var j = 0; j < this.datenstruktur.Vorgaenge.length; j++) {
                var vorg = this.datenstruktur.Vorgaenge[j];
                var gefunden = false;
                var dateParts = vorg.ZulDatText.split(".");
                var datum = new Date(dateParts[2], (dateParts[1] - 1), dateParts[0]);

                if (IstHeute(datum) == true) {
                    for (var k = 0; k < serverVorgaenge.length; k++) {
                        if (serverVorgaenge[k].Id == vorg.Id) {
                            gefunden = true;
                            break;
                        }
                    }
                    if (gefunden == false) {
                        //alert("Sync: Vorgang " + vorg.Id + " entfernt");
                        this.datenstruktur.Vorgaenge.splice(j--, 1);
                    }
                }
            }
        }
    }
    this.SaveDatenstrukturInLocalStorage();
    if (serverVorgaenge.length == 0) {
        ShowMessage("Keine neuen Vorg\u00e4nge vorhanden!", false);
    } else {
        ShowMessage(serverVorgaenge.length.toString() + " Vorg\u00e4nge vom Server geladen/aktualisiert", false);
    }
};

// Setzt einen AJAX-Post an den Server ab, um die aktuellen Vorgänge zurückzubekommen
ZLDMobileJS.prototype.GetVorgaengeFromServer = function () {
    $.ajax({
        type: "POST",
        url: "/ZLDMobile/ErfassungMobil/LoadVorgaenge",
        data: {},
        dataType: "json",
        context: this,
        beforeSend: function () { ShowBusyIndicator(); },
        complete: function () { HideBusyIndicator(); },
        success: function (result) {
            var vorgaenge = JSON.parse(result);
            if (vorgaenge == "unauthenticated") {
                this.RedirectToLoginPage();
            }
            this.SynchronisiereVorgaenge(vorgaenge);
            FillVorgangGrid();
        },
        error: function (req, status, error) {
            ShowMessage("Abrufen der Vorg\u00e4nge vom Server fehlgeschlagen. Bitte \u00fcberpr\u00fcfen Sie Ihre Internetverbindung", true);
        }
    });
};

// Setzt einen AJAX-Post an den Server ab, um die aktuellen Ämter zurückzubekommen
ZLDMobileJS.prototype.GetAemterVorgaengeFromServer = function () {
    $.ajax({
        type: "POST",
        url: "/ZLDMobile/ErfassungMobil/LoadAemterVorgaenge",
        data: {},
        dataType: "json",
        context: this,
        beforeSend: function () { ShowBusyIndicator(); },
        complete: function () { HideBusyIndicator(); },
        success: function (result) {
            var aemter = JSON.parse(result);
            if (aemter == "unauthenticated") {
                this.RedirectToLoginPage();
            }
            this.SynchronisiereAemterMitVorgaengen(aemter);
            ChangeAnzeigemodus("Kreis", true);
        },
        error: function (req, status, error) {
            ShowMessage("Abrufen der Amt-/Vorgangsdaten vom Server fehlgeschlagen. Bitte \u00fcberpr\u00fcfen Sie Ihre Internetverbindung", true);
        }
    });
};

// Lädt die aktuellen Ämter vom Server nach
ZLDMobileJS.prototype.LadeAemterMitVorgaengen = function () {
    this.GetAemterVorgaengeFromServer();
};

// Setzt einen AJAX-Post an den Server ab, um die Datenstruktur zurückzubekommen
ZLDMobileJS.prototype.GetDatenstrukturFromServer = function () {
    $.ajax({
        type: "POST",
        url: "/ZLDMobile/ErfassungMobil/LoadDatenstruktur",
        data: {},
        dataType: "json",
        context: this,
        beforeSend: function () { ShowBusyIndicator(); },
        complete: function () { HideBusyIndicator(); },
        success: function (result) {
            var dStruktur = JSON.parse(result);
            if (dStruktur == "unauthenticated") {
                this.RedirectToLoginPage();
            }
            this.datenstruktur = dStruktur;
            if (this.datenstruktur != null) {
                this.LadeAemterMitVorgaengen();
            }
        },
        error: function (req, status, error) {
            ShowMessage("Abrufen der Anwendungsdaten vom Server fehlgeschlagen. Bitte \u00fcberpr\u00fcfen Sie Ihre Internetverbindung", true);
        }
    });
};

// Setzt einen AJAX-Post an den Server ab, um die aktuellen Stammdaten zu erhalten
ZLDMobileJS.prototype.GetStammdatenFromServer = function (showMessageOnSuccess, vorgaengeLaden) {
    $.ajax({
        type: "POST",
        url: "/ZLDMobile/ErfassungMobil/LoadStammdaten",
        data: {},
        dataType: "json",
        context: this,
        beforeSend: function () { ShowBusyIndicator(); },
        complete: function () { HideBusyIndicator(); },
        success: function (result) {
            var stda = JSON.parse(result);
            if (stda == "unauthenticated") {
                this.RedirectToLoginPage();
            }
            if (this.datenstruktur != null) {
                this.datenstruktur.Stammdaten = stda;
                this.SaveDatenstrukturInLocalStorage();
                if (showMessageOnSuccess == true) {
                    ShowMessage("Stammdaten wurden aktualisiert", false);
                }
                if (vorgaengeLaden == true) {
                    this.InitVorgaenge();
                }
            }
        },
        error: function (req, status, error) {
            ShowMessage("Abrufen der Stammdaten vom Server fehlgeschlagen. Bitte \u00fcberpr\u00fcfen Sie Ihre Internetverbindung", true);
        }
    });
};

// Lädt die aktuellen Stammdaten vom Server nach
ZLDMobileJS.prototype.RefreshStammdaten = function (showMessageOnSuccess) {
    if (this.datenstruktur != null) {
        this.GetStammdatenFromServer(showMessageOnSuccess, false);
    } else {
        this.InitDatenstruktur();
    }
};

// Prüft, ob noch bearbeitete Vorgänge vergangener Tage im lokalen Speicher vorhanden sind, und lädt diese ggf., 
// andernfalls werden neue Vorgänge vom Server nachgeladen (lokal bearbeitete Vorgänge bleiben erhalten)
ZLDMobileJS.prototype.InitVorgaenge = function () {
    if (this.AlteVorgaengeVorhanden() == true) {
        this.alteVorgaengeOffen = true;
        //SetRefreshButtonEnabled(false);
        ShowMessage(("Achtung! Es sind noch nicht abgesendete alte Vorg\u00e4nge vorhanden. Diese bitte erst absenden!"), false);
        if (this.datenstruktur.AemterMitVorgaengen.length == 1) {
            var amt = this.datenstruktur.AemterMitVorgaengen[0];
            SelectKreis(amt.KurzBez, amt.AmtBez, amt.ZulDatText);
        } else {
            ChangeAnzeigemodus("Kreis", false);
        }
    } else if (this.datenstruktur != null) {
        this.LadeAemterMitVorgaengen();
    }
};

// Initialisierung bzw. Laden der Datenstruktur für den aktiven Benutzer, anschließend Laden der Vorgänge
ZLDMobileJS.prototype.InitDatenstruktur = function () {
    this.LoadDatenstrukturFromLocalStorage();
    if (this.datenstruktur != null) {
        this.GetStammdatenFromServer(false, true);
    } else {
        this.GetDatenstrukturFromServer();
    }
};

// Sendet jeweils einen bearbeiteten Vorgang an den Server und entfernt ihn anschließend aus der lokalen Vorgangsliste.
// Wenn gar kein relevanter vorhanden oder bereits alle gesendet -> Rückmeldung an User
ZLDMobileJS.prototype.SendeNaechstenVorgang = function (sendebeginn) {
    var sendeVorgang = null;

    if (sendebeginn == true) {
        this.letzteGesendeteId = "";
        this.anzGesendetOk = 0;
        this.anzGesendetFehler = 0;
        this.anzGesendetDuplikat = 0;
    }

    for (var i = 0; i < this.datenstruktur.Vorgaenge.length; i++) {
        var vorg = this.datenstruktur.Vorgaenge[i];
        if (vorg.IsShown == true && vorg.IsDirty == true && vorg.ID != this.letzteGesendeteId
            && (vorg.StatusDurchgefuehrt == true || vorg.StatusFehlgeschlagen == true || vorg.ZulDatText != vorg.ZulDatTextEdit || vorg.Amt != vorg.AmtEdit)) {
            sendeVorgang = vorg;
        }
    }
    if (sendeVorgang != null) {
        if (sendebeginn == true) {
            ShowBusyIndicator();
        }
        this.SendVorgangToServer(sendeVorgang);
    } else {
        FillVorgangGrid();
        HideBusyIndicator();
        if (sendebeginn == true) {
            ShowMessage("Es wurden keine Vorg\u00e4nge bearbeitet", false);
        } else {
            if (this.anzGesendetFehler == 0 && this.anzGesendetDuplikat == 0) {
                ShowMessage(this.anzGesendetOk + " Vorg\u00e4nge erfolgreich an Server gesendet", false);
            } else if (this.anzGesendetFehler != 0) {
                if (this.anzGesendetDuplikat != 0) {
                    ShowMessage("Vorg\u00e4nge an Server gesendet: " + this.anzGesendetOk + " OK, " + this.anzGesendetFehler + " fehlerhaft, " + this.anzGesendetDuplikat + " bereits erledigt", false);
                } else {
                    ShowMessage("Vorg\u00e4nge an Server gesendet: " + this.anzGesendetOk + " OK, " + this.anzGesendetFehler + " fehlerhaft", false);
                }
            } else {
                ShowMessage("Vorg\u00e4nge an Server gesendet: " + this.anzGesendetOk + " OK, " + this.anzGesendetDuplikat + " bereits erledigt", false);
            }
        }
    }
};

// Setzt einen AJAX-Post mit den Vorgangsdaten an den Server ab, damit diese dort gespeichert werden
ZLDMobileJS.prototype.SendVorgangToServer = function (vg) {
    $.ajax({
        type: "POST",
        url: "/ZLDMobile/ErfassungMobil/SaveVorgang",
        data: { vorgang: JSON.stringify(vg) },
        dataType: "json",
        context: this,
        success: function (result) {
            var saveErg = JSON.parse(result);
            if (saveErg == "unauthenticated") {
                HideBusyIndicator();
                this.RedirectToLoginPage();
            } else {
                this.letzteGesendeteId = saveErg.Id;
                if (saveErg.Ergebniscode == "OK") {
                    if (saveErg.Meldungstext == "NOTSAVED") {
                        this.anzGesendetDuplikat += 1;
                    } else {
                        this.anzGesendetOk += 1;
                    }
                    // Übertragenen Vorgang aus lokaler Liste entfernen
                    this.RemoveLocalVorgang(saveErg.Id);
                    //FillVorgangGrid();
                    // Wenn jetzt alle alten Vorgänge weg sind -> Refresh-Funktion freigeben
                    if (this.AlteVorgaengeVorhanden() == false) {
                        this.alteVorgaengeOffen = false;
                        //SetRefreshButtonEnabled(true);
                    }
                    // Nächsten Vorgang senden, falls vorhanden
                    this.SendeNaechstenVorgang(false);
                } else if (saveErg.Ergebniscode == "APPERROR") {
                    FillVorgangGrid();
                    HideBusyIndicator();
                    if (this.anzGesendetDuplikat != 0) {
                        ShowMessage("Serverfehler beim Speichern(vorher an Server gesendet: " + this.anzGesendetOk + " OK, " + this.anzGesendetDuplikat + " bereits erledigt): " + saveErg.Meldungstext);
                    } else if (this.anzGesendetOk != 0) {
                        ShowMessage("Serverfehler beim Speichern(vorher an Server gesendet: " + this.anzGesendetOk + " OK): " + saveErg.Meldungstext);
                    } else {
                        ShowMessage("Serverfehler beim Speichern: " + saveErg.Meldungstext);
                    }
                } else {
                    this.anzGesendetFehler += 1;
                    // Nächsten Vorgang senden, falls vorhanden
                    this.SendeNaechstenVorgang(false);
                }
            }
            
        },
        error: function (req, status, error) {
            HideBusyIndicator();
            ShowMessage("Es konnten nicht alle Daten zum Server \u00fcbertragen werden. Bitte \u00fcberpr\u00fcfen Sie Ihre Internetverbindung. (" + error + ")", true);
        }
    });
};

// Per AJAX-Post prüfen, ob der Server erreichbar ist bzw. antwortet, und ggf. Seitenfunktionalität einschränken/erweitern
ZLDMobileJS.prototype.UpdateConnectivityStatus = function () {
    $.ajax({
        type: "POST",
        url: "/ZLDMobile/ErfassungMobil/ReturnHeartbeat",
        data: {},
        dataType: "json",
        context: this,
        beforeSend: function () { ShowBusyIndicator(); },
        complete: function () { HideBusyIndicator(); },
        success: function (result) {
            ApplyConnectivityStatus(true);
        },
        error: function (req, status, error) {
            ApplyConnectivityStatus(false);
        }
    });
};

// Liste der zur Auswahl stehenden VkBurs vom Server laden
ZLDMobileJS.prototype.LoadVkBurListe = function () {
    $.ajax({
        type: "POST",
        url: "/ZLDMobile/ErfassungMobil/LoadVkBurListe",
        data: {},
        dataType: "json",
        context: this,
        beforeSend: function () { ShowBusyIndicator(); },
        complete: function () { HideBusyIndicator(); },
        success: function (result) {
            var vkBurs = JSON.parse(result);
            if (vkBurs == "unauthenticated") {
                this.RedirectToLoginPage();
            }
            FillVkBurAuswahl(vkBurs);
        },
        error: function (req, status, error) {
            ShowMessage("Laden der Kostenstelle(n) fehlgeschlagen. Bitte \u00fcberpr\u00fcfen Sie Ihre Internetverbindung. (" + error + ")", true);
        }
    });
};

// VkBur-Auswahl an Server senden, um serverseitig Stammdaten zu laden und anschließend zur Bearbeitungsansicht zu wechseln
ZLDMobileJS.prototype.SelectVkBur = function (vkBur) {
    $.ajax({
        type: "POST",
        url: "/ZLDMobile/ErfassungMobil/ApplyVkBur",
        data: { vkBur: vkBur },
        context: this,
        beforeSend: function () { ShowBusyIndicator(); },
        complete: function () { HideBusyIndicator(); },
        success: function (result) {
            if (result == "unauthenticated") {
                this.RedirectToLoginPage();
            }
            ChangeAnzeigemodus("Create", result);
        },
        error: function (req, status, error) {
            ShowMessage("Laden der Stammdaten fehlgeschlagen. Bitte \u00fcberpr\u00fcfen Sie Ihre Internetverbindung. (" + error + ")", true);
        }
    });
};

// Setzt einen AJAX-Post mit den Vorgangsdaten an den Server ab, damit diese dort gespeichert werden
ZLDMobileJS.prototype.SaveNewVorgang = function (vg) {
    $.ajax({
        type: "POST",
        url: "/ZLDMobile/ErfassungMobil/SaveNewVorgang",
        data: { vorgang: JSON.stringify(vg) },
        dataType: "json",
        context: this,
        beforeSend: function () { ShowBusyIndicator(); },
        complete: function () { HideBusyIndicator(); },
        success: function (result) {
            var saveErg = JSON.parse(result);
            if (saveErg == "unauthenticated") {
                this.RedirectToLoginPage();
            } else {
                if (saveErg.Ergebniscode == "OK") {
                    ShowMessage("Vorgang erfolgreich gespeichert", false);
                    ClearVorgangCreate();
                } else {
                    ShowMessage("Fehler beim Speichern: " + saveErg.Meldungstext, true);
                }
            }
        },
        error: function (req, status, error) {
            HideBusyIndicator();
            ShowMessage("Es konnten nicht alle Daten zum Server \u00fcbertragen werden. Bitte \u00fcberpr\u00fcfen Sie Ihre Internetverbindung. (" + error + ")", true);
        }
    });
};
