<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report40_02.aspx.vb" Inherits="CKG.Components.ComCommon.Report40_02"%>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
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
								<td></td>
								<td><asp:linkbutton id="cmdBack" runat="server" Visible="False" CssClass="StandardButton" Enabled="False">&#149;&nbsp;Zurück</asp:linkbutton></td>
							</tr>
							<tr>
								<TD vAlign="top"></TD>
								<TD vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td class="TaskTitle" colSpan="2">
												<asp:LinkButton id="lnkCreateExcel" runat="server">Excelformat</asp:LinkButton>
												<asp:HyperLink id="lnkShowCSV" runat="server" Visible="False" Target="_blank">CSV-Datei</asp:HyperLink>&nbsp;
												<asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Size="8pt" Font-Bold="True">rechte Maustaste => Ziel speichern unter...</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:hyperlink id="lnkBack" runat="server" NavigateUrl="javascript:history.back()">zurück</asp:hyperlink></td>
										</tr>
										<TR>
											<TD align="right" colSpan="2"><asp:linkbutton id="cmdSave" runat="server" Visible="False" CssClass="StandardButton">Sichern</asp:linkbutton></TD>
										<TR>
											<TD class="" width="100%" colSpan="1"><asp:label id="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:label></TD>
											<TD class="LabelExtraLarge" align="right"><asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
										</TR>
										<TR>
											<TD colSpan="2"><asp:datagrid id="DataGrid1" runat="server" Width="100%" AllowPaging="True" AllowSorting="True" bodyHeight="300" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" PageSize="50" BackColor="White" AutoGenerateColumns="False">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn DataField="H&#228;ndlername" SortExpression="H&#228;ndlername" HeaderText="H&#228;ndlername"></asp:BoundColumn>
														<asp:BoundColumn DataField="ZB2Nummer" SortExpression="ZB2Nummer" HeaderText="ZB2-Nummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Fahrgestellnummer" SortExpression="Fahrgestellnummer" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Modell" SortExpression="Modell" HeaderText="Modell"></asp:BoundColumn>
														<asp:BoundColumn DataField="HEK Nummer" SortExpression="HEK Nummer" HeaderText="HEK Nummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Eingangsdatum" SortExpression="Eingangsdatum" DataFormatString="{0:dd.MM.yyyy}" HeaderText="Eingangsdatum"></asp:BoundColumn>
														<asp:TemplateColumn HeaderText="col_Loeschen">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<HeaderTemplate>
																<asp:LinkButton id="col_Loeschen" Runat="server"></asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<P align="center">
																	<asp:LinkButton id="lb_Loeschen" runat="server" CssClass="StandardButtonSmall" Visible="True" Text="Löschen" commandname="Delete" CausesValidation="True"></asp:LinkButton></P>
															</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></TD>
										</TR>
									</TABLE>
									<asp:label id="lbl_Info" runat="server" Font-Bold="True" EnableViewState="False"></asp:label></TD>
							</tr>
							<tr>
								<td></td>
								<td><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
							</tr>
							<tr>
								<td></td>
								<td><!--#include File="../../../PageElements/Footer.html" -->
									<asp:Label id="lblHidden" runat="server" Visible="False" Width="39px"></asp:Label></td>
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
		</form>
	</body>
</HTML>
