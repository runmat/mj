<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change02_2.aspx.vb" Inherits="AppFFD.Change02_2" %>
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
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2">
									<asp:label id="lblHead" runat="server"></asp:label>
									<asp:label id="lblPageTitle" runat="server"> Auswählen</asp:label></td>
							</TR>
							<TR>
								<TD vAlign="top" width="100">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="100" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton" Visible="False"> &#149;&nbsp;Weiter</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<TD vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">
												<asp:hyperlink id="lnkKreditlimit" runat="server" CssClass="TaskTitle" NavigateUrl="Change02.aspx">Filialsuche</asp:hyperlink>&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr id="trKopfdaten" runat="server">
											<td><uc1:kopfdaten id="Kopfdaten1" runat="server"></uc1:kopfdaten></td>
											<TD></TD>
										</tr>
										<tr id="trVorgangsArt" runat="server">
											<td colSpan="2">
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<tr>
														<td class="" align="right">
															<P align="left">
																<asp:Label id="lblAnzeige" runat="server"></asp:Label><asp:radiobutton id="rbStandard" runat="server" GroupName="grpVorgaenge" Checked="True" Visible="False"></asp:radiobutton>
																<asp:radiobutton id="rbFlottengeschaeft" runat="server" GroupName="grpVorgaenge" Visible="False"></asp:radiobutton>
																<asp:radiobutton id="rbHEZ" runat="server" Visible="False" GroupName="grpVorgaenge"></asp:radiobutton></P>
														</td>
													</tr>
												</table>
												<asp:DropDownList id="ddlKontingentart" runat="server" Font-Bold="True"></asp:DropDownList>
											</td>
											<TD></TD>
										</tr>
										<TR>
											<TD class=""><asp:label id="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:label></TD>
											<TD class="" align="right">
												<asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True" Visible="False"></asp:dropdownlist></TD>
										</TR>
										<TR id="trDataGrid1" runat="server">
											<TD align="middle" colSpan="2"><asp:datagrid id="DataGrid1" runat="server" PageSize="50" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" bodyHeight="400" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" Width="100%" BackColor="White">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:TemplateColumn></asp:TemplateColumn>
														<asp:BoundColumn Visible="False" DataField="H&#228;ndlernummer" SortExpression="H&#228;ndlernummer" HeaderText="H&#228;ndlernummer"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="H&#228;ndlernummer" HeaderText="H&#228;ndler-&lt;br&gt;nummer">
															<HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:LinkButton id=LinkButton1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Händlernummer") %>' CommandName="Select" CausesValidation="False">
																</asp:LinkButton>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="H&#228;ndlername" SortExpression="H&#228;ndlername" HeaderText="H&#228;ndlername"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="Anzahl Standard endg&#252;ltig" HeaderText="Anzahl&lt;br&gt;Standard endg&#252;ltig">
															<HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
															<ItemStyle HorizontalAlign="Right"></ItemStyle>
															<ItemTemplate>
																<asp:Label id=Label4 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Anzahl Standard endgültig") %>'>
																</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=Textbox4 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Anzahl Standard endgültig") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Anzahl Standard tempor&#228;r" HeaderText="Anzahl&lt;br&gt;Standard tempor&#228;r">
															<HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
															<ItemStyle HorizontalAlign="Right"></ItemStyle>
															<ItemTemplate>
																<asp:Label id=Label1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Anzahl Standard temporär") %>'>
																</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=Textbox1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Anzahl Standard temporär") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Anzahl Flottengesch&#228;ft" HeaderText="Anzahl&lt;br&gt;erweitertes&lt;br&gt;Zahlungsziel&lt;br&gt;(Delayed&lt;br&gt;Payment)">
															<HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
															<ItemStyle HorizontalAlign="Right"></ItemStyle>
															<ItemTemplate>
																<asp:Label id=Label2 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Anzahl Flottengeschäft") %>'>
																</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=Textbox2 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Anzahl Flottengeschäft") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Anzahl HEZ" HeaderText="Anzahl&lt;br&gt;HEZ">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:Label id=Label3 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Anzahl HEZ") %>'>
																</asp:Label>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=TextBox3 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Anzahl HEZ") %>'>
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn></asp:TemplateColumn>
													</Columns>
													<PagerStyle NextPageText="N&#228;chste Seite" Font-Size="12pt" Font-Bold="True" PrevPageText="Vorherige Seite" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></TD>
											<TD align="middle" colSpan="1"></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<td>&nbsp;</td>
								<TD vAlign="top" align="left"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
							<TR>
								<td>&nbsp;</td>
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
