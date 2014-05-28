<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change81_4.aspx.vb" Inherits="AppFMS.Change81_4" %>
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
								<td class="PageNavigation" colSpan="3">
									<asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Bestätigung)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table12" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle" width="150">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150">
												<P><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton"> &#149;&nbsp;Absenden</asp:linkbutton><U></U></P>
											</TD>
										</TR>
									</TABLE>
									<P align="right">&nbsp;</P>
								</TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top"><asp:hyperlink id="lnkFahrzeugsuche" runat="server" NavigateUrl="Change04.aspx" CssClass="TaskTitle">Fahrzeugsuche</asp:hyperlink>&nbsp;<asp:hyperlink id="lnkFahrzeugAuswahl" runat="server" NavigateUrl="Change04_2.aspx" CssClass="TaskTitle">Fahrzeugauswahl</asp:hyperlink>&nbsp;<asp:label id="lblAddress" runat="server" Visible="False"></asp:label><asp:label id="lblMaterialNummer" runat="server" Visible="False"></asp:label></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left" colSpan="3">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" border="0">
													<TR>
														<TD class="LabelExtraLarge" vAlign="top" align="left"><asp:label id="lblMessage" runat="server"></asp:label></TD>
													</TR>
												</TABLE>
												<TABLE id="Table7" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
													<TR>
														<TD class="StandardTableAlternate" vAlign="top">
															<P>Fahrzeuge</P>
															<P>&nbsp;</P>
															<P>&nbsp;</P>
														</TD>
														<TD class="StandardTableAlternate" vAlign="top" align="left" colSpan="2"><asp:datagrid id="DataGrid1" runat="server" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="250" Width="100%" AutoGenerateColumns="False" AllowSorting="True">
																<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
																<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
																<Columns>
																	<asp:BoundColumn Visible="False" DataField="MANDT" SortExpression="MANDT" HeaderText="MANDT"></asp:BoundColumn>
																	<asp:BoundColumn DataField="CHASSIS_NUM" SortExpression="CHASSIS_NUM" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
																	<asp:BoundColumn DataField="LICENSE_NUM" SortExpression="LICENSE_NUM" HeaderText="Kennzeichen"></asp:BoundColumn>
																	<asp:BoundColumn DataField="LIZNR" SortExpression="LIZNR" HeaderText="Auftragsnummer"></asp:BoundColumn>
																	<asp:BoundColumn DataField="TIDNR" SortExpression="TIDNR" HeaderText="KFZ-Briefnummer"></asp:BoundColumn>
																	<asp:BoundColumn Visible="False" DataField="ZZREFERENZ1" SortExpression="ZZREFERENZ1" HeaderText="Ordernummer"></asp:BoundColumn>
																	<asp:BoundColumn DataField="STATUS" SortExpression="STATUS" HeaderText="Status"></asp:BoundColumn>
																	<asp:TemplateColumn Visible="False" HeaderText="Anfordern">
																		<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Center"></ItemStyle>
																		<ItemTemplate>
																			<asp:CheckBox id="chk0001" runat="server" Enabled="False"></asp:CheckBox>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																</Columns>
																<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Wrap="False"></PagerStyle>
															</asp:datagrid></TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
										<TR>
											<TD vAlign="top" align="left">
												<P align="left"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></P>
											</TD>
										</TR>
										<TR>
											<TD colSpan="3"><!--#include File="../../../PageElements/Footer.html" -->
												<P align="left">&nbsp;</P>
											</TD>
										</TR>
									</TABLE>
								</td>
							</tr>
						</TABLE>
					</TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
