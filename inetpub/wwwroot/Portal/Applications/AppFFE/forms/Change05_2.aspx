<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change05_2.aspx.vb" Inherits="AppFFE.Change05_2" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema"/>
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</head>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<table cellspacing="0" cellpadding="2" width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
							<tr>
								<td class="PageNavigation" colspan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server">Fahrzeugauswahl</asp:label></td>
							</tr>
							<tr>
								<td valign="top" width="120">
									<table id="Table2" borderColor="#ffffff" cellspacing="0" cellpadding="0" width="120" border="0">
										<tr>
											<td class="TaskTitle">&nbsp;</td>
										</tr>
										<tr>
											<td valign="middle" width="150"><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton">&#149;&nbsp;Weiter</asp:linkbutton></td>
										</tr>
									</table>
									<asp:calendar id="calZul" runat="server" CellPadding="0" BorderColor="Black" BorderStyle="Solid" Width="120px" Visible="False">
										<TodayDayStyle Font-Bold="True"></TodayDayStyle>
										<NextPrevStyle ForeColor="White"></NextPrevStyle>
										<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
										<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
										<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
										<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
										<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
									</asp:calendar></td>
								<td valign="top">
									<table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
										<tr>
											<td class="TaskTitle" valign="top"><asp:hyperlink id="lnkKreditlimit" runat="server" CssClass="TaskTitle" NavigateUrl="Change04.aspx">Fahrzeugsuche</asp:hyperlink>&nbsp;</td>
										</tr>
									</table>
									<table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
										<tr>
											<td colSpan="3"><uc1:kopfdaten id="Kopfdaten1" runat="server"></uc1:kopfdaten></td>
										</tr>
									</table>
									<table id="Table5" cellspacing="0" cellpadding="0" width="100%" border="0">
										<tr>
											<td valign="top" align="left" colspan="3">
                                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td class="" align="left" width="618" height="9">
                                                            <strong>
                                                                <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label></strong>
                                                        </td>
                                                            <!--<asp:HyperLink id="HyperLink1" runat="server" NavigateUrl="/PageElements/Datum.htm" Target="_blank">Zulassungsdatum:</asp:HyperLink> -->
                                                            <td nowrap align="right" height="9">
                                                                <p align="right">
                                                                    <strong>Zulassungsdatum&nbsp;</strong><a href="javascript:openinfo('../../../PageElements/Datum.htm');"><strong><img
                                                                        src="/Portal/Images/fragezeichen.gif" border="0"></strong></a>&nbsp;&nbsp;
                                                                    <asp:TextBox ID="txtZulDatum" runat="server"></asp:TextBox>&nbsp;
                                                                    <asp:LinkButton ID="Linkbutton2" runat="server" CssClass="StandardButtonTable" Width="57px">Kalender</asp:LinkButton>&nbsp;
                                                                    <asp:DropDownList ID="ddlPageSize" runat="server" Height="14px" AutoPostBack="True">
                                                                    </asp:DropDownList>
                                                                </p>
                                                            </td>
                                                    </tr>
                                                </table>
												<asp:label id="lblError" runat="server" CssClass="TextError" DESIGNTIMEDRAGDROP="61" EnableViewState="False"></asp:label></td>
										</tr>
										<tr>
											<td valign="top" align="left" colspan="3"><asp:datagrid id="DataGrid1" runat="server" Width="100%" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" bodyHeight="400" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" PageSize="50" BackColor="White">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn DataField="ZZFAHRG" SortExpression="ZZFAHRG" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="MANDT" SortExpression="MANDT" HeaderText="MANDT"></asp:BoundColumn>
														<asp:BoundColumn DataField="LIZNR" SortExpression="LIZNR" HeaderText="Vertragsnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="TIDNR" SortExpression="TIDNR" HeaderText="Kfz-Briefnummer"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="LICENSE_NUM" SortExpression="LICENSE_NUM" HeaderText="Kfz-Kennzeichen"></asp:BoundColumn>
														<asp:BoundColumn DataField="ZZREFERENZ1" SortExpression="ZZREFERENZ1" HeaderText="Ordernummer"></asp:BoundColumn>
														<asp:TemplateColumn Visible="False" SortExpression="ZZBEZAHLT" HeaderText="Bezahlt">
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox id="chkBezahlt" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.ZZBEZAHLT") %>'>
																</asp:CheckBox>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id="TextBox1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZBEZAHLT") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn Visible="False" SortExpression="ZZCOCKZ" HeaderText="COC Besch.&lt;br&gt;vorhanden">
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox id="Checkbox1" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ZZCOCKZ") %>' Enabled="False">
																</asp:CheckBox>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id="Textbox2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZCOCKZ") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="Vermiet-&lt;br&gt;fahrzeug">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:RadioButton id="chk0001" runat="server" AutoPostBack="True" GroupName="Kontingentart"></asp:RadioButton>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="Vorf&#252;hr-&lt;br&gt;fahrzeug">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:RadioButton id="chk0002" runat="server" AutoPostBack="True" GroupName="Kontingentart"></asp:RadioButton>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="HEZ">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:RadioButton id="chk0003" runat="server" AutoPostBack="True" GroupName="Kontingentart"></asp:RadioButton>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="&lt;u&gt;Nicht&lt;/u&gt;&lt;br&gt;zulassen">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:RadioButton id="chkNichtAnfordern" runat="server" AutoPostBack="True" GroupName="Kontingentart"></asp:RadioButton>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="Mindesthalte-&lt;br&gt;frist">
															<ItemTemplate>
																<asp:TextBox id="txtHaltefrist" runat="server" CssClass="LaufzeitInaktiv" Width="66px" Enabled="False" MaxLength="3"></asp:TextBox>
															</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td width="100">&nbsp;</td>
								<td></td>
							</tr>
							<tr>
								<td width="100">&nbsp;</td>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</TR>
							<tr id="ShowScript" runat="server">
								<td width="100">&nbsp;</td>
								<td>
									<SCRIPT language="JavaScript">
										<!-- //
										// window.document.Form1.elements[window.document.Form1.length-3].focus();
										//-->
										function openinfo (url) {
												fenster=window.open(url, "Zulassungsdatum", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0,width=550,height=150");
												fenster.focus();
										}
									</SCRIPT>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
