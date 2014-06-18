Erstellung der Modelklasse
==========================
Vom Kunden haben wir eine XSD Schema Datei erhalten hla-dad_130814.xsd.  Damit ich diese auch im SOAP Dienst einbinden kann, habe ich mit Hilfe der Anwendung XSD C# Klassen erstellt.
Die Anwendung XSD kann über die Visual Studio Eingabeaufforderung aufrufen.  Die Aufrufparamter sind:

xsd hla-dad_130814.xsd /classes

Dieser Aufruf generiert eine C# Datei mit den gemappten Klassen aus dem Schema (Ruecklaeuferschnittstelle.cs).  Diese Datei kann ich in das Projekt einbinden.  Achtugn: Der Namespace muss korrgiert werden.

Fiddler
=======
Damit Fiddler die Aufrufe sehen kann folgendermaßen vorgehen.
- Notepad als Administrator öffnen
- Datei C:\Windows\System32\drivers\etc\hosts öffnen
- Folgende Zeile am Ende der Datei einfügen.  Achtung: der Punkt am Ende ist Bestandteil der Adresse
127.0.0.1       localhost.

Im Aufruf Projekt (die Exe mit der die Aufrufe getesetet werden) folgene Änderungen vornehmen:
- Vor dem Aufruf der SOAP Schnittstelle den globalen Proxy setzen
WebRequest.DefaultWebProxy = new WebProxy("127.0.0.1", 8888);
- Bei der Erstellung der Web Referenz darauf achten dass am Ende der Server Bezeichnung (localhost) ein Punkt geschrieben wird.
- in der app.config folgenden Abschnitt erstellen
  <system.net>
    <defaultProxy>
      <proxy bypassonlocal="False" usesystemdefault="True" />
    </defaultProxy>
  </system.net>
- Den Abschnitt system.serviceModel\client\endpoint, Element address: bitte darauf achten dass ein Punkt am ende der lokalen Adresse geschrieben ist.



