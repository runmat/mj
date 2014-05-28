<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ShowBapis.aspx.vb" Inherits="Admin.ShowBapis"
    MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    OnClick="responseBack" Text="Zurück" />
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label" />
                        </h1>
                    </div>
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
                    <telerik:RadAjaxManager runat="server">
                        <ClientEvents OnRequestStart="onRequestStart" />
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="WebBapisDG" />
                            <telerik:AjaxSetting AjaxControlID="WebImportDG" />
                            <telerik:AjaxSetting AjaxControlID="WebExportDG" />
                            <telerik:AjaxSetting AjaxControlID="WebTabellenDG" />
                            <telerik:AjaxSetting AjaxControlID="SAPImportDG" />
                            <telerik:AjaxSetting AjaxControlID="SAPExportDG" />
                            <telerik:AjaxSetting AjaxControlID="SAPTabellenDG" />
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td colspan="3" align="center">
                                        <asp:Panel runat="server" DefaultButton="imgbSetFilter">
                                            Bapi Name:&nbsp;<asp:TextBox ID="txtFilter" runat="server" Text="**" Width="250px" />
                                            &nbsp;<asp:ImageButton ID="imgbSetFilter" runat="server" Height="20px" ImageUrl="../Images/Kreislauf_01.jpg"
                                                Visible="True" Width="20px" />
                                            &nbsp;&nbsp;<asp:ImageButton ID="imgbLookSAP" runat="server" ImageUrl="../Images/SAPLogo.gif"
                                                Visible="True" />
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="center">
                                        <b>Web-Bapis
                                            <asp:Label ID="lblWebBapisError" CssClass="TextError" runat="server" />
                                            <asp:Label ID="lblWebBapisNoData" runat="server" Visible="False" />
                                            <asp:Label ID="lblWebBapisInfo" runat="server" Font-Bold="True" />
                                        </b>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="center">
                                        <telerik:RadGrid ID="WebBapisDG" runat="server" Width="100%" CellPadding="0" AllowSorting="True"
                                            Visible="True" AllowPaging="True" AutoGenerateColumns="False" BackColor="White"
                                            BorderStyle="None" Culture="de-DE" OnNeedDataSource="WebBapisDG_NeedDataSource"
                                            OnItemCommand="WebBapisDG_ItemCommand" PageSize="20">
                                            <ClientSettings>
                                                <Resizing AllowColumnResize="true" ClipCellContentOnResize="true" />
                                                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="300" />
                                            </ClientSettings>
                                            <AlternatingItemStyle HorizontalAlign="Left" Wrap="false" />
                                            <ExportSettings HideStructureColumns="true" Excel-Format="ExcelML" ExportOnlyData="true"
                                                IgnorePaging="true" FileName="Web-Bapis" />
                                            <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                            <FooterStyle CssClass="RADGridFooter" />
                                            <PagerStyle CssClass="RADGridFooter" AlwaysVisible="true" Mode="NextPrevAndNumeric"
                                                PagerTextFormat="{4} Seite <strong>{0}</strong> von <strong>{1}</strong>, insgesamt <strong>{5}</strong> Einträge" />
                                            <MasterTableView CommandItemDisplay="Top" DataKeyNames="BapiName">
                                                <CommandItemSettings ShowExportToWordButton="false" ShowExportToExcelButton="true"
                                                    ShowExportToCsvButton="false" ShowExportToPdfButton="false" ShowAddNewRecordButton="false" />
                                                <%-- TODO: re-Enable Export To Excel --%>
                                                <Columns>
                                                    <telerik:GridTemplateColumn SortExpression="BapiName" HeaderText="LookAt" DataField="LookAt">
                                                        <HeaderStyle Width="50px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbDetails" CommandArgument='<%# Eval("BapiName") %>'
                                                                runat="server" Width="16px" CommandName="LookAt" Height="16px">
                                                                <img src="../Images/Lupe_01.gif" width="16px" height="16px" alt="Details" border="0"/>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="BapiName" SortExpression="BapiName" UniqueName="BapiName"
                                                        HeaderText="BapiName" />
                                                    <telerik:GridBoundColumn DataField="TestSap" SortExpression="TestSap" UniqueName="TestSap"
                                                        HeaderText="Test" />
                                                    <telerik:GridBoundColumn DataField="SourceModule" SortExpression="SourceModule" UniqueName="SourceModule"
                                                        HeaderText="Modul" />
                                                    <telerik:GridBoundColumn DataField="BapiDate" SortExpression="BapiDate" UniqueName="BapiDate"
                                                        HeaderText="BapiDate" DataFormatString="{0:d}" />
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="border-bottom-color: red; border-bottom-style: solid; border-width: 3;">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td colspan="3">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr class="TextLarge">
                                                <td nowrap="nowrap" valign="bottom" width="25%">
                                                    <asp:ImageButton runat="server" ID="imgbWebBapiVisible" Style="width: 1.5em; height: 1.5em;"
                                                        ImageAlign="AbsMiddle" ImageUrl="../Images/minus.gif" />
                                                    &nbsp;<strong>Web-Bapi Struktur</strong>
                                                </td>
                                                <td align="left" valign="bottom">
                                                    &nbsp;<asp:Label ID="lblWebBapiName" Font-Bold="True" runat="server" ForeColor="Red" />
                                                    &nbsp;<asp:Label ID="lblWebBapiDatum" Font-Bold="True" runat="server" ForeColor="Red" />
                                                    &nbsp;<asp:Label ID="lblWebBapiError" runat="server" EnableViewState="False" CssClass="TextError" />
                                                </td>
                                                <td align="right">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Panel ID="panelWebBapi" BorderColor="Gray" BorderStyle="Solid" BorderWidth="0"
                                            Width="100%" runat="server" Height="100%">
                                            <table id="TableKleinerAbstandVorGrid" cellspacing="0" cellpadding="0" width="100%"
                                                border="0">
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                            <tr class="TextLarge">
                                                                <td nowrap="nowrap" valign="bottom" width="25%">
                                                                    <asp:ImageButton runat="server" ID="imgbWebImportVisible" Style="width: 1.5em; height: 1.5em;"
                                                                        ImageAlign="AbsMiddle" ImageUrl="../Images/plus.gif" />
                                                                    &nbsp;<strong>Web-Bapi Import</strong>
                                                                </td>
                                                                <td align="left" valign="bottom">
                                                                    &nbsp;<asp:Label ID="lblWebImportInfo" Font-Bold="True" runat="server" />
                                                                    &nbsp;<asp:Label ID="lblWebImportNoData" runat="server" Visible="False" />
                                                                    &nbsp;<asp:Label ID="lblWebImportError" runat="server" EnableViewState="False" CssClass="TextError" />
                                                                </td>
                                                                <td align="right">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <asp:Panel ID="panelWebImport" Visible="false" BorderColor="Gray" BorderStyle="Solid"
                                                            BorderWidth="2" Width="100%" runat="server" Height="100%">
                                                            <telerik:RadGrid ID="WebImportDG" runat="server" Width="100%" CellPadding="0" AllowSorting="True"
                                                                Visible="True" AllowPaging="True" AutoGenerateColumns="False" BackColor="White"
                                                                BorderStyle="None" Culture="de-DE" OnNeedDataSource="WebImportDG_NeedDataSource"
                                                                OnDetailTableDataBind="WebImportDG_DetailTableDataBind" PageSize="20">
                                                                <ClientSettings>
                                                                    <Resizing AllowColumnResize="true" ClipCellContentOnResize="true" />
                                                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="300" />
                                                                </ClientSettings>
                                                                <ExportSettings HideStructureColumns="true" Excel-Format="ExcelML" ExportOnlyData="true"
                                                                    IgnorePaging="true" FileName="Web-Bapi-Import" />
                                                                <ItemStyle Wrap="false" />
                                                                <FooterStyle CssClass="RADGridFooter" />
                                                                <PagerStyle CssClass="RADGridFooter" AlwaysVisible="true" Mode="NextPrevAndNumeric"
                                                                    PagerTextFormat="{4} Seite <strong>{0}</strong> von <strong>{1}</strong>, insgesamt <strong>{5}</strong> Einträge" />
                                                                <MasterTableView CommandItemDisplay="Top" DataKeyNames="PARAMETER">
                                                                    <CommandItemSettings ShowExportToWordButton="false" ShowExportToExcelButton="true"
                                                                        ShowExportToCsvButton="false" ShowExportToPdfButton="false" ShowAddNewRecordButton="false" />
                                                                    <%-- TODO: re-Enable Export To Excel --%>
                                                                    <DetailTables>
                                                                        <telerik:GridTableView DataKeyNames="SpaltenName" Name="Spalten" Width="100%" AllowPaging="false">
                                                                            <Columns>
                                                                                <telerik:GridBoundColumn DataField="SpaltenName" HeaderText="Spalten Name" />
                                                                                <telerik:GridBoundColumn DataField="SpaltenTyp" HeaderText="Spalten Datentyp" />
                                                                                <telerik:GridBoundColumn DataField="Laenge" HeaderText="Länge" />
                                                                            </Columns>
                                                                        </telerik:GridTableView>
                                                                    </DetailTables>
                                                                    <Columns>
                                                                        <telerik:GridBoundColumn DataField="PARAMETER" SortExpression="PARAMETER" UniqueName="PARAMETER"
                                                                            HeaderText="Parameter Name" />
                                                                        <telerik:GridBoundColumn DataField="ParameterDATATYPE" SortExpression="ParameterDATATYPE"
                                                                            UniqueName="ParameterDATATYPE" HeaderText="Parameter Datentyp" />
                                                                        <telerik:GridBoundColumn DataField="ParameterLength" SortExpression="ParameterLength"
                                                                            UniqueName="ParameterLength" HeaderText="Parameter Länge" />
                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                        </asp:Panel>
                                                    </td>
                                                    <td>
                                                        &nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                            <tr class="TextLarge">
                                                                <td nowrap="nowrap" valign="bottom" width="25%">
                                                                    <asp:ImageButton runat="server" ID="imgbWebExportVisible" Style="width: 1.5em; height: 1.5em;"
                                                                        ImageAlign="AbsMiddle" ImageUrl="../Images/plus.gif" />
                                                                    &nbsp;<strong>Web-Bapi Export</strong>
                                                                </td>
                                                                <td align="left" valign="bottom">
                                                                    &nbsp;<asp:Label ID="lblWebExportInfo" Font-Bold="True" runat="server" />
                                                                    &nbsp;<asp:Label ID="lblWebExportNoData" runat="server" Visible="False" />
                                                                    &nbsp;<asp:Label ID="lblWebExportError" runat="server" EnableViewState="False" CssClass="TextError" />
                                                                </td>
                                                                <td align="right">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <asp:Panel ID="panelWebExport" Visible="false" BorderColor="Gray" BorderStyle="Solid"
                                                            BorderWidth="2" Width="100%" runat="server" Height="100%">
                                                            <telerik:RadGrid ID="WebExportDG" runat="server" Width="100%" CellPadding="0" AllowSorting="True"
                                                                Visible="True" AllowPaging="True" AutoGenerateColumns="False" BackColor="White"
                                                                BorderStyle="None" Culture="de-DE" OnNeedDataSource="WebExportDG_NeedDataSource"
                                                                OnDetailTableDataBind="WebExportDG_DetailTableDataBind" PageSize="20">
                                                                <ClientSettings>
                                                                    <Resizing AllowColumnResize="true" ClipCellContentOnResize="true" />
                                                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="300" />
                                                                </ClientSettings>
                                                                <ExportSettings HideStructureColumns="true" Excel-Format="ExcelML" ExportOnlyData="true"
                                                                    IgnorePaging="true" FileName="Web-Bapi-Export" />
                                                                <ItemStyle Wrap="false" />
                                                                <FooterStyle CssClass="RADGridFooter" />
                                                                <PagerStyle CssClass="RADGridFooter" AlwaysVisible="true" Mode="NextPrevAndNumeric"
                                                                    PagerTextFormat="{4} Seite <strong>{0}</strong> von <strong>{1}</strong>, insgesamt <strong>{5}</strong> Einträge" />
                                                                <MasterTableView CommandItemDisplay="Top" DataKeyNames="PARAMETER">
                                                                    <CommandItemSettings ShowExportToWordButton="false" ShowExportToExcelButton="true"
                                                                        ShowExportToCsvButton="false" ShowExportToPdfButton="false" ShowAddNewRecordButton="false" />
                                                                    <%-- TODO: re-Enable Export To Excel --%>
                                                                    <DetailTables>
                                                                        <telerik:GridTableView DataKeyNames="SpaltenName" Name="Spalten" Width="100%" AllowPaging="false">
                                                                            <Columns>
                                                                                <telerik:GridBoundColumn DataField="SpaltenName" HeaderText="Spalten Name" />
                                                                                <telerik:GridBoundColumn DataField="SpaltenTyp" HeaderText="Spalten Datentyp" />
                                                                                <telerik:GridBoundColumn DataField="Laenge" HeaderText="Länge" />
                                                                            </Columns>
                                                                        </telerik:GridTableView>
                                                                    </DetailTables>
                                                                    <Columns>
                                                                        <telerik:GridBoundColumn DataField="PARAMETER" SortExpression="PARAMETER" UniqueName="PARAMETER"
                                                                            HeaderText="Parameter Name" />
                                                                        <telerik:GridBoundColumn DataField="ParameterDATATYPE" SortExpression="ParameterDATATYPE"
                                                                            UniqueName="ParameterDATATYPE" HeaderText="Parameter Datentyp" />
                                                                        <telerik:GridBoundColumn DataField="ParameterLength" SortExpression="ParameterLength"
                                                                            UniqueName="ParameterLength" HeaderText="Parameter Länge" />
                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                        </asp:Panel>
                                                    </td>
                                                    <td>
                                                        &nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                            <tr class="TextLarge">
                                                                <td nowrap="nowrap" valign="bottom" width="25%">
                                                                    <asp:ImageButton runat="server" ID="imgbWebTabellenVisible" Style="width: 1.5em;
                                                                        height: 1.5em;" ImageAlign="AbsMiddle" ImageUrl="../Images/plus.gif" />
                                                                    &nbsp;<strong>Web-Bapi Tabellen</strong>
                                                                </td>
                                                                <td align="left" valign="bottom">
                                                                    &nbsp;<asp:Label ID="lblWebTabellenInfo" Font-Bold="True" runat="server" />
                                                                    &nbsp;<asp:Label ID="lblWebTabellenNoData" runat="server" Visible="False" />
                                                                    &nbsp;<asp:Label ID="lblWebTabellenError" runat="server" EnableViewState="False"
                                                                        CssClass="TextError" />
                                                                </td>
                                                                <td align="right">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <asp:Panel ID="panelWebTabellen" Visible="false" BorderColor="Gray" BorderStyle="Solid"
                                                            BorderWidth="2" Width="100%" runat="server" Height="100%">
                                                            <telerik:RadGrid ID="WebTabellenDG" runat="server" Width="100%" CellPadding="0" AllowSorting="True"
                                                                Visible="True" AllowPaging="True" AutoGenerateColumns="False" BackColor="White"
                                                                BorderStyle="None" Culture="de-DE" OnNeedDataSource="WebTabellenDG_NeedDataSource"
                                                                OnDetailTableDataBind="WebTabellenDG_DetailTableDataBind" PageSize="20">
                                                                <ClientSettings>
                                                                    <Resizing AllowColumnResize="true" ClipCellContentOnResize="true" />
                                                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="300" />
                                                                </ClientSettings>
                                                                <ExportSettings HideStructureColumns="true" Excel-Format="ExcelML" ExportOnlyData="true"
                                                                    IgnorePaging="true" FileName="Web-Bapi-Tabellen" />
                                                                <ItemStyle Wrap="false" />
                                                                <FooterStyle CssClass="RADGridFooter" />
                                                                <PagerStyle CssClass="RADGridFooter" AlwaysVisible="true" Mode="NextPrevAndNumeric"
                                                                    PagerTextFormat="{4} Seite <strong>{0}</strong> von <strong>{1}</strong>, insgesamt <strong>{5}</strong> Einträge" />
                                                                <MasterTableView CommandItemDisplay="Top" DataKeyNames="TabellenName">
                                                                    <CommandItemSettings ShowExportToWordButton="false" ShowExportToExcelButton="true"
                                                                        ShowExportToCsvButton="false" ShowExportToPdfButton="false" ShowAddNewRecordButton="false" />
                                                                    <%-- TODO: re-Enable Export To Excel --%>
                                                                    <DetailTables>
                                                                        <telerik:GridTableView DataKeyNames="SpaltenName" Name="Spalten" Width="100%" AllowPaging="false">
                                                                            <Columns>
                                                                                <telerik:GridBoundColumn DataField="SpaltenName" HeaderText="Spalten Name" />
                                                                                <telerik:GridBoundColumn DataField="SpaltenTyp" HeaderText="Spalten Datentyp" />
                                                                                <telerik:GridBoundColumn DataField="Laenge" HeaderText="Länge" />
                                                                            </Columns>
                                                                        </telerik:GridTableView>
                                                                    </DetailTables>
                                                                    <Columns>
                                                                        <telerik:GridBoundColumn DataField="TabellenName" SortExpression="TabellenName" UniqueName="TabellenName"
                                                                            HeaderText="Tabellen Name" />
                                                                        <telerik:GridBoundColumn DataField="Tabellengroesse" SortExpression="Tabellengroesse"
                                                                            UniqueName="Tabellengroesse" HeaderText="Tabellengröße" />
                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                        </asp:Panel>
                                                    </td>
                                                    <td>
                                                        &nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="border-top-color: red; border-top-style: solid; border-width: 3;">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="border-bottom-color: blue; border-bottom-style: solid; border-width: 3;">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td colspan="3">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr class="TextLarge">
                                                <td nowrap="nowrap" valign="bottom" width="25%">
                                                    <asp:ImageButton runat="server" ID="imgbSAPBapiVisible" Style="width: 1.5em; height: 1.5em;"
                                                        ImageAlign="AbsMiddle" ImageUrl="../Images/minus.gif" />
                                                    &nbsp;<strong><span class="style1">SAP-Bapi Struktur&nbsp;</span></strong>
                                                </td>
                                                <td align="left" valign="bottom">
                                                    &nbsp;
                                                    <asp:Label ID="lblSAPBapiName" Font-Bold="True" runat="server" ForeColor="Blue" />
                                                    &nbsp;<asp:Label ID="lblSAPBapiDatum" Font-Bold="True" runat="server" ForeColor="Blue" />
                                                    <span lang="de">&nbsp;</span><asp:Label ID="lblSAPBapiNoData" runat="server" Visible="False" />
                                                    <span lang="de">&nbsp;<asp:Label ID="lblSAPBapiError" runat="server" EnableViewState="False"
                                                        CssClass="TextError" /></span>
                                                </td>
                                                <td align="right">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Panel ID="panelSAPBapi" BorderColor="Gray" BorderStyle="Solid" BorderWidth="0"
                                            Width="100%" runat="server" Height="100%">
                                            <table id="Table2" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                            <tr class="TextLarge">
                                                                <td nowrap="nowrap" valign="bottom" width="25%">
                                                                    <asp:ImageButton runat="server" ID="imgbSAPImportVisible" Style="width: 1.5em; height: 1.5em;"
                                                                        ImageAlign="AbsMiddle" ImageUrl="../Images/plus.gif" />
                                                                    &nbsp;<strong>SAP-Bapi Import&nbsp;</strong>
                                                                </td>
                                                                <td align="left" valign="bottom">
                                                                    &nbsp;<asp:Label ID="lblSAPImportInfo" Font-Bold="True" runat="server" />
                                                                    &nbsp;<asp:Label ID="lblSAPImportNoData" runat="server" Visible="False" />
                                                                    &nbsp;<asp:Label ID="lblSAPImportError" runat="server" EnableViewState="False" CssClass="TextError" />
                                                                </td>
                                                                <td align="right">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <asp:Panel ID="panelSAPImport" Visible="false" BorderColor="Gray" BorderStyle="Solid"
                                                            BorderWidth="2" Width="100%" runat="server" Height="100%">
                                                            <telerik:RadGrid ID="SAPImportDG" runat="server" Width="100%" CellPadding="0" AllowSorting="True"
                                                                Visible="True" AllowPaging="True" AutoGenerateColumns="False" BackColor="White"
                                                                BorderStyle="None" Culture="de-DE" OnNeedDataSource="SAPImportDG_NeedDataSource"
                                                                OnDetailTableDataBind="SAPImportDG_DetailTableDataBind" PageSize="20">
                                                                <ClientSettings>
                                                                    <Resizing AllowColumnResize="true" ClipCellContentOnResize="true" />
                                                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="300" />
                                                                </ClientSettings>
                                                                <ExportSettings HideStructureColumns="true" Excel-Format="ExcelML" ExportOnlyData="true"
                                                                    IgnorePaging="true" FileName="SAP-Bapi-Import" />
                                                                <ItemStyle Wrap="false" />
                                                                <FooterStyle CssClass="RADGridFooter" />
                                                                <PagerStyle CssClass="RADGridFooter" AlwaysVisible="true" Mode="NextPrevAndNumeric"
                                                                    PagerTextFormat="{4} Seite <strong>{0}</strong> von <strong>{1}</strong>, insgesamt <strong>{5}</strong> Einträge" />
                                                                <MasterTableView CommandItemDisplay="Top" DataKeyNames="PARAMETER">
                                                                    <CommandItemSettings ShowExportToWordButton="false" ShowExportToExcelButton="true"
                                                                        ShowExportToCsvButton="false" ShowExportToPdfButton="false" ShowAddNewRecordButton="false" />
                                                                    <%-- TODO: re-Enable Export To Excel --%>
                                                                    <DetailTables>
                                                                        <telerik:GridTableView DataKeyNames="SpaltenName" Name="Spalten" Width="100%" AllowPaging="false">
                                                                            <Columns>
                                                                                <telerik:GridBoundColumn DataField="SpaltenName" HeaderText="Spalten Name" />
                                                                                <telerik:GridBoundColumn DataField="SpaltenTyp" HeaderText="Spalten Datentyp" />
                                                                                <telerik:GridBoundColumn DataField="Laenge" HeaderText="Länge" />
                                                                            </Columns>
                                                                        </telerik:GridTableView>
                                                                    </DetailTables>
                                                                    <Columns>
                                                                        <telerik:GridBoundColumn DataField="PARAMETER" SortExpression="PARAMETER" UniqueName="PARAMETER"
                                                                            HeaderText="Parameter Name" />
                                                                        <telerik:GridBoundColumn DataField="ParameterDATATYPE" SortExpression="ParameterDATATYPE"
                                                                            UniqueName="ParameterDATATYPE" HeaderText="Parameter Datentyp" />
                                                                        <telerik:GridBoundColumn DataField="ParameterLength" SortExpression="ParameterLength"
                                                                            UniqueName="ParameterLength" HeaderText="Parameter Länge" />
                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                        </asp:Panel>
                                                    </td>
                                                    <td>
                                                        &nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                            <tr class="TextLarge">
                                                                <td nowrap="nowrap" valign="bottom" width="25%">
                                                                    <asp:ImageButton runat="server" ID="imgbSAPExportVisible" Style="width: 1.5em; height: 1.5em;"
                                                                        ImageAlign="AbsMiddle" ImageUrl="../Images/plus.gif" />
                                                                    &nbsp;<strong>SAP-Bapi Export</strong>
                                                                </td>
                                                                <td align="left" valign="bottom">
                                                                    &nbsp;<asp:Label ID="lblSAPExportInfo" Font-Bold="True" runat="server" />
                                                                    &nbsp;<asp:Label ID="lblSAPExportNoData" runat="server" Visible="False" />
                                                                    &nbsp;<asp:Label ID="lblSAPExportError" runat="server" EnableViewState="False" CssClass="TextError" />
                                                                </td>
                                                                <td align="right">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <asp:Panel ID="panelSAPExport" Visible="false" BorderColor="Gray" BorderStyle="Solid"
                                                            BorderWidth="2" Width="100%" runat="server" Height="100%">
                                                            <telerik:RadGrid ID="SAPExportDG" runat="server" Width="100%" CellPadding="0" AllowSorting="True"
                                                                Visible="True" AllowPaging="True" AutoGenerateColumns="False" BackColor="White"
                                                                BorderStyle="None" Culture="de-DE" OnNeedDataSource="SAPExportDG_NeedDataSource"
                                                                OnDetailTableDataBind="SAPExportDG_DetailTableDataBind" PageSize="20">
                                                                <ClientSettings>
                                                                    <Resizing AllowColumnResize="true" ClipCellContentOnResize="true" />
                                                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="300" />
                                                                </ClientSettings>
                                                                <ExportSettings HideStructureColumns="true" Excel-Format="ExcelML" ExportOnlyData="true"
                                                                    IgnorePaging="true" FileName="SAP-Bapi-Export" />
                                                                <ItemStyle Wrap="false" />
                                                                <FooterStyle CssClass="RADGridFooter" />
                                                                <PagerStyle CssClass="RADGridFooter" AlwaysVisible="true" Mode="NextPrevAndNumeric"
                                                                    PagerTextFormat="{4} Seite <strong>{0}</strong> von <strong>{1}</strong>, insgesamt <strong>{5}</strong> Einträge" />
                                                                <MasterTableView CommandItemDisplay="Top" DataKeyNames="PARAMETER">
                                                                    <CommandItemSettings ShowExportToWordButton="false" ShowExportToExcelButton="true"
                                                                        ShowExportToCsvButton="false" ShowExportToPdfButton="false" ShowAddNewRecordButton="false" />
                                                                    <%-- TODO: re-Enable Export To Excel --%>
                                                                    <DetailTables>
                                                                        <telerik:GridTableView DataKeyNames="SpaltenName" Name="Spalten" Width="100%" AllowPaging="false">
                                                                            <Columns>
                                                                                <telerik:GridBoundColumn DataField="SpaltenName" HeaderText="Spalten Name" />
                                                                                <telerik:GridBoundColumn DataField="SpaltenTyp" HeaderText="Spalten Datentyp" />
                                                                                <telerik:GridBoundColumn DataField="Laenge" HeaderText="Länge" />
                                                                            </Columns>
                                                                        </telerik:GridTableView>
                                                                    </DetailTables>
                                                                    <Columns>
                                                                        <telerik:GridBoundColumn DataField="PARAMETER" SortExpression="PARAMETER" UniqueName="PARAMETER"
                                                                            HeaderText="Parameter Name" />
                                                                        <telerik:GridBoundColumn DataField="ParameterDATATYPE" SortExpression="ParameterDATATYPE"
                                                                            UniqueName="ParameterDATATYPE" HeaderText="Parameter Datentyp" />
                                                                        <telerik:GridBoundColumn DataField="ParameterLength" SortExpression="ParameterLength"
                                                                            UniqueName="ParameterLength" HeaderText="Parameter Länge" />
                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                        </asp:Panel>
                                                    </td>
                                                    <td>
                                                        &nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                            <tr class="TextLarge">
                                                                <td nowrap="nowrap" valign="bottom" width="25%">
                                                                    <asp:ImageButton runat="server" ID="imgbSAPTabellenVisible" Style="width: 1.5em;
                                                                        height: 1.5em;" ImageAlign="AbsMiddle" ImageUrl="../Images/plus.gif" />
                                                                    &nbsp;<strong>SAP-Bapi Tabellen</strong></span>
                                                                </td>
                                                                <td align="left" valign="bottom">
                                                                    &nbsp;<asp:Label ID="lblSAPTabellenInfo" Font-Bold="True" runat="server" />
                                                                    &nbsp;<asp:Label ID="lblSAPTabellenNoData" runat="server" Visible="False" />
                                                                    &nbsp;<asp:Label ID="lblSAPTabellenError" runat="server" EnableViewState="False"
                                                                        CssClass="TextError" />
                                                                </td>
                                                                <td align="right">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <asp:Panel ID="panelSAPTabellen" Visible="false" BorderColor="Gray" BorderStyle="Solid"
                                                            BorderWidth="2" Width="100%" runat="server" Height="100%">
                                                            <telerik:RadGrid ID="SAPTabellenDG" runat="server" Width="100%" CellPadding="0" AllowSorting="True"
                                                                Visible="True" AllowPaging="True" AutoGenerateColumns="False" BackColor="White"
                                                                BorderStyle="None" Culture="de-DE" OnNeedDataSource="SAPTabellenDG_NeedDataSource"
                                                                OnDetailTableDataBind="SAPTabellenDG_DetailTableDataBind" PageSize="20">
                                                                <ClientSettings>
                                                                    <Resizing AllowColumnResize="true" ClipCellContentOnResize="true" />
                                                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="300" />
                                                                </ClientSettings>
                                                                <ExportSettings HideStructureColumns="true" Excel-Format="ExcelML" ExportOnlyData="true"
                                                                    IgnorePaging="true" FileName="SAP-Bapi-Tabellen" />
                                                                <ItemStyle Wrap="false" />
                                                                <FooterStyle CssClass="RADGridFooter" />
                                                                <PagerStyle CssClass="RADGridFooter" AlwaysVisible="true" Mode="NextPrevAndNumeric"
                                                                    PagerTextFormat="{4} Seite <strong>{0}</strong> von <strong>{1}</strong>, insgesamt <strong>{5}</strong> Einträge" />
                                                                <MasterTableView CommandItemDisplay="Top" DataKeyNames="TabellenName">
                                                                    <CommandItemSettings ShowExportToWordButton="false" ShowExportToExcelButton="true"
                                                                        ShowExportToCsvButton="false" ShowExportToPdfButton="false" ShowAddNewRecordButton="false" />
                                                                    <%-- TODO: re-Enable Export To Excel --%>
                                                                    <DetailTables>
                                                                        <telerik:GridTableView DataKeyNames="SpaltenName" Name="Spalten" Width="100%" AllowPaging="false">
                                                                            <Columns>
                                                                                <telerik:GridBoundColumn DataField="SpaltenName" HeaderText="Spalten Name" />
                                                                                <telerik:GridBoundColumn DataField="SpaltenTyp" HeaderText="Spalten Datentyp" />
                                                                                <telerik:GridBoundColumn DataField="Laenge" HeaderText="Länge" />
                                                                            </Columns>
                                                                        </telerik:GridTableView>
                                                                    </DetailTables>
                                                                    <Columns>
                                                                        <telerik:GridBoundColumn DataField="TabellenName" SortExpression="TabellenName" UniqueName="TabellenName"
                                                                            HeaderText="Tabellen Name" />
                                                                        <telerik:GridBoundColumn DataField="Tabellengroesse" SortExpression="Tabellengroesse"
                                                                            UniqueName="Tabellengroesse" HeaderText="Tabellengröße" />
                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                        </asp:Panel>
                                                    </td>
                                                    <td>
                                                        &nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="border-top-color: blue; border-top-style: solid; border-width: 3;">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
