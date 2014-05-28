<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change80_3.aspx.vb" Inherits="CKG.Components.ComCommon.AppDokVerw.Change80_3" %>
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
	    <style type="text/css">
            .style1
            {
                width: 149px;
            }
            .style2
            {
                width: 163px;
            }
        </style>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" name="Form1" method="post" runat="server">
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
													<TR id="tr_ZulstSichtbarkeit" runat="server">
														<TD class="TextLarge"><asp:radiobutton id="rb_Zulst" runat="server" AutoPostBack="True" Text="rb_Zulst" GroupName="grpAdresse"></asp:radiobutton></TD>
														<TD class="TextLarge" vAlign="top" align="left" width="100%" colSpan="3"><asp:dropdownlist id="ddlZulDienst" runat="server"></asp:dropdownlist></TD>
													</TR>
													<TR id="trSAPAdresse" runat="server">
														<TD class="StandardTableAlternate" vAlign="top" noWrap><asp:radiobutton id="rb_SAPAdresse" runat="server" AutoPostBack="True" Text="rb_SAPAdresse" GroupName="grpAdresse"></asp:radiobutton></TD>
														<TD class="StandardTableAlternate" vAlign="top" width="100%" colSpan="3">
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
																		<TD noWrap colSpan="2"><asp:textbox id="txtSucheName" runat="server"></asp:textbox>&nbsp;</TD>
																	</TR>
																	<TR>
																		<TD></TD>
																		<TD>PLZ:</TD>
																		<TD noWrap class="style2"><asp:textbox id="txtSuchePLZ" runat="server"></asp:textbox></TD>
																		<TD class="style1">&nbsp;</TD>
																	</TR>
																	<TR>
																		<TD>&nbsp;</TD>
																		<TD>Ort:</TD>
																		<TD noWrap class="style2"><asp:textbox id="txtSucheOrt" runat="server"></asp:textbox></TD>
																		<TD class="style1"><asp:linkbutton id="btnSucheAdresse" runat="server" CssClass="StandardButtonTable">&#149;&nbsp;Suchen</asp:linkbutton></TD>
																	</TR>
																	<TR>
																		<TD></TD>
																		<TD>&nbsp;</TD>
																		<TD class="style2"></TD>
																		<TD class="style1"></TD>
																	</TR>
																	<TR>
																		<TD></TD>
																		<TD noWrap colSpan="3"><FONT color="black" size="2">
																						&nbsp;Eine
																						Suche mit Platzhaltern * ist möglich.</FONT></TD>
																	</TR>
																	<TR>
																		<TD></TD>
																		<TD noWrap colSpan="3"><asp:label id="lblErrorSuche" runat="server" CssClass="TextError" EnableViewState="False"></asp:label><asp:label id="lblInfo" runat="server" CssClass="TextExtraLarge" EnableViewState="False"></asp:label></TD>
																	</TR>
																</TABLE>
															</P>
															<asp:dropdownlist id="ddlSAPAdresse" runat="server" Width="100%" Visible="False"></asp:dropdownlist></TD>
													</TR>
													<TR>
														<TD class="TextLarge" colSpan="4"></TD>
													</TR>
													<TR id="trVersandAdrEnd0" runat="server">
														<TD class="TextLarge" colSpan="4"><asp:radiobutton id="rb_VersandAdresse" runat="server" AutoPostBack="True" Text="rb_VersandAdresse" GroupName="grpAdresse"></asp:radiobutton></TD>
													</TR>
													<TR id="trVersandAdrEnd1" runat="server">
														<TD class="StandardTableAlternate" vAlign="center" noWrap align="right"><asp:label id="lbl_Firma" runat="server">lbl_Firma</asp:label>&nbsp;&nbsp;</TD>
														<TD class="StandardTableAlternate"><asp:textbox id="txtName1" tabIndex="2" runat="server" MaxLength="35"></asp:textbox></TD>
														<TD class="StandardTableAlternate" noWrap align="right" width="108"><asp:label id="lbl_Ansprechpartner" runat="server">lbl_Ansprechpartner</asp:label>&nbsp;&nbsp;</TD>
														<TD class="StandardTableAlternate" width="100%"><asp:textbox id="txtName2" tabIndex="3" runat="server" MaxLength="35"></asp:textbox></TD>
													</TR>
													<TR id="trVersandAdrEnd2" runat="server">
														<TD class="TextLarge" vAlign="top" align="right"><asp:label id="lbl_Strasse" runat="server">lbl_Strasse</asp:label>&nbsp;&nbsp;</TD>
														<TD class="TextLarge" vAlign="top"><asp:textbox id="txtStr" tabIndex="4" runat="server" MaxLength="60"></asp:textbox></TD>
														<TD class="TextLarge" vAlign="top" noWrap align="right" width="108"><asp:label id="lbl_Hausnummer" runat="server">lbl_Hausnummer</asp:label>&nbsp;&nbsp;</TD>
														<TD class="TextLarge" vAlign="top" width="100%"><asp:textbox id="txtNr" tabIndex="5" runat="server" MaxLength="10"></asp:textbox></TD>
													</TR>
													<TR id="trVersandAdrEnd3" runat="server">
														<TD class="StandardTableAlternate" vAlign="top" align="right"><asp:label id="lbl_Postleitzahl" runat="server">lbl_Postleitzahl</asp:label>&nbsp;&nbsp;</TD>
														<TD class="StandardTableAlternate"><asp:textbox id="txtPlz" tabIndex="6" runat="server" MaxLength="10"></asp:textbox></TD>
														<TD class="StandardTableAlternate" align="right" width="108"><asp:label id="lbl_Ort" runat="server">lbl_Ort</asp:label>&nbsp;&nbsp;</TD>
														<TD class="StandardTableAlternate" width="100%"><asp:textbox id="txtOrt" tabIndex="7" runat="server" MaxLength="40"></asp:textbox></TD>
													</TR>
													<TR id="tr_Land" runat="server">
														<TD class="TextLarge" vAlign="top" align="right"><asp:label id="lbl_Land" runat="server">lbl_Land</asp:label>&nbsp;</TD>
														<TD class="TextLarge"><asp:dropdownlist id="ddlLand" tabIndex="10" runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
														<TD class="TextLarge" width="108"></TD>
														<TD class="TextLarge" width="100%"></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="top" colSpan="4">&nbsp;&nbsp;&nbsp;
														</TD>
													</TR>
													<TR id="trVersandAdrEnd4" runat="server">
														<TD class="StandardTableAlternate" vAlign="top"><asp:label id="lbl_Grund" runat="server">lbl_Grund</asp:label></TD>
														<TD class="StandardTableAlternate" vAlign="top"><asp:dropdownlist id="ddlGrund" tabIndex="10" runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
														<TD class="StandardTableAlternate" vAlign="top" width="108"><asp:label id="lbl_Auf" runat="server">lbl_Auf</asp:label></TD>
														<TD class="StandardTableAlternate" vAlign="top" width="100%"><asp:textbox id="txtAuf" tabIndex="11" runat="server" Width="200px" MaxLength="40"></asp:textbox></TD>
													</TR>
													<TR id="trVersandAdrEnd9" runat="server">
														<TD class="StandardTableAlternate" vAlign="top" align="left"><asp:label id="lbl_Bemerkung" runat="server">lbl_Bemerkung</asp:label></TD>
														<TD class="StandardTableAlternate" vAlign="top" colSpan="3"><asp:textbox id="txtBemerkung" tabIndex="12" runat="server" Width="300px" MaxLength="60"></asp:textbox></TD>
													</TR>
													<TR id="trVersandAdrEnd5" runat="server">
														<TD class="TextLarge" vAlign="top" colSpan="4">&nbsp;&nbsp;&nbsp;
														</TD>
													</TR>
													<TR id="trVersandart1" runat="server">
														<TD class="StandardTableAlternate" vAlign="top"><asp:label id="lbl_Versandart" runat="server">lbl_Versandart</asp:label></TD>
														<TD class="StandardTableAlternate" colSpan="6">
															<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0">
																<TR>
																	<TD class="TextLarge" vAlign="top" noWrap rowSpan="3"><asp:Label ID="lbl_Normal" Runat="server">lbl_Normal</asp:Label></TD>
																	<TD noWrap><asp:radiobutton id="rb_VersandStandard" runat="server" GroupName="Versandart" Checked="True" Text="rb_VersandStandard"></asp:radiobutton></TD>
																	<TD noWrap rowSpan="3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD>
																	<TD class="TextLarge" vAlign="top" noWrap rowSpan="3"><asp:Label ID="lbl_Express" Runat="server">lbl_Express</asp:Label></TD>
																	<TD noWrap><asp:radiobutton id="rb_0900" runat="server" GroupName="Versandart" Text="rb_0900"></asp:radiobutton></TD>
																</TR>
																<TR>
																	<TD noWrap><asp:radiobutton id="rb_Sendungsverfolgt" runat="server" GroupName="Versandart" Text="rb_Sendungsverfolgt"></asp:radiobutton></TD>
																	<TD noWrap><asp:radiobutton id="rb_1000" runat="server" GroupName="Versandart" Text="rb_1000"></asp:radiobutton></TD>
																</TR>
																<TR>
																	<TD noWrap>&nbsp;</TD>
																	<TD noWrap><asp:radiobutton id="rb_1200" runat="server" GroupName="Versandart" Text="rb_1200"></asp:radiobutton></TD>
																</TR>
															</TABLE>
														</TD>
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
	</body>
</HTML>
