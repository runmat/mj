<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report08.aspx.vb" Inherits="AppFFE.Report08" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	    <style type="text/css">
            .style2
            {
                width: 177px;
            }
        </style>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table4" width="100%" align="center">
				<TR>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header><asp:imagebutton id="ImageButton1" runat="server" ImageUrl="/Portal/Images/empty.gif"></asp:imagebutton></td>
				</TR>
				<TR>
					<TD>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="PageNavigation" colSpan="2" height="19"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:label></TD>
							</TR>
							<TR>
								<TD class="TaskTitle" colSpan="2">&nbsp;</TD>
							</TR>
							<TR>
								<TD vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
                                                    <tr>
                                                        <td class="TextLarge" valign="center" width="142">
                                                            Geschäftsjahr:
                                                        </td>
                                                        <td class="TextLarge" valign="center" nowrap>
                                                            <asp:TextBox ID="txtJahr" runat="server" Width="75px"></asp:TextBox>
                                                        </td>
                                                        <td class="TextLarge" valign="center">
                                                            Kontingentart:
                                                        </td>
                                                        <td class="TextLarge" valign="center">
                                                            <asp:DropDownList ID="ddlKontingentart" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td valign="center">
                                                            <asp:LinkButton ID="cmdCreate" runat="server" CssClass="StandardButton">&#149;&nbsp;Erstellen</asp:LinkButton>
                                                            <asp:Label ID="lblMessage" runat="server" CssClass="TextLarge" EnableViewState="False"></asp:Label>
                                                        </td>
                                                        <td align="right" class="TextLarge" valign="center" width="100%">
                                                            <img id="imgExcel" runat="server" Visible="False"  alt="" src="../../../images/excel.gif" style="width: 15px; height: 17px" />&nbsp;<asp:HyperLink
                                                                ID="lnkExcel" runat="server" CssClass="ExcelButton" Visible="False" Target="_blank"><strong>Excelformat</strong></asp:HyperLink>&nbsp;
                                                            <asp:Label ID="lblDownloadTip" runat="server" Visible="False" Font-Size="8pt" Font-Bold="True">&nbsp;rechte Maustaste => Ziel speichern unter...</asp:Label>&nbsp;<asp:DropDownList
                                                                ID="ddlPageSize" runat="server" Visible="False" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
												</TABLE>
												<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label>
											</TD>
										</TR>
									</TABLE>
									<asp:datagrid id="DataGrid1" runat="server" Width="100%" BackColor="White" PageSize="50" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="350" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" BorderColor="#CCCCCC" BorderWidth="1px" BorderStyle="Solid">
										<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
										<HeaderStyle Font-Size="XX-Small" Font-Names="Arial" CssClass="GridTableHead"></HeaderStyle>
										<Columns>
											<asp:BoundColumn DataField="Haendler" SortExpression="Haendler" HeaderText="H&#228;ndler">
												<HeaderStyle Font-Size="XX-Small" Font-Names="Arial"></HeaderStyle>
												<ItemStyle Font-Size="XX-Small" Font-Names="Arial" CssClass="GridTableCell"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="Filiale" SortExpression="Filiale" HeaderText="Filiale">
												<HeaderStyle Font-Size="XX-Small" Font-Names="Arial"></HeaderStyle>
												<ItemStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Left"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="Versandart" SortExpression="Versandart" HeaderText="Art">
												<HeaderStyle Font-Size="XX-Small" Font-Names="Arial"></HeaderStyle>
												<ItemStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Left" CssClass="GridTableCell"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="JAHR" SortExpression="JAHR" HeaderText="Gesch&#228;fts-&lt;br&gt;jahr">
												<HeaderStyle Font-Size="XX-Small" Font-Names="Arial"></HeaderStyle>
												<ItemStyle Font-Size="XX-Small" Font-Names="Arial" CssClass="GridTableCell"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="MONAT" SortExpression="MONAT" HeaderText="Monat">
												<HeaderStyle Font-Size="XX-Small" Font-Names="Arial"></HeaderStyle>
												<ItemStyle Font-Size="XX-Small" Font-Names="Arial" CssClass="GridTableCell"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="Anz.Versendungen" SortExpression="Anz.Versendungen" HeaderText="Anz.&lt;br&gt; Versend.">
												<HeaderStyle Font-Size="XX-Small" Font-Names="Arial"></HeaderStyle>
												<ItemStyle Font-Size="XX-Small" Font-Names="Arial" CssClass="GridTableCell"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="Durchschn.Zeitraum" SortExpression="Durchschn.Zeitraum" HeaderText="Durchschnittl.&lt;br&gt;Zeitraum/Tg.">
												<HeaderStyle Font-Size="XX-Small" Font-Names="Arial"></HeaderStyle>
												<ItemStyle Font-Size="XX-Small" Font-Names="Arial" CssClass="GridTableCell"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="Anz.Unbezahlt" SortExpression="Anz.Unbezahlt" HeaderText="Anz.&lt;br&gt;Unbezahlt">
												<HeaderStyle Font-Size="XX-Small" Font-Names="Arial"></HeaderStyle>
												<ItemStyle Font-Size="XX-Small" Font-Names="Arial" CssClass="GridTableCell"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="Proz.Unbezahlt" SortExpression="Proz.Unbezahlt" HeaderText="%">
												<HeaderStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right" BackColor="LightBlue"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="Faelligkeit" SortExpression="Faelligkeit" HeaderText="F&#228;lligkeit &lt;br&gt;(Tage)">
												<HeaderStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle Font-Size="XX-Small" Font-Names="Arial" CssClass="GridTableCell"></ItemStyle>
											</asp:BoundColumn>
											<asp:TemplateColumn SortExpression="Bez_01Tag" HeaderText="01">
												<HeaderStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right" CssClass="GridTableCell"></ItemStyle>
												<ItemTemplate>
													<asp:Label id=Label1 runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Bez_01Tag")<>"0" %>' Text='<%# DataBinder.Eval(Container, "DataItem.Bez_01Tag") %>' BorderColor="Transparent">
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="Proz_01" HeaderText="%">
												<HeaderStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right" CssClass="GridTableCell" BackColor="LightBlue"></ItemStyle>
												<ItemTemplate>
													<asp:Label id=Label12 runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Proz_01")<>"0" %>' BorderColor="Transparent" Text='<%# DataBinder.Eval(Container, "DataItem.Proz_01") %>'>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="BEZ_02Tag" HeaderText="02">
												<HeaderStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right" CssClass="GridTableCell"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label2" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Bez_02Tag")<>"0" %>' Text='<%# DataBinder.Eval(Container, "DataItem.Bez_02Tag") %>' BorderColor="Transparent">
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="Proz_02" HeaderText="%">
												<HeaderStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right" CssClass="GridTableCell" BackColor="LightBlue"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label13" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Proz_02")<>"0" %>' Text='<%# DataBinder.Eval(Container, "DataItem.Proz_02") %>' BorderColor="Transparent">
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="BEZ_03Tag" HeaderText="03">
												<HeaderStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right" CssClass="GridTableCell"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label3" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Bez_03Tag")<>"0" %>' Text='<%# DataBinder.Eval(Container, "DataItem.Bez_03Tag") %>' BorderColor="Transparent">
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="Proz_03" HeaderText="%">
												<HeaderStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right" CssClass="GridTableCell" BackColor="LightBlue"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label14" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Proz_03")<>"0" %>' Text='<%# DataBinder.Eval(Container, "DataItem.Proz_03") %>' BorderColor="Transparent">
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="BEZ_04Tag" HeaderText="04">
												<HeaderStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right" CssClass="GridTableCell"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label4" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Bez_04Tag")<>"0" %>' Text='<%# DataBinder.Eval(Container, "DataItem.Bez_04Tag") %>' BorderColor="Transparent">
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="Proz_04" HeaderText="%">
												<HeaderStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right" CssClass="GridTableCell" BackColor="LightBlue"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label15" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Proz_04")<>"0" %>' Text='<%# DataBinder.Eval(Container, "DataItem.Proz_04") %>' BorderColor="Transparent">
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="BEZ_05Tag" HeaderText="05">
												<HeaderStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right" CssClass="GridTableCell"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label5" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Bez_05Tag")<>"0" %>' Text='<%# DataBinder.Eval(Container, "DataItem.Bez_05Tag") %>' BorderColor="Transparent">
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="Proz_05" HeaderText="%">
												<HeaderStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right" CssClass="GridTableCell" BackColor="LightBlue"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label16" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Proz_05")<>"0" %>' Text='<%# DataBinder.Eval(Container, "DataItem.Proz_05") %>' BorderColor="Transparent">
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="BEZ_06Tag" HeaderText="06">
												<HeaderStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right" CssClass="GridTableCell"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label6" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Bez_06Tag")<>"0" %>' Text='<%# DataBinder.Eval(Container, "DataItem.Bez_06Tag") %>' BorderColor="Transparent">
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="Proz_06" HeaderText="%">
												<HeaderStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right" CssClass="GridTableCell" BackColor="LightBlue"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label17" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Proz_06")<>"0" %>' Text='<%# DataBinder.Eval(Container, "DataItem.Proz_06") %>' BorderColor="Transparent">
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="BEZ_07Tag" HeaderText="07">
												<HeaderStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right" CssClass="GridTableCell"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label7" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Bez_07Tag")<>"0" %>' Text='<%# DataBinder.Eval(Container, "DataItem.Bez_07Tag") %>' BorderColor="Transparent">
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="Proz_07" HeaderText="%">
												<HeaderStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right" CssClass="GridTableCell" BackColor="LightBlue"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label18" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Proz_07")<>"0" %>' Text='<%# DataBinder.Eval(Container, "DataItem.Proz_07") %>' BorderColor="Transparent">
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="BEZ_08Tag" HeaderText="08">
												<HeaderStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right" CssClass="GridTableCell"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label8" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Bez_08Tag")<>"0" %>' Text='<%# DataBinder.Eval(Container, "DataItem.Bez_08Tag") %>' BorderColor="Transparent">
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="Proz_08" HeaderText="%">
												<HeaderStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right" CssClass="GridTableCell" BackColor="LightBlue"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label19" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Proz_08")<>"0" %>' Text='<%# DataBinder.Eval(Container, "DataItem.Proz_08") %>' BorderColor="Transparent">
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="BEZ_09Tag" HeaderText="09">
												<HeaderStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right" CssClass="GridTableCell"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label9" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Bez_09Tag")<>"0" %>' Text='<%# DataBinder.Eval(Container, "DataItem.Bez_09Tag") %>' BorderColor="Transparent">
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="Proz_09" HeaderText="%">
												<HeaderStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right" CssClass="GridTableCell" BackColor="LightBlue"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label20" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Proz_09")<>"0" %>' Text='<%# DataBinder.Eval(Container, "DataItem.Proz_09") %>' BorderColor="Transparent">
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="BEZ_10Tag" HeaderText="10">
												<HeaderStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right" CssClass="GridTableCell"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label10" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Bez_10Tag")<>"0" %>' Text='<%# DataBinder.Eval(Container, "DataItem.Bez_10Tag") %>' BorderColor="Transparent">
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="Proz_10" HeaderText="%">
												<HeaderStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right" CssClass="GridTableCell" BackColor="LightBlue"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label11" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Proz_10")<>"0" %>' Text='<%# DataBinder.Eval(Container, "DataItem.Proz_10") %>' BorderColor="Transparent">
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="BEZ_11Tag" HeaderText="&gt;10">
												<HeaderStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right" CssClass="GridTableCell"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label21" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Bez_11Tag")<>"0" %>' Text='<%# DataBinder.Eval(Container, "DataItem.Bez_11Tag") %>' BorderColor="Transparent">
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="Proz_11" HeaderText="%">
												<HeaderStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle Font-Size="XX-Small" Font-Names="Arial" HorizontalAlign="Right" CssClass="GridTableCell" BackColor="LightBlue"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label22" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Proz_11")<>"0" %>' Text='<%# DataBinder.Eval(Container, "DataItem.Proz_11") %>' BorderColor="Transparent">
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
										</Columns>
										<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
									</asp:datagrid></TD>
							</TR>
							<TR>
								<TD vAlign="top" width="25">&nbsp;</TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>

