<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report201_2.aspx.vb" Inherits="AppSIXTL.Report201_2" %>
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
			<table width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Anzeige Report)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top"></TD>
								<TD vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td class="TaskTitle" colSpan="2"><asp:hyperlink id="lnkExcel" runat="server" Target="_blank" Visible="False">Excelformat</asp:hyperlink>&nbsp;&nbsp;
												<asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Size="8pt" Font-Bold="True">rechte Maustaste => Ziel speichern unter...</asp:label><asp:hyperlink id="lnkKreditlimit" runat="server" Target="_blank" Visible="False">Zusammenstellung von Abfragekriterien</asp:hyperlink></td>
										</tr>
										<TR>
											<TD align="right" colSpan="2"><asp:linkbutton id="cmdSave" runat="server" Visible="False" CssClass="StandardButton">Sichern</asp:linkbutton></TD>
										<TR>
											<TD class="" width="100%" colSpan="1"><asp:label id="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:label></TD>
											<TD class="LabelExtraLarge" align="right"><asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
										</TR>
										<TR>
											<TD colSpan="2"><asp:datagrid id="DataGrid1" runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True" bodyHeight="300" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" PageSize="50" BackColor="White">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn DataField="Leasingvertragsnr" SortExpression="Leasingvertragsnr" HeaderText="Leasingvertrags-Nr."></asp:BoundColumn>
														<asp:BoundColumn Visible="False" HeaderText="Eingang Carportliste">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="Kfz-Kennzeichen" SortExpression="Kfz-Kennzeichen" HeaderText="Kfz-Kennzeichen">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="Eingang Physisch" SortExpression="Eingang Physisch" HeaderText="Eingang Physisch" DataFormatString="{0:dd.MM.yy}">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="Abmeldeauftrag" SortExpression="Abmeldeauftrag" HeaderText="Abmeldeauftrag">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="Anzahl Schilder" SortExpression="Anzahl Schilder" HeaderText="Anzahl Schilder">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:TemplateColumn SortExpression="Anzahl Schilder" HeaderText="Form.">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:LinkButton id=LinkButton1 runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Anzahl Schilder")<>"2" %>' CssClass="StandardButtonTable" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.Kfz-Kennzeichen") %>' Text="Zeigen" CausesValidation="False" CommandName="Schilder">Zeigen</asp:LinkButton>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="Kfz-Schein" SortExpression="Kfz-Schein" HeaderText="Kfz-Schein">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:TemplateColumn SortExpression="Kfz-Schein" HeaderText="Form.">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:LinkButton id=LinkButton2 runat="server" Visible='<%# NOT DataBinder.Eval(Container, "DataItem.Kfz-Schein") IS NOTHING AndAlso NOT (CStr(DataBinder.Eval(Container, "DataItem.Kfz-Schein"))="X") %>' CssClass="StandardButtonTable" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.Kfz-Kennzeichen") %>' Text="Zeige" CausesValidation="False" CommandName="Schein">Zeigen</asp:LinkButton>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="Kfz-Brief" SortExpression="Kfz-Brief" HeaderText="Kfz Brief">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="Fahrgestellnummer" SortExpression="Fahrgestellnummer" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
													</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></TD>
										</TR>
									</TABLE>
									<asp:label id="lblInfo" runat="server" Font-Bold="True" EnableViewState="False"></asp:label></TD>
							</tr>
							<tr>
								<td></td>
								<td><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
							</tr>
							<tr>
								<td></td>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</tr>
						</TABLE>
					</td>
				</tr>
				<TR id="ShowScript" runat="server" visible="False">
					<TD>
						<script language="Javascript">
						<!-- //
						function FreigebenConfirm(Fahrgest,Vertrag,BriefNr,Kennzeichen) {
						var Check = window.confirm("Wollen Sie für dieses Fahrzeug wirklich den Status 'Bezahlt' setzen?\t\n\tFahrgestellnr.\t" + Fahrgest + "\t\n\tVertrag\t\t" + Vertrag + "\t\n\tKfz-Briefnr.\t" + BriefNr + "\n\tKfz-Kennzeichen\t" + Kennzeichen);
						return (Check);
						}
						//-->
						</script>
					</TD>
				</TR>
			</table>
			<asp:Literal id="Literal1" runat="server"></asp:Literal>
		</form>
	</body>
</HTML>
