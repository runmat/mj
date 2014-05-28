<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01_3.aspx.vb" Inherits="AppF1.Change01_3" %>

<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../controls/Kopfdaten.ascx" %>
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
    <style type="text/css">
        .style1
        {
            color: #CC0000;
        }
        .style2
        {
            height: 25px;
        }
    </style>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
        <tr>
            <td colspan="3">
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="/Portal/Images/empty.gif"
                    Width="3px"></asp:ImageButton>
            </td>
        </tr>
        <tr>
            <td valign="top" align="left" colspan="3">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="3">
                            <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Adressauswahl)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120">
                            <table bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                border="0">
                                <tr>
                                    <td class="TaskTitle">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" width="150">
                                        <asp:LinkButton ID="cmdSave" runat="server" CssClass="StandardButton">&#149;&nbsp;Weiter</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" width="150">
                                        <asp:LinkButton ID="cmdSearch" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Suchen</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        &nbsp;<asp:HyperLink ID="lnkFahrzeugsuche" runat="server" CssClass="TaskTitle" NavigateUrl="Change01.aspx">Fahrzeugsuche</asp:HyperLink>&nbsp;<asp:HyperLink
                                            ID="lnkFahrzeugAuswahl" runat="server" CssClass="TaskTitle" NavigateUrl="Change01_2.aspx">Fahrzeugauswahl</asp:HyperLink>
                                    </td>
                                </tr>
                            </table>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td valign="top" align="left" colspan="2">
                                        <uc1:Kopfdaten ID="Kopfdaten1" runat="server"></uc1:Kopfdaten>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TextLarge" valign="top" align="left" style="width: 750px; border-color: #a9a9a9; border-style:solid; border-width: 1px">
                                        <table cellspacing="0" cellpadding="5" width="100%" bgcolor="white" border="0">
                                            <tr>
                                                <td colspan="3" class="StandardTableAlternate">
                                                    <asp:Label runat="server" Font-Bold="True" Text="Zustellart wählen:"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 250px; background-color: #c8c8c8" runat="server" id="tdVersandartStandard">
                                                    <asp:RadioButton runat="server" ID="rb_Versandart_Standard" GroupName="Versandart" AutoPostBack="True" Checked="True"/>
                                                </td>
                                                <td rowspan="4" style="padding-left: 20px; border-left-style: solid; border-left-color: #d3d3d3; border-left-width: 1px">
                                                    <div runat="server" id="divVersandartStandard"> 
                                                    </div>
                                                    <div runat="server" id="divVersandartUPS" Visible="False">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton runat="server" ID="rb_UPS_Sendungsverfolgt" GroupName="VersandoptionenUPS" Checked="True"/>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <div runat="server" id="divVersandartTNT" Visible="False">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton runat="server" ID="rb_TNT_Express_0900" GroupName="VersandoptionenTNT" Checked="True"/>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton runat="server" ID="rb_TNT_Express_1000" GroupName="VersandoptionenTNT"/>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton runat="server" ID="rb_TNT_Express_1200" GroupName="VersandoptionenTNT"/>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <div runat="server" id="divVersandartDHL" Visible="False">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton runat="server" ID="rb_DHL_Express_0900" GroupName="VersandoptionenDHL" Checked="True"/>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton runat="server" ID="rb_DHL_Express_1000" GroupName="VersandoptionenDHL"/>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton runat="server" ID="rb_DHL_Express_1200" GroupName="VersandoptionenDHL"/>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <div style="margin-top: 25px">
                                                        <asp:Label runat="server" Text="Achtung: Auslieferungen erfolgen täglich bei Beauftragung vor 15:30 Uhr." Font-Bold="True"></asp:Label>
                                                    </div>
                                                </td>
                                                <td rowspan="4" style="width: 30px; vertical-align: top">
                                                    <asp:Image runat="server" ID="imgInfoVersandartStandard" ImageUrl="/Portal/Images/info_icon.png" 
                                                        ToolTip="Hinweis:&#xa;&#xa;Die Deutsche Post AG garantiert für diese Sendungen keine Lauf- und Zustellungszeiten!&#xa;&#xa;Bitte beachten Sie hierzu auch die Beförderungsbedingungen der Deutschen Post AG."/>
                                                    <asp:Image runat="server" ID="imgInfoVersandartUPS" ImageUrl="/Portal/Images/info_icon.png" Visible="False" 
                                                        ToolTip="Hinweis:&#xa;&#xa;Rechnungsstellung erfolgt monatlich direkt an den Anforderer durch den DAD.&#xa;&#xa;Alle Kosten verstehen sich netto zzgl. Mwst. Regellaufzeit 1-2 Werktage. Keine Samstagszustellung.&#xa;&#xa;Der sendungsverfolgte Versand erfolgt auf Kosten des Anforderers.&#xa;&#xa;Bitte beachten Sie hierzu die Beförderungsbedingungen des Dienstleisters."/>
                                                    <asp:Image runat="server" ID="imgInfoVersandartTNT" ImageUrl="/Portal/Images/info_icon.png" Visible="False" 
                                                        ToolTip="Hinweis:&#xa;&#xa;Rechnungsstellung erfolgt direkt an den Anforderer durch TNT.&#xa;&#xa;Alle Kosten verstehen sich netto zzgl. Mwst. (nur Auslieferungen Montag-Freitag).&#xa;&#xa;Der Expressversand erfolgt auf Kosten des Anforderers.&#xa;&#xa;Bitte beachten Sie hierzu die Beförderungsbedingungen des Dienstleisters."/>
                                                    <asp:Image runat="server" ID="imgInfoVersandartDHL" ImageUrl="/Portal/Images/info_icon.png" Visible="False" 
                                                        ToolTip="Hinweis:&#xa;&#xa;Rechnungsstellung erfolgt monatlich direkt an den Anforderer durch den DAD.&#xa;&#xa;Alle Kosten verstehen sich netto zzgl. Mwst. (auch Samstags-Auslieferungen).&#xa;&#xa;Der Expressversand erfolgt auf Kosten des Anforderers.&#xa;&#xa;Bitte beachten Sie hierzu die Beförderungsbedingungen des Dienstleisters."/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 250px" runat="server" id="tdVersandartUPS">
                                                    <asp:RadioButton runat="server" ID="rb_Versandart_UPS" GroupName="Versandart" AutoPostBack="True" style="vertical-align:20px"/>
                                                    <asp:Image runat="server" ImageUrl="/Portal/Images/UPS_Logo.png" Height="50px" style="margin-left: 5px"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 250px" runat="server" id="tdVersandartTNT">
                                                    <asp:RadioButton runat="server" ID="rb_Versandart_TNT" GroupName="Versandart" AutoPostBack="True" style="vertical-align:20px"/>
                                                    <asp:Image runat="server" ImageUrl="/Portal/Images/TNT.png" Height="50px" style="margin-left: 50px"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 250px" runat="server" id="tdVersandartDHL">
                                                    <asp:RadioButton runat="server" ID="rb_Versandart_DHL" GroupName="Versandart" AutoPostBack="True" style="vertical-align:20px"/>
                                                    <asp:Image runat="server" ImageUrl="/Portal/Images/DHL.png" Height="50px" style="margin-left: 35px"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" style="width: 750px; border-color: #a9a9a9; border-style:solid; border-width: 1px">
                                        <table cellspacing="0" cellpadding="5" width="100%" bgcolor="white" border="0">
                                            <tr>
                                                <td class="StandardTableAlternate">
                                                    <asp:Label runat="server" Font-Bold="True" Text="Versandadresse wählen:"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:RadioButton runat="server" ID="rb_Versandadresse_Hinterlegt" GroupName="Versandadresse" AutoPostBack="True"/>
                                                    &nbsp;
                                                    <asp:RadioButton runat="server" ID="rb_Versandadresse_Zulassungsstellen" GroupName="Versandadresse" AutoPostBack="True"/>
                                                    &nbsp;
                                                    <asp:RadioButton runat="server" ID="rb_Versandadresse_Manuell" GroupName="Versandadresse" AutoPostBack="True"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div runat="server" id="divZweigstellen" Visible="False">
                                                        <asp:DropDownList ID="ddlZweigstellen" runat="server"></asp:DropDownList>
                                                    </div>
                                                    <div runat="server" id="divZulassungsstellen" Visible="False">
                                                        <asp:DropDownList ID="ddlZulassungsstellen" runat="server" ></asp:DropDownList>
                                                    </div>
                                                    <div runat="server" id="divManuelleEingabe" Visible="False">
                                                        <table runat="server" cellspacing="0"
                                                            cellpadding="0" align="left" bgcolor="white" border="0">
                                                            <tr>
                                                                <td class="StandardTableAlternate" style="width: 200px">
                                                                    <asp:Label ID="lbl_Name" runat="server"></asp:Label>
                                                                </td>
                                                                <td colspan="2" class="StandardTableAlternate" style="width: 275px">
                                                                    <asp:TextBox ID="txt_Name" runat="server" Width="255px" MaxLength="40"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="StandardTableAlternate" style="width: 200px">
                                                                    <asp:Label ID="lbl_Name2" runat="server"></asp:Label>
                                                                </td>
                                                                <td colspan="2" class="StandardTableAlternate">
                                                                    <asp:TextBox ID="txt_Name2" runat="server" Width="255px" MaxLength="40"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="StandardTableAlternate" style="width: 200px">
                                                                    <asp:Label ID="lbl_Strasse" runat="server"></asp:Label>
                                                                </td>
                                                                <td class="StandardTableAlternate" style="width: 275px">
                                                                    <asp:TextBox ID="txt_Strasse" runat="server" Width="255px" MaxLength="60"></asp:TextBox>
                                                                </td>
                                                                <td class="StandardTableAlternate">
                                                                    &nbsp;&nbsp;<asp:Label ID="lbl_Nummer" runat="server"></asp:Label>
                                                                    <asp:TextBox ID="txt_Nummer" runat="server" Width="45px" MaxLength="10"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="StandardTableAlternate" style="width: 200px">
                                                                    <asp:Label ID="lbl_PLZ" runat="server"></asp:Label>&nbsp;
                                                                </td>
                                                                <td colspan="2" class="StandardTableAlternate">
                                                                    <asp:TextBox ID="txt_PLZ" runat="server" Width="99px" MaxLength="10"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="StandardTableAlternate" style="width: 200px">
                                                                    <asp:Label ID="lbl_Ort" runat="server">Ort:</asp:Label>
                                                                </td>
                                                                <td colspan="2" class="StandardTableAlternate">
                                                                    <asp:TextBox ID="txt_Ort" runat="server" Width="255px" MaxLength="40"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="StandardTableAlternate" style="width: 200px">
                                                                    <asp:Label ID="lbl_Land" runat="server">Land:</asp:Label>
                                                                </td>
                                                                <td colspan="2" class="StandardTableAlternate">
                                                                    <asp:DropDownList ID="ddl_Land" runat="server">
                                                                        <asp:ListItem Value="0" Selected="True">DE</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr id="ZeigeTEXT50" runat="server">
                                                <td valign="top">
                                                    Kunde für Anforderungen mit erweitertem
                                                    <br/>
                                                    Zahlungsziel (Delayed Payment) endgültig
                                                </td>
                                                <td class="StandardTableAlternate" valign="top" align="left">
                                                    <asp:TextBox ID="txtTEXT50" runat="server" MaxLength="50"></asp:TextBox>&nbsp;&nbsp;
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Eingabe erforderlich"
                                                        ControlToValidate="txtTEXT50"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" valign="top" align="left">
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <!--#include File="../../../PageElements/Footer.html" -->
                                        <br/>
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
