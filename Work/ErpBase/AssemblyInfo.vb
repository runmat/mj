Imports System.Reflection
Imports System.Runtime.InteropServices

' Allgemeine Informationen über eine Assembly werden über die folgende 
' Attributgruppe gesteuert. Ändern Sie diese Attributwerte, um Informationen,
' die mit einer Assembly verknüpft sind, zu bearbeiten.

' Die Werte der Assemblyattribute überprüfen

<Assembly: AssemblyTitle("Base")> 
<Assembly: AssemblyDescription("Base Library")> 
<Assembly: AssemblyCompany("Christoph Kroschke Gruppe")> 
<Assembly: AssemblyProduct("CKG Web Portal")> 
<Assembly: AssemblyCopyright("(c) 2004-2013 Christoph Kroschke Gruppe")> 
<Assembly: AssemblyTrademark("Kroschke")> 
<Assembly: CLSCompliant(True)> 

'Die folgende GUID ist für die ID der Typbibliothek, wenn dieses Projekt in COM angezeigt wird
<Assembly: Guid("BA3D0979-0748-4CF0-A4BD-C949C4EF8A0D")> 

' Versionsinformationen für eine Assembly bestehen aus den folgenden vier Werten:
'
'      Haupversion
'      Nebenversion 
'      Buildnummer
'      Revisionsnummer
'
' Sie können alle Werte angeben oder auf die standardmäßigen Build- und Revisionsnummern 
' zurückgreifen, indem Sie '*' wie unten angezeigt verwenden:

<Assembly: AssemblyVersion("2013.7.23.0")> 

' ************************************************
' $History: AssemblyInfo.vb $
' 
' *****************  Version 20  *****************
' User: Uha          Date: 9.08.07    Time: 11:09
' Updated in $/CKG/Base/Base
' Spalte "IstZeit" in Translation übernommen
' 
' *****************  Version 19  *****************
' User: Uha          Date: 8.08.07    Time: 17:22
' Updated in $/CKG/Base/Base
' Bugfixing CSV-Ausgabe
' 
' *****************  Version 18  *****************
' User: Uha          Date: 8.08.07    Time: 15:39
' Updated in $/CKG/Base/Base
' CSV-Schreiben in ExcelDocumentFactory integriert
' 
' *****************  Version 17  *****************
' User: Uha          Date: 12.07.07   Time: 13:39
' Updated in $/CKG/Base/Base
' In Excel werden jetzt auch mehrere Worksheets unterstützt
' (CreateDocument auch mit DataSet als Übergabeparameter)
' 
' *****************  Version 16  *****************
' User: Uha          Date: 9.07.07    Time: 11:45
' Updated in $/CKG/Base/Base
' Methode "CreateDocumentAndWriteToFilesystem" für Excel hinzugefügt
' 
' *****************  Version 15  *****************
' User: Uha          Date: 2.07.07    Time: 15:39
' Updated in $/CKG/Base/Base
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 14  *****************
' User: Uha          Date: 21.06.07   Time: 15:43
' Updated in $/CKG/Base/Base
' Bug in GetUser gefixt
' 
' *****************  Version 13  *****************
' User: Uha          Date: 20.06.07   Time: 18:56
' Updated in $/CKG/Base/Base
' Excelausgabe in Aspose formatiert ganze Zahlen jetzt als ganze Zahlen
' 
' *****************  Version 12  *****************
' User: Uha          Date: 20.06.07   Time: 16:18
' Updated in $/CKG/Base/Base
' Parameter ClearDurationASPX eingebracht
' 
' *****************  Version 11  *****************
' User: Uha          Date: 20.06.07   Time: 14:32
' Updated in $/CKG/Base/Base
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 8.06.07    Time: 14:26
' Updated in $/CKG/Base/Base
' WriteAuthorization um Parameter erweitert
' 
' *****************  Version 9  *****************
' User: Uha          Date: 4.06.07    Time: 16:18
' Updated in $/CKG/Base/Base
' 
' *****************  Version 8  *****************
' User: Uha          Date: 4.06.07    Time: 16:17
' Updated in $/CKG/Base/Base
' 
' 
' *****************  Version 7  *****************
' User: Uha          Date: 23.05.07   Time: 15:02
' Updated in $/CKG/Base/Base
' Fehler in Methode GetAppIDFromQueryString beseitigt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 23.05.07   Time: 12:45
' Updated in $/CKG/Base/Base
' TESTSAPUsername und SAPUsername aus Tabelle Customer entfernt
' 
' *****************  Version 5  *****************
' User: Uha          Date: 22.05.07   Time: 9:31
' Updated in $/CKG/Base/Base
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 4  *****************
' User: Uha          Date: 15.05.07   Time: 15:31
' Updated in $/CKG/Base/Base
' Änderungen aus StartApplication vom 11.05.2007
' 
' *****************  Version 3  *****************
' User: Uha          Date: 15.05.07   Time: 11:04
' Updated in $/CKG/Base/Base
' Änderungen aus StartApplication vom 11.05.2007 (Aspose-Tool,
' DataTableHelper.vb, Customer.vb)
' 
' *****************  Version 2  *****************
' User: Uha          Date: 3.05.07    Time: 18:05
' Updated in $/CKG/Base/Base
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' *****************  Version 1  *****************
' User: Uha          Date: 1.03.07    Time: 16:29
' Created in $/CKG/Base/Base
' 
' ************************************************