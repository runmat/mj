<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report23_2.aspx.vb" Inherits="AppFFE.Report23_2" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../PageElements/Kopfdaten.ascx" %>

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
								<td class="PageNavigation" colSpan="2">
									<asp:label id="lblHead" runat="server"></asp:label>
									<asp:label id="lblPageTitle" runat="server"> (Vertragssuche)</asp:label>
								</td>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdSearch" runat="server" CssClass="StandardButton">&#149;&nbsp;Suchen</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top"><asp:hyperlink id="lnkKreditlimit" runat="server" CssClass="TaskTitle" NavigateUrl="Report23.aspx">Händlersuche</asp:hyperlink>&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD><uc1:kopfdaten id="Kopfdaten1" runat="server"></uc1:kopfdaten></TD>
										</TR>
										<tr>
											<td class="LabelExtraLarge">
												<asp:label id="lblNoData" runat="server"></asp:label></td>
										</tr>
										<tr>
											<td vAlign="top" align="left">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" border="0" bgColor="white">
													<TR id="tr_Ordernummer" runat="server">
														<TD class="TextLarge" width="150">
                                                            <asp:Label ID="lbl_Ordernummer" runat="server" Text="lbl_Ordernummer"></asp:Label>
                                                        </TD>
														<TD class="TextLarge"><asp:textbox id="txtOrdernummer" runat="server" Width="350px" MaxLength="35"></asp:textbox></TD>
													</TR>
													<TR id="tr_Vertragsnummer" runat="server">
														<TD class="StandardTableAlternate" width="150">
                                                            <asp:Label ID="lbl_Vertragsnummer" runat="server" Text="lbl_Vertragsnummer"></asp:Label>
                                                        </TD>
														<TD class="StandardTableAlternate"><asp:textbox id="txtVertragsnummer" runat="server" Width="350px" MaxLength="35"></asp:textbox></TD>
													</TR>
                                                     <TR id="tr_Fahrgestellnr" runat="server">
														<TD class="TextLarge" width="150">
															<asp:Label ID="lbl_Fahrgestellnr" runat="server" 
                                                                Text="Fahrgestellnummer: &lt;br&gt;(die letzten 5 Zeichen)"></asp:Label>
                                                        </TD>
														<TD class="TextLarge">
															<asp:textbox id="txtFahrgestellnummer" runat="server" Width="350px" MaxLength="35"></asp:textbox>&nbsp;(Eingabe von Platzhaltern möglich, z.B. *12345 oder 12345*)</TD>
													</TR>
													<TR id="tr_Vorgaenge" runat="server">
														<TD class="StandardTableAlternate" width="150">Vorgänge:&nbsp;&nbsp;&nbsp;</TD>
														<TD class="StandardTableAlternate">
															<asp:RadioButton id="chkAlle" runat="server" GroupName="grpVorgaenge" Text="alle" Checked="True"></asp:RadioButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
															<asp:RadioButton id="chkAngefordert" runat="server" GroupName="grpVorgaenge" Text="angefordert"></asp:RadioButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
															<asp:RadioButton id="chkNichtAngefordert" runat="server" GroupName="grpVorgaenge" Text="nicht angefordert"></asp:RadioButton></TD>
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
 window.document.Form1.txtOrdernummer.focus();
//-->
		</script>
	</body>
</HTML>
