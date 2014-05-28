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


Partial Public Class JahresArchivManagement

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
    '''lnkUserManagement-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lnkUserManagement As Global.System.Web.UI.WebControls.HyperLink

    '''<summary>
    '''lnkContact-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lnkContact As Global.System.Web.UI.WebControls.HyperLink

    '''<summary>
    '''lnkGroupManagement-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lnkGroupManagement As Global.System.Web.UI.WebControls.HyperLink

    '''<summary>
    '''lnkOrganizationManagement-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lnkOrganizationManagement As Global.System.Web.UI.WebControls.HyperLink

    '''<summary>
    '''lnkCustomerManagement-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lnkCustomerManagement As Global.System.Web.UI.WebControls.HyperLink

    '''<summary>
    '''lnkArchivManagement-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lnkArchivManagement As Global.System.Web.UI.WebControls.HyperLink

    '''<summary>
    '''lnkJahresArchivManagement-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lnkJahresArchivManagement As Global.System.Web.UI.WebControls.HyperLink

    '''<summary>
    '''lnkAppManagement-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lnkAppManagement As Global.System.Web.UI.WebControls.HyperLink

    '''<summary>
    '''lblHead-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lblHead As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''divStart-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents divStart As Global.System.Web.UI.HtmlControls.HtmlGenericControl

    '''<summary>
    '''Label1-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents Label1 As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''tbDestinationYear-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents tbDestinationYear As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''lblDestinationYearHint-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lblDestinationYearHint As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''btnCopyYearArchives-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents btnCopyYearArchives As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''divResults-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents divResults As Global.System.Web.UI.HtmlControls.HtmlGenericControl

    '''<summary>
    '''lblResult-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lblResult As Global.System.Web.UI.WebControls.Label
End Class
