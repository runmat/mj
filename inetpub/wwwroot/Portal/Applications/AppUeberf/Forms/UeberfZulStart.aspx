<%@ Register TagPrefix="uc1" TagName="AddressSearchInputControl" Src="../Controls/AddressSearchInputControl.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="UeberfZulStart.aspx.vb" Inherits="AppUeberf.UeberfZulStart" %>
<%@ Register TagPrefix="cc1" Namespace="AppUeberf.Controls" Assembly="AppUeberf" %>
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
			<TABLE id="Table4" width="100%" align="center">
				<TR>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</TR>
				<TR>
					<TD>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TBODY>
								<TR>
									<TD class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;(
										<asp:label id="lblPageTitle" runat="server">Auswahl</asp:label>)</TD>
								</TR>
								<TR>
					</TD>
					<TD vAlign="top" width="100%">
						<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD vAlign="top" align="left" width="144">
									<TABLE class="BorderLeftBottom" id="Table1" cellSpacing="0" cellPadding="5" width="400" bgColor="white" border="0">
										<TR>
											<TD class="TextLarge" vAlign="middle" noWrap width="144"></TD>
											<TD class="TextLarge" vAlign="middle" noWrap width="49"><asp:label id="Label4" runat="server" Width="150px">Referenz-Nr.:*</asp:label></TD>
											<TD vAlign="middle" noWrap width="295"><asp:textbox id="txtRef" runat="server" Width="261px" MaxLength="20"></asp:textbox></TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="middle" noWrap width="144"></TD>
											<TD class="TextLarge" vAlign="middle" noWrap width="49" colSpan="2"></TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="middle" noWrap width="144"></TD>
											<TD class="TextLarge" vAlign="middle" noWrap colSpan="2"><STRONG>Leasingnehmer
													<asp:Label id="lblLeasingnehmerKundennummer" runat="server" Visible="False"></asp:Label></STRONG></TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="middle" noWrap width="144"></TD>
											<TD class="TextLarge" vAlign="middle" noWrap colSpan="2">
												<uc1:AddressSearchInputControl id="ctrlAddressSearchInput" runat="server"></uc1:AddressSearchInputControl></TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="middle" noWrap width="144"></TD>
											<TD class="TextLarge" vAlign="middle" noWrap width="49"><asp:label id="Label19" runat="server" Height="22px" Width="150px">Auswahl:</asp:label></TD>
											<TD vAlign="middle" noWrap width="295">
												<asp:DropDownList id="drpLeasingnehmer" runat="server" Width="267px" AutoPostBack="True"></asp:DropDownList></TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="middle" noWrap width="144"></TD>
											<TD class="TextLarge" vAlign="middle" noWrap width="49"><asp:label id="Label13" runat="server" Height="22px" Width="150px">Leasingnehmer:*</asp:label></TD>
											<TD vAlign="middle" noWrap width="295"><asp:textbox id="txtLeasingnehmer" runat="server" Width="266px"></asp:textbox></TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="middle" noWrap width="144"></TD>
											<TD class="TextLarge" vAlign="middle" noWrap width="49">
												<asp:label id="Label6" runat="server" Width="204px" Height="22px">Leasingnehmer Straﬂe*/Hausnr.*:</asp:label></TD>
											<TD vAlign="middle" noWrap width="295">
												<asp:TextBox id="txtLeasingnehmerStrasse" runat="server" Width="228px"></asp:TextBox>
												<asp:TextBox id="txtLeasingnehmerHausnr" runat="server" Width="39px"></asp:TextBox></TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="middle" noWrap width="144"></TD>
											<TD class="TextLarge" vAlign="middle" noWrap width="49"><asp:label id="Label15" runat="server" Height="22px" Width="191px">Leasingnehmer-PLZ /-Ort:*</asp:label></TD>
											<TD vAlign="middle" noWrap width="295"><asp:textbox id="txtLeasingnehmerPLZ" runat="server" Width="88px" MaxLength="5"></asp:textbox>&nbsp;
												<asp:textbox id="txtLeasingnehmerOrt" runat="server" Width="169px"></asp:textbox></TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="middle" noWrap width="144"></TD>
											<TD class="TextLarge" vAlign="middle" noWrap width="49"></TD>
											<TD vAlign="middle" noWrap width="295"></TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="middle" noWrap width="144"></TD>
											<TD class="TextLarge" vAlign="middle" noWrap width="49"><asp:label id="lblFahrzeugdaten" runat="server" Width="105px" Font-Bold="True">Fahrzeugdaten</asp:label></TD>
											<TD vAlign="middle" noWrap width="295"></TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="middle" noWrap width="144"></TD>
											<TD class="TextLarge" vAlign="middle" noWrap width="49"><asp:label id="Label1" runat="server" Width="192px"> Fahrzeugtyp*</asp:label></TD>
											<TD vAlign="middle" noWrap width="295"><asp:textbox id="txtHerstTyp" runat="server" Width="266px" MaxLength="25"></asp:textbox></TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="middle" noWrap width="144"></TD>
											<TD class="TextLarge" vAlign="middle" noWrap width="49"><asp:label id="Label3" runat="server" Width="192px">Fgst.-Nummer:</asp:label></TD>
											<TD vAlign="middle" noWrap width="295"><asp:textbox id="txtVin" runat="server" Width="266px" MaxLength="17"></asp:textbox></TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="middle" noWrap width="144"></TD>
											<TD class="TextLarge" vAlign="middle" noWrap width="49"></TD>
											<TD vAlign="middle" noWrap width="295"></TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="middle" noWrap width="144"></TD>
											<TD class="TextLarge" vAlign="middle" noWrap width="49"><STRONG>Beauftragung:</STRONG></TD>
											<TD class="" vAlign="middle" noWrap width="295"><asp:checkbox id="chkZul" runat="server" AutoPostBack="True" Checked="True" Text="Zulassung"></asp:checkbox></TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="middle" noWrap width="144"></TD>
											<TD class="TextLarge" vAlign="middle" noWrap width="49"></TD>
											<TD vAlign="middle" noWrap width="295"><asp:checkbox id="chkUeberf" runat="server" AutoPostBack="True" Checked="True" Text="‹berf¸hrung"></asp:checkbox></TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="middle" noWrap width="144"></TD>
											<TD class="TextLarge" vAlign="middle" noWrap width="49"></TD>
											<TD class="" vAlign="middle" width="295"><P align="right"><asp:imagebutton id="cmdRight1" runat="server" Height="34px" ImageUrl="../../../Images/arrowUeberfRight.gif" Width="73px"></asp:imagebutton></P>
											</TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="middle" noWrap width="144"></TD>
											<TD class="TextLarge" vAlign="middle" noWrap width="49"></TD>
											<TD vAlign="middle" width="295"></TD>
										</TR>
										<TR>
											<TD class="TextLarge" vAlign="middle" noWrap width="144"></TD>
											<TD class="TextLarge" vAlign="middle" noWrap width="49" colSpan="2">
												<P align="left">
													<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False" Width="494px" Height="83px"></asp:label></P>
											</TD>
										</TR>
									</TABLE>
									<asp:Label id="lblLeasingnehmerAnsprechpartner" runat="server" Visible="False"></asp:Label>
									<asp:Label id="lblLeasingnehmerTelefon1" runat="server" Visible="False"></asp:Label>
									<asp:Label id="lblLeasingnehmerTelefon2" runat="server" Visible="False"></asp:Label>
								</TD>
							</TR>
						</TABLE>
						<P align="left">&nbsp;</P>
					</TD>
				</TR>
				<TR>
					<TD><!--#include File="../../../PageElements/Footer.html" --></TD>
				</TR>
			</TABLE>
			</TD></TR></TBODY></TABLE></form>
	</body>
</HTML>
