<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change04_1.aspx.vb" Inherits="AppF1.Change04_1" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BusyIndicator" Src="../../../PageElements/BusyIndicator.ascx" %>

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
    
        <uc1:BusyIndicator runat="server" />

		<form id="Form1" method="post" runat="server">
		  
                    
                    
			<table cellSpacing="0" cellPadding="0" width="100%" align="center">
				<tr>
					<td colSpan="3"><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<TR>
					<TD colSpan="3">
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2" height="19"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;(<asp:label id="lblPageTitle" runat="server"></asp:label>)</td>
							</TR>
							<TR>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdSave" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Sichern</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdConfirm"  OnClientClick="Show_BusyBox1();" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Bestätigen</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdReset" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Verwerfen</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdAuthorize"  OnClientClick="Show_BusyBox1();" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Autorisieren</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdDelete" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Löschen</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdBack" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Zurück</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:hyperlink id="cmdBack2" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Zurück</asp:hyperlink></TD>
										</TR>
									</TABLE>
								</TD>
								<TD>
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top"><asp:hyperlink id="lnkKreditlimit_old" runat="server" Visible="False" NavigateUrl="Change01.aspx" CssClass="TaskTitle">Händlersuche</asp:hyperlink>&nbsp;<asp:hyperlink id="lnkKreditlimit" runat="server" NavigateUrl="Change01.aspx" CssClass="TaskTitle">Händlersuche</asp:hyperlink>&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
										<tr>
											<td class="TextLarge" vAlign="top">Händlernummer:</td>
											<td class="TextLarge" vAlign="top" width="100%" colSpan="2"><asp:label id="lblHaendlerNummer" runat="server"></asp:label></td>
										</tr>
										<tr>
											<td class="StandardTableAlternate" vAlign="top">Name:&nbsp;&nbsp;
											</td>
											<td class="StandardTableAlternate" vAlign="top" colSpan="2"><asp:label id="lblHaendlerName" runat="server"></asp:label></td>
										</tr>
										<tr>
											<td class="TextLarge" vAlign="top">Adresse:</td>
											<td class="TextLarge" vAlign="top" colSpan="2"><asp:label id="lblAdresse" runat="server"></asp:label></td>
										</tr>
									</TABLE>
									<TABLE id="Table7" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr><td><br /></td></tr>
										<tr>
											<td><asp:Label runat="server"   CssClass="TextLarge" ID="lblInfoKontingentart" Text=""></asp:Label></td>
										</tr>
										<tr><td><br /></td></tr>
										<TR>
											<TD><asp:datagrid id="DataGrid1" runat="server" BackColor="White" Width="100%" AutoGenerateColumns="False" CellPadding="3">
													<AlternatingItemStyle CssClass="StandardTableAlternate"></AlternatingItemStyle>
													<ItemStyle CssClass="TextLarge"></ItemStyle>
													<HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="KKBER" HeaderText="Kreditkontrollbereich"></asp:BoundColumn>
														<asp:BoundColumn Visible="true" DataField="KontingentArt" HeaderText="Kontingentart"></asp:BoundColumn>
														
														<asp:TemplateColumn HeaderText="Altes Kontingent">
															<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
															<ItemStyle HorizontalAlign="Right"></ItemStyle>
															<ItemTemplate>
																<asp:Label id=lblKontingent_Alt runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.KLIMK","{0:N0}") %>  ' >
																</asp:Label>
																														</ItemTemplate>
															
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="SKFOR" HeaderText="Inanspruchnahme" DataFormatString="{0:N0}">
															<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
															<ItemStyle HorizontalAlign="Right"></ItemStyle>
														</asp:BoundColumn>
														<asp:TemplateColumn HeaderText="Freies Kontingent">
															<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
															<ItemStyle HorizontalAlign="Right"></ItemStyle>
															<ItemTemplate>
																<asp:Label id=lblFrei runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FREIKONTI","{0:N0}") %>' >
																</asp:Label>
															</ItemTemplate>
															
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="Gesperrt - Alt">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox id=Gesperrt_Alt runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.CRBLB")="X" %>' Enabled="False">
																</asp:CheckBox>
															</ItemTemplate>
															
														</asp:TemplateColumn>
														<asp:TemplateColumn  HeaderText="Neues Kontingent">
															<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
															<ItemStyle HorizontalAlign="Right"></ItemStyle>
															<ItemTemplate    >
																<asp:Image id="Image2" runat="server" Width="12px" Height="12px" ImageUrl="/Portal/Images/empty.gif"></asp:Image>
																<asp:TextBox id="txtKontingent_Neu" runat="server" CssClass="InputRight" Width="50px" Text='<%# DataBinder.Eval(Container, "DataItem.EingabeKontingent","{0:N0}") %>' >
																</asp:TextBox>
																
															</ItemTemplate>
															
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="Gesperrt - Neu ">
															<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:Image id="imgGesperrt_Neu" runat="server" Width="12px" Height="12px" ImageUrl="/Portal/Images/empty.gif"></asp:Image>
																<asp:CheckBox id="chkGesperrt_Neu" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.EingabeGesperrt")="X" %>'>
																</asp:CheckBox>
															</ItemTemplate>
															
														</asp:TemplateColumn>
														
													</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Center" Wrap="False"></PagerStyle>
												</asp:datagrid></TD>
										</TR>
									</TABLE>
									<TABLE id="tblGruppenHaendler" runat="server" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
										<td>
										<br />
										</td>
										</tr>
										<tr>
										<td align="center" >
										<asp:Label runat="server"   CssClass="TextLarge" ID="lblInfoHaendler" Text="folgende Händler gehören zu dieser Gruppe"></asp:Label>
										
										</td>
										</tr>
										<tr>
										<td>
										<br />
										</td>
										</tr>
										
										<TR id="TR1" runat="server" align="center">
										<td>
										
											<asp:DataGrid  ID="DTGHaendler" ShowHeader="false" AutoGenerateColumns="false" BackColor="White" Width="50%" runat="server" >
																								<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													
											<Columns>
											<asp:BoundColumn DataField="HAENDLER_EX"  ></asp:BoundColumn>
											<asp:BoundColumn DataField="HaendlerAdresse" ></asp:BoundColumn>
											</Columns>
											</asp:DataGrid>
											</td>
										</TR>
										
									</TABLE>
									
									<TABLE id="Table8" cellSpacing="0" cellPadding="5" width="100%" border="0">
										<TR id="ConfirmMessage" runat="server">
											<TD class="LabelExtraLarge"><asp:label id="lblInformation" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<tr>
								<td>&nbsp;</td>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</tr>
						</TABLE>
					</TD>
				</TR>
				
			</table>
		</form>
	</body>
</HTML>

