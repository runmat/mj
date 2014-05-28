<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change01.aspx.vb" Inherits="AppECAN.Change01" %>
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
			<TABLE id="Table4" width="100%" align="center">
				<TR>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</TR>
				<TR>
					<TD>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="150" border="0">
										<TR>
											<TD class="TaskTitle" width="150">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdCreate" runat="server" CssClass="StandardButton"> Speichern</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdConfirm" runat="server" CssClass="StandardButton" Visible="False"> Bestätigen</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdBack" runat="server" CssClass="StandardButton" Visible="False"> Zurück</asp:linkbutton></TD>
										</TR>
									</TABLE>
									<P>&nbsp;</P>
									<P>&nbsp;</P>
								</TD>
								<TD vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;&nbsp;&nbsp;
												<asp:hyperlink id="lnkExcel" runat="server" Target="_blank">Excelformat</asp:hyperlink>&nbsp;
												<asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Bold="True" Font-Size="8pt">rechte Maustaste => Ziel speichern unter...</asp:label></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
													<TR>
														<TD class="TextLarge" vAlign="top"><asp:listbox id="ListBox1" runat="server" CssClass="ListStyle" AutoPostBack="True" Height="420px" Width="200px"></asp:listbox></TD>
														<TD class="TextLarge" vAlign="top"><asp:datagrid id="DataGrid1" runat="server" Width="100%" BackColor="White" PageSize="50" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="400" AllowSorting="True" AutoGenerateColumns="False">
																<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
																<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
																<Columns>
																	<asp:TemplateColumn HeaderText="Auswahl">
																		<ItemTemplate>
																			<asp:CheckBox id=CheckBox1 runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Selected") %>'>
																			</asp:CheckBox>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn SortExpression="Zzkunnr_Zs" HeaderText="Kunde">
																		<ItemTemplate>
																			<asp:Label id=Label1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Zzkunnr_Zs") &amp; " - " &amp; DataBinder.Eval(Container, "DataItem.Name1") %>'>
																			</asp:Label>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:BoundColumn DataField="CHASSIS_NUM" SortExpression="CHASSIS_NUM" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
																	<asp:BoundColumn DataField="ZZMODELL" SortExpression="ZZMODELL" HeaderText="Fahrzeugtyp"></asp:BoundColumn>
																	<asp:BoundColumn DataField="Verkaufsdatum" SortExpression="Verkaufsdatum" HeaderText="Verkaufsdatum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
																</Columns>
																<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
															</asp:datagrid></TD>
													</TR>
												</TABLE>
												<BR>
											</TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD vAlign="top" width="150">&nbsp;</TD>
								<TD vAlign="top"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top" width="150">&nbsp;</TD>
								<TD><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
