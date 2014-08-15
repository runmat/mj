<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Groupmanagement.aspx.vb"
    Inherits="Admin.Groupmanagement" MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="~/PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../JScript/Jquery/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="../JScript/Jquery/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <div>
        <div id="site">
            <div id="content">
                <div id="innerContent">
                    <asp:Label ID="lblError" CssClass="TextError" runat="server" EnableViewState="false" />
                    <asp:Label ID="lblMessage" CssClass="TextExtraLarge" runat="server" EnableViewState="False" />
                    <div id="navigationSubmenu">
                        <asp:HyperLink ID="lnkUserManagement" runat="server" ToolTip="Benutzer" NavigateUrl="UserManagement.aspx"
                            Text="Benutzer | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkContact" runat="server" ToolTip="Ansprechpartner" NavigateUrl="Contact.aspx"
                            Text="Ansprechpartner | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkOrganizationManagement" runat="server" ToolTip="Organisationen"
                            NavigateUrl="OrganizationManagement.aspx" Text="Organisation | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkCustomerManagement" ToolTip="Kunden" CssClass="IMGCust" runat="server"
                            NavigateUrl="CustomerManagement.aspx" Text="Kunden | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkArchivManagement" ToolTip="Archive" runat="server" CssClass="IMGArchiv"
                            NavigateUrl="ArchivManagement.aspx" Text="Archive | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkAppManagement" runat="server" ToolTip="Anwendungen" CssClass="IMGApp"
                            NavigateUrl="AppManagement.aspx" Text="Anwendungen"></asp:HyperLink>
                    </div>
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                            </h1>
                            <span id="arealnkSuche" class="AdminMgmtNav" style="display: none">&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;</span><a id="lnkSuche" class="AdminMgmtNavLink" href="javascript:void(0)" onclick="showSearchFilterArea();" style="display: none">Suche</a>
                            <span id="arealnkSuchergebnis" class="AdminMgmtNav" style="display: none">&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;</span><a id="lnkSuchergebnis" class="AdminMgmtNavLink" href="javascript:void(0)" onclick="showSearchResultArea();" style="display: none">Suchergebnis</a>
                            <%-- ihExpandStatus...Area enthält jeweils den Sollwert für den nächsten Seitenzustand, ihNewExpandStatus...Area den aktuellen --%>
                            <input id="ihExpandstatusSearchFilterArea" type="hidden" runat="server" value="1"/>
                            <input id="ihNewExpandstatusSearchFilterArea" type="hidden" runat="server" value="1"/>
                            <input id="ihExpandstatusSearchResultArea" type="hidden" runat="server" value="0"/>
                            <input id="ihNewExpandstatusSearchResultArea" type="hidden" runat="server" value="0"/>
                            <input id="ihExpandStatusInputArea" type="hidden" runat="server" value="0"/>
                            <input id="ihNewExpandStatusInputArea" type="hidden" runat="server" value="0"/>
                        </div>
                        <asp:Panel ID="DivSearch1" runat="server" DefaultButton="btnEmpty" style="display: none">
                            <div id="TableQuery">
                                <table id="tableSearch" runat="server" cellspacing="0" cellpadding="0">
                                    <tbody>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Firma:
                                            </td>
                                            <td class="firstLeft active" nowrap="nowrap" width="100%">
                                                <asp:DropDownList ID="ddlFilterCustomer" runat="server" Font-Names="Verdana,sans-serif"
                                                    Height="20px" Visible="False" Width="260px">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblCustomer" runat="server" Width="160px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery" id="trSelectOrganization" runat="server">
                                            <td class="firstLeft active">
                                                Gruppe:
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:TextBox ID="txtFilterGroupName" runat="server" CssClass="InputTextbox" Width="257px">*</asp:TextBox>
                                                <asp:Label ID="lblGroupName" runat="server" Visible="False" Width="160px"></asp:Label>
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
                                        &nbsp;<asp:LinkButton ID="lbtnNew" runat="server" Text="Neue Gruppe&amp;nbsp;&amp;#187; "
                                            CssClass="TablebuttonXLarge" Height="16px" Width="155px" Font-Names="Verdana,sans-serif"
                                            Font-Size="10px"></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <div id="Result" runat="Server" style="display: none">
                            <div id="pagination">
                                <uc2:GridNavigation ID="GridNavigation1" runat="server" PageSizeIndex="0">
                                </uc2:GridNavigation>
                            </div>
                            <div id="data">
                                <table cellspacing="0" style="border-width: 0px; border-color: #ffffff;" cellpadding="0"
                                    width="100%" align="left">
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
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGroupID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.GroupID") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:ButtonField DataTextField="GroupName" SortExpression="GroupName" HeaderText="Gruppe"
                                                            CommandName="Edit"></asp:ButtonField>
                                                        <asp:TemplateField SortExpression="IsCustomerGroup" HeaderText="Kundengruppe">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="CheckBox1" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.IsCustomerGroup") %>'>
                                                                </asp:CheckBox>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.IsCustomerGroup") %>'>
                                                                </asp:TextBox>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="IsServiceGroup" HeaderText="Service-Gruppe">
                                                            <ItemTemplate>
                                                                <asp:CheckBox runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.IsServiceGroup") %>'
                                                                    Enabled="false"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="TVShow" HeaderText="Teamviewer">
                                                            <ItemTemplate>
                                                                <asp:CheckBox runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.TVShow") %>'
                                                                    Enabled="false"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Authorizationright" SortExpression="Authorizationright"
                                                            HeaderText="Autorisierungslevel"></asp:BoundField>
                                                        <asp:ButtonField CommandName="Del" HeaderText="Löschen" ButtonType="Image" ControlStyle-Height="16px"
                                                            ControlStyle-Width="16px" ImageUrl="/Services/Images/Papierkorb_01.gif"></asp:ButtonField>
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
                        <div id="Input" runat="server" style="display: none">
                            <div id="adminInput">
                                <table id="Tablex1" runat="server" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td width="100%">
                                            <table style="border-color: #ffffff" width="100%">
                                                <tr>
                                                    <td align="left" width="50%" valign="top">
                                                        <table id="tblLeft" style="border-color: #ffffff; padding-right: 50px;" cellspacing="0"
                                                            cellpadding="0">
                                                            <tr>
                                                                <td align="left" colspan="2">
                                                                    <table id="tblGroupDaten" style="border-color: #FFFFFF" cellspacing="0" cellpadding="0"
                                                                        width="100%" border="0">
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active">
                                                                                Gruppenname:<asp:TextBox ID="txtGroupID" runat="server" Visible="False" Width="0px"
                                                                                    Height="0px" BorderStyle="None" BorderWidth="0px">-1</asp:TextBox>
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:TextBox ID="txtGroupName" Width="250px" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery" id="trCustomer" runat="server">
                                                                            <td class="firstLeft active">
                                                                                Firma:
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:TextBox ID="txtCustomer" Width="250px" runat="server" Class="InputTextbox"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td class="firstLeft active">
                                                                                Pfad zum Handbuch:
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:TextBox ID="txtDocuPath" Width="250px" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery" id="trAccountingArea" runat="server">
                                                                            <td class="firstLeft active">
                                                                                <asp:TextBox ID="txtCustomerID" runat="server" Visible="False" Width="16px" Height="16px"
                                                                                    BorderStyle="None" BorderWidth="0px">-1</asp:TextBox>
                                                                            </td>
                                                                            <td class="active">
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trTVShow" runat="server" class="formquery">
                                                                            <td class="firstLeft active">
                                                                                TeamViewer verwenden:
                                                                            </td>
                                                                            <td class="active">
                                                                                <span>
                                                                                    <asp:CheckBox ID="cbxTeamViewer" runat="server" AutoPostBack="true" Enabled="false" />
                                                                                </span>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trIsServiceGroup" runat="server" class="formquery">
                                                                            <td class="firstLeft active">
                                                                                Service-Gruppe:
                                                                            </td>
                                                                            <td class="active">
                                                                                <span>
                                                                                    <asp:CheckBox ID="cbxIsServiceGroup" runat="server" AutoPostBack="true" Enabled="false" />
                                                                                </span>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td valign="top" align="center" width="50%">
                                                        <table id="tblRight" style="border-color: #FFFFFF" cellspacing="0" cellpadding="0"
                                                            bgcolor="white" border="0">
                                                            <tr id="trPwdRules" runat="server">
                                                                <td align="left" colspan="2">
                                                                    <table id="tblPwdRules" style="border-color: #FFFFFF" cellspacing="0" cellpadding="0"
                                                                        width="100%" border="0">
                                                                        <tr class="formquery">
                                                                            <td class="active">
                                                                                Autorisierungsberechtigung:
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:DropDownList ID="ddlAuthorizationright" runat="server" Width="250px">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td class="active">
                                                                                Kundengruppe:
                                                                            </td>
                                                                            <td class="active">
                                                                                <span>
                                                                                    <asp:CheckBox ID="cbxIsCustomerGroup" runat="server"></asp:CheckBox></span>
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery" id="trStartMethod" runat="server">
                                                                            <td class="active">
                                                                                Startmethode:
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:TextBox ID="txtStartMethod" runat="server" CssClass="InputTextbox" Width="250px"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td class="active">
                                                                                n Sonderzeichen:
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:TextBox ID="txtNCapitalLetter" runat="server" CssClass="InputTextbox" Width="250px">1</asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="formquery">
                                                                            <td class="active">
                                                                                Berechtigungslevel setzen:
                                                                            </td>
                                                                            <td class="active">
                                                                                <span>
                                                                                    <asp:CheckBox ID="cbxLevel" runat="server" AutoPostBack="true" />
                                                                                </span>
                                                                            </td>
                                                                            <td>
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
                                            <table id="tblDown" cellspacing="0" cellpadding="0" style="border-width: 0px; border-color: #FFFFFF;"
                                                width="100%">
                                                <tr id="trRights" runat="server" visible="false">
                                                    <td class="InfoBoxFlat" valign="top" align="left" colspan="3">
                                                        <div class="formqueryHeader">
                                                            <span>Autorisierungslevel</span>
                                                        </div>
                                                        <table style="border-color: #FFFFFF" cellspacing="0" cellpadding="0" width="100%"
                                                            border="0">
                                                            <tr class="formquery">
                                                                <td class="firstLeft active" colspan="3">
                                                                    <table id="tableAuthLevel" runat="server" style="border-width: 0;" cellspacing="0"
                                                                        cellpadding="0" visible="False">
                                                                        <tr>
                                                                            <td style="background-color: #C2D69A; width: 340px; color: #000000; padding: 5px;">
                                                                                <div style="text-underline-color: #000000; text-decoration: underline; font-size: 12px;">
                                                                                    Berechtigungslevel
                                                                                </div>
                                                                                <br />
                                                                                <div style="text-decoration: none;">
                                                                                    Level 1: Parameter notwendig, kein Bearbeiten m&ouml;glich<br />
                                                                                    Level 2: Parameter notwendig, Bearbeiten m&ouml;glich<br />
                                                                                    Level 3: Parameter optional, Bearbeiten m&ouml;glich<br />
                                                                                </div>
                                                                            </td>
                                                                            <td style="background: #93CDDD; width: 340px; color: #000000">
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <asp:Image ID="Image2" runat="server" ImageUrl="/services/images/versandlegende.png"
                                                                        Visible="True" />
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active">
                                                                    Anwendung:&nbsp;&nbsp;
                                                                </td>
                                                                <td class="firstLeft active">
                                                                    <asp:DropDownList ID="ddlAnwendung" runat="server" AutoPostBack="true" Width="300px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3" class="firstLeft active">
                                                                    <asp:Label ID="lblAutError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="active" colspan="3">
                                                                    <div class="dataGrid">
                                                                        <asp:GridView ID="gvAutorisierung" Width="100%" runat="server" AutoGenerateColumns="False"
                                                                            CellPadding="0" GridLines="None" PageSize="10" CssClass="GridView" ShowHeader="False"
                                                                            ShowFooter="False">
                                                                            <PagerSettings Visible="False" />
                                                                            <PagerStyle Wrap="True"></PagerStyle>
                                                                            <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                                            <AlternatingRowStyle BackColor="#DEE1E0" CssClass="GridTableAlternate" />
                                                                            <RowStyle CssClass="ItemStyle" Height="40px" />
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="Anwendung">
                                                                                    <ItemTemplate>
                                                                                        <%# Eval("Name")%>
                                                                                    </ItemTemplate>
                                                                                    <EditItemTemplate>
                                                                                        <%# Eval("Name")%>
                                                                                    </EditItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Level">
                                                                                    <ItemTemplate>
                                                                                        Level&nbsp;<asp:Literal runat="server" ID="litItemLevel" Text='<%# Eval("Level")%>' />
                                                                                    </ItemTemplate>
                                                                                    <EditItemTemplate>
                                                                                        <asp:DropDownList ID="ddlEditLevel" runat="server" SelectedValue='<%# Bind("Level") %>' Width="125px">
                                                                                            <asp:ListItem Value="0">---Auswahl---</asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </EditItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Autorisierung">
                                                                                    <ItemTemplate>
                                                                                        <asp:DropDownList ID="ddlItemAutorisierung" runat="server" SelectedValue='<%# Bind("Autorisierung") %>'
                                                                                            AutoPostBack="True" OnSelectedIndexChanged="ddlAutorisierungChanged">
                                                                                            <asp:ListItem Value="1">Autorisierung</asp:ListItem>
                                                                                            <asp:ListItem Value="2">keine Autorisierung</asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </ItemTemplate>
                                                                                    <EditItemTemplate>
                                                                                        <asp:DropDownList ID="ddlEditAutorisierung" runat="server" SelectedValue='<%# Bind("Autorisierung") %>'>
                                                                                            <asp:ListItem Value="1">Autorisierung</asp:ListItem>
                                                                                            <asp:ListItem Value="2">keine Autorisierung</asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </EditItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Löschen">
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton runat="server" Width="16" Height="16" ImageUrl="/Services/Images/Papierkorb_01.gif"
                                                                                            CommandName="Del" CommandArgument='<%# Container.DataItemIndex %>' />
                                                                                    </ItemTemplate>
                                                                                    <EditItemTemplate>
                                                                                        <asp:ImageButton runat="server" Width="16"
                                                                                            Height="16" ImageUrl="/Services/Images/Plus.gif" CommandName="Add" />
                                                                                    </EditItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active" colspan="3">
                                                                    <asp:ImageButton ID="ibtNew" runat="server" ImageUrl="/Services/Images/add.png" Height="20px"
                                                                        Width="20px" />
                                                                    &nbsp;<asp:Label ID="Label3" runat="server" Style="font-size: small; font-weight: 700"
                                                                        Text="Level hinzufügen" Height="20px"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active" colspan="3">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr id="trApp" runat="server">
                                                    <td valign="top" align="left">
                                                        <div class="formqueryHeader">
                                                            <span>Anwendungen</span>
                                                        </div>
                                                        <table id="tblApp" cellspacing="0" cellpadding="0" width="100%" style="border-color: #FFFFFF"
                                                            border="0">
                                                            <tr class="formquery">
                                                                <td class="firstLeft active" width="35%">
                                                                    nicht zugewiesen
                                                                    <p>
                                                                        <asp:ListBox ID="lstAppUnAssigned" runat="server" SelectionMode="Multiple" CssClass="InputTextbox"
                                                                            Width="300px" Height="150px"></asp:ListBox>
                                                                    </p>
                                                                </td>
                                                                <td class="active" width="15%">
                                                                    <p>
                                                                        &nbsp;</p>
                                                                    <p>
                                                                        &nbsp;</p>
                                                                    <span style="padding-left: 55px"></span>
                                                                    <asp:ImageButton ID="btnAssign" runat="server" ImageUrl="/Services/Images/Pfeil_vor_01.jpg"
                                                                        ToolTip="Zuweisen" Height="37px" Width="37px" />
                                                                </td>
                                                                <td class="active" width="15%">
                                                                    <p>
                                                                        &nbsp;</p>
                                                                    <p>
                                                                        &nbsp;</p>
                                                                    <span style="padding-left: 30px">
                                                                        <asp:ImageButton ID="btnUnAssign" runat="server" ImageUrl="/Services/Images/Pfeil_zurueck_01.jpg"
                                                                            ToolTip="Entfernen" Height="37px" Width="37px" /></span>
                                                                </td>
                                                                <td class="active" width="35%">
                                                                    zugewiesen
                                                                    <p>
                                                                        <asp:ListBox ID="lstAppAssigned" runat="server" SelectionMode="Multiple" CssClass="InputTextbox"
                                                                            Width="300px" Height="150"></asp:ListBox>
                                                                    </p>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr id="trArchiv" runat="server">
                                                    <td class="InfoBoxFlat" valign="top" align="left">
                                                        <div class="formqueryHeader">
                                                            <span>Archive</span>
                                                        </div>
                                                        <table cellspacing="0" cellpadding="0" width="100%" style="border-color: #FFFFFF"
                                                            border="0">
                                                            <tr class="formquery">
                                                                <td class="firstLeft active" width="35%">
                                                                    nicht zugewiesen
                                                                    <p>
                                                                        <asp:ListBox ID="lstArchivUnAssigned" runat="server" SelectionMode="Multiple" CssClass="InputTextbox"
                                                                            Width="300px" Height="150px"></asp:ListBox>
                                                                    </p>
                                                                </td>
                                                                <td class="active" width="15%">
                                                                    <p>
                                                                        &nbsp;</p>
                                                                    <p>
                                                                        &nbsp;</p>
                                                                    <span style="padding-left: 55px"></span>
                                                                    <asp:ImageButton ID="btnAssignArchiv" runat="server" ImageUrl="/Services/Images/Pfeil_vor_01.jpg"
                                                                        ToolTip="Zuweisen" Height="37px" Width="37px" />
                                                                </td>
                                                                <td class="active" width="15%">
                                                                    <p>
                                                                        &nbsp;</p>
                                                                    <p>
                                                                        &nbsp;</p>
                                                                    <span style="padding-left: 30px">
                                                                        <asp:ImageButton ID="btnUnAssignArchiv" runat="server" ImageUrl="/Services/Images/Pfeil_zurueck_01.jpg"
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
                                                    </td>
                                                </tr>
                                                <tr id="trAbrufgruendeEndg" runat="server">
                                                    <td class="InfoBoxFlat" valign="top" align="left">
                                                        <div class="formqueryHeader">
                                                            <span>Abrufgründe endgültig</span>
                                                        </div>
                                                        <table cellspacing="0" cellpadding="0" width="100%" style="border-color: #FFFFFF"
                                                            border="0">
                                                            <tr class="formquery">
                                                                <td class="firstLeft active" width="35%">
                                                                    nicht zugewiesen
                                                                    <p>
                                                                        <asp:ListBox ID="lstAbrufgruendeEndgUnAssigned" runat="server" SelectionMode="Multiple" CssClass="InputTextbox"
                                                                            Width="300px" Height="150px"></asp:ListBox>
                                                                    </p>
                                                                </td>
                                                                <td class="active" width="15%">
                                                                    <p>
                                                                        &nbsp;</p>
                                                                    <p>
                                                                        &nbsp;</p>
                                                                    <span style="padding-left: 55px"></span>
                                                                    <asp:ImageButton ID="btnAssignAbrufgruendeEndg" runat="server" ImageUrl="/Services/Images/Pfeil_vor_01.jpg"
                                                                        ToolTip="Zuweisen" Height="37px" Width="37px" />
                                                                </td>
                                                                <td class="active" width="15%">
                                                                    <p>
                                                                        &nbsp;</p>
                                                                    <p>
                                                                        &nbsp;</p>
                                                                    <span style="padding-left: 30px">
                                                                        <asp:ImageButton ID="btnUnAssignAbrufgruendeEndg" runat="server" ImageUrl="/Services/Images/Pfeil_zurueck_01.jpg"
                                                                            ToolTip="Entfernen" Height="37px" Width="37px" /></span>
                                                                </td>
                                                                <td class="active" width="35%">
                                                                    zugewiesen
                                                                    <p>
                                                                        <asp:ListBox ID="lstAbrufgruendeEndgAssigned" runat="server" SelectionMode="Multiple" CssClass="InputTextbox"
                                                                            Width="300px" Height="150"></asp:ListBox>
                                                                    </p>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr id="trAbrufgruendeTemp" runat="server">
                                                    <td class="InfoBoxFlat" valign="top" align="left">
                                                        <div class="formqueryHeader">
                                                            <span>Abrufgründe temporär</span>
                                                        </div>
                                                        <table cellspacing="0" cellpadding="0" width="100%" style="border-color: #FFFFFF"
                                                            border="0">
                                                            <tr class="formquery">
                                                                <td class="firstLeft active" width="35%">
                                                                    nicht zugewiesen
                                                                    <p>
                                                                        <asp:ListBox ID="lstAbrufgruendeTempUnAssigned" runat="server" SelectionMode="Multiple" CssClass="InputTextbox"
                                                                            Width="300px" Height="150px"></asp:ListBox>
                                                                    </p>
                                                                </td>
                                                                <td class="active" width="15%">
                                                                    <p>
                                                                        &nbsp;</p>
                                                                    <p>
                                                                        &nbsp;</p>
                                                                    <span style="padding-left: 55px"></span>
                                                                    <asp:ImageButton ID="btnAssignAbrufgruendeTemp" runat="server" ImageUrl="/Services/Images/Pfeil_vor_01.jpg"
                                                                        ToolTip="Zuweisen" Height="37px" Width="37px" />
                                                                </td>
                                                                <td class="active" width="15%">
                                                                    <p>
                                                                        &nbsp;</p>
                                                                    <p>
                                                                        &nbsp;</p>
                                                                    <span style="padding-left: 30px">
                                                                        <asp:ImageButton ID="btnUnAssignAbrufgruendeTemp" runat="server" ImageUrl="/Services/Images/Pfeil_zurueck_01.jpg"
                                                                            ToolTip="Entfernen" Height="37px" Width="37px" /></span>
                                                                </td>
                                                                <td class="active" width="35%">
                                                                    zugewiesen
                                                                    <p>
                                                                        <asp:ListBox ID="lstAbrufgruendeTempAssigned" runat="server" SelectionMode="Multiple" CssClass="InputTextbox"
                                                                            Width="300px" Height="150"></asp:ListBox>
                                                                    </p>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr class="formquery" id="trMeldung" runat="server">
                                                    <td class="InfoBoxFlat" valign="top" align="left">
                                                        <div class="formqueryHeader">
                                                            <span>Startmeldung</span>
                                                        </div>
                                                        <table id="tblMeldung" cellspacing="0" cellpadding="0" border="0" style="border-color: #FFFFFF">
                                                            <tr>
                                                                <td class="firstLeft active" valign="top" width="60%">
                                                                    <div>
                                                                        Häufigkeit der Startmeldungsanzeige (pro Benutzer):
                                                                        <asp:TextBox ID="txtMaxReadMessageCount" runat="server" Width="40px" MaxLength="2"
                                                                            CssClass="InputTextbox"></asp:TextBox></div>
                                                                    <br />
                                                                    <asp:Label Font-Size="Medium" ID="lblInfo" runat="server" CssClass="TextError" />
                                                                    <asp:TextBox ID="txtMessageOld" runat="server" Visible="False" MaxLength="500" TextMode="MultiLine"></asp:TextBox>
                                                                </td>
                                                                <td class="firstLeft active" valign="top">
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="firstLeft">
                                                        <%-- Aus der eigentlichen Zeile herausgezogen da Styles sonst überschrieben werden--%>
                                                        <telerik:RadEditor ID="radMessage" runat="server" Width="600px" Height="300px" ToolsFile="Templates/RadEditToolsLimited.xml"
                                                            EnableResize="False">
                                                        </telerik:RadEditor>
                                                    </td>
                                                    <td>
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
                                    &nbsp;&nbsp;
                                    <asp:LinkButton CssClass="Tablebutton" ID="lbtnDelete" runat="server" Text="Löschen&amp;nbsp;&amp;#187; "
                                        Height="16px" Width="78px" Visible="False"/>&nbsp;
                                    <asp:LinkButton CssClass="Tablebutton" ID="lbtnConfirm" runat="server" Text="Bestätigen&amp;nbsp;&amp;#187; "
                                        Height="16px" Width="78px" Visible="False"/>&nbsp;
                                    <asp:LinkButton CssClass="Tablebutton" ID="lbtnSave" runat="server" Text="Speichern&amp;nbsp;&amp;#187; "
                                        Height="16px" Width="78px"/>&nbsp;
                                    <asp:LinkButton CssClass="Tablebutton" ID="lbtnCancel" runat="server" Text="Verwerfen&amp;nbsp;&amp;#187; "
                                        Height="16px" Width="78px"/>
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
                $("#<%= ihNewExpandstatusSearchFilterArea.ClientID %>").attr("value", "1");
                $("#<%= ihNewExpandstatusSearchResultArea.ClientID %>").attr("value", "0");
                $("#<%= ihNewExpandstatusInputArea.ClientID %>").attr("value", "0");
            }
            else if ($("#<%= ihExpandstatusSearchResultArea.ClientID %>").attr("value") == "1") {
                $("#arealnkSuchergebnis").hide();
                $("#lnkSuchergebnis").hide();
                $("#arealnkSuche").show();
                $("#lnkSuche").show();
                $("#<%= DivSearch1.ClientID %>").hide();
                $("#<%= Input.ClientID %>").hide();
                $("#<%= Result.ClientID %>").show();
                $("#<%= ihNewExpandstatusSearchFilterArea.ClientID %>").attr("value", "0");
                $("#<%= ihNewExpandstatusSearchResultArea.ClientID %>").attr("value", "1");
                $("#<%= ihNewExpandstatusInputArea.ClientID %>").attr("value", "0");
            }
            else {
                $("#arealnkSuche").show();
                $("#lnkSuche").show();
                $("#arealnkSuchergebnis").show();
                $("#lnkSuchergebnis").show();
                $("#<%= DivSearch1.ClientID %>").hide();
                $("#<%= Result.ClientID %>").hide();
                $("#<%= Input.ClientID %>").show();
                $("#<%= ihNewExpandstatusSearchFilterArea.ClientID %>").attr("value", "0");
                $("#<%= ihNewExpandstatusSearchResultArea.ClientID %>").attr("value", "0");
                $("#<%= ihNewExpandstatusInputArea.ClientID %>").attr("value", "1");
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
        }); 
        
    </script>
</asp:Content>
