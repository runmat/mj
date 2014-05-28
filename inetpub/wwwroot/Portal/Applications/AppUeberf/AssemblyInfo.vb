Imports System.Reflection
Imports System.Runtime.InteropServices

' Allgemeine Informationen über eine Assembly werden über die folgende 
' Attributgruppe gesteuert. Ändern Sie diese Attributwerte, um Informationen,
' die mit einer Assembly verknüpft sind, zu bearbeiten.

' Die Werte der Assemblyattribute überprüfen

<Assembly: AssemblyTitle("Anwendung Überführung")> 
<Assembly: AssemblyDescription("ASPX Webanwendung für Überführungen")> 
<Assembly: AssemblyCompany("Christoph Kroschke Gruppe")> 
<Assembly: AssemblyProduct("CKG Web Portal")> 
<Assembly: AssemblyCopyright("(c) 2004-2007 Christoph Kroschke Gruppe")> 
<Assembly: AssemblyTrademark("Kroschke")> 
<Assembly: CLSCompliant(True)> 

'Die folgende GUID ist für die ID der Typbibliothek, wenn dieses Projekt in COM angezeigt wird
<Assembly: Guid("4023C758-4853-400E-8992-55E74FD0D219")> 

' Versionsinformationen für eine Assembly bestehen aus den folgenden vier Werten:
'
'      Haupversion
'      Nebenversion 
'      Buildnummer
'      Revisionsnummer
'
' Sie können alle Werte angeben oder auf die standardmäßigen Build- und Revisionsnummern 
' zurückgreifen, indem Sie '*' wie unten angezeigt verwenden:

<Assembly: AssemblyVersion("2007.9.6.0")> 

' ************************************************
' $History: AssemblyInfo.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:06
' Created in $/CKAG/Applications/AppUeberf
' 
' *****************  Version 21  *****************
' User: Rudolpho     Date: 6.09.07    Time: 12:45
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb
' ITA: 1247
' 
' *****************  Version 20  *****************
' User: Rudolpho     Date: 30.08.07   Time: 9:57
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb
' ITA: 1247 
' 
' *****************  Version 19  *****************
' User: Rudolpho     Date: 12.07.07   Time: 16:03
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb
' In Zulg_01 leere Zulassungskreisabfrage abgefangen
' 
' *****************  Version 18  *****************
' User: Uha          Date: 4.07.07    Time: 16:51
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb
' Bugfixing: In Ueberfg_04 Verweis auf leeren dv = Session("DataViewRG")
' abgefangen.
' 
' *****************  Version 17  *****************
' User: Uha          Date: 3.07.07    Time: 19:42
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb
' FormAuth in diverse Seiten wieder eingefügt
' 
' *****************  Version 16  *****************
' User: Uha          Date: 2.07.07    Time: 16:18
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 15  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 14  *****************
' User: Uha          Date: 26.06.07   Time: 11:23
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb
' Fehlerbehandlung wieder aktiviert (AppUeberf, _Report041.aspx.vb)
' 
' *****************  Version 13  *****************
' User: Uha          Date: 26.06.07   Time: 8:35
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb
' Bildpfad fuer Pfeil korrigiert. (AppUeberf, _Report041.aspx)
' 
' *****************  Version 12  *****************
' User: Uha          Date: 26.06.07   Time: 8:19
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb
' Fahrzeugueberfuehrungen: Es fehlte das Schreiben der Kunnr in die
' Session (_Report04.aspx.vb).
' 
' *****************  Version 11  *****************
' User: Uha          Date: 29.05.07   Time: 10:14
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb
' LeasingnehmerKundennummer wird bei Ueberf_01 im Fall einer
' Freitexteingabe nicht mehr  leer übergeben.
' 
' *****************  Version 10  *****************
' User: Uha          Date: 22.05.07   Time: 10:41
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 9  *****************
' User: Uha          Date: 21.05.07   Time: 13:31
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb
' Versionsnummer hochgesetzt
' 
' *****************  Version 8  *****************
' User: Uha          Date: 3.05.07    Time: 15:20
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb
' Versionsnummer hochgesetzt
' 
' *****************  Version 7  *****************
' User: Uha          Date: 5.04.07    Time: 17:04
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb
' Verlinkung korrigiert.
' 
' *****************  Version 6  *****************
' User: Uha          Date: 5.04.07    Time: 11:15
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb
' Verlinkung der Formulare untereinander korrigiert.
' 
' *****************  Version 5  *****************
' User: Uha          Date: 8.03.07    Time: 17:10
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb
' 
' ************************************************
