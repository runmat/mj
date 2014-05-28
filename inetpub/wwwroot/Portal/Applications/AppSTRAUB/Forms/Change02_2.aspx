<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change02_2.aspx.vb" Inherits="AppSTRAUB.Change02_2" %>
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
			<table cellSpacing="0" cellPadding="2" width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2" height="19"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Vorgangsanzeige)</asp:label>&nbsp;<asp:hyperlink id="lnkKreditlimit" runat="server" CssClass="TaskTitle" Visible="False" NavigateUrl="Change13.aspx">Vorgangssuche</asp:hyperlink></td>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton"> &#149;&nbsp;Weiter</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdConfirm" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Absenden</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdReset" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Verwerfen</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdBack" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Zurück</asp:linkbutton>&nbsp;</TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top"><asp:hyperlink id="lnkExcel" runat="server" Visible="False" Target="_blank">Excelformat</asp:hyperlink>&nbsp;&nbsp;
												<asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Size="8pt" Font-Bold="True">rechte Maustaste => Ziel speichern unter...</asp:label>&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td colSpan="3" height="18"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
										</tr>
									</TABLE>
									<TABLE id="Table5" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="LabelExtraLarge" vAlign="top" align="left" colSpan="3">
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<TR>
														<TD class="LabelExtraLarge"><asp:label id="lblNoData" runat="server"></asp:label></TD>
														<TD align="right"><asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
													</TR>
												</table>
												<asp:datagrid id="DataGrid1" runat="server" BackColor="White" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="400" PageSize="50" Width="100%" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="KUNPDI" SortExpression="KUNPDI" HeaderText="PDI"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="Equipmentnummer" SortExpression="Equipmentnummer" HeaderText="Equipmentnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Hersteller" SortExpression="Hersteller" HeaderText="Hersteller"></asp:BoundColumn>
														<asp:BoundColumn DataField="Fahrgestellnummer" SortExpression="Fahrgestellnummer" HeaderText="Fahrgestell-&lt;br&gt;nummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Kennzeichen" SortExpression="Kennzeichen" HeaderText="Kennzeichen"></asp:BoundColumn>
														<asp:BoundColumn DataField="Abmeldedatum" SortExpression="Abmeldedatum" HeaderText="Fr&#252;hestes&lt;br&gt;Abmeldedatum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
														<asp:BoundColumn DataField="Erstzulassungsdatum" SortExpression="Erstzulassungsdatum" HeaderText="Datum Erstzulassung"></asp:BoundColumn>
														<asp:BoundColumn DataField="Eingangsdatum" SortExpression="Eingangsdatum" HeaderText="Eingangsdatum"></asp:BoundColumn>
														<asp:TemplateColumn HeaderText="Keine&lt;br&gt;Auswahl">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:RadioButton id=chkActionNOTHING runat="server" GroupName="grpAction" Checked='<%# DataBinder.Eval(Container, "DataItem.ActionNOTHING") %>'>
																</asp:RadioButton>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=TextBox3 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ActionNOTHING") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="L&#246;schen">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:RadioButton id=chkActionDELE runat="server" GroupName="grpAction" Checked='<%# DataBinder.Eval(Container, "DataItem.ActionDELE") %>'>
																</asp:RadioButton>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=TextBox4 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ActionDELE") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn Visible="False" DataField="Action" HeaderText="Aktion"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="Bemerkung" HeaderText="Bemerkung"></asp:BoundColumn>
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
							<TR id="ShowScript" runat="server">
								<td>&nbsp;</td>
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
