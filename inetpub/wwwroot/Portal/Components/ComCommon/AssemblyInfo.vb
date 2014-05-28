Imports System.Reflection
Imports System.Runtime.InteropServices

' Allgemeine Informationen über eine Assembly werden über die folgende 
' Attributgruppe gesteuert. Ändern Sie diese Attributwerte, um Informationen,
' die mit einer Assembly verknüpft sind, zu bearbeiten.

' Die Werte der Assemblyattribute überprüfen

<Assembly: AssemblyTitle("ComCommon")> 
<Assembly: AssemblyDescription("Components for common use")> 
<Assembly: AssemblyCompany("Christoph Kroschke Gruppe")> 
<Assembly: AssemblyProduct("CKG Web Portal")> 
<Assembly: AssemblyCopyright("(c) 2004-2007 Christoph Kroschke Gruppe")> 
<Assembly: AssemblyTrademark("Kroschke")> 
<Assembly: CLSCompliant(True)> 

'Die folgende GUID ist für die ID der Typbibliothek, wenn dieses Projekt in COM angezeigt wird
<Assembly: Guid("C3CE29A0-4513-4D00-A3A5-AC11CFC41AD3")> 

' Versionsinformationen für eine Assembly bestehen aus den folgenden vier Werten:
'
'      Haupversion
'      Nebenversion 
'      Buildnummer
'      Revisionsnummer
'
' Sie können alle Werte angeben oder auf die standardmäßigen Build- und Revisionsnummern 
' zurückgreifen, indem Sie '*' wie unten angezeigt verwenden:

<Assembly: AssemblyVersion("2008.1.8.0")> 

' ************************************************
' $History: AssemblyInfo.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:47
' Created in $/CKAG/portal/Components/ComCommon
' 
' *****************  Version 42  *****************
' User: Uha          Date: 8.01.08    Time: 9:39
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Autorisierunganwendung (funktionslos) hinzugefügt
' 
' *****************  Version 41  *****************
' User: Uha          Date: 20.12.07   Time: 11:16
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1505 Fahrzeughistorie in Testversion
' 
' *****************  Version 40  *****************
' User: Uha          Date: 19.12.07   Time: 17:39
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1511 Testversion
' 
' *****************  Version 39  *****************
' User: Uha          Date: 18.12.07   Time: 17:57
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Anforderung (temp./endg.) fast fertig
' 
' *****************  Version 38  *****************
' User: Uha          Date: 17.12.07   Time: 17:58
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Versand ZB II / Briefe - komplierfähige Zwischenversion
' 
' *****************  Version 37  *****************
' User: Uha          Date: 13.12.07   Time: 17:18
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Elemente für Temp./Endg. bzw. HEZ Anforderung hinzugefügt (Change42ff,
' fin_06, Change43ff und fin_08)
' 
' *****************  Version 36  *****************
' User: Uha          Date: 13.12.07   Time: 16:29
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1470/1502 (Unbekannter Händler) - Spaltenübersetzungen hinzugefügt
' 
' *****************  Version 35  *****************
' User: Uha          Date: 13.12.07   Time: 15:47
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Kontingentart "HEZ (in standard temporär enthalten)" in lokale
' BankBaseCredit wieder eingefügt
' 
' *****************  Version 34  *****************
' User: Jungj        Date: 13.12.07   Time: 14:02
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1466 Hinzugefügt
' 
' *****************  Version 33  *****************
' User: Uha          Date: 13.12.07   Time: 13:58
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1481/1509 (Änderung / Sperrung Händlerkontingent) Testversion
' 
' *****************  Version 32  *****************
' User: Uha          Date: 13.12.07   Time: 10:31
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' In BankBaseCredit überflüssige Methoden und Kontingentarten entfernt
' 
' *****************  Version 31  *****************
' User: Uha          Date: 13.12.07   Time: 9:49
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' fin_06.vb durch BankBaseCredit.vb ersetzt
' 
' *****************  Version 30  *****************
' User: Uha          Date: 12.12.07   Time: 15:56
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1473/1497 (Mahnstufe 3) als Testversion; ITA 1481/1509
' (Änderung/Sperrung Händlerkontingent) komplierfähig
' 
' *****************  Version 29  *****************
' User: Uha          Date: 12.12.07   Time: 13:23
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Übernahme der Suchfunktion aus FFD (ohne Anpassung)
' 
' *****************  Version 28  *****************
' User: Uha          Date: 12.12.07   Time: 10:20
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Kosmetik im Bereich Finance
' 
' *****************  Version 27  *****************
' User: Uha          Date: 12.12.07   Time: 10:08
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1469/1501 (Dokumente ohne Daten) in Testversion hinzugefügt
' 
' *****************  Version 26  *****************
' User: Uha          Date: 11.12.07   Time: 15:47
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1470 bzw. 1473/1497 ASPX-Seite und Lib hinzugefügt
' 
' *****************  Version 25  *****************
' User: Uha          Date: 11.12.07   Time: 13:56
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1468/1500 ASPX-Seite und Lib hinzugefügt
' 
' *****************  Version 24  *****************
' User: Uha          Date: 27.09.07   Time: 16:20
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1238 Bugfixing
' 
' *****************  Version 23  *****************
' User: Uha          Date: 27.09.07   Time: 10:27
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Controlnamen für Feldübersetzungen geändert
' 
' *****************  Version 22  *****************
' User: Uha          Date: 26.09.07   Time: 16:49
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' In Change01, Change03 und Change80 neues Format "GridTableHighlight"
' verwendet.
' 
' *****************  Version 21  *****************
' User: Uha          Date: 26.09.07   Time: 13:22
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Bugfixing in ITA 1237, 1181 und 1238 (Alle Floorcheck)
' 
' *****************  Version 20  *****************
' User: Uha          Date: 25.09.07   Time: 17:03
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1124 hinzugefügt (Change03/Change03_2) und allgemeines Bugfix
' Floorcheck
' 
' *****************  Version 19  *****************
' User: Uha          Date: 24.09.07   Time: 18:07
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1124: Upload Prüflisten via WEB - Nicht lauffähige Vorversion
' 
' *****************  Version 18  *****************
' User: Uha          Date: 24.09.07   Time: 17:01
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1238: Anlage Floorcheckauftrag - Testversion
' 
' *****************  Version 17  *****************
' User: Uha          Date: 20.09.07   Time: 17:19
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1181 Testversion
' 
' *****************  Version 16  *****************
' User: Uha          Date: 20.09.07   Time: 16:34
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1181: Testversion
' 
' *****************  Version 15  *****************
' User: Uha          Date: 19.09.07   Time: 17:29
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1181: Funktionslose Rohversion
' 
' *****************  Version 14  *****************
' User: Uha          Date: 18.09.07   Time: 18:15
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1261: Erste Testversion (Excelspalten müssen noch übersetzt werden;
' Rückschreiben wirft SAP-Fehler)
' 
' *****************  Version 13  *****************
' User: Uha          Date: 17.09.07   Time: 18:14
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1261: Under Construction - Keine Funktion
' 
' *****************  Version 12  *****************
' User: Uha          Date: 17.09.07   Time: 14:40
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1237: Bugfixing; Bemerkung: BAPI zum Schreiben der Bemerkung steht
' noch aus
' 
' *****************  Version 11  *****************
' User: Uha          Date: 13.09.07   Time: 18:46
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1237 - Report "Geplante Floorchecks": Bugfix 1
' 
' *****************  Version 10  *****************
' User: Uha          Date: 5.09.07    Time: 17:21
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1237 - Report "Geplante Floorchecks": Testversion fertig (keine
' Testdaten im CKQ)
' 
' *****************  Version 9  *****************
' User: Uha          Date: 9.08.07    Time: 16:39
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Bugfixing im Excelreport von "Vergabe von Feinstaubplaketten"
' 
' *****************  Version 8  *****************
' User: Uha          Date: 9.08.07    Time: 13:18
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Oberflächenänderung in "Vergabe von Feinstaubplaketten"
' 
' *****************  Version 7  *****************
' User: Uha          Date: 12.07.07   Time: 13:40
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Report "Vergabe von Feinstaubplaketten" als Testversion erzeugt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 12.07.07   Time: 9:28
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 5  *****************
' User: Uha          Date: 11.07.07   Time: 18:22
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Report "Vergabe von Feinstaubplaketten" im Rohbau erzeugt
' 
' *****************  Version 4  *****************
' User: Uha          Date: 3.07.07    Time: 9:39
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 3  *****************
' User: Uha          Date: 22.05.07   Time: 9:45
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 2  *****************
' User: Uha          Date: 3.05.07    Time: 18:27
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' *****************  Version 1  *****************
' User: Uha          Date: 1.03.07    Time: 16:59
' Created in $/CKG/Components/ComCommon/ComCommonWeb
' 
' ************************************************