<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UserManagement.aspx.vb"
    Inherits="Admin.UserManagement" MasterPageFile="MasterPage/Admin.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="/PortalZLD/PageElements/GridNavigation.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="innerContent">
                    <div id="navigationSubmenu">
                        <asp:HyperLink ID="lnkGroupManagement" runat="server" ToolTip="Gruppen" 
                            NavigateUrl="GroupManagement.aspx" Text="Gruppen | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkOrganizationManagement" runat="server" ToolTip="Organisationen"
                            NavigateUrl="OrganizationManagement.aspx" Text="Organisation | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkCustomerManagement" ToolTip="Kunden"  runat="server"
                            NavigateUrl="CustomerManagement.aspx" Text="Kunden | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkArchivManagement" ToolTip="Archive" runat="server" 
                            NavigateUrl="ArchivManagement.aspx" Text="Archive | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkAppManagement" runat="server" ToolTip="Anwendungen" 
                            NavigateUrl="AppManagement.aspx" Text="Anwendungen"></asp:HyperLink>
                    </div>
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                        </div>
                     
                        <asp:Panel ID="DivSearch" DefaultButton="btnEmpty" runat="server">
                        
                        <div id="TableQuery">
                            <table id="tableSearch" runat="server" cellspacing="0" cellpadding="0">
                                <tbody>
                                    <tr class="formquery">
                                        <td class="firstLeft active" colspan="4">
                                            <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                            <asp:Label ID="lblMessage" runat="server" CssClass="TextExtraLarge" EnableViewState="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Firma:
                                        </td>
                                        <td class="firstLeft activ" nowrap="nowrap">
                                            <asp:DropDownList ID="ddlFilterCustomer" runat="server" AutoPostBack="True" 
                                                Font-Names="Verdana,sans-serif" Visible="False" Width="260px">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblCustomer" runat="server"></asp:Label>
                                        </td>
                                        <td class="secondLeft active" nowrap="nowrap" id="tdHierarchyDisplay1">
                                            Hierarchie:
                                        </td>
                                        <td class="rightPadding" id="tdHierarchyDisplay2" >
                                            <asp:DropDownList ID="ddlHierarchyDisplay" runat="server" AutoPostBack="True"
                                                Width="260px" Font-Names="Verdana,sans-serif">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr class="formquery" id="trSelectOrganization" runat="server">
                                        <td class="firstLeft active">
                                            Organisation:
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:DropDownList ID="ddlFilterOrganization" runat="server" Width="260px"
                                                Font-Names="Verdana,sans-serif">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblOrganization" runat="server" Visible="False"></asp:Label>
                                        </td>
                                        <td id="td_EmployeeDisplay" valign="bottom" runat="server" class="secondLeft active">
                                            Nur angemeldete Benutzer:&nbsp;
                                        </td>
                                        <td id="td_EmployeeDisplay2" runat="server" class="rightPadding">
                                            <asp:CheckBox ID="chkAngemeldet" runat="server" Width="25px" />
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Gruppe:
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:DropDownList ID="ddlFilterGroup" runat="server" Width="260px"
                                                Font-Names="Verdana,sans-serif">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblGroup" runat="server" Visible="False"></asp:Label>
                                        </td>
                                        <td class="secondLeft active" id="td_EmployeeDisplay1" runat="server">
                                            Nur Mitarbeiter:
                                        </td>
                                        <td id="td_EmployeeDisplay3" class="rightPadding" runat="server">
                                            <asp:CheckBox ID="chkEmployeeDisplay" runat="server" Width="25px"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Benutzername:
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtFilterUserName" runat="server" CssClass="InputTextbox" Width="257px">*</asp:TextBox>
                                            <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="/PortalZLD/images/empty.gif"
                                                Width="1px" />
                                        </td>
                                        <td class="secondLeft active" id="td_OnlyDisabledUser1" runat="server">
                                            Nur gesperrte Benutzer:&nbsp;
                                        </td>
                                        <td class="rightPadding" id="td_OnlyDisabledUser2" runat="server">
                                            <asp:CheckBox ID="chkOnlyDisabledUser" runat="server" Width="25px" />
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" >
                                            Referenz:
                                        </td>
                                        <td class="firstLeft activ">
                                            <asp:TextBox ID="txtFilterReferenz" runat="server" CssClass="InputTextbox" Width="257px">*</asp:TextBox>
                                        </td>
                                        <td class="secondLeft active" id="td_LastLoginBefore1">
                                            letzter Login älter als:
                                        </td>
                                        <td class="rightPadding" id="td_LastLoginBefore2" style="width: 100%">
                                            <asp:TextBox ID="txtLastLoginBefore" runat="server" CssClass="InputTextbox" MaxLength="10"
                                                ToolTip="" Width="260px" />
                                            <ajaxToolkit:CalendarExtender ID="defaultCalendarExtenderVon" runat="server" TargetControlID="txtLastLoginBefore" />
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                        </td>
                                        <td style="width: 35%">
                                            &nbsp;</td>
                                        <td style="width: 50%">
                                        </td>
                                        <td style="width: 50%">
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                        <td align="right" colspan="3" nowrap="nowrap" class="rightPadding">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr style="background-color: #dfdfdf; height: 22px">
                                        <td colspan="4">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <div id="QueryFooter" runat="server">
                                <div id="dataQueryFooter">
                                    &nbsp;<asp:LinkButton ID="lbtnNotApproved" runat="server" Text="Nicht freiggb. Benutz.&amp;nbsp;&amp;#187; "
                                        CssClass="TablebuttonXLarge" Height="21px" Width="155px" Font-Names="Verdana,sans-serif"></asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="lbtnNew" runat="server" Text="Neuer Benutzer&amp;nbsp;&amp;#187; "
                                        CssClass="TablebuttonXLarge" Height="21px" Width="155px" Font-Names="Verdana,sans-serif"
                                        Font-Size="10px"></asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="lbtnCancel0" runat="server" Text="Neue Suche&amp;nbsp;&amp;#187; "
                                        CssClass="TablebuttonXLarge" Height="21px" Width="155px" Font-Names="Verdana,sans-serif"
                                        Font-Size="10px" Visible="False"></asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="btnSuche" runat="server" Text="Suchen&amp;nbsp;&amp;#187; " CssClass="TablebuttonXLarge"
                                        Height="21px" Width="155px" Font-Names="Verdana,sans-serif" Font-Size="10px"></asp:LinkButton>
                                </div>
                            </div>
                        </div></asp:Panel>
                       
                        <div id="Result" runat="Server" visible="false">
                            <div class="ExcelDiv">
                                <div align="right" id="trSearchSpacer" runat="server" >
                                    <img src="/PortalZLD/Images/iconXLS.gif" alt="Excel herunterladen" />
                                    <asp:HyperLink ID="lnkExcel" runat="server" Visible="False" ForeColor="White">Excel-Download: 
                                                                        rechte Maustaste -&gt; Speichern unter...</asp:HyperLink>
                                </div>
                            </div>
                            <div id="pagination">
                                <asp:Label ID="lblNotApprovedMode" runat="server" Visible="False" Width="100%" ForeColor="#772D34"
                                    Font-Bold="True" BackColor="Transparent" BorderWidth="1px" BorderStyle="Solid"
                                    BorderColor="#772D34"> <center>Freigabeliste</center></asp:Label>
                                <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                            </div>
                            <div id="data">
                                <asp:Panel ID="Panel1" runat="server">
                                    <table cellspacing="0" style="border-color: #ffffff" cellpadding="0" align="left"
                                        border="0">
                                        <tr id="trSearchResult" runat="server">
                                            <td align="left">
                                                <asp:GridView ID="dgSearchResult"  runat="server" AllowSorting="True"
                                                    AutoGenerateColumns="False" Width="100%" CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0"
                                                    AllowPaging="True" GridLines="None" PageSize="10" EditRowStyle-Wrap="False" PagerStyle-Wrap="True"
                                                    CssClass="GridView">
                                                    <PagerSettings Visible="False" />
                                                    <PagerStyle Wrap="True" />
                                                    <HeaderStyle CssClass="GridTableHead" ></HeaderStyle>
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
                                                        <asp:ButtonField DataTextField="UserName" ItemStyle-Font-Underline="true" ItemStyle-ForeColor="#595959" SortExpression="UserName" CommandName="Edit"
                                                            HeaderText="Benutzer" />
                                                        <asp:TemplateField HeaderText="Firmenname">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_CustomerName" CssClass="TableLinkHead" CommandArgument="CustomerName"
                                                                    CommandName="Sort" runat="server">Firmenname</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label51" runat="server" CssClass="TableLabel" Text='<%# DataBinder.Eval(Container, "DataItem.CustomerName") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="AccountingArea"  CommandArgument="AccountingArea"
                                                                    CommandName="Sort" runat="server" >Buchungs- kreis</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label51x" runat="server" CssClass="TableLabel" Text='<%# DataBinder.Eval(Container, "DataItem.AccountingArea") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Reference" SortExpression="Reference" HeaderText="Referenz">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="GroupName" SortExpression="GroupName" HeaderText="Gruppe">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="OrganizationName" SortExpression="OrganizationName" HeaderText="Orga.">
                                                        </asp:BoundField>
                                                        <asp:TemplateField SortExpression="CustomerAdmin" HeaderText="Firmen- admin">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbxSRCustomerAdmin" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container.DataItem, "CustomerAdmin") %>'>
                                                                </asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="TestUser" HeaderText="Test">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkTestUser" Enabled="False" Checked='<%# DataBinder.Eval(Container.DataItem, "TestUser")%>' runat="server" />                                                                                                                                                                                                   
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="LastPwdChange" SortExpression="LastPwdChange" HeaderText="Kennwort geändert">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="LastLogin" SortExpression="LastLogin" HeaderText="letztes Login">
                                                        </asp:BoundField>
                                                        <asp:TemplateField SortExpression="PwdNeverExpires" HeaderStyle-Wrap="True" HeaderText="Kennwort läuft ab">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkNeverExpires" Enabled="False" Checked='<%# DataBinder.Eval(Container.DataItem, "PwdNeverExpires")%>' runat="server" />                                                                     
                                                            </ItemTemplate>
                                                            <HeaderStyle Wrap="True" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FailedLogins" SortExpression="FailedLogins" HeaderText="Anmeld.- Fehlvers.">
                                                        </asp:BoundField>
                                                        <asp:TemplateField SortExpression="AccountIsLockedOut" HeaderText="Konto gesperrt">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkLockedOut" Enabled="False" Checked='<%# DataBinder.Eval(Container.DataItem, "AccountIsLockedOut")%>' runat="server" />                                                                                       
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="LoggedOn" HeaderText="Angemeldet">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkLoggedOn" Enabled="False" Checked='<%# DataBinder.Eval(Container.DataItem, "LoggedOn")%>' runat="server" />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LoggedOn") %>'>
                                                                </asp:TextBox>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="URLRemoteLoginKey" SortExpression="URLRemoteLoginKey" HeaderText="RemoteLoginKey">
                                                        </asp:BoundField>
                                                        <asp:ButtonField CommandName="Del"  HeaderText="Löschen" ButtonType="Image" 
                                                            ImageUrl="/PortalZLD/Images/Papierkorb_01.gif"  ControlStyle-Height="16px" 
                                                            ControlStyle-Width="16px">
                                                            <ControlStyle Height="16px" Width="16px" />
                                                        </asp:ButtonField>
                                                        <asp:BoundField Visible="False" DataField="CreatedBy" SortExpression="CreatedBy"
                                                            HeaderText="CreatedBy"></asp:BoundField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>

                                </asp:Panel>
                            </div>
                                    <div class="dataFooter">
                                        &nbsp;
                                    </div>   
                                                           
                        </div>
                        <div id="Input" runat="server" visible="False">
                            <div id="adminInput">
                                <table id="Tablex1" runat="server"  cellspacing="0"
                                    cellpadding="0" >
                                    <tr id="trEditUser" runat="server" >
                                        <td>
                                            <table border="0" style="border-color: #ffffff">
                                                        <tr class="formquery">
                                                            <td class="firstLeft active" colspan="4">
                                                                <asp:Label ID="lblErrorSave" CssClass="TextError" runat="server"></asp:Label>
                                                                <asp:Label ID="lblMessageSave" runat="server" CssClass="TextExtraLarge" EnableViewState="False"></asp:Label>
                                                            </td>
                                                        </tr>                                              
                                                <tr >
                                                    <td >
                                                  
                                                        <table id="tblLeft" style="border-color: #ffffff; padding-right: 50px;" 
                                                            cellspacing="0" cellpadding="0">
                                                            <tr class="formquery">
                                                                <td class="firstLeft active">
                                                                    Benutzername:
                                                                </td>
                                                                <td class="active">
                                                                    <asp:TextBox ID="txtUserName" runat="server" CssClass="InputTextbox" 
                                                                        MaxLength="12"></asp:TextBox><asp:TextBox
                                                                        ID="txtUserID" runat="server" Visible="False" Width="10px" Height="10px" ForeColor="#CEDBDE"
                                                                        BackColor="#CEDBDE" BorderWidth="0px" BorderStyle="None">-1</asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="trAnrede" runat="server" class="formquery">
                                                                <td class="firstLeft active">
                                                                    Anrede:
                                                                </td>
                                                                <td class="active">
                                                                    <asp:DropDownList ID="ddlTitle" runat="server" AutoPostBack="True" 
                                                                        CssClass="DropDowns">
                                                                        <asp:ListItem Value="-" Selected="True">&lt;Bitte ausw&#228;hlen&gt;</asp:ListItem>
                                                                        <asp:ListItem Value="Herr">Herr</asp:ListItem>
                                                                        <asp:ListItem Value="Frau">Frau</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr id="trVorname" runat="server" class="formquery">
                                                                <td class="firstLeft active">
                                                                    Vorname:
                                                                </td>
                                                                <td class="active">
                                                                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="trNachname" runat="server" class="formquery">
                                                                <td class="firstLeft active">
                                                                    Nachname:
                                                                </td>
                                                                <td class="active">
                                                                    <asp:TextBox ID="txtLastName" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="trDomainUser" runat="server" class="formquery">
                                                                <td class="firstLeft active">
                                                                    Windowsbenutzer</td>
                                                                <td class="active">
                                                                   <asp:TextBox ID="txtDomainUser" runat="server" CssClass="InputTextbox"></asp:TextBox></td>
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
                                                                <td class="active">
                                                                    <asp:TextBox ID="txtStore" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="trTestUser" runat="server" class="formquery">
                                                                <td class="firstLeft active">
                                                                    Test-Zugang:
                                                                </td>
                                                                <td class="active">
                                                                    <span><asp:CheckBox ID="cbxTestUser" runat="server"></asp:CheckBox></span>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery" id="trCustomer" runat="server">
                                                                <td class="firstLeft active">
                                                                    Firma:
                                                                </td>
                                                                <td class="active">
                                                                    <asp:DropDownList ID="ddlCustomer" runat="server" AutoPostBack="True" 
                                                                        CssClass="DropDowns">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery"  runat="server">
                                                                <td class="firstLeft active">
                                                                    Gültig ab:</td>
                                                                <td class="active">
                                                                    <asp:TextBox ID="txtValidFrom" runat="server" Width="160px" MaxLength="10"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery" id="trCustomerAdmin0" runat="server">
                                                                <td class="firstLeft active">
                                                                    <b>Firmenadministration</b>
                                                                </td>
                                                                <td class="active">
                                                                    &nbsp;&nbsp; &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery" id="trCustomerAdmin1" runat="server">
                                                                <td class="firstLeft active">
                                                                    Keine Berechtigung
                                                                </td>
                                                                <td class="active">
                                                                    <span><asp:RadioButton ID="cbxNoCustomerAdmin" runat="server" Checked="True" GroupName="grpFirmenadministration" /></span>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery" id="trCustomerAdmin2" runat="server">
                                                                <td class="firstLeft active">
                                                                    Einzelner Kunde
                                                                </td>
                                                                <td class="active">
                                                                    <span><asp:RadioButton ID="cbxCustomerAdmin" runat="server" GroupName="grpFirmenadministration" /></span>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery" id="trCustomerAdmin3" runat="server">
                                                                <td class="firstLeft active">
                                                                    Administrator Filiale
                                                                </td>
                                                                <td class="active">
                                                                   <span> <asp:RadioButton ID="cbxFirstLevelAdmin" runat="server" GroupName="grpFirmenadministration" /></span>
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
                                                                    <asp:DropDownList ID="ddlOrganizations" runat="server"
                                                                        CssClass="DropDowns">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery" id="trOrganizationAdministrator" runat="server">
                                                                <td class="firstLeft active">
                                                                    Organisationadministrator:
                                                                </td>
                                                                <td class="active">
                                                                    <span><asp:CheckBox ID="cbxOrganizationAdmin" runat="server"></asp:CheckBox></span>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery" id="trMail" runat="server">
                                                                <td class="firstLeft active">
                                                                    E-Mail (x@y.z):
                                                                </td>
                                                                <td class="active">
                                                                    <asp:TextBox ID="txtMail" runat="server" MaxLength="75" CssClass="InputTextbox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery" id="trPhone" runat="server">
                                                                <td class="firstLeft active">
                                                                   Telefon:</td>
                                                                <td class="active">
                                                                    <asp:TextBox ID="txtPhone" runat="server" MaxLength="75" CssClass="InputTextbox"></asp:TextBox></td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active">
                                                                    Benutzerhistorie:
                                                                </td>
                                                                <td class="active">
                                                                    <asp:HyperLink ID="hlUserHistory" runat="server" Target="UserHist">Anzeigen</asp:HyperLink>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <asp:TextBox ID="txtPasswordAutoBAK" runat="server" Visible="False" Width="160px"
                                                            Enabled="False" ReadOnly="True"></asp:TextBox><asp:TextBox ID="txtPAsswordConfirmAutoBAK"
                                                                runat="server" Visible="False" Width="160px" Enabled="False" ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                    <td >
                                                        <table id="tblRight" cellspacing="0" style="border-color: #ffffff" cellpadding="0"
                                                            width="100%">
                                                            <tr class="formquery">
                                                                <td class="firstLeft active" nowrap="nowrap">
                                                                    letzte Kennwortänderung:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <span><asp:Label ID="lblLastPwdChange" runat="server" Width="160px" 
                                                                        CssClass="InputTextbox"></asp:Label></span>
                                                                </td>
                                                            </tr>
                                                            <tr id="trPwdNeverExpires" class="formquery" runat="server">
                                                                <td class="firstLeft active">
                                                                    Kennwort läuft nie ab:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <span><asp:CheckBox ID="cbxPwdNeverExpires" runat="server"></asp:CheckBox></span>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active" nowrap="nowrap">
                                                                    fehlgeschlagene Anmeldungen:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <asp:Label ID="lblFailedLogins" runat="server" Width="160px" 
                                                                        CssClass="InputTextbox"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active">
                                                                    Konto gesperrt:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <asp:CheckBox ID="cbxAccountIsLockedOut" runat="server" Width="25px"></asp:CheckBox>
                                                                    <asp:CheckBox ID="cbxApproved" runat="server" Visible="False" Width="25px"></asp:CheckBox>
                                                                    <asp:Label ID="lblLockedBy" runat="server" visible="false"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active">
                                                                    Angemeldet:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <asp:CheckBox ID="chkLoggedOn" runat="server" Width="25px"></asp:CheckBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="trReadMessageCount" runat="server" class="formquery">
                                                                <td class="firstLeft active" nowrap="nowrap">
                                                                    Anzahl der Startmeldungs-Anzeigen:
                                                                </td>
                                                                <td valign="top" align="left" class="active">
                                                                    <asp:TextBox ID="txtReadMessageCount" runat="server" MaxLength="2" 
                                                                        CssClass="InputTextbox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="trNewPassword" runat="server" class="formquery">
                                                                <td class="firstLeft active">
                                                                    Neues Passwort setzen:
                                                                </td>
                                                                <td nowrap="nowrap" align="left" class="active">
                                                                    <asp:CheckBox ID="chkNewPasswort" runat="server" Width="25px"></asp:CheckBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="trPassword" runat="server" class="formquery">
                                                                <td class="firstLeft active">
                                                                    Passwort:
                                                                </td>
                                                                <td nowrap="nowrap" align="left" class="active">
                                                                    <asp:TextBox ID="txtPassword" runat="server" Visible="true" TextMode="Password"
                                                                        CssClass="InputTextbox"></asp:TextBox><asp:LinkButton ID="btnCreatePassword" runat="server"
                                                                            CssClass="StandardButtonTable" Visible="False">Kennwort generieren</asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                            <tr id="trConfirmPassword" runat="server" class="formquery">
                                                                <td class="firstLeft active">
                                                                    Passwort bestätigen:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <asp:TextBox ID="txtConfirmPassword" runat="server" Visible="true"
                                                                        TextMode="Password" CssClass="InputTextbox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="trEmployee01" runat="server" class="formquery">
                                                                <td class="firstLeft active">
                                                                    Benutzer ist Mitarbeiter:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <asp:CheckBox ID="chkEmployee" runat="server" Width="25px"></asp:CheckBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="trEmployee02" runat="server" class="formquery">
                                                                <td class="firstLeft active">
                                                                    Hierarchie:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <asp:DropDownList ID="ddlHierarchy" runat="server" AutoPostBack="True" 
                                                                        CssClass="DropDowns">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr id="trEmployee03" runat="server" class="formquery">
                                                                <td class="firstLeft active">
                                                                    Abteilung:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <asp:TextBox ID="txtDepartment" runat="server" MaxLength="75" 
                                                                        CssClass="InputTextbox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="trEmployee04" runat="server" class="formquery">
                                                                <td class="firstLeft active">
                                                                    Position:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <asp:TextBox ID="txtPosition" runat="server" MaxLength="75" 
                                                                        CssClass="InputTextbox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="trEmployee05" runat="server" class="formquery">
                                                                <td class="firstLeft active">
                                                                    Telefon:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <asp:TextBox ID="txtTelephone" runat="server" MaxLength="75" 
                                                                        CssClass="InputTextbox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="trEmployee06" runat="server" class="formquery">
                                                                <td class="firstLeft active">
                                                                    Telefax:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <asp:TextBox ID="txtFax" runat="server" MaxLength="75" CssClass="InputTextbox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="trEmployee07" runat="server" class="formquery">
                                                                <td class="firstLeft active">
                                                                    <asp:Label ID="lblPicture" runat="server" Font-Bold="True">Bild hochladen:</asp:Label>
                                                                    <br />
                                                                    <asp:Image ID="Image1" runat="server" Height="150px" Width="100px" />
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <asp:Label ID="lblPictureName" runat="server"></asp:Label>
                                                                    <br />
                                                                    <strong>
                                                                        <input id="upFile" runat="server" name="File1" size="11" type="file" /><br />
                                                                    </strong>
                                                                    <br />
                                                                    <asp:LinkButton ID="btnUpload" runat="server"  Width="95px" 
                                                                        CssClass="Textlarge">&#8226;&nbsp;Hinzufügen</asp:LinkButton>
                                                                    &nbsp;&nbsp;
                                                                    <asp:LinkButton ID="btnRemove" runat="server"  Width="95px">Entfernen</asp:LinkButton><br />
                                                                    &nbsp;<br />
                                                                    Darstellung auf 150 x 100 Pixeln.
                                                                </td>
                                                            </tr>
                                                            <tr id="trUrlRemoteLoginKey" runat="server" class="formquery" visible="false">
                                                                <td class="firstLeft active" colspan="2" style="padding-top: 20px; padding-bottom: 20px; vertical-align: top">
                                                                    <asp:Label ID="lblUrlRemoteLogin" runat="server" Font-Bold="True">URL Remote Login Schlüssel:</asp:Label>
                                                                    <div style="margin-top: 7px; padding-left: 40px; padding-bottom: 7px;">
                                                                        <% If (String.IsNullOrEmpty(lblUrlRemoteLoginKey.Text)) Then%>
                                                                            <asp:Label runat="server" ID="lblUrlRemoteLoginEmptyHint" ForeColor="#a6a6a6" Font-Bold="True"> >>Leer<< &nbsp;&nbsp;&nbsp;(Schlüssel noch nicht generiert)</asp:Label>
                                                                        <%Else%>
                                                                            <asp:Label runat="server" ID="lblUrlRemoteLoginKey" ForeColor="Black" Font-Bold="True" />
                                                                        <%End If%>
                                                                        <div style="padding-top: 7px;">
                                                                            <asp:LinkButton ID="lbtnUrlRemoteLoginKey" runat="server" Text="Schlüssel&nbsp;generieren&nbsp;&#187;" Height="16px"  />
                                                                            <% If (Not String.IsNullOrEmpty(lblUrlRemoteLoginKey.Text)) Then%>
                                                                            <span style="margin-left: 20px;">
                                                                                <asp:LinkButton ID="lbtnUrlRemoteLoginKeyRemove" runat="server" Text="Schlüssel&nbsp;entfernen&nbsp;&#187;" Height="16px"  />
                                                                            </span>
                                                                            <%End If%>
                                                                        </div>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                             
  
                                             
                                                <tr  class="formquery">
                                                    <td class="firstLeft active">
                                                        &nbsp;</td>
                                                    <td class="active">
                                                        &nbsp;</td>
                                                </tr>
                                             
  
                                             
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <div  style="background-color: #dfdfdf; height: 22px;">
                                    &nbsp;
                                </div>                                
                                <div id="dataFooter">
                                    <asp:LinkButton  ID="lbtnApprove" runat="server" Text="Benutzer freigeben&amp;nbsp;&amp;#187; "
                                        CssClass="Tablebutton" Height="16px" Width="155px" Font-Size="10px" Visible="False"></asp:LinkButton>
<%--                                    &nbsp;<asp:LinkButton  ID="lbtnDistrict" runat="server" Text="Distrikte&amp;nbsp;&amp;#187; "
                                        CssClass="Tablebutton" Height="16px" Width="78px"></asp:LinkButton>--%>
                                    &nbsp;<asp:LinkButton  ID="lbtnDelete" runat="server" Text="Löschen&amp;nbsp;&amp;#187; "
                                        CssClass="Tablebutton" Height="16px" Width="78px"></asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="lbtnSave" runat="server" Text="Speichern&amp;nbsp;&amp;#187; "
                                        CssClass="Tablebutton" Height="16px" Width="78px"></asp:LinkButton>
                                    &nbsp;<asp:LinkButton  ID="lbtnCancel" runat="server" Text="Verwerfen&amp;nbsp;&amp;#187; "
                                        CssClass="Tablebutton" Height="16px" Width="78px"></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
