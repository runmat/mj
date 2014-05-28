<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change10_Korrektur_2.aspx.vb" Inherits="AppAvis.Change10_Korrektur_2" EnableEventValidation="false"%>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
    <style type="text/css">
        .SelectTable tr td
        {
            padding: 3px 0px 5px 0px;
        }
        .SearchTable tbody
        {
            border: solid 1px black;
        }
        .SearchTable tr td
        {
            padding: 5px 0px 5px 10px;
            background-color: #cccccc;
        }
        .TableHead
        {
            padding: 5px;
            text-align: left;
            color: #FFFFFF;
            background-color: #CC0033;
        }
    </style>
    <script type="text/javascript">
        function StationscodeChanged(sender, args) {
            var station = args.get_newValue().toUpperCase();
            var stationOld = args.get_oldValue();
            var objDatum = $get("rdpTermin_dateInput_text");
            var datum = "";

            if (objDatum != null) {
                datum = objDatum.value;
            }

            if (station != null && station != "") {
                RowsSelected(datum, station);
                if (stationOld != null && stationOld != "") {
                    RowsDeselected(datum, stationOld);
                }
            } else {
                if (stationOld != null && stationOld != "") {
                    RowsDeselected(datum,stationOld);
                }
            }

            //__doPostBack("<%=rtbStationscode.ClientID %>", null);
        }

        function DateChanged(sender, args) {
            var datum = args.get_newValue();
            var datumOld = args.get_oldValue();
            var objStation = $get("rtbStationscode_text");
            var station = "";
            
            if (objStation != null) {
                station = objStation.value.toUpperCase();
            }

            if (datum != null && datum != "" && datum.length==10) {
                RowsSelected(datum, station);
                if (datumOld != null && datumOld != "" && datumOld.length == 10) {
                    RowsDeselected(datumOld, station);
                }
                if (checkAnlieferdatumOk(datum) == false) {
                    alert("Achtung, es wurden Fahrzeuge für diese Auslieferung ausgewählt, deren Auslieferungsdatum vor dem Zulassungsdatum liegt!");
                }
            } else {
                if (datumOld != null && datumOld != "" && datumOld.length == 10) {
                    RowsDeselected(datumOld,station);
                }
            }
        }

        function RowSelected(sender, args) {
            //            var rowindex = args._itemIndexHierarchical;
            //            var item = args.get_gridDataItem();
            var objStation = $get("rtbStationscode_text");
            var station = "";
            var objDatum = $get("rdpTermin_dateInput_text");
            var datum = "";

            if (objStation != null) {
                station = objStation.value.toUpperCase();
            }

            if (objDatum != null) {
                datum = objDatum.value;
            }

            if (datum != "" && station != "") {
                var tbl = $find("<%=rgAuslastung.ClientID %>");
                var mtv = tbl.get_masterTableView();
                var items = mtv.get_dataItems();

                for (var i = 0; i < items.length; i++) {

                    var row = mtv.get_dataItems()[i];
                    if (row.get_cell("STATION").innerHTML == station) {
                        var cell = row.get_cell(datum + " Dispo");
                        if (cell != null) {
                            var count = cell.innerHTML;
                            count = parseInt(count);
                            count++;
                            cell.innerHTML = count;
                        }
                        break;
                    }
                }
            }
        }

        function RowsSelected(datum, station) {
            var itemsResult = getSelectedItemsOfTable("<%=rgResult.ClientID %>");
            if (itemsResult != null) {
                for (var j = 0; j < itemsResult.length; j++) {

                    //var itemResult = itemsResult[j];

                    if (datum != null && datum != "") {
                        var itemsAuslastung = getItemsOfTable("<%=rgAuslastung.ClientID %>");

                        for (var i = 0; i < itemsAuslastung.length; i++) {

                            var row = itemsAuslastung[i];
                            if (row.get_cell("STATION").innerHTML == station) {
                                var cell = row.get_cell(datum + " Dispo");
                                if (cell != null) {
                                    var count = cell.innerHTML;
                                    count = parseInt(count);
                                    count++;
                                    cell.innerHTML = count;
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        function RowDeselected(sender, args) {
            //            var rowindex = args._itemIndexHierarchical;
            //            var item = args.get_gridDataItem();
            var objStation = $get("rtbStationscode_text");
            var station = "";
            var objDatum = $get("rdpTermin_dateInput_text");
            var datum = "";

            if (objStation != null) {
                station = objStation.value.toUpperCase();
            }

            if (objDatum != null) {
                datum = objDatum.value;
            }

            if (datum != "" && station != "") {
                var tbl = $find("<%=rgAuslastung.ClientID %>");
                var mtv = tbl.get_masterTableView();
                var items = mtv.get_dataItems();

                for (var i = 0; i < items.length; i++) {

                    var row = mtv.get_dataItems()[i];
                    if (row.get_cell("STATION").innerHTML == station) {
                        var cell = row.get_cell(datum + " Dispo");
                        if (cell != null) {
                            var count = cell.innerHTML;
                            count = parseInt(count);
                            count--;
                            cell.innerHTML = count;
                        }
                        break;
                    }
                }
            }
        }

        function RowsDeselected(datum, station) {
            var itemsResult = getSelectedItemsOfTable("<%=rgResult.ClientID %>");
            if (itemsResult != null) {
                for (var j = 0; j < itemsResult.length; j++) {

                    //var itemResult = itemsResult[j];                    

                    if (datum != null && datum != "") {
                        var itemsAuslastung = getItemsOfTable("<%=rgAuslastung.ClientID %>");

                        for (var i = 0; i < itemsAuslastung.length; i++) {
                            var row = itemsAuslastung[i];
                            if (row.get_cell("STATION").innerHTML == station) {
                                var cell = row.get_cell(datum + " Dispo");
                                if (cell != null) {
                                    var count = cell.innerHTML;
                                    count = parseInt(count);
                                    count--;
                                    cell.innerHTML = count;
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }
        
        // Prüfen, ob gewähltes Anlieferdatum vor dem Zulassungsdatum liegt
        function checkAnlieferdatumOk(datum) {
            var dateParts = datum.split(".");
            var anlieferdat = new Date(dateParts[2], (dateParts[1] - 1), dateParts[0]);
            
            var itemsResult = getItemsOfTable("<%=rgResult.ClientID %>");
            if (itemsResult != null) {
                for (var i = 0; i < itemsResult.length; i++) {                
                    dateParts = itemsResult[i].get_cell("ZULDAT").innerHTML.split(".");
                    var zuldat = new Date(dateParts[2], (dateParts[1] - 1), dateParts[0]);

                    if (anlieferdat < zuldat) {
                        return false;
                    }
                }
            }
            return true;
        }

        /* Liefert die GridDataItems einer Tabelle anhand ihres Namens */
        function getItemsOfTable(tablename) {
            try {
                var tbl = $find(tablename);
                var mtv = tbl.get_masterTableView();
                var items = mtv.get_dataItems();

                return items;
            } catch (ex) {
                return null;
            }
        }

        /* Liefert die selektierten GridDataItems einer Tabelle anhand ihres Namens */
        function getSelectedItemsOfTable(tablename) {
            try {
                var tbl = $find(tablename);
                var mtv = tbl.get_masterTableView();
                var items = mtv.get_selectedItems();

                return items;
            } catch (ex) {
                return null;
            }
        } 
    </script>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
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
                            <asp:Label ID="lblPageTitle" runat="server"> (Stornieren/ Ändern)</asp:Label>
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
                                        <asp:LinkButton ID="cmdCreate" runat="server" CssClass="StandardButton"> &#149;&nbsp;Weiter</asp:LinkButton>
                                        <asp:LinkButton ID="cmdSend" runat="server" CssClass="StandardButton" Visible="false"> &#149;&nbsp;Korrigieren</asp:LinkButton>                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdStorno" runat="server" CssClass="StandardButton" Visible="false"> &#149;&nbsp;Stornieren</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdBack" runat="server" CssClass="StandardButton"> &#149;&nbsp;Zurück</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
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
                                        <td>
                                            <table id="tblTargetAdress" cellspacing="0" cellpadding="0" width="100%" border="0"
                                                class="SearchTable" style="margin-bottom: 10px;">
                                                <thead>
                                                    <tr>
                                                        <th class="TableHead" valign="top" align="left" colspan="6">
                                                            Zieldaten
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <td valign="top">
                                                            Augewählte Fahrzeuge an Station
                                                        </td>
                                                        <td valign="top">
                                                            Stationscode<br />
                                                            <telerik:RadTextBox ID="rtbStationscode" runat="server" MaxLength="12" Width="80px" AutoPostBack="true"></telerik:RadTextBox>
                                                        </td>
                                                        <td valign="top">
                                                            Termin<br />
                                                            <!-- EventValidation=false ,da die Autokorrektur bei unvollständige Datumswerten einen Fehler auslöst -->
                                                            <telerik:RadDatePicker ID="rdpTermin" runat="server" DateInput-ClientEvents-OnValueChanging="DateChanged" AutoPostBack="true" >
                                                                <Calendar runat="server" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x" ></Calendar>
                                                                <DateInput runat="server" DisplayDateFormat="dd.MM.yyyy" DateFormat="dd.MM.yyyy" LabelWidth="" AutoPostBack="True" >
                                                                    <ClientEvents OnValueChanging="DateChanged"></ClientEvents>
                                                                </DateInput>
                                                            </telerik:RadDatePicker>
                                                        </td>
                                                        <td valign="top">
                                                            Uhrzeit<br />
                                                            <telerik:RadTimePicker ID="rtpUhrzeit" runat="server" 
                                                                SelectedDate="10/18/2013 16:00:00">
                                                            </telerik:RadTimePicker>
                                                        </td>
                                                        <td valign="top">
                                                            Spediteur<br />
                                                            <telerik:RadComboBox ID="rcbSpediteur" runat="server"></telerik:RadComboBox>
                                                        </td>
                                                        <td rowspan="4" valign="top" width="100%">
                                                            Neuwagenauslieferung<br />
                                                            <telerik:RadGrid ID="rgAuslastung" runat="server" PageSize="10" AllowPaging="true"
                                                                AllowSorting="true" AutoGenerateColumns="true" GridLines="None" CellSpacing="0"
                                                                Visible="true" AllowColumnsReorder="true">
                                                                <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                                <ClientSettings AllowKeyboardNavigation="True">
                                                                    <Scrolling AllowScroll="false" UseStaticHeaders="True" FrozenColumnsCount="0" />
                                                                    <Resizing AllowColumnResize="true" AllowResizeToFit="True" ResizeGridOnColumnResize="false" />
                                                                </ClientSettings>
                                                                <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Fixed" GroupsDefaultExpanded="true">
                                                                    <SortExpressions>
                                                                        <telerik:GridSortExpression FieldName="STATION" SortOrder="Ascending" />
                                                                    </SortExpressions>
                                                                    <HeaderStyle ForeColor="#000000" />
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top">
                                                            Stationsname
                                                        </td>
                                                        <td></td>
                                                        <td colspan="3" valign="top" valign="top">
                                                            Name1<br />
                                                            <telerik:RadTextBox ID="rtbStation" runat="server" MaxLength="35" Width="240px">
                                                            </telerik:RadTextBox><br />
                                                            Name2<br />
                                                            <telerik:RadTextBox ID="rtbStation2" runat="server" MaxLength="35" Width="240px">
                                                            </telerik:RadTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top">
                                                            Straße/ Nr.
                                                        </td>
                                                        <td></td>
                                                        <td colspan="3" valign="top" style="white-space:nowrap;">
                                                            <telerik:RadTextBox ID="rtbStraße" runat="server" MaxLength="35" Width="172px">
                                                            </telerik:RadTextBox>
                                                            <telerik:RadTextBox ID="rtbHausnummer" runat="server" MaxLength="10" Width="60px">
                                                            </telerik:RadTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top">
                                                            PLZ/ Ort
                                                        <td></td>
                                                        <td colspan="3" valign="top" style="white-space:nowrap;">
                                                            <telerik:RadTextBox ID="rtbPlz" runat="server" MaxLength="5" Width="60px">
                                                            </telerik:RadTextBox>
                                                            <telerik:RadTextBox ID="rtbOrt" runat="server" MaxLength="40" Width="172px">
                                                            </telerik:RadTextBox>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label><br/>
                                            <asp:Label ID="lblSuccess" runat="server" CssClass="TextSuccess" EnableViewState="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table id="tblResult" cellspacing="0" cellpadding="0" width="100%" height="100%"
                                                border="0" class="ResultTable">
                                                <thead>
                                                    <tr>
                                                        <th class="TaskTitle" valign="top" width="100%" align="left">
                                                            &nbsp;
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <td width="100%" height="100%">
                                                            <telerik:RadGrid ID="rgResult" runat="server" PageSize="200" AllowPaging="true" AllowSorting="true"
                                                                AutoGenerateColumns="False" GridLines="None" CellSpacing="0" AllowMultiRowSelection="true" on>
                                                                <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                                <ClientSettings AllowKeyboardNavigation="True">
                                                                    <Scrolling AllowScroll="false" UseStaticHeaders="True" FrozenColumnsCount="0" />
                                                                    <Resizing AllowColumnResize="true" AllowResizeToFit="True" ResizeGridOnColumnResize="false" />
                                                                    <Selecting AllowRowSelect="true" />
                                                                    <ClientEvents OnRowSelected="RowSelected" OnRowDeselected="RowDeselected" />
                                                                </ClientSettings>
                                                                <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Fixed" GroupsDefaultExpanded="true">
                                                                    <SortExpressions>
                                                                        <telerik:GridSortExpression FieldName="BEAUFDAT" SortOrder="Ascending" />
                                                                    </SortExpressions>
                                                                    <HeaderStyle ForeColor="#000000" />
                                                                    <Columns>
                                                                        <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderText="Beauftragung">
                                                                        </telerik:GridClientSelectColumn>
                                                                        <telerik:GridBoundColumn SortExpression="RowID" DataField="RowID" UniqueName="RowID"
                                                                            HeaderText="RowID" Visible="false" />
                                                                        <telerik:GridBoundColumn SortExpression="Status" DataField="StatusT" HeaderText="Status" /> 
                                                                        <telerik:GridBoundColumn SortExpression="CHASSIS_NUM" DataField="CHASSIS_NUM" HeaderText="Fahrgestellnummer"
                                                                            ItemStyle-Width="150px" HeaderStyle-Width="150px" />
                                                                        <telerik:GridBoundColumn SortExpression="MVA_NUMMER" DataField="MVA_NUMMER" HeaderText="MVA" />
                                                                        <telerik:GridBoundColumn SortExpression="KENNZEICHEN" DataField="KENNZEICHEN" HeaderText="Kennzeichen" />
                                                                        <telerik:GridBoundColumn SortExpression="MODELL" DataField="MODELL" HeaderText="Modell" />
                                                                        <telerik:GridBoundColumn SortExpression="VERMIET_GRP" DataField="VERMIET_GRP" HeaderText="VGrp"
                                                                            FilterControlToolTip="Vermietgruppe" />
                                                                        <telerik:GridBoundColumn SortExpression="SPERRVERMERK" DataField="SPERRVERMERK" HeaderText="Sperrvermerk" />
                                                                        <telerik:GridBoundColumn SortExpression="VERWENDUNGSZWECK" DataField="VERWENDUNGSZWECK"
                                                                            HeaderText="Verwendungs- zweck" />
                                                                        <telerik:GridBoundColumn SortExpression="OWNER_CODE" DataField="OWNER_CODE" HeaderText="Owner-Code" />
                                                                        <telerik:GridBoundColumn SortExpression="EX_KUNNR" DataField="EX_KUNNR" HeaderText="Station" UniqueName="EX_KUNNR" />
                                                                        <telerik:GridBoundColumn SortExpression="CARPORT" DataField="CARPORT" HeaderText="Spedition" />
                                                                        <telerik:GridBoundColumn SortExpression="BEAUFDAT" DataField="BEAUFDAT" HeaderText="Termin"
                                                                            DataFormatString="{0:dd.MM.yyyy}" />
                                                                        <telerik:GridBoundColumn SortExpression="UHRZEIT" DataField="UHRZEIT" HeaderText="Uhrzeit"
                                                                            DataFormatString="{0:t}" DataType="System.String" />
                                                                        <telerik:GridBoundColumn SortExpression="ZULDAT" DataField="ZULDAT" HeaderText="Zulassung"
                                                                            DataFormatString="{0:dd.MM.yyyy}" />
                                                                        <telerik:GridBoundColumn SortExpression="FARBE_DE" DataField="FARBE_DE" HeaderText="Farbe" />
                                                                        <telerik:GridBoundColumn SortExpression="NAVIGATION" DataField="NAVIGATION" HeaderText="Navigation" />
                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
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