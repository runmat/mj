<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change01Edit.aspx.vb" Inherits="AppFFD.Change01Edit" %>
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
			<table cellSpacing="0" cellPadding="0" width="100%" align="center">
				<tr>
					<td colSpan="3"><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<TR>
					<TD colSpan="3">
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2" height="19"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;(
									<asp:label id="lblPageTitle" runat="server"></asp:label>)</td>
							</TR>
							<TR>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdSave" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Sichern</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdConfirm" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Bestätigen</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdReset" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Verwerfen</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdAuthorize" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Autorisieren</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdDelete" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Löschen</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdBack" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Zurück</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:hyperlink id="cmdBack2" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Zurück</asp:hyperlink></TD>
										</TR>
									</TABLE>
								</TD>
								<TD>
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top"><asp:hyperlink id="lnkKreditlimit_old" runat="server" Visible="False" NavigateUrl="Change01.aspx" CssClass="TaskTitle">Händlersuche</asp:hyperlink>&nbsp;<asp:hyperlink id="lnkKreditlimit" runat="server" NavigateUrl="Change01.aspx" CssClass="TaskTitle">Händlersuche</asp:hyperlink>&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
										<tr>
											<td class="TextLarge" vAlign="top">Händlernummer:</td>
											<td class="TextLarge" vAlign="top" width="100%" colSpan="2"><asp:label id="lblHaendlerNummer" runat="server"></asp:label></td>
										</tr>
										<tr>
											<td class="StandardTableAlternate" vAlign="top">Name:&nbsp;&nbsp;
											</td>
											<td class="StandardTableAlternate" vAlign="top" colSpan="2"><asp:label id="lblHaendlerName" runat="server"></asp:label></td>
										</tr>
										<tr>
											<td class="TextLarge" vAlign="top">Adresse:</td>
											<td class="TextLarge" vAlign="top" colSpan="2"><asp:label id="lblAdresse" runat="server"></asp:label></td>
										</tr>
									</TABLE>
									<TABLE id="Table7" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td></td>
										</tr>
										<TR>
											<TD><asp:datagrid id="DataGrid1" runat="server" BackColor="White" Width="100%" AutoGenerateColumns="False" CellPadding="3">
													<AlternatingItemStyle CssClass="StandardTableAlternate"></AlternatingItemStyle>
													<ItemStyle CssClass="TextLarge"></ItemStyle>
													<HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="Kreditkontrollbereich" HeaderText="Kreditkontrollbereich"></asp:BoundColumn>
														<asp:BoundColumn DataField="Kontingentart" HeaderText="Kontingentart"></asp:BoundColumn>
														<asp:TemplateColumn HeaderText="Altes Kontingent">
															<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
															<ItemStyle HorizontalAlign="Right"></ItemStyle>
															<ItemTemplate>
																<asp:Label id=lblKontingent_Alt runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kontingent_Alt") %>'>
																</asp:Label>
																<asp:Label id=lblRichtwert_Alt runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.Richtwert_Alt") %>'>
																</asp:Label>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=TextBox5 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kontingent_Alt") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="Ausschoepfung" HeaderText="Inanspruchnahme">
															<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
															<ItemStyle HorizontalAlign="Right"></ItemStyle>
														</asp:BoundColumn>
														<asp:TemplateColumn HeaderText="Freies Kontingent">
															<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
															<ItemStyle HorizontalAlign="Right"></ItemStyle>
															<ItemTemplate>
																<asp:Label id=lblFrei runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Frei") %>'>
																</asp:Label>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=TextBox3 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Frei") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="Gesperrt - Alt">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox id=Gesperrt_Alt runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Gesperrt_Alt") %>' Enabled="False">
																</asp:CheckBox>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=TextBox1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Gesperrt_Alt") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="Neues Kontingent">
															<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
															<ItemStyle HorizontalAlign="Right"></ItemStyle>
															<ItemTemplate>
																<asp:Image id="Image2" runat="server" Width="12px" Height="12px" ImageUrl="/Portal/Images/empty.gif"></asp:Image>
																<asp:TextBox id=txtKontingent_Neu runat="server" CssClass="InputRight" Width="50px" Text='<%# DataBinder.Eval(Container, "DataItem.Kontingent_Neu") %>'>
																</asp:TextBox>
																<asp:TextBox id=txtRichtwert_Neu runat="server" CssClass="InputRight" Width="50px" Text='<%# DataBinder.Eval(Container, "DataItem.Richtwert_Neu") %>'>
																</asp:TextBox>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=Textbox9 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kontingent_Neu") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="Gesperrt - Neu ">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:Image id="imgGesperrt_Neu" runat="server" Width="12px" Height="12px" ImageUrl="/Portal/Images/empty.gif"></asp:Image>
																<asp:CheckBox id=chkGesperrt_Neu runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Gesperrt_Neu") %>'>
																</asp:CheckBox>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=Textbox4 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Gesperrt_Neu") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn Visible="False" HeaderText="ZeigeKontingentart">
															<ItemTemplate>
																<asp:CheckBox id=chkZeigeKontingentart runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ZeigeKontingentart") %>'>
																</asp:CheckBox>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=TextBox2 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZeigeKontingentart") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
													</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Center" Wrap="False"></PagerStyle>
												</asp:datagrid></TD>
										</TR>
									</TABLE>
									<TABLE id="Table8" cellSpacing="0" cellPadding="5" width="100%" border="0">
										<TR id="ConfirmMessage" runat="server">
											<TD class="LabelExtraLarge"><asp:label id="lblInformation" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<tr>
								<td>&nbsp;</td>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</tr>
						</TABLE>
					</TD>
				</TR>
				<tr id="FocusScript" runat="server">
					<td colSpan="3">
						<script language="JavaScript">
							<!-- //
							window.document.Form1.elements[4].focus();
							//-->
						</script>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
