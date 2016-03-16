<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CustomerManagement.aspx.vb"
    Inherits="Admin.CustomerManagement" EnableEventValidation="false" MasterPageFile="MasterPage/Admin.Master" %>
<%@ Import Namespace="CKG.Base.Kernel.Security" %>
<%@ Import Namespace="GeneralTools.Models" %>

<%@ Import Namespace="System.Web.UI.WebControls.WebParts" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="atkHtmlEdit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../PageElements/GridNavigation.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Button ID="btnUpload" runat="server" Text="UplSubmit" Style="display: none;" />
    <script type="text/javascript">

        var HighlightAnimations = {};
        function Highlight(el) {
            if (HighlightAnimations[el.uniqueID] == null) {
                HighlightAnimations[el.uniqueID] = AjaxControlToolkit.Animation.createAnimation({
                    AnimationName: "color",
                    duration: 0.5,
                    property: "style",
                    propertyKey: "backgroundColor",
                    startValue: "#FFFF90",
                    endValue: "#FFFFFF"
                }, el);
            }
            HighlightAnimations[el.uniqueID].stop();
            HighlightAnimations[el.uniqueID].play();
        }


        function validationFailed(sender, eventArgs) {
            alert('Nur die Dateientypen *.jpg, *.jpeg, *.png und *.gif werden unterstützt!');
            onFileUploaded("", "");

        }
  
    </script>
    <script src="../JScript/Jquery/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="../JScript/Jquery/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <link href="../JScript/Jquery/MSDropdown/dd.css" rel="stylesheet" type="text/css" />
    <script src="../JScript/Jquery/MSDropdown/js/jquery.dd.js" type="text/javascript"></script>
    <script src="../PageElements/SearchForm/Scripts/jquery-textbox-selection.js" type="text/javascript"></script>
    <div>
        <div id="site">
            <div id="content">
                <div id="innerContent">
                    <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                    <asp:Label ID="lblMessage" runat="server" CssClass="TextExtraLarge" EnableViewState="False"></asp:Label>
                    <div id="navigationSubmenu">
                        <asp:HyperLink ID="lnkUserManagement" runat="server" ToolTip="Benutzer" NavigateUrl="UserManagement.aspx"
                            Text="Benutzer | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkContact" runat="server" ToolTip="Ansprechpartner" NavigateUrl="Contact.aspx"
                            Text="Ansprechpartner | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkGroupManagement" runat="server" ToolTip="Gruppen" NavigateUrl="GroupManagement.aspx"
                            Text="Gruppen | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkOrganizationManagement" runat="server" ToolTip="Organisationen"
                            NavigateUrl="OrganizationManagement.aspx" Text="Organisation | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkAppManagement" runat="server" ToolTip="Anwendungen" NavigateUrl="AppManagement.aspx"
                            Text="Anwendungen | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkArchivManagement" ToolTip="Archive" runat="server" NavigateUrl="ArchivManagement.aspx"
                            Text="Archive | "></asp:HyperLink>
                    </div>
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                            </h1>
                            <span id="arealnkSuche" class="AdminMgmtNav" style="display: none">&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;</span><a id="lnkSuche" class="AdminMgmtNavLink" href="javascript:void(0)" onclick="showSearchFilterArea();" style="display: none">Suche</a>
                            <span id="arealnkSuchergebnis" class="AdminMgmtNav" style="display: none">&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;</span><a id="lnkSuchergebnis" class="AdminMgmtNavLink" href="javascript:void(0)" onclick="showSearchResultArea();" style="display: none">Suchergebnis</a>
                            <%-- ih... enthält jeweils den Sollwert für den nächsten Seitenzustand --%>
                            <input id="ihExpandstatusSearchFilterArea" type="hidden" runat="server" value="1"/>
                            <input id="ihExpandstatusSearchResultArea" type="hidden" runat="server" value="0"/>
                            <input id="ihExpandStatusInputArea" type="hidden" runat="server" value="0"/>
                        </div>
                        <asp:Panel ID="DivSearch1" runat="server" DefaultButton="btnEmpty" style="display: none">
                            <div id="TableQuery">
                                <table id="tableSearch" runat="server" cellspacing="0" cellpadding="0">
                                    <tbody>
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Firma:
                                            </td>
                                            <td class="firstLeft active" nowrap="nowrap" width="100%">
                                                <asp:TextBox ID="txtFilterCustomerName" runat="server" CssClass="InputTextbox">**</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                &nbsp;
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:TextBox ID="txtEmpty" runat="server" CssClass="InputTextbox" Width="160px" Visible="False">*</asp:TextBox>
                                                <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="../images/empty.gif"
                                                    Width="1px" />
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                &nbsp;
                                            </td>
                                            <td class="firstLeft active">
                                            </td>
                                        </tr>
                                        <tr style="background-color: #dfdfdf; height: 22px">
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div id="QueryFooter" runat="server">
                                    <div id="dataQueryFooter">
                                        &nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton class="Tablebutton" ID="btnSuche" runat="server"
                                            Text="Suchen&amp;nbsp;&amp;#187; " CssClass="TablebuttonXLarge" Height="16px"
                                            Width="155px" Font-Names="Verdana,sans-serif" Font-Size="10px"></asp:LinkButton>
                                        &nbsp;<asp:LinkButton ID="lbtnNew" runat="server" Text="Neuer Kunde&amp;nbsp;&amp;#187; "
                                            CssClass="TablebuttonXLarge" Height="16px" Width="155px" Font-Names="Verdana,sans-serif"
                                            Font-Size="10px"></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <div id="Result" runat="Server" style="display: none">
                            <div id="pagination">
                                <uc2:GridNavigation ID="GridNavigation1" runat="server" PageSizeIndex="0" />
                            </div>
                            <div id="data">
                                <asp:Panel ID="Panel1" runat="server">
                                    <table cellspacing="0" style="border-color: #ffffff" cellpadding="0" width="100%"
                                        align="left" border="0">
                                        <tbody>
                                            <tr id="trSearchResult" runat="server">
                                                <td align="left">
                                                    <asp:GridView ID="dgSearchResult" Width="1100px" runat="server" AutoGenerateColumns="False"
                                                        CellPadding="0" CellSpacing="0" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                                        AllowSorting="true" AllowPaging="True" CssClass="GridView customer-grid" PageSize="10">
                                                        <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                                        <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                        <PagerSettings Visible="False" />
                                                        <RowStyle CssClass="ItemStyle" />
                                                        <EmptyDataRowStyle BackColor="#DFDFDF" />
                                                        <Columns>
                                                            <asp:TemplateField Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCustomerID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CustomerID") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:ButtonField CommandName="Del" HeaderStyle-ForeColor="White" HeaderText="Löschen"
                                                                ButtonType="Image" ImageUrl="../../Images/Papierkorb_01.gif" ControlStyle-Height="16px"
                                                                ControlStyle-Width="16px"></asp:ButtonField>
                                                            <asp:ButtonField DataTextField="CustomerName" SortExpression="CustomerName" HeaderText="Kunde"
                                                                CommandName="Edit"></asp:ButtonField>
                                                            <asp:BoundField DataField="KUNNR" SortExpression="KUNNR" HeaderText="KUNNR"></asp:BoundField>
                                                            <asp:BoundField DataField="AccountingArea" SortExpression="AccountingArea" HeaderText="Buchungs- kreis">
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CName" SortExpression="CName" HeaderText="Kontakt-Name">
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CAddress" SortExpression="CAddress" HeaderText="Kontakt- Adresse">
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Mail-Adresse" HeaderStyle-ForeColor="White">
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="HyperLink1" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.CMail") %>'
                                                                        Text='<%# DataBinder.Eval(Container, "DataItem.CMailDisplay") %>' runat="server"></asp:HyperLink>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:HyperLinkField DataNavigateUrlFields="CWeb" DataTextField="CWebDisplay" SortExpression="CWebDisplay"
                                                                HeaderText="Web-Adresse"></asp:HyperLinkField>
                                                            <asp:BoundField DataField="CountUsers" SortExpression="CountUsers" HeaderText="Benutzeranzahl">
                                                            </asp:BoundField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;&nbsp;
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </asp:Panel>
                            </div>
                            <div>
                                &nbsp;
                            </div>
                        </div>
                        <div id="Input" runat="server" style="display: none">
                            <div id="adminInput" style="padding-top: 10px">
                                <ajaxToolkit:TabContainer runat="server" ID="Tabs" ActiveTabIndex="3" Width="100%"
                                    CssClass="ajax__tab_xp" BorderStyle="None">
                                    <%-- Height="500px"--%>
                                    <ajaxToolkit:TabPanel runat="server" ID="KunnDat" HeaderText="Kunden-Daten">
                                        <ContentTemplate>
                                            <table id="tblKunDaten" style="border-color: #FFFFFF; font-size: 10px;" cellspacing="0"
                                                cellpadding="0" width="100%" border="0">
                                                <tr>
                                                    <td colspan="2">
                                                        <table id="tblLeft" style="margin-right: 30px; border-color: #FFFFFF;">
                                                            <tr class="formquery">
                                                                <td class="firstLeft active">
                                                                    Firmenname:
                                                                    <input type="hidden" id="ihCustomerID" runat="server" value="-1" />
                                                                </td>
                                                                <td class="active" width="100%">
                                                                    <asp:TextBox ID="txtCustomerName" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active">
                                                                    KUNNR
                                                                </td>
                                                                <td class="active">
                                                                    <asp:TextBox ID="txtKUNNR" runat="server" CssClass="InputTextbox">0</asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery" id="trMaster" runat="server">
                                                                <td class="firstLeft active">
                                                                    Master
                                                                </td>
                                                                <td class="active">
                                                                    <span>
                                                                        <asp:CheckBox ID="cbxMaster" runat="server"></asp:CheckBox></span>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery" id="trAccountingArea" runat="server">
                                                                <td class="firstLeft active">
                                                                    Buchungskreis
                                                                </td>
                                                                <td class="active">
                                                                    <asp:DropDownList ID="ddlAccountingArea" runat="server">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active">
                                                                    Portallink:
                                                                </td>
                                                                <td class="active">
                                                                    <span>
                                                                        <asp:DropDownList ID="ddlPortalLink" runat="server" />
                                                                    </span>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active">
                                                                    Ausschließlich Login über<br/>o.g. Link erlauben:
                                                                </td>
                                                                <td class="active">
                                                                    <span>
                                                                        <asp:CheckBox ID="cbxForceSpecifiedLoginLink" runat="server"/>
                                                                    </span>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active">
                                                                    Redirect-URL nach Logout:
                                                                </td>
                                                                <td class="active">
                                                                    <asp:TextBox ID="txtLogoutLink" runat="server" CssClass="InputTextbox"/>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery" id="trPortalType" runat="server">
                                                                <td class="firstLeft active">
                                                                    Portal-Typ:
                                                                </td>
                                                                <td class="active">
                                                                    <span>
                                                                        <asp:DropDownList ID="ddlPortalType" runat="server" />
                                                                    </span>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery" id="trMvcSelectionUrl" runat="server">
                                                                <td class="firstLeft active">
                                                                    Selection Typ (nur MVC):
                                                                </td>
                                                                <td class="active">
                                                                    <span>
                                                                        <asp:DropDownList ID="ddlMvcSelectionType" runat="server" CssClass="selection-type-dropdown" />
                                                                    </span>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery" id="tr2" runat="server">
                                                                <td class="firstLeft active">
                                                                    Selection URL (nur MVC):
                                                                </td>
                                                                <td class="active">
                                                                    <asp:TextBox ID="txtMvcSelectionUrl" runat="server" CssClass="InputTextbox selection-url-textbox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery" id="trKundeSperr" runat="server">
                                                                <td class="firstLeft active">
                                                                    Kunde sperren:
                                                                </td>
                                                                <td class="active">
                                                                    <span>
                                                                        <asp:CheckBox ID="chkKundenSperre" runat="server" Enabled="false" />
                                                                    </span>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery" id="trTeamViewer" runat="server">
                                                                <td class="firstLeft active">
                                                                    TeamViewer verwenden:
                                                                </td>
                                                                <td class="active">
                                                                    <span>
                                                                        <asp:CheckBox ID="chkTeamviewer" runat="server" />
                                                                    </span>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active">
                                                                    Kontakt-Name:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <asp:TextBox ID="txtCName" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td colspan="2">
                                                        <table id="tblRight" style="border-color: #FFFFFF;">
                                                            <tr class="formquery">
                                                                <td colspan="2">
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active">
                                                                    Referenz 1:
                                                                </td>
                                                                <td class="active">
                                                                    <asp:DropDownList ID="ddlReferenzTyp1" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active">
                                                                    Referenz 2:
                                                                </td>
                                                                <td class="active">
                                                                    <asp:DropDownList ID="ddlReferenzTyp2" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active">
                                                                    Referenz 3:
                                                                </td>
                                                                <td class="active">
                                                                    <asp:DropDownList ID="ddlReferenzTyp3" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active">
                                                                    Referenz 4<br/>(bool -> Checkbox):
                                                                </td>
                                                                <td class="active">
                                                                    <asp:DropDownList ID="ddlReferenzTyp4" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td colspan="2">
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td colspan="2">
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td colspan="2">
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td colspan="2">
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td colspan="2">
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td colspan="2">
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td colspan="2">
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td colspan="2">
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td colspan="2">
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="vertical-align: top; width: 175px">
                                                        Kontakt-Adresse:
                                                    </td>
                                                    <td class="active" colspan="3">
                                                        <atkHtmlEdit:Editor ID="EditCAddress" runat="server" Width="680px"></atkHtmlEdit:Editor>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="width: 175px">
                                                        Mailadresse Anzeigetext:
                                                    </td>
                                                    <td class="active" colspan="3">
                                                        <asp:TextBox ID="txtCMailDisplay" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="width: 175px">
                                                        Mailadresse:
                                                    </td>
                                                    <td class=" active" colspan="3">
                                                        <asp:TextBox ID="txtCMail" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td nowrap="nowrap" class="firstLeft active" style="width: 175px">
                                                        Web-Adresse Anzeigetext:
                                                    </td>
                                                    <td align="left" class="active" colspan="3">
                                                        <asp:TextBox ID="txtCWebDisplay" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td height="22" class="firstLeft active" style="width: 175px">
                                                        Web-Adresse:
                                                    </td>
                                                    <td align="left" class=" active" colspan="3">
                                                        <asp:TextBox ID="txtCWeb" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td nowrap="nowrap" colspan="4" class="firstLeft active" style="border-left: solid 1px #595959;
                                                        border-right: solid 1px #595959; border-top: solid 1px #595959; background-color: #bbbbbb;
                                                        color: White; padding-bottom: 5px;">
                                                        Kundenkontakt
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td nowrap="nowrap" class="firstLeft active" style="border-left: solid 1px #595959; width: 175px">
                                                        Kundenpostfach:
                                                    </td>
                                                    <td align="left" class="active" style="border-right: solid 1px #595959;" colspan="3">
                                                        <asp:TextBox ID="txtKundepostfach" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td nowrap="nowrap" class="firstLeft active" style="border-left: solid 1px #595959; width: 175px">
                                                        Kundenhotline:
                                                    </td>
                                                    <td align="left" class="active" style="border-right: solid 1px #595959;" colspan="3">
                                                        <asp:TextBox ID="txtKundenhotline" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td nowrap="nowrap" class="firstLeft active" style="border-left: solid 1px #595959;
                                                        border-bottom: solid 1px #595959; width: 175px">
                                                        Kundenfax:
                                                    </td>
                                                    <td align="left" class="active" style="border-right: solid 1px #595959; border-bottom: solid 1px #595959;
                                                        padding-bottom: 5px;" colspan="3">
                                                        <asp:TextBox ID="txtKundenfax" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </ajaxToolkit:TabPanel>
                                    <ajaxToolkit:TabPanel runat="server" ID="LoginRules" HeaderText="Login/Passwort-Regeln">
                                        <ContentTemplate>
                                            <table id="Table3" style="border-color: #FFFFFF; font-size: 10px;" cellspacing="0"
                                                cellpadding="0" width="100%" border="0">
                                                <tr class="formquery">
                                                    <td colspan="2">
                                                        <div class="formqueryHeader">
                                                            <span>Login-Regeln</span>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Neues Passwort&nbsp;nach n Tagen:
                                                    </td>
                                                    <td class="active" width="100%">
                                                        <asp:TextBox ID="txtNewPwdAfterNDays" runat="server" CssClass="InputTextbox">60</asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Konto sperren nach n Fehlversuchen:
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtLockedAfterNLogins" runat="server" CssClass="InputTextbox">3</asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Mehrfaches Login erlaubt:
                                                    </td>
                                                    <td class="active">
                                                        <span>
                                                            <asp:CheckBox ID="chkAllowMultipleLogin" runat="server" Checked="True"></asp:CheckBox></span>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Remote Login über URL erlaubt:
                                                    </td>
                                                    <td class="active">
                                                        <span>
                                                            <asp:CheckBox ID="chkAllowUrlRemoteLogin" runat="server" Checked="False"></asp:CheckBox></span>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td colspan="2">
                                                        <div class="formqueryHeader">
                                                            <span>Passwort-Regeln</span>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Mindestlänge:
                                                    </td>
                                                    <td class="cellBorderGray active">
                                                        <asp:TextBox ID="txtPwdLength" runat="server" CssClass="InputTextbox">8</asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        n numerische Zeichen:
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtPwdNNumeric" runat="server" CssClass="InputTextbox">1</asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        n Großbuchstaben:
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtNCapitalLetter" runat="server" CssClass="InputTextbox">1</asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        n Sonderzeichen:
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtNSpecialCharacter" runat="server" CssClass="InputTextbox">1</asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery" id="trPwdHistoryNEntries" runat="server">
                                                    <td id="Td1" class="firstLeft active" runat="server">
                                                        Sperre letze n Passwörter:
                                                    </td>
                                                    <td id="Td2" class="active" runat="server">
                                                        <asp:TextBox ID="txtPwdHistoryNEntries" runat="server" CssClass="InputTextbox">6</asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Passwort nicht per Email:
                                                    </td>
                                                    <td class="active">
                                                        <span>
                                                            <asp:CheckBox ID="cbxPwdDontSendEmail" runat="server"></asp:CheckBox>
                                                        </span>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Username nicht per Email:
                                                    </td>
                                                    <td class="active">
                                                        <span>
                                                            <asp:CheckBox ID="cbxUsernameSendEmail" runat="server"></asp:CheckBox>
                                                        </span>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Namensangaben keine Pflichtfelder:
                                                    </td>
                                                    <td class="active">
                                                        <span>
                                                            <asp:CheckBox ID="cbxNameInputOptional" runat="server"></asp:CheckBox></span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </ajaxToolkit:TabPanel>
                                    <ajaxToolkit:TabPanel runat="server" ID="Benutzer" HeaderText="Benutzer/Organisation">
                                        <HeaderTemplate>
                                            Benutzer/Organisation
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <table id="tblStyle1" style="border-color: #FFFFFF; font-size: 10px;" cellspacing="0"
                                                cellpadding="0" width="100%" border="0">
                                                <tr class="formquery">
                                                    <td colspan="2">
                                                        <div class="formqueryHeader">
                                                            <span>Benutzer</span>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Max. Anzahl&nbsp;Benutzer:
                                                    </td>
                                                    <td class="active" style="width: 200px">
                                                        <asp:TextBox ID="txtMaxUser" runat="server" Width="50px" CssClass="InputTextbox">600</asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="vertical-align: top">
                                                        Kundenadministration:
                                                    </td>
                                                    <td style="vertical-align: top;width: 200px" align="left">
                                                        <asp:UpdatePanel ID="rbUpdate" runat="server">
                                                            <ContentTemplate>
                                                                <asp:RadioButton ID="rbKeine" Text="keine" runat="server" AutoPostBack="True" GroupName="Admin"
                                                                    Width="15px" />
                                                                <br />
                                                                <asp:RadioButton ID="rbvollst" Text="vollständig" runat="server" GroupName="Admin"
                                                                    AutoPostBack="True" Width="15px" />
                                                                <br />
                                                                <asp:RadioButton ID="rbeing" Text="eingeschränkt" runat="server" GroupName="Admin"
                                                                    AutoPostBack="True" Width="15px" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td id="Td3" class="firstLeft active" style="vertical-align: top;" runat="server">
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="rbeing" EventName="CheckedChanged" />
                                                            </Triggers>
                                                            <ContentTemplate>
                                                                <asp:Label ID="lblKundenadministrationInfo" class="firstLeft active"  style="font-weight: bold; padding-left: 0px" 
                                                                    runat="server">Beschreibung eingeschränkte<br />Administration:</asp:Label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td id="Td4" class="active" runat="server" style="width: 200px">
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="rbeing" EventName="CheckedChanged" />
                                                            </Triggers>
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtKundenadministrationBeschreibung" runat="server" TextMode="MultiLine"
                                                                    Rows="3"></asp:TextBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td id="Td5" class="firstLeft active" style="vertical-align: top;" runat="server">
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="rbKeine" EventName="CheckedChanged" />
                                                            </Triggers>
                                                            <ContentTemplate>
                                                                <asp:Label ID="lblKundenadministrationContact" class="firstLeft active"  style="font-weight: bold; padding-left: 0px" 
                                                                    runat="server">Hinweis/Kontakt<br />Kundenadministration:</asp:Label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td id="Td6" class="active" runat="server" style="width: 200px">
                                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="rbKeine" EventName="CheckedChanged" />
                                                            </Triggers>
                                                            <ContentTemplate>
                                                                <atkHtmlEdit:Editor ID="EditKundenadministrationContact" runat="server" Width="680px"></atkHtmlEdit:Editor>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td id="Td7" class="firstLeft active" style="vertical-align: top;" runat="server">
                                                        Autom. Benutzersperrung nach:
                                                    </td>
                                                    <td class="active" style="vertical-align: top;width: 200px">
                                                        <asp:TextBox ID="txtUserLockTime" runat="server" MaxLength="4" Width="40px"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="ftbeUserLockTime" runat="server" TargetControlID="txtUserLockTime"
                                                            FilterType="Numbers" Enabled="True">
                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                        &nbsp;Tagen
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td id="Td8" class="firstLeft active" style="vertical-align: top;" runat="server">
                                                        Autom. Benutzer löschen
                                                        <br />
                                                        nach Sperrung:
                                                    </td>
                                                    <td class="active" style="vertical-align: top;width: 200px">
                                                        <asp:TextBox ID="txtUserDeleteTime" runat="server" MaxLength="4" Width="40px"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="ftbeUserDeleteTime" runat="server" TargetControlID="txtUserDeleteTime"
                                                            FilterType="Numbers" Enabled="True">
                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                        &nbsp;Tagen
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr id="trKundenInfo" runat="server">
                                                    <td class="InfoBoxFlat" colspan="2" style="background-color: #DDDDDD">
                                                        <table runat="server" width="100%" id="tblKundenInfo">
                                                            <tr>
                                                                <td class="firstLeft active">
                                                                    <u>Eintragen</u>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table id="tblVersandanschrift" runat="server" cellspacing="0" cellpadding="5" width="50%"
                                                                        style="border-width: 0px">
                                                                        <tr>
                                                                            <td class="firstLeft active">
                                                                                <asp:Label ID="lbl_Name" Text="Name" runat="server"></asp:Label>
                                                                            </td>
                                                                            <td class="active" width="305px">
                                                                                <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                                                                            </td>
                                                                            <td class="firstLeft active">
                                                                                <asp:Label ID="lbl_Telefon" Text="Telefon" runat="server"></asp:Label>
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:TextBox ID="txtTelefon" runat="server"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="firstLeft active">
                                                                                <asp:Label ID="lbl_Vorname" Text="Vorname" runat="server"></asp:Label>
                                                                            </td>
                                                                            <td class="active" width="305px">
                                                                                <asp:TextBox ID="txtVorname" runat="server"></asp:TextBox>
                                                                            </td>
                                                                            <td class="firstLeft active">
                                                                                <asp:Label ID="lbl_Telefax" Text="Telefax" runat="server"></asp:Label>
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:TextBox ID="txtTelefax" runat="server"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="firstLeft active">
                                                                                <asp:Label ID="lbl_Email" Text="Email" runat="server"></asp:Label>
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:TextBox ID="txtEmail" runat="server" Width="200px"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="3" nowrap="nowrap">
                                                                                <asp:RadioButtonList ID="rblPersonType" width="50%" RepeatDirection="Horizontal" runat="server" style="padding-bottom: 3px; border-width: 0px">
                                                                                    <asp:ListItem Value="Businessowner">Businessowner</asp:ListItem>
                                                                                    <asp:ListItem Value="Adminperson">Admin Person</asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:LinkButton ID="lbEintragen" runat="server" Text="eintragen"> </asp:LinkButton>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="firstLeft active">
                                                                  <u>Businessowner</u>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:GridView ID="gvBusinessOwner" Width="100%" runat="server" BackColor="White"
                                                                        AllowPaging="True" AutoGenerateColumns="False" BorderColor="Black"
                                                                        BorderStyle="Solid">
                                                                        <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                                        <RowStyle CssClass="ItemStyle"></RowStyle>
                                                                        <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                                        <Columns>
                                                                            <asp:BoundField DataField="ID" Visible="false" />
                                                                            <asp:TemplateField HeaderText="Löschen" ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton Visible="true" ID="lbEntfernen" CommandName="entfernen" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.ID") %>'
                                                                                        runat="server"><img src="../../Images/del.png" border="0"> </asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                                                                            <asp:BoundField DataField="Vorname" HeaderText="Vorname" SortExpression="Vorname" />
                                                                            <asp:BoundField DataField="Email" HeaderText="E-Mail" SortExpression="Email" />
                                                                            <asp:BoundField DataField="Telefon" HeaderText="Telefon" SortExpression="Telefon" />
                                                                            <asp:BoundField DataField="Telefax" HeaderText="Telefax" SortExpression="Telefax" />
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                              <tr>
                                                                <td class="firstLeft active">
                                                                  <u>Administrationsberechtigte Person</u>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:GridView ID="gvAdminPerson" Width="100%" runat="server" BackColor="White"
                                                                        AllowPaging="True" AutoGenerateColumns="False" BorderColor="Black"
                                                                        BorderStyle="Solid">
                                                                        <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                                        <RowStyle CssClass="ItemStyle"></RowStyle>
                                                                        <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                                        <Columns>
                                                                            <asp:BoundField DataField="ID" Visible="false" />
                                                                            <asp:TemplateField HeaderText="Löschen" ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton Visible="true" ID="lbEntfernen" CommandName="entfernen" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.ID") %>'
                                                                                        runat="server"><img src="../../Images/del.png" border="0"> </asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                                                                            <asp:BoundField DataField="Vorname" HeaderText="Vorname" SortExpression="Vorname" />
                                                                            <asp:BoundField DataField="Email" HeaderText="E-Mail" SortExpression="Email" />
                                                                            <asp:BoundField DataField="Telefon" HeaderText="Telefon" SortExpression="Telefon" />
                                                                            <asp:BoundField DataField="Telefax" HeaderText="Telefax" SortExpression="Telefax" />
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td colspan="2">
                                                        <div class="formqueryHeader">
                                                            <span>Organisation</span>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Organisationsanzeige ein:
                                                    </td>
                                                    <td class="active" style="width: 200px">
                                                        <span>
                                                            <asp:CheckBox ID="chkShowOrganization" runat="server"></asp:CheckBox></span>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Organisations-Admin auf
                                                        <br />
                                                        Kundengruppe beschränken:
                                                    </td>
                                                    <td class="active" style="width: 200px">
                                                        <span>
                                                            <asp:CheckBox ID="cbxOrgAdminRestrictToCustomerGroup" runat="server"></asp:CheckBox></span>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Distriktzuordnung:
                                                    </td>
                                                    <td class="active" style="width: 200px">
                                                        <span>
                                                            <asp:CheckBox ID="chkShowDistrikte" runat="server"></asp:CheckBox></span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </ajaxToolkit:TabPanel>
                                    <ajaxToolkit:TabPanel runat="server" ID="IP_Style" HeaderText="IP-Adressen/Style">
                                        <ContentTemplate>
                                            <asp:UpdatePanel ID="IPUpdate" runat="server">
                                                <ContentTemplate>
                                                <asp:Label ID="LogoError" runat="server" CssClass="TextError" EnableViewState="False" Visible="False"></asp:Label>
                                                <asp:Label ID="LogoDebug" runat="server" CssClass="TextExtraLarge" EnableViewState="False" Visible="False"></asp:Label>
                                                    <table id="tblIpAddresses" style="border-color: #FFFFFF; font-size: 10px;" cellspacing="0"
                                                        cellpadding="0" width="100%" border="0">
                                                        <tr class="formquery">
                                                            <td colspan="5">
                                                                <div class="formqueryHeader">
                                                                    <span>Beschränkungen auf IP-Adressen</span>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active">
                                                                Adress-Kontrolle:
                                                            </td>
                                                            <td class="active" colspan="4">
                                                                <span>
                                                                    <asp:CheckBox ID="chkIpRestriction" runat="server"></asp:CheckBox></span>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active">
                                                                Standard-User-Name:
                                                            </td>
                                                            <td class="active" colspan="4">
                                                                <asp:TextBox ID="txtIpStandardUser" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active">
                                                                Liste gültiger Adressen:
                                                            </td>
                                                            <td class="active" colspan="4">
                                                                <div style="padding-right: 400px;">
                                                                    <asp:Repeater ID="Repeater1" runat="server">
                                                                        <HeaderTemplate>
                                                                            <div style="background-color: #dfdfdf; height: 10px; width: 100%">
                                                                                &nbsp;</div>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table class="" border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                <tr>
                                                                                    <td style="padding: 2px">
                                                                                        <img alt="" src="/Portal/Images/arrow.gif" border="0" />
                                                                                    </td>
                                                                                    <td style="padding: 2px">
                                                                                        <b>
                                                                                            <%#DataBinder.Eval(Container.DataItem, "IpAddress")%></b></b>
                                                                                    </td>
                                                                                    <td style="padding: 2px">
                                                                                    </td>
                                                                                    <td style="padding: 2px">
                                                                                        <asp:LinkButton ID="btnDel" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "IpAddress")%>'
                                                                                            runat="server">Löschen</asp:LinkButton>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active">
                                                                IP-Adresse
                                                            </td>
                                                            <td class="active" colspan="4">
                                                                <asp:TextBox ID="txtIpAddress" runat="server" CssClass="InputTextbox"></asp:TextBox><asp:LinkButton
                                                                    ID="btnNewIpAddress" runat="server" Width="58px">Hinzufügen</asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td colspan="5">
                                                                <div class="formqueryHeader">
                                                                    <span>Style</span>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active">
                                                                Pfad zum Logo:
                                                            </td>
                                                            <td class="active">
                                                                <asp:TextBox ID="txtLogoPath" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <telerik:RadAsyncUpload runat="server" ID="RadAsyncUpload1" EnableInlineProgress="false"
                                                                    EnableViewState="false" ViewStateMode="Disabled" AllowedFileExtensions="jpg,jpeg,png,gif"
                                                                    MaxFileSize="524288" MaxFileInputsCount="1" OnFileUploaded="RadAsyncUpload1_FileUploaded"
                                                                    OnClientFileUploaded="onFileUploaded" OnClientValidationFailed="validationFailed"
                                                                    InputSize="2" Localization-Cancel="Abbrechen" Localization-Remove="Löschen" Localization-Select="Hochladen"
                                                                    Height="20px" Width="180px" Skin="Simple" ToolTip="Das Logo sollte eine Größe von 220 x 70 Pixel haben." />
                                                            </td>
                                                            <td style="width: 200px">
                                                                <asp:Image alt="img" ID="imgLogoThumb" runat="server" Width="180px" Height="60px"
                                                                    EnableViewState="False" Style="display: none" />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="btnDelKundenLogo" runat="server" AlternateText="Kundenlogo löschen" 
                                                                    ImageUrl="../Images/del.png" Height="16px" Width="16px" ToolTip="Kundenlogo löschen"/>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active">
                                                                Pfad zum Logo rechts:
                                                            </td>
                                                            <td class="active">
                                                                <asp:TextBox ID="txtLogoPath2" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <div id="Div_Buchungskreis_DDL">
                                                                    <telerik:RadComboBox ID="imageDDL" runat="server" OnClientSelectedIndexChanged="imageDDLChanged">
                                                                        <%-- Items werden im code behind gefüllt --%>
                                                                    </telerik:RadComboBox>
                                                                </div>
                                                                <div id="Div_Buchungskreis_Upload" style="display: none">
                                                                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncUpload2" EnableInlineProgress="false"
                                                                        EnableViewState="false" ViewStateMode="Disabled" AllowedFileExtensions="jpg,jpeg,png,gif"
                                                                        MaxFileSize="524288" MaxFileInputsCount="1" OnFileUploaded="RadAsyncUpload2_FileUploaded"
                                                                        OnClientFileUploaded="onFileUploaded" OnClientValidationFailed="validationFailed"
                                                                        InputSize="2" Localization-Cancel="Abbrechen" Localization-Remove="Löschen" Localization-Select="Hochladen"
                                                                        Height="20px" Width="180px" Skin="Simple" ToolTip="Das Logo sollte eine Größe von 220 x 70 Pixel haben." />
                                                                </div>
                                                            </td>
                                                            <td style="width: 200px">
                                                                <asp:Image alt="img" ID="Image_Buchungskreis_aktiv" runat="server" Style="display: none"
                                                                    EnableViewState="False" Width="180px" Height="60px" />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="btnDelBukrsLogo" runat="server" AlternateText="Mandantenlogo löschen" 
                                                                    ImageUrl="../Images/del.png" Height="16px" Width="16px" ToolTip="Mandantenlogo löschen"/>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td id="titleHeaderBereich" class="firstLeft active">
                                                                Pfad zum Hintergrundbild des Headers:
                                                            </td>
                                                            <td class="active">
                                                                <asp:TextBox ID="txtHeaderBackgroundPath" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <div id="Div_HeaderBackgroundPath_DDL">
                                                                    <telerik:RadComboBox ID="imageHeaderDDL" runat="server" OnClientSelectedIndexChanged="imageHeaderDDLChanged">
                                                                        <%-- Items werden im code behind gefüllt --%>
                                                                    </telerik:RadComboBox>
                                                                </div>
                                                                <div id="Div_HeaderBackgroundPath_Upload" style="display: none">
                                                                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncUpload3" EnableInlineProgress="false"
                                                                        EnableViewState="false" ViewStateMode="Disabled" AllowedFileExtensions="jpg,jpeg,png,gif"
                                                                        MaxFileSize="524288" MaxFileInputsCount="1" OnFileUploaded="RadAsyncUpload3_FileUploaded"
                                                                        OnClientFileUploaded="onFileUploaded" OnClientValidationFailed="validationFailed"
                                                                        InputSize="2" Localization-Cancel="Abbrechen" Localization-Remove="Löschen" Localization-Select="Hochladen"
                                                                        Height="20px" Width="180px" Skin="Simple" ToolTip="Das Logo sollte eine Größe von 947 x 125 Pixel haben.">
                                                                    </telerik:RadAsyncUpload>
                                                                </div>
                                                            </td>
                                                            <td style="width: 200px">
                                                                <div id="Div_Image_HeaderBackgroundPath_aktiv">
                                                                    <asp:Image alt="img" ID="Image_HeaderBackgroundPath_aktiv" runat="server" Style="display: none"
                                                                        EnableViewState="False" Width="180px" Height="30px" />
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="btnDelHeaderLogo" runat="server" AlternateText="Headerlogo löschen" 
                                                                    ImageUrl="../Images/del.png" Height="16px" Width="16px" ToolTip="Headerlogo löschen"/>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td colspan="5">
                                                                <div id="divHeaderImageZoom" style="display: none">
                                                                    <img id="imgHeaderImageZoom" alt="img" style="max-width: 886px" />
                                                                </div>
                                                                <div>
                                                                    <input id="ihHeaderImageZoomStatus" type="hidden" value="0"/>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active">
                                                                Pfad zu den Stylesheets:
                                                            </td>
                                                            <td class=" active" colspan="4">
                                                                <asp:TextBox ID="txtCssPath" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="formquery">
                                                            <td class="firstLeft active">
                                                                Pfad zum Handbuch:
                                                            </td>
                                                            <td class="active" colspan="4">
                                                                <asp:TextBox ID="txtDocuPath" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </ContentTemplate>
                                    </ajaxToolkit:TabPanel>
                                    <ajaxToolkit:TabPanel runat="server" ID="Apps" HeaderText="Anwendungen">
                                        <ContentTemplate>
                                            <table id="tblApp" cellspacing="0" cellpadding="0" width="100%" style="border-color: #FFFFFF;
                                                font-size: 10px; padding-bottom: 0" border="0">
                                                <tr class="formquery">
                                                    <td class="active" colspan="2">
                                                        <asp:Panel runat="server" DefaultButton="lbtFilterUnassignedApps">
                                                            <table style="border: none; padding-bottom: 0">
                                                                <tr>
                                                                    <td style="width: 70%">
                                                                        nicht zugewiesen
                                                                    </td>
                                                                    <td style="width: 20%">
                                                                        <asp:TextBox runat="server" ID="txtFilterUnassignedApps" />
                                                                    </td>
                                                                    <td style="width: 10%">
                                                                        <asp:LinkButton class="Tablebutton" ID="lbtFilterUnassignedApps" runat="server" Text="Suchen&amp;nbsp;&amp;#187; "
                                                                            CssClass="Tablebutton" Height="16px" Width="78px"></asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                        <div style="width: 100%; max-width: 892px; margin-top: 5px">        
                                                            <telerik:RadGrid ID="rgAppUnAssigned" runat="server" AllowSorting="True" 
                                                                AutoGenerateColumns="False" GridLines="None" Culture="de-DE"   
                                                                OnNeedDataSource="rgAppUnAssigned_NeedDataSource" >
                                                                <ClientSettings>
                                                                    <Scrolling ScrollHeight="265px" AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="1" />
                                                                </ClientSettings>
                                                                <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Auto" AllowPaging="false" >
                                                                    <SortExpressions>
                                                                        <telerik:GridSortExpression FieldName="AppFriendlyName" SortOrder="Ascending" />
                                                                    </SortExpressions>
                                                                    <HeaderStyle ForeColor="White" />
                                                                    <Columns>
                                                                        <telerik:GridBoundColumn DataField="AppID" SortExpression="AppID" Visible="false" />
                                                                        <telerik:GridTemplateColumn Groupable="false" UniqueName="Auswahl" >
                                                                            <HeaderStyle Width="25px" />
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox runat="server" ID="chkSelect" style="width: 16px; height: 16px" />
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridBoundColumn DataField="AppFriendlyName" SortExpression="AppFriendlyName" HeaderText="Name" >
                                                                            <HeaderStyle Width="150px" />
                                                                            <ItemStyle Wrap="false" />
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="AppURL" SortExpression="AppURL" HeaderText="URL" >
                                                                            <HeaderStyle Width="200px" />
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="AppTechType" SortExpression="AppTechType" HeaderText="Technologie" >
                                                                            <HeaderStyle Width="75px" />
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="AppDescription" SortExpression="AppDescription" HeaderText="Beschreibung" >
                                                                            <HeaderStyle Width="200px" />
                                                                            <ItemStyle Wrap="false" />
                                                                        </telerik:GridBoundColumn>
                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                        </div>
                                                    </td> 
                                                </tr>
                                                <tr class="formquery">
                                                    <td align="right">
                                                        <asp:ImageButton ID="btnAssign" runat="server" ImageUrl="../../Images/Pfeil_abwaerts_01.jpg"
                                                            ToolTip="Zuweisen" Height="37px" Width="37px" style="margin-right: 10px" />
                                                    </td>
                                                    <td align="left">
                                                        <asp:ImageButton ID="btnUnAssign" runat="server" ImageUrl="../../Images/Pfeil_aufwaerts_01.jpg"
                                                            ToolTip="Entfernen" Height="37px" Width="37px" style="margin-left: 10px" /></span>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="active" colspan="2">
                                                        zugewiesen
                                                        <div style="width: 100%; max-width: 892px; margin-top: 5px">       
                                                            <telerik:RadGrid ID="rgAppAssigned" runat="server" AllowSorting="True" 
                                                                AutoGenerateColumns="False" GridLines="None" Culture="de-DE" >
                                                                <ClientSettings>
                                                                    <Scrolling ScrollHeight="265px" AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="1" />
                                                                </ClientSettings>
                                                                <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Auto" AllowPaging="false" >
                                                                    <SortExpressions>
                                                                        <telerik:GridSortExpression FieldName="AppFriendlyName" SortOrder="Ascending" />
                                                                    </SortExpressions>
                                                                    <HeaderStyle ForeColor="White" />
                                                                    <Columns>
                                                                        <telerik:GridBoundColumn DataField="AppID" SortExpression="AppID" Visible="false" />
                                                                        <telerik:GridTemplateColumn Groupable="false" UniqueName="Auswahl" >
                                                                            <HeaderStyle Width="25px" />
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox runat="server" ID="chkSelect" style="width: 16px; height: 16px" />
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridBoundColumn DataField="AppFriendlyName" SortExpression="AppFriendlyName" HeaderText="Name" >
                                                                            <HeaderStyle Width="150px" />
                                                                            <ItemStyle Wrap="false" />
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="AppURL" SortExpression="AppURL" HeaderText="URL" >
                                                                            <HeaderStyle Width="200px" />
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="AppTechType" SortExpression="AppTechType" HeaderText="Techn." >
                                                                            <HeaderStyle Width="60px" />
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="AppDescription" SortExpression="AppDescription" HeaderText="Beschreibung" >
                                                                            <HeaderStyle Width="60px" />
                                                                            <ItemStyle Wrap="false" />
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridTemplateColumn Groupable="false" UniqueName="MvcDefaultFavorite" SortExpression="AppIsMvcDefaultFavorite" HeaderText="MVC Favorit">
                                                                            <HeaderStyle Width="60px" />
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="CheckBox1" runat="server" EnableViewState="True" AutoPostBack="True" style="width: 16px; height: 16px" 
                                                                                        Checked='<%# DataBinder.Eval(Container, "DataItem.AppIsMvcDefaultFavorite") %>' 
                                                                                        ToolTip='<%# DataBinder.Eval(Container, "DataItem.AppID").ToString() %>' />
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                            <table id="tabAppSettings" cellspacing="0" cellpadding="0" width="100%" style="border-color: #FFFFFF;
                                                font-size: 10px;" border="0">
                                                <tr class="formquery">
                                                    <td class="firstLeft active" width="35%">
                                                        Kundenspezifische Anwendungseinstellungen
                                                        <p style="margin-bottom: 0px">
                                                            <asp:ListBox ID="lstCustomerSettings" runat="server" CssClass="InputTextbox" AutoPostBack="True"
                                                                Width="100%" Height="23px" Rows="1"></asp:ListBox>
                                                        </p>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Setting:&nbsp
                                                        <asp:TextBox Width="120px" ID="txtKey" runat="server" />
                                                        &nbsp Value:&nbsp
                                                        <asp:TextBox ID="txtValue" runat="server" Width="250px" />
                                                        &nbspDescription:&nbsp
                                                        <asp:TextBox Width="200px" ID="txtDescript" runat="server" />
                                                        &nbsp
                                                        <asp:LinkButton ToolTip="Hinzufügen oder Ändern" ID="lbOk" CssClass="Tablebutton"
                                                            Height="16px" Width="78px" runat="server" Text="Hinzufügen&amp;nbsp;&amp;#187" />
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        <asp:GridView ID="gvCustomerSettings" runat="server" EnableModelValidation="True"
                                                            AutoGenerateColumns="False" CssClass="GridView">
                                                            <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                                            <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                            <RowStyle CssClass="ItemStyle" />
                                                            <PagerSettings Visible="False" />
                                                            <EmptyDataRowStyle BackColor="#DFDFDF" />
                                                            <Columns>
                                                                <asp:CommandField ShowEditButton="True" ButtonType="Image" EditImageUrl="../../Images/Tool.jpg">
                                                                    <ControlStyle Width="16px" Height="14px" />
                                                                    <HeaderStyle Width="25px" />
                                                                </asp:CommandField>
                                                                <asp:BoundField DataField="SettingsName" HeaderText="SettingsName" SortExpression="SettingsName"
                                                                    HtmlEncode="False">
                                                                    <HeaderStyle Width="120px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Value" HeaderText="Value" InsertVisible="False" SortExpression="Value"
                                                                    HtmlEncode="False">
                                                                    <ItemStyle Wrap="True" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description"
                                                                    HtmlEncode="False">
                                                                    <ItemStyle Wrap="True" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CustomerID" HeaderText="CustomerID" Visible="False" SortExpression="CustomerID" />
                                                                <asp:BoundField DataField="AppID" HeaderText="AppID" Visible="False" SortExpression="AppID" />
                                                                <asp:CommandField ShowDeleteButton="True" ButtonType="Image" DeleteImageUrl="../../Images/Papierkorb_01.gif">
                                                                    <ControlStyle Width="12px" Height="14px" />
                                                                    <HeaderStyle Width="25px" />
                                                                </asp:CommandField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </ajaxToolkit:TabPanel>
                                    <ajaxToolkit:TabPanel runat="server" ID="TPArchive" HeaderText="Archive">
                                        <ContentTemplate>
                                            <table id="tblArchiv" cellspacing="0" cellpadding="0" width="100%" style="border-color: #FFFFFF;
                                                font-size: 10px;" border="0">
                                                <tr class="formquery">
                                                    <td class="firstLeft active" width="35%">
                                                        nicht zugewiesen
                                                        <p>
                                                            <asp:ListBox ID="lstArchivUnAssigned" runat="server" SelectionMode="Multiple" CssClass="InputTextbox"
                                                                Width="300px" Height="150px"></asp:ListBox>
                                                        </p>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="btnAssignArchiv" Style="margin-left: 40px" runat="server" ImageUrl="../../Images/Pfeil_vor_01.jpg"
                                                            ToolTip="Zuweisen" Height="37px" Width="37px" />
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="btnUnAssignArchiv" runat="server" ImageUrl="../../Images/Pfeil_zurueck_01.jpg"
                                                            ToolTip="Entfernen" Height="37px" Width="37px" /></span>
                                                    </td>
                                                    <td class="active" width="35%">
                                                        zugewiesen
                                                        <p>
                                                            <asp:ListBox ID="lstArchivAssigned" runat="server" SelectionMode="Multiple" CssClass="InputTextbox"
                                                                Width="300px" Height="150"></asp:ListBox>
                                                        </p>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </ajaxToolkit:TabPanel>
                                    <ajaxToolkit:TabPanel runat="server" ID="TPSilverDAT" HeaderText="DAT">
                                        <ContentTemplate>
                                            <table id="Table1" cellspacing="0" cellpadding="0" width="100%" style="border-color: #FFFFFF;
                                                font-size: 10px;" border="0">
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lblSDCustomerNumber" runat="server" Text="Kundennummer:" AssociatedControlID="txtSDCustomerNumber" />
                                                    </td>
                                                    <td style="width: 100%;">
                                                        <asp:TextBox ID="txtSDCustomerNumber" runat="server" CssClass="InputTextbox" />
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lblSDUserName" runat="server" Text="Zugangsname:" AssociatedControlID="txtSDUserName" />
                                                    </td>
                                                    <td style="width: 100%;">
                                                        <asp:TextBox ID="txtSDUserName" runat="server" CssClass="InputTextbox" />
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lblSDPassword" runat="server" Text="Passwort:" AssociatedControlID="txtSDPassword" />
                                                    </td>
                                                    <td style="width: 100%;">
                                                        <asp:TextBox ID="txtSDPassword" runat="server" CssClass="InputTextbox" />
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lblSDLoginName" runat="server" Text="LoginName Bankenlinie:" AssociatedControlID="txtSDLoginName" />
                                                    </td>
                                                    <td style="width: 100%;">
                                                        <asp:TextBox ID="txtSDLoginName" runat="server" CssClass="InputTextbox" />
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lblSDSignatur" runat="server" Text="Signatur:" AssociatedControlID="txtSDSignatur" />
                                                    </td>
                                                    <td style="width: 100%;">
                                                        <asp:TextBox ID="txtSDSignatur" runat="server" CssClass="InputTextbox" />
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lblSDSignatur2" runat="server" Text="Signatur2:" AssociatedControlID="txtSDSignatur2" />
                                                    </td>
                                                    <td style="width: 100%;">
                                                        <asp:TextBox ID="txtSDSignatur2" runat="server" CssClass="InputTextbox" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </ajaxToolkit:TabPanel>

                                    <ajaxToolkit:TabPanel runat="server" ID="Settings" HeaderText="Settings">
                                        <ContentTemplate>
                                            <table id="Table2" cellspacing="0" cellpadding="0" width="100%" style="border-color: #FFFFFF;
                                                font-size: 10px;" border="0">
                                                <thead>
                                                <tr class="formquery">
                                                    
                                                    <td style="width: 100%;" colspan="2">
                                                        Für diesen Kunden verfügbare Rechte (aktivierte Rechte sind markiert)
                                                    </td>

                                                </tr>
                                                </thead>

                                                <tr class="formquery">
                                                    
                                                    <td style="width: 100%;" colspan="2">
                                                        <!-- Rechte_anpassen --> 
                                                        <telerik:RadGrid ID="rgRights" runat="server" AllowSorting="False" 
                                                                AutoGenerateColumns="False" GridLines="None" Culture="de-DE"
                                                                OnNeedDataSource="rgRights_NeedDataSource">
                                                              
                                                                <ClientSettings>
                                                                    <Scrolling ScrollHeight="265px" AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="1" />
                                                                </ClientSettings>

                                                                <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Auto" AllowPaging="false" >
                                                                    <SortExpressions>
                                                                        <telerik:GridSortExpression FieldName="CategoryID" SortOrder="Ascending" />
                                                                    </SortExpressions>
                                                                    <HeaderStyle ForeColor="White" />
                                                                    
                                                                    <Columns>
                                                                        
                                                                        <telerik:GridTemplateColumn Groupable="false" UniqueName="Auswahl1" HeaderText="Aktivieren" >
                                                                            <HeaderStyle Width="35px" />
                                                                            <ItemStyle></ItemStyle>
                                                                            <ItemTemplate>
                                                                                  <asp:CheckBox ID="cbxSetRight" runat="server" EnableViewState="True" AutoPostBack="False" style="width: 16px; height: 16px" 
                                                                                                            Checked='<%# Eval("HasSettings") %>' />
                                                                             </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                    
                                                                        <telerik:GridBoundColumn DataField="CategoryID" SortExpression="CategoryID" HeaderText="Recht / Setting" UniqueName="CategoryID" >
                                                                             <HeaderStyle Width="150px" />
                                                                            <ItemStyle></ItemStyle>
                                                                            
                                                                        </telerik:GridBoundColumn>

                                                                        <telerik:GridBoundColumn DataField="Description" HeaderText="Beschreibung" UniqueName="Beschreibung" >
                                                                             <HeaderStyle Width="250px" />
                                                                            <ItemStyle></ItemStyle>
                                                                        </telerik:GridBoundColumn>
                                                                       
                                                                    </Columns>

                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                        
                                                    </td>

                                                </tr>
                                                
                                            </table>
                                        </ContentTemplate>
                                    </ajaxToolkit:TabPanel>
                                    
                                </ajaxToolkit:TabContainer>

                                <div id="dataFooter">
                                    &nbsp;&nbsp;
                                    <asp:LinkButton class="Tablebutton" ID="lbtnDelete" runat="server" Text="Löschen&amp;nbsp;&amp;#187; "
                                        CssClass="Tablebutton" Height="16px" Width="78px" Visible="False"></asp:LinkButton>
                                    &nbsp;
                                    <asp:LinkButton class="Tablebutton" ID="lbtnSave" runat="server" Text="Speichern&amp;nbsp;&amp;#187; "
                                        CssClass="Tablebutton" Height="16px" Width="78px"></asp:LinkButton>
                                    &nbsp;
                                    <asp:LinkButton class="Tablebutton" ID="lbtnCancel" runat="server" Text="Verwerfen&amp;nbsp;&amp;#187; "
                                        CssClass="Tablebutton" Height="16px" Width="78px" ></asp:LinkButton>
                                    &nbsp;
                                    <asp:LinkButton class="Tablebutton" ID="lbtnOpenCopyOptions" runat="server" Text="Kopieren&amp;nbsp;&amp;#187; "
                                        CssClass="Tablebutton" Height="16px" Width="78px"></asp:LinkButton>
                                    <asp:Button ID="btnFake" runat="server" Text="" Style="display: none" />
                                    <ajaxToolkit:ModalPopupExtender ID="copyOptions" runat="server" TargetControlID="btnFake"
                                        PopupControlID="mb" BackgroundCssClass="modalBackground" DropShadow="true" CancelControlID="lbtnCancelOptions"
                                        X="380" Y="250">
                                    </ajaxToolkit:ModalPopupExtender>
                                    <asp:Panel ID="mb" runat="server" Width="385px" Height="150px" BackColor="White"
                                        BorderColor="#999999" BorderStyle="Solid" BorderWidth="1" Style="display: none">
                                        <div style="padding-left: 10px; padding-top: 15px;">
                                            <h2>
                                                Optionen</h2>
                                        </div>
                                        <div style="padding-left: 10px; padding-top: 15px;">
                                            <asp:CheckBox runat="server" ID="keepApplications" Text="Anwendungen beibehalten" />
                                        </div>
                                        <div style="padding-left: 10px; padding-top: 15px;">
                                            <asp:LinkButton class="Tablebutton" ID="lbtnCopy" runat="server" Text="Kopieren&amp;nbsp;&amp;#187; "
                                                CssClass="Tablebutton" Height="16px" Width="78px" Visible="true"></asp:LinkButton>
                                            &nbsp;
                                            <asp:LinkButton class="Tablebutton" ID="lbtnCancelOptions" runat="server" Text="Abbrechen"
                                                CssClass="Tablebutton" Height="16px" Width="78px" Visible="true"></asp:LinkButton>
                                        </div>
                                    </asp:Panel>
                                    <asp:Button ID="btnFake2" runat="server" Text="" Style="display: none" />
                                    <ajaxToolkit:ModalPopupExtender ID="confirmWindow" runat="server" TargetControlID="btnFake2"
                                        PopupControlID="mb2" BackgroundCssClass="modalBackground" DropShadow="true" X="550" Y="250">
                                    </ajaxToolkit:ModalPopupExtender>
                                    <asp:Panel ID="mb2" runat="server" Width="385px" Height="200px" BackColor="White"
                                        BorderColor="#999999" BorderStyle="Solid" BorderWidth="1" Style="display: none">
                                        <div style="padding-left: 10px; padding-top: 5px;">
                                            <asp:PlaceHolder ID="plhConfirm" runat="server"></asp:PlaceHolder>
                                        </div>
                                        <div style="padding-left: 10px; padding-top: 15px;">
                                            <asp:LinkButton class="Tablebutton" ID="lbtnConfirm" runat="server" Text="Bestätigen&amp;nbsp;&amp;#187; "
                                                CssClass="Tablebutton" Height="16px" Width="78px" Visible="true"></asp:LinkButton>
                                            &nbsp;
                                            <asp:LinkButton class="Tablebutton" ID="lbtnCancelConfirm" runat="server" Text="Ändern"
                                                CssClass="Tablebutton" Height="16px" Width="78px" Visible="true"></asp:LinkButton>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        function CheckCollapseExpandStatus() {
            if ($("#<%= ihExpandstatusSearchFilterArea.ClientID %>").attr("value") == "1") {
                $("#arealnkSuche").hide();
                $("#lnkSuche").hide();
                $("#arealnkSuchergebnis").hide();
                $("#lnkSuchergebnis").hide();
                $("#<%= Result.ClientID %>").hide();
                $("#<%= Input.ClientID %>").hide();
                $("#<%= DivSearch1.ClientID %>").show();
            }
            else if ($("#<%= ihExpandstatusSearchResultArea.ClientID %>").attr("value") == "1") {
                $("#arealnkSuchergebnis").hide();
                $("#lnkSuchergebnis").hide();
                $("#arealnkSuche").show();
                $("#lnkSuche").show();
                $("#<%= DivSearch1.ClientID %>").hide();
                $("#<%= Input.ClientID %>").hide();
                $("#<%= Result.ClientID %>").show();
            }
            else {
                $("#arealnkSuche").show();
                $("#lnkSuche").show();
                $("#arealnkSuchergebnis").show();
                $("#lnkSuchergebnis").show();
                $("#<%= DivSearch1.ClientID %>").hide();
                $("#<%= Result.ClientID %>").hide();
                $("#<%= Input.ClientID %>").show();
            }
        }

        function showSearchFilterArea() {
            $("#<%= ihExpandstatusSearchFilterArea.ClientID %>").attr("value", "1");
            $("#<%= ihExpandstatusSearchResultArea.ClientID %>").attr("value", "0");
            $("#<%= ihExpandstatusInputArea.ClientID %>").attr("value", "0");
            CheckCollapseExpandStatus();
        }

        function showSearchResultArea() {
            $("#<%= ihExpandstatusSearchFilterArea.ClientID %>").attr("value", "0");
            $("#<%= ihExpandstatusSearchResultArea.ClientID %>").attr("value", "1");
            $("#<%= ihExpandstatusInputArea.ClientID %>").attr("value", "0");
            CheckCollapseExpandStatus();
        }

        $(function () {
            CheckCollapseExpandStatus();

            $("#<% = Image_HeaderBackgroundPath_aktiv.ClientId %>").click(function () {
                if ($("#ihHeaderImageZoomStatus").val() == "0") {
                    $("#imgHeaderImageZoom").attr("src", $(this).attr("src"));
                    $("#ihHeaderImageZoomStatus").val("1");
                    $("#divHeaderImageZoom").show();
                }
                else {
                    $("#ihHeaderImageZoomStatus").val("0");
                    $("#divHeaderImageZoom").hide();
                }
            });

            $("#imgHeaderImageZoom").click(function () {
                $("#ihHeaderImageZoomStatus").val("0");
                $("#divHeaderImageZoom").hide();
            });
        }); 
        
    </script>
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript">
            function imageDDLChanged() {
                var imageDDLWert = $find("<%= imageDDL.ClientID %>").get_value();
                if ((imageDDLWert != "Upload") && (imageDDLWert != "Choose")) {
                    $("#<%= txtLogoPath2.ClientID %>").val(imageDDLWert);
                    $("#<%= Image_Buchungskreis_aktiv.ClientID %>").attr("src", imageDDLWert);
                    $("#<%= Image_Buchungskreis_aktiv.ClientID %>").show();
                }
                else {
                    $("#<%= Image_Buchungskreis_aktiv.ClientID %>").hide();
                }
                if (imageDDLWert == "Upload") {
                    $("#Div_Buchungskreis_Upload").show();
                }
                else {
                    $("#Div_Buchungskreis_Upload").hide();
                }
            }

            function imageHeaderDDLChanged() {
                var imageHeaderDDLWert = $find("<%= imageHeaderDDL.ClientID %>").get_value();
                if ((imageHeaderDDLWert != "Upload") && (imageHeaderDDLWert != "Choose")) {
                    $("#<%= txtHeaderBackgroundPath.ClientID %>").val(imageHeaderDDLWert);
                    $("#<%= Image_HeaderBackgroundPath_aktiv.ClientID %>").attr("src", imageHeaderDDLWert);
                    $("#<%= Image_HeaderBackgroundPath_aktiv.ClientID %>").show();
                }
                else {
                    $("#<%= Image_HeaderBackgroundPath_aktiv.ClientID %>").hide();
                }
                if (imageHeaderDDLWert == "Upload") {
                    $("#Div_HeaderBackgroundPath_Upload").show();
                }
                else {
                    $("#Div_HeaderBackgroundPath_Upload").hide();
                }
            }

        </script>
    </telerik:RadScriptBlock>

    <script type="text/javascript">

            function EnableDisableSelectionUrlTextboxInner() {
                if ($(".selection-type-dropdown").val() == "Url") {
                    $(".selection-url-textbox").removeAttr("readonly");
                }
                else {
                    $(".selection-url-textbox").attr("readonly", "readonly");
                }
            }

            function EnableDisableSelectionUrlTextbox() {
                EnableDisableSelectionUrlTextboxInner();

                    $(".selection-type-dropdown").off("change").on("change",
                    function () {

                        if ($(this).val() == "Favorites")
                            $(".selection-url-textbox").val("");

                        if ($(this).val() == "Dashboard")
                            $(".selection-url-textbox").val("Common/Dashboard/Index");

                        EnableDisableSelectionUrlTextboxInner();
                    }
                );
            }

            function PrepareCustomerSearchTextbox() {
                var searchCustomerTextbox = $("#<%= txtFilterCustomerName.ClientID %>");
                if (typeof (searchCustomerTextbox) == "undefined" || searchCustomerTextbox == null)
                    return;

                searchCustomerTextbox.focus();
                $("#<%= txtFilterCustomerName.ClientID %>").setCaretPos(2);
            }

            function PrepareCustomerSearchResultsAutomaticLoad() {
                var redirect = <%= (DirectCast(Session("objUser"), User).Reference2.NotNullOrEmpty() = "AutoAdminRedirect").ToString().ToLower() %>;
                if (!redirect) 
                    return;

                if ($("#<%= Result.ClientID %>").css("display") == "none")
                    return;

                if ($(".customer-grid tr").length != 2)
                    return;

                 $("#<%= Result.ClientID %>").html("<div class='auto-redirect-link'>Sie werden automatisch weitergeleitet ...</div>");

                __doPostBack('ctl00$ContentPlaceHolder1$dgSearchResult', 'Edit$0')
            }

            function InitControls() {

                PrepareCustomerSearchResultsAutomaticLoad();

                EnableDisableSelectionUrlTextbox();

                PrepareCustomerSearchTextbox();
            }

            $(function () {
                InitControls();
            });         

    </script>
</asp:Content>
