<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Benutzer.aspx.cs" Inherits="Kantine.Benutzer"
    MasterPageFile="Kantine.Master" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <div id="Main" style="text-align: center;">
        <br />
        <div style="float: left;">
            <asp:Label ID="lblError" runat="server" class="Error" meta:resourcekey="lblErrorResource1"></asp:Label>
        </div>
        <br />
        <br />
        <div class="Heading">
            Benutzerliste
        </div>
        <div class="Rahmen">           
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>                        
                        <telerik:RadGrid ID="rgBenutzer" runat="server" GridLines="None" Width="100%" BorderWidth="0px"
                            Culture="de-DE" CellSpacing="0" AllowSorting="True" ShowFooter="True" AutoGenerateColumns="False"
                            OnNeedDataSource="rgBenutzerNeedDataSource" OnPageIndexChanged="rgBenutzerPageIndexChanged"
                            PageSize="100" OnPageSizeChanged="rgBenutzerPageSizeChanged" OnItemDataBound="rgBenutzer_ItemDataBound"
                            AllowPaging="True" Height="600px" OnItemCommand="rgBenutzer_ItemCommand">
                            <ClientSettings AllowKeyboardNavigation="True">
                                <Scrolling AllowScroll="true" UseStaticHeaders="True" FrozenColumnsCount="0" />
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
                                    <telerik:GridBoundColumn DataField="Benutzername" HeaderText="Benutzername" ReadOnly="true"
                                        SortExpression="Benutzername" UniqueName="Benutzername" />
                                    <telerik:GridBoundColumn DataField="Nachname" HeaderText="Nachname" ReadOnly="true"
                                        SortExpression="Nachname" UniqueName="Nachname" />
                                    <telerik:GridBoundColumn DataField="Vorname" HeaderText="Vorname" ReadOnly="true"
                                        SortExpression="Vorname" UniqueName="Vorname" />
                                    <telerik:GridBoundColumn DataField="Kundennummer" HeaderText="Kundennummer" ReadOnly="true"
                                        SortExpression="Kundennummer" UniqueName="Kundennummer" />
                                    <telerik:GridTemplateColumn HeaderText="Admin" SortExpression="Admin">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" Enabled="false" Checked='<%#Eval("Admin")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Benutzer" SortExpression="Useradmin">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" Enabled="false" Checked='<%#Eval("Useradmin")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Verkäufer" SortExpression="Seller">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" Enabled="false" Checked='<%#Eval("Seller")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Gesperrt" SortExpression="Gesperrt">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" Enabled="false" Checked='<%#Eval("Gesperrt")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridButtonColumn ButtonType="LinkButton" ItemStyle-CssClass="ButtonMiddleTelerik ButtonLinks"
                                        Text="Zurücksetzen" CommandName="PWReset" HeaderText="Passwort" ItemStyle-Width="86px" HeaderStyle-Width="86px" />
                                    <%--<asp:ButtonColumn ButtonType="LinkButton" ItemStyle-CssClass="ButtonTelerik ButtonLinks" CommandName="CreateBC"
                                            HeaderText="Barcode" Text="Barcode" />--%>
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
                        <asp:Button ID="btnNewUser" runat="server" Text="Neuer Benutzer" CssClass="ButtonMiddle"
                            OnClick="btnNewUser_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
