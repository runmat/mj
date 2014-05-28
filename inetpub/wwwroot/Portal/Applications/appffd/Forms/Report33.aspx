<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report33.aspx.vb" Inherits="AppFFD.Report33" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:Styles id="ucStyles" runat="server"></uc1:Styles>
	</HEAD>
	<body topmargin="0" leftmargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<table width="100%" align="center">
				<tr>
					<td><uc1:Header id="ucHeader" runat="server"></uc1:Header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2">
									<asp:Label id="lblHead" runat="server"></asp:Label>&nbsp;
									<asp:label id="lblPageTitle" runat="server"> (Anzeige Report)</asp:label></td>
							</TR>
							<TR>
								<TD vAlign="top" width="120" class="TaskTitle">&nbsp;</TD>
								<TD vAlign="top" class="TaskTitle"><asp:hyperlink id="lnkKreditlimit" runat="server" CssClass="TaskTitle" NavigateUrl="Report01.aspx" Visible="False">Zusammenstellung von Abfragekriterien</asp:hyperlink>&nbsp;</TD>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TextHeader" width="150"></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150">
												<asp:LinkButton id="cmdSave" runat="server" CssClass="StandardButton" Enabled="False">OK</asp:LinkButton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150">
												<asp:LinkButton id="cmdPrint" runat="server" CssClass="StandardButton" Visible="False">Drucken</asp:LinkButton></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0" bgColor="white">
										<tr>
											<td><BR>
												Sehr geehrte Damen und Herren,<BR>
												<BR>
												Für die unten aufgeführten Kfz-Briefe konnten wir leider noch keinen 
												Wiedereingang (temporärer Versand) bzw. Zahlungseingang nach 6 Arbeitstagen ab 
												dem Tag des endgültigen Versandes verzeichnen.<BR>
												<BR>
												Sollte sich diese Erinnerung mit der Rücksendung bzw. mit der Bezahlung 
												überschnitten haben, betrachten Sie bitte diese Meldung als gegenstandslos.<BR>
												<BR>
											</td>
										</tr>
										<TR>
											<TD>
												<table cellpadding="0" cellspacing="0" width="100%" border="0">
													<TBODY>
														<tr>
															<td class="LabelExtraLarge">
																<asp:HyperLink id="lnkExcel" runat="server" Visible="False" Target="_blank">Excelformat</asp:HyperLink>&nbsp;&nbsp;
																<asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Bold="True" Font-Size="8pt">rechte Maustaste => Ziel speichern unter...</asp:label></td>
											</TD>
											<td align="right"><asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist></td>
										</TR>
									</TABLE>
								</td>
							<TR>
								<TD class="LabelExtraLarge">
									<asp:Label id="lblNoData" runat="server" Visible="False"></asp:Label></TD>
							</TR>
							<TR>
								<TD><asp:datagrid id="DataGrid1" runat="server" Width="100%" AllowPaging="True" AllowSorting="True" bodyHeight="400" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" PageSize="50">
										<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
										<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
										<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False"></PagerStyle>
									</asp:datagrid></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
				<tr>
					<td width="100"></td>
					<td><asp:label id="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:label></td>
				</tr>
				<tr>
					<td width="100"></td>
					<td><!--#include File="../../../PageElements/Footer.html" --></td>
				</tr>
			</table>
			</TD></TR>
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
			</TBODY></TABLE>
		</form>
	</body>
</HTML>
