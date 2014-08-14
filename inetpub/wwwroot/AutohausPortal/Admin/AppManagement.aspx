<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AppManagement.aspx.vb"
    Inherits="Admin.AppManagement" MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="PageElements/GridNavigation.ascx" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="innerContent">
                    <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                    <asp:Label ID="lblMessage" runat="server" CssClass="TextExtraLarge" EnableViewState="False"></asp:Label>
                    <div id="navigationSubmenu">
                        <asp:HyperLink ID="lnkUserManagement" runat="server" ToolTip="Benutzer" NavigateUrl="UserManagement.aspx"
                            Text="Benutzer | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkContactManagement" runat="server" ToolTip="Ansprechpartner" 
                            NavigateUrl="Contact.aspx" Text="Ansprechpartner | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkGroupManagement" runat="server" ToolTip="Gruppen" NavigateUrl="GroupManagement.aspx"
                            Text="Gruppen | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkOrganizationManagement" runat="server" ToolTip="Organisationen"
                            NavigateUrl="OrganizationManagement.aspx" Text="Organisation | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkCustomerManagement" ToolTip="Kunden" runat="server" NavigateUrl="CustomerManagement.aspx"
                            Text="Kunden | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkArchivManagement" ToolTip="Archive" runat="server" NavigateUrl="ArchivManagement.aspx"
                            Text="Archive | "></asp:HyperLink>
                            
                    </div>
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                        </div>
                        <asp:Panel id="DivSearch" runat="server" DefaultButton="btnEmpty">
                            <div id="TableQuery">
                                <table id="tableSearch" runat="server" cellspacing="0" cellpadding="0">
                                    <tbody>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Anwendung:
                                            </td>
                                            <td class="firstLeft active" nowrap="nowrap" width="100%">
                                                <asp:TextBox ID="txtFilterAppName" runat="server" CssClass="InputTextbox">*</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Freundlicher Name:
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:TextBox ID="txtFilterAppFriendlyName" runat="server" 
                                                    CssClass="InputTextbox">*</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                &nbsp;
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:TextBox ID="txtEmpty" runat="server" CssClass="InputTextbox" Width="160px" Visible="False">*</asp:TextBox>
                                                <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="images/empty.gif"
                                                    Width="1px" />
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                            </td>
                                            <td style="width: 35%">
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                &nbsp;
                                            </td>
                                            <td align="right" nowrap="nowrap" class="rightPadding">
                                                &nbsp;
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
                                        &nbsp;<asp:LinkButton ID="lbtnNew" runat="server" Text="Neue Anwendung&amp;nbsp;&amp;#187; "
                                            CssClass="TablebuttonXLarge" Height="16px" Width="155px" Font-Names="Verdana,sans-serif"
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
                                                    AllowPaging="True" GridLines="None" PageSize="20" EditRowStyle-Wrap="False" PagerStyle-Wrap="True"
                                                    CssClass="GridView">
                                                    <PagerSettings Visible="False" />
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <EditRowStyle Wrap="False"></EditRowStyle>
                                                    <Columns>
                                                        <asp:BoundField DataField="AppID" SortExpression="AppID" HeaderText="AppID"></asp:BoundField>
                                                        <asp:ButtonField DataTextField="AppName" SortExpression="AppName" HeaderText="Anwendung"
                                                            CommandName="Edit"></asp:ButtonField>
                                                        <asp:BoundField DataField="AppFriendlyName" SortExpression="AppFriendlyName" HeaderText="Freundlicher Name" />
                                                        <asp:TemplateField SortExpression="AppInMenu" HeaderText="im Menü">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="CheckBox1" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.AppInMenu") %>'>
                                                                </asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="AppType" SortExpression="AppType" HeaderText="Typ" />
                                                        <asp:BoundField DataField="AppParentName" SortExpression="AppParentName" HeaderText="gehört zu" />
                                                        <asp:TemplateField SortExpression="BatchAuthorization" HeaderText="Sammel-&lt;br&gt;autorisierung">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="CheckBox2" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.BatchAuthorization") %>'>
                                                                </asp:CheckBox>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BatchAuthorization") %>'>
                                                                </asp:TextBox>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:ButtonField CommandName="Del" HeaderText="Löschen" ButtonType="Image" ImageUrl="Images/Papierkorb_01.gif" ControlStyle-Height="16px" ControlStyle-Width="16px">
                                                        </asp:ButtonField>
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
                        <div id="Input" runat="server" visible="false">
                            <div id="adminInput">
                                <table id="Tablex1" class="" runat="server" cellspacing="0" cellpadding="0" width="100%"
                                    border="0">
                                    <tr>
                                        <td>
                                            <table style="border-color: #ffffff">
                                                <tr class="formquery">
                                                    <td class="firstLeft active" colspan="2">
                                                        &nbsp;
                                                        <asp:Label ID="lblInputError" CssClass="TextError" runat="server"></asp:Label>
                                                        <asp:Label ID="lblInputMessage" runat="server" CssClass="TextExtraLarge" EnableViewState="False"></asp:Label>
                                                    </td>
                                                </tr>
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
                                                                                Anwendung:
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:TextBox ID="txtAppName" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                                <asp:TextBox ID="txtAppID" runat="server" Visible="False" Width="0px" Height="0px"
                                                                                    BorderStyle="None" BorderWidth="0px">-1</asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active">
                                                                                Anwendungs-Name:
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:TextBox ID="txtAppFriendlyName" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active" nowrap="nowrap">
                                                                                Typ:
                                                                            </td>
                                                                            <td align="left" class="active">
                                                                                <asp:DropDownList ID="ddlAppType" runat="server" AutoPostBack="True">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active">
                                                                                URL:
                                                                            </td>
                                                                            <td align="left" class="active">
                                                                                <asp:TextBox ID="txtAppURL" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active">
                                                                                Parameter:
                                                                            </td>
                                                                            <td align="left" class="active">
                                                                                <asp:TextBox ID="txtAppParam" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td height="22" align="center" class="firstLeft active">
                                                                                <asp:LinkButton ID="lnkColumnTranslation" runat="server" CssClass="LinksVerwaltung">Spaltenübersetzungen&nbsp;» </asp:LinkButton>
                                                                            </td>
                                                                            <td class="active">
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active">
                                                                                <asp:LinkButton ID="lnkFieldTranslation" runat="server" CssClass="LinksVerwaltung">Feldübersetzungen&nbsp;» </asp:LinkButton>
                                                                            </td>
                                                                            <td class="active">
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active" colspan="2">
                                                                                <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
                                                                               <span id="belongsTo" ></span>
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
                                                                <td class="active">
                                                                    in Menü:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <span>
                                                                        <asp:CheckBox ID="cbxAppInMenu" runat="server"></asp:CheckBox></span>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="active">
                                                                    Kommentar:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <asp:TextBox ID="txtAppComment" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="active">
                                                                    Gehört zu:
                                                                </td>
                                                                <td align="left" class="active" style="width:100%">
                                                                <div style="width:100%;">
                                                                    <asp:DropDownList ID="ddlAppParent" runat="server" 
                                                                        style="width:100%;height:auto; font-size:11px">
                                                                    </asp:DropDownList></div>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="active">
                                                                    Reihenfolge im Menü:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <asp:TextBox ID="txtAppRank" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="active">
                                                                    Autorisierungslevel:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <asp:DropDownList ID="ddlAuthorizationlevel" runat="server" AutoPostBack="True">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td nowrap="nowrap" class="active">
                                                                    Sammelautorisierung:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <span>
                                                                        <asp:CheckBox ID="cbxBatchAuthorization" runat="server"></asp:CheckBox></span>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td nowrap="nowrap" class="active">
                                                                    Schwellwert(in Sekunden):</td>
                                                                <td align="left" class="active">
                                                                    <asp:TextBox ID="txtSchwellwert" runat="server" MaxLength="2" Width="30px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td nowrap="nowrap" class="active">
                                                                    Anwendung ist Liste:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <span>
                                                                        <asp:CheckBox ID="cbxIsList" runat="server"></asp:CheckBox></span>
                                                                </td>
                                                            </tr>
        
                                                            <tr class="formquery">
                                                                <td nowrap="nowrap" class="active">
                                                                    &nbsp;
                                                                </td>
                                                                <td align="left" class="active">
                                                                    &nbsp;
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
                                    &nbsp; &nbsp;<asp:LinkButton class="Tablebutton" ID="lbtnDelete" runat="server" Text="Löschen&amp;nbsp;&amp;#187; "
                                        CssClass="Tablebutton" Height="16px" Width="78px" Visible="False"></asp:LinkButton>&nbsp;<asp:LinkButton
                                            class="Tablebutton" ID="lbtnConfirm" runat="server" Text="Bestätigen&amp;nbsp;&amp;#187; "
                                            CssClass="Tablebutton" Height="16px" Width="78px" Visible="False"></asp:LinkButton>&nbsp;<asp:LinkButton
                                                class="Tablebutton" ID="lbtnSave" runat="server" Text="Speichern&amp;nbsp;&amp;#187; "
                                                CssClass="Tablebutton" Height="16px" Width="78px" Visible="False"></asp:LinkButton>&nbsp;<asp:LinkButton
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
