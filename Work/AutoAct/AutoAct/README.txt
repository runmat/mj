Assembly Abhängigkeiten.  
Die Anwendung hat eine Abhängigkeit auf die folgenden Assemblies:

SAP Konnektivität
DynSapProxyFactories.dll
DynSapProxyObjectErp35.dll
ERPConnect35.dll
GeneralTools.dll
SapORM.Contracts.dll
SapORM.Models.dll
SapORM.Services.dll

REST API
RestSharp.dll -> via nuget eingebunden
Newtonsoft.Json -> via nuget eingebunden

Referenzen
==========
Die Assemblies für die SAP Konnektivität werden aus dem Verzeichnis ..\..\SapORM\shell\bin\Debug\ ermittelt.
Frage: wie geht es dann mit dem Build Server? Eine Möglichkeit: 
- den kompletten Workspace auf dem Build Server laden
- erster Schritt bei konfiguration Build ist ein Build für SapORM
- ein Kopieren der Assemblies ist nicht notwendig da AutoAct relative Referenzen verwendet um die dlls zu finden
- Build von AutoAct läuft dann ohne Probleme

Verbindungsdaten
================
Die Anwendung bezieht die Verbdiungsdaten aus der eingebetteten Resource web.config in SapORM.Services.  Die Initialisierung der Verbidnungsdaten erfolgt durch den Aufruf 
ConfigurationMerger.MergeTestWebConfigAppSettings();
In einer Test- oder Produktionsumgebung müssen andere Konfigurationsdaten ermittelt werden.  
Jetzt ist es möglich Verbindungsdaten in der app.config abzulegen.  
app.config beinhaltet die Verbindungsdaten für Debug
app.release.config beinhaltet die Vebindungsdaten für Release
Das Dokument http://msdn.microsoft.com/de-de/library/dd465318(v=vs.100).aspx beschreibt die Handhabung der Transformationen
Kann MergeTestWebConfigAppSettings mit der eignen Konfigurationsdatei aufgerufen werden?

Authentifizierung gegenüber AutoAct
===================================
Release: Verbindungsdaten werden auf SAP per Kunde eingetragen
Test/Debug: Verbindungsdaten müssen in der app.config vorhanden sein. Kunden Verbindungsaten aus SAP werden ignoriert

Proxy Einstellungen
===================
Die IT Administration hat auf der Firewall Seite eine Ausnahmeregel für die Adresse http://www.sandbox.autoact.de erstellt.  Dies bedeutet dass diese Anwendung Verbindungen zur Adresse aufbauen können ohne dass das firewall eine Authentifizierung verlangt.
Um die Anwendung in einer Umgebung auszuführen wo keine Ausnahmeregel für das Firewall existiert, bitte folgenden Abschnitt in der app.config erstellen.
  <system.net>
    <defaultProxy useDefaultCredentials="true">
      <proxy proxyaddress="http://proxy.kroschke.de:8080" bypassonlocal="false"/>
    </defaultProxy>
    <settings>
      <servicePointManager expect100Continue="false"/>
    </settings>
  </system.net>

Viele Ids
=========
ein Fahrzeug hat drei Identifikatoren
- Eine Identifikation seitens des Kunden des DADs.  Die interne Nummer des Fahrzeugs im IT-System des Kunden
- Eine Belegnummer in SAP
- Eine Inserats Id in AutoAct

Wenn ich z.B. ein Fahrzeug in AutoAct ansprechen möchte, z.B. Anhang hochladen, dann muss ich die InseratsId verwenden
Wenn ich den Übertragungsstatus für ein Fahrzeug in DADs SAP ändern möchte dann muss ich die SAP Belegsnummer verwenden
Kommunikation mit den Kunden des DAD's verwendet die interne Nummer des Kunden
Problem: das Bindeglied zwischen den beiden Systemen ist die Referenz Id des Kunden. 
Um einen Mapping herzustellen zwischen den System muss ich alle Fahrzeuge (SAP) und Vehicles (AutoAct) für einen Kunden laden und via die Referenz mappen.

Anhänge und Bilder
==================
Warum ist das Laden der Anhänge (Attachments) und das Laden der Bilder getrennt von einander?
Der Grund ist ein Implementierungsdetail der AutoAct. Das Hochladen von Dokumenten ist anders gestaltet als der von Bildern
- Dokumente erfodern Paramter zum Dokument wie Verwendungs (Zustand-, Schadensbericht) -> Bilder haben das nicht
- Name des Dokuments -> Bilder haben das nicht

Warum werden Anhänge nicht en masse geladen?
Dies ist möglich laut der Spezifikation der API.  Problem dabei ist dass folgende Reihenfolge der Parts (in http Request) erforderlich ist, index des Dokuments in Klammern
- Type[0]
- Name[0]
- File[0]
- Type[1]
- Name[1]
- File[1]

RestSharp erlaubt eine solche Justierung der Paramter nicht. Übertragung sieht folgendermaßen aus
- Type[0]
- Name[0]
- Type[1]
- Name[1]
- File[0]
- File[1]
Daher werden die Dateien einzeln hochgeladen