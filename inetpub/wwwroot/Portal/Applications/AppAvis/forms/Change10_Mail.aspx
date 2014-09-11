<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change10_Mail.aspx.vb" Inherits="AppAvis.Change10_Mail" %>

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
        .TableHead{
            padding:5px;
            text-align:left;
            color: #FFFFFF;
            background-color: #CC0033;
        }
    </style>   
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
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
                                        <asp:LinkButton ID="cmdCreate" runat="server" CssClass="StandardButton" Visible="True"> &#149;&nbsp;Versenden</asp:LinkButton>
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
                                        <th class="TaskTitle" valign="top" align="left">
                                            &nbsp;
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                     <tr>
                                        <td style="padding-left: 10px;" valign="top" align="left">
                                            <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label><br />
                                            <asp:Label ID="lblSuccess" runat="server" CssClass="TextSuccess" EnableViewState="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="tdOverview" runat="server">
                                        <td style="padding-left: 10px;" valign="top" align="left">
                                            <table>
                                                <tr>
                                                    <td width="100%" height="100%" valign="top">
                                                        <telerik:RadGrid ID="rgMail" runat="server" AllowPaging="false" AllowSorting="true" AllowMultiRowSelection="True"
                                                            AutoGenerateColumns="False" GridLines="None" CellSpacing="0">
                                                            <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                            <ClientSettings AllowKeyboardNavigation="True">
                                                                <Scrolling AllowScroll="False" UseStaticHeaders="true" SaveScrollPosition="True" />
                                                                <Resizing AllowColumnResize="true" AllowResizeToFit="True" ResizeGridOnColumnResize="true" />
                                                                <Selecting AllowRowSelect="True" />
                                                            </ClientSettings>
                                                            <MasterTableView GroupLoadMode="Client" TableLayout="Fixed" GroupsDefaultExpanded="true"
                                                                Width="100%">
                                                                <HeaderStyle ForeColor="#000000" />
                                                                <Columns>
                                                                    <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderText="Versenden">
                                                                    </telerik:GridClientSelectColumn>
                                                                    <telerik:GridBoundColumn SortExpression="AUF_NEUW_TRANSP" DataField="AUF_NEUW_TRANSP"
                                                                        HeaderText="Auftragsnummer" />
                                                                    <telerik:GridBoundColumn SortExpression="PDISTANDORT" DataField="PDISTANDORT" HeaderText="Carport" />
                                                                    <telerik:GridBoundColumn SortExpression="SPEDITEUR" DataField="SPEDITEUR" HeaderText="Spediteur" />
                                                                    <telerik:GridBoundColumn SortExpression="ANLDAT" DataField="ANLDAT" HeaderText="Datum Disposition"
                                                                        DataFormatString="{0:dd.MM.yyyy}" />
                                                                    <telerik:GridBoundColumn SortExpression="DATMAIL" DataField="DATMAIL" HeaderText="Datum Mail"
                                                                        DataFormatString="{0:dd.MM.yyyy}" />
                                                                    <telerik:GridBoundColumn SortExpression="WEBUSER" DataField="WEBUSER" HeaderText="Disponent" />
                                                                    <telerik:GridBoundColumn SortExpression="EX_KUNNR" DataField="EX_KUNNR" HeaderText="Station" />
                                                                    <telerik:GridBoundColumn SortExpression="BEAUFDAT" DataField="BEAUFDAT" HeaderText="Anlieferdatum"
                                                                        DataFormatString="{0:dd.MM.yyyy}" />
                                                                    <telerik:GridBoundColumn SortExpression="Summe" DataField="Summe" HeaderText="Anzahl Fahrzeuge" />
                                                                    <telerik:GridTemplateColumn HeaderText="Details">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="btnDetails" CommandName="Details" runat="server" Text="Details"
                                                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem,"AUF_NEUW_TRANSP") %>'
                                                                                CssClass="StandardButton"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <%--<telerik:GridTemplateColumn HeaderText="Versenden">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="btnVersenden" CommandName="Versenden" runat="server" Text="Versenden"
                                                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem,"AUF_NEUW_TRANSP") %>'
                                                                                CssClass="StandardButton"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>--%>
                                                                </Columns>
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="tdDetailsGrid" runat="server" Visible="False">
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
                                                            <telerik:RadGrid ID="rgDetails" runat="server" PageSize="200" AllowPaging="true" AllowSorting="true"
                                                                AutoGenerateColumns="False" GridLines="None" CellSpacing="0" AllowMultiRowSelection="False">
                                                                <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                                <ClientSettings AllowKeyboardNavigation="True">
                                                                    <Scrolling AllowScroll="false" UseStaticHeaders="True" FrozenColumnsCount="0" />
                                                                    <Resizing AllowColumnResize="true" AllowResizeToFit="True" ResizeGridOnColumnResize="false" />
                                                                    <Selecting AllowRowSelect="False" />
                                                                </ClientSettings>
                                                                <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Fixed" GroupsDefaultExpanded="true">
                                                                    <SortExpressions>
                                                                        <telerik:GridSortExpression FieldName="BEAUFDAT" SortOrder="Ascending" />
                                                                    </SortExpressions>
                                                                    <HeaderStyle ForeColor="#000000" />
                                                                    <Columns>
                                                                        <telerik:GridBoundColumn SortExpression="RowID" DataField="RowID" UniqueName="RowID"
                                                                            HeaderText="RowID" Visible="false" />
                                                                        <telerik:GridBoundColumn SortExpression="AUF_NEUW_TRANSP" DataField="AUF_NEUW_TRANSP" 
                                                                            UniqueName="AUF_NEUW_TRANSP" HeaderText="Auftragsnummer" />
                                                                        <telerik:GridBoundColumn SortExpression="PDISTANDORT" DataField="PDISTANDORT" HeaderText="Carport" />
                                                                        <telerik:GridBoundColumn SortExpression="ANLDAT" DataField="ANLDAT" HeaderText="Datum Disposition"
                                                                            DataFormatString="{0:dd.MM.yyyy}" />
                                                                        <telerik:GridBoundColumn SortExpression="WEBUSER" DataField="WEBUSER" HeaderText="Disponent" />
                                                                        <telerik:GridBoundColumn SortExpression="EX_KUNNR" DataField="EX_KUNNR" HeaderText="Station" />
                                                                        <telerik:GridBoundColumn SortExpression="BEAUFDAT" DataField="BEAUFDAT" HeaderText="Anlieferdatum"
                                                                            DataFormatString="{0:dd.MM.yyyy}" />
                                                                        <telerik:GridBoundColumn SortExpression="CHASSIS_NUM" DataField="CHASSIS_NUM" HeaderText="Fahrgestellnummer"
                                                                            ItemStyle-Width="150px" HeaderStyle-Width="150px" />
                                                                        <telerik:GridBoundColumn SortExpression="MVA_NUMMER" DataField="MVA_NUMMER" HeaderText="MVA" />
                                                                        <telerik:GridBoundColumn SortExpression="KENNZEICHEN" DataField="KENNZEICHEN" HeaderText="Kennzeichen" />
                                                                        <telerik:GridBoundColumn SortExpression="ZULDAT" DataField="ZULDAT" HeaderText="Zulassung"
                                                                            DataFormatString="{0:dd.MM.yyyy}" />
                                                                        <telerik:GridBoundColumn SortExpression="MODELL" DataField="MODELL" HeaderText="Modell" />
                                                                        <telerik:GridBoundColumn SortExpression="OWNER_CODE" DataField="OWNER_CODE" HeaderText="Owner-Code" />
                                                                        <telerik:GridBoundColumn SortExpression="VERWENDUNGSZWECK" DataField="VERWENDUNGSZWECK"
                                                                            HeaderText="Verwendungs- zweck" />
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
                        <td valign="top">
                            
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