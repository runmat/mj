<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report24.aspx.vb" Inherits="AppFFE.Report24" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../PageElements/Kopfdatenhaendler.ascx" %>
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
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Vertragssuche)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150"><asp:linkbutton id="cmdSearch" runat="server" CssClass="StandardButton">Suchen</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top"><asp:hyperlink id="lnkKreditlimit" runat="server" CssClass="TaskTitle" NavigateUrl="Report23.aspx" Visible="False">Händlersuche</asp:hyperlink>&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD><uc1:kopfdaten id="Kopfdaten1" runat="server"></uc1:kopfdaten></TD>
										</TR>
										<tr>
											<td class="LabelExtraLarge">&nbsp;
												<asp:label id="lblNoData" runat="server"></asp:label></td>
										</tr>
										<tr>
											<td vAlign="top" align="left">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
													<TR id="tr_Vertragsnummer" runat="server">
														<TD class="TextLarge" width="150"><asp:label id="lbl_Vertragsnr" runat="server">lbl_Vertragsnr</asp:label>&nbsp;</TD>
														<TD class="TextLarge"><asp:textbox id="txtVertragsNr" runat="server" Width="250px" MaxLength="11"></asp:textbox></TD>
													</TR>
													<TR id="tr_Ordernummer" runat="server">
														<TD class="StandardTableAlternate" width="150" height="15"><asp:label id="lbl_Order" runat="server">lbl_Order</asp:label>&nbsp;&nbsp;</TD>
														<TD class="StandardTableAlternate" height="15"><asp:textbox id="txtOrderNr" runat="server" Width="250px" MaxLength="35"></asp:textbox></TD>
													</TR>
													<TR id="tr_Briefnummer" runat="server">
														<TD class="StandardTableAlternate" width="150" height="15"><asp:label id="lbl_Briefnummer" runat="server">lbl_Briefnummer</asp:label>&nbsp;&nbsp;</TD>
														<TD class="StandardTableAlternate" height="15"><asp:textbox id="txtBriefnummer" runat="server" Width="250px" MaxLength="35"></asp:textbox></TD>
													</TR>
													<TR id="tr_Fahrgestellnummer" runat="server">
														<TD class="TextLarge" width="150" height="36"><asp:label id="lbl_Fahrgestellnummer" runat="server">lbl_Fahrgestellnummer</asp:label></TD>
														<TD class="TextLarge" height="36"><asp:textbox id="txtFahrgestellnummer" runat="server" Width="250px" MaxLength="35"></asp:textbox>(Eingabe von Platzhaltern möglich, z.B. *12345 oder 12345*)</TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" width="150">Vorgänge:&nbsp;&nbsp;&nbsp;</TD>
														<TD class="StandardTableAlternate"><asp:radiobutton id="chkAlle" runat="server" GroupName="grpVorgaenge" Text="alle" Checked="True"></asp:radiobutton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
															<asp:radiobutton id="chkAngefordert" runat="server" GroupName="grpVorgaenge" Text="angefordert"></asp:radiobutton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
															<asp:radiobutton id="chkNichtAngefordert" runat="server" GroupName="grpVorgaenge" Text="nicht angefordert"></asp:radiobutton></TD>
													</TR>
												</TABLE>
												<br>
											</td>
										</tr>
									</TABLE>
								</td>
							</tr>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD vAlign="top"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
		<script language="JavaScript">
<!-- //
 window.document.Form1.txtOrderNr.focus();
//-->
		</script>
		&nbsp;
	</body>
</HTML>
