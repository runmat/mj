<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change10_Korrektur.aspx.vb" Inherits="AppAvis.Change10_Korrektur" %>
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
                                        <asp:LinkButton ID="cmdCreate" runat="server" CssClass="StandardButton"> &#149;&nbsp;Weiter</asp:LinkButton>
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
                                        <th class="TaskTitle" valign="top" align="left" colspan="5">
                                            &nbsp;
                                        </th>
                                        <th class="TaskTitle" valign="top" width="100%">
                                            &nbsp;
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <%--<td valign="top">
                                            <table id="tblSearch" runat="server" cellspacing="0" cellpadding="0" class="SearchTable">
                                                <thead>
                                                    <tr>
                                                        <th colspan="5" class="TableHead">Zeitraum</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
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
                                                        <td style="padding-left: 10px;">
                                                            Bis:
                                                        </td>
                                                        <td>
                                                            <telerik:RadDatePicker ID="txtDatZulBis" runat="server">
                                                            </telerik:RadDatePicker>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="white-space: nowrap;">
                                                            Datum Freisetzung
                                                        </td>
                                                        <td>
                                                            Von:
                                                        </td>
                                                        <td>
                                                            <telerik:RadDatePicker ID="txtDatFreiVon" runat="server">
                                                            </telerik:RadDatePicker>
                                                        </td>
                                                        <td style="padding-left: 10px;">
                                                            Bis:
                                                        </td>
                                                        <td>
                                                            <telerik:RadDatePicker ID="txtDatFreiBis" runat="server">
                                                            </telerik:RadDatePicker>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>--%>
                                        <td style="padding-left: 10px;" valign="top">
                                            <asp:Panel runat="server" DefaultButton="cmdCreate">
                                                <table id="tblFilter" runat="server" cellspacing="0" cellpadding="0" class="SearchTable">
                                                    <thead>
                                                        <tr>
                                                            <th class="TableHead" valign="top" align="left" colspan="2">
                                                                Auftragsnummer
                                                            </th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <tr>
                                                            <td style="padding-right: 5px;">
                                                                Auftragsnummer
                                                            </td>
                                                            <td width="100%" style="padding-right: 5px;">
                                                                <telerik:RadTextBox ID="rtbAuftragsnummer" runat="server" MaxLength="10" Width="200px">
                                                                </telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </asp:Panel>
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