<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change01_2.aspx.vb" Inherits="AppEC.Change01_2" %>
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
									<td class="PageNavigation" colSpan="2" height="19"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;(Bestätigen)</td>
								</TR>
								<tr>
									<TD class="" vAlign="top">
										<P>&nbsp;</P>
									</TD>
									<TD vAlign="top" align="left">
										<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
											<TR>
												<TD class="TaskTitle" align="right">&nbsp;</TD>
											</TR>
											<TR>
												<TD class="LabelExtraLarge">
													<TABLE id="Table9" cellSpacing="1" cellPadding="1" width="100%" border="0">
														<TR>
															<TD vAlign="top" align="left" colSpan="2"><asp:label id="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:label></TD>
														</TR>
														<TR>
															<TD vAlign="top" align="left" colSpan="2"><asp:label id="lblTableTitle" runat="server" Font-Bold="True"> Anzahl Datensätze:&nbsp;</asp:label><asp:label id="lblAnzahl" runat="server" EnableViewState="False" Font-Bold="True"></asp:label></TD>
														</TR>
													</TABLE>
													<asp:datagrid id="dataGrid" runat="server" CssClass="tableMain" BackColor="White" Width="100%" AutoGenerateColumns="False" AllowSorting="True" PageSize="50" headerCSS="tableHeader" bodyCSS="tableBody" bodyHeight="300">
														<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
														<HeaderStyle CssClass="GridTableHead" VerticalAlign="Top"></HeaderStyle>
														<Columns>
															<asp:BoundColumn Visible="False" DataField="RowId" SortExpression="RowId" HeaderText="Lfd.Nr."></asp:BoundColumn>
															<asp:BoundColumn DataField="ModelId" SortExpression="ModelId" HeaderText="Model-ID"></asp:BoundColumn>
															<asp:BoundColumn DataField="ModellBezeichnung" SortExpression="ModellBezeichnung" HeaderText="Modell"></asp:BoundColumn>
															<asp:BoundColumn DataField="BatchId" SortExpression="BatchId" HeaderText="Batch-ID"></asp:BoundColumn>
															<asp:BoundColumn DataField="UnitNrVon" SortExpression="UnitNrVon" HeaderText="Unit-Nr. von"></asp:BoundColumn>
															<asp:BoundColumn DataField="UnitNrBis" SortExpression="UnitNrBis" HeaderText="Unit-Nr. bis"></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Unitnummern" SortExpression="Unitnummern" HeaderText="Unit-Nummern"></asp:BoundColumn>
															<asp:BoundColumn DataField="AuftragsNrVon" SortExpression="AuftragsNrVon" HeaderText="Auftrags-Nr. von"></asp:BoundColumn>
															<asp:BoundColumn DataField="AuftragsNrBis" SortExpression="AuftragsNrBis" HeaderText="Auftrags-Nr. bis"></asp:BoundColumn>
															<asp:BoundColumn DataField="Laufzeit" SortExpression="Laufzeit" HeaderText="Laufzeit"></asp:BoundColumn>
															<asp:BoundColumn Visible="False" DataField="LaufzeitBindung" SortExpression="LaufzeitBindung"></asp:BoundColumn>
															<asp:BoundColumn Visible="False" DataField="Fahrzeuggruppe" SortExpression="Fahrzeuggruppe"></asp:BoundColumn>
															<asp:BoundColumn Visible="True" DataField="Kennzeichenserie" HeaderText="Kennzeichenserie" SortExpression="Kennzeichenserie"></asp:BoundColumn>
															<asp:TemplateColumn SortExpression="Laufzeitbindung" HeaderText="Laufzeit-&lt;br&gt;Bindung">
																<ItemTemplate>
																	<asp:Label id=Label3 runat="server" Visible='<%# Ctype(DataBinder.Eval(Container, "DataItem.Laufzeitbindung"),Boolean)=True %>'>Ja</asp:Label>
																	<asp:Label id=Label2 runat="server" Visible='<%# Ctype(DataBinder.Eval(Container, "DataItem.Laufzeitbindung"),Boolean)=False %>'>Nein</asp:Label>
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn SortExpression="Fahrzeuggruppe" HeaderText="Fahrzeug-&lt;br&gt;Gruppe">
																<ItemTemplate>
																	<asp:Label id="Label1" runat="server" Visible='<%# Ctype(DataBinder.Eval(Container, "DataItem.Fahrzeuggruppe"),Boolean)=True %>'>PKW</asp:Label>
																	<asp:Label id="Label4" runat="server" Visible='<%# Ctype(DataBinder.Eval(Container, "DataItem.Fahrzeuggruppe"),Boolean)=False %>'>LKW</asp:Label>
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn SortExpression="Leasing" HeaderText="Leasing">
																<ItemTemplate>
																	<asp:Label id=Label7 runat="server" Visible='<%# Ctype(DataBinder.Eval(Container, "DataItem.Leasing"),Boolean)=True %>'>Ja</asp:Label>
																	<asp:Label id=Label8 runat="server" Visible='<%# Ctype(DataBinder.Eval(Container, "DataItem.Leasing"),Boolean)=False %>'>Nein</asp:Label>
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn SortExpression="Winterbereifung" HeaderText="M+S">
																<ItemTemplate>
																	<asp:Label id="Label9" runat="server" Visible='<%# Ctype(DataBinder.Eval(Container, "DataItem.Winterbereifung"),Boolean)=True %>'>Ja</asp:Label>
																	<asp:Label id="Label10" runat="server" Visible='<%# Ctype(DataBinder.Eval(Container, "DataItem.Winterbereifung"),Boolean)=False %>'>Nein</asp:Label>
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn SortExpression="NavigationsSystem" HeaderText="Nav.">
																<ItemTemplate>
																	<asp:Label id="Label11" runat="server" Visible='<%# Ctype(DataBinder.Eval(Container, "DataItem.NavigationsSystem"),Boolean)=True %>'>Ja</asp:Label>
																	<asp:Label id="Label12" runat="server" Visible='<%# Ctype(DataBinder.Eval(Container, "DataItem.NavigationsSystem"),Boolean)=False %>'>Nein</asp:Label>
																</ItemTemplate>
															</asp:TemplateColumn>
															
																														
															<asp:TemplateColumn Visible="False" HeaderText="Absenden">
																<ItemTemplate>
																	<asp:CheckBox id="CheckBox2" runat="server" Checked="True"></asp:CheckBox>
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn HeaderText="Status">
																<ItemTemplate>
																	<asp:Label id=Label5 runat="server" ForeColor="Red" Visible='<%# NOT (typeof (DataBinder.Eval(Container, "DataItem.Status")) is System.DBNull) AndAlso DataBinder.Eval(Container, "DataItem.Status")<>"Gespeichert." %>' Text='<%# DataBinder.Eval(Container, "DataItem.Status") %>'>
																	</asp:Label>
																	<asp:Label id=Label6 runat="server" Visible='<%# NOT (typeof (DataBinder.Eval(Container, "DataItem.Status")) is System.DBNull) AndAlso DataBinder.Eval(Container, "DataItem.Status")="Gespeichert." %>' Text='<%# DataBinder.Eval(Container, "DataItem.Status") %>'>
																	</asp:Label>
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn HeaderText="Bearbeiten">
																<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
																<ItemStyle HorizontalAlign="Center"></ItemStyle>
																<ItemTemplate>
																	<asp:ImageButton id=Imagebutton2 runat="server" ImageUrl="/Portal/Images/select.gif" CommandName="Select" Visible='<%# (typeof (DataBinder.Eval(Container, "DataItem.Status")) is System.DBNull) OrElse DataBinder.Eval(Container, "DataItem.Status")<>"Gespeichert." %>'>
																	</asp:ImageButton>
																</ItemTemplate>
															</asp:TemplateColumn>
														</Columns>
													</asp:datagrid></TD>
											</TR>
											<TR id="trSumme" runat="server">
												<TD class="LabelExtraLarge">
													<TABLE id="Table2" cellSpacing="0" cellPadding="0" width="100%" border="0">
														<TR>
															<TD align="left"><asp:linkbutton id="Linkbutton1" runat="server" CssClass="StandardButton"> &#149;&nbsp;Erfassen</asp:linkbutton></TD>
															<TD align="right">&nbsp;
																<asp:linkbutton id="btnSaveSAP" runat="server" CssClass="StandardButton"> &#149;&nbsp;Liste abschicken!</asp:linkbutton></TD>
														</TR>
													</TABLE>
												</TD>
											</TR>
										</TABLE>
										<!--#include File="../../../PageElements/Footer.html" -->
										<P align="left">&nbsp;</P>
									</TD>
								</tr>
							</TABLE>
		</form>
		</TD></TR></TBODY></TABLE>
	</body>
</HTML>
