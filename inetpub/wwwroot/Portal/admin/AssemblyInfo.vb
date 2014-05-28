Imports System.Reflection
Imports System.Runtime.InteropServices

' Allgemeine Informationen über eine Assembly werden über die folgende 
' Attributgruppe gesteuert. Ändern Sie diese Attributwerte, um Informationen,
' die mit einer Assembly verknüpft sind, zu bearbeiten.

' Die Werte der Assemblyattribute überprüfen

<Assembly: AssemblyTitle("Admin")> 
<Assembly: AssemblyDescription("Web Portal Administration GUI")> 
<Assembly: AssemblyCompany("Christoph Kroschke Gruppe")> 
<Assembly: AssemblyProduct("CKG Web Portal")> 
<Assembly: AssemblyCopyright("(c) 2004-2007 Christoph Kroschke Gruppe")> 
<Assembly: AssemblyTrademark("Kroschke")> 
<Assembly: CLSCompliant(True)> 

'Die folgende GUID ist für die ID der Typbibliothek, wenn dieses Projekt in COM angezeigt wird
<Assembly: Guid("6B19C36E-A695-4609-BB6E-CA5ED4252BD9")> 

' Versionsinformationen für eine Assembly bestehen aus den folgenden vier Werten:
'
'      Haupversion
'      Nebenversion 
'      Buildnummer
'      Revisionsnummer
'
' Sie können alle Werte angeben oder auf die standardmäßigen Build- und Revisionsnummern 
' zurückgreifen, indem Sie '*' wie unten angezeigt verwenden:

<Assembly: AssemblyVersion("2008.1.21.0")> 

' ************************************************
' $History: AssemblyInfo.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 14:47
' Created in $/CKAG/admin
' 
' *****************  Version 22  *****************
' User: Uha          Date: 21.01.08   Time: 18:09
' Updated in $/CKG/Admin/AdminWeb
' ITA 1644: Ermöglicht Login nur mit IP und festgelegtem Benutzer
' 
' *****************  Version 21  *****************
' User: Uha          Date: 18.09.07   Time: 18:34
' Updated in $/CKG/Admin/AdminWeb
' FieldTranslation erweitert um die Unterstützung von Datagrid-Columns
' 
' *****************  Version 20  *****************
' User: Uha          Date: 12.09.07   Time: 15:35
' Updated in $/CKG/Admin/AdminWeb
' ITA 1263: Ergänzung Radiobuttons für Nicht-Standard Kunde/Sprache
' sperren
' 
' *****************  Version 19  *****************
' User: Uha          Date: 12.09.07   Time: 15:17
' Updated in $/CKG/Admin/AdminWeb
' ITA 1263: Pflege der Feldübersetzungen
' 
' *****************  Version 18  *****************
' User: Uha          Date: 30.08.07   Time: 15:17
' Updated in $/CKG/Admin/AdminWeb
' ITA 1280: Bugfix
' 
' *****************  Version 17  *****************
' User: Uha          Date: 30.08.07   Time: 12:36
' Updated in $/CKG/Admin/AdminWeb
' ITA 1280: Paßwortversand im Web auf Benutzerwunsch
' 
' *****************  Version 16  *****************
' User: Uha          Date: 27.08.07   Time: 17:13
' Updated in $/CKG/Admin/AdminWeb
' ITA 1269: Archivverwaltung/-zuordnung jetzt auch per Web
' 
' *****************  Version 15  *****************
' User: Uha          Date: 9.08.07    Time: 11:38
' Updated in $/CKG/Admin/AdminWeb
' Spalte "IstZeit" in Translation übernommen
' 
' *****************  Version 14  *****************
' User: Uha          Date: 17.07.07   Time: 19:01
' Updated in $/CKG/Admin/AdminWeb
' Excelausgabe in SAPMonitoring angepasst
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 13.07.07   Time: 10:24
' Updated in $/CKG/Admin/AdminWeb
' 2 Firmenlogos hinzugefügt
' 
' *****************  Version 12  *****************
' User: Uha          Date: 9.07.07    Time: 17:52
' Updated in $/CKG/Admin/AdminWeb
' Excelliste für SAPMonitoring geändert
' 
' *****************  Version 11  *****************
' User: Uha          Date: 4.07.07    Time: 15:21
' Updated in $/CKG/Admin/AdminWeb
' Bugfixing für Application.ReAssign (Save Zuordnungen, falls es sich um
' ein Child handelt)
' 
' *****************  Version 10  *****************
' User: Uha          Date: 4.07.07    Time: 10:51
' Updated in $/CKG/Admin/AdminWeb
' Verschönerung + Bugfixing für SAPMonitoring mit hierarchischem Grid
' 
' *****************  Version 9  *****************
' User: Uha          Date: 4.07.07    Time: 9:19
' Updated in $/CKG/Admin/AdminWeb
' Bugfixing zu Änderung einer Child-Applikation setzt Rechte bezüglich
' Kunden und Gruppen neu laut Parent-Applikation
' 
' *****************  Version 8  *****************
' User: Uha          Date: 3.07.07    Time: 19:40
' Updated in $/CKG/Admin/AdminWeb
' Änderung einer Child-Applikation setzt Rechte bezüglich Kunden und
' Gruppen neu laut Parent-Applikation
' 
' *****************  Version 7  *****************
' User: Uha          Date: 3.07.07    Time: 18:48
' Updated in $/CKG/Admin/AdminWeb
' Absolute Pfade für Plus/Minus-Grafik im hierarchischen Grid eingefügt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 3.07.07    Time: 16:00
' Updated in $/CKG/Admin/AdminWeb
' SAPMonitoring mit hierarchischem Grid
' 
' *****************  Version 5  *****************
' User: Uha          Date: 3.07.07    Time: 12:45
' Updated in $/CKG/Admin/AdminWeb
' Anzeige ASPX Logging
' 
' *****************  Version 4  *****************
' User: Uha          Date: 22.05.07   Time: 11:27
' Updated in $/CKG/Admin/AdminWeb
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 3  *****************
' User: Uha          Date: 15.05.07   Time: 15:31
' Updated in $/CKG/Admin/AdminWeb
' Änderungen aus StartApplication vom 11.05.2007
' 
' *****************  Version 2  *****************
' User: Uha          Date: 13.03.07   Time: 10:53
' Updated in $/CKG/Admin/AdminWeb
' History-Eintrag vorbereitet
' 
' *****************  Version 1  *****************
' User: Uha          Date: 1.03.07    Time: 16:45
' Created in $/CKG/Admin/AdminWeb
' 
' ************************************************