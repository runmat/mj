<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Helpdesk01.aspx.vb" Inherits="CKG.Components.ComCommon.Helpdesk01" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Helpdesk01</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</HEAD>
	<body bgColor="#ffffff">
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" width="100%">
				<tr>
					<td>
						<TABLE id="Table4" cellSpacing="1" cellPadding="1" width="100%" border="0">
							<TR>
								<TD><uc1:header id="ucHeader" runat="server"></uc1:header></TD>
							</TR>
							<TR>
								<TD class="PageNavigation"><asp:label id="lblHead" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD class="TaskTitle">&nbsp;</TD>
							</TR>
						</TABLE>
					</td>
				</tr>
				<tr>
					<td vAlign="center" align="middle">
						<TABLE class="HelpDeskTable" id="Table2" cellSpacing="1" cellPadding="1" width="550" border="0">
							<TR>
								<TD vAlign="top" align="middle">
									<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="500" border="0" runat="server">
										<TR>
											<TD vAlign="top"><asp:label id="Label6" runat="server" Font-Bold="True" Font-Underline="True">Vorgangsart:</asp:label></TD>
											<TD><asp:radiobuttonlist id="rbVorgang" runat="server" AutoPostBack="True">
													<asp:ListItem Value="1" Selected="True">Neuanlage</asp:ListItem>
													<asp:ListItem Value="2">&#196;nderung</asp:ListItem>
													<asp:ListItem Value="3">L&#246;schung</asp:ListItem>
												</asp:radiobuttonlist></TD>
										</TR>
										<TR>
											<TD><asp:label id="Label1" runat="server" Font-Bold="True">Gruppenname:</asp:label></TD>
											<TD noWrap width="100%"><asp:textbox id="txtGroup" runat="server" Width="150px" BackColor="White"></asp:textbox><asp:dropdownlist id="ddlGroups" runat="server" AutoPostBack="True" Visible="False" Width="150px" BackColor="White"></asp:dropdownlist></TD>
										</TR>
										<TR>
											<TD colSpan="2">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="top" colSpan="2"><asp:datagrid id="dgApps" runat="server" BorderColor="Gray" AutoGenerateColumns="False" Width="100%">
													<HeaderStyle Font-Bold="True"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="AppID" SortExpression="AppID" HeaderText="AppID"></asp:BoundColumn>
														<asp:BoundColumn DataField="AppType" SortExpression="AppType" HeaderText="Typ">
															<ItemStyle Wrap="False"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="AppFriendlyName" SortExpression="AppFriendlyName" HeaderText="Anwendung">
															<ItemStyle Wrap="False"></ItemStyle>
														</asp:BoundColumn>
														<asp:TemplateColumn HeaderText="Zugriff">
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox id="cbxAccess" runat="server"></asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
												</asp:datagrid></TD>
										</TR>
										<TR>
											<TD vAlign="top" colSpan="2">&nbsp;</TD>
										</TR>
										<TR>
											<TD colSpan="2"><asp:linkbutton id="btnConfirm" runat="server" CssClass="StandardButton">Abschicken</asp:linkbutton></TD>
										</TR>
									</TABLE>
									<asp:label id="lblMessage" runat="server" Font-Bold="True" Visible="False" Font-Size="Large">Auftrag versendet.</asp:label><asp:label id="lblError" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="Red"></asp:label></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
				<tr>
					<td align="middle"><!--#include File="../../PageElements/Footer.html" --></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
