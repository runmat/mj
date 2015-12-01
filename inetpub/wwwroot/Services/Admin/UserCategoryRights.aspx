<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UserCategoryRights.aspx.vb" Inherits="Admin.UserCategoryRights"
    MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .editable
        {
            background-color: ivory;
            padding: 6px 1px 1px 1px;
        }
    </style>
    <div>
        <div id="site">
            <div id="content">
                <div id="innerContent">
                    <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                    <asp:Label ID="lblMessage" runat="server" CssClass="TextExtraLarge" EnableViewState="False"></asp:Label>
                    <div id="navigationSubmenu">
                        &nbsp;
                    </div>
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                        </div>
                        <asp:Panel ID="DivSearch1" runat="server" DefaultButton="btnEmpty">
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
                                                <asp:DropDownList ID="ddlFilterCustomer" runat="server" AutoPostBack="True" Font-Names="Verdana,sans-serif"
                                                    Height="20px" Visible="False" Width="260px">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblCustomer" runat="server" Width="160px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery" id="trSelectOrganization" runat="server">
                                            <td class="firstLeft active">
                                                Organisation:
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:DropDownList ID="ddlFilterOrganization" runat="server" AutoPostBack="True" Font-Names="Verdana,sans-serif"
                                                    Height="20px" Width="260px">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblOrganizationName" runat="server" Visible="False" Width="160px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Gruppe:
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:DropDownList ID="ddlFilterGroup" runat="server" Width="260px" Font-Names="Verdana,sans-serif">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblGroup" runat="server" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Benutzername:
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:TextBox ID="txtFilterUserName" runat="server" CssClass="InputTextbox" Width="257px">*</asp:TextBox>
                                                <asp:ImageButton ID="ImageButton1" runat="server" Height="16px" ImageUrl="../images/empty.gif"
                                                    Width="1px" />
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Rechte:<br /><br />
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:CheckBox ID="cbNurMitRechten" runat="server" Width="257px"  /><br /><br />
                                            </td>
                                        </tr>
                                        <tr class="formquery" style="display:none"> 
                                            <td class="firstLeft active">
                                                &nbsp;
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:TextBox ID="txtEmpty" runat="server" CssClass="InputTextbox" Width="160px" Visible="False">*</asp:TextBox>
                                                <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="../images/empty.gif"
                                                    Width="1px" />
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
                                        <asp:LinkButton class="Tablebutton" ID="btnSuche" runat="server" Text="&amp;nbsp;&amp;#187; Suchen"
                                            CssClass="Tablebutton" Height="16px" Width="78px" Font-Names="Verdana,sans-serif"
                                            Font-Size="10px"></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <div id="Result" runat="Server" visible="false">
                            <div class="ExcelDiv">
                                <div align="right" id="trSearchSpacer" runat="server">
                                    &nbsp;
                                </div>
                            </div>
                            <div id="pagination">
                                <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                            </div>
                            <div id="data">
                                <table cellspacing="0" style="border-color: #ffffff" cellpadding="0" width="100%"
                                    align="left" border="0">
                                    <tbody>
                                        <tr id="trSearchResult" runat="server">
                                            <td align="left">
                                                <asp:GridView ID="dgSearchResult" Width="100%" runat="server" AllowSorting="True"
                                                    AutoGenerateColumns="False" CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0"
                                                    AllowPaging="True" GridLines="None" PageSize="10" EditRowStyle-Wrap="False" PagerStyle-Wrap="True"
                                                    CssClass="GridView">
                                                    <PagerSettings Visible="False" />
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <EditRowStyle Wrap="False"></EditRowStyle>
                                                    <Columns>
                                                        <asp:TemplateField Visible="False" HeaderText="UserID">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_UserID" CommandArgument="UserID" CommandName="Sort" runat="server">Firmenname</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblUserID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.UserID") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="UserName" SortExpression="UserName" HeaderText="Benutzer" />
                                                        <asp:BoundField DataField="CustomerName" SortExpression="CustomerName" HeaderText="Kunde" />
                                                        <asp:BoundField DataField="GroupName" SortExpression="GroupName" HeaderText="Gruppe" />
                                                        <asp:BoundField DataField="OrganizationName" SortExpression="OrganizationName" HeaderText="Orga." />
                                                        <asp:TemplateField SortExpression="HasCategoryRights" HeaderText="Category">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbxHasCategoryRights" AutoPostBack="True" OnCheckedChanged="cbxHasCategoryRights_CheckedChanged"
                                                                    runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "HasCategoryRights") %>'
                                                                    CssClass="editable" Enabled="True"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="CategoryRightsCreateUser" SortExpression="CategoryRightsCreateUser" HeaderText="Geändert von">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CategoryRightsCreateDate" SortExpression="CategoryRightsCreateUser" HeaderText="am">
                                                        </asp:BoundField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div class="dataFooter">
                                &nbsp;
                            </div>
                        </div>
                        <div id="Input" runat="server" visible="False">
                            <div id="adminInput">
                                <table id="Tablex1" class="" runat="server" cellspacing="0" cellpadding="0" width="100%"
                                    border="0">
                                    <tr>
                                        <td>
                                            <table style="border-color: #ffffff">
                                                <tr id="trEditUser" runat="server">
                                                    <td align="left" width="33%" valign="top">
                                                        <table id="tblLeft" style="border-color: #ffffff; padding-right: 50px;" cellspacing="0"
                                                            cellpadding="0">
                                                            <tr>
                                                                <td align="left" colspan="2">
                                                                    <table id="tblGroupDaten" style="border-color: #FFFFFF" cellspacing="0" cellpadding="0"
                                                                        width="100%" border="0">
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active">
                                                                                Benutzername:<asp:TextBox ID="txtUserID" runat="server" Visible="False" Width="0px"
                                                                                    Height="0px" BorderStyle="None" BorderWidth="0px">-1</asp:TextBox>
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:TextBox ID="txtUserName" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active">
                                                                                Anrede:<asp:TextBox ID="txtOrganizationID" runat="server" Visible="False" Width="0px"
                                                                                    Height="0px" BorderStyle="None" BorderWidth="0px">-1</asp:TextBox>
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:DropDownList ID="ddlTitle" CssClass="DropDpwns" runat="server" Height="20px"
                                                                                    Font-Names="Verdana,sans-serif" AutoPostBack="True">
                                                                                    <asp:ListItem Value="-" Selected="True">&lt;Bitte ausw&#228;hlen&gt;</asp:ListItem>
                                                                                    <asp:ListItem Value="Herr">Herr</asp:ListItem>
                                                                                    <asp:ListItem Value="Frau">Frau</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active">
                                                                                Vorname:
                                                                            </td>
                                                                            <td align="left" class="active">
                                                                                <asp:TextBox ID="txtFirstName" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active" nowrap="nowrap">
                                                                                Nachname:
                                                                            </td>
                                                                            <td align="left" class="active">
                                                                                <asp:TextBox ID="txtLastName" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active">
                                                                                Kundenreferenz:
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:TextBox ID="txtReference" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active">
                                                                                Filiale:
                                                                            </td>
                                                                            <td align="left" class="active">
                                                                                <asp:TextBox ID="txtStore" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery" id="trTestUser" runat="server">
                                                                            <td class="firstLeft active">
                                                                                Test-Zugang:
                                                                            </td>
                                                                            <td align="left" class="active">
                                                                                <span>
                                                                                    <asp:CheckBox ID="cbxTestUser" runat="server"></asp:CheckBox></span>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery" id="trCustomer" runat="server">
                                                                            <td class="firstLeft active">
                                                                                Firma:
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:DropDownList ID="ddlCustomer" runat="server" AutoPostBack="True" CssClass="DropDowns">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="Tr1" class="formquery" runat="server">
                                                                            <td class="firstLeft active">
                                                                                Gültig ab:
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:TextBox ID="txtValidFrom" runat="server" Width="160px" Height="20px" MaxLength="10"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="Tr2" class="formquery" runat="server">
                                                                            <td class="firstLeft active">
                                                                                Gültig bis:
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:TextBox ID="txtValidTo" runat="server" Width="160px" Height="20px" MaxLength="10"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery" id="trCustomerAdmin2" runat="server">
                                                                            <td class="firstLeft active">
                                                                                Firmenadministrator:
                                                                            </td>
                                                                            <td class="active">
                                                                                <span>
                                                                                    <asp:CheckBox ID="cbxCustomerAdmin" runat="server"></asp:CheckBox></span>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery" id="trCustomerAdmin1" runat="server">
                                                                            <td class="firstLeft active">
                                                                                First-Level-Admin:
                                                                            </td>
                                                                            <td class="active">
                                                                                <span>
                                                                                    <asp:CheckBox ID="cbxFirstLevelAdmin" runat="server"></asp:CheckBox></span>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery" id="trGroup" runat="server">
                                                                            <td class="firstLeft active">
                                                                                Gruppe:
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:DropDownList ID="ddlGroups" runat="server" CssClass="DropDowns">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery" id="trOrganization" runat="server">
                                                                            <td class="firstLeft active">
                                                                                Organisation:
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:DropDownList ID="ddlOrganizations" runat="server" CssClass="DropDowns">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery" id="trOrganizationAdministrator" runat="server">
                                                                            <td class="firstLeft active">
                                                                                Organisationadministrator:
                                                                            </td>
                                                                            <td class="active">
                                                                                <span>
                                                                                    <asp:CheckBox ID="cbxOrganizationAdmin" runat="server"></asp:CheckBox></span>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>
                                                        <table id="tblRight" style="border-color: #FFFFFF" cellspacing="0" cellpadding="0"
                                                            bgcolor="white" border="0">
                                                            <tr class="formquery">
                                                                <td class="firstLeft active" nowrap="nowrap">
                                                                    letzte Passwortänderung:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <span>
                                                                        <asp:Label ID="lblLastPwdChange" runat="server" Width="160px" CssClass="InputTextbox"></asp:Label></span>
                                                                </td>
                                                            </tr>
                                                            <tr id="trPwdNeverExpires" class="formquery" runat="server">
                                                                <td class="firstLeft active">
                                                                    Passwort läuft nie ab:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <span>
                                                                        <asp:CheckBox ID="cbxPwdNeverExpires" runat="server"></asp:CheckBox></span>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active" nowrap="nowrap">
                                                                    fehlgeschlagene Anmeldungen:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <asp:Label ID="lblFailedLogins" runat="server" Width="160px" CssClass="InputTextbox"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active">
                                                                    Konto gesperrt:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <span>
                                                                        <asp:CheckBox ID="cbxAccountIsLockedOut" runat="server" Width="25px"></asp:CheckBox>
                                                                        <asp:CheckBox ID="cbxApproved" runat="server" Visible="False" Width="25px"></asp:CheckBox></span>
                                                                    <asp:Label ID="lblLockedBy" runat="server" Visible="false"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active">
                                                                    Angemeldet:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <span>
                                                                        <asp:CheckBox ID="chkLoggedOn" runat="server" Width="25px"></asp:CheckBox></span>
                                                                </td>
                                                            </tr>
                                                            <tr id="trMatrix" runat="server" visible="False" class="formquery">
                                                                <td class="firstLeft active">
                                                                    Matrix gefüllt:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <span>
                                                                        <asp:CheckBox ID="chk_Matrix1" runat="server" AutoPostBack="True" Width="25px"></asp:CheckBox></span>
                                                                </td>
                                                            </tr>
                                                            <tr id="trReadMessageCount" runat="server" class="formquery">
                                                                <td class="firstLeft active" nowrap="nowrap">
                                                                    Anzahl der Startmeldungs-Anzeigen:
                                                                </td>
                                                                <td valign="top" align="left" class="active">
                                                                    <asp:TextBox ID="txtReadMessageCount" runat="server" MaxLength="2" CssClass="InputTextbox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="trNewPassword" runat="server" class="formquery">
                                                                <td class="firstLeft active">
                                                                    Neues Passwort setzen:
                                                                </td>
                                                                <td nowrap="nowrap" align="left" class="active">
                                                                    <span>
                                                                        <asp:CheckBox ID="chkNewPasswort" runat="server" Width="25px"></asp:CheckBox></span>
                                                                </td>
                                                            </tr>
                                                            <tr id="trPassword" runat="server" class="formquery">
                                                                <td class="firstLeft active">
                                                                    Passwort:
                                                                </td>
                                                                <td nowrap="nowrap" align="left" class="active">
                                                                    <asp:TextBox ID="txtPassword" runat="server" Visible="true" TextMode="Password" CssClass="InputTextbox"></asp:TextBox><asp:LinkButton
                                                                        ID="btnCreatePassword" runat="server" CssClass="StandardButtonTable" Visible="False">Passwort generieren</asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                            <tr id="trConfirmPassword" runat="server" class="formquery">
                                                                <td class="firstLeft active">
                                                                    Passwort bestätigen:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <asp:TextBox ID="txtConfirmPassword" runat="server" Visible="true" TextMode="Password"
                                                                        CssClass="InputTextbox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <div style="background-color: #dfdfdf; height: 22px;">
                                    &nbsp;
                                </div>
                                <div id="dataFooter">
                                    <asp:LinkButton class="Tablebutton" ID="lbtnSave" runat="server" Text="Speichern&amp;nbsp;&amp;#187; "
                                        CssClass="Tablebutton" Height="16px" Width="78px" Visible="False"></asp:LinkButton><asp:LinkButton
                                            class="Tablebutton" ID="lbtnCancel" runat="server" Text="Verwerfen&amp;nbsp;&amp;#187; "
                                            CssClass="Tablebutton" Height="16px" Width="78px" Visible="False"></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
