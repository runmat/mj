

Neues Portal "PortalORM" für die Umstellung auf die neue SapConnector Schnittstelle "SapORM"
[ITA 6186 (Ablösung SAP-Connector und BizTalkAdapter im Portalbereich)]


1.
In der Portal-Datenbank eine neue Zeile in Tabelle "WebUserUploadLoginLink" einfügen
	ID TEXT
	7	http://localhost/PortalORM/Start/Login.aspx


2.
Im Kundenbereich einstellen:
- Login-URL = http://localhost/PortalORM/Start/Login.aspx
- CCS-Dateien und Bilder-Verzeichnisse ändern  von  /Portal/  auf  /PortalORM/


3.
Am IIS ein neues virtuelles Verzeichnis erstellen:  
"PortalORM"




