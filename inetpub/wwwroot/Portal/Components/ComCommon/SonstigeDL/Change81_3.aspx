<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change81_3.aspx.vb" Inherits="CKG.Components.ComCommon.Change81_3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
    <head runat="server">
        <title></title>
        <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
        <meta content="Visual Basic 7.0" name="CODE_LANGUAGE" />
        <meta content="JavaScript" name="vs_defaultClientScript" />
        <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema" />
        <uc1:styles id="ucStyles" runat="server"></uc1:styles>
    </head>
    <body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="scriptmanager1" EnableScriptGlobalization="true" runat="server">
    </asp:ScriptManager>
    <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
        <tr>
            <td colspan="3">
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../Images/empty.gif"
                    Width="3px"></asp:ImageButton>
            </td>
        </tr>
        <tr>
            <td valign="top" align="left" colspan="3">
                <table id="Table10" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="3">
                            <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Beauftragungsdetails)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120">
                            <table id="Table12" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120" border="0">
                                <tr>
                                    <td class="TaskTitle" width="150">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdSave" runat="server" CssClass="StandardButton"> &#149;&nbsp;Weiter</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdSearch" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Suchen</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        <asp:HyperLink ID="lnkFahrzeugsuche" runat="server" CssClass="TaskTitle" NavigateUrl="Change04.aspx">Fahrzeugsuche</asp:HyperLink>&nbsp;<asp:HyperLink
                                            ID="lnkFahrzeugAuswahl" runat="server" CssClass="TaskTitle" NavigateUrl="Change04_2.aspx">Fahrzeugauswahl</asp:HyperLink>
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td valign="top" align="left" >
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TextLarge" valign="top" align="left"  height="130">
                                        <table id="Table1" cellspacing="0" cellpadding="0" border="0">
                                            <tr>
                                                <td class="TextLarge" colspan="2">
                                                    <asp:DropDownList ID="ddlDienstleistung" runat="server" Width="358px" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" colspan="2">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" colspan="2">
                                                    <table id="Table2" bordercolor="#a9a9a9" cellspacing="0" cellpadding="0" border="1">
                                                        <tr id="trNeuerHalter1" runat="server">
                                                            <td class="TextLarge" colspan="2">
                                                                Neuer Halter
                                                            </td>
                                                        </tr>
                                                        <tr id="trNeuerHalter2" runat="server">
                                                            <td class="StandardTableAlternate" colspan="2">
                                                                <table id="Table9" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                    <tr>
                                                                        <td class="StandardTableAlternate" align="left">
                                                                            Name&nbsp;1
                                                                        </td>
                                                                        <td class="StandardTableAlternate">
                                                                            <asp:TextBox ID="txtHalterName1" runat="server" Width="250px"></asp:TextBox>
                                                                        </td>
                                                                        <td class="StandardTableAlternate" align="left">
                                                                            Name&nbsp;2
                                                                        </td>
                                                                        <td class="StandardTableAlternate">
                                                                            <asp:TextBox ID="txtHalterName2" runat="server" Width="250px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="StandardTableAlternate" align="left">
                                                                            Straße
                                                                        </td>
                                                                        <td class="StandardTableAlternate">
                                                                            <asp:TextBox ID="txtHalterStrasse" runat="server" Width="250px"></asp:TextBox>
                                                                        </td>
                                                                        <td class="StandardTableAlternate">
                                                                            Hausnr
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtHalterHausnr" runat="server" Width="62px" MaxLength="7"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="StandardTableAlternate" align="left">
                                                                            PLZ
                                                                        </td>
                                                                        <td class="StandardTableAlternate">
                                                                            <asp:TextBox ID="txtHalterPLZ" runat="server" Width="250px"></asp:TextBox>
                                                                        </td>
                                                                        <td class="StandardTableAlternate" align="left">
                                                                            Ort
                                                                        </td>
                                                                        <td class="StandardTableAlternate">
                                                                            <asp:TextBox ID="txtHalterOrt" runat="server" Width="250px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr id="trNeuerStandort1" runat="server">
                                                            <td colspan="2">
                                                                <table id="Table44" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                    <tr>
                                                                        <td class="TextLarge">
                                                                            Neuer Standort
                                                                        </td>
                                                                        <td align="right">
                                                                            <asp:LinkButton ID="lnkStandortAnzeige" runat="server">Anzeigen</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr id="trNeuerStandort2" runat="server">
                                                            <td class="TextLarge" colspan="2">
                                                                <table id="Table4" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                    <tr>
                                                                        <td class="StandardTableAlternate" align="left">
                                                                            Name 1
                                                                        </td>
                                                                        <td class="StandardTableAlternate">
                                                                            <asp:TextBox ID="txtStandortName1" runat="server" Width="250px"></asp:TextBox>
                                                                        </td>
                                                                        <td class="StandardTableAlternate" align="left">
                                                                            Name&nbsp;2
                                                                        </td>
                                                                        <td class="StandardTableAlternate">
                                                                            <asp:TextBox ID="txtStandortName2" runat="server" Width="250px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="TextLarge" align="left">
                                                                            Straße
                                                                        </td>
                                                                        <td class="TextLarge">
                                                                            <asp:TextBox ID="txtStandortStrasse" runat="server" Width="250px"></asp:TextBox>
                                                                        </td>
                                                                        <td class="TextLarge">
                                                                            Hausnr
                                                                        </td>
                                                                        <td class="TextLarge">
                                                                            <asp:TextBox ID="txtStandortHausnr" runat="server" Width="62px" MaxLength="7"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="StandardTableAlternate" align="left">
                                                                            PLZ
                                                                        </td>
                                                                        <td class="StandardTableAlternate">
                                                                            <asp:TextBox ID="txtStandortPLZ" runat="server" Width="250px"></asp:TextBox>
                                                                        </td>
                                                                        <td class="StandardTableAlternate" align="left">
                                                                            Ort
                                                                        </td>
                                                                        <td class="StandardTableAlternate">
                                                                            <asp:TextBox ID="txtStandortOrt" runat="server" Width="250px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr id="trWunschkennzeichen" runat="server">
                                                            <td class="TextLarge">
                                                                Wunschkennzeichen
                                                            </td>
                                                            <td>
                                                                <table id="Table8" cellspacing="0" cellpadding="0" width="180" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="txtKreis" runat="server" Width="30px"></asp:TextBox><asp:Label ID="lblKreis"
                                                                                runat="server"></asp:Label>&nbsp; -&nbsp;&nbsp;
                                                                        </td>
                                                                        <td align="right">
                                                                            <asp:TextBox ID="txtWunschkennzeichen" runat="server" Width="92px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr id="trReserviertAuf" runat="server">
                                                            <td class="StandardTableAlternate">
                                                                reserviert auf
                                                            </td>
                                                            <td class="StandardTableAlternate">
                                                                <asp:TextBox ID="txtReserviertAuf" runat="server" Width="181px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr id="trVersicherungstraeger" runat="server">
                                                            <td class="TextLarge" width="231">
                                                                Versicherungsträger
                                                            </td>
                                                            <td>
                                                                <table id="Table5" cellspacing="0" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td width="182px">
                                                                            <asp:TextBox ID="txtVersicherungstraeger" runat="server" Width="182px"></asp:TextBox>
                                                                        </td>
                                                                        <td width="50" align="right">
                                                                            <asp:Label ID="lblEVB" runat="server">eVB</asp:Label>&nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtEVB" runat="server" Width="173px" MaxLength="7"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr id="trGueltigkeit" runat="server">
                                                            <td class="TextLarge">
                                                                Gültigkeit*
                                                            </td>
                                                            <td>
                                                                <table id="Table11" cellspacing="0" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td align="right" width="182px" nowrap="nowrap">
                                                                            <asp:Label ID="Label2" runat="server">von</asp:Label>
                                                                            &nbsp;
                                                                            <asp:TextBox ID="txtDatumVon" runat="server" Width="100px" ToolTip="Gültigkeit von"></asp:TextBox>
                                                                        </td>
                                                                        <td width="50" align="right">
                                                                            <asp:Label ID="Label1" runat="server">bis</asp:Label>&nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtDatumbis" runat="server" Width="100px" ToolTip="Gültigkeit bis"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr id="trEmpfaenger1" runat="server">
                                                            <td class="StandardTableAlternate" colspan="2">
                                                                Empfänger Schein / Schilder
                                                            </td>
                                                        </tr>
                                                        <tr id="trEmpfaenger2" runat="server">
                                                            <td class="StandardTableAlternate" colspan="2">
                                                                <table id="Table7" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                    <tr>
                                                                        <td class="StandardTableAlternate" align="left">
                                                                            Name 1
                                                                        </td>
                                                                        <td class="StandardTableAlternate">
                                                                            <asp:TextBox ID="txtEmpfaengerName1" runat="server" Width="250px"></asp:TextBox>
                                                                        </td>
                                                                        <td class="StandardTableAlternate" align="left">
                                                                            Name 2
                                                                        </td>
                                                                        <td class="StandardTableAlternate">
                                                                            <asp:TextBox ID="txtEmpfaengerName2" runat="server" Width="250px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="StandardTableAlternate" align="left">
                                                                            Straße
                                                                        </td>
                                                                        <td class="StandardTableAlternate">
                                                                            <asp:TextBox ID="txtEmpfaengerStrasse" runat="server" Width="250px"></asp:TextBox>
                                                                        </td>
                                                                        <td class="StandardTableAlternate">
                                                                            Hausnr
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtEmpfaengerHausnr" runat="server" Width="62px" MaxLength="7"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="StandardTableAlternate" align="left">
                                                                            PLZ
                                                                        </td>
                                                                        <td class="StandardTableAlternate">
                                                                            <asp:TextBox ID="txtEmpfaengerPLZ" runat="server" Width="250px"></asp:TextBox>
                                                                        </td>
                                                                        <td class="StandardTableAlternate" align="left">
                                                                            Ort
                                                                        </td>
                                                                        <td class="StandardTableAlternate">
                                                                            <asp:TextBox ID="txtEmpfaengerOrt" runat="server" Width="250px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr id="trDurchfuehrungsdatum" runat="server">
                                                            <td class="TextLarge" width="231">
                                                                gewünschtes Durchführungsdatum&nbsp;&nbsp;<br>
                                                                (tt.mm.jjjj)
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtDurchfuehrungsDatum" runat="server" Width="180px"></asp:TextBox>&nbsp;&nbsp;
                                                                <cc1:CalendarExtender ID="txtDurchfuehrungsDatum_CalendarExtender" runat="server"
                                                                    Format="dd.MM.yyyy" PopupPosition="BottomLeft" Animated="true" Enabled="True"
                                                                    TargetControlID="txtDurchfuehrungsDatum">
                                                                </cc1:CalendarExtender>
                                                                <cc1:MaskedEditExtender ID="meeZulassungsdatum" runat="server" TargetControlID="txtDurchfuehrungsDatum"
                                                                    Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                                </cc1:MaskedEditExtender>
                                                                <cc1:MaskedEditValidator ID="mevZulassungsdatum" runat="server" ControlToValidate="txtDurchfuehrungsDatum"
                                                                    ControlExtender="meeZulassungsdatum" Display="none" IsValidEmpty="True" Enabled="true"
                                                                    EmptyValueMessage="Bitte geben Sie ein gültiges Durchführungsdatum ein" InvalidValueMessage="Bitte geben Sie ein gültiges Zulassungsdatum ein">
                                                                </cc1:MaskedEditValidator>
                                                                <br />
                                                            </td>
                                                        </tr>
                                                        <tr id="trHinweis" runat="server">
                                                            <td class="TextLarge" width="231" height="25">
                                                                Hinweis
                                                            </td>
                                                            <td height="25">
                                                                <asp:Label ID="lblHinweis" runat="server" Width="374px" ForeColor="Red" Font-Bold="True"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="trBemerkung" runat="server">
                                                            <td class="StandardTableAlternate" valign="top" width="231">
                                                                Bemerkung
                                                            </td>
                                                            <td class="StandardTableAlternate">
                                                                <asp:TextBox ID="txtBemerkung" runat="server" Width="374px" Height="71px" TextMode="MultiLine"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr id="trHinweisGueltigkeit" runat="server" visible="False">
                                                            <td colspan="2">
                                                                <div dir="ltr" align="left">
                                                                    <font face="Arial" size="2"><span class="951411616-07022008">* Bitte folgendes Datumsformat
                                                                        "TT.MM.JJJJ" zur Eingabe nutzen!</span></font></div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" >
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td >
                                        <!--#include File="../../../PageElements/Footer.html" -->
                                        <br />
                                    </td>
                                </tr>
                            </table>
                            <asp:Literal ID="litScript" runat="server"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</HTML>
