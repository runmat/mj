<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change80_5.aspx.vb" Inherits="AppALD.Change80_5" %>
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
			<table cellSpacing="0" cellPadding="2" width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Fahrzeugauswahl)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton"> &#149;&nbsp;Alle Freigeben</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;<asp:hyperlink id="lnkKreditlimit" runat="server" CssClass="TaskTitle" NavigateUrl="Change04.aspx" Visible="False">Fahrzeugsuche</asp:hyperlink></TD>
										</TR>
									</TABLE>
									<TABLE id="Table5" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left" colSpan="3" height="41">
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<TR>
														<TD class="LabelExtraLarge" align="left" width="618" height="9"><asp:label id="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:label>
														<TD noWrap align="right" height="9">
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
													<HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:TemplateColumn SortExpression="MANDT" HeaderText="Auswahl">
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox id=chkSelect runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.MANDT")="99" %>' Enabled='<%# typeof (DataBinder.Eval(Container, "DataItem.Status")) is System.DBNull %>'>
																</asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn Visible="False" DataField="id" SortExpression="id" HeaderText="ID"></asp:BoundColumn>
														<asp:BoundColumn DataField="Erstellt" SortExpression="Erstellt" HeaderText="Erstellt"></asp:BoundColumn>
														<asp:BoundColumn DataField="Benutzer" SortExpression="Benutzer" HeaderText="Benutzer"></asp:BoundColumn>
														<asp:BoundColumn DataField="m_strSucheFahrgestellNr" SortExpression="m_strSucheFahrgestellNr" HeaderText="Fahrg.Nr."></asp:BoundColumn>
														<asp:BoundColumn DataField="m_liznr" SortExpression="m_liznr" HeaderText="LV-Nr."></asp:BoundColumn>
														<asp:BoundColumn DataField="m_versandadrtext" SortExpression="m_versandadrtext" HeaderText="Versandadr."></asp:BoundColumn>
														<asp:BoundColumn DataField="VersandartShow" SortExpression="VersandartShow" HeaderText="Versand"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="m_abckz" HeaderText="Versandart">
															<ItemTemplate>
																<asp:Label id=Label1 runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.m_abckz")="1" OR DataBinder.Eval(Container, "DataItem.m_abckz")="3" %>' ForeColor="LimeGreen">temporär</asp:Label>
																<asp:Literal id=Literal1 runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.m_abckz")="3" %>' Text="<br>-><br>">
																</asp:Literal>
																<asp:Label id=Label2 runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.m_abckz")="2" OR DataBinder.Eval(Container, "DataItem.m_abckz")="3" %>' ForeColor="Red">endgültig</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="Status" SortExpression="Status" HeaderText="Status"></asp:BoundColumn>
														<asp:TemplateColumn>
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:LinkButton id=btnFreigeben runat="server" CssClass="StandardButtonTable" Width="100px" Enabled='<%# typeof (DataBinder.Eval(Container, "DataItem.Status")) is System.DBNull %>' CommandName="Freigeben">Freigeben</asp:LinkButton>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn>
															<ItemTemplate>
																<asp:LinkButton id=LinkButton3 runat="server" CssClass="StandardButtonTable" Visible='<%# typeof (DataBinder.Eval(Container, "DataItem.Status")) is System.DBNull or 1=1 %>' Width="100px" Enabled='<%# (typeof (DataBinder.Eval(Container, "DataItem.Status")) is System.DBNull) OrElse (NOT DataBinder.Eval(Container, "DataItem.Status") = "Vorgang OK") %>' CommandName="delete">Storno</asp:LinkButton>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn Visible="False" DataField="m_strHaendlernummer" SortExpression="m_strHaendlernummer" HeaderText="m_strHaendlernummer"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="m_strHalterNummer" SortExpression="m_strHalterNummer" HeaderText="m_strHalterNummer"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="m_strStandortNummer" SortExpression="m_strStandortNummer" HeaderText="m_strStandortNummer"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="m_strZielFirma" SortExpression="m_strZielFirma" HeaderText="m_strZielFirma"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="m_strZielFirma2" SortExpression="m_strZielFirma2" HeaderText="m_strZielFirma2"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="m_strZielStrasse" SortExpression="m_strZielStrasse" HeaderText="m_strZielStrasse"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="m_strZielHNr" SortExpression="m_strZielHNr" HeaderText="m_strZielHNr"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="m_strZielPLZ" SortExpression="m_strZielPLZ" HeaderText="m_strZielPLZ"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="m_strZielOrt" SortExpression="m_strZielOrt" HeaderText="m_strZielOrt"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="m_strZielLand" SortExpression="m_strZielLand" HeaderText="m_strZielLand"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="m_strAuf" SortExpression="m_strAuf" HeaderText="m_strAuf"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="m_strBetreff" SortExpression="m_strBetreff" HeaderText="m_strBetreff"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="m_blnALDZulassung" SortExpression="m_blnALDZulassung" HeaderText="m_blnALDZulassung"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="m_strSucheFahrgestellNr" SortExpression="m_strSucheFahrgestellNr" HeaderText="m_strSucheFahrgestellNr"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="m_strSucheKennzeichen" SortExpression="m_strSucheKennzeichen" HeaderText="m_strSucheKennzeichen"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="m_strSucheLeasingvertragsNr" SortExpression="m_strSucheLeasingvertragsNr" HeaderText="m_strSucheLeasingvertragsNr"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="m_kbanr" SortExpression="m_kbanr" HeaderText="m_kbanr"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="m_zulkz" SortExpression="m_zulkz" HeaderText="m_zulkz"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="m_Fahrzeuge" SortExpression="m_Fahrzeuge" HeaderText="m_Fahrzeuge"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="m_versandadr_ZE" SortExpression="m_versandadr_ZE" HeaderText="m_versandadr_ZE"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="m_versandadr_ZS" SortExpression="m_versandadr_ZS" HeaderText="m_versandadr_ZS"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="m_versandadrtext" SortExpression="m_versandadrtext" HeaderText="m_versandadrtext"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="m_versicherung" SortExpression="m_versicherung" HeaderText="m_versicherung"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="m_material" SortExpression="m_material" HeaderText="m_material"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="m_schein" SortExpression="m_schein" HeaderText="m_schein"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="m_abckz" SortExpression="m_abckz" HeaderText="m_abckz"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="m_equ" SortExpression="m_equ" HeaderText="m_equ"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="m_kennz" SortExpression="m_kennz" HeaderText="m_kennz"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="m_tidnr" SortExpression="m_tidnr" HeaderText="m_tidnr"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="m_liznr" SortExpression="m_liznr" HeaderText="m_liznr"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="m_versgrund" SortExpression="m_versgrund" HeaderText="m_versgrund"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="m_versgrundText" SortExpression="m_versgrundText" HeaderText="m_versgrundText"></asp:BoundColumn>
													</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></TD>
										</TR>
									</TABLE>
								</td>
							</tr>
							<TR>
								<TD width="120">&nbsp;</TD>
								<TD></TD>
							</TR>
							<tr>
								<td colSpan="3"><!--#include File="../../../PageElements/Footer.html" --><br>
								</td>
							</tr>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
