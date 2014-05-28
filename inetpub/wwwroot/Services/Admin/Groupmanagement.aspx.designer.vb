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


Partial Public Class Groupmanagement

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
    '''ihExpandstatusSearchFilterArea-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents ihExpandstatusSearchFilterArea As Global.System.Web.UI.HtmlControls.HtmlInputHidden

    '''<summary>
    '''ihNewExpandstatusSearchFilterArea-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents ihNewExpandstatusSearchFilterArea As Global.System.Web.UI.HtmlControls.HtmlInputHidden

    '''<summary>
    '''ihExpandstatusSearchResultArea-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents ihExpandstatusSearchResultArea As Global.System.Web.UI.HtmlControls.HtmlInputHidden

    '''<summary>
    '''ihNewExpandstatusSearchResultArea-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents ihNewExpandstatusSearchResultArea As Global.System.Web.UI.HtmlControls.HtmlInputHidden

    '''<summary>
    '''ihExpandStatusInputArea-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents ihExpandStatusInputArea As Global.System.Web.UI.HtmlControls.HtmlInputHidden

    '''<summary>
    '''ihNewExpandStatusInputArea-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents ihNewExpandStatusInputArea As Global.System.Web.UI.HtmlControls.HtmlInputHidden

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
    '''ddlFilterCustomer-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents ddlFilterCustomer As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''lblCustomer-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lblCustomer As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''trSelectOrganization-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents trSelectOrganization As Global.System.Web.UI.HtmlControls.HtmlTableRow

    '''<summary>
    '''txtFilterGroupName-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents txtFilterGroupName As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''lblGroupName-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lblGroupName As Global.System.Web.UI.WebControls.Label

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
    '''GridNavigation1-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents GridNavigation1 As Global.CKG.Services.GridNavigation

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
    '''txtGroupID-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents txtGroupID As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''txtGroupName-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents txtGroupName As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''trCustomer-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents trCustomer As Global.System.Web.UI.HtmlControls.HtmlTableRow

    '''<summary>
    '''txtCustomer-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents txtCustomer As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''txtDocuPath-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents txtDocuPath As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''trAccountingArea-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents trAccountingArea As Global.System.Web.UI.HtmlControls.HtmlTableRow

    '''<summary>
    '''txtCustomerID-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents txtCustomerID As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''trTVShow-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents trTVShow As Global.System.Web.UI.HtmlControls.HtmlTableRow

    '''<summary>
    '''cbxTeamViewer-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents cbxTeamViewer As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''trIsServiceGroup-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents trIsServiceGroup As Global.System.Web.UI.HtmlControls.HtmlTableRow

    '''<summary>
    '''cbxIsServiceGroup-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents cbxIsServiceGroup As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''trPwdRules-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents trPwdRules As Global.System.Web.UI.HtmlControls.HtmlTableRow

    '''<summary>
    '''ddlAuthorizationright-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents ddlAuthorizationright As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''cbxIsCustomerGroup-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents cbxIsCustomerGroup As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''trStartMethod-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents trStartMethod As Global.System.Web.UI.HtmlControls.HtmlTableRow

    '''<summary>
    '''txtStartMethod-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents txtStartMethod As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''txtNCapitalLetter-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents txtNCapitalLetter As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''cbxLevel-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents cbxLevel As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''trRights-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents trRights As Global.System.Web.UI.HtmlControls.HtmlTableRow

    '''<summary>
    '''tableAuthLevel-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents tableAuthLevel As Global.System.Web.UI.HtmlControls.HtmlTable

    '''<summary>
    '''Image2-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents Image2 As Global.System.Web.UI.WebControls.Image

    '''<summary>
    '''ddlAnwendung-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents ddlAnwendung As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''lblAutError-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lblAutError As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''gvAutorisierung-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents gvAutorisierung As Global.System.Web.UI.WebControls.GridView

    '''<summary>
    '''ibtNew-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents ibtNew As Global.System.Web.UI.WebControls.ImageButton

    '''<summary>
    '''Label3-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents Label3 As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''trApp-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents trApp As Global.System.Web.UI.HtmlControls.HtmlTableRow

    '''<summary>
    '''lstAppUnAssigned-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lstAppUnAssigned As Global.System.Web.UI.WebControls.ListBox

    '''<summary>
    '''btnAssign-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents btnAssign As Global.System.Web.UI.WebControls.ImageButton

    '''<summary>
    '''btnUnAssign-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents btnUnAssign As Global.System.Web.UI.WebControls.ImageButton

    '''<summary>
    '''lstAppAssigned-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lstAppAssigned As Global.System.Web.UI.WebControls.ListBox

    '''<summary>
    '''trArchiv-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents trArchiv As Global.System.Web.UI.HtmlControls.HtmlTableRow

    '''<summary>
    '''lstArchivUnAssigned-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lstArchivUnAssigned As Global.System.Web.UI.WebControls.ListBox

    '''<summary>
    '''btnAssignArchiv-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents btnAssignArchiv As Global.System.Web.UI.WebControls.ImageButton

    '''<summary>
    '''btnUnAssignArchiv-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents btnUnAssignArchiv As Global.System.Web.UI.WebControls.ImageButton

    '''<summary>
    '''lstArchivAssigned-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lstArchivAssigned As Global.System.Web.UI.WebControls.ListBox

    '''<summary>
    '''trMeldung-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents trMeldung As Global.System.Web.UI.HtmlControls.HtmlTableRow

    '''<summary>
    '''txtMaxReadMessageCount-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents txtMaxReadMessageCount As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''lblInfo-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents lblInfo As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''txtMessageOld-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents txtMessageOld As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''radMessage-Steuerelement
    '''</summary>
    '''<remarks>
    '''Automatisch generiertes Feld
    '''Um dies zu ändern, verschieben Sie die Felddeklaration aus der Designerdatei in eine Code-Behind-Datei.
    '''</remarks>
    Protected WithEvents radMessage As Global.Telerik.Web.UI.RadEditor

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
