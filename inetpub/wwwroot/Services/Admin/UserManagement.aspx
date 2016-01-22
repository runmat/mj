<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UserManagement.aspx.vb"
    Inherits="Admin.UserManagement" MasterPageFile="MasterPage/Admin.Master" %>
<%@ Import Namespace="CKG.Base.Kernel.Security" %>
<%@ Import Namespace="GeneralTools.Models" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <script src="../JScript/Jquery/jquery-1.8.0.min.js" type="text/javascript"></script>
        <script src="../JScript/Jquery/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
        <link href="../JScript/Jquery/MSDropdown/dd.css" rel="stylesheet" type="text/css" />
        <script src="../JScript/Jquery/MSDropdown/js/jquery.dd.js" type="text/javascript"></script>
        <script src="../PageElements/SearchForm/Scripts/jquery-textbox-selection.js" type="text/javascript"></script>
        <div id="site">
            <div id="content">
                <div id="innerContent">
                    <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                    <asp:Label ID="lblMessage" runat="server" CssClass="TextExtraLarge" EnableViewState="False"></asp:Label>
                    <div id="navigationSubmenu">
                        <asp:HyperLink ID="lnkContact" runat="server" ToolTip="Ansprechparter" NavigateUrl="Contact.aspx"
                            Text="Ansprechpartner | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkGroupManagement" runat="server" ToolTip="Gruppen" NavigateUrl="GroupManagement.aspx"
                            Text="Gruppen | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkOrganizationManagement" runat="server" ToolTip="Organisationen"
                            NavigateUrl="OrganizationManagement.aspx" Text="Organisation | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkCustomerManagement" ToolTip="Kunden" runat="server" NavigateUrl="CustomerManagement.aspx"
                            Text="Kunden | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkArchivManagement" ToolTip="Archive" runat="server" NavigateUrl="ArchivManagement.aspx"
                            Text="Archive | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkJahresArchivManagement" ToolTip="Archive" runat="server" NavigateUrl="JahresArchivManagement.aspx"
                            Text="Jahres-Archive | "></asp:HyperLink>
                        <asp:HyperLink ID="lnkAppManagement" runat="server" ToolTip="Anwendungen" NavigateUrl="AppManagement.aspx"
                            Text="Anwendungen"></asp:HyperLink>
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
                        <asp:Panel ID="DivSearch" DefaultButton="btnEmpty" runat="server" style="display: none">
                            <div id="TableQuery">
                                <table id="tableSearch" runat="server" cellspacing="0" cellpadding="0">
                                    <tbody>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Firma:
                                            </td>
                                            <td class="firstLeft activ" nowrap="nowrap">
                                                <asp:DropDownList ID="ddlFilterCustomer" runat="server" AutoPostBack="True" Font-Names="Verdana,sans-serif"
                                                    Visible="False" Width="260px">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblCustomer" runat="server"></asp:Label>
                                            </td>
                                            <td class="secondLeft active" nowrap="nowrap" id="tdHierarchyDisplay1">
                                                Hierarchie:
                                            </td>
                                            <td class="rightPadding" id="tdHierarchyDisplay2">
                                                <asp:DropDownList ID="ddlHierarchyDisplay" runat="server" AutoPostBack="True" Width="260px"
                                                    Font-Names="Verdana,sans-serif">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr class="formquery" id="trSelectOrganization" runat="server">
                                            <td class="firstLeft active">
                                                Organisation:
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:DropDownList ID="ddlFilterOrganization" runat="server" Width="260px" Font-Names="Verdana,sans-serif">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblOrganization" runat="server" Visible="False"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
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
                                            <td class="secondLeft active" id="td_OnlyEmployees1" runat="server">
                                                Nur Mitarbeiter:
                                            </td>
                                            <td id="td_OnlyEmployees2" class="rightPadding" runat="server">
                                                <asp:CheckBox ID="chkEmployeeDisplay" runat="server" Width="25px"></asp:CheckBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Benutzername:
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:TextBox ID="txtFilterUserName" runat="server" CssClass="InputTextbox" Width="257px">**</asp:TextBox>
                                                <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="../images/empty.gif"
                                                    Width="1px" TabIndex="-1" />
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Vorname:
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:TextBox ID="txtFilterFirstName" runat="server" CssClass="InputTextbox" Width="257px">*</asp:TextBox>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Nachname:
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:TextBox ID="txtFilterLastName" runat="server" CssClass="InputTextbox" Width="257px">*</asp:TextBox>
                                            </td>
                                            <td class="secondLeft active" id="td_OnlyLoggedinUser1" runat="server" >
                                                Nur angemeldete Benutzer:&nbsp;
                                            </td>
                                            <td class="rightPadding" id="td_OnlyLoggedinUser2" runat="server" >
                                                <asp:CheckBox ID="chkAngemeldet" runat="server" Width="25px" />
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Email:
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:TextBox ID="txtFilterMail" runat="server" CssClass="InputTextbox" Width="257px">*</asp:TextBox>
                                            </td>
                                            <td class="secondLeft active" id="td_OnlyDisabledUser1" runat="server">
                                                Nur gesperrte Benutzer:&nbsp;
                                            </td>
                                            <td class="rightPadding" id="td_OnlyDisabledUser2" runat="server">
                                                <asp:CheckBox ID="chkOnlyDisabledUser" runat="server" Width="25px" />
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
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
                                                &nbsp;
                                            </td>
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
                                        &nbsp;<asp:LinkButton ID="btnSuche" runat="server" Text="Suchen&amp;nbsp;&amp;#187; "
                                            CssClass="TablebuttonXLarge" Height="21px" Width="155px" Font-Names="Verdana,sans-serif"
                                            Font-Size="10px"></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <div id="Result" runat="Server" style="display: none" >
                            <div id="pagination">
                                <asp:Label ID="lblNotApprovedMode" runat="server" Visible="False" Width="100%" ForeColor="#772D34"
                                    Font-Bold="True" BackColor="Transparent" BorderWidth="1px" BorderStyle="Solid"
                                    BorderColor="#772D34"> <center>Freigabeliste</center></asp:Label>
                                <input id="ihNotApprovedMode" type="hidden" runat="server" value="0"/>
                            </div>
                            <div ID="divLegende" style="margin-bottom: 5px" runat="server" visible="false"><u>Legende:</u> <img id="Img1" runat="server" alt="SelfAdmLevel1" src="../Images/SelfAdmIcon1.gif" height="16" width="16" align="middle" /> = SelfAdministration-Level 1, <img id="Img2" runat="server" alt="SelfAdmLevel2" src="../Images/SelfAdmIcon2.gif" height="16" width="16" align="middle" /> = SelfAdministration-Level 2 (Benutzerkonten nur durch Kunden bearbeitbar)</div>
                            <div id="divTelerik">
                                <script type="text/javascript">
                                    function onRequestStart(sender, args) {
                                        if (args.get_eventTarget().indexOf("ExportToExcelButton") >= 0 ||
                                            args.get_eventTarget().indexOf("ExportToWordButton") >= 0 ||
                                            args.get_eventTarget().indexOf("ExportToPdfButton") >= 0 ||
                                            args.get_eventTarget().indexOf("ExportToCsvButton") >= 0) {
                                            args.set_enableAjax(false);
                                        }
                                    }

                                </script>
                                <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                                    <ClientEvents OnRequestStart="onRequestStart" />
                                    <AjaxSettings>
                                        <telerik:AjaxSetting AjaxControlID="rgSearchResult">
                                            <UpdatedControls>
                                            </UpdatedControls>
                                        </telerik:AjaxSetting>
                                    </AjaxSettings>
                                </telerik:RadAjaxManager>
                                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
                                <telerik:RadGrid ID="rgSearchResult" AutoGenerateColumns="False" runat="server" GridLines="None"
                                    Width="905px" Height="550px" Culture="de-DE" EnableHeaderContextMenu="True" CellSpacing="0"
                                    ClientSettings-AllowColumnsReorder="false" VirtualItemCount="2" AllowPaging="True" 
                                    AllowSorting="True" CellPadding="0" EnableAjaxSkinRendering="False" GroupingEnabled="False">
                                    <ExportSettings HideStructureColumns="false" IgnorePaging="true" OpenInNewWindow="true"
                                        ExportOnlyData="true" FileName="BenutzerExport">
                                        <Excel Format="ExcelML"></Excel>
                                    </ExportSettings>
                                    <ClientSettings AllowKeyboardNavigation="True">
                                        <Scrolling AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="4"></Scrolling>
                                        <Resizing AllowColumnResize="true" ClipCellContentOnResize="False"></Resizing>
                                    </ClientSettings>
                                    <MasterTableView CommandItemDisplay="Top" Summary="UserManagement" TableLayout="Auto"
                                        Width="100%">
                                        <EditFormSettings>
                                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                            </EditColumn>
                                        </EditFormSettings>
                                        <PagerStyle Mode="NextPrevAndNumeric" PagerTextFormat="{4} Seite <strong>{0}</strong> von <strong>{1}</strong>, insgesamt <strong>{5}</strong> Einträge"
                                            AlwaysVisible="True" />
                                        <CommandItemSettings ShowExportToWordButton="false" ShowExportToExcelButton="true"
                                            ShowExportToCsvButton="false" ShowExportToPdfButton="false" ExportToWordText="Export to Word"
                                            ExportToExcelText="Export to XLS" ExportToCsvText="Export to CSV" ExportToPdfText="Export to PDF"
                                            ShowAddNewRecordButton="false" />
                                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                        </RowIndicatorColumn>
                                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                        </ExpandCollapseColumn>
                                        <Columns>
                                            <telerik:GridTemplateColumn DataField="UserID" Visible="false" HeaderText="UserID">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_UserID" CommandArgument="UserID" CommandName="Sort" runat="server">UserID</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUserID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.UserID") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Font-Underline="True" ForeColor="#000099" Wrap="False" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn DataField="UserName" HeaderText="Benutzer">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="UserName" CommandArgument="UserName" CommandName="Sort" runat="server">Benutzer</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton Font-Underline="true" ForeColor="DarkBlue" Text='<%# DataBinder.Eval(Container.DataItem, "UserName") %>'
                                                        runat="server" ID="lbUserName" CommandName="Edit" />
                                                </ItemTemplate>
                                                <ItemStyle Font-Underline="true" ForeColor="DarkBlue" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="SelfAdministration" DataField="SelfAdministration" Visible="false" HeaderText="Adm">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="SelfAdministration" CommandArgument="SelfAdministration" CommandName="Sort"
                                                        runat="server">Adm</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Image ID="imgSRSelfAdministration" runat="server" AlternateText='<%# GetSelfAdministrationImageURL(DataBinder.Eval(Container.DataItem, "SelfAdministration")) %>' 
                                                        ImageUrl='<%# GetSelfAdministrationImageURL(DataBinder.Eval(Container.DataItem, "SelfAdministration")) %>' Height="16px" Width="16px" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="FirstName" SortExpression="FirstName" HeaderText="Vorname"/>
                                            <telerik:GridBoundColumn DataField="LastName" SortExpression="LastName" HeaderText="Nachname"/>
                                            <telerik:GridBoundColumn DataField="mail" SortExpression="mail" HeaderText="Email"/>
                                            <telerik:GridBoundColumn DataField="telephone2" SortExpression="telephone2" HeaderText="Telefon"/>
                                            <telerik:GridTemplateColumn DataField="CustomerName" HeaderText="Firmenname">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_CustomerName" CssClass="TableLinkHead" CommandArgument="CustomerName"
                                                        CommandName="Sort" Text="Firmenname" runat="server">Firmenname</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label51" runat="server" CssClass="TableLabel" Text='<%# DataBinder.Eval(Container, "DataItem.CustomerName") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="GroupName" SortExpression="GroupName" HeaderText="Gruppe"/>
                                            <telerik:GridBoundColumn DataField="OrganizationName" SortExpression="OrganizationName" HeaderText="Orga."/>
                                            <telerik:GridTemplateColumn DataField="AccountIsLockedOut" HeaderText="Konto gesperrt">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="AccountIsLockedOut" CommandArgument="AccountIsLockedOut" CommandName="Sort"
                                                        runat="server">Konto gesperrt</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cbxSRAccountIsLockedOut" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "AccountIsLockedOut") %>'
                                                        Enabled="False"></asp:CheckBox>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn DataField="AccountingArea" HeaderText="Buchungskreis">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="AccountingArea" CommandArgument="AccountingArea" CommandName="Sort"
                                                        runat="server">Buchungskreis</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label51x" runat="server" CssClass="TableLabel" Text='<%# DataBinder.Eval(Container, "DataItem.AccountingArea") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="Reference" SortExpression="Reference" HeaderText="Referenz"/>
                                            <telerik:GridTemplateColumn DataField="CustomerAdmin" HeaderText="Firmenadmin" HeaderStyle-Width="40px">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="CustomerAdmin" CommandArgument="CustomerAdmin" CommandName="Sort"
                                                        runat="server">Firmenadmin</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cbxSRCustomerAdmin" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container.DataItem, "CustomerAdmin") %>'>
                                                    </asp:CheckBox>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn DataField="TestUser" HeaderText="Test">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="TestUser" CommandArgument="TestUser" CommandName="Sort" runat="server">Test</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cbxSRTestUser" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "TestUser") %>'
                                                        Enabled="False"></asp:CheckBox>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="LastPwdChange" SortExpression="LastPwdChange" HeaderText="Passwort geändert"/>
                                            <telerik:GridBoundColumn DataField="LastLogin" SortExpression="LastLogin" HeaderText="letztes Login"/>
                                            <telerik:GridTemplateColumn DataField="PwdNeverExpires" HeaderText="Passwort läuft nie ab:">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="PwdNeverExpires" CommandArgument="PwdNeverExpires" CommandName="Sort"
                                                        runat="server">Passwort läuft ab</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cbxSRPwdNeverExpires" runat="server" Checked='<%# not(DataBinder.Eval(Container.DataItem, "PwdNeverExpires")) %>'
                                                        Enabled="False"></asp:CheckBox>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="FailedLogins" SortExpression="FailedLogins" HeaderText="Anmeld.- Fehlvers."/>
                                            <telerik:GridTemplateColumn DataField="LoggedOn" HeaderText="Angemeldet">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="LoggedOn" CommandArgument="LoggedOn" CommandName="Sort" runat="server">Angemeldet</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSRLoggedOn" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.LoggedOn") %>'>
                                                    </asp:CheckBox>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LoggedOn") %>'>
                                                    </asp:TextBox>
                                                </EditItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="URLRemoteLoginKey" SortExpression="URLRemoteLoginKey" HeaderText="RemoteLoginKey"/>
                                            <telerik:GridTemplateColumn HeaderText="Löschen">
                                                <HeaderTemplate>
                                                    <asp:LinkButton Visible="true" ID="Löschen" CommandArgument="Löschen" CommandName="Sort"
                                                        runat="server">Löschen</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" ID="cmdDelete" CommandName="Del" ImageUrl="../../Images/Papierkorb_01.gif"
                                                        Width="15px" Height="15px" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn Visible="False" DataField="CreatedBy" SortExpression="CreatedBy" HeaderText="CreatedBy"/>
                                        </Columns>
                                        <EditFormSettings EditFormType="Template" EditColumn-EditFormColumnIndex="0">
                                        </EditFormSettings>
                                    </MasterTableView>
                                    <HeaderStyle CssClass="RadGridHeader" Wrap="true" />
                                    <CommandItemStyle Font-Underline="False" />
                                    <ItemStyle Wrap="true" />
                                    <PagerStyle AlwaysVisible="True" />
                                    <FilterMenu EnableImageSprites="False">
                                    </FilterMenu>
                                </telerik:RadGrid>
                                <%--mithilfe dieses Feldes soll verhindert werden, dass das Grid beim Page_Load alle Daten ungefiltert in die DataSource lädt--%>
                                <input id="ihIsInitialDataLoad" type="hidden" runat="server" value="1"/>
                            </div>
                            <div class="dataFooter">
                                &nbsp;
                            </div>
                        </div>
                        <div id="Input" runat="server" style="display: none">
                            <div id="adminInput">
                                <table id="Tablex1" runat="server" cellspacing="0" cellpadding="0">
                                    <tr id="trEditUser" runat="server">
                                        <td>
                                            <table border="0" style="border-color: #ffffff">
                                                <tr class="formquery">
                                                    <td class="firstLeft active" colspan="4">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table id="tblLeft" style="border-color: #ffffff; padding-right: 50px;" cellspacing="0" cellpadding="0">
                                                            <tr class="formquery" id="trCustomer" runat="server">
                                                                <td class="firstLeft active">
                                                                    Firma:
                                                                </td>
                                                                <td class="active">
                                                                    <asp:DropDownList ID="ddlCustomer" runat="server" AutoPostBack="True" CssClass="DropDowns">
                                                                    </asp:DropDownList>
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
                                                            <tr class="formquery">
                                                                <td class="firstLeft active">
                                                                    Benutzername:
                                                                </td>
                                                                <td class="active">
                                                                    <asp:TextBox ID="txtUserName" runat="server" CssClass="InputTextbox"></asp:TextBox><asp:TextBox
                                                                        ID="txtUserID" runat="server" Visible="False" Width="10px" Height="10px" ForeColor="#CEDBDE"
                                                                        BackColor="#CEDBDE" BorderWidth="0px" BorderStyle="None">-1</asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="trAnrede" runat="server" class="formquery">
                                                                <td class="firstLeft active">
                                                                    Anrede:
                                                                </td>
                                                                <td class="active">
                                                                    <asp:DropDownList ID="ddlTitle" runat="server" CssClass="DropDowns">
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
                                                            <tr class="formquery">
                                                                <td class="firstLeft active">
                                                                    Filiale:
                                                                </td>
                                                                <td class="active">
                                                                    <asp:TextBox ID="txtStore" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="trReference" runat="server" class="formquery">
                                                                <td class="firstLeft active">
                                                                    <asp:Label ID="lblReferenceType" runat="server" Font-Bold="True" />
                                                                </td>
                                                                <td class="active">
                                                                    <asp:TextBox ID="txtReference" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="trReference2" runat="server" class="formquery">
                                                                <td class="firstLeft active">
                                                                    <asp:Label ID="lblReferenceType2" runat="server" Font-Bold="True" />
                                                                </td>
                                                                <td class="active">
                                                                    <asp:TextBox ID="txtReference2" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="trReference3" runat="server" class="formquery">
                                                                <td class="firstLeft active">
                                                                    <asp:Label ID="lblReferenceType3" runat="server" Font-Bold="True" />
                                                                </td>
                                                                <td class="active">
                                                                    <asp:TextBox ID="txtReference3" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="trReference4" runat="server" class="formquery">
                                                                <td class="firstLeft active">
                                                                    <asp:Label ID="lblReferenceType4" runat="server" Font-Bold="True" />
                                                                </td>
                                                                <td class="active">
                                                                    <span><asp:CheckBox ID="cbxReference4" runat="server"></asp:CheckBox></span>
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
                                                                    Telefon::
                                                                </td>
                                                                <td class="active">
                                                                    <asp:TextBox ID="txtPhone" runat="server" MaxLength="75" CssClass="InputTextbox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="trTestUser" runat="server" class="formquery">
                                                                <td class="firstLeft active">
                                                                    Test-Zugang:
                                                                </td>
                                                                <td class="active">
                                                                    <span>
                                                                        <asp:CheckBox ID="cbxTestUser" runat="server"></asp:CheckBox></span>
                                                                </td>
                                                            </tr>
                                                            <tr id="Tr1" class="formquery" runat="server">
                                                                <td class="firstLeft active">
                                                                    Gültig ab:
                                                                </td>
                                                                <td class="active">
                                                                    <asp:TextBox ID="txtValidFrom" runat="server" Width="160px" MaxLength="10"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="Tr2" class="formquery" runat="server">
                                                                <td class="firstLeft active">
                                                                    Gültig bis:
                                                                </td>
                                                                <td class="active">
                                                                    <asp:TextBox ID="txtValidTo" runat="server" Width="160px" MaxLength="10"></asp:TextBox>
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
                                                                    <span>
                                                                        <asp:RadioButton ID="cbxNoCustomerAdmin" runat="server" Checked="True" GroupName="grpFirmenadministration"
                                                                            AutoPostBack="true" /></span>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery" id="trCustomerAdmin2" runat="server">
                                                                <td class="firstLeft active">
                                                                    Einzelner Kunde
                                                                </td>
                                                                <td class="active">
                                                                    <span>
                                                                        <asp:RadioButton ID="cbxCustomerAdmin" runat="server" GroupName="grpFirmenadministration"
                                                                            AutoPostBack="true" /></span>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery" id="trCustomerAdmin3" runat="server">
                                                                <td class="firstLeft active">
                                                                    Firstlevel-Administrator
                                                                </td>
                                                                <td class="active">
                                                                    <span>
                                                                        <asp:RadioButton ID="cbxFirstLevelAdmin" runat="server" GroupName="grpFirmenadministration"
                                                                            AutoPostBack="true" /></span>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery" id="trOrganizationAdministrator" runat="server">
                                                                <td class="firstLeft active">
                                                                    Organisationadministrator:
                                                                </td>
                                                                <td class="active">
                                                                    <span>
                                                                        <asp:CheckBox ID="cbxOrganizationAdmin" runat="server" AutoPostBack="true"></asp:CheckBox></span>
                                                                </td>
                                                            </tr>
                                                            <tr class="formquery">
                                                                <td class="firstLeft active">
                                                                    Benutzerhistorie:
                                                                </td>
                                                                <td class="active">
                                                                    <%--<asp:HyperLink ID="hlUserHistory" runat="server" Target="UserHist">Anzeigen</asp:HyperLink>--%>
                                                                    <asp:HyperLink ID="hlUserHistory" runat="server" Target="UserHistory.aspx">Anzeigen</asp:HyperLink>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <asp:TextBox ID="txtPasswordAutoBAK" runat="server" Visible="False" Width="160px"
                                                            Enabled="False" ReadOnly="True"></asp:TextBox><asp:TextBox ID="txtPAsswordConfirmAutoBAK"
                                                                runat="server" Visible="False" Width="160px" Enabled="False" ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                    <td valign="top">
                                                        <table id="tblRight" cellspacing="0" style="border-color: #ffffff" cellpadding="0"
                                                            width="100%">
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
                                                                    <asp:CheckBox ID="cbxAccountIsLockedOut" runat="server" Width="25px"></asp:CheckBox>
                                                                    <asp:CheckBox ID="cbxApproved" runat="server" Visible="False" Width="25px"></asp:CheckBox>
                                                                    <asp:Label ID="lblLockedBy" runat="server" Visible="false"></asp:Label>
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
                                                            <tr id="trMatrix" runat="server" visible="False" class="formquery">
                                                                <td class="firstLeft active">
                                                                    Matrix gefüllt:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <asp:CheckBox ID="chk_Matrix1" runat="server" AutoPostBack="True" Width="25px"></asp:CheckBox>
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
                                                                    <asp:CheckBox ID="chkNewPasswort" runat="server" Width="25px"></asp:CheckBox>
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
                                                                    <asp:DropDownList ID="ddlHierarchy" runat="server" AutoPostBack="True" CssClass="DropDowns">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr id="trEmployee03" runat="server" class="formquery">
                                                                <td class="firstLeft active">
                                                                    Abteilung:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <asp:TextBox ID="txtDepartment" runat="server" MaxLength="75" CssClass="InputTextbox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="trEmployee04" runat="server" class="formquery">
                                                                <td class="firstLeft active">
                                                                    Position:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <asp:TextBox ID="txtPosition" runat="server" MaxLength="75" CssClass="InputTextbox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="trEmployee05" runat="server" class="formquery">
                                                                <td class="firstLeft active">
                                                                    Telefon:
                                                                </td>
                                                                <td align="left" class="active">
                                                                    <asp:TextBox ID="txtTelephone" runat="server" MaxLength="75" CssClass="InputTextbox"></asp:TextBox>
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
                                                                    <asp:LinkButton ID="btnUpload" runat="server" Width="95px" CssClass="Textlarge">&#8226;&nbsp;Hinzufügen</asp:LinkButton>
                                                                    &nbsp;&nbsp;
                                                                    <asp:LinkButton ID="btnRemove" runat="server" Width="95px">Entfernen</asp:LinkButton><br />
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
                                                            <tr id="trMasterUser" runat="server" class="formquery" visible="false">
                                                                <td style="padding-left: 10px;">
                                                                    <asp:LinkButton ID="lbtnOpenMasterUserOptions" runat="server" Text="Master-User&amp;nbsp;&amp;#187; "
                                                                        CssClass="TablebuttonLarge" Height="16px" Width="128px" />
                                                                </td>
                                                                <td>&nbsp;</td>
                                                            </tr>

                                                        </table>

                                                           <div id="BenutzerPanel">
																	    
                                                            <div style="padding-left: 0px; padding-top: 10px;">
                                                               <h3>User-Rechte</h3>

                                                              <telerik:RadGrid ID="drUserRights" runat="server" AllowSorting="False" 
                                                                        AutoGenerateColumns="False" GridLines="None" Culture="de-DE"
                                                                       >
                                                                  
                                                             

                                                                <ClientSettings>
                                                                    <Scrolling ScrollHeight="265px" AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="1" />
                                                                </ClientSettings>

                                                                <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Auto" AllowPaging="false" >
                                                                    <SortExpressions>
                                                                        <telerik:GridSortExpression FieldName="CategoryID" SortOrder="Ascending" />
                                                                    </SortExpressions>
                                                                    <HeaderStyle ForeColor="White" />

                                                                    <Columns>

                                                                              <telerik:GridTemplateColumn Groupable="false" UniqueName="SettingsValue" >
                                                                                        <HeaderStyle Width="150px" />
                                                                                                <ItemTemplate>
                                                                                                          <asp:TextBox ID="Recht1"  Visible='<%# DataBinder.Eval(Container.DataItem, "IsTextBoxVisible") %>' 
                                                                                                                name='<%# DataBinder.Eval(Container.DataItem, "CategoryId") %>' 
                                                                                                                runat="server" 
                                                                                                                text='<%# DataBinder.Eval(Container.DataItem, "SettingsValue").toString() %>' 
                                                                                                           />
                                                                                                           
                                                                                                            <asp:Checkbox ID="Recht2"  Visible='<%# DataBinder.Eval(Container.DataItem, "IsCheckBoxVisible") %>' 
                                                                                                                    name='<%# DataBinder.Eval(Container.DataItem, "CategoryId") %>' 
                                                                                                                    runat="server" 
                                                                                                                     Checked='<%# DataBinder.Eval(Container, "DataItem.SettingsValue") %>' 
                                                                                                             />
                                                                                                         
                                                                                                </ItemTemplate>
                                                                                 </telerik:GridTemplateColumn>
                                                                                 
                                                                        <telerik:GridBoundColumn DataField="CategoryID" SortExpression="CategoryID" HeaderText="Recht / Setting" UniqueName="CategoryID" >
                                                                             <HeaderStyle Width="150px" />
                                                                            <ItemStyle></ItemStyle>
                                                                        </telerik:GridBoundColumn>

                                                                    </Columns>

                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                                            
                                                                </div>
														</div>

                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <div style="background-color: #dfdfdf; height: 22px;">
                                    &nbsp;
                                </div>
                                <div id="DivMatrix" runat="server">
                                    <asp:Table ID="Matrix" runat="server" Visible="False" Style="border-color: #ffffff">
                                    </asp:Table>
                                </div>
                                <div id="dataFooter">
                                    <asp:LinkButton ID="lbtnApprove" runat="server" Text="Benutzer freigeben&amp;nbsp;&amp;#187; "
                                        CssClass="TablebuttonXLarge" Height="16px" Width="155px" Font-Size="10px" Visible="False"></asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="lbtnDistrict" runat="server" Text="Distrikte&amp;nbsp;&amp;#187; "
                                        CssClass="Tablebutton" Height="16px" Width="78px" Visible="false"></asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="lbtnDelete" runat="server" Text="Löschen&amp;nbsp;&amp;#187; "
                                        CssClass="Tablebutton" Height="16px" Width="78px"></asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="lbtnSave" runat="server" Text="Speichern&amp;nbsp;&amp;#187; "
                                        CssClass="Tablebutton" Height="16px" Width="78px"></asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="lbtnCancel" runat="server" Text="Verwerfen&amp;nbsp;&amp;#187; "
                                        CssClass="Tablebutton" Height="16px" Width="78px"></asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="lbtnCopy" runat="server" Text="Kopieren&amp;nbsp;&amp;#187; "
                                        CssClass="Tablebutton" Height="16px" Width="78px"></asp:LinkButton>
                                    <asp:Button ID="btnFake" runat="server" Text="" Style="display: none" />
                                    <ajaxToolkit:ModalPopupExtender ID="masterUserOptions" runat="server" TargetControlID="btnFake"
                                        PopupControlID="mb" BackgroundCssClass="modalBackground" DropShadow="true" CancelControlID="lbtnCancelMasterUser"
                                        X="380" Y="250">
                                    </ajaxToolkit:ModalPopupExtender>
                                    <asp:Panel ID="mb" runat="server" Width="385px" Height="150px" BackColor="White"
                                        BorderColor="#999999" BorderStyle="Solid" BorderWidth="1" Style="display: none">
                                        <div style="padding-left: 10px; padding-top: 15px;">
                                            <h2>
                                                Masteruser</h2>
                                        </div>
                                        <div style="padding-left: 10px; padding-top: 15px;">
                                            <asp:Label runat="server" ID="lblMasterUser" EnableViewState="false" />
                                        </div>
                                        <div style="padding-left: 10px; padding-top: 15px;">
                                            <asp:LinkButton class="Tablebutton" ID="lbtnCancelMasterUser" runat="server" Text="Abbrechen"
                                                CssClass="Tablebutton" Height="16px" Width="78px" />
                                            &nbsp;
                                            <asp:LinkButton class="Tablebutton" ID="lbtnMasterUser" runat="server" Text="Ok"
                                                CssClass="Tablebutton" Height="16px" Width="78px" />
                                        </div>
                                    </asp:Panel>
                                    
                                    
                                    <asp:Panel ID="rightPanel" runat="server" Width="385px" Height="150px" BackColor="White"
                                        BorderColor="red" BorderStyle="Solid" BorderWidth="1" Style="display: none">
                                        <div style="padding-left: 10px; padding-top: 15px;">
                                            <h2>Rechte</h2>
                                        </div>

                                        <div style="padding-left: 10px; padding-top: 15px;">
                                            
                                        </div>

                                        <div style="padding-left: 10px; padding-top: 15px;">
                                            
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
                $("#<%= DivSearch.ClientID %>").show();
                $("#<%= txtFilterUserName.ClientID %>").setCaretPos(2);
            }
            else if ($("#<%= ihExpandstatusSearchResultArea.ClientID %>").attr("value") == "1") {
                $("#arealnkSuchergebnis").hide();
                $("#lnkSuchergebnis").hide();
                $("#arealnkSuche").show();
                $("#lnkSuche").show();
                $("#<%= DivSearch.ClientID %>").hide();
                $("#<%= Input.ClientID %>").hide();
                $("#<%= Result.ClientID %>").show();
            }
            else {
                $("#arealnkSuche").show();
                $("#lnkSuche").show();
                $("#arealnkSuchergebnis").show();
                $("#lnkSuchergebnis").show();
                $("#<%= DivSearch.ClientID %>").hide();
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

        function PrepareUserSearchResultsAutomaticLoad() {
            var redirect = <%= (DirectCast(Session("objUser"), User).Reference2.NotNullOrEmpty() = "AutoAdminRedirect").ToString().ToLower() %>;
            if (!redirect) 
                return;

            if ($("#<%= Result.ClientID %>").css("display") == "none")
                return;

            if ($(".rgRow").length != 1)
                return;

            $("#<%= Result.ClientID %>").html("<div class='auto-redirect-link'>Sie werden automatisch weitergeleitet ...</div>");

            __doPostBack('ctl00$ContentPlaceHolder1$rgSearchResult$ctl00$ctl04$lbUserName', '');
        }

        $(function () {
            CheckCollapseExpandStatus();

            PrepareUserSearchResultsAutomaticLoad();
        }); 
        
    </script>
</asp:Content>
