<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change205_4.aspx.vb" Inherits="AppARVAL.Change205_4" %>
<%--<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>--%>
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
			<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
				<tr>
					<td colSpan="3"><uc1:header id="ucHeader" runat="server"></uc1:header></td>
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
                                                <asp:HyperLink ID="lnkFahrzeugsuche" runat="server" NavigateUrl="Change04.aspx" CssClass="TaskTitle">Fahrzeugsuche</asp:HyperLink>&nbsp;<asp:HyperLink
                                                    ID="lnkFahrzeugAuswahl" runat="server" NavigateUrl="Change04_2.aspx" CssClass="TaskTitle"
                                                    Visible="False">Fahrzeugauswahl</asp:HyperLink>&nbsp;<asp:HyperLink ID="lnkAdressAuswahl"
                                                        runat="server" NavigateUrl="Change04_3.aspx" CssClass="TaskTitle" Visible="False">Adressauswahl</asp:HyperLink><asp:Label
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
                                                            <asp:Label ID="lblVersandAdresse" runat="server" Font-Underline="True">Versandadresse:</asp:Label>
                                                        </td>
                                                        <td class="TextLarge" valign="top" align="left" colspan="2">
                                                            <asp:Label ID="lblVersand" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr id="trVersandArt" runat="server">
                                                        <td class="" valign="top" nowrap>
                                                            <asp:Label ID="lblVersandartTxt" runat="server" Font-Underline="True">Versandart:</asp:Label>
                                                        </td>
                                                        <td class="TextLarge" valign="top" align="left" colspan="2">
                                                            <asp:Label ID="lblVersandart" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="" valign="top" nowrap>
                                                            <asp:Label ID="lblVersandgrundText" runat="server" Font-Underline="True">Versandgrund:</asp:Label>
                                                        </td>
                                                        <td class="TextLarge" valign="top" align="left" colspan="2">
                                                            <asp:Label ID="lblVersandGrund" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="StandardTableAlternate" valign="top">
                                                            <p>
                                                                Fahrzeuge</p>
                                                            <p>
                                                                &nbsp;</p>
                                                            <p>
                                                                &nbsp;</p>
                                                        </td>
                                                        <td class="StandardTableAlternate" valign="top" align="left" colspan="2">
                                                            <asp:DataGrid ID="DataGrid1" runat="server" headerCSS="tableHeader" bodyCSS="tableBody"
                                                                CssClass="tableMain" bodyHeight="250" Width="100%" AutoGenerateColumns="False"
                                                                AllowSorting="True">
                                                                <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                                <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
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
                                                                    <asp:BoundColumn DataField="STATUS" SortExpression="STATUS" HeaderText="Status">
                                                                    </asp:BoundColumn>
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
                                            <td colspan="3">
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
</HTML>
