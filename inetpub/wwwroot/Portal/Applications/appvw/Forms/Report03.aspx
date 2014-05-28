<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report03.aspx.vb" Inherits="AppVW.Report03" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta name="vs_showGrid" content="True">
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
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Ausgabe)</asp:label></td>
							</TR>
							<TR>
								<TD class="TaskTitle" colSpan="2">&nbsp;&nbsp;&nbsp;&nbsp;</TD>
							</TR>
							<tr>
								<TD vAlign="top" width="133">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="middle" width="100%"><asp:linkbutton id="cmdForward" runat="server" CssClass="StandardButton"> &#149;&nbsp;Abrufen</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="100%"></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td colSpan="3" height="21"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
											<TD height="21"></TD>
										</tr>
										<TR>
											<TD height="18" width="20">
												<P align="left"><STRONG>Mahnstufe:
												</P>
												</STRONG>
											</TD>
											<TD height="18">
												<P align="left">
													<asp:RadioButtonList id="rdbMahnstufe" runat="server" Width="202px" RepeatDirection="Horizontal" CssClass="tableMain" Height="9px">
														<asp:ListItem Value="0">0</asp:ListItem>
														<asp:ListItem Value="1">1</asp:ListItem>
														<asp:ListItem Value="2">2</asp:ListItem>
														<asp:ListItem Value="3" Selected="True">3</asp:ListItem>
													</asp:RadioButtonList></P>
											</TD>
											<TD colSpan="3" height="18"></TD>
											<TD height="18"></TD>
										</TR>
										<TR>
											<TD class="" width="100%" colSpan="3"><strong>&nbsp;<asp:label id="lblNoData" runat="server"></asp:label></strong></TD>
											<TD>
												<P align="right"><asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True" Visible="False"></asp:dropdownlist></P>
											</TD>
										</TR>
									</TABLE>
									<TABLE id="Table5" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD colSpan="2">
												<asp:datagrid id="DataGrid1" runat="server" Width="100%" AllowPaging="True" AllowSorting="True" bodyHeight="400" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" PageSize="50" BackColor="White" AutoGenerateColumns="False" Visible="False">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn DataField="LIZNR" SortExpression="LIZNR" HeaderText="IKZ-Nummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="CHASSIS_NUM" SortExpression="CHASSIS_NUM" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="LICENSE_NUM" SortExpression="LICENSE_NUM" HeaderText="Kennzeichen"></asp:BoundColumn>
														<asp:BoundColumn DataField="REPLA_DATE" SortExpression="REPLA_DATE" HeaderText="Zulassungsdatum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
														<asp:BoundColumn DataField="ZZMADAT" SortExpression="ZZMADAT" HeaderText="Mahndatum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
														<asp:BoundColumn DataField="ZZMAHNS" SortExpression="ZZMAHNS" HeaderText="Mahnstufe"></asp:BoundColumn>
														<asp:BoundColumn DataField="ZZVERZG" SortExpression="ZZVERZG" HeaderText="Verzögerungsgrund"></asp:BoundColumn>
														<asp:BoundColumn DataField="NAME1" SortExpression="NAME1" HeaderText="Name"></asp:BoundColumn>
														<asp:BoundColumn DataField="STREET" SortExpression="STREET" HeaderText="Strasse"></asp:BoundColumn>
														<asp:BoundColumn DataField="POST_CODE1" SortExpression="POST_CODE1" HeaderText="PLZ"></asp:BoundColumn>
														<asp:BoundColumn DataField="CITY1" SortExpression="CITY1" HeaderText="Ort"></asp:BoundColumn>
														<asp:BoundColumn DataField="TEL_NUMBER" SortExpression="TEL_NUMBER" HeaderText="Tel.-Nr."></asp:BoundColumn>
													</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></TD>
										</TR>
										<tr>
											<td vAlign="top" align="left" colSpan="3"></td>
										</tr>
									</TABLE>
								</td>
							</tr>
							<TR>
								<td width="133">&nbsp;</td>
								<td></td>
							</TR>
							<TR>
								<td width="133">&nbsp;</td>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</TR>
							<TR id="ShowScript" runat="server">
								<td width="133">&nbsp;</td>
								<td>
									<script language="JavaScript">
										<!-- //
										window.document.Form1.elements[window.document.Form1.length-1].focus();
										//-->
									</script>
								</td>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
