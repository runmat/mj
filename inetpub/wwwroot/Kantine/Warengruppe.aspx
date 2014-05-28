<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Warengruppe.aspx.cs" Inherits="Kantine.Warengruppe.Warengruppe"
    MasterPageFile="Kantine.Master" EnableEventValidation="false"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
            Warengruppen
        </div>
        <div class="Rahmen">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <telerik:RadGrid ID="rgWarengruppen" runat="server" GridLines="None" Width="100%" BorderWidth="0px"
                            Culture="de-DE" CellSpacing="0" AllowSorting="True" ShowFooter="True" AutoGenerateColumns="False"
                            OnNeedDataSource="rgWarengruppenNeedDataSource" OnPageIndexChanged="rgWarengruppenPageIndexChanged"
                            PageSize="100" OnPageSizeChanged="rgWarengruppenPageSizeChanged" OnItemDataBound="rgWarengruppen_ItemDataBound"
                            AllowPaging="True" Height="600px" OnItemCommand="rgWarengruppen_ItemCommand">
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
                                    <telerik:GridBoundColumn DataField="WarengruppeID" HeaderText="WarengruppeID" ReadOnly="true"
                                        SortExpression="WarengruppeID" UniqueName="WarengruppeID" Visible="false" />                                    
                                    <telerik:GridBoundColumn DataField="BezeichnungWarengruppe" HeaderText="Warengruppe" ReadOnly="true"
                                        SortExpression="BezeichnungWarengruppe" UniqueName="BezeichnungWarengruppe" />
                                    <telerik:GridButtonColumn ButtonType="LinkButton" ItemStyle-CssClass="ButtonMiddleTelerik ButtonLinks"
                                        Text="Bearbeiten" CommandName="Bearbeiten" HeaderText="Bearbeiten" ItemStyle-Width="86px" HeaderStyle-Width="86px" />                               
                                    <telerik:GridButtonColumn ButtonType="LinkButton" ItemStyle-CssClass="ButtonTelerik ButtonLinks"
                                        CommandName="Löschen" HeaderText="Löschen" Text="Löschen" ItemStyle-Width="58px" HeaderStyle-Width="58px"/>
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
                                <asp:Label ID="lblAusgabe" runat="server" Visible="false" Style="font-weight: bold;"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Button ID="btnNewMaterialGroup" runat="server" Text="Neue Warengruppe" CssClass="ButtonLarge"
                            OnClick="btnNewMaterialGroup_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>


