<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change50.aspx.vb" Inherits="CKG.Components.ComCommon.Change50" %>
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
	    
        <uc1:BusyIndicator ID="BusyIndicator1" runat="server" />

		<form id="Form1" method="post" runat="server">
		 
			<TABLE id="Table4" width="100%" align="center">
				<TR>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</TR>
				<TR>
					<TD>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="PageNavigation" colSpan="2" height="19"><asp:label id="lblHead" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD class="TaskTitle" colSpan="2" height="19"></TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">
									<asp:linkbutton id="cmdSearch" OnClientClick="Show_BusyBox1();" runat="server" CssClass="StandardButton"> &#149;&nbsp;Suchen</asp:linkbutton></TD>
								<TD vAlign="top">
								</TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD vAlign="top">
									<asp:RadioButtonList id="rblSearch" runat="server">
										<asp:ListItem Value="&#196;nderung nach Wiedereingang" Selected="True">&#196;nderung nach Wiedereingang</asp:ListItem>
										<asp:ListItem Value="COC">Fehlende COC-Dokumente</asp:ListItem>
										<asp:ListItem Value="AAG">Abweichender Abrufgrund</asp:ListItem>
									</asp:RadioButtonList></TD>
							</TR>
							<TR>
								<TD vAlign="top"></TD>
								<TD vAlign="top"><asp:label id="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
