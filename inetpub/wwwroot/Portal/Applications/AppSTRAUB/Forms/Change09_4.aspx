<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change09_4.aspx.vb" Inherits="AppSTRAUB.Change09_4" %>
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
					<td colSpan="3"><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<TR>
					<TD vAlign="top" align="left" colSpan="3">
						<TABLE id="Table10" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="3"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Best‰tigung)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table12" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton">&#149;&nbsp;Absenden</asp:linkbutton></TD>
										</TR>
									</TABLE>
									&nbsp;
								</TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top"><asp:hyperlink id="lnkFahrzeugsuche" runat="server" CssClass="TaskTitle" NavigateUrl="Change09.aspx">Fahrzeugsuche</asp:hyperlink>&nbsp;<asp:hyperlink id="lnkFahrzeugAuswahl" runat="server" CssClass="TaskTitle" NavigateUrl="Change09_2.aspx" Visible="False">Fahrzeugauswahl</asp:hyperlink>&nbsp;<asp:hyperlink id="lnkAdressAuswahl" runat="server" CssClass="TaskTitle" NavigateUrl="Change09_3.aspx" Visible="False">Adressauswahl</asp:hyperlink></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td vAlign="top" align="left" colSpan="3"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
										</tr>
										<tr>
											<td vAlign="top" align="left" colSpan="3">
												<TABLE class="BorderFull" id="Table7" cellSpacing="0" cellPadding="3" bgColor="white" border="0">
													<TR>
														<TD vAlign="top" width="13">&nbsp;</TD>
														<TD vAlign="top" align="left" colSpan="2"></TD>
													</TR>
													<TR class="FadeOutOne">
														<TD class="" vAlign="top" width="13"><U>Versandart:</U></TD>
														<TD class="" vAlign="top" align="left" colSpan="2" rowSpan="1"><asp:label id="lblMaterialNummer" runat="server" Visible="False"></asp:label></TD>
													</TR>
													<TR>
														<TD class="" vAlign="top" width="13">&nbsp;</TD>
														<TD class="" vAlign="top" align="left" colSpan="2">ï&nbsp;<asp:label id="lblVersandart" runat="server" CssClass="TextBoxStyleLarge"></asp:label></TD>
													</TR>
													<TR class="FadeOutOne">
														<TD vAlign="top" width="13"><U>Versandadresse:</U></TD>
														<TD vAlign="top" align="left" colSpan="2"></TD>
													</TR>
													<TR>
														<TD class="" vAlign="top" width="13"></TD>
														<TD class="" vAlign="top" align="left" colSpan="2">
															<TABLE id="Table1" cellSpacing="1" cellPadding="1" border="0">
																<TR>
																	<TD>Name
																	</TD>
																	<TD>&nbsp;</TD>
																	<TD><asp:textbox id="txtName1" runat="server" CssClass="TextBoxStyleLarge" Width="200px" Enabled="False" ToolTip="Max. 128 Zeichen" MaxLength="128"></asp:textbox></TD>
																	<TD><FONT color="red"></FONT></TD>
																</TR>
																<TR>
																	<TD>Name 2</TD>
																	<TD></TD>
																	<TD><asp:textbox id="txtName2" tabIndex="1" runat="server" CssClass="TextBoxStyleLarge" Width="200px" Enabled="False" ToolTip="Max. 128 Zeichen" MaxLength="128"></asp:textbox></TD>
																	<TD></TD>
																</TR>
																<TR>
																	<TD>Straﬂe / Nr</TD>
																	<TD></TD>
																	<TD><asp:textbox id="txtStrasse" tabIndex="2" runat="server" CssClass="TextBoxStyleLarge" Width="200px" Enabled="False" ToolTip="Max. 128 Zeichen" MaxLength="128"></asp:textbox></TD>
																	<TD><asp:textbox id="txtNummer" tabIndex="3" runat="server" CssClass="TextBoxStyleLarge" Width="50px" Enabled="False" ToolTip="Max. 10 Zeichen" MaxLength="10"></asp:textbox></TD>
																</TR>
																<TR>
																	<TD>Postleitzahl</TD>
																	<TD></TD>
																	<TD><asp:textbox id="txtPLZ" tabIndex="4" runat="server" CssClass="TextBoxStyleLarge" Width="200px" Enabled="False" ToolTip="Max. 10 Zeichen" MaxLength="10"></asp:textbox></TD>
																	<TD><FONT color="red"></FONT></TD>
																</TR>
																<TR>
																	<TD>Ort</TD>
																	<TD></TD>
																	<TD><asp:textbox id="txtOrt" tabIndex="5" runat="server" CssClass="TextBoxStyleLarge" Width="200px" Enabled="False" ToolTip="Max. 128 Zeichen" MaxLength="128"></asp:textbox></TD>
																	<TD><FONT color="red"></FONT></TD>
																</TR>
															</TABLE>
														</TD>
													</TR>
												</TABLE>
												<strong>
													<asp:label id="lblMessage" runat="server"></asp:label></strong></td>
										</tr>
										<TR class="FadeOutOne">
											<TD vAlign="top" align="left" colSpan="3">
												<TABLE id="Table2" cellSpacing="1" cellPadding="3" border="0">
													<TR>
														<TD><strong><U>Vorg‰nge:</U></strong></TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
										<tr>
											<td colSpan="3"><asp:datagrid id="DataGrid1" runat="server" AllowSorting="True" AutoGenerateColumns="False" Width="100%" bodyHeight="200" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn DataField="EQUNR" SortExpression="EQUNR" HeaderText="Equipmentnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="EQTYP" SortExpression="EQTYP" HeaderText="Equipmenttyp"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="MANDT" SortExpression="MANDT" HeaderText="MANDT"></asp:BoundColumn>
														<asp:BoundColumn DataField="ZZFAHRG" SortExpression="ZZFAHRG" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="LICENSE_NUM" SortExpression="LICENSE_NUM" HeaderText="Kennzeichen"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="TIDNR" SortExpression="TIDNR" HeaderText="Briefnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="LIZNR" SortExpression="LIZNR" HeaderText="Referenznummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="TEXT200" SortExpression="TEXT200" HeaderText="Referenz"></asp:BoundColumn>
														<asp:BoundColumn DataField="COMMENT" HeaderText="Status"></asp:BoundColumn>
														<asp:TemplateColumn HeaderText="Standard&lt;br&gt;tempor&#228;r">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox id="chk0001" runat="server" Enabled="False"></asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="Standard&lt;br&gt;endg&#252;ltig">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox id="chk0002" runat="server" Enabled="False"></asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Wrap="False"></PagerStyle>
												</asp:datagrid></td>
										</tr>
									</TABLE>
								</td>
								<!--#include File="../../../PageElements/Footer.html" --></tr>
						</TABLE>
					</TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
