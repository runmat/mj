<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UserManagement.aspx.vb"
    Inherits="CKG.Admin.UserManagement" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="DAD DEUTSCHER AUTO DIENST GmbH" name="Copyright">
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
    <style type="text/css">
        .style1
        {
            height: 22px;
        }
    </style>
</head>
<body leftmargin="0" topmargin="0">
    <form id="Form1" method="post" runat="server">
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
                        <tr>
                            <td class="PageNavigation" colspan="2">
                                Administration (Benutzerverwaltung)
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
                                        <td valign="middle">
                                            <asp:LinkButton ID="lbtnNew" runat="server" CssClass="StandardButton"> &#149;&nbsp;Neuen Benutzer anlegen</asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle">
                                            <asp:LinkButton ID="lbtnNotApproved" runat="server" CssClass="StandardButton"> &#149;&nbsp;Nicht freigegebene Benutzer</asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle">
                                            <asp:LinkButton ID="lbtnApprove" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Benutzer freigeben</asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" width="150">
                                            <asp:LinkButton ID="lbtnSave" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Speichern</asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" width="150">
                                            <asp:LinkButton ID="lbtnCancel" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Verwerfen</asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" width="150">
                                            <asp:LinkButton ID="lbtnDistrict" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Distrikt - zuordnung</asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" width="150">
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
                                                <asp:HyperLink ID="lnkGroupManagement" runat="server" CssClass="TaskTitle" NavigateUrl="GroupManagement.aspx">Gruppenverwaltung</asp:HyperLink><asp:HyperLink
                                                    ID="lnkOrganizationManagement" runat="server" CssClass="TaskTitle" NavigateUrl="OrganizationManagement.aspx">Organisationsverwaltung</asp:HyperLink><asp:HyperLink
                                                        ID="lnkCustomerManagement" runat="server" CssClass="TaskTitle" NavigateUrl="CustomerManagement.aspx">Kundenverwaltung</asp:HyperLink>
                                                <asp:HyperLink ID="lnkAppManagement" runat="server" CssClass="TaskTitle" NavigateUrl="AppManagement.aspx"
                                                    Visible="False">Anwendungsverwaltung</asp:HyperLink>
                                                <asp:HyperLink ID="lnkArchivManagement" runat="server" CssClass="TaskTitle" NavigateUrl="ArchivManagement.aspx">Archivverwaltung</asp:HyperLink>&nbsp;
                                            </td>
                                        </tr>
                                        <tr id="trSearch" runat="server">
                                            <td align="left" height="182">
                                                <table bgcolor="white" border="0" runat="server">
                                                                                                     
                                                    <tr>
                                                        <td valign="bottom" width="100">
                                                            Firma:
                                                        </td>
                                                        <td valign="bottom" width="160">
                                                            <asp:Label ID="lblCustomer" runat="server" CssClass="InfoBoxFlat" Width="160px"></asp:Label><asp:DropDownList
                                                                ID="ddlFilterCustomer" runat="server" Visible="False" Width="160px" AutoPostBack="True"
                                                                Height="20px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            &nbsp;&nbsp; &nbsp;&nbsp;
                                                        </td>
                                                        <td valign="bottom">
                                                            Nur angemeldete Benutzer
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkAngemeldet" runat="server"></asp:CheckBox>
                                                        </td>
                                                    </tr>
                                                    <tr id="trSelectOrganization" runat="server">
                                                        <td valign="bottom" width="100">
                                                            <p>
                                                                Organisation:</p>
                                                        </td>
                                                        <td valign="bottom" width="160">
                                                            <asp:Label ID="lblOrganization" runat="server" CssClass="InfoBoxFlat" Visible="False"
                                                                Width="160px"></asp:Label><asp:DropDownList ID="ddlFilterOrganization" runat="server"
                                                                    Width="160px" Height="20px">
                                                                </asp:DropDownList>
                                                        </td>
                                                        <td>&nbsp;&nbsp; &nbsp;&nbsp;</td>
                                                        <td valign="bottom" id="td_EmployeeDisplay1">
                                                            Nur Mitarbeiter:
                                                        </td>
                                                        <td id="td_EmployeeDisplay2">
                                                            <asp:CheckBox ID="chkEmployeeDisplay" runat="server"></asp:CheckBox>
                                                        </td>
                                                    </tr>
                                                    <tr  runat="server">
                                                        <td valign="bottom" width="100">
                                                            Gruppe:
                                                        </td>
                                                        <td valign="bottom" width="160">
                                                            <asp:Label ID="lblGroup" runat="server" CssClass="InfoBoxFlat" Visible="False" Width="160px"></asp:Label><asp:DropDownList
                                                                ID="ddlFilterGroup" runat="server" Width="160px" Height="20px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            &nbsp;&nbsp; &nbsp;&nbsp;
                                                        </td>
                                                        <td valign="bottom" id="td_OnlyDisabledUser1">
                                                            Nur gesperrte Benutzer:
                                                        </td>
                                                        <td id="td_OnlyDisabledUser2">
                                                            <asp:CheckBox ID="chkOnlyDisabledUser" runat="server"></asp:CheckBox>
                                                        </td>
                                                    </tr>
                                                    <tr runat="server">
                                                       
                                                        <td valign="bottom" width="100">
                                                            Benutzername:
                                                        </td>
                                                        <td valign="bottom" width="160">
                                                            <asp:TextBox ID="txtFilterUserName" runat="server" Width="160px" Height="20px">*</asp:TextBox>
                                                            </td>
                                                        <td>
                                                            <asp:ImageButton ID="btnEmpty" runat="server"  ImageUrl="../images/empty.gif" 
                                                                Height="16px" />
                                                        </td>
                                                        <td valign="bottom" id="td_LastLoginBefore1">
                                                            letzter Login älter als:
                                                        </td>
                                                        <td id="td_LastLoginBefore2">
                                                            <asp:TextBox ID="txtLastLoginBefore" runat="server"></asp:TextBox>
                                                            &nbsp;<span lang="de"><asp:ImageButton ID="imgbCalendar" runat="server" 
                                                                ImageUrl="../images/calendar.jpg" />
                                                            </span>&nbsp;<asp:Calendar ID="calLastLogin" runat="server" BorderColor="Black" BorderStyle="Solid"
                                                                CellPadding="0" Visible="False" Width="160px">
                                                                <TodayDayStyle Font-Bold="True" />
                                                                <NextPrevStyle ForeColor="White" />
                                                                <DayHeaderStyle BackColor="Silver" Font-Bold="True" />
                                                                <SelectedDayStyle BackColor="#FF8080" />
                                                                <TitleStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                                                                <WeekendDayStyle ForeColor="Silver" />
                                                                <OtherMonthDayStyle ForeColor="Silver" />
                                                            </asp:Calendar>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="bottom" width="100">
                                                            Referenz:
                                                        </td>
                                                        <td valign="bottom" width="160">
                                                            <asp:TextBox ID="txtFilterReferenz" runat="server" Width="160px" Height="20px">*</asp:TextBox>
                                                        </td>
                                                        <td valign="bottom">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                                                  <tr id="trHierarchyDisplay" runat="server">
                                                        <td valign="bottom" width="100">
                                                            Hierarchie:
                                                        </td>
                                                        <td valign="bottom" width="160">
                                                            <asp:DropDownList ID="ddlHierarchyDisplay" runat="server" Width="160px" AutoPostBack="True"
                                                                Height="20px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td valign="bottom">
                                                          
                                                        </td>
                                                    </tr>
                                                    <tr runat="server">
                                                        <td colspan="3" align="left" valign="bottom">
                                                           &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr  runat="server">
                                                        <td colspan="3" align="left" valign="bottom">
                                                            <asp:LinkButton ID="btnSuche" runat="server" CssClass="StandardButton">Suchen</asp:LinkButton>
                                                        </td>
                                                      
                                                    </tr>
                                                </table>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr width="100%">
                <td colspan="10">
                    <table cellspacing="0" cellpadding="0" width="100%" align="left" border="0">
                        <tbody>
                            <tr id="trSearchSpacer" runat="server">
                                <td width="10px">
                                    &nbsp;
                                </td>
                                <td align="left" height="25" style="font-weight: 700">
                                    &nbsp;<asp:Label ID="lblBenutzer" runat="server"></asp:Label>
                                    <br />
                                    &nbsp;&nbsp;
                                    <br />
                                    <asp:HyperLink ID="lnkExcel" runat="server" Visible="False">Excel-Download: 
                                                        rechte Maustaste -&gt; Speichern unter...</asp:HyperLink>
                                    <br />
                                    &nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr id="trSearchResult" runat="server">
                            <td width="10px">&nbsp;</td>
                                <td align="left">
                                    <asp:Label ID="lblNotApprovedMode" runat="server" Visible="False" Width="100%" ForeColor="Red"
                                        Font-Bold="True" BackColor="Transparent" BorderWidth="1px" BorderStyle="Solid"
                                        BorderColor="Red">
															<center>Freigabeliste</center>
                                    </asp:Label>
                                    <asp:DataGrid ID="dgSearchResult" runat="server" Width="100%" BackColor="White"
                                        BorderWidth="1px" BorderStyle="Solid" BorderColor="Black" AllowPaging="True"
                                        AutoGenerateColumns="False" AllowSorting="True">
                                        <SelectedItemStyle BackColor="#FFE0C0"></SelectedItemStyle>
                                        <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" CssClass="GridTableHead" VerticalAlign="Top">
                                        </HeaderStyle>
                                        <Columns>
                                            <asp:BoundColumn Visible="False" DataField="UserID" SortExpression="UserID" HeaderText="UserID">
                                            </asp:BoundColumn>
                                            <asp:ButtonColumn DataTextField="UserName" SortExpression="UserName" HeaderText="Benutzername" CommandName="Edit">  
                                            </asp:ButtonColumn>
                                            <asp:TemplateColumn SortExpression="CustomerName" HeaderText="col_CustomerName">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_CustomerName" CommandArgument="CustomerName" CommandName="Sort"
                                                        runat="server">col_CustomerName</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label51" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CustomerName") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn SortExpression="AccountingArea" HeaderText="col_AccountingArea">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_AccountingArea" CommandArgument="AccountingArea" CommandName="Sort"
                                                        runat="server">col_AccountingArea</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label51x" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AccountingArea") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn DataField="Reference" SortExpression="Reference" HeaderText="Kunden-&lt;br&gt;referenz">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="GroupName" SortExpression="GroupName" HeaderText="Gruppe">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="OrganizationName" SortExpression="OrganizationName" HeaderText="Organisation">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="mail" SortExpression="mail" HeaderText="EMail">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="telephone" SortExpression="telephone" HeaderText="Telefon">
                                            </asp:BoundColumn>
                                            <asp:TemplateColumn SortExpression="CustomerAdmin" HeaderText="Firmen-&lt;br&gt;Admin">
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cbxSRCustomerAdmin" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container.DataItem, "CustomerAdmin") %>'>
                                                    </asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn SortExpression="TestUser" HeaderText="Test">
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cbxSRTestUser" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "TestUser") %>'
                                                        Enabled="False"></asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn DataField="LastPwdChange" SortExpression="LastPwdChange" HeaderText="letzte&lt;br&gt;Kennwort&#228;nderung">
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="LastLogin" SortExpression="LastLogin" HeaderText="letztes Login">
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:TemplateColumn SortExpression="PwdNeverExpires" HeaderText="Kennwort&lt;br&gt;l&#228;uft nicht ab">
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cbxSRPwdNeverExpires" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "PwdNeverExpires") %>'
                                                        Enabled="False"></asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn DataField="FailedLogins" SortExpression="FailedLogins" HeaderText="Anmelde-&lt;br&gt;Fehlversuche">
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:TemplateColumn SortExpression="AccountIsLockedOut" HeaderText="Konto&lt;br&gt;gesperrt">
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cbxSRAccountIsLockedOut" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "AccountIsLockedOut") %>'
                                                        Enabled="False"></asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn SortExpression="LoggedOn" HeaderText="Angemeldet">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSRLoggedOn" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.LoggedOn") %>'>
                                                    </asp:CheckBox>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LoggedOn") %>'>
                                                    </asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="l&#246;schen">
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ibtnSRDelete" runat="server" CausesValidation="false" CommandName="Delete"
                                                        ImageUrl="../Images/icon_nein_s.gif"></asp:ImageButton>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn Visible="False" DataField="CreatedBy" SortExpression="CreatedBy"
                                                HeaderText="CreatedBy"></asp:BoundColumn>
                                              
                                        </Columns>
                                        <PagerStyle Mode="NumericPages"></PagerStyle>
                                    </asp:DataGrid>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
                          
            <tr>
                <td>
                    <table id="Tablex1" cellspacing="0" cellpadding="0" width="100%" border="0">
                      
                        <tr>
                            <td width="120">
                                &nbsp;
                            </td>
                            
                            
                            
                            <td>
                                <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                    border="0">
                                    <tbody>
                                        <tr>
                                            <td align="left">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                            </td>
                                        </tr>
                                        <tr id="trEditUser" runat="server">
                                            <td align="left">
                                                <table width="740" border="0">
                                                    <tr>
                                                        <td valign="top" align="left">
                                                            <table id="tblLeft" cellspacing="2" cellpadding="0" width="345" bgcolor="white" border="0">
                                                                 
                                                                
                                                                <tr>
                                                                    <td height="22">
                                                                        Benutzername:
                                                                    </td>
                                                                    <td height="22">
                                                                        <asp:TextBox ID="txtUserName" runat="server" Width="160px" Height="20px"></asp:TextBox><asp:TextBox
                                                                            ID="txtUserID" runat="server" Visible="False" Width="10px" Height="10px" ForeColor="#CEDBDE"
                                                                            BackColor="#CEDBDE" BorderWidth="0px" BorderStyle="None">-1</asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trAnrede" runat="server">
                                                                    <td height="22">
                                                                        Anrede:
                                                                    </td>
                                                                    <td height="22">
                                                                        <asp:DropDownList ID="ddlTitle" runat="server" Width="160px" AutoPostBack="True"
                                                                            Height="20px">
                                                                            <asp:ListItem Value="-" Selected="True">&lt;Bitte ausw&#228;hlen&gt;</asp:ListItem>
                                                                            <asp:ListItem Value="Herr">Herr</asp:ListItem>
                                                                            <asp:ListItem Value="Frau">Frau</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trVorname" runat="server">
                                                                    <td height="22">
                                                                        Vorname:
                                                                    </td>
                                                                    <td height="22">
                                                                        <asp:TextBox ID="txtFirstName" runat="server" Width="160px" Height="20px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trNachname" runat="server">
                                                                    <td height="22">
                                                                        Nachname:
                                                                    </td>
                                                                    <td height="22">
                                                                        <asp:TextBox ID="txtLastName" runat="server" Width="160px" Height="20px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td height="22">
                                                                        Kundenreferenz:
                                                                    </td>
                                                                    <td height="22">
                                                                        <asp:TextBox ID="txtReference" runat="server" Width="160px" Height="20px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td height="22">
                                                                        Filiale:
                                                                    </td>
                                                                    <td height="22">
                                                                        <asp:TextBox ID="txtStore" runat="server" Width="160px" Height="20px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trTestUser" runat="server">
                                                                    <td height="22">
                                                                        Test-Zugang:
                                                                    </td>
                                                                    <td height="22">
                                                                        <asp:CheckBox ID="cbxTestUser" runat="server"></asp:CheckBox>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trCustomer" runat="server">
                                                                    <td height="22">
                                                                        Firma:
                                                                    </td>
                                                                    <td height="22">
                                                                        <asp:DropDownList ID="ddlCustomer" runat="server" Width="160px" AutoPostBack="True"
                                                                            Height="20px">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td height="22">
                                                                        Gültig ab:</td>
                                                                    <td height="22">
                                                                        <asp:TextBox ID="txtValidFrom" runat="server" Width="160px" Height="20px" 
                                                                            MaxLength="10"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trCustomerAdmin0" runat="server">
                                                                    <td height="22">
                                                                        <b>Firmenadministration</b>
                                                                    </td>
                                                                    <td height="22">
                                                                        &nbsp;&nbsp; &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr id="trCustomerAdmin1" runat="server">
                                                                    <td height="22">
                                                                        Keine Berechtigung
                                                                    </td>
                                                                    <td height="22">
                                                                        <asp:RadioButton ID="cbxNoCustomerAdmin" runat="server" Checked="True" GroupName="grpFirmenadministration" AutoPostBack="true"/>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trCustomerAdmin2" runat="server">
                                                                    <td height="22">
                                                                        Einzelner Kunde
                                                                    </td>
                                                                    <td height="22">
                                                                        <asp:RadioButton ID="cbxCustomerAdmin" runat="server" GroupName="grpFirmenadministration"  AutoPostBack="true" />
                                                                    </td>
                                                                </tr>
                                                                <tr id="trCustomerAdmin3" runat="server">
                                                                    <td height="22">
                                                                        Firstlevel-Administrator
                                                                    </td>
                                                                    <td height="22">
                                                                        <asp:RadioButton ID="cbxFirstLevelAdmin" runat="server" GroupName="grpFirmenadministration" AutoPostBack="true" />
                                                                    </td>
                                                                </tr>
                                                                <tr id="trCustomerAdmin" runat="server">
                                                                    <td height="22">
                                                                        &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
                                                                    </td>
                                                                    <td height="22">
                                                                        &nbsp;&nbsp;&nbsp; &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr id="trGroup" runat="server">
                                                                    <td height="22">
                                                                        Gruppe:
                                                                    </td>
                                                                    <td height="22">
                                                                        <asp:DropDownList ID="ddlGroups" runat="server" Width="160px" Height="20px">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trOrganization" runat="server">
                                                                    <td height="22">
                                                                        Organisation:
                                                                    </td>
                                                                    <td height="22">
                                                                        <asp:DropDownList ID="ddlOrganizations" runat="server" Width="160px" Height="20px">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trOrganizationAdministrator" runat="server">
                                                                    <td height="22">
                                                                        Organisationadministrator:
                                                                    </td>
                                                                    <td height="22">
                                                                        <asp:CheckBox ID="cbxOrganizationAdmin" runat="server"  AutoPostBack="true"></asp:CheckBox>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trMail" runat="server">
                                                                    <td height="22">
                                                                        E-Mail (x@y.z):
                                                                    </td>
                                                                    <td height="22">
                                                                        <asp:TextBox ID="txtMail" runat="server" Width="160px" MaxLength="75"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trPhone" runat="server">
                                                                    <td height="22">
                                                                       Telefon:
                                                                    </td>
                                                                    <td height="22">
                                                                        <asp:TextBox ID="txtPhone" runat="server" Width="160px" MaxLength="75" ></asp:TextBox>
                                                                    </td>
                                                                </tr>                                                                
                                                                          <tr>
                                                                    <td height="22">
                                                                        Benutzerhistorie:
                                                                    </td>
                                                                    <td height="22">
                                                                        <asp:HyperLink ID="hlUserHistory" runat="server" Target="UserHist">Anzeigen</asp:HyperLink>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:TextBox ID="txtPasswordAutoBAK" runat="server" Visible="False" Width="160px"
                                                                Enabled="False" ReadOnly="True"></asp:TextBox><asp:TextBox ID="txtPAsswordConfirmAutoBAK"
                                                                    runat="server" Visible="False" Width="160px" Enabled="False" ReadOnly="True"></asp:TextBox>
                                                        </td>
                                                        <td width="100%">
                                                        </td>
                                                        <td valign="top" align="right">
                                                            <table id="tblRight" cellspacing="2" cellpadding="0" width="345" bgcolor="white"
                                                                border="0">
                                                                <tr>
                                                                    <td height="22">
                                                                        letzte Kennwortänderung:
                                                                    </td>
                                                                    <td align="left" height="22">
                                                                        <asp:Label ID="lblLastPwdChange" runat="server" CssClass="InfoBoxFlat" Width="160px"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trPwdNeverExpires" runat="server">
                                                                    <td height="22">
                                                                        Kennwort läuft nie ab:
                                                                    </td>
                                                                    <td align="left" height="22">
                                                                        <asp:CheckBox ID="cbxPwdNeverExpires" runat="server"></asp:CheckBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td height="22">
                                                                        fehlgeschlagene Anmeldungen:
                                                                    </td>
                                                                    <td align="left" height="22">
                                                                        <asp:Label ID="lblFailedLogins" runat="server" CssClass="InfoBoxFlat" Width="160px"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td height="22">
                                                                        Konto gesperrt:
                                                                    </td>
                                                                    <td align="left" height="22">
                                                                        <asp:CheckBox ID="cbxAccountIsLockedOut" runat="server"></asp:CheckBox><asp:CheckBox
                                                                            ID="cbxApproved" runat="server" Visible="False"></asp:CheckBox><asp:Label ID="lblLockedBy" runat="server" visible="false"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td height="22">
                                                                        Angemeldet:
                                                                    </td>
                                                                    <td align="left" height="22">
                                                                        <asp:CheckBox ID="chkLoggedOn" runat="server"></asp:CheckBox>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trMatrix" runat="server" visible="False">
                                                                    <td height="22">
                                                                        Matrix gefüllt:
                                                                    </td>
                                                                    <td align="left" height="22">
                                                                        <asp:CheckBox ID="chk_Matrix1" runat="server" AutoPostBack="True"></asp:CheckBox>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trReadMessageCount" runat="server">
                                                                    <td height="22">
                                                                        Anzahl der<br>
                                                                        Startmeldungs-Anzeigen:
                                                                    </td>
                                                                    <td valign="top" align="left" height="22">
                                                                        <asp:TextBox ID="txtReadMessageCount" runat="server" Width="40px" MaxLength="2"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trNewPassword" runat="server">
                                                                    <td height="22">
                                                                        Neues Passwort setzen:
                                                                    </td>
                                                                    <td nowrap="nowrap" align="left" height="22">
                                                                        <asp:CheckBox ID="chkNewPasswort" runat="server"></asp:CheckBox>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trPassword" runat="server">
                                                                    <td height="22">
                                                                        Passwort:
                                                                    </td>
                                                                    <td nowrap="nowrap" align="left" height="22">
                                                                        <asp:TextBox ID="txtPassword" runat="server" Visible="true" Width="160px" TextMode="Password"></asp:TextBox><asp:LinkButton
                                                                            ID="btnCreatePassword" runat="server" CssClass="StandardButtonTable" Visible="False">Kennwort generieren</asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trConfirmPassword" runat="server">
                                                                    <td height="22">
                                                                        Passwort bestätigen:
                                                                    </td>
                                                                    <td align="left" height="22">
                                                                        <asp:TextBox ID="txtConfirmPassword" runat="server" Visible="true" Width="160px"
                                                                            TextMode="Password"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td height="22">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td align="left" height="22">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td height="22">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td align="left" height="22">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr id="trEmployee01" runat="server">
                                                                    <td height="22" bgcolor="Silver">
                                                                        Benutzer ist Mitarbeiter:
                                                                    </td>
                                                                    <td align="left" height="22" bgcolor="Silver">
                                                                        <asp:CheckBox ID="chkEmployee" runat="server"></asp:CheckBox>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trEmployee02" runat="server">
                                                                    <td height="22" bgcolor="Silver">
                                                                        Hierarchie:
                                                                    </td>
                                                                    <td align="left" height="22" bgcolor="Silver">
                                                                        <asp:DropDownList ID="ddlHierarchy" runat="server" Width="160px" AutoPostBack="True"
                                                                            Height="20px">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trEmployee03" runat="server">
                                                                    <td height="22" bgcolor="Silver">
                                                                        Abteilung:
                                                                    </td>
                                                                    <td align="left" height="22" bgcolor="Silver">
                                                                        <asp:TextBox ID="txtDepartment" runat="server" Width="160px" Height="20px" MaxLength="75"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trEmployee04" runat="server">
                                                                    <td height="22" bgcolor="Silver">
                                                                        Position:
                                                                    </td>
                                                                    <td align="left" height="22" bgcolor="Silver">
                                                                        <asp:TextBox ID="txtPosition" runat="server" Width="160px" Height="20px" MaxLength="75"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trEmployee05" runat="server">
                                                                    <td bgcolor="Silver" class="style1">
                                                                        Telefon:
                                                                    </td>
                                                                    <td align="left" bgcolor="Silver" class="style1">
                                                                        <asp:TextBox ID="txtTelephone" runat="server" Width="160px" Height="20px" MaxLength="75"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trEmployee06" runat="server">
                                                                    <td height="22" bgcolor="Silver">
                                                                        Telefax:
                                                                    </td>
                                                                    <td align="left" height="22" bgcolor="Silver">
                                                                        <asp:TextBox ID="txtFax" runat="server" Width="160px" Height="20px" MaxLength="75"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trEmployee07" runat="server">
                                                                    <td height="22" bgcolor="Silver">
                                                                        <asp:Label ID="lblPicture" runat="server" BackColor="Silver">Bild hochladen</asp:Label>
                                                                        :<br />
                                                                        <asp:Image ID="Image1" runat="server" Height="150px" Width="100px" />
                                                                    </td>
                                                                    <td align="left" height="22" bgcolor="Silver">
                                                                        <asp:Label ID="lblPictureName" runat="server" BackColor="Silver"></asp:Label>
                                                                        <br />
                                                                        <strong>
                                                                            <input id="upFile" runat="server" name="File1" size="9" type="file" /><br />
                                                                        </strong>
                                                                        <asp:LinkButton ID="btnUpload" runat="server" CssClass="StandardButtonTable" Width="95px">•&nbsp;Hinzufügen</asp:LinkButton>
                                                                        &nbsp;&nbsp;
                                                                        <asp:LinkButton ID="btnRemove" runat="server" CssClass="StandardButtonTable" Width="95px">•&nbsp;Entfernen</asp:LinkButton><br />
                                                                        &nbsp;<br />
                                                                        Darstellung auf 150 x 100 Pixeln.
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" align="left">
                                                        </td>
                                                        <td width="100%">
                                                        </td>
                                                        <td valign="top" align="right">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" align="left">
                                                        </td>
                                                        <td width="100%">
                                                        </td>
                                                        <td valign="top" align="right">
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:Table ID="Matrix" runat="server" Visible="False">
                                                </asp:Table>
                                            </td>
                                        </tr>
                                        <tr>
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
                                    <br />
                                           <asp:Label ID="lblKundenadministrationInfo" runat="server" CssClass="textError" ></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <!--#include File="../PageElements/Footer.html" -->
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
    </form>

</body>
</html>
