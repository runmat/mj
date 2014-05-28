<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change80_3.aspx.vb" Inherits="AppFMS.Change80_3" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
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
											<TD class="TaskTitle" vAlign="top"><asp:hyperlink id="lnkFahrzeugsuche" runat="server" CssClass="TaskTitle" NavigateUrl="Change04.aspx">Fahrzeugsuche</asp:hyperlink>&nbsp;<asp:hyperlink id="lnkFahrzeugAuswahl" runat="server" CssClass="TaskTitle" NavigateUrl="Change04_2.aspx">Fahrzeugauswahl</asp:hyperlink></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td vAlign="top" align="left" colSpan="3"></td>
										</tr>
										<tr>
											<td class="TextLarge" vAlign="top" align="left" colSpan="3">
												<TABLE id="Table2" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
													<TR id="trZulst" runat="server">
														<TD class="TextLarge"><asp:radiobutton id="rb_Zulst" runat="server" GroupName="grpAdresse" Text="Zulassungsstelle:" AutoPostBack="True"></asp:radiobutton></TD>
														<TD class="TextLarge" vAlign="top" colSpan="3" align="left" width="100%"><asp:dropdownlist id="ddlZulDienst" runat="server"></asp:dropdownlist></TD>
													</TR>
													<TR id="trSAPAdresse" runat="server">
														<TD class="StandardTableAlternate" vAlign="top" noWrap><asp:radiobutton id="rb_SAPAdresse" runat="server" GroupName="grpAdresse" Text="Hinterlegte Adresse" AutoPostBack="True"></asp:radiobutton></TD>
														<TD class="StandardTableAlternate" vAlign="top" colSpan="3" width="100%">
															<P align="left">
																<TABLE class="TableSearch" id="tblSuche" cellSpacing="1" cellPadding="1" border="0" runat="server">
																	<TR>
																		<TD></TD>
																		<TD colSpan="3"><strong>Bitte Vorauswahl durchführen:</strong></TD>
																	</TR>
																	<TR>
																		<TD>&nbsp;&nbsp;</TD>
																		<TD colSpan="3">&nbsp;</TD>
																	</TR>
																	<TR>
																		<TD></TD>
																		<TD>Name:</TD>
																		<TD noWrap colSpan="2">
																			<asp:textbox id="txtSucheName" runat="server"></asp:textbox>&nbsp;<FONT color="red" size="2">*</FONT></TD>
																	</TR>
																	<TR>
																		<TD></TD>
																		<TD>PLZ:</TD>
																		<TD noWrap><asp:textbox id="txtSuchePLZ" runat="server"></asp:textbox></TD>
																		<TD>
																			<asp:linkbutton id="btnSucheAdresse" runat="server" CssClass="StandardButtonTable">&#149;&nbsp;Suchen</asp:linkbutton></TD>
																	</TR>
																	<TR>
																		<TD></TD>
																		<TD>&nbsp;</TD>
																		<TD></TD>
																		<TD></TD>
																	</TR>
																	<TR>
																		<TD></TD>
																		<TD noWrap colSpan="3"><FONT color="red"><FONT size="2">* Eingabe erforderlich.<FONT color="black">
																						Suche mit Platzhaltern ist nicht möglich.</FONT></FONT></FONT></TD>
																	</TR>
																	<TR>
																		<TD></TD>
																		<TD noWrap colSpan="3">
																			<asp:label id="lblErrorSuche" runat="server" CssClass="TextError" EnableViewState="False"></asp:label>
																			<asp:label id="lblInfo" runat="server" CssClass="TextExtraLarge" EnableViewState="False"></asp:label></TD>
																	</TR>
																</TABLE>
															</P>
															<asp:dropdownlist id="ddlSAPAdresse" runat="server" Width="100%" Visible="False"></asp:dropdownlist>
														</TD>
													</TR>
													<TR>
														<TD class="TextLarge" colSpan="4"></TD>
													</TR>
													<TR>
														<TD class="TextLarge" colSpan="4">
															<asp:radiobutton id="rb_VersandAdresse" runat="server" AutoPostBack="True" Text="Versandanschrift:" GroupName="grpAdresse"></asp:radiobutton></TD>
													</TR>
													<TR id="trVersandAdrEnd1" runat="server">
														<TD class="StandardTableAlternate" vAlign="center" noWrap align="right">Firma / 
															Name:&nbsp;&nbsp;</TD>
														<TD class="StandardTableAlternate"><asp:textbox id="txtName1" tabIndex="2" runat="server" MaxLength="35"></asp:textbox></TD>
														<TD class="StandardTableAlternate" noWrap align="right" width="108">Ansprechpartner 
															/ Zusatz:&nbsp;&nbsp;</TD>
														<TD class="StandardTableAlternate" width="100%"><asp:textbox id="txtName2" tabIndex="3" runat="server" MaxLength="35"></asp:textbox></TD>
													</TR>
													<TR id="trVersandAdrEnd2" runat="server">
														<TD class="TextLarge" vAlign="top" align="right">Straße.:&nbsp;&nbsp;</TD>
														<TD class="TextLarge" vAlign="top"><asp:textbox id="txtStr" tabIndex="4" runat="server" MaxLength="60"></asp:textbox></TD>
														<TD class="TextLarge" vAlign="top" noWrap align="right" width="108">Hausnummer.:&nbsp;&nbsp;</TD>
														<TD class="TextLarge" vAlign="top" width="100%"><asp:textbox id="txtNr" tabIndex="5" runat="server" MaxLength="10"></asp:textbox></TD>
													</TR>
													<TR id="trVersandAdrEnd3" runat="server">
														<TD class="StandardTableAlternate" vAlign="top" align="right">Postleitzahl:&nbsp;&nbsp;</TD>
														<TD class="StandardTableAlternate"><asp:textbox id="txtPlz" tabIndex="6" runat="server" MaxLength="10"></asp:textbox></TD>
														<TD class="StandardTableAlternate" align="right" width="108">Ort:&nbsp;&nbsp;</TD>
														<TD class="StandardTableAlternate" width="100%"><asp:textbox id="txtOrt" tabIndex="7" runat="server" MaxLength="40"></asp:textbox></TD>
													</TR>
													<TR id="trVersandAdrEnd6" runat="server">
														<TD class="TextLarge" vAlign="top"></TD>
														<TD class="TextLarge"><asp:textbox id="txtLand" tabIndex="8" runat="server" Visible="False" MaxLength="3"></asp:textbox></TD>
														<TD class="TextLarge" width="108"></TD>
														<TD class="TextLarge" width="100%"></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="top" colSpan="4">&nbsp;&nbsp;&nbsp;
														</TD>
													</TR>
													<TR id="trVersandAdrEnd4" runat="server">
														<TD class="StandardTableAlternate" vAlign="top"><asp:label id="txtGrund" runat="server">Versandgrund:</asp:label></TD>
														<TD class="StandardTableAlternate" vAlign="top"><asp:dropdownlist id="ddlGrund" tabIndex="10" runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
														<TD class="StandardTableAlternate" vAlign="top" width="108"><asp:label id="lblAuf" runat="server">auf:&nbsp;&nbsp;&nbsp;</asp:label></TD>
														<TD class="StandardTableAlternate" vAlign="top" width="100%"><asp:textbox id="txtAuf" tabIndex="11" runat="server" Width="200px" MaxLength="40"></asp:textbox></TD>
													</TR>
													<TR id="trVersandAdrEnd9" runat="server">
														<TD class="StandardTableAlternate" vAlign="top" align="left">Bemerkung:&nbsp;&nbsp;</TD>
														<TD class="StandardTableAlternate" vAlign="top" colSpan="3"><asp:textbox id="txtBemerkung" tabIndex="12" runat="server" Width="300px" MaxLength="60"></asp:textbox></TD>
													</TR>
													<TR id="trVersandAdrEnd5" runat="server">
														<TD class="TextLarge" vAlign="top" colSpan="4">&nbsp;&nbsp;&nbsp;
														</TD>
													</TR>
													<TR id="trVersandart1" runat="server">
														<TD class="StandardTableAlternate" vAlign="top"><asp:label id="lblVersandart" runat="server">Versandart:</asp:label></TD>
														<TD class="StandardTableAlternate" colSpan="6">
                                                            <asp:radiobutton id="chkVersandStandard" runat="server" GroupName="Versandart" 
                                                                Text="Standardversand" Checked="True"></asp:radiobutton><asp:radiobutton id="chk0900" runat="server" GroupName="Versandart" Text="vor 9:00 Uhr*"></asp:radiobutton><asp:radiobutton id="chk1000" runat="server" GroupName="Versandart" Text="vor 10:00 Uhr*"></asp:radiobutton><asp:radiobutton id="chk1200" runat="server" GroupName="Versandart" Text="vor 12:00 Uhr*"></asp:radiobutton></TD>
													</TR>
												</TABLE>
											</td>
										</tr>
										<tr id="trVersandart2" runat="server">
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
		<script language="JavaScript">
			<!-- //
				function DoWarning() {
					if((window.document.Form1.chk0900.checked == true) || (window.document.Form1.chk1000.checked == true) || (window.document.Form1.chk1200.checked == true))
					{
						if(window.confirm("Achtung! Expressversandart verursacht zusätzliche Kosten.") != true)
						{
							window.document.Form1.chkVersandStandard.checked = true;
						}
					}
				}
			//-->
		</script>
	</body>
</HTML>
