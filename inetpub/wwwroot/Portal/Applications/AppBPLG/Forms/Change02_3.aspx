<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change02_3.aspx.vb" Inherits="AppBPLG.Change02_3" %>

<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../PageElements/Kopfdaten.ascx" %>
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
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
        <tbody>
            <tr>
                <td colspan="3">
                    <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
                    <asp:ImageButton ID="ImageButton1" runat="server" Width="3px" ImageUrl="/Portal/Images/empty.gif">
                    </asp:ImageButton>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" colspan="3">
                    <table id="Table10" cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tbody>
                            <tr>
                                <td class="PageNavigation" colspan="3">
                                    <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Adressauswahl)</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" width="120">
                                    <table id="Table12" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                        border="0">
                                        <tr>
                                            <td class="TaskTitle">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" width="150">
                                                <asp:LinkButton ID="cmdSearch" runat="server" CssClass="StandardButton">&#149;&nbsp;Suchen</asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" width="150">
                                                <asp:LinkButton ID="cmdSave" runat="server" CssClass="StandardButton">&#149;&nbsp;Weiter</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top">
                                    <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td class="TaskTitle" valign="top">
                                                &nbsp;<asp:HyperLink ID="lnkFahrzeugsuche" runat="server" CssClass="TaskTitle">Fahrzeugsuche</asp:HyperLink>&nbsp;<asp:HyperLink
                                                    ID="lnkFahrzeugAuswahl" runat="server" CssClass="TaskTitle">Fahrzeugauswahl</asp:HyperLink>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tbody>
                                            <tr>
                                                <td valign="top" align="left" colspan="3">
                                                    <uc1:Kopfdaten ID="Kopfdaten1" runat="server"></uc1:Kopfdaten>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" align="left" colspan="3">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" align="left" colspan="3">
                                                    &nbsp;<asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" valign="top" align="left" colspan="3">
                                                    <table id="Table2" cellspacing="0" cellpadding="5" with="100%" bgcolor="white" border="0">
                                                        <tr>
                                                            <td class="StandardTableAlternate">
                                                                <u>Zustellungsart:</u>
                                                            </td>
                                                            <td class="StandardTableAlternate" nowrap>
                                                                <asp:RadioButton ID="rb_VersandStandard" runat="server" Text="rb_VersandStandard"
                                                                    Checked="True" GroupName="Versandart"></asp:RadioButton><br>
                                                                &nbsp;&nbsp;<font color="red">(siehe Hinweis)</font>&nbsp;&nbsp;
                                                            </td>
                                                            <td class="StandardTableAlternate" nowrap>
                                                                <asp:RadioButton ID="rb_0900" runat="server" Text="rb_0900" GroupName="Versandart">
                                                                </asp:RadioButton>&nbsp;&nbsp;<br>
                                                                &nbsp;&nbsp;
                                                                <asp:Label ID="lbl_0900" runat="server"> lbl_0900</asp:Label>&nbsp;&nbsp;
                                                            </td>
                                                            <td class="StandardTableAlternate" nowrap>
                                                                <asp:RadioButton ID="rb_1000" runat="server" Text="rb_1000" GroupName="Versandart">
                                                                </asp:RadioButton>&nbsp;&nbsp;<br>
                                                                &nbsp;&nbsp;<asp:Label ID="lbl_1000" runat="server"> lbl_1000</asp:Label>&nbsp;&nbsp;
                                                            </td>
                                                            <td class="StandardTableAlternate" nowrap>
                                                                <asp:RadioButton ID="rb_1200" runat="server" Text="rb_1200" GroupName="Versandart">
                                                                </asp:RadioButton>&nbsp;&nbsp;<br>
                                                                &nbsp;&nbsp;
                                                                <asp:Label ID="lbl_1200" runat="server"> lbl_1200</asp:Label>&nbsp;&nbsp;
                                                            </td>
                                                            <td class="StandardTableAlternate" nowrap>
                                                                <asp:RadioButton ID="rb_Sendungsverfolgt" runat="server" Text="rb_Sendungsverfolgt"
                                                                    GroupName="Versandart"></asp:RadioButton>
                                                                <br />
                                                                <asp:Label ID="lbl_Sendungsverfolgt" runat="server">lbl_Sendungsverfolgt</asp:Label>
                                                            </td>
                                                            <td class="StandardTableAlternate" width="100%">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" valign="top" align="left" colspan="3">
                                                    <table id="Table2" cellspacing="0" cellpadding="5" width="100%" bgcolor="white" border="0">
                                                        <tbody>
                                                            <tr>
                                                                <td class="StandardTableAlternate" width="450" colspan="3">
                                                                    <em>Achtung </em>:&nbsp;Die Nettopreise verstehen sich pro Sendung.
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="StandardTableAlternate" width="135">
                                                                    <u></u>
                                                                    <asp:RadioButton ID="rb_Zweigstellen" runat="server" Text="<u>Versandadresse:</u>"
                                                                        Checked="True" GroupName="grpVersand" AutoPostBack="True"></asp:RadioButton>
                                                                </td>
                                                                <td class="StandardTableAlternate" nowrap colspan="2">
                                                                    <asp:DropDownList ID="cmbZweigstellen" runat="server">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="StandardTableAlternate" width="135">
                                                                    <asp:RadioButton ID="rb_ZulStelle" runat="server" Text="<u>Zulassungsstelle:</u>"
                                                                        GroupName="grpVersand" AutoPostBack="True"></asp:RadioButton>
                                                                </td>
                                                                <td class="StandardTableAlternate" nowrap colspan="2">
                                                                    <asp:DropDownList ID="ddl_ZulStelle" runat="server" Visible="False">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr id="tr_Adresse" runat="server">
                                                                <td class="StandardTableAlternate" valign="top" width="135">
                                                                    <asp:RadioButton ID="rb_Manuell" runat="server" Text="<u> manuelle Eingabe:</u>"
                                                                        GroupName="grpVersand" AutoPostBack="True"></asp:RadioButton>
                                                                </td>
                                                                <td class="StandardTableAlternate" nowrap colspan="2">
                                                                    <table id="tbl_Adresse" visible="False" runat="server" align="left" border="0" bgcolor="white"
                                                                        width="423" cellpadding="5" cellspacing="0" height="155">
                                                                        <tr>
                                                                            <td class="StandardTableAlternate">
                                                                                <asp:Label ID="Label3" runat="server">Name:</asp:Label>
                                                                            </td>
                                                                            <td class="StandardTableAlternate">
                                                                                <asp:TextBox ID="txt_Name" runat="server" Width="255px"></asp:TextBox>
                                                                            </td>
                                                                            <td class="StandardTableAlternate" width="188">
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="StandardTableAlternate">
                                                                                <asp:Label ID="lbl_Name2" runat="server">lbl_Name2</asp:Label>
                                                                            </td>
                                                                            <td class="StandardTableAlternate">
                                                                                <asp:TextBox ID="txt_Name2" runat="server" Width="255px"></asp:TextBox>
                                                                            </td>
                                                                            <td class="StandardTableAlternate" width="188">
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="StandardTableAlternate">
                                                                                <asp:Label ID="Label2" runat="server">Strasse:</asp:Label>
                                                                            </td>
                                                                            <td class="StandardTableAlternate">
                                                                                <asp:TextBox ID="txt_Strasse" runat="server" Width="254px"></asp:TextBox>
                                                                            </td>
                                                                            <td class="StandardTableAlternate" width="188">
                                                                                <asp:Label ID="lbl_HausNummer" runat="server">Nr.:</asp:Label>&nbsp;
                                                                                <asp:TextBox ID="txt_Nummer" runat="server" Width="45px" MaxLength="10"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="StandardTableAlternate" height="27">
                                                                                <asp:Label ID="lbl_PLZ" runat="server">PLZ:</asp:Label>
                                                                            </td>
                                                                            <td class="StandardTableAlternate" height="27">
                                                                                <asp:TextBox ID="txt_PLZ" runat="server" Width="99px"></asp:TextBox>
                                                                            </td>
                                                                            <td class="StandardTableAlternate" height="27">
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="StandardTableAlternate">
                                                                                <asp:Label ID="Label1" runat="server">Ort:</asp:Label>
                                                                            </td>
                                                                            <td class="StandardTableAlternate">
                                                                                <asp:TextBox ID="txt_Ort" runat="server" Width="255px"></asp:TextBox>
                                                                            </td>
                                                                            <td class="StandardTableAlternate" width="188">
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="StandardTableAlternate" height="27">
                                                                                <asp:Label ID="lbl_Land" runat="server">Land:</asp:Label>
                                                                            </td>
                                                                            <td class="StandardTableAlternate" height="27">
                                                                                <asp:DropDownList ID="ddl_Land" runat="server">
                                                                                </asp:DropDownList>
                                                                                <td class="StandardTableAlternate" height="27">
                                                                                </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="left" valign="top">
                                                    <table class="InfoText" id="Table4" cellspacing="1" cellpadding="1" border="0">
                                                        <tr>
                                                            <td>
                                                                <strong><u>Hinweis:<br />
                                                                </u></strong>Die Deutsche Post AG garantiert für&nbsp;Standardsendungen keine Zustellzeiten<br />
                                                                und gibt die Zustellwahrscheinlichkeit wie folgt an:&nbsp;<br />
                                                                <br />
                                                                &nbsp;&nbsp;&nbsp;-95% aller Sendungen werden dem Empfänger innerhalb von 24 Stunden
                                                                zugestellt,<br />
                                                                &nbsp;&nbsp;&nbsp;-3% aller Sendungen benötigen zwischen 24 und 48 Stunden bis zur
                                                                Zustellung.<br />
                                                                <br />
                                                                Bitte beachten Sie hierzu auch die Beförderungsbedingungen der Deutschen Post AG.
                                                            </td>
                                                        </tr>
                                                    </table>
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
        </tbody>
    </table>
    </form>
</body>
</html>
