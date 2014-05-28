<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change08_3.aspx.vb" Inherits="CKG.Components.ComCommon.Change08_3" %>

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
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" name="Form1" method="post" runat="server">
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
                            <table id="Table12" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                border="0">
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
                                    <td valign="top" align="left" colspan="3">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TextLarge" valign="top" align="left" colspan="3">
                                        <table id="Table2" cellspacing="0" cellpadding="5" width="100%" bgcolor="white" border="0">
                                            <tr id="tr_ZulstSichtbarkeit" runat="server">
                                                <td class="TextLarge">
                                                    <asp:RadioButton ID="rb_Zulst" runat="server" AutoPostBack="True" Text="rb_Zulst"
                                                        GroupName="grpAdresse"></asp:RadioButton>
                                                </td>
                                                <td class="TextLarge" valign="top" align="left" width="100%" colspan="3">
                                                    <asp:DropDownList ID="ddlZulDienst" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr id="trSAPAdresse" runat="server">
                                                <td class="StandardTableAlternate" valign="top" nowrap>
                                                    <asp:RadioButton ID="rb_SAPAdresse" runat="server" AutoPostBack="True" Text="rb_SAPAdresse"
                                                        GroupName="grpAdresse"></asp:RadioButton>
                                                </td>
                                                <td class="StandardTableAlternate" valign="top" width="100%" colspan="3">
                                                    <p align="left">
                                                        <table class="TableSearch" id="tblSuche" cellspacing="1" cellpadding="1" border="0"
                                                            runat="server">
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td colspan="3">
                                                                    <strong>Bitte Vorauswahl durchführen:</strong>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;&nbsp;
                                                                </td>
                                                                <td colspan="3">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    Name:
                                                                </td>
                                                                <td nowrap colspan="2">
                                                                    <asp:TextBox ID="txtSucheName" runat="server"></asp:TextBox>&nbsp;<font color="red"
                                                                        size="2">*</font>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    PLZ:
                                                                </td>
                                                                <td nowrap>
                                                                    <asp:TextBox ID="txtSuchePLZ" runat="server"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:LinkButton ID="btnSucheAdresse" runat="server" CssClass="StandardButtonTable">&#149;&nbsp;Suchen</asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td nowrap colspan="3">
                                                                    <font color="red"><font size="2">* Eingabe erforderlich.<font color="black"> Suche mit
                                                                        Platzhaltern ist nicht möglich.</font></font></font>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td nowrap colspan="3">
                                                                    <asp:Label ID="lblErrorSuche" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label><asp:Label
                                                                        ID="lblInfo" runat="server" CssClass="TextExtraLarge" EnableViewState="False"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </p>
                                                    <asp:DropDownList ID="ddlSAPAdresse" runat="server" Width="100%" Visible="False">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" colspan="4">
                                                </td>
                                            </tr>
                                            <tr id="trVersandAdrEnd0" runat="server">
                                                <td class="TextLarge" colspan="4">
                                                    <asp:RadioButton ID="rb_VersandAdresse" runat="server" AutoPostBack="True" Text="rb_VersandAdresse"
                                                        GroupName="grpAdresse"></asp:RadioButton>
                                                </td>
                                            </tr>
                                            <tr id="trVersandAdrEnd1" runat="server">
                                                <td class="StandardTableAlternate" valign="center" nowrap align="right">
                                                    <asp:Label ID="lbl_Firma" runat="server">lbl_Firma</asp:Label>&nbsp;&nbsp;
                                                </td>
                                                <td class="StandardTableAlternate">
                                                    <asp:TextBox ID="txtName1" TabIndex="2" runat="server" MaxLength="35"></asp:TextBox>
                                                </td>
                                                <td class="StandardTableAlternate" nowrap align="right" width="108">
                                                    <asp:Label ID="lbl_Ansprechpartner" runat="server">lbl_Ansprechpartner</asp:Label>&nbsp;&nbsp;
                                                </td>
                                                <td class="StandardTableAlternate" width="100%">
                                                    <asp:TextBox ID="txtName2" TabIndex="3" runat="server" MaxLength="35"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="trVersandAdrEnd2" runat="server">
                                                <td class="TextLarge" valign="top" align="right">
                                                    <asp:Label ID="lbl_Strasse" runat="server">lbl_Strasse</asp:Label>&nbsp;&nbsp;
                                                </td>
                                                <td class="TextLarge" valign="top">
                                                    <asp:TextBox ID="txtStr" TabIndex="4" runat="server" MaxLength="60"></asp:TextBox>
                                                </td>
                                                <td class="TextLarge" valign="top" nowrap align="right" width="108">
                                                    <asp:Label ID="lbl_Hausnummer" runat="server">lbl_Hausnummer</asp:Label>&nbsp;&nbsp;
                                                </td>
                                                <td class="TextLarge" valign="top" width="100%">
                                                    <asp:TextBox ID="txtNr" TabIndex="5" runat="server" MaxLength="10"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="trVersandAdrEnd3" runat="server">
                                                <td class="StandardTableAlternate" valign="top" align="right">
                                                    <asp:Label ID="lbl_Postleitzahl" runat="server">lbl_Postleitzahl</asp:Label>&nbsp;&nbsp;
                                                </td>
                                                <td class="StandardTableAlternate">
                                                    <asp:TextBox ID="txtPlz" TabIndex="6" runat="server" MaxLength="10"></asp:TextBox>
                                                </td>
                                                <td class="StandardTableAlternate" align="right" width="108">
                                                    <asp:Label ID="lbl_Ort" runat="server">lbl_Ort</asp:Label>&nbsp;&nbsp;
                                                </td>
                                                <td class="StandardTableAlternate" width="100%">
                                                    <asp:TextBox ID="txtOrt" TabIndex="7" runat="server" MaxLength="40"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="tr_Land" runat="server">
                                                <td class="TextLarge" valign="top" align="right">
                                                    <asp:Label ID="lbl_Land" runat="server">lbl_Land</asp:Label>&nbsp;
                                                </td>
                                                <td class="TextLarge">
                                                    <asp:DropDownList ID="ddlLand" TabIndex="10" runat="server" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="TextLarge" width="108">
                                                </td>
                                                <td class="TextLarge" width="100%">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" valign="top" colspan="4">
                                                    &nbsp;&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                            <tr id="trVersandAdrEnd4" runat="server">
                                                <td class="StandardTableAlternate" valign="top">
                                                    <asp:Label ID="lbl_Grund" runat="server">lbl_Grund</asp:Label>
                                                </td>
                                                <td class="StandardTableAlternate" valign="top">
                                                    <asp:DropDownList ID="ddlGrund" TabIndex="10" runat="server" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="StandardTableAlternate" valign="top" width="108">
                                                    <asp:Label ID="lbl_Auf" runat="server">lbl_Auf</asp:Label>
                                                </td>
                                                <td class="StandardTableAlternate" valign="top" width="100%">
                                                    <asp:TextBox ID="txtAuf" TabIndex="11" runat="server" Width="200px" MaxLength="40"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="trVersandAdrEnd9" runat="server">
                                                <td class="StandardTableAlternate" valign="top" align="left">
                                                    <asp:Label ID="lbl_Bemerkung" runat="server">lbl_Bemerkung</asp:Label>
                                                </td>
                                                <td class="StandardTableAlternate" valign="top" colspan="3">
                                                    <asp:TextBox ID="txtBemerkung" TabIndex="12" runat="server" Width="300px" MaxLength="60"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="trVersandAdrEnd5" runat="server">
                                                <td class="TextLarge" valign="top" colspan="4">
                                                    &nbsp;&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                            <tr id="trVersandart1" runat="server">
                                                <td class="StandardTableAlternate" valign="top">
                                                    <asp:Label ID="lbl_Versandart" runat="server">lbl_Versandart</asp:Label>
                                                </td>
                                                <td class="StandardTableAlternate" colspan="6">
                                                    <table id="Table1" cellspacing="0" cellpadding="0" border="0">
                                                        <tr>
                                                            <td class="TextLarge" valign="top" nowrap rowspan="3">
                                                                <asp:Label ID="lbl_Normal" runat="server">lbl_Normal</asp:Label>
                                                            </td>
                                                            <td nowrap>
                                                                <asp:RadioButton ID="rb_VersandStandard" runat="server" GroupName="Versandart" Checked="True"
                                                                    Text="rb_VersandStandard"></asp:RadioButton>
                                                            </td>
                                                            <td nowrap rowspan="3">
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            </td>
                                                            <td class="TextLarge" valign="top" nowrap rowspan="3">
                                                                <asp:Label ID="lbl_Express" runat="server">lbl_Express</asp:Label>
                                                            </td>
                                                            <td nowrap>
                                                                <asp:RadioButton ID="rb_0900" runat="server" GroupName="Versandart" Text="rb_0900">
                                                                </asp:RadioButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td nowrap>
                                                                <asp:RadioButton ID="rb_Sendungsverfolgt" runat="server" GroupName="Versandart" Text="rb_Sendungsverfolgt">
                                                                </asp:RadioButton>
                                                            </td>
                                                            <td nowrap>
                                                                <asp:RadioButton ID="rb_1000" runat="server" GroupName="Versandart" Text="rb_1000">
                                                                </asp:RadioButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td nowrap>
                                                                &nbsp;
                                                            </td>
                                                            <td nowrap>
                                                                <asp:RadioButton ID="rb_1200" runat="server" GroupName="Versandart" Text="rb_1200">
                                                                </asp:RadioButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trVersandart2" runat="server">
                                    <td valign="top" align="left" colspan="3">
                                        *Diese Versandarten sind mit zusätzlichen Kosten verbunden.
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" colspan="3">
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <!--#include File="../../../PageElements/Footer.html" -->
                                        <br>
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
</html>
