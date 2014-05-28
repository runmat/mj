<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Vorerf02.aspx.vb" Inherits="AppKroschke.Vorerf02" %>
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
			<input id="txtOrtsKzOld" type="hidden" name="txtOrtsKzOld" runat="server"> <INPUT id="txtFree2" type="hidden" name="txtFree2" runat="server">
			<table width="100%" align="center">
				<TBODY>
					<tr>
						<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
					</tr>
					<tr>
						<td>
							<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
								<TR>
									<td class="PageNavigation" colSpan="2" height="19"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;(
										<asp:label id="lblPageTitle" runat="server" Font-Bold="True"></asp:label>)</td>
								</TR>
								<tr>
									<TD class="" vAlign="top" width="100">
										<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="100" border="0">
											<TR>
												<TD class="TaskTitle">&nbsp;</TD>
											</TR>
											<TR>
												<TD height="12">
													<P align="left"><asp:linkbutton id="btnSaveSAP" runat="server" CssClass="StandardButton"> &#149;&nbsp;Absenden</asp:linkbutton></P>
												</TD>
											</TR>
											<TR>
												<TD height="12"><asp:linkbutton id="btnErfassen" runat="server" CssClass="StandardButton"> &#149;&nbsp;Erfassen</asp:linkbutton></TD>
											</TR>
											<TR>
												<TD height="12">
													<P>&nbsp;</P>
												</TD>
											</TR>
										</TABLE>
										<P>&nbsp;</P>
									</TD>
									<TD vAlign="top" align="left">
										<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
											<TR>
												<TD class="TaskTitle" align="right">&nbsp;<asp:label id="lblFilter" runat="server" Font-Bold="True" Visible="False"></asp:label><asp:label id="lblDatensatz" runat="server" Font-Bold="True" Visible="False"></asp:label></TD>
											</TR>
											<TR>
												<TD class="LabelExtraLarge">
													<TABLE id="Table9" cellSpacing="1" cellPadding="1" width="100%" border="0">
														<TR>
															<TD vAlign="top" align="left" colSpan="2"><asp:label id="lblMsg" runat="server" Font-Bold="True" EnableViewState="False"></asp:label><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
														</TR>
														<TR>
															<TD vAlign="top" align="left" colSpan="2"><asp:label id="lblTableTitle" runat="server" Font-Bold="True"> Liste erfasster Datensätze:&nbsp;</asp:label><asp:label id="lblAnzahl" runat="server" Font-Bold="True" EnableViewState="False"></asp:label></TD>
														</TR>
													</TABLE>
													<asp:datagrid id="dataGrid" runat="server" CssClass="tableMain" bodyHeight="300" bodyCSS="tableBody" headerCSS="tableHeader" PageSize="50" AllowSorting="True" AutoGenerateColumns="False" Width="100%" BackColor="White">
														<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
														<HeaderStyle CssClass="GridTableHead" VerticalAlign="Top"></HeaderStyle>
														<Columns>
															<asp:BoundColumn Visible="False" DataField="id" SortExpression="id" HeaderText="ID"></asp:BoundColumn>
															<asp:BoundColumn DataField="id_sap" SortExpression="id_sap" HeaderText="ID"></asp:BoundColumn>
															<asp:BoundColumn DataField="toDelete" SortExpression="toDelete" HeaderText="L&#246;sch">
																<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
																<ItemStyle Font-Bold="True" HorizontalAlign="Center"></ItemStyle>
															</asp:BoundColumn>
															<asp:TemplateColumn SortExpression="kundenname" HeaderText="Kunde">
																<ItemTemplate>
																	<asp:Literal id=Literal1 runat="server" Text='<%# "<a name=""" &amp; DataBinder.Eval(Container, "DataItem.id_sap") &amp; """>" &amp; DataBinder.Eval(Container, "DataItem.kundenname") &amp; "</a>" %>'>
																	</asp:Literal>
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:BoundColumn DataField="dienstleistung" SortExpression="dienstleistung" HeaderText="Dienstleistung"></asp:BoundColumn>
															<asp:BoundColumn DataField="zulassungsdatum" SortExpression="zulassungsdatum" HeaderText="Zul.Datum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
															<asp:BoundColumn Visible="False" DataField="toSave" SortExpression="toSave" HeaderText="ToSave"></asp:BoundColumn>
															<asp:BoundColumn DataField="haltername" SortExpression="haltername" HeaderText="Referenz"></asp:BoundColumn>
															<asp:TemplateColumn Visible="False" HeaderText="Absenden">
																<ItemTemplate>
																	<asp:CheckBox id="CheckBox2" runat="server" Checked="True"></asp:CheckBox>
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:BoundColumn DataField="str_wunschkennz" SortExpression="str_wunschkennz" HeaderText="Kennz."></asp:BoundColumn>
															<asp:TemplateColumn HeaderText="Status">
																<ItemTemplate>
																	<asp:Label id=Label1 runat="server" Visible='<%# (Not (Typeof (DataBinder.Eval(Container, "DataItem.status")) is System.DBNull) AndAlso (DataBinder.Eval(Container, "DataItem.status")<>"Vorgang OK") AndAlso (DataBinder.Eval(Container, "DataItem.status")<>"Vorgang gelöscht")) %>' Text='<%# DataBinder.Eval(Container, "DataItem.status") %>' Font-Size="X-Small" ForeColor="Red">
																	</asp:Label>
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn HeaderText="Bearb.">
																<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
																<ItemStyle HorizontalAlign="Center"></ItemStyle>
																<ItemTemplate>
																	<asp:ImageButton id="ImageButton1" runat="server" ImageUrl="/Portal/Images/lupe.gif" CommandName="Select"></asp:ImageButton>
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn HeaderText="L&#246;schen">
																<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
																<ItemStyle HorizontalAlign="Center"></ItemStyle>
																<ItemTemplate>
																	<asp:ImageButton id="ImageButton2" runat="server" ImageUrl="/Portal/Images/Icon_nein_s.gif" CommandName="Delete"></asp:ImageButton>
																</ItemTemplate>
															</asp:TemplateColumn>
														</Columns>
													</asp:datagrid></TD>
											</TR>
											<TR>
												<TD class="LabelExtraLarge" align="left" colSpan="2"><asp:hyperlink id="lnkExcel" runat="server" Visible="False" Target="_blank">Excelformat</asp:hyperlink><STRONG></STRONG>&nbsp;<asp:label id="lblDownloadTip" runat="server" Font-Bold="True" Visible="False" Font-Size="8pt">rechte Maustaste => Ziel speichern unter...</asp:label></TD>
											</TR>
											<tr>
												<TD class="LabelExtraLarge" align="left" colSpan="2"><A id="lnkPopUp" href="Javascript:openPopUp();" runat="server" Visible="False">Drucken...</A><STRONG></STRONG></TD>
											</tr>
										</TABLE>
										<!--#include File="../../../PageElements/Footer.html" --></TD>
								</tr>
							</TABLE>
							<asp:literal id="Literal2" runat="server"></asp:literal>
		</form>
		</TD></TR></TBODY></TABLE>
	</body>
</HTML>
