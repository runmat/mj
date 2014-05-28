<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change49.aspx.vb" Inherits="CKG.Components.ComCommon.Change49" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
        <table width="100%" align="center">
            <tbody>
                <tr>
                    <td>
                        <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table id="Table0" cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tbody>
                                <tr>
                                    <td class="PageNavigation" nowrap colspan="2">
                                        <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> - Fahrzeugsuche</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TaskTitle" nowrap colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="121"
                                            border="0">
                                            <tr>
                                                <td class="" width="57">
                                                    <asp:LinkButton ID="cmdSearch" runat="server" CssClass="StandardButton"> &#149;&nbsp;Suchen</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top">
                                        <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tbody>
                                                <tr>
                                                    <td valign="top" align="left">
                                                        <table id="Table1" cellspacing="0" cellpadding="5" width="100%" border="0" bgcolor="white">
                                                            <tbody>
                                                                <tr id="tr_Kennzeichen">
                                                                    <td class="TextLarge" width="150">
                                                                        <asp:Label ID="lbl_Kennzeichen" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;
                                                                    </td>
                                                                    <td class="TextLarge">
                                                                        <asp:TextBox ID="txtKennzeichen" runat="server" Width="200px" MaxLength="35"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr id="tr_Kontonummer">
                                                                    <td class="StandardTableAlternate" width="150">
                                                                        <asp:Label ID="lbl_Kontonummer" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;
                                                                    </td>
                                                                    <td class="StandardTableAlternate">
                                                                        <asp:TextBox ID="txtVertragsnummer" runat="server" Width="200px" MaxLength="35"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr id="tr_Fahrgestellnummer">
                                                                    <td class="TextLarge" width="150">
                                                                        <asp:Label ID="lbl_Fahrgestellnummer" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td class="TextLarge">
                                                                        <asp:TextBox ID="txtFahrgestellnummer" runat="server" Width="200px" MaxLength="35"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr id="tr_zb2Nummer">
                                                                    <td class="StandardTableAlternate" width="150">
                                                                        <asp:Label ID="lbl_zb2Nummer" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;
                                                                    </td>
                                                                    <td class="StandardTableAlternate">
                                                                        <asp:TextBox ID="txtZB2Nummer" runat="server" Width="200px" MaxLength="35"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                        <br>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" width="120">
                                        &nbsp;
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" width="120">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <!--#include File="../../../PageElements/Footer.html" -->
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
        </form>
		<script language="JavaScript">
<!-- //
 window.document.Form1.txtVertragsnummer.focus();
//-->
		</script>
        </TR></TBODY></TABLE></TR></TBODY></TABLE></TR></TBODY></TABLE></TR></TBODY></TABLE></FORM>
	</body>
</HTML>
