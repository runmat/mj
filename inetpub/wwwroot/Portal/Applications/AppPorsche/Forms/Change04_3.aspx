<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change04_3.aspx.vb" Inherits="AppPorsche.Change04_3" %>

<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema" />
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body>
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
                <table id="Table10" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="3">
                            <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Adressauswahl)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120">
                            <table id="Table12" style="border-color: #FFFFFF" cellspacing="0" cellpadding="0"
                                width="120" border="0">
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
                                        &nbsp;<asp:HyperLink ID="lnkFahrzeugsuche" runat="server" CssClass="TaskTitle" NavigateUrl="Change04.aspx">Fahrzeugsuche</asp:HyperLink>&nbsp;<asp:HyperLink
                                            ID="lnkFahrzeugAuswahl" runat="server" CssClass="TaskTitle" NavigateUrl="Change04_2.aspx">Fahrzeugauswahl</asp:HyperLink>
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
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
                                        <table id="Table2" cellspacing="0" cellpadding="5" width="100%" bgcolor="white" border="0">
                                            <tr>
                                                <td class="StandardTableAlternate">
                                                    <u>Zustellungsart:</u><br />
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td class="StandardTableAlternate" nowrap="nowrap" width="100px">
                                                    <asp:RadioButton ID="chkVersandStandard" runat="server" GroupName="Versandart" Checked="True"
                                                        Text="Standard"></asp:RadioButton><br />
                                                    &nbsp;<font color="red">(siehe Hinweis)</font>
                                                </td>
                                                <td class="StandardTableAlternate" width="200px" nowrap="nowrap">
                                                    <asp:RadioButton ID="rb_0900" runat="server" GroupName="Versandart" Text="rb_0900">
                                                    </asp:RadioButton><br />
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbl_0900" runat="server" Text="Label">lbl_0900</asp:Label>
                                                </td>
                                                <td class="StandardTableAlternate" width="200px" id="idPreis1" nowrap="nowrap" runat="server">
                                                    <asp:RadioButton ID="rb_1000" runat="server" GroupName="Versandart" Text="rb_1000">
                                                    </asp:RadioButton><br />
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbl_1000" runat="server" Text="Label">lbl_1000</asp:Label>
                                                </td>
                                                <td class="StandardTableAlternate" id="idPreis2" nowrap="nowrap" colspan="1" rowspan="1"
                                                    runat="server" width="200px">
                                                    <asp:RadioButton ID="rb_1200" runat="server" GroupName="Versandart" Text="rb_1200">
                                                    </asp:RadioButton><br />
                                                   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbl_1200" runat="server" Text="lbl_1200"></asp:Label>
                                                </td>
                                                <td width="100%" class="StandardTableAlternate">
                                                    <asp:RadioButton ID="rb_SendungsVerfolgt" runat="server" Text="rb_SendungsVerfolgt"
                                                        GroupName="Versandart"></asp:RadioButton>
                                                    <br />
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbl_SendungsVerfolgt" runat="server">lbl_SendungsVerfolgt</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="StandardTableAlternate" colspan="6">
                                                    <em>Achtung </em>:&nbsp;Die Nettopreise verstehen sich pro Sendung.
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="StandardTableAlternate">
                                                    <u>Versandadresse</u>:
                                                </td>
                                                <td class="StandardTableAlternate" nowrap="nowrap" colspan="3">
                                                    <asp:DropDownList ID="cmbZweigstellen" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="StandardTableAlternate" nowrap="nowrap">
                                                </td>
                                                <td class="StandardTableAlternate" nowrap="nowrap">
                                                    <asp:RadioButton ID="chkZweigstellen" runat="server" GroupName="grpVersand" Checked="True"
                                                        Text="<u>Versandadresse:</u>" Visible="False"></asp:RadioButton>
                                                </td>
                                            </tr>
                                        </table>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" colspan="3">
                                        <table class="InfoText" id="Table4" cellspacing="1" cellpadding="1" border="0">
                                            <tr>
                                                <td>
                                                    <strong><u>Hinweis:<br />
                                                    </u></strong>Die Deutsche Post AG garantiert für&nbsp;Standardsendungen keine Zustellzeiten<br />
                                                    und gibt die Zustellwahrscheinlichkeit wie folgt an:&nbsp;
                                                    <br />
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
                                <tr>
                                    <td valign="top" align="left" colspan="3">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <!--#include File="../../../PageElements/Footer.html" -->
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

    <script language="JavaScript" type="text/javascript">
			<!--        //
        window.document.Form1.elements[window.document.Form1.length - 2].focus();
			//-->
    </script>

</body>
</html>
