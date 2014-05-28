<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change41_1.aspx.vb" Inherits="CKG.Components.ComCommon.Change41_1" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
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
			<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td colSpan="2">
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Auswählen)</asp:label></td>
							</TR>
							<TR>
								<TD vAlign="top" colSpan="2">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top"><asp:linkbutton id="lb_Haendlersuche" runat="server"></asp:linkbutton></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr id="trVorgangsArt" runat="server">
											<td colSpan="2">
											<TD width="37"></TD>
										</tr>
										<TR>
											<TD class="" width="100%"><strong><asp:label id="lblNoData" runat="server" Visible="False"></asp:label></strong></TD>
											<TD class="LabelExtraLarge" align="right"><asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
										</TR>
										<TR id="trPageSize" runat="server">
											<TD class="LabelExtraLarge" align="left" colSpan="2"><STRONG></STRONG></TD>
											<TD class="LabelExtraLarge" align="right" width="37" height="6"></TD>
										</TR>
										<TR id="trDataGrid1" runat="server">
											<TD align="center" colSpan="2"><asp:datagrid id="DataGrid1" runat="server" PageSize="50" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" bodyHeight="400" cssclass="tableMain" Width="100%" BackColor="White">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="H&#228;ndlernummer" SortExpression="H&#228;ndlernummer" HeaderText="H&#228;ndlernummer"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="H&#228;ndlernummer" HeaderText="col_Haendlernummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Haendlernummer" runat="server" CommandArgument="Händlernummer" CommandName="Sort"></asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:LinkButton id="lbHaendlernummer" runat="server" CommandName="Select" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.ZZREFERENZ1")%>' CausesValidation="False" Text='<%# DataBinder.Eval(Container, "DataItem.Händlernummer") %>'>
																</asp:LinkButton>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Anzahl Standard endg&#252;ltig" HeaderText="Anzahl Standard endg&#252;ltig">
															<ItemTemplate>
																<asp:Label id=Label4 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Anzahl Standard endgültig") %>'>
																</asp:Label>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=Textbox4 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Anzahl Standard endgültig") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Anzahl Standard tempor&#228;r" HeaderText="Anzahl Standard tempor&#228;r">
															<ItemTemplate>
																<asp:Label id=Label1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Anzahl Standard temporär") %>'>
																</asp:Label>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=Textbox1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Anzahl Standard temporär") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Anzahl Flottengesch&#228;ft" HeaderText="col_Flottengeschaeft">
															<HeaderTemplate>
																<asp:LinkButton id="col_Flottengeschaeft" runat="server" CommandName="Sort" CommandArgument="Anzahl Flottengesch&#228;ft">col_Flottengeschaeft</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id=Label2 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Anzahl Flottengeschäft") %>'>
																</asp:Label>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=Textbox2 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Anzahl Flottengeschäft") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="H&#228;ndlername" SortExpression="H&#228;ndlername" HeaderText="H&#228;ndlername"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="Ort" HeaderText="col_Ort">
															<HeaderTemplate>
																<asp:LinkButton id="col_Ort" runat="server" CommandName="Sort" CommandArgument="Ort">col_Ort</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id="Label3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Ort") %>'>
																</asp:Label>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id="Textbox3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Ort") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="ZZREFERENZ1" HeaderText="col_ZZREFERENZ1">
															<HeaderTemplate>
																<asp:LinkButton id="col_ZZREFERENZ1" runat="server" CommandName="Sort" CommandArgument="ZZREFERENZ1">col_ZZREFERENZ1</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:LinkButton id="lbZZREFERENZ1" runat="server" CommandName="SelectZZREFERENZ1" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.ZZREFERENZ1")%>'  Text='<%# DataBinder.Eval(Container, "DataItem.ZZREFERENZ1") %>'>
																</asp:LinkButton>
															</ItemTemplate>				
															
														</asp:TemplateColumn>
													</Columns>
													<PagerStyle NextPageText="N&#228;chste Seite" Font-Size="12pt" Font-Bold="True" PrevPageText="Vorherige Seite" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></TD>
											<TD align="center" width="37"></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<td width="148">&nbsp;<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
								<TD vAlign="top" align="left"></TD>
							</TR>
							<TR>
								<td width="148">&nbsp;</td>
								<TD vAlign="top" align="left"><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
				<TR id="ShowScript" runat="server" visible="False">
					<TD>
						<script language="Javascript">
						<!-- //
						function FreigebenConfirm(Auftrag,Vertrag,Angefordert,Fahrgest,BriefNr) {
						var Check = window.confirm("Wollen Sie diesen Kfz-Brief wirklich freigeben?\n\tVertrag\t\t" + Vertrag + "\n\tAngefordert am\t" + Angefordert + "\n\tFahrgestellnr.\t" + Fahrgest + "\t\n\tKfz-Briefnr.\t" + BriefNr);
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
