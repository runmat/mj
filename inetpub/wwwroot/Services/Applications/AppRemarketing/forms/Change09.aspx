<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Change09.aspx.cs" Inherits="AppRemarketing.forms.Change09" MasterPageFile="../Master/AppMaster.Master" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu" style="margin-top: 10px; margin-bottom: 10px">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" OnClick="lbBack_Click" CausesValidation="False"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" >
                    <ClientEvents OnRequestStart="onRequestStart" />
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="innerContentRight">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="innerContentRight" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                    </AjaxSettings>
                </telerik:RadAjaxManager>
                <script type="text/javascript">
                    function onRequestStart(sender, args) {
                        if (args.get_eventTarget().indexOf("ExportToExcelButton") >= 0) {
                            args.set_enableAjax(false);
                        }
                    }
                    function openinfo(url) {
                        fenster = window.open(url, "Uploadstruktur", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0,width=750,height=350");
                        fenster.focus();
                    }
                </script>
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading" style="float: none">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:Label ID="lblError" runat="server" CssClass="TextError" ></asp:Label>
                    <div id="paginationQuery" style="float: none">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="active">
                                    Neue Abfrage
                                </td>
                                <td align="right">
                                    <div id="queryImage">
                                        <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="../../../Images/queryArrow.gif"
                                            ToolTip="Abfrage öffnen" Visible="false" OnClick="NewSearch_Click" />
                                        <asp:ImageButton ID="NewSearchUp" runat="server" ToolTip="Abfrage schließen" ImageUrl="../../../Images/queryArrowUp.gif"
                                            Visible="false" OnClick="NewSearchUp_Click" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:Panel ID="Panel1" runat="server" DefaultButton="lbCreate">
                        <div id="TableQuery" runat="server" style="margin-bottom: 10px">
                            <table cellpadding="0" cellspacing="0">
                                <tr class="formquery" style="border-bottom-style:solid;border-bottom-width:1px">
                                    <td class="firstLeft active" style="width:120px">
                                        Auswahl:</td>
                                    <td class="active">
                                        <span>
                                    <asp:RadioButton ID="rbUpload" runat="server" AutoPostBack="True" Checked="True" 
                                        GroupName="Auswahl" OnCheckedChanged="Auswahl_CheckedChanged" 
                                        Text="Upload" />
                                    <asp:RadioButton ID="rbEinzelerfassung" runat="server" AutoPostBack="True" 
                                        GroupName="Auswahl" OnCheckedChanged="Auswahl_CheckedChanged" 
                                        Text="Einzelerfassung" />
                                    </span>
                                        </td>
                                </tr>
                                <tr id="tr_Upload" runat="server" class="formquery">
                                    <td class="firstLeft active">
                                        Upload:
                                    </td>
                                    <td class="active">
                                        <input ID="upFileFin" runat="server" name="upFileFin" size="49" type="file" />
                                        <a href="javascript:openinfo('InfoUpload3.htm');">
                                        <img alt="Struktur Uploaddatei" border="0" height="16px" 
                                            src="/Services/Images/info.gif" title="Struktur Uploaddatei" 
                                            width="16px" /></a> &nbsp; * max. 
                                        900 Datensätze
                                    </td>
                                </tr>
                                <tr id="tr_Fin" runat="server" class="formquery" visible="false">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        Fahrgestellnummer
                                    </td>
                                    <td class="active" nowrap="nowrap">
                                        <asp:TextBox ID="txtFin" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ControlToValidate="txtFin" ID="rfvFin" runat="server"
                                            ErrorMessage="Eingabe erforderlich."></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr id="tr_Kennzeichen" runat="server" class="formquery" visible="false">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        Kennzeichen
                                    </td>
                                    <td class="active" nowrap="nowrap">
                                        <asp:TextBox ID="txtKennzeichen" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ControlToValidate="txtKennzeichen" ID="rfvKennzeichen"
                                            runat="server" ErrorMessage="Eingabe erforderlich."></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr id="tr_Beschreibung" runat="server" class="formquery" visible="false">
                                    <td class="firstLeft active" nowrap="nowrap" style="vertical-align:top">
                                        Beschreibung
                                    </td>
                                    <td class="active" nowrap="nowrap" style="vertical-align:top">
                                        <asp:TextBox ID="txtBeschreibung" runat="server" Rows="3" TextMode="MultiLine" 
                                            Width="450px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ControlToValidate="txtBeschreibung" ID="rfvBeschreibung"
                                            runat="server" ErrorMessage="Eingabe erforderlich."></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr id="tr_Preis" runat="server" class="formquery" visible="false">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        Preis ohne Währung
                                    </td>
                                    <td class="active" nowrap="nowrap">
                                        <asp:TextBox ID="txtPreis" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ControlToValidate="txtPreis" ID="rfcPreis" runat="server"
                                            ErrorMessage="Eingabe erforderlich."></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr id="tr_Schadensdatum" runat="server" class="formquery" visible="false">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        Schadensdatum
                                    </td>
                                    <td class="active" nowrap="nowrap">
                                        <asp:TextBox ID="txtDatum" runat="server"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CE_Datumvon" runat="server" Format="dd.MM.yyyy"
                                            PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtDatum">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MEE_Datum" runat="server" TargetControlID="txtDatum"
                                            Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                        </cc1:MaskedEditExtender>
                                        <asp:RequiredFieldValidator ControlToValidate="txtDatum" ID="rfvDatum" runat="server"
                                            ErrorMessage="Eingabe erforderlich."></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr id="tr_Repariert" runat="server" class="formquery" visible="false">
                                    <td class="firstLeft active" nowrap="nowrap" style="vertical-align:top">
                                        Repariert
                                    </td>
                                    <td class="active" nowrap="nowrap" style="vertical-align:top">
                                        <asp:CheckBox ID="cbxRepariert" runat="server"/>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                &nbsp;
                            </div>
                        </div>
                    </asp:Panel>
                    <div id="dataQueryFooter">
                        <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px"
                            OnClientClick="Show_BusyBox1();" OnClick="lbCreate_Click">» Laden </asp:LinkButton>
                    </div>
                    <div id="Result" runat="Server" visible="false">
                        <table cellspacing="0" cellpadding="0" style="width: 100%" bgcolor="white" border="0">
                            <tr>
                                <td>
                                    <div style="width: 100%; max-width: 909px">        
                                        <telerik:RadGrid ID="rgGrid1" runat="server" PageSize="15" AllowSorting="True" 
                                            AutoGenerateColumns="False" GridLines="None" Culture="de-DE" EnableHeaderContextMenu="true" 
                                            OnExcelMLExportRowCreated="rgGrid1_ExcelMLExportRowCreated" 
                                            OnExcelMLExportStylesCreated="rgGrid1_ExcelMLExportStylesCreated" 
                                            OnItemCommand="rgGrid1_ItemCommand" OnItemCreated="rgGrid1_ItemCreated" 
                                            OnNeedDataSource="rgGrid1_NeedDataSource" 
                                            OnItemDataBound="rgGrid1_ItemDataBound" ShowGroupPanel="True" >
                                            <ExportSettings HideStructureColumns="true">
                                                <Excel Format="ExcelML" />
                                            </ExportSettings>
                                            <ClientSettings AllowColumnsReorder="true" AllowKeyboardNavigation="true" AllowDragToGroup="true">
                                                <Scrolling ScrollHeight="480px" AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="1" />
                                                <Resizing AllowColumnResize="True" ClipCellContentOnResize="False" />
                                            </ClientSettings>
                                            <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Auto" AllowPaging="true" 
                                                CommandItemDisplay="Top" >
                                                <PagerStyle Mode="NextPrevAndNumeric" ></PagerStyle>
                                                <CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false"
                                                    ExportToExcelText="Excel herunterladen">
                                                </CommandItemSettings>
                                                <SortExpressions>
                                                    <telerik:GridSortExpression FieldName="Fahrgestellnummer" SortOrder="Ascending" />
                                                </SortExpressions>
                                                <HeaderStyle ForeColor="White" />
                                                <Columns>
                                                    <telerik:GridTemplateColumn DataField="Fahrgestellnummer" SortExpression="Fahrgestellnummer" Groupable="false" >
                                                        <HeaderStyle Width="120px" />
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblFahrgestellnummer" Text='<%# Eval("Fahrgestellnummer") %>'>
                                                            </asp:Label>
                                                            <asp:TextBox ID="txtFin" runat="server" Visible="False" 
                                                                Text='<%# Eval("Fahrgestellnummer") %>' 
                                                                BorderColor="Red" Width="160px" BorderWidth="1px">
                                                            </asp:TextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn DataField="Kennzeichen" SortExpression="Kennzeichen" Groupable="false">
                                                        <HeaderStyle Width="120px" />
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblKennzeichen" Text='<%# Eval("Kennzeichen") %>'>
                                                            </asp:Label>
                                                            <asp:TextBox ID="txtKennzeichen" runat="server" Visible="False" 
                                                                Text='<%# Eval("Kennzeichen") %>' 
                                                                BorderColor="Red" Width="100px" BorderWidth="1px">
                                                            </asp:TextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="Beschreibung" SortExpression="Beschreibung" >
                                                        <HeaderStyle Width="100px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Betrag" SortExpression="Betrag" DataFormatString="{0:c}" >
                                                        <HeaderStyle Width="80px" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn DataField="Schadensdatum" SortExpression="Schadensdatum" >
                                                        <HeaderStyle Width="105px" />
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblSchadensdatum" Text='<%# Eval("Schadensdatum") %>'>
                                                            </asp:Label>
                                                            <asp:TextBox ID="txtSchadensdatum" runat="server" Visible="False" 
                                                                Text='<%# Eval("Schadensdatum") %>' 
                                                                BorderColor="Red" Width="100px" BorderWidth="1px">
                                                            </asp:TextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="Repariert" SortExpression="Repariert" >
                                                        <HeaderStyle Width="80px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ZBEM" SortExpression="ZBEM" Visible="false" UniqueName="Bemerkung" >
                                                        <HeaderStyle Width="100px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ID" SortExpression="ID" Visible="false" UniqueName="ID" >
                                                    </telerik:GridBoundColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="dataFooter" style="float:right;width:100%;text-align:right">
                        <div style="float:right;width:100%;text-align:right">
                            <asp:LinkButton ID="lbSend" runat="server" CssClass="Tablebutton"
                                Width="78px" onclick="lbSend_Click" Visible ="false" style="margin-top:10px;margin-bottom:5px">» Senden</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
