<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report12_2.aspx.vb" Inherits="AppEC.Report12_2" %>
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
								<td vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD align="left" colSpan="3">
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<TBODY>
														<tr>
															<td class="TaskTitle"><asp:hyperlink id="lnkKreditlimit" runat="server" CssClass="TaskTitle" Visible="False" NavigateUrl="Report01.aspx"> &#171;&nbsp;Zurück</asp:hyperlink>&nbsp;•&nbsp;<asp:hyperlink id="lnkExcel" runat="server" CssClass="TaskTitle" Visible="False" Target="_blank">Excelformat</asp:hyperlink>&nbsp;
																<asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Size="8pt" Font-Bold="True">rechte Maustaste => Ziel speichern unter...</asp:label></td>
											</TD>
										</TR>
									</TABLE>
									<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label>
								</td>
								<TD align="right"></TD>
							<TR>
								<TD align="left" colSpan="3">&nbsp;</TD>
								<TD align="right"></TD>
							</TR>
							<TR>
								<TD class="TextExtraLarge"><asp:label id="lblNoData" runat="server" Visible="False"></asp:label></TD>
								<TD align="right" colSpan="2">
									<asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True" Visible="False"></asp:dropdownlist></TD>
							</TR>
							<TR>
								<TD colSpan="3"><asp:datagrid id="DataGrid1" runat="server" BackColor="White" AutoGenerateColumns="False" PageSize="50" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="400" AllowPaging="True" Width="100%">
										<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
										<Columns>
											<asp:BoundColumn DataField="DatumZulassung" SortExpression="DatumZulassung" HeaderText="Zulassungsdatum" DataFormatString="{0:dd.MM.yyyy}">
												<ItemStyle VerticalAlign="Top"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="PDINummer" SortExpression="PDINummer" HeaderText="PDINummer">
												<ItemStyle VerticalAlign="Top"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="PDI" SortExpression="PDI" HeaderText="PDI">
												<ItemStyle VerticalAlign="Top"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="Modellcode" SortExpression="Modellcode" HeaderText="Modellcode">
												<ItemStyle VerticalAlign="Top"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="Modell" SortExpression="Modell" HeaderText="Modell">
												<ItemStyle VerticalAlign="Top"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="Hersteller" SortExpression="Hersteller" HeaderText="Hersteller">
												<ItemStyle VerticalAlign="Top"></ItemStyle>
											</asp:BoundColumn>
											<asp:TemplateColumn HeaderText="Anzahl">
												<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
												<ItemTemplate>
													<asp:Label id=Label4 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Anzahl") %>'>
													</asp:Label>
													<asp:Label id=lblBreak runat="server" Visible='<%# Not TypeOf (DataBinder.Eval(Container, "DataItem.Tagessumme")) Is System.DBNull AndAlso (DataBinder.Eval(Container, "DataItem.Tagessumme") <>0) %>'>
														<br>
													</asp:Label>
													<asp:Label id=Label5 runat="server" Visible='<%# Not TypeOf (DataBinder.Eval(Container, "DataItem.Tagessumme")) Is System.DBNull AndAlso (DataBinder.Eval(Container, "DataItem.Tagessumme") <>0) %>' CssClass="HighLightOne" Text='<%# "Tagessumme&amp;nbsp;=&amp;nbsp;" &amp; DataBinder.Eval(Container, "DataItem.Tagessumme") %>'>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="KennzeichenVon" SortExpression="KennzeichenVon" HeaderText="KennzeichenVon">
												<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="KennzeichenBis" SortExpression="KennzeichenBis" HeaderText="KennzeichenBis">
												<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="Tagessumme" SortExpression="Tagessumme" HeaderText="Tagessumme">
												<ItemStyle VerticalAlign="Top"></ItemStyle>
											</asp:BoundColumn>
										</Columns>
										<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
									</asp:datagrid></TD>
								<TD colSpan="1"></TD>
							</TR>
						</TABLE>
						&nbsp;
					</td>
				</tr>
				<TR>
					<TD vAlign="top"></TD>
					<TD vAlign="top"></TD>
				</TR>
				<tr>
					<td></td>
					<td></td>
				</tr>
				<tr>
					<td></td>
					<td><!--#include File="../../../PageElements/Footer.html" --></td>
				</tr>
			</table>
			</TD></TR></TBODY></TABLE></form>
	</body>
</HTML>
