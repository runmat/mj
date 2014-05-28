<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change80_3.aspx.vb" Inherits="AppALD.Change80_3" %>
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
														<TD class="TextLarge" vAlign="top" colSpan="3"><asp:dropdownlist id="ddlZulDienst" runat="server"></asp:dropdownlist></TD>
													</TR>
													<TR id="trSAPAdresse" runat="server">
														<TD class="StandardTableAlternate" noWrap><asp:radiobutton id="rb_SAPAdresse" runat="server" GroupName="grpAdresse" Text="Hinterlegte Adresse:" AutoPostBack="True"></asp:radiobutton></TD>
														<TD class="StandardTableAlternate" vAlign="top" colSpan="3">
															<P><asp:dropdownlist id="ddlSAPAdresse" runat="server"></asp:dropdownlist></P>
														</TD>
													</TR>
													<TR id="trSAPAdresseBetreff" runat="server">
														<TD class="StandardTableAlternate" noWrap align="right">Betreffzeile:&nbsp;&nbsp;
														</TD>
														<TD class="StandardTableAlternate" vAlign="top" colSpan="3"><asp:textbox id="txtBetreffHA" tabIndex="1" runat="server" Width="300px" MaxLength="60"></asp:textbox></TD>
													</TR>
													<TR>
														<TD class="TextLarge" colSpan="4"><asp:radiobutton id="rb_VersandAdresse" runat="server" GroupName="grpAdresse" Text="Versandanschrift:" AutoPostBack="True"></asp:radiobutton></TD>
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
														<TD class="TextLarge" vAlign="top" align="right">Straﬂe.:&nbsp;&nbsp;</TD>
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
													<TR id="trVersandAdrEnd7" runat="server">
														<TD class="TextLarge" vAlign="top" align="right">Betreffzeile:&nbsp;&nbsp;
														</TD>
														<TD class="TextLarge" colSpan="3"><asp:textbox id="txtBetreff" tabIndex="9" runat="server" Width="300px" MaxLength="60"></asp:textbox></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="top" colSpan="4">&nbsp;&nbsp;&nbsp;
														</TD>
													</TR>
													<TR id="trVersandAdrEnd4" runat="server">
														<TD class="StandardTableAlternate" vAlign="top"><asp:label id="txtGrund" runat="server">Versandgrund:</asp:label></TD>
														<TD class="StandardTableAlternate" vAlign="top"><asp:dropdownlist id="ddlGrund" runat="server" AutoPostBack="True" tabIndex="10"></asp:dropdownlist></TD>
														<TD class="StandardTableAlternate" vAlign="top" width="108"><asp:label id="lblAuf" runat="server">auf:&nbsp;&nbsp;&nbsp;</asp:label></TD>
														<TD class="StandardTableAlternate" vAlign="top" width="100%"><asp:textbox id="txtAuf" tabIndex="11" runat="server" Width="200px" MaxLength="40"></asp:textbox></TD>
													</TR>
													<TR id="trVersandAdrEnd8" runat="server">
														<TD class="StandardTableAlternate" vAlign="top" align="right">Beschreibung:&nbsp;&nbsp;</TD>
														<TD class="StandardTableAlternate" vAlign="top" colSpan="3"><asp:textbox id="txtBetreff2" tabIndex="12" runat="server" Width="300px" MaxLength="60"></asp:textbox></TD>
													</TR>
													<TR id="trVersandAdrEnd5" runat="server">
														<TD class="TextLarge" vAlign="top" colSpan="4">&nbsp;&nbsp;&nbsp;
														</TD>
													</TR>
													<TR id="trZeit" runat="server">
														<TD class="StandardTableAlternate" vAlign="top"><asp:label id="lblVersandart" runat="server">Versandart:</asp:label></TD>
														<TD class="StandardTableAlternate" colSpan="6"><asp:radiobutton id="chkVersandStandard" runat="server" GroupName="Versandart" Text="innerhalb von 24 bis 48 h" Checked="True"></asp:radiobutton><asp:radiobutton id="chk0900" runat="server" GroupName="Versandart" Text="vor 9:00 Uhr*"></asp:radiobutton><asp:radiobutton id="chk1000" runat="server" GroupName="Versandart" Text="vor 10:00 Uhr*"></asp:radiobutton><asp:radiobutton id="chk1200" runat="server" GroupName="Versandart" Text="vor 12:00 Uhr*"></asp:radiobutton></TD>
													</TR>
												</TABLE>
											</td>
										</tr>
										<tr>
											<td vAlign="top" align="left" colSpan="3">*Diese Versandarten sind mit zus‰tzlichen 
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
						if(window.confirm("Achtung! Expressversandart verursacht zus‰tzliche Kosten.") != true)
						{
							window.document.Form1.chkVersandStandard.checked = true;
						}
					}
				}
			//-->
		</script>
	</body>
</HTML>
