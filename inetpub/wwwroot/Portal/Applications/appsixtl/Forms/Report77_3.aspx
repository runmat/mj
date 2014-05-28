<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report77_3.aspx.vb" Inherits="AppSIXTL.Report01_2" %>
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
				<TR>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="PageNavigation" colSpan="2">
									<uc1:Header id="ucHeader" runat="server" Visible="False"></uc1:Header></TD>
							</TR>
							<TR>
								<td class="PageNavigation" colSpan="2">
									<asp:label id="lblHead" runat="server"></asp:label>
									<asp:label id="lblPageTitle" runat="server"> (Statusdaten)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top">
								</TD>
								<TD vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td class="TaskTitle" colSpan="2" align="right">&nbsp;&nbsp;
												<asp:hyperlink id="lnkFahrzeugsuche" runat="server" CssClass="TaskTitle" NavigateUrl="javascript:window.close()">Fenster schließen</asp:hyperlink></td>
										</tr>
										<TR>
											<TD align="right" colSpan="2">
											</TD>
										<TR>
											<TD class="" colSpan="1" width="100%"><asp:label id="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:label></TD>
											<TD class="LabelExtraLarge" align="right"></TD>
										</TR>
										<TR>
											<TD colSpan="2"><asp:datagrid id="DataGrid1" runat="server" BackColor="White" PageSize="50" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="300" AllowSorting="True" AllowPaging="True" Width="100%" AutoGenerateColumns="False">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn DataField="MNCOD" SortExpression="MNCOD" HeaderText="Aktionscode"></asp:BoundColumn>
														<asp:BoundColumn DataField="MATXT" SortExpression="MATXT" HeaderText="Vorgang"></asp:BoundColumn>
														<asp:BoundColumn DataField="PSTER" SortExpression="PSTER" HeaderText="Statusdatum"></asp:BoundColumn>
														<asp:BoundColumn DataField="AEDAT" SortExpression="AEDAT" HeaderText="&#196;nderungs-&lt;br&gt;datum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
														<asp:BoundColumn DataField="AEZEIT" SortExpression="AEZEIT" HeaderText="&#196;nderungs-&lt;br&gt;Zeit" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
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
		</form>
	</body>
</HTML>
