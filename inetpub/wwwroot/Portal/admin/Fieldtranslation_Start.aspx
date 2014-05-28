<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Fieldtranslation_Start.aspx.vb" Inherits="CKG.Admin.Fieldtranslation_Start" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
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
						<TABLE id="Table4" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="PageNavigation" colSpan="2">
									<asp:label id="lblHead" runat="server">Administration</asp:label>
									<asp:label id="lblPageTitle" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">
									<TABLE id="Table5" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center"><asp:linkbutton id="cmdback" runat="server" 
                                                    CssClass="StandardButton">•&nbsp;zurück</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<TD vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;</TD>
										</TR>
									</TABLE>
								<table id="table7" bgcolor="white" border="0" height="30">
                                        <tr>
                                            <td valign="bottom" width="127">
                                                Anwendung:</td>
                                            <td valign="bottom">
                                                <asp:TextBox ID="txtFilterAppName" runat="server" Height="20px" Width="160px">*</asp:TextBox>
                                            </td>
                                            <td valign="bottom">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="bottom" width="127">
                                                Freundlicher Name:</td>
                                            <td valign="bottom">
                                                <asp:TextBox ID="txtFilterAppFriendlyName" runat="server" Height="20px" 
                                                    Width="160px">*</asp:TextBox>
                                            </td>
                                            <td valign="bottom">
                                                <asp:Button ID="btnSuche" runat="server" CssClass="StandardButton" 
                                                    Text="Suchen" />
                                            </td>
                                        </tr>
                                    </table>
                                </TD>
							</TR>
							
						<tr id="trSearchResult"   runat="server">
                            <td>
                            </td>
                                    <td align="left">
                                    <br />
                                        <asp:DataGrid ID="DataGrid1" runat="server" Width="100%" AllowPaging="True"
                                            AutoGenerateColumns="False" AllowSorting="True" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="1px" BackColor="White">
                                            <SelectedItemStyle BackColor="#FFE0C0"></SelectedItemStyle>
                                            <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" CssClass="GridTableHead" VerticalAlign="Top">
                                            </HeaderStyle>
                                            <Columns>
                                                <asp:BoundColumn Visible="true" DataField="AppID" SortExpression="AppID" HeaderText="Application ID">
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="Anwendung" SortExpression="AppName">
                                               <ItemTemplate>
                                                
                                                <asp:LinkButton  runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AppName")%>' CommandName="goToFieldTranslation"  ID="lbAppLink" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.AppUrl")%>'></asp:LinkButton>
                                              
                                                </ItemTemplate>
                                                </asp:TemplateColumn>
                                                                                              <asp:BoundColumn DataField="AppFriendlyName" SortExpression="AppFriendlyName" HeaderText="Freundlicher Name">
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn SortExpression="AppInMenu" HeaderText="im Men&#252;">
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.AppInMenu") %>'
                                                            Enabled="False"></asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="AppType" SortExpression="AppType" HeaderText="Typ"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="AppParentName" SortExpression="AppParentName" HeaderText="geh&#246;rt zu">
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn SortExpression="BatchAuthorization" HeaderText="Sammel-&lt;br&gt;autorisierung">
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CheckBox2" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.BatchAuthorization") %>'>
                                                        </asp:CheckBox>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BatchAuthorization") %>'>
                                                        </asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateColumn>
                                              </Columns>
                                            <PagerStyle Mode="NumericPages"></PagerStyle>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
												
							<TR>
								<TD>&nbsp;</TD>
								<TD vAlign="top" align="left"><asp:label id="lblError" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD>&nbsp;</TD>
								<TD vAlign="top" align="left"><!--#include File="../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
		</body>
</HTML>
