<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DatenarchivierungDAD.aspx.vb"
    Inherits="CKG.Admin.DatenarchivierungDAD" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
    
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table4" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server">Administration</asp:Label><asp:Label ID="lblPageTitle"
                                runat="server">  (Datenselektion)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120">
                            <table id="Table5" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                border="0">
                                <tr>
                                    <td class="TaskTitle">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center">
                                        <asp:LinkButton ID="cmdCreate" runat="server" CssClass="StandardButton">&#149;&nbsp;Suchen</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center">
                                        <asp:LinkButton ID="cmdBack" runat="server" CssClass="StandardButton" Visible="false">&#149;&nbsp;Zurück</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top" <%--width="100%"--%>>
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <table id="TblSearch" cellspacing="0" cellpadding="0" width="100%" border="0" runat="server">
                                <tr id="trSearch" runat="server">
                                    <td align="left">
                                        <table bgcolor="white" border="0">
                                            <tr>
                                                <td valign="top" width="100" style="font-weight:bold;">
                                                    Fahrgestellnummer:
                                                </td>
                                                <td valign="top" width="160">
                                                    <asp:TextBox ID="txtFahrgestellnummer" runat="server" Width="150px"></asp:TextBox>
                                                    <asp:Label ID="lblFahrgestellnummer" runat="server" Visible="false"></asp:Label>
                                                </td>
                                                <td valign="top" width="20">
                                                    &nbsp;
                                                </td>
                                                <td valign="top" width="100" style="font-weight:bold;">
                                                    Fahrzeugbrief:
                                                </td>
                                                <td valign="top" width="160">
                                                    <asp:TextBox ID="txtBrief" runat="server" Width="150px"></asp:TextBox>
                                                    <asp:Label ID="lblBrief" runat="server" Visible="false"></asp:Label>
                                                </td>
                                                <td valign="top" width="20">
                                                    &nbsp;
                                                </td>
                                                <td valign="top" width="100" style="font-weight:bold;">
                                                    Kennzeichen:
                                                </td>
                                                <td valign="top" width="160">
                                                    <asp:TextBox ID="txtKennzeichen" runat="server" Width="100px"></asp:TextBox>
                                                    <asp:Label ID="lblKennzeichen" runat="server" Visible="false"></asp:Label>
                                                </td>                                                
                                            </tr>
                                            <tr>
                                                <td valign="top" width="100%" colspan="9">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table id="TblKundenauswahl" cellpadding="0" width="100%" border="0" runat="server"
                                visible="false">
                                <tr>
                                    <td valign="top" width="250" align="left" style="font-weight:bold;">
                                        <asp:Label runat="server" Text="Kunde:&nbsp;"></asp:Label>
                                    </td>
                                    <td valign="top" width="200" align="left">
                                        <asp:DropDownList ID="ddlKunde" runat="server" visible="false" Width="200">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblKundenname" runat="server" visible="false" Width="200"></asp:Label>
                                    </td>
                                    <td width="95%">
                                        <asp:LinkButton ID="cmdKundenWaehlen" runat="server" CssClass="StandardButton" Visible="false">&#149;&nbsp;wählen</asp:LinkButton>
                                    </td>                                                                        
                                </tr>
                                <tr>
                                    <td colspan="3" width="100%">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <table id="TblFahrzeugdaten" cellspacing="0" cellpadding="0" width="100%" border="0"
                                runat="server" visible="false">
                                <tr>
                                    <td valign="top" width="250px" style="font-weight:bold;">
                                        <asp:Label runat="server" text="Auftrags-Nr.:&nbsp;"></asp:Label>
                                    </td>
                                    <td valign="top" width="250">
                                        <asp:Label ID="lblAuftragsnummer" runat="server"></asp:Label>
                                    </td>
                                    <td width="20">
                                        &nbsp;
                                    </td>
                                    <td valign="top" width="250" style="font-weight:bold;">
                                        Unit-Nr.:
                                    </td>
                                    <td valign="top" width="250">
                                        <asp:Label ID="lblUnitnummer" runat="server"></asp:Label>
                                    </td>
                                    <td width="50%"></td>
                                </tr>
                                <tr>
                                    <td valign="top" width="250" style="font-weight:bold;">
                                        Farbziffer:
                                    </td>
                                    <td valign="top" width="250">
                                        
                                        <asp:Label ID="lblFarbziffer" runat="server"></asp:Label>
                                        
                                    </td>
                                    <td  width="20">
                                        &nbsp;
                                    </td>
                                    <td valign="top" width="250" style="font-weight:bold;">
                                        Halter:
                                    </td>
                                    <td valign="top" width="250">
                                    
                                        <asp:Label ID="lblHaltername1" runat="server"></asp:Label><br />
                                        <asp:Label ID="lblHaltername2" runat="server"></asp:Label>
                                    </td>
                                    <td width="50%"></td>
                                </tr>
                                <tr>
                                    <td valign="top" width="250" style="font-weight:bold;">
                                        Datum Zulassung:
                                    </td>
                                    <td valign="top" width="250">
                                    
                                        <asp:Label ID="lblDatumZulassung" runat="server"></asp:Label>
                                    
                                    </td>
                                    <td width="20">
                                        &nbsp;
                                    </td>
                                    <td valign="top" width="250" style="font-weight:bold;">
                                        Datum Abmeldung:
                                    </td>
                                    <td valign="top" width="250">                                    
                                        <asp:Label ID="lblDatumAbmeldung" runat="server"></asp:Label>                                    
                                    </td>
                                    <td width="50%"></td>
                                </tr>
                                <tr>
                                    <td valign="top" width="250" style="font-weight:bold;">
                                        Datum Carportliste erfasst:
                                    </td>
                                    <td valign="top" width="250">                                    
                                        <asp:Label ID="lblDatumCarportliste" runat="server"></asp:Label>                                    
                                    </td>
                                    <td width="20">
                                        &nbsp;
                                    </td>
                                    <td valign="top" width="250" style="font-weight:bold;">
                                        Datum Schlüssel versendet:
                                    </td>
                                    <td valign="top" width="250">                                    
                                        <asp:Label ID="lblDatumSchluesselVersendet" runat="server"></asp:Label>                                    
                                    </td>
                                    <td width="50%"></td>
                                </tr>
                                <tr>
                                    <td valign="top" width="250" style="font-weight:bold;">
                                        Abmeldung vom LBV zurück:
                                    </td>
                                    <td valign="top" width="250">                                    
                                        <asp:Label ID="lblAbmeldungLBV" runat="server"></asp:Label>                                    
                                    </td>
                                    <td width="20">
                                        &nbsp;
                                    </td>
                                    <td valign="top" width="250" style="font-weight:bold;">
                                        Datum Händlerversand gedruckt:
                                    </td>
                                    <td valign="top" width="250">                                    
                                        <asp:Label ID="lblDatumHaendlerversandGedruckt" runat="server"></asp:Label>                                    
                                    </td>
                                    <td width="50%"></td>
                                </tr>
                                <tr>
                                    <td valign="top" width="250" style="font-weight:bold;">
                                        Datum Brief nach Abmeldung versendet:
                                    </td>
                                    <td valign="top" width="250">                                    
                                        <asp:Label ID="lblDatumBriefVersendet" runat="server"></asp:Label>                                    
                                    </td>
                                    <td width="20">
                                        &nbsp;
                                    </td>
                                    <td valign="top" width="250" style="font-weight:bold;">
                                        Händleradresse:
                                    </td>
                                    <td  valign="top" width="250">
                                        <asp:Label ID="lblHaendlerName1" runat="server"></asp:Label><br />
                                        <asp:Label ID="lblHaendlerName2" runat="server"></asp:Label><br />
                                        <asp:Label ID="lblHaendlerStraße" runat="server"></asp:Label><br />
                                        <asp:Label ID="lblHaendlerPLZ" runat="server"></asp:Label>&nbsp;<asp:Label ID="lblHaendlerOrt" runat="server"></asp:Label><br />
                                        <asp:Label ID="lblHaendlerLand" runat="server"></asp:Label>
                                    </td>
                                    <td width="50%"></td>
                                </tr>
                                <tr>
                                    <td colspan="6" width="100%">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td width="120">
                            &nbsp;
                        </td>
                        <td valign="top" align="left">
                            <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="120">
                            &nbsp;
                        </td>
                        <td valign="top" align="left">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
