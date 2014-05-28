<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report03_1.aspx.vb" Inherits="AppCommonCarRent.Report03_1" %>
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
								<TD vAlign="top"></TD>
								<TD vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td class="TaskTitle" colSpan="2"> &nbsp;</td>
										</tr>
										<TR>
											<TD align="right" colSpan="2"><asp:linkbutton id="cmdSave" runat="server" Visible="False" CssClass="StandardButton">Sichern</asp:linkbutton></TD>
										<TR>
											<TD class="" width="100%" colSpan="1"><asp:label id="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:label>
                                               
                                                <span lang="de">&nbsp;</span></TD>
											<TD class="LabelExtraLarge" align="right" nowrap="nowrap">
                                                <asp:ImageButton ID="imgbExcel" runat="server" Height="20px" ImageUrl="../../../Images/excel.gif"
                                                    Visible="True" Width="20px" /> &nbsp;
											<asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
										</TR>
										<TR>
											<TD colSpan="2"><asp:datagrid id="DataGrid1" runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True" bodyHeight="300" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" PageSize="50" BackColor="White">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														                                                       
                                                       
                                                        <asp:TemplateColumn HeaderText="col_Referenznummer">
                                                                                                                    <headertemplate>
                                                                <asp:LinkButton ID="col_Referenznummer" runat="server" CommandName="Sort" CommandArgument="Referenznummer">col_Referenznummer</asp:LinkButton>
                                                            </headertemplate>
                                                            <itemtemplate>
                                                                <asp:Label ID="Label21q" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Referenznummer") %>'>
                                                                </asp:Label>
                                                            </itemtemplate>
                                                        </asp:TemplateColumn>
                                                        
                                                   							
														
                                                        <asp:TemplateColumn HeaderText="col_KfzKennzeichen">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_KfzKennzeichen" runat="server" CommandName="Sort" CommandArgument="KfzKennzeichen">col_KfzKennzeichen</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label21q1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.KfzKennzeichen") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        
                                                        <asp:TemplateColumn HeaderText="col_EingangPhysisch">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_EingangPhysisch" runat="server" CommandName="Sort" CommandArgument="EingangPhysisch">col_EingangPhysisch</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label21q12" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.EingangPhysisch","{0:d}") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                       
                                                        <asp:TemplateColumn HeaderText="col_Abmeldeauftrag">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Abmeldeauftrag" runat="server" CommandName="Sort" CommandArgument="Abmeldeauftrag">col_Abmeldeauftrag</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label21q122" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Abmeldeauftrag") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                       
                                                        <asp:TemplateColumn HeaderText="col_AnzahlSchilder">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_AnzahlSchilder" runat="server" CommandName="Sort" CommandArgument="AnzahlSchilder">col_AnzahlSchilder</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label21q1d22" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AnzahlSchilder") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
														
														<asp:TemplateColumn  HeaderText="Form.">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
                                                                <asp:ImageButton runat="server" ID="imgbSchilder" ImageUrl="../../../Images/pdf.gif"
                                                                    Visible='<%# NOT (CStr(DataBinder.Eval(Container, "DataItem.AnzahlSchilder"))="2") %>'
                                                                    CommandArgument='<%# DataBinder.Eval(Container, "DataItem.KfzKennzeichen") %>'
                                                                    CommandName="Schilder" />
															</ItemTemplate>
														</asp:TemplateColumn>
                                                        <asp:TemplateColumn HeaderText="col_KfzSchein">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_KfzSchein" runat="server" CommandName="Sort" CommandArgument="KfzSchein">col_KfzSchein</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label21dq1sd22" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.KfzSchein") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
																												
														<asp:TemplateColumn  HeaderText="Form.">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
															<asp:ImageButton runat="server" ID="imgbSchein" ImageUrl="../../../Images/pdf.gif"
                                                                Visible='<%# NOT (CStr(DataBinder.Eval(Container, "DataItem.KfzSchein"))="X") %>'
                                                                CommandArgument='<%# DataBinder.Eval(Container, "DataItem.KfzKennzeichen") %>'
                                                                CommandName="Schein" />
																
															</ItemTemplate>
														</asp:TemplateColumn>
                                                        <asp:TemplateColumn HeaderText="col_KfzBrief">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_KfzBrief" runat="server" CommandName="Sort" CommandArgument="Kfz-Brief">col_KfzBrief</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label21dqw1sd22" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.KfzBrief") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
														
														
													</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></TD>
										</TR>
									</TABLE>
									<asp:label id="lblInfo" runat="server" Font-Bold="True" EnableViewState="False"></asp:label></TD>
							</tr>
							<tr>
								<td></td>
								<td><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
							</tr>
							<tr>
								<td></td>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</tr>
						</TABLE>
					</td>
				</tr>
				
			</table>
			
		</form>
	</body>
</HTML>
