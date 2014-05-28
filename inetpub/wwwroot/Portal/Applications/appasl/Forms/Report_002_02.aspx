<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report_002_02.aspx.vb"
    Inherits="AppASL.Report_002_02" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table width="100%" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server" Visible="False"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;(
                            <asp:Label ID="lblPageTitle" runat="server"> Detaildaten</asp:Label>)
                        </td>
                    </tr>
                    <tr>
                        <td class="TaskTitle" align="right" colspan="2">
                            &nbsp;
                            <asp:HyperLink ID="lnkFahrzeugsuche" runat="server" NavigateUrl="javascript:window.close()"
                                CssClass="TaskTitle">Fenster schließen</asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                        </td>
                        <td valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="5" width="100%" border="0">
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td>
                                        <table id="Table10" cellspacing="0" cellpadding="5" bgcolor="white" border="0">
                                            <tr>
                                                <td class="" valign="top" nowrap align="left">
                                                    <asp:Label ID="Label8" runat="server" CssClass="DetailTableFontBold" Font-Bold="True">LV-Nr. / Status</asp:Label>:
                                                </td>
                                                <td class="TextLarge" valign="top" nowrap align="left" colspan="3">
                                                    <asp:Label ID="lblLVNr" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>&nbsp;/&nbsp;
                                                    <asp:Label ID="lblStatus" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" width="91">
                                                    <asp:Label ID="Label17" runat="server" CssClass="DetailTableFont" Font-Size="">Angelegt am:</asp:Label>
                                                </td>
                                                <td valign="top" align="left">
                                                    <asp:Label ID="lblAntrag" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left">
                                                    <asp:Label ID="Label25" runat="server" CssClass="DetailTableFont" Font-Size="">Leasingdauer:</asp:Label>
                                                </td>
                                                <td class="" valign="top" align="left" colspan="3">
                                                    <asp:Label ID="lblLBeginn" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>&nbsp;-
                                                    <asp:Label ID="lblLEnde" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="PageNavigation">
                                                <td valign="top" nowrap align="left">
                                                    <font size="3">
                                                        <asp:Label ID="Label6" runat="server" CssClass="DetailTableFontBold" Font-Bold="True">Leasingnehmer</asp:Label></font>
                                                </td>
                                                <td valign="top" nowrap align="left" colspan="3">
                                                    <font size="3"><font size="2"></font></font>
                                                </td>
                                                <td valign="top" nowrap align="left" width="91">
                                                    <font size="3"><font size="3">
                                                        <asp:Label ID="Label35" runat="server" CssClass="DetailTableFontBold" Font-Bold="True">Konzern</asp:Label></font></font>
                                                </td>
                                                <td valign="top" nowrap align="left">
                                                    <font size="3"><font size="2"></font></font>
                                                </td>
                                                <td valign="top" nowrap align="left">
                                                    <asp:Label ID="Label7" runat="server" CssClass="DetailTableFontBold" Font-Bold="True">Versicherungsgeber</asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" width="100%" colspan="2">
                                                </td>
                                            </tr>
                                            <tr id="Tr5" runat="server">
                                                <td class="" valign="top" nowrap align="left">
                                                    <asp:Label ID="Label10" runat="server" CssClass="DetailTableFont">Name:</asp:Label>
                                                </td>
                                                <td class="" valign="top" nowrap align="left" colspan="3">
                                                    <asp:Label ID="lblNameLN" runat="server" CssClass="DetailTableFont" Font-Bold="True"></asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" width="91">
                                                    <asp:Label ID="Label28" runat="server" CssClass="DetailTableFont">Name:</asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left">
                                                    <asp:Label ID="lblName1" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                                <td class="" valign="top" nowrap align="left">
                                                    <asp:Label ID="Label13" runat="server" CssClass="DetailTableFont" Font-Size="">Versich.Schein-Nr.:</asp:Label>
                                                </td>
                                                <td class="" valign="top" nowrap align="left" colspan="2">
                                                    <asp:Label ID="lblVschein" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" nowrap align="left">
                                                    <asp:Label ID="Label37" runat="server" CssClass="DetailTableFont">Name 2:</asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" colspan="3">
                                                    <asp:Label ID="lblName2LN" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" width="91">
                                                    <asp:Label ID="Label29" runat="server" CssClass="DetailTableFont">Name 2:</asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left">
                                                    <asp:Label ID="lblName2" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left">
                                                    <asp:Label ID="Label14" runat="server" CssClass="DetailTableFont" Font-Size="">Versicherungsdauer:</asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" colspan="2">
                                                    <asp:Label ID="lblVBeginn" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>&nbsp;-&nbsp;
                                                    <asp:Label ID="lblVEnde" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" nowrap align="left">
                                                    <asp:Label ID="Label38" runat="server" CssClass="DetailTableFont">Name 3:</asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" colspan="3">
                                                    <asp:Label ID="lblName3LN" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" width="91">
                                                    <asp:Label ID="Label30" runat="server" CssClass="DetailTableFont">Name 3:</asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left">
                                                    <asp:Label ID="lblName3" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left">
                                                    <asp:Label ID="Label9" runat="server" CssClass="DetailTableFont" Font-Size="">Name:</asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" colspan="2">
                                                    <asp:Label ID="lblNameVG" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" nowrap align="left">
                                                    <asp:Label ID="Label22" runat="server" CssClass="DetailTableFont" Font-Size="">Straße:</asp:Label>
                                                </td>
                                                <td class="TextLarge" valign="top" nowrap align="left" colspan="3">
                                                    <asp:Label ID="lblStrLN" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" width="91">
                                                    <asp:Label ID="Label31" runat="server" CssClass="DetailTableFont">Straße:</asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left">
                                                    <asp:Label ID="lblStras_ZO" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                                <td class="" valign="top" nowrap align="left">
                                                    <asp:Label ID="Label11" runat="server" CssClass="DetailTableFont" Font-Size="">Straße:</asp:Label>
                                                </td>
                                                <td class="TextLarge" valign="top" nowrap align="left" colspan="2">
                                                    <asp:Label ID="lblStrVG" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="" valign="top" nowrap align="left" height="26">
                                                    <asp:Label ID="Label21" runat="server" CssClass="DetailTableFont" Font-Size=""> Postleitzahl:</asp:Label>
                                                </td>
                                                <td class="" valign="top" nowrap align="left" colspan="3">
                                                    <asp:Label ID="lblPLZLN" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" width="91">
                                                    <asp:Label ID="Label32" runat="server" CssClass="DetailTableFont">Plz:</asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left">
                                                    <asp:Label ID="lblPstlz_ZO" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left">
                                                    <asp:Label ID="Label12" runat="server" CssClass="DetailTableFont" Font-Size=""> Postleitzahl:</asp:Label>
                                                </td>
                                                <td class="" valign="top" nowrap align="left" colspan="2" height="26">
                                                    <asp:Label ID="lblPLZVG" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr id="Tr1" runat="server">
                                                <td class="" valign="top" nowrap align="left">
                                                    <asp:Label ID="Label23" runat="server" CssClass="DetailTableFont" Font-Size="">Ort:</asp:Label>
                                                </td>
                                                <td class="TextLarge" valign="top" nowrap align="left" colspan="3">
                                                    <asp:Label ID="lblOrtLN" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" width="91">
                                                    <asp:Label ID="Label33" runat="server" CssClass="DetailTableFont">Ort:</asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left">
                                                    <asp:Label ID="lblOrt_ZO" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                                <td class="" valign="top" nowrap align="left">
                                                    <asp:Label ID="Label39" runat="server" CssClass="DetailTableFont" Font-Size="">Ort:</asp:Label>
                                                </td>
                                                <td class="TextLarge" valign="top" nowrap align="left" colspan="2">
                                                    <asp:Label ID="lblOrtVG" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="" valign="top" nowrap align="left" height="30">
                                                    <asp:Label ID="Label24" runat="server" CssClass="DetailTableFont" Font-Size=""> Kunden-Nr.:</asp:Label>
                                                </td>
                                                <td class="" valign="top" nowrap align="left" colspan="3" height="30">
                                                    <asp:Label ID="lblKonzernID" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" width="91" height="30">
                                                    <asp:Label ID="Label34" runat="server" CssClass="DetailTableFont">Kunden-Nr:</asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" height="30">
                                                    <asp:Label ID="lblKonzs_ZO" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                                <td class="" valign="top" nowrap align="left">
                                                </td>
                                                <td class="" valign="top" nowrap align="left" colspan="2">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="" valign="top" nowrap align="left" height="27">
                                                    <asp:Label ID="Label18" runat="server" CssClass="DetailTableFont" Font-Size="">Versand:</asp:Label>
                                                </td>
                                                <td class="TextLarge" valign="top" nowrap align="left" colspan="3" height="27">
                                                    <asp:Label ID="lblVersandLN" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" height="27">
                                                </td>
                                                <td valign="top" nowrap align="left" height="27">
                                                </td>
                                                <td class="" valign="top" nowrap align="left" height="27">
                                                    <asp:Label ID="Label15" runat="server" CssClass="DetailTableFont" Font-Size="">Versand:</asp:Label>
                                                </td>
                                                <td class="TextLarge" valign="top" nowrap align="left" colspan="2" height="27">
                                                    <asp:Label ID="lblVersandVG" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" nowrap align="left">
                                                    <asp:Label ID="Label42" runat="server" CssClass="DetailTableFont" Font-Size="">Rückgabe:</asp:Label>
                                                </td>
                                                <td class="TextLarge" valign="top" nowrap align="left" colspan="3">
                                                    <asp:Label ID="lblRueckLN" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left">
                                                </td>
                                                <td valign="top" nowrap align="left">
                                                </td>
                                                <td valign="top" nowrap align="left">
                                                    <asp:Label ID="Label43" runat="server" CssClass="DetailTableFont" Font-Size="">Rückgabe:</asp:Label>
                                                </td>
                                                <td class="TextLarge" valign="top" nowrap align="left" colspan="2" height="28">
                                                    <asp:Label ID="lblRueckVG" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="" valign="top" nowrap align="left" width="154">
                                                    <asp:Label ID="Label19" runat="server" CssClass="DetailTableFont" Font-Size=""> Eingang unvollständig:</asp:Label>
                                                </td>
                                                <td class="" valign="top" nowrap align="left" colspan="3">
                                                    <asp:DropDownList ID="lblUnvLN" runat="server" CssClass="DetailTableFont" Width="200px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td valign="top" nowrap align="left">
                                                </td>
                                                <td valign="top" nowrap align="left">
                                                </td>
                                                <td class="" valign="top" nowrap align="left">
                                                    <asp:Label ID="Label16" runat="server" CssClass="DetailTableFont" Font-Size="">Eingang unvollständig:</asp:Label>
                                                </td>
                                                <td class="" valign="top" nowrap align="left" colspan="2">
                                                    <asp:DropDownList ID="lblUnvVG" runat="server" CssClass="DetailTableFont" Width="200px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr class="PageNavigation">
                                                <td valign="top" nowrap align="left" width="154" height="2">
                                                    <strong>
                                                        <asp:Label ID="Label1" runat="server" CssClass="DetailTableFontBold" Font-Size="">Mahndaten</asp:Label></strong>
                                                </td>
                                                <td class="" valign="top" nowrap align="left" colspan="3" height="2">
                                                </td>
                                                <td valign="top" nowrap align="left" height="2">
                                                </td>
                                                <td valign="top" nowrap align="left" height="2">
                                                </td>
                                                <td valign="top" nowrap align="left">
                                                </td>
                                                <td class="" valign="top" nowrap align="left" colspan="2" height="2">
                                                    <p>
                                                        &nbsp;</p>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" nowrap align="left">
                                                    <asp:Label ID="Label26" runat="server" CssClass="DetailTableFont" Font-Size="">Zuletzt gemahnt / Stufe:</asp:Label>
                                                </td>
                                                <td class="TextLarge" valign="top" nowrap align="left" colspan="3">
                                                    <asp:Label ID="lblMadatLN" runat="server" CssClass="DetailTableFont" Font-Bold="True"></asp:Label>&nbsp;/&nbsp;<asp:Label
                                                        ID="lblMahnsLN" runat="server" CssClass="DetailTableFont" Font-Bold="True"></asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left">
                                                </td>
                                                <td valign="top" nowrap align="left">
                                                </td>
                                                <td valign="top" nowrap align="left">
                                                    <asp:Label ID="Label27" runat="server" CssClass="DetailTableFont" Font-Size="">Zuletzt gemahnt / Stufe:</asp:Label>
                                                </td>
                                                <td class="TextLarge" valign="top" nowrap align="left" colspan="2">
                                                    <asp:Label ID="lblMadatVG" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                        Font-Size=""></asp:Label>&nbsp;/&nbsp;<asp:Label ID="lblMahnsVG" runat="server" CssClass="DetailTableFont"
                                                            Font-Bold="True" Font-Size=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="PageNavigation">
                                                <td valign="top" nowrap align="left">
                                                    <asp:Label ID="Label20" runat="server" CssClass="DetailTableFontBold" Font-Bold="True">Versicherungsumfang</asp:Label>
                                                </td>
                                                <td class="TextLarge" valign="top" nowrap align="left" colspan="8">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" nowrap align="left">
                                                </td>
                                                <td class="TextLarge" valign="top" nowrap align="left" colspan="6">
                                                    <asp:Label ID="lblVersUmf" runat="server" Font-Bold="True"></asp:Label>
                                                </td>
                                                <td class="TextLarge" valign="top" nowrap align="left">
                                                </td>
                                                <td class="TextLarge" valign="top" nowrap align="left">
                                                </td>
                                            </tr>
                                            <tr class="PageNavigation">
                                                <td valign="top" nowrap align="left" width="154">
                                                    <strong>
                                                        <asp:Label ID="Label5" runat="server" CssClass="DetailTableFontBold" Font-Size="">Fahrzeugdaten</asp:Label></strong>
                                                </td>
                                                <td class="" valign="top" nowrap align="left" colspan="3">
                                                </td>
                                                <td valign="top" nowrap align="left">
                                                    <asp:Label ID="lblVersumfang" runat="server" CssClass="DetailTableFontBold" Font-Size=""> Bemerkungen</asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left">
                                                </td>
                                                <td valign="top" nowrap align="left">
                                                    <asp:Label ID="Label36" runat="server" CssClass="DetailTableFont" Font-Size="">Kundenbetreuer(in):</asp:Label>
                                                </td>
                                                <td class="" valign="top" nowrap align="left" colspan="3">
                                                    <strong>
                                                        <p align="left">
                                                            <asp:Label ID="lblKonzs_ZK" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                                Font-Size=""></asp:Label>&nbsp;
                                                            <asp:Label ID="lblName1_ZK" runat="server" CssClass="DetailTableFont" Font-Bold="True"
                                                                Font-Size=""></asp:Label></p>
                                                    </strong>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" nowrap align="left">
                                                    <asp:Label ID="Label4" runat="server" CssClass="DetailTableFont" Font-Size=""> Erstzulassung:</asp:Label>
                                                </td>
                                                <td class="TextLarge" valign="top" nowrap align="left" colspan="3">
                                                    <asp:Label ID="lblEz" runat="server" CssClass="DetailTableFont" Font-Bold="True"></asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left">
                                                    <p align="left">
                                                        <asp:Label ID="lblInfo" runat="server" CssClass="TextError" Font-Bold="True" Font-Size=""></asp:Label></p>
                                                </td>
                                                <td valign="top" nowrap align="left" colspan="5">
                                                    <p align="left">
                                                        <table id="tblBemerkungen" cellspacing="1" cellpadding="1" width="100%" border="0"
                                                            runat="server">
                                                            <tr>
                                                                <td nowrap>
                                                                    <asp:Label ID="lblB1" runat="server" CssClass="DetailTableFont" Font-Size=""></asp:Label>
                                                                </td>
                                                                <td nowrap>
                                                                    <asp:Label ID="lblB2" runat="server" CssClass="DetailTableFont" Font-Size=""></asp:Label>
                                                                </td>
                                                                <td nowrap>
                                                                    <asp:Label ID="lblB3" runat="server" CssClass="DetailTableFont" Font-Size=""></asp:Label>
                                                                </td>
                                                                <td nowrap width="100%">
                                                                    <asp:Label ID="lblB4" runat="server" CssClass="DetailTableFont" Font-Size=""></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </p>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" nowrap align="left">
                                                    <asp:Label ID="Label40" runat="server" CssClass="DetailTableFont" Font-Size="">Hersteller / Typ:</asp:Label>
                                                </td>
                                                <td class="TextLarge" valign="top" nowrap align="left" colspan="3">
                                                    <asp:Label ID="lblHerst" runat="server" CssClass="DetailTableFont" Font-Bold="True"></asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" colspan="6">
                                                    <p align="left">
                                                        <asp:DropDownList ID="ddl1" runat="server" CssClass="DetailTableFont">
                                                        </asp:DropDownList>
                                                    </p>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" nowrap align="left">
                                                    <asp:Label ID="Label3" runat="server" CssClass="DetailTableFont" Font-Size=""> Kennzeichen:</asp:Label>
                                                </td>
                                                <td class="TextLarge" valign="top" nowrap align="left" colspan="3">
                                                    <asp:Label ID="lblKennz" runat="server" CssClass="DetailTableFont" Font-Bold="True"></asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" colspan="5" height="28">
                                                    <p align="left">
                                                        <asp:Label ID="lblBemerkungen" runat="server" CssClass="" Font-Size=""><u>Bemerkungen 
																		erfassen:</u></asp:Label></p>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" nowrap align="left">
                                                    <asp:Label ID="Label41" runat="server" CssClass="DetailTableFont" Font-Size="">Fahrzeugart:</asp:Label>
                                                </td>
                                                <td class="TextLarge" valign="top" nowrap align="left" colspan="3">
                                                    <asp:Label ID="lblFzArt" runat="server" CssClass="DetailTableFont" Font-Bold="True"></asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" colspan="5">
                                                    <table id="tblBem1" cellspacing="1" cellpadding="1" border="0" runat="server">
                                                        <tr>
                                                            <td nowrap>
                                                                1:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBem1" runat="server" MaxLength="25"></asp:TextBox>
                                                            </td>
                                                            <td nowrap>
                                                                2:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBem2" runat="server" MaxLength="25"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" nowrap align="left" height="17">
                                                    <asp:Label ID="Label2" runat="server" CssClass="DetailTableFont" Font-Size="">Fahrgestellnr.:</asp:Label>
                                                </td>
                                                <td class="TextLarge" valign="top" nowrap align="left" colspan="3" height="17">
                                                    <asp:Label ID="lblFGNr" runat="server" CssClass="DetailTableFont" Font-Bold="True"></asp:Label>
                                                </td>
                                                <td valign="top" nowrap align="left" colspan="5">
                                                    <table id="tblBem2" cellspacing="1" cellpadding="1" border="0" runat="server">
                                                        <tr>
                                                            <td nowrap>
                                                                3:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBem3" runat="server" MaxLength="25"></asp:TextBox>
                                                            </td>
                                                            <td nowrap>
                                                                4:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBem4" runat="server" MaxLength="25"></asp:TextBox>&nbsp;<asp:LinkButton
                                                                    ID="btnSave" runat="server" CssClass="StandardButtonTable"> &#149;&nbsp;Speichern</asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="lblError" runat="server" CssClass="TextError" Font-Size="" EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <!--#include File="../../../PageElements/Footer.html" -->
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="ShowScript" runat="server" visible="False">
            <td>
                <script language="Javascript">
						<!--                    //
                    function FreigebenConfirm(Fahrgest, Vertrag, BriefNr, Kennzeichen) {
                        var Check = window.confirm("Wollen Sie für dieses Fahrzeug wirklich den Status 'Bezahlt' setzen?\t\n\tFahrgestellnr.\t" + Fahrgest + "\t\n\tVertrag\t\t" + Vertrag + "\t\n\tKfz-Briefnr.\t" + BriefNr + "\n\tKfz-Kennzeichen\t" + Kennzeichen);
                        return (Check);
                    }
						//-->
                </script>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
