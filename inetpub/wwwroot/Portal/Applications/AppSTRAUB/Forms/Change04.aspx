<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change04.aspx.vb" Inherits="AppSTRAUB.Change04" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
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
			<table cellSpacing="0" cellPadding="2" width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Modell-Pflege)</asp:label></td>
							</TR>
							<TR>
								<TD class="TaskTitle" colSpan="2">&nbsp;</TD>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton"> &#149;&nbsp;Speichern</asp:linkbutton></TD>
										</TR>
									</TABLE>
									<asp:linkbutton id="cmdBack" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Zurück</asp:linkbutton>
								</TD>
								<td vAlign="top" align="left">
									<TABLE id="Table5" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left" colSpan="3" width="100%">
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<TR>
														<TD class="LabelExtraLarge" align="left" width="100%"><asp:label id="lblNoData" runat="server" Font-Bold="True" Visible="False"></asp:label>
														<TD noWrap align="right">
															<P align="right">&nbsp;
																<asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True" Height="14px"></asp:dropdownlist></P>
														</TD>
													</TR>
												</table>
												<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top" align="left" colSpan="3"><asp:datagrid id="DataGrid1" runat="server" BackColor="White" PageSize="50" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="400" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" Width="100%">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="EQUNR" SortExpression="Equipment" HeaderText="Equipment"></asp:BoundColumn>
														<asp:BoundColumn DataField="ZZHERST_TEXT" SortExpression="ZZHERST_TEXT" HeaderText="Hersteller"></asp:BoundColumn>
														<asp:BoundColumn DataField="ZZKLARTEXT_TYP" SortExpression="ZZKLARTEXT_TYP" HeaderText="Hersteller Typ"></asp:BoundColumn>
														<asp:BoundColumn DataField="ZZFAHRG" SortExpression="ZZFAHRG" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="ZZBEZEI" SortExpression="ZZBEZEI" HeaderText="Bezeichnung"></asp:BoundColumn>
														<asp:BoundColumn DataField="ZZHUBRAUM" SortExpression="ZZHUBRAUM" HeaderText="Hubraum"></asp:BoundColumn>
														<asp:BoundColumn DataField="ZZAUSF" SortExpression="ZZAUSF" HeaderText="Ausf&#252;hrung"></asp:BoundColumn>
														<asp:BoundColumn DataField="ZZNAVI" SortExpression="ZZNAVI" HeaderText="Naiv"></asp:BoundColumn>
														<asp:BoundColumn DataField="ZZSIPP3" SortExpression="ZZSIPP3" HeaderText="Schaltung"></asp:BoundColumn>
														<asp:BoundColumn DataField="ZZNENNLEISTUNG" SortExpression="ZZNENNLEISTUNG" HeaderText="KW Zahl"></asp:BoundColumn>
														<asp:BoundColumn DataField="ZZKRAFTSTOFF_TXT" SortExpression="ZZKRAFTSTOFF_TXT" HeaderText="Kraftstoffart"></asp:BoundColumn>
														<asp:TemplateColumn HeaderText="Typ-Code">
															<ItemTemplate>
																<asp:TextBox id="txtModell" runat="server" CssClass="DropDownStyle" Width="122px"></asp:TextBox>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="Model-Code">
															<ItemTemplate>
																<asp:TextBox id="txtModelcode" runat="server" CssClass="DropDownStyle" Width="100px"></asp:TextBox>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="Status" SortExpression="Status" HeaderText="Status"></asp:BoundColumn>
													</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></TD>
										</TR>
									</TABLE>
								</td>
							</tr>
							<TR>
								<TD width="120">&nbsp;</TD>
								<TD><FONT size="1"></FONT></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
