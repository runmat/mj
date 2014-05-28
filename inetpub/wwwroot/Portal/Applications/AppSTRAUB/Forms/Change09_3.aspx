<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change09_3.aspx.vb" Inherits="AppSTRAUB.Change09_3" %>
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
					<td colSpan="3"><uc1:header id="ucHeader" runat="server"></uc1:header><asp:imagebutton id="ImageButton1" runat="server" Width="3px" ImageUrl="/Portal/Images/empty.gif"></asp:imagebutton></td>
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
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdSearch" runat="server" CssClass="StandardButton">&#149;&nbsp;Suchen</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton">&#149;&nbsp;Weiter</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;<asp:hyperlink id="lnkFahrzeugsuche" runat="server" CssClass="TaskTitle" NavigateUrl="Change09.aspx">Fahrzeugsuche</asp:hyperlink>&nbsp;<asp:hyperlink id="lnkFahrzeugAuswahl" runat="server" CssClass="TaskTitle" NavigateUrl="Change09_2.aspx">Fahrzeugauswahl</asp:hyperlink></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td class="TextLarge" vAlign="top" align="left" colSpan="3">
												<TABLE class="BorderFull" id="Table2" cellSpacing="0" cellPadding="3" bgColor="#ffffff" border="0">
													<TR>
														<TD class="">&nbsp;</TD>
														<TD class="" colSpan="5"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
														<TD class="">&nbsp;</TD>
													</TR>
													<TR class="FadeOutOne">
														<TD></TD>
														<TD colSpan="5"><strong>Zustellungsart</strong></TD>
														<TD noWrap></TD>
													</TR>
													<TR>
														<TD class=""></TD>
														<TD class="">&nbsp;&nbsp;</TD>
														<TD class="" noWrap><asp:radiobutton id="chkVersandStandard" runat="server" Text="Standard" Checked="True" GroupName="Versandart"></asp:radiobutton><BR>
															&nbsp;<FONT color="red">(siehe Hinweis)</FONT></TD>
														<TD class="" noWrap colSpan="1" rowSpan="1"><asp:radiobutton id="chk0900" runat="server" Text="Express (vor 9:00 Uhr)" GroupName="Versandart"></asp:radiobutton><BR>
															&nbsp;&nbsp;&nbsp;&nbsp; (28,00 Euro Netto)</TD>
														<TD class="" id="idPreis1" noWrap runat="server"><asp:radiobutton id="chk1000" runat="server" Text="vor 10:00 Uhr" GroupName="Versandart"></asp:radiobutton><BR>
															&nbsp;&nbsp;&nbsp;&nbsp; (23,00 Euro Netto)</TD>
														<TD class="" id="idPreis2" noWrap colSpan="1" rowSpan="1" runat="server"><asp:radiobutton id="chk1200" runat="server" Text="vor 12:00 Uhr" GroupName="Versandart"></asp:radiobutton><BR>
															&nbsp;&nbsp;&nbsp;&nbsp; (17,80 Euro Netto)</TD>
														<TD class="" noWrap></TD>
													</TR>
													<TR>
														<TD class=""></TD>
														<TD class="" colSpan="3"><EM><U>Achtung</U> </EM>:&nbsp;Die Nettopreise verstehen 
															sich pro Sendung.</TD>
														<TD class="" noWrap></TD>
														<TD class="" noWrap></TD>
														<TD class="" noWrap></TD>
													</TR>
													<TR>
														<TD></TD>
														<TD colSpan="3">&nbsp;</TD>
														<TD noWrap></TD>
														<TD noWrap></TD>
														<TD noWrap></TD>
													</TR>
													<TR class="FadeOutOne">
														<TD class=""></TD>
														<TD class=""><strong>Versandadresse</strong></TD>
														<TD class="" noWrap colSpan="2"></TD>
														<TD class="" noWrap></TD>
														<TD class="" noWrap></TD>
														<TD class="" noWrap></TD>
													</TR>
													<TR>
														<TD class=""></TD>
														<TD class=""></TD>
														<TD class="" noWrap colSpan="4">
															<TABLE id="Table1" cellSpacing="1" cellPadding="1" border="0">
																<TR>
																	<TD>Name
																	</TD>
																	<TD>&nbsp;</TD>
																	<TD><asp:textbox id="txtName1" runat="server" Width="200px" CssClass="TextBoxStyleLarge" MaxLength="128" ToolTip="Max. 128 Zeichen"></asp:textbox></TD>
																	<TD><FONT color="red">*</FONT></TD>
																	<TD></TD>
																</TR>
																<TR>
																	<TD>Name 2</TD>
																	<TD></TD>
																	<TD><asp:textbox id="txtName2" tabIndex="1" runat="server" Width="200px" CssClass="TextBoxStyleLarge" MaxLength="128" ToolTip="Max. 128 Zeichen"></asp:textbox></TD>
																	<TD></TD>
																	<TD></TD>
																</TR>
																<TR>
																	<TD>Straße / Nr</TD>
																	<TD></TD>
																	<TD><asp:textbox id="txtStrasse" tabIndex="2" runat="server" Width="200px" CssClass="TextBoxStyleLarge" MaxLength="128" ToolTip="Max. 128 Zeichen"></asp:textbox></TD>
																	<TD><asp:textbox id="txtNummer" tabIndex="3" runat="server" Width="50px" CssClass="TextBoxStyleLarge" MaxLength="10" ToolTip="Max. 10 Zeichen"></asp:textbox></TD>
																	<TD><FONT color="red">*</FONT></TD>
																</TR>
																<TR>
																	<TD>Postleitzahl</TD>
																	<TD></TD>
																	<TD><asp:textbox id="txtPLZ" tabIndex="4" runat="server" CssClass="TextBoxStyleLarge" MaxLength="5" ToolTip="Numerisch, 5-Stellig" Width="200px"></asp:textbox></TD>
																	<TD><FONT color="red">*</FONT></TD>
																	<TD></TD>
																</TR>
																<TR>
																	<TD>Ort</TD>
																	<TD></TD>
																	<TD>
																		<asp:TextBox id="txtOrt" runat="server" CssClass="TextBoxStyleLarge" tabIndex="5" MaxLength="128" ToolTip="Max. 128 Zeichen" Width="200px"></asp:TextBox></TD>
																	<TD><FONT color="red">*</FONT></TD>
																	<TD></TD>
																</TR>
															</TABLE>
														</TD>
														<TD class="" noWrap></TD>
													</TR>
													<TR>
														<TD></TD>
														<TD>&nbsp;</TD>
														<TD noWrap colSpan="4" class="TextBoxStyle"><FONT color="red">*</FONT>Eingabe 
															erforderlich</TD>
														<TD noWrap></TD>
													</TR>
													<TR class="DropDownStyle">
														<TD></TD>
														<TD class="DropDownStyle"><U>Hinweis:</U></TD>
														<TD noWrap colSpan="4"></TD>
														<TD noWrap></TD>
													</TR>
													<TR>
														<TD class="DropDownStyle"></TD>
														<TD class="DropDownStyle"></TD>
														<TD noWrap colSpan="4" class="DropDownStyle">Die Deutsche Post AG garantiert 
															für&nbsp;Standardsendungen keine Zustellzeiten<BR>
															und gibt die Zustellwahrscheinlichkeit wie folgt an:&nbsp;
															<BR>
															<BR>
															&nbsp;&nbsp;&nbsp;•&nbsp;95% aller Sendungen werden dem Empfänger innerhalb von 
															24 Stunden zugestellt,<BR>
															&nbsp;&nbsp;&nbsp;•&nbsp;3% aller Sendungen benötigen zwischen 24 und 48 
															Stunden bis zur Zustellung.<BR>
															<BR>
															Bitte beachten Sie hierzu auch die Beförderungsbedingungen der Deutschen Post 
															AG!</TD>
														<TD class="DropDownStyle" noWrap></TD>
													</TR>
												</TABLE>
												&nbsp;</td>
										</tr>
										<TR>
											<TD vAlign="top" align="left" colSpan="3"></TD>
										</TR>
										<TR>
											<TD vAlign="top" align="left" colSpan="3"></TD>
										</TR>
										<tr>
											<td colSpan="3"><!--#include File="../../../PageElements/Footer.html" --><br>
											</td>
										</tr>
									</TABLE>
								</td>
							</tr>
						</TABLE>
					</TD>
				</TR>
			</table>
		</form>
		<script language="JavaScript">
			<!-- //
			window.document.Form1.elements[window.document.Form1.length-2].focus();
			//-->
		</script>
	</body>
</HTML>
