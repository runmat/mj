<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GroupManagement.aspx.vb"
    Inherits="CKG.Admin.GroupManagement" ValidateRequest="false"%>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="DAD DEUTSCHER AUTO DIENST GmbH" name="Copyright" />
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema" />
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body leftmargin="0" topmargin="0">
    <form id="Form1" method="post" runat="server">

    <script language="javascript" type="text/javascript" id="ScrollPosition">
<!--

        function sstchur_SmartScroller_GetCoords() {

            var scrollX, scrollY;

            if (document.all) {
                if (!document.documentElement.scrollLeft)
                    scrollX = document.body.scrollLeft;
                else
                    scrollX = document.documentElement.scrollLeft;

                if (!document.documentElement.scrollTop)
                    scrollY = document.body.scrollTop;
                else
                    scrollY = document.documentElement.scrollTop;
            }
            else {
                scrollX = window.pageXOffset;
                scrollY = window.pageYOffset;
            }



            document.forms["Form1"].xCoordHolder.value = scrollX;
            document.forms["Form1"].yCoordHolder.value = scrollY;

        }

        function sstchur_SmartScroller_Scroll() {


            var x = document.forms["Form1"].xCoordHolder.value;
            var y = document.forms["Form1"].yCoordHolder.value;
            window.scrollTo(x, y);

        }

        window.onload = sstchur_SmartScroller_Scroll;
        window.onscroll = sstchur_SmartScroller_GetCoords;
        window.onkeypress = sstchur_SmartScroller_GetCoords;
        window.onclick = sstchur_SmartScroller_GetCoords;
  
// -->
    </script>

    <input type="hidden" id="xCoordHolder" runat="server" />
    <input type="hidden" id="yCoordHolder" runat="server" />
    <table cellspacing="0" cellpadding="0" width="100%" align="center">
        <tbody>
            <tr>
                <td>
                    <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tbody>
                            <tr>
                                <td class="PageNavigation" colspan="2" height="25">
                                    Administration (Gruppenverwaltung)
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" width="120">
                                    <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                        border="0">
                                        <tr>
                                            <td class="TaskTitle">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="center" width="150">
                                                <asp:LinkButton ID="lbtnNew" runat="server" CssClass="StandardButton"> &#149;&nbsp;Neue Gruppe anlegen</asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="center" width="150">
                                                <asp:LinkButton ID="lbtnSave" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Speichern</asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="center" width="150">
                                                <asp:LinkButton ID="lbtnCancel" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Verwerfen</asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="center" width="150">
                                                <asp:LinkButton ID="lbtnDelete" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Löschen</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top">
                                    <table cellspacing="0" cellpadding="0" width="100%" align="left" border="0">
                                        <tbody>
                                            <tr>
                                                <td class="TaskTitle">
                                                    <asp:HyperLink ID="lnkUserManagement" runat="server" CssClass="TaskTitle" NavigateUrl="UserManagement.aspx">Benutzerverwaltung</asp:HyperLink><asp:HyperLink
                                                        ID="lnkOrganizationManagement" runat="server" CssClass="TaskTitle" NavigateUrl="OrganizationManagement.aspx">Organisationsverwaltung</asp:HyperLink><asp:HyperLink
                                                            ID="lnkCustomerManagement" runat="server" CssClass="TaskTitle" NavigateUrl="CustomerManagement.aspx">Kundenverwaltung</asp:HyperLink>
                                                    <asp:HyperLink ID="lnkAppManagement" runat="server" CssClass="TaskTitle" NavigateUrl="AppManagement.aspx"
                                                        Visible="False">Anwendungsverwaltung</asp:HyperLink>
                                                    <asp:HyperLink ID="lnkArchivManagement" runat="server" CssClass="TaskTitle" NavigateUrl="ArchivManagement.aspx">Archivverwaltung</asp:HyperLink>&nbsp;
                                                </td>
                                            </tr><tr id="trSearch" runat="server">
                            
                                <td align="left">
                                    <table bgcolor="white" border="0">
                                        <tr>
                                            <td valign="bottom" width="100">
                                                Firma:
                                            </td>
                                            <td valign="bottom" width="160">
                                                <asp:Label ID="lblCustomer" runat="server" CssClass="InfoBoxFlat" Width="160px"></asp:Label><asp:DropDownList
                                                    ID="ddlFilterCustomer" runat="server" Visible="False" Width="160px" Height="20px">
                                                </asp:DropDownList>
                                            </td>
                                            <td valign="bottom">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="bottom" width="100">
                                                Gruppe:
                                            </td>
                                            <td valign="bottom" width="160">
                                                <asp:Label ID="lblGroupName" runat="server" CssClass="InfoBoxFlat" Visible="False"
                                                    Width="160px"></asp:Label><asp:TextBox ID="txtFilterGroupName" runat="server" Visible="False"
                                                        Width="160px" Height="20px">*</asp:TextBox>
                                            </td>
                                            <td valign="bottom">
                                                <asp:LinkButton ID="btnSuche" runat="server" CssClass="StandardButton">Suche</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            
                            <tr id="trSearchSpacer" runat="server">
                            <td></td>
                                <td align="left" height="25">
                                </td>
                            </tr>
                            <tr id="trSearchResult" runat="server">
                            <td></td>
                                <td align="left">
                                    <asp:DataGrid ID="dgSearchResult" runat="server" Width="100%" BackColor="White" AllowPaging="True"
                                        AutoGenerateColumns="False" AllowSorting="True" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="1px">
                                        <SelectedItemStyle BackColor="#FFE0C0"></SelectedItemStyle>
                                        <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" CssClass="GridTableHead" VerticalAlign="Top">
                                        </HeaderStyle>
                                        <Columns>
                                            <asp:BoundColumn Visible="False" DataField="GroupID" SortExpression="GroupID" HeaderText="GroupID">
                                            </asp:BoundColumn>
                                            <asp:ButtonColumn DataTextField="GroupName" SortExpression="GroupName" HeaderText="Gruppe"
                                                CommandName="Edit"></asp:ButtonColumn>
                                            <asp:TemplateColumn SortExpression="IsCustomerGroup" HeaderText="Kundengruppe">
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CheckBox1" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.IsCustomerGroup") %>'>
                                                    </asp:CheckBox>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.IsCustomerGroup") %>'>
                                                    </asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn SortExpression="IsServiceGroup" HeaderText="Service-Gruppe">
                                                <ItemTemplate>
                                                    <asp:CheckBox runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.IsServiceGroup") %>' Enabled="false">
                                                    </asp:CheckBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn SortExpression="TVShow" HeaderText="Teamviewer">
                                                <ItemTemplate>
                                                    <asp:CheckBox runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.TVShow") %>' Enabled="false">
                                                    </asp:CheckBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn DataField="Authorizationright" SortExpression="Authorizationright"
                                                HeaderText="Autorisierungs-&lt;br&gt;level">
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:TemplateColumn HeaderText="l&#246;schen">
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ibtnSRDelete" runat="server" CausesValidation="false" CommandName="Delete"
                                                        ImageUrl="../Images/icon_nein_s.gif"></asp:ImageButton>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <PagerStyle Mode="NumericPages"></PagerStyle>
                                    </asp:DataGrid>
                                </td>
                            </tr>
                            <tr id="trEditUser" runat="server">
                            <td></td>
                                <td align="left">
                                    <table width="740" border="0">
                                        <tr>
                                            <td valign="top" align="left">
                                                <table id="tblLeft" cellspacing="2" cellpadding="0" width="345" bgcolor="white" border="0">
                                                    <tr>
                                                        <td height="22">
                                                            Gruppenname:
                                                        </td>
                                                        <td align="right" height="22">
                                                            <asp:TextBox ID="txtGroupID" runat="server" Visible="False" Width="10px" Height="10px"
                                                                BackColor="#CEDBDE" BorderStyle="None" BorderWidth="0px" ForeColor="#CEDBDE">-1</asp:TextBox><asp:TextBox
                                                                    ID="txtGroupName" runat="server" Width="160px" Height="20px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr id="trCustomer" runat="server">
                                                        <td height="22">
                                                            Firma:
                                                        </td>
                                                        <td align="right" height="22">
                                                            <asp:TextBox ID="txtCustomerID" runat="server" Visible="False" Width="10px" Height="10px"
                                                                BackColor="#CEDBDE" BorderStyle="None" BorderWidth="0px" ForeColor="#CEDBDE">-1</asp:TextBox><asp:TextBox
                                                                    ID="txtCustomer" runat="server" CssClass="InfoBoxFlat" Width="160px" Height="20px"
                                                                    Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td height="22">
                                                            Pfad zum Handbuch:
                                                        </td>
                                                        <td align="right" height="22">
                                                            <asp:TextBox ID="txtDocuPath" runat="server" Width="160px" Height="20px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr id="trTVShow" runat="server">
                                                        <td height="22">
                                                            TeamViewer verwenden:
                                                        </td>
                                                        <td align="right" height="22">
                                                            <span>
                                                                <asp:CheckBox ID="cbxTeamViewer" runat="server" AutoPostBack="true" Enabled="false" />
                                                            </span>
                                                        </td>
                                                    </tr>
                                                    <tr id="trIsServiceGroup" runat="server">
                                                        <td height="22">
                                                            Service-Gruppe:
                                                        </td>
                                                        <td align="right" height="22">
                                                            <span>
                                                                <asp:CheckBox ID="cbxIsServiceGroup" runat="server" AutoPostBack="true" Enabled="False" />
                                                            </span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td valign="top" align="left">
                                                <table id="tblRight" cellspacing="2" cellpadding="0" width="345" bgcolor="white"
                                                    border="0">
                                                    <tr id="trAuthorization" runat="server">
                                                        <td height="22">
                                                            Autorisierungs-<br />
                                                            berechtigung:
                                                        </td>
                                                        <td valign="top" align="right" height="22">
                                                            <asp:DropDownList ID="ddlAuthorizationright" runat="server" Width="160px" Height="20px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr id="trCustomergroup" runat="server">
                                                        <td height="22">
                                                            Kundengruppe:
                                                        </td>
                                                        <td valign="top" align="right" height="22">
                                                            <asp:CheckBox ID="cbxIsCustomerGroup" runat="server"></asp:CheckBox>
                                                        </td>
                                                    </tr>
                                                    <tr id="trStartMethod" runat="server">
                                                        <td height="22">
                                                            Startmethode:
                                                        </td>
                                                        <td valign="top" align="right" height="22">
                                                            <asp:TextBox ID="txtStartMethod" runat="server" Width="160px" Height="20px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="trApp" runat="server">
                                            <td class="InfoBoxFlat" align="left" colspan="2" valign="top">
                                                <table id="tblApp" cellspacing="0" cellpadding="0" bgcolor="white" border="0">
                                                    <tr>
                                                        <td align="middle" colspan="3" height="22">
                                                            Anwendungen
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td height="22">
                                                            nicht zugewiesen
                                                        </td>
                                                        <td align="right" height="22">
                                                        </td>
                                                        <td align="left" height="22">
                                                            zugewiesen
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td height="22">
                                                            <asp:ListBox ID="lstAppUnAssigned" runat="server" Width="370px" Height="135px" SelectionMode="Multiple">
                                                            </asp:ListBox>
                                                        </td>
                                                        <td align="right" height="22">
                                                            <asp:Button ID="btnAssign" runat="server" CssClass="StandardButton" Width="20px"
                                                                Text=">"></asp:Button><br>
                                                            <br>
                                                            <asp:Button ID="btnUnAssign" runat="server" CssClass="StandardButton" Width="20px"
                                                                Text="<"></asp:Button>
                                                        </td>
                                                        <td align="right" height="22">
                                                            <asp:ListBox ID="lstAppAssigned" runat="server" Width="370px" Height="135px" SelectionMode="Multiple">
                                                            </asp:ListBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" height="22">
                                                            (durch drücken Strg-Taste können mehrere Einträge markiert werden)
                                                        </td>
                                                    </tr>
                                                </table>
                                                &nbsp;&nbsp;
                                                <br />
                                                <table id="tblAutorisierung" runat="server" cellspacing="0" cellpadding="0" bgcolor="white"
                                                    border="0" width="100%">
                                                    <tr>
                                                        <td align="middle" colspan="3" height="22" style="width: 100%">
                                                            Berechtigungen:
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:GridView CellPadding="2" ID="gvAutLevel" CssClass="tableMain" AutoGenerateColumns="false"
                                                                AllowPaging="false" BackColor="White" Width="100%" runat="server">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="White" CssClass="GridTableHead" />
                                                                <PagerStyle Font-Size="12pt" Font-Bold="True" HorizontalAlign="Left" Wrap="False">
                                                                </PagerStyle>
                                                                <PagerSettings Mode="Numeric" Position="Top" />
                                                                <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                                <Columns>
                                                                    <asp:TemplateField Visible="False">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAppID" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.AppID"))%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField HeaderText="Anwendung" DataField="AppFriendlyName" />
                                                                    <asp:TemplateField HeaderText="mit Autorisierung" ItemStyle-HorizontalAlign="Center"
                                                                        HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkWithAut" runat="server" Checked='<%# (DataBinder.Eval(Container, "DataItem.WithAuthorization")) = 1 %>' />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="kein Übersteuern" ItemStyle-HorizontalAlign="Center"
                                                                        HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:RadioButton ID="rbLevel0" runat="server" Checked='<%# (DataBinder.Eval(Container, "DataItem.LevelAppToGroup")) = 0 %>'
                                                                                GroupName="Levels" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Level 1" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:RadioButton ID="rbLevel1" runat="server" Checked='<%# (DataBinder.Eval(Container, "DataItem.LevelAppToGroup")) = 1 %>'
                                                                                GroupName="Levels" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Level 2" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:RadioButton ID="rbLevel2" runat="server" Checked='<%# (DataBinder.Eval(Container, "DataItem.LevelAppToGroup")) = 2 %>'
                                                                                GroupName="Levels" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Level 3" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:RadioButton ID="rbLevel3" runat="server" Checked='<%# (DataBinder.Eval(Container, "DataItem.LevelAppToGroup")) = 3 %>'
                                                                                GroupName="Levels" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Level 4" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:RadioButton ID="rbLevel4" runat="server" Checked='<%# (DataBinder.Eval(Container, "DataItem.LevelAppToGroup")) = 4 %>'
                                                                                GroupName="Levels" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Level 5" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:RadioButton ID="rbLevel5" runat="server" Checked='<%# (DataBinder.Eval(Container, "DataItem.LevelAppToGroup")) = 5 %>'
                                                                                GroupName="Levels" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Level 6" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:RadioButton ID="rbLevel6" runat="server" Checked='<%# (DataBinder.Eval(Container, "DataItem.LevelAppToGroup")) = 6 %>'
                                                                                GroupName="Levels" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <table id="tblArchiv" cellspacing="0" cellpadding="0" bgcolor="white" border="0">
                                                    <tr>
                                                        <td align="middle" colspan="3" height="22">
                                                            Archive
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td height="22">
                                                            nicht zugewiesen
                                                        </td>
                                                        <td align="right" height="22">
                                                        </td>
                                                        <td align="left" height="22">
                                                            zugewiesen
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td height="22">
                                                            <asp:ListBox ID="lstArchivUnAssigned" runat="server" Width="370px" Height="135px"
                                                                SelectionMode="Multiple"></asp:ListBox>
                                                        </td>
                                                        <td align="right" height="22">
                                                            <asp:Button ID="btnAssignArchiv" runat="server" CssClass="StandardButton" Width="20px"
                                                                Text=">"></asp:Button><br>
                                                            <br />
                                                            <asp:Button ID="btnUnAssignArchiv" runat="server" CssClass="StandardButton" Width="20px"
                                                                Text="<"></asp:Button>
                                                        </td>
                                                        <td align="right" height="22">
                                                            <asp:ListBox ID="lstArchivAssigned" runat="server" Width="370px" Height="135px" SelectionMode="Multiple">
                                                            </asp:ListBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" height="22">
                                                            (durch drücken Strg-Taste können mehrere Einträge markiert werden)
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <table id="tblAbrufgruendeEndg" runat="server" cellspacing="0" cellpadding="0" bgcolor="white"
                                                    border="0">
                                                    <tr>
                                                        <td align="center" colspan="3" height="22">
                                                            <span lang="de">Abrufgründe endgültig</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td height="22">
                                                            nicht zugewiesen
                                                        </td>
                                                        <td align="right" height="22">
                                                        </td>
                                                        <td align="left" height="22">
                                                            zugewiesen
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td height="22">
                                                            <asp:ListBox ID="lbxAbrufgruendeEndgUnAssign" runat="server" Width="370px" Height="135px"
                                                                SelectionMode="Multiple"></asp:ListBox>
                                                        </td>
                                                        <td align="right" height="22">
                                                            <asp:Button ID="btnAssignAbrufgrundEndg" runat="server" CssClass="StandardButton"
                                                                Width="20px" Text=">"></asp:Button><br />
                                                            <br />
                                                            <asp:Button ID="btnUnAssignAbrufgrundEndg" runat="server" CssClass="StandardButton"
                                                                Width="20px" Text="<"></asp:Button>
                                                        </td>
                                                        <td align="right" height="22">
                                                            <asp:ListBox ID="lbxAbrufgruendeEndgAssign" runat="server" Width="370px" Height="135px"
                                                                SelectionMode="Multiple"></asp:ListBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" height="22">
                                                            (durch drücken Strg-Taste können mehrere Einträge markiert werden)
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table id="tblAbrufgruendeTemp" runat="server" cellspacing="0" cellpadding="0" bgcolor="white"
                                                    border="0">
                                                    <tr>
                                                        <td align="center" colspan="3" height="22">
                                                            <span lang="de">Abrufgründe temporär</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td height="22">
                                                            nicht zugewiesen
                                                        </td>
                                                        <td align="right" height="22">
                                                        </td>
                                                        <td align="left" height="22">
                                                            zugewiesen
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td height="22">
                                                            <asp:ListBox ID="lbxAbrufgruendeTempUnAssign" runat="server" Width="370px" Height="135px"
                                                                SelectionMode="Multiple"></asp:ListBox>
                                                        </td>
                                                        <td align="right" height="22">
                                                            <asp:Button ID="btnAssignAbrufgrundTemp" runat="server" CssClass="StandardButton"
                                                                Width="20px" Text=">"></asp:Button><br />
                                                            <br />
                                                            <asp:Button ID="btnUnAssignAbrufgrundTemp" runat="server" CssClass="StandardButton"
                                                                Width="20px" Text="<"></asp:Button>
                                                        </td>
                                                        <td align="right" height="22">
                                                            <asp:ListBox ID="lbxAbrufgruendeTempAssign" runat="server" Width="370px" Height="135px"
                                                                SelectionMode="Multiple"></asp:ListBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" height="22">
                                                            (durch drücken Strg-Taste können mehrere Einträge markiert werden)
                                                        </td>
                                                    </tr>
                                                </table>
                                                &nbsp;&nbsp;
                                                <br />
                                                <table id="tblEmployee" cellspacing="0" cellpadding="0" bgcolor="white" border="0"
                                                    style="display: none" visible="false">
                                                    <tr>
                                                        <td align="center" colspan="3" height="22">
                                                            Verantwortliche Mitarbeiter
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td height="22">
                                                            nicht zugewiesen
                                                        </td>
                                                        <td align="right" height="22">
                                                        </td>
                                                        <td align="left" height="22">
                                                            zugewiesen
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td height="22">
                                                            <asp:ListBox ID="lstEmployeeUnAssigned" runat="server" Width="370px" Height="135px"
                                                                SelectionMode="Multiple"></asp:ListBox>
                                                        </td>
                                                        <td align="right" height="22">
                                                            <asp:Button ID="btnAssignEmployee" runat="server" CssClass="StandardButton" Width="20px"
                                                                Text=">"></asp:Button><br />
                                                            <br />
                                                            <asp:Button ID="btnUnAssignEmployee" runat="server" CssClass="StandardButton" Width="20px"
                                                                Text="<"></asp:Button>
                                                        </td>
                                                        <td align="right" height="22">
                                                            <asp:ListBox ID="lstEmployeeAssigned" runat="server" Width="370px" Height="135px"
                                                                SelectionMode="Multiple"></asp:ListBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" height="22">
                                                            (durch drücken Strg-Taste können mehrere Einträge markiert werden)
                                                        </td>
                                                    </tr>
                                                </table>
                                                &nbsp;&nbsp;
                                                <br>
                                                Startmeldung:<br />
                                                <asp:TextBox ID="txtMessage" runat="server" Width="533px" MaxLength="500" Height="75px"
                                                    Rows="5" TextMode="MultiLine" CausesValidation="false"></asp:TextBox>
                                                <br />
                                                <asp:TextBox ID="txtMessageOld" runat="server" Visible="False" Width="631px" MaxLength="500"
                                                    TextMode="MultiLine"></asp:TextBox><br />
                                                Häufigkeit der Startmeldungsanzeige (pro Benutzer):&nbsp;&nbsp;
                                                <asp:TextBox ID="txtMaxReadMessageCount" runat="server" Width="40px" MaxLength="2"></asp:TextBox>&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                            <td></td>
                                <td align="left" height="25">
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Label ID="lblMessage" runat="server" CssClass="TextExtraLarge" EnableViewState="False"></asp:Label><asp:Label
                        ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <!--#include File="../PageElements/Footer.html" -->
                </td>
            </tr>
        </tbody>
    </table>
    <%--</TD></TR></TBODY></TABLE>--%></form>
</body>
</html>
