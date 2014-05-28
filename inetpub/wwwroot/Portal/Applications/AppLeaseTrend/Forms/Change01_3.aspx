<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change01_3.aspx.vb" Inherits="AppLeaseTrend.Change01_3" %>
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
					<td colSpan="3"><uc1:header id="ucHeader" runat="server"></uc1:header><asp:imagebutton id="ImageButton1" runat="server" ImageUrl="/Portal/Images/empty.gif" Width="3px"></asp:imagebutton></td>
				</tr>
				<TR>
					<TD vAlign="top" align="left" colSpan="3">
						<TABLE id="Table10" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="3"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Adressauswahl)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table12" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle" width="150">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton"> &#149;&nbsp;Weiter</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdSearch" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Suchen</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<TD vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top"><asp:hyperlink id="lnkFahrzeugsuche" runat="server" CssClass="TaskTitle" NavigateUrl="Change205.aspx">Fahrzeugsuche</asp:hyperlink>&nbsp;<asp:hyperlink id="lnkFahrzeugAuswahl" runat="server" CssClass="TaskTitle" NavigateUrl="Change205_2.aspx">Fahrzeugauswahl</asp:hyperlink></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td vAlign="top" align="left" colSpan="3">
												<TABLE id="Table1" height="23" cellSpacing="1" cellPadding="1" width="100%" border="0">
													<TR>
														<TD noWrap width="103"><asp:label id="lblLeasingnehmer" runat="server">Leasingnehmer:</asp:label></TD>
														<TD noWrap width="60"><asp:dropdownlist id="ddlLeasingnehmer" runat="server" Width="200px"></asp:dropdownlist></TD>
														<TD noWrap width="64"><asp:label id="Label1" runat="server" Width="60px">Lieferant:</asp:label></TD>
														<TD noWrap width="221"><asp:dropdownlist id="ddlLieferant" runat="server" Width="200px"></asp:dropdownlist></TD>
														<TD noWrap width="64"><asp:label id="lblVersandAdresse" runat="server">Zulassungsstelle:</asp:label></TD>
														<TD noWrap>
															<P><asp:dropdownlist id="ddlZulDienst" runat="server" Width="200px"></asp:dropdownlist></P>
														</TD>
													</TR>
												</TABLE>
											</td>
										</tr>
										<tr>
											<td class="TextLarge" vAlign="top" align="left" colSpan="3">
												<TABLE id="Table2" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
													<TR id="trVersandAdrTemp" runat="server">
														<TD class="StandardTableAlternate" height="32"></TD>
														<TD class="StandardTableAlternate" vAlign="top" width="183" height="32"></TD>
														<TD class="StandardTableAlternate" vAlign="top" width="58" height="32">&nbsp;</TD>
														<TD class="StandardTableAlternate" vAlign="top" width="100%" height="32">
															<P>&nbsp;</P>
														</TD>
													</TR>
													<TR id="trVersandAdrEnd1" runat="server">
														<TD class="StandardTableAlternate" vAlign="center" align="left">Name:</TD>
														<TD class="StandardTableAlternate" width="183"><asp:textbox id="txtName1" tabIndex="1" runat="server" Width="240px" MaxLength="35"></asp:textbox></TD>
														<TD class="StandardTableAlternate" align="left" width="58">Name2:</TD>
														<TD class="StandardTableAlternate" width="100%"><asp:textbox id="txtName2" tabIndex="1" runat="server" Width="240px" MaxLength="35"></asp:textbox></TD>
													</TR>
													<TR id="trVersandAdrEnd2" runat="server">
														<TD class="StandardTableAlternate" vAlign="top" align="left">Str.:</TD>
														<TD class="StandardTableAlternate" vAlign="top" width="183"><asp:textbox id="txtStr" tabIndex="2" runat="server" Width="240px" MaxLength="60"></asp:textbox></TD>
														<TD class="StandardTableAlternate" vAlign="top" noWrap align="left" width="58"></TD>
														<TD class="StandardTableAlternate" vAlign="top" width="100%"></TD>
													</TR>
													<TR id="trVersandAdrEnd3" runat="server">
														<TD class="StandardTableAlternate" vAlign="top" align="left">Plz.:</TD>
														<TD class="StandardTableAlternate" width="183"><asp:textbox id="txtPlz" tabIndex="4" runat="server" Width="79px" MaxLength="10"></asp:textbox></TD>
														<TD class="StandardTableAlternate" align="left" width="58">Ort:</TD>
														<TD class="StandardTableAlternate" width="100%"><asp:textbox id="txtOrt" tabIndex="5" runat="server" Width="240px" MaxLength="40"></asp:textbox></TD>
													</TR>
													<TR id="trVersandAdrEnd6" runat="server">
														<TD class="StandardTableAlternate" vAlign="top"></TD>
														<TD class="StandardTableAlternate" width="183"><asp:textbox id="txtLand" tabIndex="4" runat="server" Visible="False" MaxLength="3">DE</asp:textbox></TD>
														<TD class="StandardTableAlternate" width="58"></TD>
														<TD class="StandardTableAlternate" width="100%"></TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" vAlign="top">
															<asp:label id="lblFormularart" runat="server" Width="65px" Visible="False">Formularart:   </asp:label></TD>
														<TD class="StandardTableAlternate" width="183">
															<asp:dropdownlist id="ddlFormular" runat="server" Width="237px" Visible="False"></asp:dropdownlist></TD>
														<TD class="StandardTableAlternate" width="58"></TD>
														<TD class="StandardTableAlternate" width="100%"></TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" vAlign="top" colSpan="4"></TD>
													</TR>
													<TR id="trVersandAdrEnd4" runat="server">
														<TD class="TextLarge" vAlign="baseline">
															<asp:label id="lblVersandgrund" runat="server" Width="65px" Visible="False">Grund:</asp:label></TD>
														<TD class="" width="183"><asp:dropdownlist id="ddlGrund" runat="server" Width="175px"></asp:dropdownlist></TD>
														<TD class="" vAlign="baseline" align="left" width="100%" colSpan="2">&nbsp;</TD>
													</TR>
													<TR>
														<TD class="TextLarge" id="tdInput1" vAlign="baseline" align="left" runat="server"></TD>
														<TD id="tdInput" noWrap colSpan="3" runat="server"><asp:textbox id="txtGrundBemerkung" tabIndex="4" runat="server" Width="300px" MaxLength="60"></asp:textbox></TD>
													</TR>
													<TR id="trVersandAdrEnd5" runat="server">
														<TD class="StandardTableAlternate" vAlign="top" colSpan="4">&nbsp;</TD>
													</TR>
													<TR id="trZeit" runat="server">
														<TD class="StandardTableAlternate" vAlign="top"><asp:label id="lblVersandart" runat="server">Versandart:</asp:label></TD>
														<TD class="StandardTableAlternate" colSpan="6">
                                                            <asp:radiobutton id="chkVersandStandard" runat="server" GroupName="Versandart" 
                                                                Checked="True" Text="sendungsverfolgt"></asp:radiobutton><asp:radiobutton id="chk0900" runat="server" GroupName="Versandart" Text="vor 9:00 Uhr*"></asp:radiobutton><asp:radiobutton id="chk1000" runat="server" GroupName="Versandart" Text="vor 10:00 Uhr*"></asp:radiobutton><asp:radiobutton id="chk1200" runat="server" GroupName="Versandart" Text="vor 12:00 Uhr*"></asp:radiobutton><asp:radiobutton id="chkZweigstellen" runat="server" Visible="False" GroupName="grpVersand" Checked="True" Text="Zweigstellen:"></asp:radiobutton><asp:radiobutton id="chkZulassungsstellen" runat="server" Visible="False" GroupName="grpVersand" Text="Zulassungsstellen:"></asp:radiobutton></TD>
													</TR>
												</TABLE>
											</td>
										</tr>
										<tr>
											<td vAlign="top" align="left" colSpan="3">*Diese Versandarten sind mit zusätzlichen 
												Kosten verbunden.
											</td>
										</tr>
										<TR>
											<TD vAlign="top" align="left" colSpan="3"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
										</TR>
										<tr>
											<td colSpan="3"><!--#include File="../../../PageElements/Footer.html" --><br>
											</td>
										</tr>
									</TABLE>
									<asp:literal id="litScript" runat="server"></asp:literal></TD>
							</tr>
						</TABLE>
					</TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
