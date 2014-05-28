Imports System.Reflection
Imports System.Runtime.InteropServices

' Allgemeine Informationen über eine Assembly werden über die folgende 
' Attributgruppe gesteuert. Ändern Sie diese Attributwerte, um Informationen,
' die mit einer Assembly verknüpft sind, zu bearbeiten.

' Die Werte der Assemblyattribute überprüfen

<Assembly: AssemblyTitle("Anwendung FFD")> 
<Assembly: AssemblyDescription("ASPX Webanwendung für FFD")> 
<Assembly: AssemblyCompany("Christoph Kroschke Gruppe")> 
<Assembly: AssemblyProduct("CKG Web Portal")> 
<Assembly: AssemblyCopyright("(c) 2004-2007 Christoph Kroschke Gruppe")> 
<Assembly: AssemblyTrademark("Kroschke")> 
<Assembly: CLSCompliant(True)> 

'Die folgende GUID ist für die ID der Typbibliothek, wenn dieses Projekt in COM angezeigt wird
<Assembly: Guid("1F9D1EF3-6032-4421-9C0F-C45BA147C8F2")> 

' Versionsinformationen für eine Assembly bestehen aus den folgenden vier Werten:
'
'      Haupversion
'      Nebenversion 
'      Buildnummer
'      Revisionsnummer
'
' Sie können alle Werte angeben oder auf die standardmäßigen Build- und Revisionsnummern 
' zurückgreifen, indem Sie '*' wie unten angezeigt verwenden:

<Assembly: AssemblyVersion("2007.11.23.0")> 

' ************************************************
' $History: AssemblyInfo.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd
' 
' *****************  Version 28  *****************
' User: Rudolpho     Date: 23.11.07   Time: 15:55
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' ITA: 1372 OR
' 
' *****************  Version 27  *****************
' User: Uha          Date: 29.08.07   Time: 10:05
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' ITA 1224: Neue Ergebnisspalte "Kunde_Unbekannt" hinzugefügt
' 
' *****************  Version 26  *****************
' User: Uha          Date: 22.08.07   Time: 14:14
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' ITA 1208: Excel-Ergebnisausgabe hinzugefügt
' 
' *****************  Version 25  *****************
' User: Uha          Date: 22.08.07   Time: 13:43
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' Link auf Fahrzeughistorie aus Datagrid1 in Change81_2 entfernt
' 
' *****************  Version 24  *****************
' User: Uha          Date: 22.08.07   Time: 13:23
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' ITA 1208: Bugfixing 1
' 
' *****************  Version 23  *****************
' User: Uha          Date: 22.08.07   Time: 12:30
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' ITA 1208 Testversion
' 
' *****************  Version 22  *****************
' User: Uha          Date: 21.08.07   Time: 17:37
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' ITA 1208: Kompilierfähige Vorversion mit Teilfunktionalität
' 
' *****************  Version 21  *****************
' User: Uha          Date: 20.08.07   Time: 16:17
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' 
' *****************  Version 20  *****************
' User: Uha          Date: 16.08.07   Time: 11:39
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' ITAs 1162, 1223 und 1161 werden jetzt über Report11.aspx abgewickelt.
' Report14 wieder komplett gelöscht.
' 
' *****************  Version 19  *****************
' User: Uha          Date: 15.08.07   Time: 17:30
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' ITA 1223: "Gesamtbestand Fahrzeugbriefe" (Report14) hinzugefügt
' 
' *****************  Version 18  *****************
' User: Uha          Date: 15.08.07   Time: 16:18
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' ITA 1224: "Hinterlegung ALM-Daten" (Change80) hinzugefügt
' 
' *****************  Version 17  *****************
' User: Rudolpho     Date: 14.08.07   Time: 10:01
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' Bugfix: Report31_2 - FillGrid" if objBank.AuftragsUebersicht Is
' Nothing" hinzugefügt
' 
' *****************  Version 16  *****************
' User: Uha          Date: 13.08.07   Time: 10:55
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' Schreibfehler in AssemblyVersion beseitigt
' 
' *****************  Version 15  *****************
' User: Uha          Date: 13.08.07   Time: 10:39
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' CSV-Ausgabe in MDR Report "Versendete Zulassungsdaten" inegriert
' 
' *****************  Version 14  *****************
' User: Uha          Date: 9.08.07    Time: 15:51
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' Report "Versendete Zulassungsdaten" - 1. Version ohne Excel Download
' 
' *****************  Version 13  *****************
' User: Uha          Date: 9.08.07    Time: 11:12
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' Report "Versendete Zulassungsdaten" vorbereitet
' 
' *****************  Version 12  *****************
' User: Uha          Date: 8.08.07    Time: 13:06
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' SAPProxy_MDR hinzugefügt
' 
' *****************  Version 11  *****************
' User: Uha          Date: 2.07.07    Time: 17:40
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 10  *****************
' User: Uha          Date: 2.07.07    Time: 12:52
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 8.06.07    Time: 15:36
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' Abgleich Beyond Compare
' 
' *****************  Version 8  *****************
' User: Uha          Date: 23.05.07   Time: 9:40
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 7  *****************
' User: Uha          Date: 22.05.07   Time: 13:20
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 6  *****************
' User: Uha          Date: 21.05.07   Time: 14:22
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' Änderungen im Vergleich zur Startapplikation zum Stand 21.05.2007
' 
' *****************  Version 5  *****************
' User: Uha          Date: 3.05.07    Time: 18:05
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' *****************  Version 4  *****************
' User: Uha          Date: 8.03.07    Time: 17:02
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb
' 
' ************************************************
