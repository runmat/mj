<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change03.aspx.vb" Inherits="CKG.Components.ComCommon.Change03" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../PageElements/Kopfdaten.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../PageElements/Styles.ascx" %>
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
		<form id="Form1" method="post" encType="multipart/form-data" runat="server">
			<table width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top" width="120" height="192">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle" width="120">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="120"><asp:linkbutton id="lb_Upload" runat="server" CssClass="StandardButton">lb_Upload</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="120"><asp:linkbutton id="lb_Back" runat="server" CssClass="StandardButton">lb_Back</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="120"><IMG height="1" src="../../images/empty.gif" width="120" border="0"></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top" height="192">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="tblUpload" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
										<tr>
											<td vAlign="top" align="left">
												<TABLE id="tbl0001" cellSpacing="0" cellPadding="5" width="100%" border="0">
													<TR id="tr_Auftrag" runat="server">
														<TD class="TextLarge" vAlign="top" noWrap align="right"><asp:label id="lbl_Auftrag" runat="server">lbl_Auftrag</asp:label>&nbsp;</TD>
														<TD class="TextLarge"><asp:literal id="litAuftragShow" runat="server"></asp:literal></TD>
													</TR>
													<TR id="tr_Haendler" runat="server">
														<TD class="TextLarge" vAlign="top" noWrap align="right"><asp:label id="lbl_Haendler" runat="server">lbl_Haendler</asp:label>&nbsp;</TD>
														<TD class="TextLarge"><asp:literal id="litHaendlerShow" runat="server"></asp:literal></TD>
													</TR>
													<TR>
														<TD class="TextLarge" noWrap align="right">Dateiauswahl <A href="javascript:openinfo('Info01.htm');">
																<IMG src="../../images/fragezeichen.gif" border="0"></A>:&nbsp;&nbsp;</TD>
														<TD class="TextLarge"><INPUT id="upFile" type="file" size="49" name="File1" runat="server">&nbsp;</TD>
													</TR>
													<TR>
														<TD class="TextLarge" noWrap align="right">&nbsp;</TD>
														<TD class="TextLarge">&nbsp;
															<asp:label id="lblExcelfile" runat="server"></asp:label></TD>
													</TR>
												</TABLE>
											</td>
										</tr>
									</TABLE>
									<TABLE id="tblSelection" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td vAlign="top" align="left"><asp:datagrid id="Datagrid2" runat="server" BackColor="White" PageSize="50" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="300" AutoGenerateColumns="False" AllowPaging="True" Width="100%">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="Vbeln" SortExpression="Vbeln" HeaderText="Vbeln"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="Wldat" SortExpression="Wldat" HeaderText="Wldat" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="Kunnr" SortExpression="Kunnr" HeaderText="Kunnr"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="Name1" SortExpression="Name1" HeaderText="Name1"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="Name2" SortExpression="Name2" HeaderText="Name2"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="Post_code1" SortExpression="Post_code1" HeaderText="Post_code1"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="City1" SortExpression="City1" HeaderText="City1"></asp:BoundColumn>
														<asp:TemplateColumn>
															<ItemTemplate>
																<asp:Literal id=Literal22 runat="server" Text='<%# "<a name=""" &amp; DataBinder.Eval(Container, "DataItem.Vbeln") &amp; """><font color=""#FFFFFF"">.</font></a>" %>'>
																</asp:Literal>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Vbeln" HeaderText="col_Vbeln">
															<HeaderTemplate>
																<asp:LinkButton id="col_Vbeln" runat="server" CommandName="Sort" CommandArgument="Vbeln">col_Vbeln</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Vbeln") %>' ID="Label1" NAME="Label1">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Wldat" HeaderText="col_Wldat">
															<HeaderTemplate>
																<asp:LinkButton id="col_Wldat" runat="server" CommandName="Sort" CommandArgument="Wldat">col_Wldat</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Wldat", "{0:dd.MM.yyyy}") %>' ID="Label3" NAME="Label3">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Kunnr" HeaderText="col_Kunnr">
															<HeaderTemplate>
																<asp:LinkButton id="col_Kunnr" runat="server" CommandName="Sort" CommandArgument="Kunnr">col_Kunnr</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kunnr") %>' ID="LabelKunnr" NAME="LabelKunnr">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Name1" HeaderText="col_Name1">
															<HeaderTemplate>
																<asp:LinkButton id="col_Name1" runat="server" CommandName="Sort" CommandArgument="Name1">col_Name1</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Name1") %>' ID="LabelName1" NAME="LabelName1">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Name2" HeaderText="col_Name2">
															<HeaderTemplate>
																<asp:LinkButton id="col_Name2" runat="server" CommandName="Sort" CommandArgument="Name2">col_Name2</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Name2") %>' ID="LabelName2" NAME="LabelName2">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Street" HeaderText="col_Street">
															<HeaderTemplate>
																<asp:LinkButton id="col_Street" runat="server" CommandName="Sort" CommandArgument="Street">col_Street</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Street") %>' ID="LabelStreet" NAME="LabelStreet">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Post_code1" HeaderText="col_Post_code1">
															<HeaderTemplate>
																<asp:LinkButton id="col_Post_code1" runat="server" CommandName="Sort" CommandArgument="Post_code1">col_Post_code1</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Post_code1") %>' ID="LabelPost_code1" NAME="LabelPost_code1">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="City1" HeaderText="col_City1">
															<HeaderTemplate>
																<asp:LinkButton id="col_City1" runat="server" CommandName="Sort" CommandArgument="City1">col_City1</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.City1") %>' ID="LabelCity1" NAME="LabelCity1">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Gebiet" HeaderText="col_Gebiet">
															<HeaderTemplate>
																<asp:LinkButton id="col_Gebiet" runat="server" CommandName="Sort" CommandArgument="Gebiet">col_Gebiet</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Gebiet") %>' ID="LabelGebiet" NAME="LabelGebiet">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Txt30" HeaderText="col_Txt30">
															<HeaderTemplate>
																<asp:LinkButton id="col_Txt30" runat="server" CommandName="Sort" CommandArgument="Txt30">col_Txt30</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Txt30") %>' ID="Label2" NAME="Label2">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn>
															<ItemTemplate>
																<asp:LinkButton id="lb_Auswahl" runat="server" CssClass="StandardButtonTable" CausesValidation="false" Text="lb_Auswahl" CommandName="Select">lb_Auswahl</asp:LinkButton>
															</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid><br>
											</td>
										</tr>
									</TABLE>
									<asp:literal id="Literal1" runat="server"></asp:literal></td>
							</tr>
							<TR>
								<TD vAlign="top" width="120">&nbsp;</TD>
								<TD vAlign="top"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">&nbsp;</TD>
								<td><!--#include File="../../PageElements/Footer.html" --></td>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
		<SCRIPT language="JavaScript">										
				<!--
						function openinfo (url) {
								fenster=window.open(url, "Zulassungsdatum", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0");
								fenster.focus();
						}
				-->
		</SCRIPT>
	</body>
</HTML>
