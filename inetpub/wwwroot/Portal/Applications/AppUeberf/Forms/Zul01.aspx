<%@ Register TagPrefix="uc1" TagName="AddressSearchInputControl" Src="../Controls/AddressSearchInputControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ProgressControl" Src="../Controls/ProgressControl.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CKG.Portal.PageElements" Assembly="CKG.Portal"   %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Zul01.aspx.vb" Inherits="AppUeberf.Zul01" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<META content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<META content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<META content="JavaScript" name="vs_defaultClientScript">
		<META content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</HEAD>
	<BODY leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<FORM id="Form1" method="post" runat="server">
			<TABLE width="100%" align="center">
				<TR>
					<TD>
						<uc1:header id="ucHeader" runat="server"></uc1:header></TD>
				</TR>
				<TR>
					<TD width="100%" colSpan="3">
						<TABLE id="Table10" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="PageNavigation" colSpan="2">
									<asp:label id="lblHead" runat="server"></asp:label>&nbsp;(
									<asp:label id="lblPageTitle" runat="server">Fahrzeugdaten</asp:label>)</TD>
							</TR>
							<TR>
								<TD style="WIDTH: 144px" vAlign="top" width="144">
									<asp:calendar id="calVon" runat="server" Width="120px" CellPadding="0" BorderStyle="Solid" BorderColor="Black" Visible="False">
										<TodayDayStyle Font-Bold="True"></TodayDayStyle>
										<NextPrevStyle ForeColor="White"></NextPrevStyle>
										<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
										<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
										<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
										<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
										<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
									</asp:calendar></TD>
								<TD vAlign="top">
									<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
										<TR>
											<TD style="WIDTH: 456px" width="456" colSpan="2">
												<uc1:ProgressControl id="ProgressControl1" runat="server"></uc1:ProgressControl></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 456px" width="456">&nbsp;</TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 456px; HEIGHT: 12px" width="456"><STRONG>Halter</STRONG></TD>
											<TD style="HEIGHT: 12px"></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 456px; HEIGHT: 12px" width="456">
												<uc1:addresssearchinputcontrol id="ctrlAddressSearchHalter" runat="server"></uc1:addresssearchinputcontrol></TD>
											<TD style="HEIGHT: 12px"></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 456px; HEIGHT: 12px" width="456">
												<asp:label id="Label20" runat="server" Width="150px" Height="22px"> Auswahl:</asp:label>
												<asp:dropdownlist id="drpHalter" runat="server" Width="294px" AutoPostBack="True"></asp:dropdownlist></TD>
											<TD style="HEIGHT: 12px">
												<asp:CheckBox id="chkUbernahmeLeasingnehmer" runat="server" AutoPostBack="True" Text="Leasingnehmerdaten übernehmen"></asp:CheckBox></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 456px; HEIGHT: 12px" width="456">
												<asp:label id="Label1" runat="server" Width="150px" Height="22px">Haltername:*</asp:label>
												<asp:textbox id="txtHalter" runat="server" Width="292px"></asp:textbox></TD>
											<TD style="HEIGHT: 12px"></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 456px" width="456" height="21">
												<asp:label id="Label3" runat="server" Width="150px" Height="22px">Halter-PLZ/-Ort:*</asp:label>
												<asp:textbox id="txtHalterPLZ" runat="server" Width="88px" MaxLength="5"></asp:textbox>&nbsp;
												<asp:textbox id="txtHalterOrt" runat="server" Width="196px"></asp:textbox></TD>
											<TD height="21"></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 456px" width="456" height="21"></TD>
											<TD height="21"></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 456px" width="456" height="21">
												<asp:label id="Label16" runat="server" Width="150px" Height="14px" Font-Bold="True">Versicherungsnehmer:*</asp:label></TD>
											<TD height="21"></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 456px" width="456" height="21">
												<asp:radiobuttonlist id="rdbVersicherungsnehmer" runat="server" Width="258px" Height="10px" RepeatDirection="Horizontal" TextAlign="Left">
													<asp:ListItem Value="1">Halter</asp:ListItem>
													<asp:ListItem Value="2">Leasinggesellschaft</asp:ListItem>
													<asp:ListItem Value="3">Leasingnehmer</asp:ListItem>
												</asp:radiobuttonlist></TD>
											<TD height="21"></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 456px" width="456" height="21">
												<asp:label id="Label21" runat="server" Width="150px" Height="22px">Auswahl Versicherer:</asp:label>
												<asp:dropdownlist id="drpVersicherer" runat="server" Width="292px"></asp:dropdownlist></TD>
											<TD height="21"></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 456px" width="456" height="21">
												<asp:label id="Label17" runat="server" Width="150px" Height="22px">Versicherer:*</asp:label>
												<asp:textbox id="txtVersicherer" runat="server" Width="292px"></asp:textbox></TD>
											<TD height="21"></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 456px" width="456" height="21"></TD>
											<TD height="21"></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 456px" width="456" height="21">
												<asp:label id="Label18" runat="server" Width="198px" Height="18px" Font-Bold="True">KFZ-Steuer-Zahlung durch:*</asp:label></TD>
											<TD height="21"></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 456px" width="456" height="21">
												<asp:radiobuttonlist id="rdSteuerzahlung" runat="server" Width="240px" Height="4px" RepeatDirection="Horizontal" TextAlign="Left">
													<asp:ListItem Value="1">Halter</asp:ListItem>
													<asp:ListItem Value="2">Leasinggesellschaft</asp:ListItem>
													<asp:ListItem Value="3">Leasingnehmer</asp:ListItem>
												</asp:radiobuttonlist></TD>
											<TD height="21"></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 456px" width="456" height="21">
												<asp:label id="Label6" runat="server" Width="150px" Height="22px">gew. Zulassungsdatum:</asp:label>
												<asp:textbox id="txtAbgabetermin" runat="server" Width="88px"></asp:textbox>
												<asp:linkbutton id="btnVon" runat="server" Width="100px" CssClass="StandardButtonTable"> &#149;&nbsp;Kalender...</asp:linkbutton></TD>
											<TD height="21"></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 456px" width="456" colSpan="2" height="21">
												<asp:Label id="Label4" runat="server" Width="707px" ForeColor="Blue">Bundesweite Zulassungen benötigen 3 Werktage; 2 Tage bis zur Zulassung + 1 Tag für die Rücklieferung.</asp:Label></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 456px" width="456" height="21"></TD>
											<TD height="21"></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 456px" width="456">
												<asp:label id="Label12" runat="server" Width="150px" Font-Bold="True">Zulassungskreis:</asp:label>
												<asp:textbox id="txtZulkreis" runat="server" Width="39px" Font-Bold="True" Enabled="False"></asp:textbox>&nbsp;&nbsp;&nbsp;
												<asp:linkbutton id="btnZulkreis" runat="server" Width="177px" CssClass="StandardButtonTable"> &#149;&nbsp;Zulassungskeis ermitteln*</asp:linkbutton></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 456px" width="456">
												<asp:label id="Label2" runat="server" Width="150px">1. Wunschkennzeichen:</asp:label>
												<asp:textbox id="txtKennzeichen1" runat="server" Width="39px" Enabled="False"></asp:textbox>
												<asp:label id="Label10" runat="server" Width="11px" Font-Bold="True"> -</asp:label>
												<asp:textbox id="txtKennZusatz1" runat="server" Width="81px" MaxLength="7"></asp:textbox></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 456px" width="456">
												<asp:label id="Label7" runat="server" Width="150px">2. Wunschkennzeichen:</asp:label>
												<asp:textbox id="txtKennzeichen2" runat="server" Width="39px" Enabled="False"></asp:textbox>
												<asp:label id="Label9" runat="server" Width="11px" Font-Bold="True"> -</asp:label>
												<asp:textbox id="txtKennZusatz2" runat="server" Width="81px" MaxLength="7"></asp:textbox></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 456px" width="456">
												<asp:label id="Label8" runat="server" Width="150px">3. Wunschkennzeichen:</asp:label>
												<asp:textbox id="txtKennzeichen3" runat="server" Width="39px" Enabled="False"></asp:textbox>
												<asp:label id="Label11" runat="server" Width="11px" Font-Bold="True"> -</asp:label>
												<asp:textbox id="txtKennZusatz3" runat="server" Width="81px" MaxLength="7"></asp:textbox></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 456px" width="456"></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 456px" width="456">
												<asp:Panel id="pnlSchilderversand" runat="server" Width="429px" Height="114px">
													<TABLE id="tblSchilderversand" cellSpacing="1" cellPadding="1" width="300" border="0">
														<TR>
															<TD>
																<asp:label id="Label5" runat="server" Width="150px" Font-Bold="True">Schilderversand an:</asp:label></TD>
															<TD></TD>
														</TR>
														<TR>
															<TD colSpan="2">
																<uc1:AddressSearchInputControl id="ctrlAddressSearchSchilder" runat="server"></uc1:AddressSearchInputControl></TD>
														</TR>
														<TR>
															<TD>
																<asp:label id="Label22" runat="server" Width="150px" Height="22px"> Auswahl:</asp:label></TD>
															<TD>
																<asp:dropdownlist id="drpSchilderversand" runat="server" Width="294px" AutoPostBack="True"></asp:dropdownlist></TD>
														</TR>
														<TR>
															<TD>
																<asp:label id="Label13" runat="server" Width="150px" Height="22px">Name:</asp:label></TD>
															<TD>
																<asp:TextBox id="txtSchilderversandName" runat="server" Width="292px"></asp:TextBox></TD>
														</TR>
														<TR>
															<TD>
																<asp:label id="Label15" runat="server" Width="150px" Height="22px">Straße, Hausnummer:</asp:label></TD>
															<TD>
																<asp:TextBox id="txtSchilderversandStraßeHausnr" runat="server" Width="292px"></asp:TextBox></TD>
														</TR>
														<TR>
															<TD>
																<asp:label id="Label19" runat="server" Width="150px" Height="22px">PLZ, Ort:</asp:label></TD>
															<TD>
																<asp:TextBox id="txtSchilderversandPLZOrt" runat="server" Width="292px"></asp:TextBox></TD>
														</TR>
													</TABLE>
												</asp:Panel></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 456px" width="456">Bemerkung:
											</TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 456px" width="456">
												<P><cc1:textareacontrol id="txtBemerkung" runat="server" Width="406px" Height="50px" MaxLength="256" TextMode="MultiLine"></cc1:textareacontrol></P>
											</TD>
											<TD></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD style="WIDTH: 144px" vAlign="top" width="144"></TD>
								<TD vAlign="top"></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 144px" vAlign="top" width="144"></TD>
								<TD vAlign="top">&nbsp;
									<asp:label id="lblError" runat="server" Width="770px" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 144px" vAlign="top" width="144"></TD>
								<TD vAlign="top">
									<TABLE id="Table2" cellSpacing="1" cellPadding="1" width="100%" border="0">
										<TR>
											<TD style="WIDTH: 324px">
												<P align="right"><asp:imagebutton id="cmdBack" runat="server" Width="73px" Height="34px" ImageUrl="../../../Images/arrowUeberfLeft.gif"></asp:imagebutton></P>
											</TD>
											<TD style="WIDTH: 79px"></TD>
											<TD><asp:imagebutton id="cmdRight1" runat="server" Width="73px" Height="34px" ImageUrl="../../../Images/arrowUeberfRight.gif"></asp:imagebutton></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 324px">&nbsp;</TD>
											<TD style="WIDTH: 79px"></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 324px"></TD>
											<TD style="WIDTH: 79px"><asp:label id="Label14" runat="server" Width="80px" Font-Bold="True" ForeColor="Red">*=Pflichtfeld</asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD style="WIDTH: 324px"></TD>
											<TD style="WIDTH: 79px"></TD>
											<TD></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD style="WIDTH: 144px" vAlign="top" width="144"></TD>
								<TD vAlign="top"></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</FORM>
	</BODY>
</HTML>
