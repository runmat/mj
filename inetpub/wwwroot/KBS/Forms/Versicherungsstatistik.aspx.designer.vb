'------------------------------------------------------------------------------
' <automatisch generiert>
'     Der Code wurde von einem Tool generiert.
'
'     Änderungen an der Datei führen möglicherweise zu falschem Verhalten, und sie gehen verloren, wenn
'     der Code erneut generiert wird. 
' </automatisch generiert>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Partial Public Class Versicherungsstatistik

    '''<summary>
    '''lb_zurueck-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lb_zurueck As Global.System.Web.UI.WebControls.LinkButton

    '''<summary>
    '''lblHead-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lblHead As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''tab1-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents tab1 As Global.System.Web.UI.HtmlControls.HtmlTable

    '''<summary>
    '''Label1-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents Label1 As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''ddlFilter-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents ddlFilter As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''txtEVB-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents txtEVB As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''ddlArtikel-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents ddlArtikel As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''ibtnFilter-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents ibtnFilter As Global.System.Web.UI.WebControls.ImageButton

    '''<summary>
    '''ibtnNoFilter-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents ibtnNoFilter As Global.System.Web.UI.WebControls.ImageButton

    '''<summary>
    '''txtDatumVon-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents txtDatumVon As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''CetxtDatumVon-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents CetxtDatumVon As Global.AjaxControlToolkit.CalendarExtender

    '''<summary>
    '''MeetxtDatumVon-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents MeetxtDatumVon As Global.AjaxControlToolkit.MaskedEditExtender

    '''<summary>
    '''Label6-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents Label6 As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''txtDatumBis-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents txtDatumBis As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''CetxtDatumBis-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents CetxtDatumBis As Global.AjaxControlToolkit.CalendarExtender

    '''<summary>
    '''MeetxtDatumBis-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents MeetxtDatumBis As Global.AjaxControlToolkit.MaskedEditExtender

    '''<summary>
    '''cmdCreate-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents cmdCreate As Global.System.Web.UI.WebControls.LinkButton

    '''<summary>
    '''lblError-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lblError As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''lblMessage-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lblMessage As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''dataHeader-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents dataHeader As Global.System.Web.UI.HtmlControls.HtmlGenericControl

    '''<summary>
    '''lblNoData-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lblNoData As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''gvVersicherungen-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents gvVersicherungen As Global.System.Web.UI.WebControls.GridView
End Class
