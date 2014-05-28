<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report_03.aspx.vb" Inherits="AppCCU.Report_03"%>
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
			<table width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server" Visible="False"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" noWrap colSpan="2"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;(Klärfallformular)</td>
							</TR>
							<TR>
								<TD class="TaskTitle" colSpan="2">
									<P align="right">
										<asp:hyperlink id="lnkFahrzeugsuche" runat="server" CssClass="TaskTitle" NavigateUrl="javascript:window.close()">Fenster schließen</asp:hyperlink></P>
								</TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120" height="19"></TD>
								<td vAlign="top" height="19">
									<TABLE id="Table2" class="TableFrame" cellSpacing="1" cellPadding="3" width="430" border="0">
										<TR>
											<TD colSpan="2"><asp:label id="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:label>&nbsp;</TD>
										</TR>
										<TR>
											<TD>LV-Nr:</TD>
											<TD width="100%">
												<strong>
													<asp:Label id="lblLVNr" runat="server"></asp:Label></strong></TD>
										</TR>
										<TR>
											<TD noWrap>LV beendet zum</TD>
											<TD>
												<asp:TextBox id="txtDatum" runat="server" Width="100px"></asp:TextBox>
												<asp:Image id="Image1" runat="server" ImageUrl="/Portal/Customize/CCU/info.GIF" ToolTip="Format: TT.MM.JJJJ"></asp:Image></TD>
										</TR>
										<TR>
											<TD noWrap>SB ist in Ordnung</TD>
											<TD>
												<asp:CheckBox id="cbxSB" runat="server"></asp:CheckBox>&nbsp;</TD>
										</TR>
										<TR>
											<TD noWrap>Höhe der Entschädigung im<br>
												Schadensfall ist in Ordnung</TD>
											<TD>
												<asp:CheckBox id="cbxEnt" runat="server"></asp:CheckBox>&nbsp;</TD>
										</TR>
										<TR>
											<TD noWrap>Versichererwechsel</TD>
											<TD>
												<asp:CheckBox id="cbxVers" runat="server"></asp:CheckBox>&nbsp;</TD>
										</TR>
										<TR>
											<TD noWrap>Fahrzeugwechsel</TD>
											<TD>
												<asp:CheckBox id="cbxFahrz" runat="server"></asp:CheckBox>&nbsp;</TD>
										</TR>
										<TR>
											<TD noWrap>Sonstiges</TD>
											<TD noWrap>
												<asp:TextBox id="txtBemerkung" runat="server" Width="256px" MaxLength="256"></asp:TextBox>
												<asp:Image id="Image2" runat="server" ImageUrl="/Portal/Customize/CCU/info.GIF" ToolTip="Maximal 256 Zeichen"></asp:Image></TD>
										</TR>
										<TR>
											<TD noWrap>&nbsp;</TD>
											<TD noWrap></TD>
										</TR>
										<TR>
											<TD noWrap></TD>
											<TD noWrap>
												<P align="right">
													<asp:LinkButton id="btnAbsenden" runat="server" CssClass="StandardButtonTable">&#149;&nbsp;Formular absenden</asp:LinkButton></P>
											</TD>
										</TR>
									</TABLE>
								</td>
							</TR>
							<tr>
								<td></td>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</tr>
						</TABLE>
					</td>
				</tr>
				<TR id="ShowScript" runat="server" visible="False">
					<TD>
					</TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
