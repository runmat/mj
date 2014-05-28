<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report201_3.aspx.vb" Inherits="AppDCB.Report201_3" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="GridLayout">
    <table height="735" cellspacing="0" cellpadding="0" width="965" border="0" ms_2d_layout="TRUE">
        <tr valign="top">
            <td width="965" height="735">
                <form id="Form1" method="post" runat="server">
                <table height="963" cellspacing="0" cellpadding="0" width="733" border="0" ms_2d_layout="TRUE">
                    <tr>
                        <td width="1" height="0">
                        </td>
                        <td width="1" height="0">
                        </td>
                        <td width="1" height="0">
                        </td>
                        <td width="1" height="0">
                        </td>
                        <td width="1" height="0">
                        </td>
                        <td width="1" height="0">
                        </td>
                        <td width="4" height="0">
                        </td>
                        <td width="4" height="0">
                        </td>
                        <td width="26" height="0">
                        </td>
                        <td width="40" height="0">
                        </td>
                        <td width="77" height="0">
                        </td>
                        <td width="1" height="0">
                        </td>
                        <td width="1" height="0">
                        </td>
                        <td width="1" height="0">
                        </td>
                        <td width="1" height="0">
                        </td>
                        <td width="1" height="0">
                        </td>
                        <td width="1" height="0">
                        </td>
                        <td width="218" height="0">
                        </td>
                        <td width="8" height="0">
                        </td>
                        <td width="26" height="0">
                        </td>
                        <td width="1" height="0">
                        </td>
                        <td width="1" height="0">
                        </td>
                        <td width="5" height="0">
                        </td>
                        <td width="111" height="0">
                        </td>
                        <td width="200" height="0">
                        </td>
                    </tr>
                    <tr valign="top">
                        <td height="28">
                        </td>
                        <td colspan="10">
                            <asp:Label ID="Label17" runat="server" Font-Size="Smaller" Font-Names="Arial">Fahrzeug- und Aufbauart</asp:Label>
                        </td>
                        <td colspan="10">
                        </td>
                        <td colspan="3">
                            <asp:Label ID="Label1" runat="server" Font-Size="Smaller" Font-Names="Arial">Den</asp:Label>
                        </td>
                        <td rowspan="3">
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="20" height="24">
                        </td>
                        <td colspan="4">
                            <asp:Label ID="Label2" runat="server" Font-Size="Smaller" Font-Names="Arial">Telefon</asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="21" height="28">
                        </td>
                        <td colspan="3">
                            <asp:Label ID="Label3" runat="server" Font-Size="Smaller" Font-Names="Arial">BN</asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="21" height="24">
                        </td>
                        <td colspan="4">
                            <asp:Label ID="Label4" runat="server" Font-Size="Smaller" Font-Names="Arial">Uhrzeit: __________________________</asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="18" height="24">
                            <asp:Label ID="Label5" runat="server" Font-Size="XX-Small" Font-Names="Arial">(Bei Antwortschreiben bitte auch das amtl. Kennzeichen angeben)</asp:Label>
                        </td>
                        <td colspan="7" rowspan="2">
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="9" height="4">
                        </td>
                        <td colspan="9" rowspan="2">
                            <asp:Label ID="Label45" runat="server" Font-Names="Arial" Font-Size="Small">Stadt Flensburg<br>FB 1/Kfz-Zulassung und F�hrerscheinstelle<br>Rudolf-Diesel-Str. 3a<br>24941 Flensburg</asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="9" height="96">
                        </td>
                        <td colspan="4">
                        </td>
                        <td colspan="3" rowspan="6">
                            <asp:Label ID="lblError" runat="server" Font-Size="XX-Large" Font-Names="Arial" ForeColor="Red"
                                Visible="False" Width="223px" Font-Bold="True">Fehler beim Seitenaufbau.</asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="2" height="2">
                        </td>
                        <td colspan="8" rowspan="2">
                            <asp:Label ID="Label6" runat="server" Font-Size="Small" Font-Names="Arial" Font-Bold="True">Anzeige</asp:Label>
                        </td>
                        <td colspan="12">
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="2" height="29">
                        </td>
                        <td colspan="8">
                            <asp:Label ID="Label7" runat="server" Font-Size="Smaller" Font-Names="Arial">(Verdacht einer Straftat liegt nicht vor)</asp:Label>
                        </td>
                        <td colspan="4">
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="3" height="15">
                        </td>
                        <td colspan="6">
                            <asp:Label ID="Label10" runat="server" Font-Size="XX-Small" Font-Names="Arial">Betreff</asp:Label>
                        </td>
                        <td colspan="13">
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="3" height="23">
                        </td>
                        <td colspan="15">
                            <asp:Label ID="Label9" runat="server" Font-Size="Smaller" Font-Names="Arial">Verlust von Fahrzeug-Kennzeichen</asp:Label>
                        </td>
                        <td colspan="4">
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="4" height="16">
                        </td>
                        <td colspan="5">
                            <asp:Label ID="Label12" runat="server" Font-Size="XX-Small" Font-Names="Arial">Bezug</asp:Label>
                        </td>
                        <td colspan="13">
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="5" height="39">
                        </td>
                        <td colspan="6">
                            <asp:Label ID="Label11" runat="server" Font-Size="Smaller" Font-Names="Arial">PDV 350 - 21.3050.2</asp:Label>
                        </td>
                        <td colspan="14">
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="24" height="1">
                        </td>
                        <td rowspan="3">
                            <asp:CheckBox ID="CheckBox2" runat="server" Text="   hinteres" Enabled="False" Checked="True">
                            </asp:CheckBox>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="17" height="4">
                        </td>
                        <td colspan="6" rowspan="2">
                            <asp:Label ID="lblFahrzeugkennzeichen" runat="server" Font-Bold="True">________________________________</asp:Label>
                        </td>
                        <td rowspan="2">
                            <asp:CheckBox ID="CheckBox1" runat="server" Text="   vorderes" Enabled="False" Checked="True">
                            </asp:CheckBox>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="4" height="20">
                        </td>
                        <td colspan="7">
                            <asp:Label ID="Label13" runat="server" Font-Size="Smaller" Font-Names="Arial">Fahrzeugkennzeichen</asp:Label>
                        </td>
                        <td colspan="6">
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="16" height="6">
                        </td>
                        <td colspan="7" rowspan="2">
                            <asp:Label ID="Label15" runat="server" Font-Bold="True">________________________________</asp:Label>
                        </td>
                        <td colspan="2" rowspan="2">
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="3" height="30">
                        </td>
                        <td colspan="7">
                            <asp:Label ID="Label14" runat="server" Font-Size="Smaller" Font-Names="Arial">Verlustort</asp:Label>
                        </td>
                        <td colspan="6">
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="4" height="25">
                        </td>
                        <td colspan="7">
                            <asp:Label ID="Label16" runat="server" Font-Size="Smaller" Font-Names="Arial" Font-Bold="True">Fahrzeugdaten</asp:Label>
                        </td>
                        <td colspan="14">
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="16" height="1">
                        </td>
                        <td colspan="9" rowspan="2">
                            <asp:Label ID="lblFahrzeugUndAufbauart" runat="server" Font-Bold="True">___________________________________________________________________</asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="4" height="22">
                        </td>
                        <td colspan="7">
                            <asp:Label ID="Label19" runat="server" Font-Size="Smaller" Font-Names="Arial">Fahrzeug- und Aufbauart</asp:Label>
                        </td>
                        <td colspan="5">
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="15" height="3">
                        </td>
                        <td colspan="10" rowspan="2">
                            <asp:Label ID="lblHersteller" runat="server" Font-Bold="True">___________________________________________________________________</asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="4" height="20">
                        </td>
                        <td colspan="6">
                            <asp:Label ID="Label23" runat="server" Font-Size="Smaller" Font-Names="Arial">Hersteller</asp:Label>
                        </td>
                        <td colspan="5">
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="15" height="3">
                        </td>
                        <td colspan="10" rowspan="2">
                            <asp:Label ID="lblTypUndAusfuehrung" runat="server" Font-Bold="True">___________________________________________________________________</asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="4" height="21">
                        </td>
                        <td colspan="7">
                            <asp:Label ID="Label25" runat="server" Font-Size="Smaller" Font-Names="Arial">Typ und Ausf�hrung</asp:Label>
                        </td>
                        <td colspan="4">
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="14" height="3">
                        </td>
                        <td colspan="11" rowspan="2">
                            <asp:Label ID="lblFIN" runat="server" Font-Bold="True">___________________________________________________________________</asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="4" height="30">
                        </td>
                        <td colspan="7">
                            <asp:Label ID="Label24" runat="server" Font-Size="Smaller" Font-Names="Arial">Fahrzeug-Ident.-Nr.</asp:Label>
                        </td>
                        <td colspan="3">
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="6" height="23">
                        </td>
                        <td colspan="5">
                            <asp:Label ID="Label20" runat="server" Font-Size="Smaller" Font-Names="Arial" Font-Bold="True">Anzeigender</asp:Label>
                        </td>
                        <td colspan="14">
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="13" height="2">
                        </td>
                        <td colspan="12" rowspan="2">
                            <asp:Label ID="Label29" runat="server" Font-Bold="True">___________________________________________________________________</asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="4" height="19">
                        </td>
                        <td colspan="7">
                            <asp:Label ID="Label21" runat="server" Font-Size="Smaller" Font-Names="Arial">Name / Geburtsname</asp:Label>
                        </td>
                        <td colspan="2">
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="13" height="5">
                        </td>
                        <td colspan="12" rowspan="2">
                            <asp:Label ID="Label30" runat="server" Font-Bold="True">___________________________________________________________________</asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="6" height="19">
                        </td>
                        <td colspan="4">
                            <asp:Label ID="Label22" runat="server" Font-Size="Smaller" Font-Names="Arial">Vornamen</asp:Label>
                        </td>
                        <td colspan="3">
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="12" height="5">
                        </td>
                        <td colspan="13" rowspan="2">
                            <asp:Label ID="Label32" runat="server" Font-Bold="True">___________________________________________________________________</asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="5" height="20">
                        </td>
                        <td colspan="5">
                            <asp:Label ID="Label26" runat="server" Font-Size="Smaller" Font-Names="Arial">geb. am / in</asp:Label>
                        </td>
                        <td colspan="2">
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="13" height="4">
                        </td>
                        <td colspan="12" rowspan="2">
                            <asp:Label ID="Label33" runat="server" Font-Bold="True">___________________________________________________________________</asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="6" height="22">
                        </td>
                        <td colspan="4">
                            <asp:Label ID="Label27" runat="server" Font-Size="Smaller" Font-Names="Arial">wohnhaft</asp:Label>
                        </td>
                        <td colspan="3">
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="12" height="4">
                        </td>
                        <td colspan="13" rowspan="2">
                            <asp:Label ID="Label31" runat="server" Font-Bold="True">___________________________________________________________________</asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="5" height="31">
                        </td>
                        <td colspan="6">
                            <asp:Label ID="Label28" runat="server" Font-Size="Smaller" Font-Names="Arial">Telefon privat / dienstlich</asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="5" height="20">
                        </td>
                        <td colspan="6">
                            <asp:Label ID="Label34" runat="server" Font-Size="Smaller" Font-Names="Arial" Font-Bold="True">Fahrzeughalter</asp:Label>
                        </td>
                        <td colspan="14">
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="14" height="3">
                        </td>
                        <td colspan="11" rowspan="2">
                            <asp:Label ID="lblName" runat="server" Font-Bold="True">___________________________________________________________________</asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="5" height="22">
                        </td>
                        <td colspan="6">
                            <asp:Label ID="Label35" runat="server" Font-Size="Smaller" Font-Names="Arial">Name / Geburtsname</asp:Label>
                        </td>
                        <td colspan="3">
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="12" height="4">
                        </td>
                        <td colspan="13" rowspan="2">
                            <asp:Label ID="Label37" runat="server" Font-Bold="True">___________________________________________________________________</asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="6" height="21">
                        </td>
                        <td colspan="4">
                            <asp:Label ID="Label36" runat="server" Font-Size="Smaller" Font-Names="Arial">geb. am / in</asp:Label>
                        </td>
                        <td colspan="2">
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="11" height="2">
                        </td>
                        <td colspan="14" rowspan="2">
                            <asp:Label ID="lblWohnhaft" runat="server" Font-Bold="True">___________________________________________________________________</asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="6" height="22">
                        </td>
                        <td colspan="4">
                            <asp:Label ID="Label38" runat="server" Font-Size="Smaller" Font-Names="Arial">wohnhaft</asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="13" height="3">
                        </td>
                        <td colspan="12" rowspan="2">
                            <asp:Label ID="Label40" runat="server" Font-Bold="True">___________________________________________________________________</asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="5" height="31">
                        </td>
                        <td colspan="6">
                            <asp:Label ID="Label39" runat="server" Font-Size="Smaller" Font-Names="Arial">Telefon privat / dienstlich</asp:Label>
                        </td>
                        <td colspan="2">
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="5" height="24">
                        </td>
                        <td colspan="13">
                            <asp:Label ID="Label41" runat="server" Font-Size="Smaller" Font-Names="Arial" Font-Bold="True">Erkl�rung des Anzeigenden</asp:Label>
                        </td>
                        <td colspan="7">
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="5" height="48">
                        </td>
                        <td colspan="20">
                            <asp:Label ID="Label42" runat="server" Font-Size="Smaller" Font-Names="Arial" Width="609px"
                                Height="39px">Das / die Kennzeichen wurden vom Halter bei R�ckgabe des Leasingfahrzeuges trotz Aufforderung nicht zur�ck gegeben. Der Halter ist nicht bereit, eine Verlusterkl�rung abzugeben.</asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="5" height="33">
                        </td>
                        <td colspan="20">
                            <asp:Label ID="Label46" runat="server" Font-Names="Arial" Font-Size="Smaller" Width="606px"
                                Height="28px">Ich werde die Zulassungsstelle f�r Kraftfahrzeuge umgehend verst�ndigen, wenn ich das / die Kennzeichen ohne deren Mitwirkung zur�ckerhalten sollte.</asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="18" height="1">
                        </td>
                        <td colspan="7" height="1">
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="7" height="19">
                        </td>
                        <td colspan="11">
                            <asp:Label ID="Label44" runat="server" Font-Bold="True">______________________________</asp:Label>
                        </td>
                        <td colspan="7" >
                            <asp:Label ID="Label43" runat="server" Font-Bold="True">______________________________</asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="7" height="1">
                        </td>
                        <td colspan="11">
                            <asp:Label ID="Label18" runat="server" Font-Size="XX-Small" Font-Names="Arial">Unterschrift des Sachbearbeiters</asp:Label>
                        </td>
                        <td colspan="6">
                            <asp:Label ID="Label8" runat="server" Font-Size="XX-Small" Font-Names="Arial">Unterschrift des Anzeigenden</asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="26" height="14">
                        </td>
                    </tr>
                </table>
                </form>
            </td>
        </tr>
    </table>
</body>
</html>
