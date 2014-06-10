<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Verkaufsuebersicht.aspx.cs"
    Inherits="Kantine.Verkaufsübersicht" MasterPageFile="Kantine.Master" EnableEventValidation="false" %>

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
            Verkaufsübersicht
        </div>
        <div class="Rahmen">
            <div style="padding: 15px 5px 10px 10px; text-align: center; white-space: nowrap;">
                <span style="vertical-align: top; height: 34px;">
                    <asp:Label ID="Label1" runat="server" Style="font-weight: bold;">Von:</asp:Label>
                    &nbsp; <span>
                        <asp:TextBox ID="txtVon" runat="server"></asp:TextBox>
                        <cc1:CalendarExtender ID="CalenderExtendertxtVon" runat="server" TargetControlID="txtVon">
                        </cc1:CalendarExtender>
                        <cc1:FilteredTextBoxExtender ID="FTBEVon" TargetControlID="txtVon" runat="server"
                            ValidChars="0123456789." FilterMode="ValidChars">
                        </cc1:FilteredTextBoxExtender>
                        <cc1:MaskedEditExtender ID="MEEtxtVon" runat="server" MaskType="Date" Mask="99/99/9999"
                            TargetControlID="txtVon">
                        </cc1:MaskedEditExtender>
                    </span>&nbsp;
                    <asp:Label ID="Label2" runat="server" Style="font-weight: bold;">Bis:</asp:Label>
                    &nbsp; <span>
                        <asp:TextBox ID="txtBis" runat="server"></asp:TextBox>
                        <cc1:CalendarExtender ID="CalendarExtendertxtBis" runat="server" TargetControlID="txtBis">
                        </cc1:CalendarExtender>
                        <cc1:FilteredTextBoxExtender ID="FTBEBis" TargetControlID="txtBis" runat="server"
                            ValidChars="0123456789." FilterMode="ValidChars">
                        </cc1:FilteredTextBoxExtender>
                        <cc1:MaskedEditExtender ID="MEEtxtBis" runat="server" MaskType="Date" Mask="99/99/9999"
                            TargetControlID="txtBis">
                        </cc1:MaskedEditExtender>
                    </span>&nbsp; <span>
                        <asp:Label ID="Label3" runat="server" Style="font-weight: bold;">Artikel:</asp:Label>
                        &nbsp;
                        <asp:DropDownList ID="ddlArtikel" runat="server">
                        </asp:DropDownList>
                    </span></span>&nbsp;
                <asp:Button ID="btnShow" runat="server" Text="Anzeigen" OnClick="btnShow_Click" Width="100px"
                    Style="text-align: center; height: 36px; min-height: 30px;" CssClass="ButtonTouch" />
            </div>
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
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
                                    <telerik:GridBoundColumn DataField="Datum" HeaderText="Datum" ReadOnly="true"
                                        SortExpression="Datum" UniqueName="Datum" Visible="false" />                                    
                                    <telerik:GridBoundColumn DataField="Kundennummer" HeaderText="Kundennummer" ReadOnly="true"
                                        SortExpression="Kundennummer" UniqueName="Kundennummer" />
                                    <telerik:GridBoundColumn DataField="Benutzername" HeaderText="Kunde" ReadOnly="true"
                                        SortExpression="Benutzername" UniqueName="Benutzername" />
                                    <telerik:GridBoundColumn DataField="Aktion" HeaderText="Aktion" ReadOnly="true"
                                        SortExpression="Aktion" UniqueName="Aktion" />
                                    <telerik:GridBoundColumn DataField="Artikel" HeaderText="Artikel" ReadOnly="true"
                                        SortExpression="Artikel" UniqueName="Artikel" />
                                    <telerik:GridBoundColumn DataField="Betrag" HeaderText="Betrag in &#8364;" ReadOnly="true"
                                        SortExpression="Betrag" UniqueName="Betrag" />
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                        <hr />
                        <div style="text-align: right; white-space: nowrap; font-weight: bold;">
                            <asp:Label runat="server" Text="Summe Einnahme: "></asp:Label>
                            <asp:Label ID="lblSumme" runat="server" Text="0,00"></asp:Label>
                            <asp:Label runat="server" Text="&#8364;"></asp:Label>
                        </div>
                    </td>
                </tr>
            </table>
            <div>
                &nbsp;</div>
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
