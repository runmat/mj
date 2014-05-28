<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change10_Report.aspx.vb" Inherits="AppAvis.Change10_Report" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>    
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
     <style type="text/css">
        .SelectTable tr td{
            padding: 3px 0px 5px 0px;      
            }
        .SearchTable tbody{
            border: solid 1px black;        
        }
        .SearchTable tr td{
            padding: 5px 0px 5px 10px;
            background-color: #cccccc;
            }
    </style>  
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <ClientEvents OnRequestStart="onRequestStart" />
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rgAuslastung">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgAuslastung" />
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
    </script>
    <table id="Table4" width="100%" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table0" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label>
                            <asp:Label ID="lblPageTitle" runat="server"> (Auswahl)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120">
                           <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                border="0">
                                <tr>
                                    <td class="TaskTitle" width="150">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdCreate" runat="server" CssClass="StandardButton"> &#149;&nbsp;Erstellen</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdBack" runat="server" CssClass="StandardButton"> •&nbsp;Zurück</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top" rowspan="2">
                            <table id="tblSelect" cellspacing="0" cellpadding="0" width="100%" border="0" class="SelectTable">
                                <thead>
                                    <tr>
                                        <th class="TaskTitle" valign="top" align="left" width="100%">
                                            &nbsp;
                                        </th>                                      
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td valign="top">
                                            <table id="tblSearch" cellspacing="0" cellpadding="0" class="SearchTable">
                                                <tbody>
                                                    <tr>
                                                        <td valign="top">
                                                            <table cellspacing="0" cellpadding="0" class="SearchTable">
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="white-space: nowrap;" colspan="2">
                                                                        Carport:
                                                                    </td>
                                                                    <td>
                                                                        <telerik:RadTextBox ID="rtbCarport" runat="server" MaxLength="3" Width="50px">
                                                                        </telerik:RadTextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="white-space: nowrap;" colspan="2">
                                                                        Distrikt:
                                                                    </td>
                                                                    <td>
                                                                        <telerik:RadTextBox ID="rtbDistrikt" runat="server" MaxLength="2" Width="50px">
                                                                        </telerik:RadTextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="white-space: nowrap;">
                                                                        Datum Zulassung
                                                                    </td>
                                                                    <td>
                                                                        Von:
                                                                    </td>
                                                                    <td>
                                                                        <telerik:RadDatePicker ID="txtDatZulVon" runat="server">
                                                                        </telerik:RadDatePicker>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                    </td>
                                                                    <td style="padding-left: 10px;">
                                                                        Bis:
                                                                    </td>
                                                                    <td>
                                                                        <telerik:RadDatePicker ID="txtDatZulBis" runat="server">
                                                                        </telerik:RadDatePicker>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td rowspan="5" valign="top" width="100%">
                                                            Neuwagenauslieferung
                                                            <asp:ImageButton ID="ibtnExcelExport" runat="server" ImageUrl="..\..\..\Images\excel.gif"
                                                                Width="16" Height="16" />
                                                            <br />
                                                            <telerik:RadGrid ID="rgAuslastung" runat="server" PageSize="10" AllowPaging="true"
                                                                AllowSorting="true" AutoGenerateColumns="true" GridLines="None" CellSpacing="0"
                                                                Visible="true">
                                                                <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                                <ClientSettings AllowKeyboardNavigation="True">
                                                                    <Scrolling AllowScroll="false" UseStaticHeaders="True" FrozenColumnsCount="0" />
                                                                    <Resizing AllowColumnResize="true" AllowResizeToFit="True" ResizeGridOnColumnResize="false" />
                                                                </ClientSettings>
                                                                <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Fixed" GroupsDefaultExpanded="true">
                                                                    <CommandItemSettings ShowExportToExcelButton="False" ShowAddNewRecordButton="false"
                                                                        ExportToExcelText="Excel herunterladen" ShowRefreshButton="False"></CommandItemSettings>
                                                                    <SortExpressions>
                                                                        <telerik:GridSortExpression FieldName="STATION" SortOrder="Ascending" />
                                                                    </SortExpressions>
                                                                    <HeaderStyle ForeColor="#000000" />
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td height="100%">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>                                       
                                    </tr>                                    
                                </tbody>
                            </table>
                            <table id="tblResult" cellspacing="0" cellpadding="0" width="100%" height="100%" border="0" class="ResultTable">
                                <thead>
                                    <tr>
                                        <th class="TaskTitle" valign="top" width="100%" align="left" style="margin-left: 3px;">
                                            &nbsp;
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td width="100%" height="100%">
                                            <telerik:RadGrid ID="rgResult" runat="server" PageSize="200" AllowPaging="true" AllowSorting="true"
                                                AllowMultiRowSelection="false" AutoGenerateColumns="False" GridLines="None" CellSpacing="0"
                                                OnPageSizeChanged="rgResult_OnPageSizeChanged" Culture="de-DE">
                                                <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                <ExportSettings HideStructureColumns="true" ExportOnlyData="True">
                                                    <Excel Format="ExcelML" />
                                                </ExportSettings>
                                                <ClientSettings AllowKeyboardNavigation="True">
                                                    <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="True"/>
                                                    <Resizing AllowColumnResize="true" AllowResizeToFit="True" ResizeGridOnColumnResize="true" />
                                                </ClientSettings>
                                                <MasterTableView  GroupLoadMode="Client" TableLayout="Fixed" GroupsDefaultExpanded="true" Width="100%" 
                                                        CommandItemDisplay="Top">
                                                    <CommandItemSettings ShowExportToExcelButton="True" ShowAddNewRecordButton="false" 
                                                             ExportToExcelText="Excel herunterladen" ShowRefreshButton="False"/>                                                   
                                                    <HeaderStyle ForeColor="#000000" />
                                                    <Columns>
                                                        <telerik:GridBoundColumn SortExpression="CARPORT" DataField="CARPORT" HeaderText="Carport" /> 
                                                        <telerik:GridBoundColumn SortExpression="HERST_ID" DataField="HERST_ID" HeaderText="Herst. ID" />
                                                        <telerik:GridBoundColumn SortExpression="MODEL_ID" DataField="MODEL_ID" HeaderText="Model ID" />
                                                        <telerik:GridBoundColumn SortExpression="MODELLGRUPPE" DataField="MODELLGRUPPE" HeaderText="Modell- Gruppe" />
                                                        <telerik:GridBoundColumn SortExpression="BESCHREIBUNG" DataField="BESCHREIBUNG" HeaderText="Modell- Bezeichnung" />
                                                        <telerik:GridBoundColumn SortExpression="BESCHREIREIFENARTBUNG" DataField="REIFENART" HeaderText="Reifenart" />
                                                        <telerik:GridBoundColumn SortExpression="CHASSIS_NUM" DataField="CHASSIS_NUM" HeaderText="Fahrgestellnummer" ItemStyle-Width="150px" HeaderStyle-Width="150px" />
                                                        <telerik:GridBoundColumn SortExpression="EING_DAT" DataField="EING_DAT" HeaderText="Datum Eingang" DataFormatString="{0:dd.MM.yyyy}" />
                                                        <telerik:GridBoundColumn SortExpression="BEREIT_DAT" DataField="BEREIT_DAT" HeaderText="Datum Bereit" DataFormatString="{0:dd.MM.yyyy}" />
                                                        <telerik:GridBoundColumn SortExpression="ZB2NR" DataField="ZB2NR" HeaderText="ZBII Nummer" />
                                                        <telerik:GridBoundColumn SortExpression="KENNZ" DataField="KENNZ" HeaderText="Kennzeichen" />
                                                        <telerik:GridBoundColumn SortExpression="ZULDAT" DataField="ZULDAT" HeaderText="Zulassung" DataFormatString="{0:dd.MM.yyyy}"/>
                                                        <telerik:GridBoundColumn SortExpression="DISTRIKT" DataField="DISTRIKT" HeaderText="VGrp" FilterControlToolTip="Distrikt" />
                                                        <telerik:GridBoundColumn SortExpression="EX_KUNNR" DataField="EX_KUNNR" HeaderText="Station" />
                                                        <telerik:GridBoundColumn SortExpression="BEAUFDAT" DataField="BEAUFDAT" HeaderText="Termin" DataFormatString="{0:dd.MM.yyyy}" />
                                                        <telerik:GridBoundColumn SortExpression="SPEDITEUR" DataField="SPEDITEUR" HeaderText="Spediteur" />
                                                        <telerik:GridBoundColumn SortExpression="LFMONAT" DataField="LFMONAT" HeaderText="Liefermonat" />
                                                        <telerik:GridBoundColumn SortExpression="ZULASSUNGSORT" DataField="ZULASSUNGSORT" HeaderText="Zulassungsort" />                                                                                                           
                                                        <telerik:GridBoundColumn SortExpression="FZG_ART" DataField="FZG_ART" HeaderText="Fahrzeugart"/>
                                                        <telerik:GridBoundColumn SortExpression="AUFBAUART" DataField="AUFBAUART" HeaderText="Aufbauart" />
                                                        <telerik:GridBoundColumn SortExpression="KRAFTSTOFF" DataField="KRAFTSTOFF" HeaderText="Kraftstoffart" />
                                                        <telerik:GridBoundColumn SortExpression="NAVIGATION" DataField="NAVIGATION" HeaderText="Navi" />
                                                        <telerik:GridBoundColumn SortExpression="NAVI_CD" DataField="NAVI_CD" HeaderText="Navi CD" />
                                                        <telerik:GridBoundColumn SortExpression="FARBE_DE" DataField="FARBE_DE" HeaderText="Farbe" />
                                                        <telerik:GridBoundColumn SortExpression="VERMGRP" DataField="VERMGRP" HeaderText="Vermietgruppe" />
                                                        <telerik:GridBoundColumn SortExpression="VERWENDUNGSZWECK" DataField="VERWENDUNGSZWECK" HeaderText="Verwendungszweck" />
                                                        <telerik:GridBoundColumn SortExpression="BEZAHLTKENNZ" DataField="BEZAHLTKENNZ" HeaderText="Bezahltkennzeichen" />                                                                                                                  
                                                        <telerik:GridBoundColumn SortExpression="OWNER_CODE" DataField="OWNER_CODE" HeaderText="Owner-Code" />
                                                        <telerik:GridBoundColumn SortExpression="EINK_INDIKATOR" DataField="EINK_INDIKATOR" HeaderText="Einkaufsindikator" />
                                                    </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </td>
                                    </tr>
                                </tbody>                            
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
                        </td>
                        <td valign="top">
                            <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
                        </td>
                        <td>
                            <!--#include File="../../../PageElements/Footer.html" -->
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>