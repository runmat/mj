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


Partial Public Class ForbiddenUserNameManagement

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
    '''DivSearch1-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents DivSearch1 As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''tableSearch-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents tableSearch As Global.System.Web.UI.HtmlControls.HtmlTable

    '''<summary>
    '''trSelectOrganization-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents trSelectOrganization As Global.System.Web.UI.HtmlControls.HtmlTableRow

    '''<summary>
    '''txtFilterForbiddenUserNameName-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents txtFilterForbiddenUserNameName As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''lblForbiddenUserNameName-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lblForbiddenUserNameName As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''txtEmpty-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents txtEmpty As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''btnEmpty-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents btnEmpty As Global.System.Web.UI.WebControls.ImageButton

    '''<summary>
    '''QueryFooter-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents QueryFooter As Global.System.Web.UI.HtmlControls.HtmlGenericControl

    '''<summary>
    '''btnSuche-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents btnSuche As Global.System.Web.UI.WebControls.LinkButton

    '''<summary>
    '''lbtnNew-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lbtnNew As Global.System.Web.UI.WebControls.LinkButton

    '''<summary>
    '''Result-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents Result As Global.System.Web.UI.HtmlControls.HtmlGenericControl

    '''<summary>
    '''trSearchSpacer1-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents trSearchSpacer1 As Global.System.Web.UI.HtmlControls.HtmlGenericControl


    '''<summary>
    '''trSearchResult-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents trSearchResult As Global.System.Web.UI.HtmlControls.HtmlTableRow

    '''<summary>
    '''dgSearchResult-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents dgSearchResult As Global.System.Web.UI.WebControls.GridView

    '''<summary>
    '''Input-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents Input As Global.System.Web.UI.HtmlControls.HtmlGenericControl

    '''<summary>
    '''Tablex1-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents Tablex1 As Global.System.Web.UI.HtmlControls.HtmlTable

    '''<summary>
    '''trEditUser-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents trEditUser As Global.System.Web.UI.HtmlControls.HtmlTableRow

    '''<summary>
    '''txtForbiddenUserNameName-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents txtForbiddenUserNameName As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''txtID-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents txtID As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''lbtnDelete-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lbtnDelete As Global.System.Web.UI.WebControls.LinkButton

    '''<summary>
    '''lbtnConfirm-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lbtnConfirm As Global.System.Web.UI.WebControls.LinkButton

    '''<summary>
    '''lbtnSave-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lbtnSave As Global.System.Web.UI.WebControls.LinkButton

    '''<summary>
    '''lbtnCancel-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lbtnCancel As Global.System.Web.UI.WebControls.LinkButton
End Class
