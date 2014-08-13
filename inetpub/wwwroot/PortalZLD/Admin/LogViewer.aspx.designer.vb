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


Partial Public Class LogViewer

    '''<summary>
    '''lblHead-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lblHead As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''UpdatePanel1-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents UpdatePanel1 As Global.System.Web.UI.UpdatePanel

    '''<summary>
    '''tab1-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents tab1 As Global.System.Web.UI.HtmlControls.HtmlTable

    '''<summary>
    '''lblError-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lblError As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''lblCustomer-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lblCustomer As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''ddlFilterCustomer-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents ddlFilterCustomer As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''trGruppe-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents trGruppe As Global.System.Web.UI.HtmlControls.HtmlTableRow

    '''<summary>
    '''lblGroup-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lblGroup As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''ddlFilterGroup-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents ddlFilterGroup As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''trOrganisation-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents trOrganisation As Global.System.Web.UI.HtmlControls.HtmlTableRow

    '''<summary>
    '''lblOrganisation-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lblOrganisation As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''ddlFilterOrganization-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents ddlFilterOrganization As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''txtUserID-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents txtUserID As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''lblUserName-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lblUserName As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''txtFilterUserName-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents txtFilterUserName As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''btnSuche-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents btnSuche As Global.System.Web.UI.WebControls.LinkButton

    '''<summary>
    '''lblVonDatum-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lblVonDatum As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''txtVonDatum-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents txtVonDatum As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''txtVonDatum_CalendarExtender-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents txtVonDatum_CalendarExtender As Global.AjaxControlToolkit.CalendarExtender

    '''<summary>
    '''lblBisDatum-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lblBisDatum As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''txtBisDatum-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents txtBisDatum As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''txtBisDatum_CalendarExtender-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents txtBisDatum_CalendarExtender As Global.AjaxControlToolkit.CalendarExtender

    '''<summary>
    '''ddlAction-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents ddlAction As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Queryfooter-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents Queryfooter As Global.System.Web.UI.HtmlControls.HtmlGenericControl

    '''<summary>
    '''lbCreate-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lbCreate As Global.System.Web.UI.WebControls.LinkButton

    '''<summary>
    '''Result-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents Result As Global.System.Web.UI.HtmlControls.HtmlGenericControl

    '''<summary>
    '''NavExcel-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents NavExcel As Global.System.Web.UI.HtmlControls.HtmlGenericControl

    '''<summary>
    '''lnkCreateExcel1-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lnkCreateExcel1 As Global.System.Web.UI.WebControls.LinkButton

    '''<summary>
    '''TblUser-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents TblUser As Global.System.Web.UI.HtmlControls.HtmlTable

    '''<summary>
    '''dgSearchResult-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents dgSearchResult As Global.System.Web.UI.WebControls.DataGrid
End Class
