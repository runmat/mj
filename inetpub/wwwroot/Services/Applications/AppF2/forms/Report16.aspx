<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report16.aspx.vb" Inherits="AppF2.Report16"
    MasterPageFile="../MasterPage/AppMaster.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" CssClass="firstLeft active"
                    Text="zurück"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td class="active">
                                        <asp:Label ID="lblNewSearch" runat="server" Text="Neue Abfrage" Visible="False"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <div id="queryImage">
                                            <asp:ImageButton ID="ibtNewSearch" runat="server" ImageUrl="../../../Images/queryArrowUp.gif"
                                                OnClick="ibtNewSearch_Click" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <asp:Panel ID="divTrenn" runat="server" Visible="false">
                            <div id="PlaceHolderDiv">
                            </div>
                        </asp:Panel>
                    </div>
                    <asp:Panel ID="divSelection" runat="server" Visible="true">
                        <div id="TableQuery" style="margin-bottom: 10px">
                            <table runat="server" cellpadding="0" cellspacing="0" width="100%">
                                <tbody>
                                    <tr class="formquery">
                                        <td class="firstLeft active" colspan="3">
                                            <asp:Label ID="lblError" CssClass="TextError" runat="server" Font-Bold="True" Visible="false"
                                                EnableViewState="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td width="100px" class="firstLeft active" style="height: 22px;">
                                            <asp:Label ID="lbl_Berichtstyp" runat="server" Text="Reporttyp:" Width="100px" />
                                        </td>
                                        <td class="firstLeft active" style="height: 22px;">
                                            <asp:DropDownList ID="ddl_Berichtstyp" runat="server" Width="140px" AutoPostBack="True" OnSelectedIndexChanged="ddl_Berichtstyp_SelectedIndexChanged">
                                                <asp:ListItem Text="Tagesbericht" Value="Tagesbericht" Selected="True" />
                                                <asp:ListItem Text="Monatsbericht" Value="Monatsbericht" />
                                                <asp:ListItem Text="Quartalsbericht" Value="Quartalsbericht" />
                                                <asp:ListItem Text="Halbjahresbericht" Value="Halbjahresbericht" />
                                                <asp:ListItem Text="Jahresbericht" Value="Jahresbericht" />
                                                <asp:ListItem Text="Auswahl Periode" Value="Auswahl Periode" />
                                            </asp:DropDownList>
                                        </td> 
                                        <td style="padding-right: 10px; padding-bottom:5px;" width="100%" rowspan="3">
                                            <div class="new_layout">
                                                <div id="infopanel" class="infopanel">
                                                    <label>
                                                        Tipp!</label>
                                                    <div>
                                                        Zum Anzeigen der Liste bitte auf die Schaltfläche<br />
                                                        'Abrufen' klicken
                                                    </div>
                                                </div>
                                            </div>
                                        </td>                                       
                                    </tr>
                                    <tr id="trVonDatum" runat="server" class="formquery">
                                        <td width="100px" class="firstLeft active" style="height: 22px">
                                            <asp:Label ID="lbl_Datum_von" runat="server" Text="Report vom:" />
                                        </td>
                                        <td class="firstLeft active" style="height: 22px">
                                            <asp:TextBox ID="txt_Datum_von" runat="server" Width="100px" AutoPostBack="True" />
                                            <ajaxToolkit:CalendarExtender ID="txt_Datum_von_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="txt_Datum_von" Animated="False" Format="dd.MM.yyyy" />
                                        </td>
                                    </tr>
                                    <tr id="trBisDatum" runat="server" class="formquery" visible="false">
                                        <td width="100px" class="firstLeft active" style="height: 22px">
                                            <asp:Label ID="lbl_Datum_bis" runat="server" Text="Datum bis:" />
                                        </td>
                                        <td class="firstLeft active" style="height: 22px">
                                            <asp:TextBox ID="txt_Datum_bis" runat="server" Width="100px" AutoPostBack="True" />
                                            <ajaxToolkit:CalendarExtender ID="txt_Datum_bis_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="txt_Datum_bis" Animated="False" Format="dd.MM.yyyy" />
                                        </td>
                                    </tr>
                                    <tr id="trBerichtszeitraum" runat="server" class="formquery" visible="false">
                                        <td width="100px" class="firstLeft active" style="height: 22px">
                                            <asp:Label ID="lbl_ZeitraumBez" runat="server" Text="Monat:" />
                                        </td>
                                        <td class="firstLeft active" style="height: 22px">
                                            <asp:DropDownList ID="ddl_Zeitraum" runat="server" Width="50px" style="margin-right: 15px" AutoPostBack="True">
                                                <asp:ListItem Text="1" Value="1" Selected="True" />
                                                <asp:ListItem Text="2" Value="2" />
                                                <asp:ListItem Text="3" Value="3" />
                                                <asp:ListItem Text="4" Value="4" />
                                                <asp:ListItem Text="5" Value="5" />
                                                <asp:ListItem Text="6" Value="6" />
                                                <asp:ListItem Text="7" Value="7" />
                                                <asp:ListItem Text="8" Value="8" />
                                                <asp:ListItem Text="9" Value="9" />
                                                <asp:ListItem Text="10" Value="10" />
                                                <asp:ListItem Text="11" Value="11" />
                                                <asp:ListItem Text="12" Value="12" />
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txtJahr" runat="server" Width="40px" Text="2013" />
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtJahr" MaskType="Number" Mask="9999">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td width="100px" class="firstLeft active" style="height: 22px;">
                                            <asp:Label ID="lbl_Skill" runat="server" Text="Skill:" />
                                        </td>
                                        <td class="firstLeft active" style="height: 22px">
                                            <asp:DropDownList ID="ddl_Skill" runat="server" Width="100px" AutoPostBack="True">
                                                <asp:ListItem Text="I Targo" Value="I Targo" Selected="True" />
                                                <asp:ListItem Text="I Targo verm." Value="I Targo verm" />
                                            </asp:DropDownList>
                                        </td>                                        
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" colspan="3">
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div id="footer" runat="server" style="background-color: #dfdfdf; height: 22px;">
                            &nbsp;
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="PnResult" Visible="false" runat="server">
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
                        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                            <script type="text/javascript">
                        <!--

                                var hasChanges, inputs, dropdowns, editedRow;

                                function ShowColumnHeaderMenu(ev, columnName) {
                                    alert("");
                                    var grid = $find("<%=rgTelefon.ClientID %>");
                                    var columns = grid.get_masterTableView().get_columns();
                                    for (var i = 0; i < columns.length; i++) {
                                        if (columns[i].get_uniqueName() == columnName) {
                                            columns[i].showHeaderMenu(ev, 75, 20);
                                        }
                                    }
                                }

                         -->
                            </script>
                        </telerik:RadCodeBlock>
                        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                            <ClientEvents OnRequestStart="onRequestStart" />
                            <AjaxSettings>
                                <telerik:AjaxSetting AjaxControlID="rgTelefon">
                                    <UpdatedControls>
                                        <telerik:AjaxUpdatedControl ControlID="rgTelefon" LoadingPanelID="RadAjaxLoadingPanel1" />
                                        <telerik:AjaxUpdatedControl ControlID="RadWindowManager1" />
                                    </UpdatedControls>
                                </telerik:AjaxSetting>
                            </AjaxSettings>
                        </telerik:RadAjaxManager>
                        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
                        <telerik:RadGrid ID="rgTelefon" runat="server" GridLines="None" Width="100%" BorderWidth="0px"
                            Culture="de-DE" CellSpacing="0" AllowSorting="True" ClientSettings-AllowColumnsReorder="false"
                            ShowFooter="True" AutoGenerateColumns="False" OnNeedDataSource="rgTelefonNeedDataSource"
                            OnExcelMLExportRowCreated="rgTelefon_ExcelMLExportRowCreated" OnExcelMLExportStylesCreated="rgTelefon_ExcelMLExportStylesCreated"
                            AllowPaging="True" OnCustomAggregate="rgTelefonOnCustomAggregate">
                            <ExportSettings HideStructureColumns="true" IgnorePaging="True" OpenInNewWindow="True"
                                ExportOnlyData="true">
                                <Excel Format="ExcelML"></Excel>
                            </ExportSettings>
                            <MasterTableView DataKeyNames="" CommandItemDisplay="Top" Summary="Telefoniereport"
                                TableLayout="Auto" Width="100%" EditMode="InPlace" AllowAutomaticUpdates="True">
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
                                <RowIndicatorColumn Visible="True" FilterControlAltText="">
                                </RowIndicatorColumn>
                                <ExpandCollapseColumn Visible="false" FilterControlAltText="Filter ExpandColumn column">
                                </ExpandCollapseColumn>
                                <Columns>
                                    <telerik:GridBoundColumn HeaderStyle-Wrap="true" DataType="System.DateTime" UniqueName="datInterval" DataField="datInterval" SortExpression="datInterval"
                                        FooterText="Gesamt:" HeaderTooltip="Interval">
                                        <HeaderStyle Wrap="True" Width="120px" />
                                        <ItemStyle Wrap="false" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderStyle-Wrap="true" UniqueName="datSkill" DataField="datSkill" SortExpression="datSkill"
                                        FooterText=" " HeaderTooltip="Skill">
                                        <HeaderStyle Wrap="True"></HeaderStyle>
                                        <ItemStyle Wrap="false" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderStyle-Wrap="true" DataType="System.Int32" UniqueName="datSumAnr" SortExpression="datSumAnr"
                                        DataField="datSumAnr" Aggregate="Sum" FooterAggregateFormatString="{0:0.##}" FooterText=" " HeaderTooltip="Summe Anrufe">
                                        <HeaderStyle Wrap="True" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderStyle-Wrap="true" UniqueName="datAnzAng" DataField="datAnzAng" SortExpression="datAnzAng"
                                        DataType="System.Int32" Aggregate="Sum" FooterAggregateFormatString="{0:0.##}" FooterText=" " HeaderTooltip="Anzahl angenommene Anrufe">
                                        <HeaderStyle Wrap="True"></HeaderStyle>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderStyle-Wrap="true" UniqueName="datLost" DataField="datLost" SortExpression="datLost"
                                        DataType="System.Int32" Aggregate="Sum" FooterAggregateFormatString="{0:0.##}" FooterText=" " HeaderTooltip="Lost Calls">
                                        <HeaderStyle Wrap="True" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderStyle-Wrap="true" UniqueName="datAnzAngK20" DataField="datAnzAngK20" SortExpression="datAnzAngK20"
                                        DataType="System.Int32" Aggregate="Sum" FooterAggregateFormatString="{0:0.##}" FooterText=" " HeaderTooltip="Anzahl angenommene Anrufe <= 20 Sekunden">
                                        <HeaderStyle Wrap="True" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderStyle-Wrap="true" UniqueName="datAnzAngG20" DataField="datAnzAngG20" SortExpression="datAnzAngG20"
                                        DataType="System.Int32" Aggregate="Sum" FooterAggregateFormatString="{0:0.##}" FooterText=" " HeaderTooltip="Anzahl angenommene Anrufe > 20 Sekunden">
                                        <HeaderStyle Wrap="True" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderStyle-Wrap="true" UniqueName="datSlIn20Sec" DataField="datSlIn20Sec" SortExpression="datSlIn20Sec"
                                        DataType="System.Double" Aggregate="Custom" FooterAggregateFormatString="{0:0.##}" FooterText=" " HeaderTooltip="Servicelevel: Annahme mind. 80% aller Anrufe in weniger als 20 Sekunden">
                                        <HeaderStyle CssClass="StyleGesamt" ForeColor="Red" Wrap="True" />
                                        <FooterStyle ForeColor="Red" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderStyle-Wrap="true" UniqueName="datSlGesamt" DataField="datSlGesamt" SortExpression="datSlGesamt"
                                        DataType="System.Double" Aggregate="Custom" FooterAggregateFormatString="{0:0.##}" FooterText=" " HeaderTooltip="Servicelevel gesamt">
                                        <HeaderStyle Wrap="True" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderStyle-Wrap="true" UniqueName="datGDauerMin" DataField="datGDauerMin" SortExpression="datGDauerMin"
                                        DataType="System.Int32" Aggregate="Custom" FooterAggregateFormatString="{0:0.##}" FooterText=" " HeaderTooltip="Gesprächsdauer Minimum (in Sekunden)">
                                        <HeaderStyle Wrap="True" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderStyle-Wrap="true" UniqueName="datGDauerMax" DataField="datGDauerMax" SortExpression="datGDauerMax"
                                        DataType="System.Int32" Aggregate="Max" FooterAggregateFormatString="{0:0.##}" FooterText=" " HeaderTooltip="Gesprächsdauer Maximum (in Sekunden)">
                                        <HeaderStyle Wrap="True" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderStyle-Wrap="true" UniqueName="datGDauerDs" DataField="datGDauerDs" SortExpression="datGDauerDs"
                                        DataType="System.Int32" Aggregate="Custom" FooterAggregateFormatString="{0:0.##}" FooterText=" " HeaderTooltip="Gesprächsdauer durchschn. (in Sekunden)">
                                        <HeaderStyle Wrap="True" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderStyle-Wrap="true" UniqueName="datWDauerMin" DataField="datWDauerMin" SortExpression="datWDauerMin"
                                        DataType="System.Int32" Aggregate="Custom" FooterAggregateFormatString="{0:0.##}" FooterText=" " HeaderTooltip="Wartedauer Minimum (in Sekunden)">
                                        <HeaderStyle Wrap="True" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderStyle-Wrap="true" UniqueName="datWDauerMax" DataField="datWDauerMax" SortExpression="datWDauerMax"
                                        DataType="System.Int32" Aggregate="Max" FooterAggregateFormatString="{0:0.##}" FooterText=" " HeaderTooltip="Wartedauer Maximum (in Sekunden)">
                                        <HeaderStyle Wrap="True" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderStyle-Wrap="true" UniqueName="datWDauerDs" DataField="datWDauerDs" SortExpression="datWDauerDs"
                                        DataType="System.Int32" Aggregate="Custom" FooterAggregateFormatString="{0:0.##}" FooterText=" "
                                        HeaderTooltip="Wartedauer durchschn. (in Sekunden)">
                                        <HeaderStyle Wrap="True" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderStyle-Wrap="true" UniqueName="datWzVorAuflegenMin" DataField="datWzVorAuflegenMin"
                                        SortExpression="datWzVorAuflegenMin" DataType="System.Int32" Aggregate="Custom" FooterAggregateFormatString="{0:0.##}"
                                        FooterText=" " HeaderTooltip="Wartezeit vor Auflegen Minimum (in Sekunden)">
                                        <HeaderStyle Wrap="True"></HeaderStyle>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderStyle-Wrap="true" UniqueName="datWzVorAuflegenMax" DataField="datWzVorAuflegenMax"
                                        SortExpression="datWzVorAuflegenMax" DataType="System.Int32" Aggregate="Max" FooterAggregateFormatString="{0:0.##}"
                                        FooterText=" " HeaderTooltip="Wartezeit vor Auflegen Maximum (in Sekunden)">
                                        <HeaderStyle Wrap="True" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderStyle-Wrap="true" UniqueName="datWzVorAuflegenDs" DataField="datWzVorAuflegenDs"
                                        SortExpression="datWzVorAuflegenDs" DataType="System.Int32" Aggregate="Custom" FooterAggregateFormatString="{0:0.##}"
                                        FooterText=" " HeaderTooltip="Wartezeit vor Auflegen durchschn. (in Sekunden)">
                                        <HeaderStyle Wrap="True" />
                                    </telerik:GridBoundColumn>
                                </Columns>
                                <EditFormSettings>
                                    <EditColumn ButtonType="ImageButton" InsertText="Insert Order" UpdateText="Update record"
                                        UniqueName="EditCommandColumn1" CancelText="Cancel edit">
                                    </EditColumn>
                                </EditFormSettings>
                            </MasterTableView>
                            <HeaderStyle CssClass="RadGridHeader" Wrap="true" />
                            <FooterStyle ForeColor="DarkBlue" Font-Bold="true" />
                            <PagerStyle AlwaysVisible="True" />
                            <FilterMenu EnableImageSprites="False" />
                            <ClientSettings AllowKeyboardNavigation="True">
                                <Scrolling AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="1" />
                                <Resizing AllowColumnResize="True" AllowResizeToFit="True" ResizeGridOnColumnResize="True" />
                            </ClientSettings>
                        </telerik:RadGrid>
                        <telerik:GridTextBoxColumnEditor ID="GridTextBoxColumnEditor1" runat="server" TextBoxStyle-Width="180px" />
                        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" />
                    </asp:Panel>
                    <div id="DataFooter">
                        <table width="100%">
                            <tr>
                                <td align="right">
                                    <br />
                                    <asp:LinkButton ID="lb_Execute" runat="server" CssClass="Tablebutton" Width="78px"
                                        Height="16px" CausesValidation="False" Font-Underline="False" OnClick="ExecuteClick">» Abrufen</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="Div3">
                        &nbsp;
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
