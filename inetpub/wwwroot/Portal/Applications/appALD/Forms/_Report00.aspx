<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="_Report00.aspx.vb" Inherits="AppALD._Report00" %>
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
			<table width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2" height="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Abfragekriterien)</asp:label></td>
							</TR>
							<tr>
								<TD class="StandardTableButtonFrame" vAlign="top"><asp:linkbutton id="btnFinish" runat="server" CssClass="StandardButtonTable">&#149;&nbsp;Suchen</asp:linkbutton><BR>
									&nbsp;&nbsp;<BR>
									<TABLE id="tblSearch" cellSpacing="0" cellPadding="0" border="0" runat="server">
									</TABLE>
								</TD>
								<TD vAlign="top" width="100%">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;<asp:label id="lblDatensatz" runat="server" Font-Bold="True" Visible="False"></asp:label>
												<asp:label id="lblNoData" runat="server" Font-Bold="True"></asp:label></TD>
										</TR>
										<TR>
											<TD class="LabelExtraLarge" vAlign="top" align="left"><asp:datagrid id="DataGrid1" runat="server" AllowSorting="True" bodyHeight="400" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" BackColor="White" PageSize="50" Width="100%">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:TemplateColumn Visible="False">
															<HeaderStyle Width="30px"></HeaderStyle>
															<ItemTemplate>
																<asp:ImageButton id="ImageButton1" runat="server" ToolTip="Vorschau anzeigen" ImageUrl="/Portal/Images/camera.gif" CommandName="vorschau"></asp:ImageButton>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn>
															<ItemTemplate>
																<asp:ImageButton id=Imagebutton3 runat="server" Visible='<%# typeof (DataBinder.Eval(Container,"DataItem.Bilder")) is System.DBNull %>' ToolTip="Dokumente vom Server laden" ImageUrl="/Portal/Images/PDF_grey.gif" CommandName="ansicht">
																</asp:ImageButton>
																<asp:HyperLink id=HyperLink2 runat="server" Visible='<%# not (typeof (DataBinder.Eval(Container,"DataItem.Bilder")) is System.DBNull) %>' ToolTip="Dokumente anzeigen (.PDF)" ImageUrl="/Portal/Images/pdf.gif" Target="_blank" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Bilder") %>'>HyperLink</asp:HyperLink>
																<asp:Literal id=Literal2 runat="server" Text='<%# "<a name=""" &amp; DataBinder.Eval(Container, "DataItem.DOC_ID") &amp; """><font color=""#FFFFFF"">.</font></a>" %>'>
																</asp:Literal>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn Visible="False" HeaderText="Seiten">
															<ItemTemplate>
																<asp:HyperLink id=HyperLink1 runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Bilder") %>' Text='<%# DataBinder.Eval(Container, "DataItem.Link") %>' Target="_blank">
																</asp:HyperLink>
															</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid>
												<TABLE id="Table9" cellSpacing="0" cellPadding="5" width="100%" border="0">
													<TR>
														<TD vAlign="top" align="left" colSpan="2"><asp:dropdownlist id="ddlArchive" runat="server" Font-Bold="True" Visible="False" Font-Names="Courier New"></asp:dropdownlist></TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
									</TABLE>
									<asp:label id="lblError" runat="server" CssClass="TextError"></asp:label></TD>
							</tr>
						</TABLE>
					</td>
				</tr>
			</table>
			</TD></TR></TBODY></TABLE>
			<asp:Literal id="Literal1" runat="server"></asp:Literal></form>
	</body>
</HTML>
