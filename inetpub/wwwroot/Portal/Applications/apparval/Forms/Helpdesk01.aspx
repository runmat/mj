<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Helpdesk01.aspx.vb" Inherits="AppARVAL.Helpdesk01"%>
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
						<TABLE class="HelpDeskTable" id="Table2" cellSpacing="1" cellPadding="1" border="0">
							<TR>
								<TD vAlign="top" align="middle">
									<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server" height="0">
										<TR>
											<TD vAlign="top" noWrap class="TableBackground"><STRONG>&nbsp;Ihre DAD Hotline-Nummer*</STRONG></TD>
											<TD class="TableBackground">&nbsp;</TD>
											<TD class="TableBackground"></TD>
										</TR>
										<TR>
											<TD></TD>
											<TD noWrap>&nbsp;</TD>
											<TD noWrap width="100%">
												<P><FONT face="Courier New" size="4">01805 - 2 7 8 2 5 1<br>
														01805 - A R V A L 1</FONT></P>
											</TD>
										</TR>
										<TR>
											<TD>&nbsp;</TD>
											<TD noWrap>&nbsp;</TD>
											<TD noWrap width="100%"></TD>
										</TR>
										<TR>
											<TD class="TableBackground"><STRONG>&nbsp;Unsere Servicezeiten</STRONG></TD>
											<TD class="TableBackground">&nbsp;</TD>
											<TD class="TableBackground"></TD>
										</TR>
										<TR>
											<TD></TD>
											<TD noWrap>&nbsp;</TD>
											<TD noWrap width="100%">Montag bis Freitag 08:00 - 17:30</TD>
										</TR>
										<TR>
											<TD>&nbsp;</TD>
											<TD noWrap>&nbsp;</TD>
											<TD noWrap width="100%"></TD>
										</TR>
										<TR>
											<TD align="right">
												<asp:Image id="Image1" runat="server" ImageUrl="/Portal/Images/DADlogo.gif" BorderStyle="None"></asp:Image></TD>
											<TD vAlign="center">&nbsp;</TD>
											<TD width="100%" vAlign="center">DAD DEUTSCHER AUTO DIENST GmbH<br>
												Bogenstrasse 26<br>
												22926 Ahrensburg</TD>
										</TR>
										<TR>
											<TD>&nbsp;</TD>
											<TD noWrap>&nbsp;</TD>
											<TD noWrap width="100%"></TD>
										</TR>
										<TR>
											<TD align="right"></TD>
											<TD noWrap>&nbsp;</TD>
											<TD noWrap width="100%">
												<asp:Image id="Image3" runat="server" ImageUrl="/Portal/Images/umschlag.gif"></asp:Image>&nbsp;<A href="mailto:arval@dad.de">arval@dad.de</A></TD>
										</TR>
										<TR>
											<TD align="left">*<FONT size="1">12 Cent / Minute</FONT></TD>
											<TD noWrap>&nbsp;</TD>
											<TD noWrap width="100%">
												<asp:Image id="Image2" runat="server" ImageUrl="/Portal/Images/globus.gif"></asp:Image>&nbsp;<A href="http://www.dad.de" target="_blank">http://www.dad.de</A></TD>
										</TR>
									</TABLE>
									<asp:Label id="lblError" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="Red"></asp:Label>
								</TD>
							</TR>
						</TABLE>
					</td>
				</tr>
				<tr>
					<td align="middle"><!--#include File="../../../PageElements/Footer.html" --></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
