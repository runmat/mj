<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report06.aspx.vb" Inherits="AppALD.Report06" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:Styles id="ucStyles" runat="server"></uc1:Styles>
	</HEAD>
	<body topmarign="0" leftmargin="0">
		<form id="Form1" method="post" runat="server">
			<table width="750" align="center" cellpadding="10" cellspacing="0" border="0">
				<tr>
					<td><uc1:Header id="ucHeader" runat="server"></uc1:Header></td>
				</tr>
				<tr>
					<td class="TextMainHeader" vAlign="top" align="left">
						<asp:Label id="lblHead" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td class="TextExtraLarge" vAlign="top" align="left">&nbsp;&nbsp;&nbsp;
						<BR>
						Bitte schränken Sie den Suchzeitraum ein. Beachten Sie hierbei, dass Recherchen 
						über Zeiträume, die länger als drei Monate zurückliegen nicht möglich sind.<BR>
						Für Abfragen über einen längeren Zeitraum wenden Sie sich bitte an Ihren 
						Kundenbetreuer.<br>
						&nbsp;&nbsp; </B></FONT></td>
				</tr>
				<TR>
					<TD vAlign="top" align="left">
						<asp:radiobutton id="rdbAlle" runat="server" Text="Alle Aufträge" Checked="True" GroupName="rdbAll"></asp:radiobutton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<asp:radiobutton id="rdbKlaerfall" runat="server" Text="Nur Klärfälle" GroupName="rdbAll"></asp:radiobutton><BR>
						&nbsp;&nbsp;&nbsp;&nbsp;</TD>
				</TR>
				<TR>
					<TD vAlign="top" align="left">von:
						<asp:textbox id="txtVonDatum" runat="server" MaxLength="10">TTMMJJJJ</asp:textbox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
						bis:
						<asp:textbox id="txtBisDatum" runat="server" MaxLength="10">TTMMJJJJ</asp:textbox></TD>
				</TR>
				<TR>
					<TD vAlign="top" align="left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					</TD>
				</TR>
				<TR>
					<TD vAlign="top" align="left"><asp:hyperlink id="lnkExcel" runat="server" NavigateUrl="Report06_2.aspx?Show=EXCEL" Target="_blank" Visible="False" CssClass="TextHeader">Excel-Version</asp:hyperlink>&nbsp;&nbsp;
						<asp:label id="lblDownloadTip" runat="server" Visible="False" Font-Bold="True" Font-Size="8pt">rechte Maustaste => Ziel speichern unter...</asp:label></TD>
				</TR>
				<TR>
					<TD vAlign="top" align="left"><asp:hyperlink id="lnkHTML" runat="server" NavigateUrl="Report06_2.aspx?Show=HTML" Target="_blank" Visible="False" CssClass="TextHeader">HTML-Version</asp:hyperlink></TD>
				</TR>
				<TR>
					<TD vAlign="top" align="left"><asp:button id="cmdReport" runat="server" Text="Report erzeugen" CssClass="StandardButton"></asp:button></TD>
				</TR>
				<TR>
					<TD vAlign="top" align="left"><asp:label id="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:label></TD>
				</TR>
				<tr>
					<td><!--#include File="../../../PageElements/Footer.html" --></td>
				</tr>
			</table>
		</form>
		<script language="JavaScript">
<!-- //
 window.document.Form1.txtVonDatum.focus();
 window.document.Form1.txtVonDatum.select();
//-->
		</script>
	</body>
</HTML>
