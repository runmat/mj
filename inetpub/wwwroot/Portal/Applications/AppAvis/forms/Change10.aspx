<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change10.aspx.vb" Inherits="AppAvis.Change10" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>    
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
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
                        <td class="TaskTitle" width="150">
                            &nbsp;
                        </td>
                        <td class="TaskTitle">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="150">
                            <asp:LinkButton ID="btnTransportbeauftragung" runat="server" class="StandardButton" Width="140px"
                                PostBackUrl="Change10_Beauftragung.aspx">Transport-Beauftragung</asp:LinkButton>
                        </td>
                        <td>
                            <label style="margin:5px;">Beauftragen Sie hier neue Transporte an Stationen.</label>                            
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="150">
                            <asp:LinkButton ID="btnTransportbeauftragungUpload" runat="server" class="StandardButton" Width="140px"
                                PostBackUrl="Change10_BeauftragungUpload.aspx">Transport-Beauftragung Upload</asp:LinkButton>
                        </td>
                        <td>
                            <label style="margin:5px;">Beauftragen Sie hier neue Transporte an Stationen per Datei-Upload.</label>                            
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="150">
                            <asp:LinkButton ID="btnKorrekturStorno" runat="server" class="StandardButton" Width="140px" PostBackUrl="Change10_Korrektur.aspx">Korrektur/ Stornierung</asp:LinkButton>
                        </td>
                        <td>
                            <label style="margin:5px;">Korrigieren Sie hier Ihre vorhanden Aufträge oder stornieren Sie diese.</label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="150">
                            <asp:LinkButton ID="btnReport" runat="server" class="StandardButton" Width="140px" PostBackUrl="Change10_Report.aspx">Reporting</asp:LinkButton>
                        </td>
                        <td>
                            <label style="margin:5px;">Verschaffen Sie sich einen Überblick über Ihre Aufträge.</label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="150">
                            <asp:LinkButton ID="btnMailBeauftragung" runat="server" class="StandardButton" Width="140px" PostBackUrl="Change10_Report.aspx">
                                Mail-Beauftragung
                            </asp:LinkButton>
                        </td>
                        <td>
                            <label style="margin:5px;">Schließen Sie Ihre Aufträge ab, in dem Sie disponierte Fahrzeuge per Mail beauftragen.</label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
                        </td>
                        <td valign="top">
                            <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False" style="margin:5px;"></asp:Label>
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