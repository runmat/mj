<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report66_2.aspx.vb" Inherits="AppSIXT.Report66_2" %>
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
											<TD class="TaskTitle" colSpan="4"><asp:hyperlink id="lnkExcel" runat="server" Target="_blank" Visible="False">Excelformat</asp:hyperlink>&nbsp;&nbsp;
												<asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Size="8pt" Font-Bold="True">rechte Maustaste => Ziel speichern unter...</asp:label></TD>
										</tr>
										<TR>
											<TD class="" colSpan="4"><asp:label id="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:label><asp:label id="lblInfo" runat="server" Font-Bold="True" DESIGNTIMEDRAGDROP="99" EnableViewState="False"></asp:label></TD>
										</TR>
										<TR>
											<TD noWrap width="100%" colSpan="2">&nbsp;<asp:linkbutton id="btnBack" runat="server" CssClass="StandardButtonTable">&#149;&nbsp;Zurück</asp:linkbutton></TD>
											<TD class="LabelExtraLarge" align="right"><asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
										</TR>
									</TABLE>
									<asp:datagrid id="DataGrid1" runat="server" Width="100%" AllowPaging="True" AllowSorting="True" bodyHeight="300" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" PageSize="50" BackColor="White" AutoGenerateColumns="False">
										<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
										<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
										<Columns>
											<asp:BoundColumn DataField="Typ" SortExpression="Typ" HeaderText="Typ"></asp:BoundColumn>
											<asp:BoundColumn DataField="Kennzeichen" SortExpression="Kennzeichen" HeaderText="Kennzeichen"></asp:BoundColumn>
											<asp:BoundColumn DataField="Fahrgestellnummer" SortExpression="Fahrgestellnummer" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
											<asp:BoundColumn DataField="Versandadresse" SortExpression="Versandadresse" HeaderText="Versandadresse"></asp:BoundColumn>
											<asp:BoundColumn DataField="Beauftragung" DataFormatString="{0:dd.MM.yy}" SortExpression="Beauftragung" HeaderText="Beauftragung"></asp:BoundColumn>
											<asp:BoundColumn DataField="Fehlertext" SortExpression="Fehlertext" HeaderText="Fehlertext"></asp:BoundColumn>
											<asp:BoundColumn DataField="Treuhandgeber" SortExpression="Treuhandgeber" HeaderText="Treuhandgeber"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="Flag_Briefversand" SortExpression="Flag_Briefversand" HeaderText="Briefversand"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="Flag_Schluesselversand" SortExpression="Flag_Schluesselversand" HeaderText="Schluesselversand"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="Status" SortExpression="Status" HeaderText="Status"></asp:BoundColumn>
											<asp:TemplateColumn HeaderText="L&#246;schen">
												<ItemTemplate>
													<asp:CheckBox id="cbxDelete" runat="server"></asp:CheckBox>
												</ItemTemplate>
											</asp:TemplateColumn>
										</Columns>
										<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
									</asp:datagrid></TD>
							</tr>
							<TR>
								<TD></TD>
								<TD>
									<P align="left">
										<asp:label id="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:label></P>
								</TD>
							</TR>
							<TR>
								<TD></TD>
								<TD>
									<P align="right"><asp:linkbutton id="btnDelete" runat="server" CssClass="StandardButtonTable">&#149;&nbspAbsenden</asp:linkbutton></P>
								</TD>
							</TR>
							<TR>
								<TD></TD>
								<TD>&nbsp;</TD>
							</TR>
							<TR>
								<TD></TD>
								<TD><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
				<TR id="ShowScript" runat="server" visible="False">
					<TD></TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
