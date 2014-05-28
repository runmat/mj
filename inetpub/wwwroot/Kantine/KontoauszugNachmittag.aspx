<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KontoauszugNachmittag.aspx.cs" Inherits="Kantine.KontoauszugNachmittag"
    MasterPageFile="Kantine.Master" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--<%@ Register Src="PageElements/GridNavigation.ascx" TagName="GridNavigation" TagPrefix="nav" %>--%>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <div id="Main" style="text-align: center;">
        <br />
        <div style="float: left;">
            <asp:Label ID="lblError" runat="server" class="Error"></asp:Label>
        </div>
        <br />
        <br />
        <div class="Heading">
            Aktionen Nachmittagsmodus
        </div>
        <div class="Rahmen" style="padding: 0px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                        </telerik:RadAjaxManager>
                        <telerik:RadGrid ID="rgAuszug" runat="server" GridLines="None" Width="100%" BorderWidth="0px"
                            Culture="de-DE" CellSpacing="0" AllowSorting="True" ShowFooter="True" AutoGenerateColumns="False"
                            OnNeedDataSource="rgAuszugNeedDataSource" OnPageIndexChanged="rgAuszugPageIndexChanged"
                            PageSize="100" OnPageSizeChanged="rgAuszugPageSizeChanged" OnItemDataBound="rgAuszug_ItemDataBound"
                            AllowPaging="True" Height="600px">
                            <ClientSettings AllowKeyboardNavigation="True">
                                <Scrolling AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="0" />
                                <Resizing AllowColumnResize="true" AllowResizeToFit="True" ResizeGridOnColumnResize="False" />
                            </ClientSettings>
                            <FooterStyle ForeColor="DarkBlue" Font-Bold="true" />
                            <HeaderStyle CssClass="RadGridHeader" Wrap="true" />
                            <MasterTableView DataKeyNames="" CommandItemDisplay="Top" TableLayout="Auto" Width="100%"
                                AllowAutomaticUpdates="True">
                                <PagerStyle Mode="NextPrevAndNumeric" PagerTextFormat="{4} Seite <strong>{0}</strong> von <strong>{1}</strong>, insgesamt <strong>{5}</strong> Einträge"
                                    AlwaysVisible="True" />
                                <CommandItemSettings ShowExportToWordButton="false" ShowExportToExcelButton="false"
                                    ShowExportToCsvButton="false" ShowExportToPdfButton="false" ExportToWordText="Export to Word"
                                    ExportToExcelText="Export to XLS" ExportToCsvText="Export to CSV" ExportToPdfText="Export to PDF"
                                    ShowAddNewRecordButton="false" />
                                <Columns>
                                    <telerik:GridBoundColumn DataField="Datum" HeaderText="Datum" ReadOnly="true" SortExpression="Datum"
                                        UniqueName="Datum" />
                                    <telerik:GridBoundColumn DataField="Kundennummer" HeaderText="Kundennummer" ReadOnly="true"
                                        SortExpression="Kundennummer" UniqueName="Kundennummer" />
                                    <telerik:GridBoundColumn DataField="Benutzername" HeaderText="Benutzername" ReadOnly="true"
                                        SortExpression="Benutzername" UniqueName="Benutzername" />
                                    <telerik:GridBoundColumn DataField="Aktion" HeaderText="Aktion" ReadOnly="true" SortExpression="Aktion"
                                        UniqueName="Aktion" />
                                    <telerik:GridBoundColumn DataField="Artikel" HeaderText="Artikel" ReadOnly="true"
                                        SortExpression="Artikel" UniqueName="Artikel" />
                                    <telerik:GridBoundColumn DataField="Betrag" HeaderText="Betrag" ReadOnly="true" SortExpression="Betrag"
                                        UniqueName="Betrag" />
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>         
        </div>
        <div class="Buttoncontainer">
            <table>
                <tr>
                    <td width="100%">
                        <asp:UpdatePanel runat="server" ID="UP1" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblAusgabe" runat="server" Visible="false" Style="font-weight: bold;"></asp:Label></ContentTemplate>
                        </asp:UpdatePanel>
                    </td>                    
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
