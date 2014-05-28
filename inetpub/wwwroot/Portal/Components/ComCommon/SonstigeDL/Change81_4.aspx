<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change81_4.aspx.vb" Inherits="CKG.Components.ComCommon.Change81_4" %>

<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
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
<style>
    .Gridlines tr td{
        border: #a9a9a9 solid 1px;
        padding-left:3px;
    }
    .Gridlines tr td table tr td{
        border: none;
        padding-left:0px;
        white-space: nowrap;
    }
    .Gridlines tr .TableInGrid td{
        padding-left: 0px;
    }
</style>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
        <tr>
            <td colspan="3">
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td valign="top" align="left" colspan="3">
                <table id="Table10" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="3">
                            <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Bestätigung)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120">
                            <table id="Table12" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                border="0">
                                <tr>
                                    <td class="TaskTitle" width="150">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <p>
                                            <asp:LinkButton ID="cmdSave" runat="server" CssClass="StandardButton"> &#149;&nbsp;Absenden</asp:LinkButton><u></u></p>
                                    </td>
                                </tr>
                            </table>
                            <p align="right">
                                &nbsp;</p>
                        </td>
                        <td valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        <asp:HyperLink ID="lnkFahrzeugsuche" runat="server" CssClass="TaskTitle" NavigateUrl="Change04.aspx">Fahrzeugsuche</asp:HyperLink>&nbsp;<asp:HyperLink
                                            ID="lnkFahrzeugAuswahl" runat="server" CssClass="TaskTitle" NavigateUrl="Change04_2.aspx">Fahrzeugauswahl</asp:HyperLink>&nbsp;<asp:HyperLink
                                                ID="lnkAdressAuswahl" runat="server" CssClass="TaskTitle" NavigateUrl="Change04_3.aspx">Beauftragungsdetails</asp:HyperLink><asp:Label
                                                    ID="lblAddress" runat="server" Visible="False"></asp:Label><asp:Label ID="lblMaterialNummer"
                                                        runat="server" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td valign="top" align="left" colspan="3">
                                        <table id="Table1" cellspacing="0" cellpadding="5" width="100%" border="0">
                                            <tr>
                                                <td class="LabelExtraLarge" valign="top" align="left">
                                                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <table id="Table7" cellspacing="0" cellpadding="5" width="100%" bgcolor="white" border="0">
                                            <tr id="trVersandTemp" runat="server">
                                                <td class="" valign="top" nowrap>
                                                    <asp:Label ID="lblBeauftragteDienstleistung" runat="server" Font-Underline="True">Beauftragte Dienstleistung:</asp:Label>
                                                </td>
                                                <td class="TextLarge" valign="top" align="left" colspan="2" width="100%">
                                                    <asp:Label ID="lblBeauftragteDienstleistungAnzeige" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr id="trVersandArt" runat="server">
                                                <td class="" valign="top" nowrap>
                                                    <asp:Label ID="lblEingaben" runat="server" Font-Underline="True">Eingaben:</asp:Label>
                                                </td>
                                                <td class="TextLarge" valign="top" align="left">
                                                    <table id="Table2" cellspacing="0" cellpadding="0" class="Gridlines">
                                                        <tr id="trNeuerHalter01" runat="server">
                                                            <td class="TextLarge" colspan="2">
                                                                &nbsp;&nbsp;&nbsp;Neuer Halter
                                                            </td>
                                                        </tr>
                                                        <tr id="trNeuerHalter02" runat="server" class="TableInGrid">
                                                            <td class="TextLarge" colspan="2">
                                                                <table id="Table9" cellspacing="0" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td class="StandardTableAlternate" align="left">
                                                                            &nbsp;&nbsp;&nbsp;Name 1&nbsp;&nbsp;&nbsp;&nbsp;
                                                                        </td>
                                                                        <td class="StandardTableAlternate">
                                                                            <asp:Label ID="lblHalterName1" runat="server" Width="250px"></asp:Label>
                                                                        </td>
                                                                        <td class="StandardTableAlternate" align="left">
                                                                            &nbsp;&nbsp;&nbsp;Name 2&nbsp;&nbsp;&nbsp;&nbsp;
                                                                        </td>
                                                                        <td class="StandardTableAlternate">
                                                                            <asp:Label ID="lblHalterName2" runat="server" Width="250px"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="TextLarge" align="left">
                                                                            &nbsp;&nbsp;&nbsp;Straße &nbsp;&nbsp;&nbsp;
                                                                        </td>
                                                                        <td class="TextLarge">
                                                                            <asp:Label ID="lblHalterStrasse" runat="server" Width="250px"></asp:Label>
                                                                        </td>
                                                                        <td class="TextLarge">
                                                                            &nbsp;&nbsp;&nbsp;Hausnr.
                                                                        </td>
                                                                        <td class="TextLarge">
                                                                            <asp:Label ID="lblHalterHausnr" runat="server" Width="250px"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="StandardTableAlternate" align="left">
                                                                            &nbsp;&nbsp;&nbsp;PLZ&nbsp;&nbsp;&nbsp;&nbsp;
                                                                        </td>
                                                                        <td class="StandardTableAlternate">
                                                                            <asp:Label ID="lblHalterPLZ" runat="server" Width="250px"></asp:Label>
                                                                        </td>
                                                                        <td class="StandardTableAlternate" align="left">
                                                                            &nbsp;&nbsp;&nbsp;Ort &nbsp;&nbsp;&nbsp;
                                                                        </td>
                                                                        <td class="StandardTableAlternate">
                                                                            <asp:Label ID="lblHalterOrt" runat="server" Width="250px"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr id="trNeuerStandort01" runat="server">
                                                            <td class="TextLarge" colspan="2">
                                                                &nbsp;&nbsp;&nbsp;Neuer Standort
                                                            </td>
                                                        </tr>
                                                        <tr id="trNeuerStandort02" runat="server" class="TableInGrid">
                                                            <td class="TextLarge" colspan="2">
                                                                <table id="Table11" cellspacing="0" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td class="StandardTableAlternate" align="left">
                                                                            &nbsp;&nbsp;&nbsp;Name 1&nbsp;&nbsp;&nbsp;&nbsp;
                                                                        </td>
                                                                        <td class="StandardTableAlternate">
                                                                            <asp:Label ID="lblStandortName1" runat="server" Width="250px"></asp:Label>
                                                                        </td>
                                                                        <td class="StandardTableAlternate" align="left">
                                                                            &nbsp;&nbsp;&nbsp;Name 2&nbsp;&nbsp;&nbsp;&nbsp;
                                                                        </td>
                                                                        <td class="StandardTableAlternate">
                                                                            <asp:Label ID="lblStandortName2" runat="server" Width="250px"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="TextLarge" align="left">
                                                                            &nbsp;&nbsp;&nbsp;Straße&nbsp; &nbsp;&nbsp;
                                                                        </td>
                                                                        <td class="TextLarge">
                                                                            <asp:Label ID="lblStandortStrasse" runat="server" Width="250px"></asp:Label>
                                                                        </td>
                                                                        <td class="TextLarge">
                                                                            &nbsp;&nbsp;&nbsp;Hausnr.
                                                                        </td>
                                                                        <td class="TextLarge">
                                                                            <asp:Label ID="lblStandortHausnr" runat="server" Width="250px"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="StandardTableAlternate" align="left">
                                                                            &nbsp;&nbsp;&nbsp;PLZ&nbsp;&nbsp;&nbsp;&nbsp;
                                                                        </td>
                                                                        <td class="StandardTableAlternate">
                                                                            <asp:Label ID="lblStandortPLZ" runat="server" Width="250px"></asp:Label>
                                                                        </td>
                                                                        <td class="StandardTableAlternate" align="left">
                                                                            &nbsp;&nbsp;&nbsp;Ort&nbsp; &nbsp;&nbsp;
                                                                        </td>
                                                                        <td class="StandardTableAlternate">
                                                                            <asp:Label ID="lblStandortOrt" runat="server" Width="250px"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr id="trWunschkennzeichen" runat="server">
                                                            <td class="TextLarge">
                                                                &nbsp;&nbsp;&nbsp;Wunschkennzeichen
                                                            </td>
                                                            <td >
                                                                <table id="Table8" cellspacing="0" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td class="TextLarge">
                                                                            <asp:Label ID="lblKreis" runat="server"></asp:Label>&nbsp; -&nbsp;&nbsp;
                                                                        </td>
                                                                        <td class="TextLarge">
                                                                            <asp:Label ID="lblWunschkennzeichen" runat="server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr id="trReserviertAuf" runat="server">
                                                            <td class="TextLarge">
                                                                &nbsp;&nbsp;&nbsp;reserviert auf
                                                            </td>
                                                            <td class="TextLarge">
                                                                <asp:Label ID="lblReserviertAuf" runat="server" Width="250px"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="trVersicherungstraeger" runat="server">
                                                            <td class="TextLarge">
                                                                &nbsp;&nbsp;&nbsp;Versicherungsträger
                                                            </td>
                                                            <td class="TextLarge">
                                                                <asp:Label ID="lblVersicherungstraeger" runat="server" Width="250px"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="trevb" runat="server">
                                                            <td class="TextLarge">
                                                                &nbsp;&nbsp;&nbsp;eVB
                                                            </td>
                                                            <td class="TextLarge">
                                                                <asp:Label ID="lblEVB" runat="server" Width="250px"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="TrGueltigkeit" runat="server">
                                                            <td class="TextLarge">
                                                                &nbsp;&nbsp;&nbsp;Gültigkeit
                                                            </td>
                                                            <td class="TextLarge">
                                                                von &nbsp;&nbsp;
                                                                <asp:Label ID="lblDatumVON" runat="server" Width="100px"></asp:Label> &nbsp;&nbsp;
                                                                bis &nbsp;&nbsp;
                                                                <asp:Label ID="lblDatumBis" runat="server" Width="100px"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="trEmpfaenger01" runat="server">
                                                            <td class="TextLarge" colspan="2">
                                                                &nbsp;&nbsp;&nbsp;Empfänger Schein / Schilder
                                                            </td>
                                                        </tr>
                                                        <tr id="trEmpfaenger02" runat="server" class="TableInGrid">
                                                            <td class="TextLarge" colspan="2">
                                                                <table id="Table4" cellspacing="0" cellpadding="0" border="0" width="100%">
                                                                    <tr>
                                                                        <td class="StandardTableAlternate" align="left">
                                                                            &nbsp;&nbsp;&nbsp;Name 1&nbsp;&nbsp;&nbsp;&nbsp;
                                                                        </td>
                                                                        <td class="StandardTableAlternate">
                                                                            <asp:Label ID="lblEmpfaengerName1" runat="server" Width="250px"></asp:Label>
                                                                        </td>
                                                                        <td class="StandardTableAlternate" align="left">
                                                                            &nbsp;&nbsp;&nbsp;Name 2&nbsp;&nbsp;&nbsp;&nbsp;
                                                                        </td>
                                                                        <td class="StandardTableAlternate">
                                                                            <asp:Label ID="lblEmpfaengerName2" runat="server" Width="250px"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="TextLarge" align="left">
                                                                            &nbsp;&nbsp;&nbsp;Straße&nbsp;&nbsp;&nbsp;
                                                                        </td>
                                                                        <td class="TextLarge">
                                                                            <asp:Label ID="lblEmpfaengerStrasse" runat="server" Width="250px"></asp:Label>
                                                                        </td>
                                                                        <td class="TextLarge">
                                                                            &nbsp;&nbsp;&nbsp;Hausnr.
                                                                        </td>
                                                                        <td class="TextLarge" >
                                                                            <asp:Label ID="lblEmpfaengerHausnr" runat="server" Width="250px"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="StandardTableAlternate" align="left">
                                                                            &nbsp;&nbsp;&nbsp;PLZ&nbsp;&nbsp;&nbsp;&nbsp;
                                                                        </td>
                                                                        <td class="StandardTableAlternate">
                                                                            <asp:Label ID="lblEmpfaengerPLZ" runat="server" Width="250px"></asp:Label>
                                                                        </td>
                                                                        <td class="StandardTableAlternate" align="left">
                                                                            &nbsp;&nbsp;&nbsp;Ort&nbsp; &nbsp;&nbsp;
                                                                        </td>
                                                                        <td class="StandardTableAlternate">
                                                                            <asp:Label ID="lblEmpfaengerOrt" runat="server" Width="250px"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr id="trDurchfuehrungsdatum" runat="server">
                                                            <td class="TextLarge">
                                                                &nbsp;&nbsp;&nbsp;gewünschtes Durchführungsdatum&nbsp;&nbsp;
                                                            </td>
                                                            <td class="TextLarge">
                                                                <asp:Label ID="lblDurchfuehrungsDatum" runat="server" Width="250px"></asp:Label><br>
                                                            </td>
                                                        </tr>
                                                        <tr id="trHinweis" runat="server">
                                                            <td class="TextLarge" height="25">
                                                                &nbsp;&nbsp;&nbsp;Hinweis
                                                            </td>
                                                            <td class="TextLarge" height="25">
                                                                <asp:Label ID="lblHinweis" runat="server" Width="374px"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="trBemerkung" runat="server">
                                                            <td class="TextLarge" valign="top">
                                                                &nbsp;&nbsp;&nbsp;Bemerkung
                                                            </td>
                                                            <td class="TextLarge">
                                                                <asp:Label ID="lblBemerkung" runat="server" Width="374px" Height="71px"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <p>
                                                        <asp:Label ID="lblFahrzeugangaben" runat="server" Font-Underline="True">Fahrzeugangaben:</asp:Label></p>
                                                    <p>
                                                        &nbsp;</p>
                                                </td>
                                                <td valign="top" align="left">
                                                    <asp:DataGrid ID="DataGrid1" runat="server" Width="100%" headerCSS="tableHeader"
                                                        bodyCSS="tableBody" CssClass="tableMain" bodyHeight="250" AutoGenerateColumns="False"
                                                        AllowSorting="True">
                                                        <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                        <HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundColumn Visible="False" DataField="MANDT" SortExpression="MANDT" HeaderText="MANDT">
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="CHASSIS_NUM" SortExpression="CHASSIS_NUM" HeaderText="Fahrgestellnummer">
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="LICENSE_NUM" SortExpression="LICENSE_NUM" HeaderText="Kennzeichen">
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="LIZNR" SortExpression="LIZNR" HeaderText="Leasing-&lt;br&gt;vertrags-Nr.">
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="TIDNR" SortExpression="TIDNR" HeaderText="KFZ-Briefnummer">
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn Visible="False" DataField="ZZREFERENZ1" SortExpression="ZZREFERENZ1"
                                                                HeaderText="Ordernummer"></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="STATUS" ItemStyle-Font-Bold="true" SortExpression="STATUS"
                                                                HeaderText="Status"></asp:BoundColumn>
                                                            <asp:TemplateColumn Visible="False" HeaderText="Anfordern">
                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chk0001" runat="server" Enabled="False"></asp:CheckBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                        </Columns>
                                                        <PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True"
                                                            PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Wrap="False"></PagerStyle>
                                                    </asp:DataGrid>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left">
                                        <p align="left">
                                            <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label></p>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <!--#include File="../../../PageElements/Footer.html" -->
                                        <p align="left">
                                            &nbsp;</p>
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
