<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change06.aspx.vb" Inherits="AppFFE.Change06"%>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../PageElements/Kopfdaten.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Suche" Src="../../../PageElements/Suche.ascx" %>
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
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;
									<asp:label id="lblPageTitle" runat="server"> (Fahrzeugsuche)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top" width="100">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="120"><asp:linkbutton id="cmdSearch" runat="server" CssClass="StandardButton">&#149;&nbsp;Suchen</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="120"><asp:linkbutton id="btnSave" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Speichern</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="120"><asp:linkbutton id="Linkbutton1" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Löschen</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="120"><asp:linkbutton id="btnBack" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Zurück</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td vAlign="top" align="left">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" border="0">
													<TR id="tr_Vertragsnummer" runat="server">
														<TD class="TextLarge" width="150">
															<asp:label id="lbl_Vertragsnr" runat="server">lbl_Vertragsnr</asp:label>&nbsp;</TD>
														<TD class="TextLarge"><asp:textbox id="txtVertragsNr" runat="server" MaxLength="11"></asp:textbox></TD>
														<TD class="TextLarge" width="100%"><asp:textbox id="txtHaendlernr" runat="server" BackColor="Transparent" BorderWidth="0px" Enabled="False"></asp:textbox></TD>
													</TR>
													<TR id="tr_FahrgestellNr" runat="server">
														<TD class="TextLarge" width="150"><asp:label id="lbl_FahrgestellNr" runat="server">lbl_FahrgestellNr</asp:label></TD>
														<TD class="TextLarge"><asp:textbox id="txtFahrgNr" runat="server" MaxLength="17"></asp:textbox></TD>
														<TD class="TextLarge" width="100%"></TD>
													</TR>
													<TR id="tr_OrderNr" runat="server">
														<TD class="TextLarge" width="150"><asp:label id="lbl_OrderNr" runat="server" Enabled="False">lbl_OrderNr</asp:label></TD>
														<TD class="TextLarge"><asp:textbox id="txtOrderNr" runat="server" MaxLength="11" Enabled="False"></asp:textbox></TD>
														<TD class="TextLarge" width="100%"></TD>
													</TR>
													<TR id="tr_BriefNr" runat="server">
														<TD class="TextLarge" width="150"><asp:label id="lbl_BriefNr" runat="server">lbl_BriefNr</asp:label></TD>
														<TD class="TextLarge"><asp:textbox id="txtBriefNr" runat="server" MaxLength="8"></asp:textbox></TD>
														<TD class="TextLarge" width="100%"></TD>
													</TR>
													<TR id="tr_Versand" runat="server">
														<TD class="TextLarge" width="150"><asp:label id="lbl_VersandDatum" runat="server" Enabled="False">lbl_VersandDatum:</asp:label></TD>
														<TD class="TextLarge"><asp:textbox id="txtVersandDatum" runat="server" MaxLength="11" Enabled="False"></asp:textbox></TD>
														<TD class="TextLarge" width="100%"></TD>
													</TR>
													<TR id="tr_Bezahlt" runat="server">
														<TD class="TextLarge" width="150"><asp:label id="lbl_Bezahlt" runat="server" Enabled="False">lbl_Bezahlt</asp:label></TD>
														<TD class="TextLarge"><asp:checkbox id="cbxBezahlt" runat="server" Enabled="False"></asp:checkbox></TD>
														<TD class="TextLarge" width="100%"></TD>
													</TR>
													<TR id="tr_Status" runat="server">
														<TD class="TextLarge" width="150"><asp:label id="lbl_Status" runat="server" Enabled="False">lbl_Status</asp:label></TD>
														<TD class="TextLarge">
                                                            <asp:RadioButton ID="rb_Temp" runat="server" GroupName="KKBER" Text="rb_Temp" />
                                                            <asp:RadioButton ID="rb_Endg" runat="server" GroupName="KKBER" Text="rb_Endg" />
                                                            <asp:RadioButton ID="rb_DP" runat="server" Enabled="False" GroupName="KKBER" 
                                                                Text="rb_DP" /></TD>
														<TD class="TextLarge" noWrap width="100%"><asp:label id="lbl_Faelligkeit" runat="server" Enabled="False"> Fälligkeit (gilt nur für DP):&nbsp;</asp:label><asp:textbox id="txt_Faelligkeit" runat="server" MaxLength="10" Width="100px" Enabled="False"></asp:textbox></TD>
													</TR>
													<TR id="tr_Statusold" runat="server">
														<TD class="TextLarge" width="150">
															<asp:label id="lbl_Statusold" runat="server" Enabled="False">lbl_Statusold</asp:label></TD>
														<TD class="TextLarge">
															<asp:radiobuttonlist id="rbKKBEROLD" runat="server" Enabled="False" RepeatDirection="Horizontal" AutoPostBack="True">
																<asp:ListItem Value="1">temp.</asp:ListItem>
																<asp:ListItem Value="2">endg.</asp:ListItem>
																<asp:ListItem Value="4">DP</asp:ListItem>
															</asp:radiobuttonlist></TD>
														<TD class="TextLarge" noWrap width="100%">
															<asp:label id="lblFaelligkeitOLD" runat="server" Enabled="False"> Fälligkeit (gilt nur für DP):&nbsp;</asp:label>
															<asp:textbox id="txtFaelligDatumOLD" runat="server" MaxLength="10" Width="100px" Enabled="False"></asp:textbox></TD>
													</TR>
												</TABLE>
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
								<td><!--#include File="../../../PageElements/Footer.html" --><strong>
												<asp:label id="lblMessage" runat="server"  EnableViewState="False"></asp:label></strong></td>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
		<script language="JavaScript">
<!-- //
 window.document.Form1.txtVertragsNr.focus();
//-->
		</script>
	</body>
</HTML>
