<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change03_1.aspx.vb" Inherits="AppCommonCarRent.Change03_1" %>
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
			<table cellSpacing="0" cellPadding="2" width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2">
									<asp:label id="lblHead" runat="server"></asp:label>
									<asp:label id="lblPageTitle" runat="server"> (Vorgangsanzeige)</asp:label>
								</td>
							</TR>
							<TR>
								<TD class="TaskTitle"  align="right" colSpan="2">&nbsp;
                                  
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
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
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td colSpan="3" height="18">
												<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
											<TD height="18"></TD>
										</tr>
										<TR>
											<TD class="" colSpan="3" width="100%"><strong>&nbsp;<asp:label id="lblNoData" runat="server"></asp:label></strong></TD>
											<TD>
												<P align="right">
                                                    <asp:ImageButton ID="imgbExcel"  runat="server" Height="20px" ImageUrl="../../../Images/excel.gif"
                                                        Visible="True" Width="20px" />&nbsp;
													<asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist></P>
											</TD>
										</TR>
									</TABLE>
									<TABLE id="Table5" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="" vAlign="top" align="left" colSpan="3">
												<asp:datagrid id="DataGrid1" runat="server" AllowSorting="True" AllowPaging="True" Width="100%" PageSize="50" bodyHeight="400" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" BackColor="White" AutoGenerateColumns="False">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
															<asp:BoundColumn Visible="False" DataField="Equipmentnummer" SortExpression="Equipmentnummer" HeaderText="Equipmentnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Fahrgestellnummer" SortExpression="Fahrgestellnummer" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="Briefnummer" SortExpression="Briefnummer" HeaderText="Briefnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Kennzeichen" SortExpression="Kennzeichen" HeaderText="Kennzeichen"></asp:BoundColumn>
														<asp:BoundColumn DataField="Modellbezeichnung" SortExpression="Modellbezeichnung" HeaderText="Modellbezeichnung"></asp:BoundColumn>
														<asp:BoundColumn DataField="Hersteller" SortExpression="Hersteller" HeaderText="Hersteller"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Laufzeit" SortExpression="Laufzeit" HeaderText="Laufzeit">
                                                        </asp:BoundColumn>
														<asp:BoundColumn DataField="Abmeldedatum" SortExpression="Abmeldedatum" HeaderText="Fr&#252;hestes Abmeldedatum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
														<asp:BoundColumn DataField="Zulassungsdatum" SortExpression="Zulassungsdatum" HeaderText="Zulassungsdatum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="TageZuMindesthaltefrist" SortExpression="TageZuMindesthaltefrist"
                                                            HeaderText="fehlende Tage zur Mindesthaltefrist">
                                                        </asp:BoundColumn>
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
														<asp:TemplateColumn Visible="False" HeaderText="Haltefrist einhalten &lt;br&gt; und ausblenden">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:RadioButton id="chkActionDISABLE" runat="server" GroupName="grpAction" Enabled="False"></asp:RadioButton>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="Haltefrist ignorieren &lt;br&gt; und abmelden">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:RadioButton id=chkActionDELE runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ActionDELE") %>' GroupName="grpAction">
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
													<PagerStyle NextPageText="" Font-Size="12pt" Font-Bold="True" PrevPageText="" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid>
											</TD>
										</TR>
										<tr>
											<td vAlign="top" align="left" colSpan="3"></td>
										</tr>
									</TABLE>
								</td>
							</tr>
							<TR>
								<td>&nbsp;</td>
								<td></td>
							</TR>
							<TR>
								<td>&nbsp;</td>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</TR>
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
