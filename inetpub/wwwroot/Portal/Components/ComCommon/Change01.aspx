<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change01.aspx.vb" Inherits="CKG.Components.ComCommon.Change01" %>
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
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table4" width="100%" align="center">
				<TR>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</TR>
				<TR>
					<TD>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="PageNavigation"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;&nbsp;&nbsp;
												<asp:hyperlink id="lnkExcel" runat="server" Target="_blank">Excelformat</asp:hyperlink>&nbsp;&nbsp;&nbsp;&nbsp;
												<asp:label id="lblDownloadTip" runat="server" Font-Size="8pt" Font-Bold="True" Visible="False">rechte Maustaste => Ziel speichern unter...</asp:label></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left"><asp:panel id="PanelDatagrids" runat="server">
													<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
														<TR>
															<TD class="TextLarge" vAlign="top" colSpan="2">
																<P>
																	<asp:datagrid id="Datagrid2" runat="server" AllowPaging="True" Width="100%" AutoGenerateColumns="False" bodyHeight="200" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" PageSize="50" BackColor="White">
																		<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
																		<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
																		<Columns>
																			<asp:BoundColumn Visible="False" DataField="Kunnr" SortExpression="Kunnr" HeaderText="Kunnr-Hidden"></asp:BoundColumn>
																			<asp:TemplateColumn SortExpression="Kunnr" HeaderText="col_Kunnr">
																				<HeaderTemplate>
																					<asp:LinkButton id="col_Kunnr" runat="server" CommandName="Sort" CommandArgument="Kunnr">col_Kunnr_I</asp:LinkButton>
																				</HeaderTemplate>
																				<ItemTemplate>
																					<asp:Label id=lblKunnr_IShow2 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kunnr") %>'>
																					</asp:Label>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:TemplateColumn SortExpression="Name1" HeaderText="col_Name1">
																				<HeaderTemplate>
																					<asp:LinkButton id="col_Name1" runat="server" CommandName="Sort" CommandArgument="Name1">col_Name1</asp:LinkButton>
																				</HeaderTemplate>
																				<ItemTemplate>
																					<asp:Literal id=Literal22 runat="server" Text='<%# "<a name=""" &amp; DataBinder.Eval(Container, "DataItem.Kunnr") &amp; """>" &amp; DataBinder.Eval(Container, "DataItem.Name1") &amp; "</a>" %>'>
																					</asp:Literal>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:TemplateColumn SortExpression="Name2" HeaderText="col_Name2">
																				<HeaderTemplate>
																					<asp:LinkButton id="col_Name2" runat="server" CommandName="Sort" CommandArgument="Name2">col_Name2</asp:LinkButton>
																				</HeaderTemplate>
																				<ItemTemplate>
																					<asp:Label id=lblName2Show runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Name2") %>'>
																					</asp:Label>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:TemplateColumn SortExpression="Land1" HeaderText="col_Land1">
																				<HeaderTemplate>
																					<asp:LinkButton id="col_Land1" runat="server" CommandName="Sort" CommandArgument="Land1">col_Land1</asp:LinkButton>
																				</HeaderTemplate>
																				<ItemTemplate>
																					<asp:Label id=lblLand1Show runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Land1") %>'>
																					</asp:Label>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:TemplateColumn SortExpression="Pstlz" HeaderText="col_Pstlz">
																				<HeaderTemplate>
																					<asp:LinkButton id="col_Pstlz" runat="server" CommandName="Sort" CommandArgument="Pstlz">col_Pstlz</asp:LinkButton>
																				</HeaderTemplate>
																				<ItemTemplate>
																					<asp:Label id=lblPstlzShow runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Pstlz") %>'>
																					</asp:Label>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:TemplateColumn SortExpression="Ort01" HeaderText="col_Ort01">
																				<HeaderTemplate>
																					<asp:LinkButton id="col_Ort01" runat="server" CommandName="Sort" CommandArgument="Ort01">col_Ort01</asp:LinkButton>
																				</HeaderTemplate>
																				<ItemTemplate>
																					<asp:Label id=lblOrt01Show runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Ort01") %>'>
																					</asp:Label>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:TemplateColumn SortExpression="Stras" HeaderText="col_Stras">
																				<HeaderTemplate>
																					<asp:LinkButton id="col_Stras" runat="server" CommandName="Sort" CommandArgument="Stras">col_Stras</asp:LinkButton>
																				</HeaderTemplate>
																				<ItemTemplate>
																					<asp:Label id=lblStrasShow runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Stras") %>'>
																					</asp:Label>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:TemplateColumn SortExpression="Telf1" HeaderText="col_Telf1">
																				<HeaderTemplate>
																					<asp:LinkButton id="col_Telf1" runat="server" CommandName="Sort" CommandArgument="Telf1">col_Telf1</asp:LinkButton>
																				</HeaderTemplate>
																				<ItemTemplate>
																					<asp:Label id=lblTelf1Show runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Telf1") %>'>
																					</asp:Label>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:TemplateColumn SortExpression="Telfx" HeaderText="col_Telfx">
																				<HeaderTemplate>
																					<asp:LinkButton id="col_Telfx" runat="server" CommandName="Sort" CommandArgument="Telfx">col_Telfx</asp:LinkButton>
																				</HeaderTemplate>
																				<ItemTemplate>
																					<asp:Label id=lblTelfxShow runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Telfx") %>'>
																					</asp:Label>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:TemplateColumn SortExpression="Smtp_Addr" HeaderText="col_Smtp_Addr">
																				<HeaderTemplate>
																					<asp:LinkButton id="col_Smtp_Addr" runat="server" CommandName="Sort" CommandArgument="Smtp_Addr">col_Smtp_Addr</asp:LinkButton>
																				</HeaderTemplate>
																				<ItemTemplate>
																					<asp:Label id=lblSmtp_AddrShow runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Smtp_Addr") %>'>
																					</asp:Label>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:TemplateColumn SortExpression="Katr9" HeaderText="col_Katr9">
																				<HeaderTemplate>
																					<asp:LinkButton id="col_Katr9" runat="server" CommandName="Sort" CommandArgument="Katr9">col_Katr9</asp:LinkButton>
																				</HeaderTemplate>
																				<ItemTemplate>
																					<asp:Label id=lblKatr9Show runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Katr9") %>'>
																					</asp:Label>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:TemplateColumn SortExpression="Vtext" HeaderText="col_Vtext">
																				<HeaderTemplate>
																					<asp:LinkButton id="col_Vtext" runat="server" CommandName="Sort" CommandArgument="Vtext">col_Vtext</asp:LinkButton>
																				</HeaderTemplate>
																				<ItemTemplate>
																					<asp:Label id=lblVtextShow runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Vtext") %>'>
																					</asp:Label>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:TemplateColumn>
																				<ItemTemplate>
																					<asp:LinkButton id="lb_Aendern" runat="server" CssClass="StandardButtonTable" CausesValidation="False" CommandName="Aendern" Text="lbAendern">lb_Aendern</asp:LinkButton>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																		</Columns>
																		<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
																	</asp:datagrid></P>
															</TD>
														</TR>
														<TR>
															<TD class="TextLarge" vAlign="top" width="150"></TD>
															<TD class="TextLarge" vAlign="top">
																<asp:literal id="Literal1" runat="server"></asp:literal></TD>
														</TR>
													</TABLE>
												</asp:panel><asp:panel id="PanelAdressAenderung" runat="server">
													<TABLE id="Table5" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
														<TR id="tr_Kunnr_I" runat="server">
															<TD class="TextLarge" vAlign="top" width="150">
																<asp:Label id="lbl_Kunnr_I" runat="server">lblKunnr_I</asp:Label>&nbsp;&nbsp;
															</TD>
															<TD class="TextLarge" vAlign="top">
																<asp:Label id="lblKunnr_IShow" runat="server"></asp:Label></TD>
														</TR>
														<TR id="tr_Name1" runat="server">
															<TD class="TextLarge" vAlign="top" width="150">
																<asp:Label id="lbl_Name1" runat="server">lblName1</asp:Label>&nbsp;&nbsp;
															</TD>
															<TD class="TextLarge" vAlign="top">
																<asp:TextBox id="txtName1" runat="server" Width="300px" MaxLength="35"></asp:TextBox></TD>
														</TR>
														<TR id="tr_Name2" runat="server">
															<TD class="TextLarge" vAlign="top" width="150">
																<asp:Label id="lbl_Name2" runat="server">lblName2</asp:Label>&nbsp;&nbsp;
															</TD>
															<TD class="TextLarge" vAlign="top">
																<asp:TextBox id="txtName2" runat="server" Width="300px" MaxLength="35"></asp:TextBox></TD>
														</TR>
														<TR id="tr_Land1" runat="server">
															<TD class="TextLarge" vAlign="top" width="150">
																<asp:Label id="lbl_Land1" runat="server">lblLand1</asp:Label>&nbsp;&nbsp;
															</TD>
															<TD class="TextLarge" vAlign="top">
																<asp:TextBox id="txtLand1" runat="server" Width="300px" MaxLength="3"></asp:TextBox></TD>
														</TR>
														<TR id="tr_Pstlz" runat="server">
															<TD class="TextLarge" vAlign="top" width="150">
																<asp:Label id="lbl_Pstlz" runat="server">lblPstlz</asp:Label>&nbsp;&nbsp;
															</TD>
															<TD class="TextLarge" vAlign="top">
																<asp:TextBox id="txtPstlz" runat="server" Width="300px" MaxLength="5"></asp:TextBox></TD>
														</TR>
														<TR id="tr_Ort01" runat="server">
															<TD class="TextLarge" vAlign="top" width="150">
																<asp:Label id="lbl_Ort01" runat="server">lblOrt01</asp:Label>&nbsp;&nbsp;
															</TD>
															<TD class="TextLarge" vAlign="top">
																<asp:TextBox id="txtOrt01" runat="server" Width="300px" MaxLength="35"></asp:TextBox></TD>
														</TR>
														<TR id="tr_Stras" runat="server">
															<TD class="TextLarge" vAlign="top" width="150">
																<asp:Label id="lbl_Stras" runat="server">lblStras</asp:Label>&nbsp;&nbsp;
															</TD>
															<TD class="TextLarge" vAlign="top">
																<asp:TextBox id="txtStras" runat="server" Width="300px" MaxLength="35"></asp:TextBox></TD>
														</TR>
														<TR id="tr_Telf1" runat="server">
															<TD class="TextLarge" vAlign="top" width="150">
																<asp:Label id="lbl_Telf1" runat="server">lblTelf1</asp:Label>&nbsp;&nbsp;
															</TD>
															<TD class="TextLarge" vAlign="top">
																<asp:TextBox id="txtTelf1" runat="server" Width="300px" MaxLength="16"></asp:TextBox></TD>
														</TR>
														<TR id="tr_Telfx" runat="server">
															<TD class="TextLarge" vAlign="top" width="150">
																<asp:Label id="lbl_Telfx" runat="server">lblTelfx</asp:Label>&nbsp;&nbsp;
															</TD>
															<TD class="TextLarge" vAlign="top">
																<asp:TextBox id="txtTelfx" runat="server" Width="300px" MaxLength="31"></asp:TextBox></TD>
														</TR>
														<TR id="tr_Smtp_Addr" runat="server">
															<TD class="TextLarge" vAlign="top" width="150">
																<asp:Label id="lbl_Smtp_Addr" runat="server">lblSmtp_Addr</asp:Label>&nbsp;&nbsp;
															</TD>
															<TD class="TextLarge" vAlign="top">
																<asp:TextBox id="txtSmtp_Addr" runat="server" Width="300px" MaxLength="241"></asp:TextBox></TD>
														</TR>
														<TR id="tr_Katr9" runat="server">
															<TD class="TextLarge" vAlign="top" width="150">
																<asp:Label id="lbl_Katr9" runat="server">lblKatr9</asp:Label>&nbsp;&nbsp;
															</TD>
															<TD class="TextLarge" vAlign="top">
																<asp:DropDownList id="ddlKatr9" runat="server" Width="300px"></asp:DropDownList></TD>
														</TR>
														<TR>
															<TD class="TextLarge" vAlign="top" width="150"></TD>
															<TD class="TextLarge" vAlign="top">
																<asp:LinkButton id="lb_Aendern2" runat="server" CssClass="StandardButtonTable" CausesValidation="False" CommandName="Freigeben">lb_Aendern2</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
																<asp:LinkButton id="lb_Back2" runat="server" CssClass="StandardButtonTable">lb_Back2</asp:LinkButton></TD>
														</TR>
														<TR>
															<TD class="TextLarge" vAlign="top" width="150"></TD>
															<TD class="TextLarge" vAlign="top"></TD>
														</TR>
													</TABLE>
												</asp:panel><BR>
											</TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD vAlign="top"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
							<TR>
								<TD><!--#include File="../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
