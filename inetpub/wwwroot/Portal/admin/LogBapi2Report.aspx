<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LogBapi2Report.aspx.vb"
    Inherits="CKG.Admin.LogBapi2Report" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema" />
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body>
    <form id="form1" runat="server">
    
    <div>
 <asp:ScriptManager ID="Scriptmanager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true"></asp:ScriptManager>
        <table cellspacing="0" cellpadding="0" width="100%" align="center">
            <tbody>
                <tr>
                    <td>
                        <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tbody>
                                <tr>
                                    <td class="PageNavigation" colspan="2" height="25">
                                        <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Ergebnisanzeige)</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" width="120">
                                        <table id="Table2" cellspacing="0" cellpadding="0" width="120"
                                            border="0">
                                            <tr>
                                                <td class="TaskTitle">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" width="150">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" width="150">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" width="150">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" width="150">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top">
                                        <table cellspacing="0" cellpadding="0" width="100%" align="left" border="0">
                                            <tbody>
                                                <tr>
                                                    <td class="TaskTitle" colspan="2">
                                                        &nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                                <tr id="trSearch" runat="server">
                                                    <td align="left" colspan="2">
                                                        <table bgcolor="white" border="0">
                                                            <tr>
                                                                <td valign="bottom" width="100">
                                                                    &nbsp;&nbsp;
                                                                </td>
                                                                <td valign="bottom">
                                                                    &nbsp;
                                                                </td>
                                                                <td valign="bottom">
                                                                    &nbsp;
                                                                </td>
                                                                <td valign="bottom" width="100%">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="bottom" width="125" nowrap="nowrap">
                                                                    Kunde:
                                                                </td>
                                                                <td valign="bottom">
                                                                    <asp:Label ID="lblCustomer" runat="server" CssClass="InfoBoxFlat" Width="160px" Visible="False"></asp:Label>
                                                                    <asp:DropDownList ID="ddlFilterCustomer" runat="server" Width="192px" Height="20px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td valign="bottom">
                                                                </td>
                                                                <td valign="bottom">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="bottom" width="125" nowrap="nowrap">
                                                                    BAPI:
                                                                </td>
                                                                <td valign="bottom">
                                                                    <asp:Label ID="lblBapiName" runat="server" CssClass="InfoBoxFlat" Visible="False"
                                                                        Width="160px"></asp:Label>
                                                                    <asp:TextBox ID="txtBapiName" runat="server" Height="16px" Width="186px" BorderColor="Silver"
                                                                        BorderStyle="Solid" BorderWidth="1px">*</asp:TextBox>
                                                                </td>
                                                                <td valign="bottom">
                                                                    &nbsp;
                                                                </td>
                                                                <td valign="bottom">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="bottom" width="125" nowrap="nowrap">
                                                                    Freundlicher Name:
                                                                </td>
                                                                <td valign="bottom">
                                                                    <asp:Label ID="lblFriendName" runat="server" CssClass="InfoBoxFlat" Visible="False"
                                                                        Width="160px"></asp:Label>
                                                                    <asp:TextBox ID="txtFriendName" runat="server" Height="16px" Width="186px" BorderColor="Silver"
                                                                        BorderStyle="Solid" BorderWidth="1px">*</asp:TextBox>
                                                                </td>
                                                                <td valign="bottom">
                                                                    &nbsp;
                                                                </td>
                                                                <td valign="bottom">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="bottom" width="125" nowrap="nowrap">
                                                                    Technischer Name:
                                                                </td>
                                                                <td valign="bottom">
                                                                    <asp:Label ID="lblTechName" runat="server" CssClass="InfoBoxFlat" Visible="False"
                                                                        Width="160px"></asp:Label>
                                                                    <asp:TextBox ID="txtTechName" runat="server" Width="186px" Height="16px" BorderColor="Silver"
                                                                        BorderStyle="Solid" BorderWidth="1px">*</asp:TextBox>
                                                                </td>
                                                                <td valign="bottom">
                                                                    &nbsp;
                                                                </td>
                                                                <td valign="bottom" width="100%">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="bottom" width="125" nowrap="nowrap">
                                                                    im Zeitraum von:
                                                                </td>
                                                                <td valign="bottom" nowrap="nowrap">
                                                                    <asp:TextBox BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" runat="server"
                                                                        Width="75px" ToolTip="Datum von" ID="txtDateVon" MaxLength="10" autocomplete="off"
                                                                        Height="16px" />
                                                                    <ajaxToolkit:CalendarExtender ID="txtDateVon_CalendarExtender" runat="server" 
                                                                        Enabled="True" TargetControlID="txtDateVon">
                                                                    </ajaxToolkit:CalendarExtender>
                                                                    &nbsp;bis:&nbsp;
                                                                    <asp:TextBox BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" runat="server"
                                                                        Width="75px" ToolTip="Datum bis" ID="txtDateBis" MaxLength="10" autocomplete="off"
                                                                        Height="16px" />
                                                                    <ajaxToolkit:CalendarExtender ID="txtDateBis_CalendarExtender" runat="server" 
                                                                        Enabled="True" TargetControlID="txtDateBis">
                                                                    </ajaxToolkit:CalendarExtender>
                                                                 </td>
                                                                <td valign="bottom">
                                                                    &nbsp;
                                                                </td>
                                                                <td valign="bottom" width="100%">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="bottom" width="125" nowrap="nowrap">
                                                                    &nbsp;
                                                                </td>
                                                                <td valign="bottom" align="right">
                                                                    <asp:LinkButton ID="btnSuche" runat="server" CssClass="StandardButton" Height="16px"
                                                                        Width="100px">Suche</asp:LinkButton>
                                                                </td>
                                                                <td valign="bottom">
                                                                    &nbsp;
                                                                </td>
                                                                <td valign="bottom" width="100%">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr id="trSearchSpacer" runat="server">
                                                    <td align="left" height="25" colspan="2">
                                                        &nbsp
                                                    </td>
                                                </tr>
                                                <tr id="trPaging" runat="server">
                                                    <td align="left" nowrap="nowrap">
                                                        <asp:Label ID="lblAnzahl" runat="server" Text="lblAnzahl" Font-Bold="True"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <strong>&nbsp;<img alt="" src="../images/excel.gif" style="width: 16px; height: 16px" />&nbsp;
                                                            <asp:LinkButton CssClass="ExcelButton" ID="lnkCreateExcel" runat="server">Excelformat</asp:LinkButton>&nbsp;
                                                            Anzahl Vorgänge / Seite </strong>
                                                        <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr id="trSearchResult" runat="server">
                                                    <td align="left" colspan="2">
                                                        <asp:GridView ID="gvResult" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                            CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0" AllowPaging="True" GridLines="Both"
                                                            PageSize="20" Width="100%">
                                                            <HeaderStyle BackColor="#666666" ForeColor="White" Height="22px" />
                                                            <PagerStyle BackColor="#666666" ForeColor="White" />
                                                            <Columns>
                                                                <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />
                                                                <asp:BoundField DataField="Kunnr" SortExpression="Kunnr" HeaderText="Kundennr." />
                                                                <asp:BoundField DataField="Kundenbezeichnung" SortExpression="Kundenbezeichnung"
                                                                    HeaderText="Kunde" />
                                                                <asp:BoundField DataField="AppID" SortExpression="AppID" HeaderText="AppID" />
                                                                <asp:BoundField DataField="Freundlicher_Name" SortExpression="Freundlicher_Name"
                                                                    HeaderText="Freundlicher Name" />
                                                                <asp:BoundField DataField="Technischer_Name" SortExpression="Technischer_Name" HeaderText="Technischer Name" />
                                                                <asp:BoundField DataField="Pfad" HeaderText="Pfad" SortExpression="Pfad" Visible="false" />
                                                                <asp:BoundField DataField="BAPI" HeaderText="BAPI" SortExpression="BAPI" />
                                                                <asp:BoundField DataField="LastUse" HeaderText="zuletzt verwendet" SortExpression="LastUse" />
                                                            </Columns>
                                                            <AlternatingRowStyle BackColor="#DEE1E0"></AlternatingRowStyle>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" height="25" colspan="2">
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
                    <td>
                        <asp:Label ID="lblMessage" runat="server" CssClass="TextExtraLarge" EnableViewState="False"></asp:Label><asp:Label
                            ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <!--#include File="../PageElements/Footer.html" -->
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
