<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UeberfDADStart.aspx.vb"
    Inherits="AppUeberf.UeberfDADStart" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
    <style type="text/css">
        .style1
        {
            width: 133px;
        }
        .style3
        {
            width: 130px;
        }
        #Table2
        {
            width: 817px;
        }
        .style41
        {
            width: 175px;
        }
        .style42
        {
            width: 83px;
        }
        .Headline
        {
            vertical-align: middle;
            color: #009900;
            height: 12px;
        }
        .style46
        {
            width: 133px;
            height: 29px;
        }
        .style48
        {
            width: 175px;
            height: 12px;
        }
        .style49
        {
            height: 12px;
        }
        .style50
        {
            width: 175px;
            height: 11px;
        }
        .style54
        {
            font-size: xx-small;
        }
        .style55
        {
            color: #0033CC;
        }
        .LabelColumn {
            vertical-align: top;
            padding: 3px 5px 0px 3px;
            white-space: nowrap;
        }
        .InputColumn {
            vertical-align: top;
            padding: 3px 5px 0px 3px;
            white-space: nowrap;
        }
    </style>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table width="100%" align="center" cellspacing="0" cellpadding="0">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td valign="top" align="left" width="100%" colspan="3">
                <table id="Table10" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;(
                            <asp:Label ID="lblPageTitle" runat="server">Start</asp:Label>)
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 144px" valign="top" width="174">
                            <table id="Table12" cellspacing="0" cellpadding="0" border="0">
                                <tr>
                                    <td width="150">
                                        <asp:LinkButton ID="lbtWeiter" TabIndex="12" runat="server" CssClass="StandardButtonTable"
                                            Width="100px"> •&nbsp;Weiter</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" width="150">
                                        <asp:LinkButton ID="lbtBack" TabIndex="12" runat="server" CssClass="StandardButtonTable"
                                            Width="100px"> •&nbsp;Zurück</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 917px" valign="top">
                            <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td style="width: 437px" width="437">
                                        <table id="table14" cellpadding="0" cellspacing="0" class="style2">
                                            <tr>
                                                <td class="style42">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Literal ID="ltAnzeige" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td valign="top" align="middle" width="100%">
                            <table id="Table2" cellspacing="0" cellpadding="5" bgcolor="white" border="0" align="left">
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td class="Headline">
                                        <strong>Beauftragung:</strong>
                                    </td>
                                    <td class="Headline">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style41" valign="center" nowrap>
                                        &nbsp;
                                    </td>
                                    <td class="style1" valign="center" nowrap>
                                        <asp:Panel ID="pnlStart" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td class="LabelColumn">
                                                    </td>
                                                    <td class="InputColumn">
                                                        <asp:CheckBox ID="chkZul" runat="server" Text="Zulassung" AutoPostBack="True"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="LabelColumn">
                                                    </td>
                                                    <td class="InputColumn">
                                                        <asp:CheckBox ID="chkUeberf" runat="server" Text="Neufahrzeugauslieferung" AutoPostBack="True">
                                                        </asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="LabelColumn">
                                                        &nbsp;
                                                    </td>
                                                    <td class="InputColumn">
                                                        <asp:CheckBox ID="chkRueck" runat="server" Text="Rückführung/Anschlussfahrt" AutoPostBack="True">
                                                        </asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="LabelColumn">
                                                        &nbsp;
                                                    </td>
                                                    <td class="InputColumn">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="LabelColumn">
                                                        <asp:Label ID="lblReferenz" runat="server" Text="Referenz-Nr.*"></asp:Label>
                                                    </td>
                                                    <td class="InputColumn" >
                                                        <asp:TextBox ID="txtReferenzNr" runat="server" MaxLength="10" width="100%"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="LabelColumn">
                                                        <asp:Label ID="lblReferenzNrRuecktour" runat="server" Text="Referenz-Rückfahrt*"
                                                            Visible="False"></asp:Label>
                                                    </td>
                                                    <td class="InputColumn">
                                                        <asp:TextBox ID="txtReferenzNrRuecktour" runat="server" Visible="False" MaxLength="20" width="100%"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr id="tr_Buchungscode" runat="server">
                                                    <td class="LabelColumn">
                                                        <asp:Label ID="lblBuchungscode" runat="server" Text="Buchungscode*"></asp:Label>
                                                    </td>
                                                    <td class="InputColumn">
                                                        <asp:TextBox ID="txtBuchungscode" runat="server" MaxLength="12" width="100%"></asp:TextBox>
                                                    </td>
                                                    <td class="InputColumn" style="width: 147px; text-align: left; padding-left: 20px;" rowspan="2">
                                                        <asp:Panel ID="Panel1" runat="server" BackColor="Silver" BorderColor="Black" BorderStyle="Solid"
                                                            BorderWidth="1px" Style="padding-top: 5px; padding-bottom: 5px; padding-left: 10px;
                                                            padding-right: 10px; margin-bottom: 5px; margin-right: 20px" Width="160px">
                                                            <span class="style54"><b><span class="style55">Mögliche Buchungscodes:</span><br />
                                                            </b>5500 - keine Weiterbelastung<br />
                                                                6600 - volle Weiterbelastung<br />
                                                                8800 - Sonderfall</span></asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="LabelColumn" style="font-size: 12px;">
                                                       * = Pflichtfeld
                                                    </td>
                                                    <td class="InputColumn">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table id="table13" cellpadding="0" cellspacing="0" class="style2">
                                <tr>
                                    <td class="style3">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:Label ID="lblError" runat="server" Width="821px" EnableViewState="False" CssClass="TextError"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
