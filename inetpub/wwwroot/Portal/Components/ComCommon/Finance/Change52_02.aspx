<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change52_02.aspx.vb" Inherits="CKG.Components.ComCommon.Change52_02"%>
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
								<td></td>
								<td></td>
							</tr>
							<tr>
								<TD vAlign="top"></TD>
								<TD vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td class="TaskTitle" colSpan="2"><asp:LinkButton id="lb_Back" runat="server">Back</asp:LinkButton></td>
										</tr>
										<TR>
											<TD align="left" colSpan="2"><asp:linkbutton id="cmdsave" runat="server" CssClass="StandardButton">&#149;&nbsp;Absenden</asp:linkbutton></TD>
										<TR>
											<TD align="left" colSpan="2">
											</TD>
										</TR>
										<TR>
											<TD align="left" colSpan="2"></TD>
										</TR>
										<TR>
											<TD class="" width="100%" colSpan="1"><asp:label id="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:label></TD>
											<TD class="LabelExtraLarge" align="right"><asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
										</TR>
										<TR>
											<TD colSpan="2"><asp:datagrid id="DataGrid1" runat="server" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="250" Width="100%" AutoGenerateColumns="False" AllowSorting="True">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:TemplateColumn SortExpression="H&#228;ndlername" HeaderText="col_Haendlername">
															<HeaderTemplate>
																<asp:LinkButton id="col_Haendlername" runat="server" CommandName="Sort" CommandArgument="Händlername">col_Haendlername</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id="Label7" runat="server" NAME="Label4" Text='<%# DataBinder.Eval(Container, "DataItem.Händlername") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id=Label4 runat="server" NAME="Label4" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Modell" HeaderText="col_Modell">
															<HeaderTemplate>
																<asp:LinkButton id="col_Modell" runat="server" CommandName="Sort" CommandArgument="Modell">col_Modell</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id="Label2" runat="server" Width="55px" NAME="Label2" Text='<%# DataBinder.Eval(Container, "DataItem.Modell") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Nummer ZB2" HeaderText="col_NummerZB2">
															<HeaderTemplate>
																<asp:LinkButton id="col_NummerZB2" runat="server" CommandName="Sort" CommandArgument="Nummer ZB2">col_NummerZB2</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Nummer ZB2") %>' ID="Label3" NAME="Label3">
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
															<HeaderTemplate>
																<asp:LinkButton id="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="Kennzeichen">col_Kennzeichen</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id=Label5 runat="server" NAME="Label5" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Eingangsdatum" HeaderText="col_Eingangsdatum">
															<HeaderTemplate>
																<asp:LinkButton id="col_Eingangsdatum" runat="server" CommandArgument="Eingangsdatum" CommandName="Sort">col_Eingangsdatum</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id=Label6 runat="server" NAME="Label8" Text='<%# DataBinder.Eval(Container, "DataItem.Eingangsdatum") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="col_Haendlernummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Haendlernummer" runat="server" CommandArgument="X" CommandName="Sort">col_Haendlernummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:TextBox id="txt_Haendlnr" runat="server" Width="85px"></asp:TextBox>
																<asp:Label id="lblHdnr" runat="server" Visible="False"></asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="col_Lizenznr">
															<HeaderTemplate>
																<asp:LinkButton id="col_Lizenznr" runat="server" CommandArgument="X" CommandName="Sort">col_Lizenznr</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:TextBox id="Textbox2" runat="server" Width="85px"></asp:TextBox>
																<asp:Label id="lblLiznr" runat="server" Visible="False"></asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="col_Finart">
															<HeaderTemplate>
																<asp:LinkButton id="col_Finart" runat="server" CommandArgument="X" CommandName="Sort">col_Finart</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:TextBox id="Textbox1" runat="server" Width="85px"></asp:TextBox>
																<asp:Label id="lblFinart" runat="server" Visible="False"></asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="col_Label">
															<HeaderTemplate>
																<asp:LinkButton id="col_Label" runat="server" CommandArgument="X" CommandName="Sort">col_Label</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:TextBox id="Textbox3" runat="server" Width="85px"></asp:TextBox>
																<asp:Label id="lblLabel" runat="server" Visible="False"></asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn Visible="False" DataField="Status" SortExpression="Status" HeaderText="Status"></asp:BoundColumn>
													</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Wrap="False"></PagerStyle>
												</asp:datagrid></TD>
										</TR>
									</TABLE>
									<asp:label id="lbl_Info" runat="server" Font-Bold="True" EnableViewState="False"></asp:label></TD>
							</tr>
							<tr>
								<td></td>
								<td><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
							</tr>
							<tr>
								<td></td>
								<td><!--#include File="../../../PageElements/Footer.html" --><asp:label id="lblHidden" runat="server" Visible="False" Width="39px"></asp:label></td>
							</tr>
						</TABLE>
					</td>
				</tr>
				<TR id="ShowScript" runat="server" visible="False">
					<TD>
						<script language="Javascript">
						<!-- //
						function FreigebenConfirm(Fahrgest,Vertrag,BriefNr,Kennzeichen) {
						var Check = window.confirm("Wollen Sie für dieses Fahrzeug wirklich den Status 'Bezahlt' setzen?\t\n\tFahrgestellnr.\t" + Fahrgest + "\t\n\tVertrag\t\t" + Vertrag + "\t\n\tKfz-Briefnr.\t" + BriefNr + "\n\tKfz-Kennzeichen\t" + Kennzeichen);
						return (Check);
						}
						//-->
						</script>
					</TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
