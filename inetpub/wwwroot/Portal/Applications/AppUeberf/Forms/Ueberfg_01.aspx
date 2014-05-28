<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Ueberfg_01.aspx.vb" Inherits="AppUeberf.Ueberfg_01" %>
<%@ Register TagPrefix="uc1" TagName="AddressSearchInputControl" Src="../Controls/AddressSearchInputControl.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Ueberfg_01</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</HEAD>
<body leftmargin="0" topmargin="0">
    <form id="Form1" method="post" runat="server">
    <table id="Table4" width="100%" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td class="PageNavigation" colspan="2">
                <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;(
                <asp:Label ID="lblPageTitle" runat="server">Adressdaten</asp:Label>)
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table id="Table5" style="width: 1032px; height: 27px" cellspacing="1" cellpadding="1"
                    width="1032" border="0">
                    <tr>
                        <td style="width: 431px">
                        </td>
                        <td style="width: 360px">
                            <strong>Schritt&nbsp;2 von 4</strong>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table1" cellspacing="1" cellpadding="1" width="100%" border="0">
                    <tr>
                        <td style="width: 113px">
                        </td>
                        <td style="width: 449px">
                            <asp:Label ID="lblKundeName1" runat="server" Font-Italic="True" Font-Bold="True"
                                Width="312px"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblKundeStrasse" runat="server" Font-Italic="True" Font-Bold="True"
                                Width="272px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="120">
                        </td>
                        <td style="width: 449px">
                            <asp:Label ID="lblKundeAnsprechpartner" runat="server" Font-Italic="True" Font-Bold="True"
                                Width="307px"></asp:Label>
                        </td>
                        <td>
                            <p>
                                <asp:Label ID="lblKundePlzOrt" runat="server" Font-Italic="True" Font-Bold="True"
                                    Width="134px"></asp:Label></p>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 113px">
                            &nbsp;
                        </td>
                        <td style="width: 449px">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 113px">
                        </td>
                        <td style="width: 449px">
                            <strong>
                                <asp:Label ID="Label1" runat="server" Width="122px" Height="17px">Abholung</asp:Label></strong>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 113px">
                        </td>
                        <td style="width: 449px">
                            <strong>
                                <uc1:AddressSearchInputControl ID="ctrlAddressSearchAbholung" runat="server"></uc1:AddressSearchInputControl>
                            </strong>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
                <table id="Table2" cellspacing="1" cellpadding="1" width="100%" border="0">
                    <tr>
                        <td width="121" style="width: 121px">
                        </td>
                        <td width="120">
                            Auswahl
                        </td>
                        <td style="width: 255px">
                            <asp:DropDownList ID="drpAbholung" runat="server" Width="217px" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 68px">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td width="121" style="width: 121px">
                        </td>
                        <td width="120">
                            Firma / Name*
                        </td>
                        <td style="width: 255px">
                            <asp:TextBox ID="txtAbName" runat="server" Width="220px" MaxLength="35"></asp:TextBox>
                        </td>
                        <td style="width: 68px">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 121px">
                        </td>
                        <td style="width: 108px">
                            Strasse*
                        </td>
                        <td style="width: 255px">
                            <asp:TextBox ID="txtAbStrasse" runat="server" Width="218px"></asp:TextBox>
                        </td>
                        <td style="width: 68px">
                            Nr.*
                        </td>
                        <td>
                            <asp:TextBox ID="txtAbNr" runat="server" Width="73px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 121px">
                        </td>
                        <td style="width: 108px">
                            PLZ*
                        </td>
                        <td style="width: 255px">
                            <asp:TextBox ID="txtAbPLZ" runat="server" Width="88px" MaxLength="5"></asp:TextBox>
                        </td>
                        <td style="width: 68px">
                            Ort*
                        </td>
                        <td>
                            <asp:TextBox ID="txtAbOrt" runat="server" Width="299px" MaxLength="35"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 121px">
                        </td>
                        <td style="width: 108px">
                            Ansprechpartner*
                        </td>
                        <td style="width: 255px">
                            <asp:TextBox ID="txtAbAnsprechpartner" runat="server" Width="223px" MaxLength="35"></asp:TextBox>
                        </td>
                        <td style="width: 68px">
                            1. Tel.:*
                        </td>
                        <td>
                            <asp:TextBox ID="txtAbTelefon" runat="server" Width="299px" MaxLength="16"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 121px">
                        </td>
                        <td style="width: 108px">
                            &nbsp;Fax
                        </td>
                        <td style="width: 255px">
                            <asp:TextBox ID="txtAbFax" runat="server" Width="223px" MaxLength="16"></asp:TextBox>
                        </td>
                        <td style="width: 68px">
                            2. Tel.:
                        </td>
                        <td>
                            <asp:TextBox ID="txtAbTelefon2" runat="server" Width="299px" MaxLength="16"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 121px">
                        </td>
                        <td style="width: 108px">
                            &nbsp;
                        </td>
                        <td style="width: 255px">
                        </td>
                        <td style="width: 68px">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 121px">
                        </td>
                        <td style="width: 108px">
                            <strong>Anlieferung</strong>
                        </td>
                        <td style="width: 255px">
                        </td>
                        <td style="width: 68px">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td width="121" style="width: 121px">
                        </td>
                        <td colspan="3">
                            <uc1:AddressSearchInputControl ID="ctrlAddressSearchAnlieferung" runat="server">
                            </uc1:AddressSearchInputControl>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 121px">
                        </td>
                        <td style="width: 108px">
                            Auswahl
                        </td>
                        <td style="width: 255px">
                            <asp:DropDownList ID="drpAnlieferung" runat="server" Width="220px" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 68px">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 121px">
                        </td>
                        <td style="width: 108px">
                            Firma / Name*
                        </td>
                        <td style="width: 255px">
                            <asp:TextBox ID="txtAnName" runat="server" Width="219px" MaxLength="35"></asp:TextBox>
                        </td>
                        <td style="width: 68px">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 121px">
                        </td>
                        <td style="width: 108px">
                            Strasse*
                        </td>
                        <td style="width: 255px">
                            <asp:TextBox ID="txtAnStrasse" runat="server" Width="214px" MaxLength="35"></asp:TextBox>
                        </td>
                        <td style="width: 68px">
                            Nr.*
                        </td>
                        <td>
                            <asp:TextBox ID="txtAnNr" runat="server" Width="73px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 121px">
                        </td>
                        <td style="width: 108px">
                            PLZ*
                        </td>
                        <td style="width: 255px">
                            <asp:TextBox ID="txtAnPLZ" runat="server" Width="88px" MaxLength="5"></asp:TextBox>
                        </td>
                        <td style="width: 68px">
                            Ort*
                        </td>
                        <td>
                            <asp:TextBox ID="txtAnOrt" runat="server" Width="299px" MaxLength="35"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 121px">
                        </td>
                        <td style="width: 108px">
                            Ansprechpartner*
                        </td>
                        <td style="width: 255px">
                            <asp:TextBox ID="txtAnAnsprechpartner" runat="server" Width="220px" MaxLength="35"></asp:TextBox>
                        </td>
                        <td style="width: 68px">
                            1. Tel.:*
                        </td>
                        <td>
                            <asp:TextBox ID="txtAnTelefon" runat="server" Width="299px" MaxLength="16"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 121px">
                        </td>
                        <td style="width: 108px">
                            &nbsp;Fax
                        </td>
                        <td style="width: 255px">
                            <asp:TextBox ID="txtAnFax" runat="server" Width="220px" MaxLength="16"></asp:TextBox>
                        </td>
                        <td style="width: 68px">
                            2. Tel.:
                        </td>
                        <td>
                            <asp:TextBox ID="txtAnTelefon2" runat="server" Width="299px" MaxLength="16"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 121px">
                        </td>
                        <td style="width: 108px">
                            &nbsp;
                        </td>
                        <td style="width: 255px">
                        </td>
                        <td style="width: 68px">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 121px">
                        </td>
                        <td style="width: 108px">
                        </td>
                        <td style="width: 255px">
                            <p align="right">
                                <asp:ImageButton ID="cmdBack" runat="server" Width="73px" Height="34px" ImageUrl="/Portal/Images/arrowUeberfLeft.gif">
                                </asp:ImageButton></p>
                        </td>
                        <td style="width: 68px">
                        </td>
                        <td>
                            <asp:ImageButton ID="cmdRight1" runat="server" Width="73px" Height="34px" ImageUrl="/Portal/Images/arrowUeberfRight.gif">
                            </asp:ImageButton>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 121px">
                        </td>
                        <td style="width: 108px">
                            &nbsp;
                        </td>
                        <td style="width: 255px">
                        </td>
                        <td style="width: 68px">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 121px">
                        </td>
                        <td style="width: 108px">
                        </td>
                        <td style="width: 255px">
                        </td>
                        <td style="width: 68px">
                        </td>
                        <td>
                            <strong><font color="red">* = Pflichtfeld</font></strong>
                        </td>
                    </tr>
                </table>
                <table id="Table3" cellspacing="1" cellpadding="1" width="100%" border="0">
                    <tr>
                        <td style="width: 119px" align="center">
                        </td>
                        <td align="center">
                            <p align="left">
                                <asp:Label ID="lblError" runat="server" Width="325px" Height="19px" EnableViewState="False"
                                    CssClass="TextError"></asp:Label></p>
                        </td>
                    </tr>
                </table>
                <p>
                    &nbsp;</p>
            </td>
        </tr>
    </table>
    </form>
</body>
</HTML>
