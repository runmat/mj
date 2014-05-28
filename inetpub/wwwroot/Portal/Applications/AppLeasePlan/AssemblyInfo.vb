Imports System.Reflection
Imports System.Runtime.InteropServices

' Allgemeine Informationen über eine Assembly werden über die folgende 
' Attributgruppe gesteuert. Ändern Sie diese Attributwerte, um Informationen,
' die mit einer Assembly verknüpft sind, zu bearbeiten.

' Die Werte der Assemblyattribute überprüfen

<Assembly: AssemblyTitle("Anwendung LeasePlan")> 
<Assembly: AssemblyDescription("ASPX Webanwendung für LeasePlan")> 
<Assembly: AssemblyCompany("Christoph Kroschke Gruppe")> 
<Assembly: AssemblyProduct("CKG Web Portal")> 
<Assembly: AssemblyCopyright("(c) 2004-2007 Christoph Kroschke Gruppe")> 
<Assembly: AssemblyTrademark("Kroschke")> 
<Assembly: CLSCompliant(True)> 

'Die folgende GUID ist für die ID der Typbibliothek, wenn dieses Projekt in COM angezeigt wird
<Assembly: Guid("AB28F5D8-3707-41DC-AC43-9D6472D8E9E3")> 

' Versionsinformationen für eine Assembly bestehen aus den folgenden vier Werten:
'
'      Haupversion
'      Nebenversion 
'      Buildnummer
'      Revisionsnummer
'
' Sie können alle Werte angeben oder auf die standardmäßigen Build- und Revisionsnummern 
' zurückgreifen, indem Sie '*' wie unten angezeigt verwenden:

<Assembly: AssemblyVersion("2007.9.27.2")> 

' ************************************************
' $History: AssemblyInfo.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 14:52
' Created in $/CKAG/Applications/AppLeasePlan
' 
' *****************  Version 22  *****************
' User: Uha          Date: 27.09.07   Time: 18:18
' Updated in $/CKG/Applications/AppLeasePlan/AppLeasePlanWeb
' ITA 1262 Ergänzung
' 
' *****************  Version 21  *****************
' User: Uha          Date: 27.09.07   Time: 15:58
' Updated in $/CKG/Applications/AppLeasePlan/AppLeasePlanWeb
' ITAs 1260 und 1262
' 
' *****************  Version 20  *****************
' User: Uha          Date: 27.09.07   Time: 11:24
' Updated in $/CKG/Applications/AppLeasePlan/AppLeasePlanWeb
' Controlnamen für Feldübersetzungen geändert
' 
' *****************  Version 19  *****************
' User: Uha          Date: 26.09.07   Time: 18:10
' Updated in $/CKG/Applications/AppLeasePlan/AppLeasePlanWeb
' BAPI Z_M_LAND_PLZ_001 hinzugefügt und BAPI Z_M_BRIEFANFORDERUNG_FMS
' erweitert.
' 
' *****************  Version 18  *****************
' User: Uha          Date: 17.09.07   Time: 9:52
' Updated in $/CKG/Applications/AppLeasePlan/AppLeasePlanWeb
' In Leaseplan_1 wird der Fehler "Leerer Versandgrund" jetzt abgefangen
' (Kein Objektfehler auf Change80_3.aspx mehr)
' 
' *****************  Version 17  *****************
' User: Uha          Date: 13.09.07   Time: 9:25
' Updated in $/CKG/Applications/AppLeasePlan/AppLeasePlanWeb
' ITA 1263: Formatierung in Top-Tabelle von Report02_2.aspx geändert
' 
' *****************  Version 16  *****************
' User: Uha          Date: 12.09.07   Time: 19:18
' Updated in $/CKG/Applications/AppLeasePlan/AppLeasePlanWeb
' Lagerort wurde falsch gefüllt
' 
' *****************  Version 15  *****************
' User: Uha          Date: 12.09.07   Time: 17:39
' Updated in $/CKG/Applications/AppLeasePlan/AppLeasePlanWeb
' ITA 1263 - Testversion
' 
' *****************  Version 14  *****************
' User: Uha          Date: 5.09.07    Time: 10:44
' Updated in $/CKG/Applications/AppLeasePlan/AppLeasePlanWeb
' Fehlerhafter Bildverweis in Change81.aspx korrigiert
' 
' *****************  Version 13  *****************
' User: Uha          Date: 4.09.07    Time: 13:04
' Updated in $/CKG/Applications/AppLeasePlan/AppLeasePlanWeb
' IT 1241: Mehrere Fahrzeuge zur Auswahl möglich
' 
' *****************  Version 12  *****************
' User: Uha          Date: 2.07.07    Time: 18:10
' Updated in $/CKG/Applications/AppLeasePlan/AppLeasePlanWeb
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 11  *****************
' User: Uha          Date: 20.06.07   Time: 15:53
' Updated in $/CKG/Applications/AppLeasePlan/AppLeasePlanWeb
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 10  *****************
' User: Uha          Date: 23.05.07   Time: 9:23
' Updated in $/CKG/Applications/AppLeasePlan/AppLeasePlanWeb
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 9  *****************
' User: Uha          Date: 22.05.07   Time: 12:47
' Updated in $/CKG/Applications/AppLeasePlan/AppLeasePlanWeb
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 8  *****************
' User: Uha          Date: 15.05.07   Time: 16:56
' Updated in $/CKG/Applications/AppLeasePlan/AppLeasePlanWeb
' Änderungen aus StartApplication vom 11.05.2007
' 
' *****************  Version 7  *****************
' User: Uha          Date: 8.03.07    Time: 17:05
' Updated in $/CKG/Applications/AppLeasePlan/AppLeasePlanWeb
' 
' ************************************************
