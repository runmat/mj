<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report201_4.aspx.vb" Inherits="AppSIXTL.Report201_4" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="GridLayout">
		<TABLE height="679" cellSpacing="0" cellPadding="0" width="942" border="0" ms_2d_layout="TRUE">
			<TR vAlign="top">
				<TD width="942" height="679">
					<form id="Form1" method="post" runat="server">
						<TABLE height="940" cellSpacing="0" cellPadding="0" width="677" border="0" ms_2d_layout="TRUE">
							<TR vAlign="top">
								<TD width="1" height="6"></TD>
								<TD width="1"></TD>
								<TD width="188"></TD>
								<TD width="34"></TD>
								<TD width="137"></TD>
								<TD width="44"></TD>
								<TD width="32"></TD>
								<TD width="1"></TD>
								<TD width="239"></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="3" height="64"></TD>
								<TD colSpan="6">
									<asp:Label id="Label1" runat="server" Font-Names="Arial" Font-Size="Large" Height="26px" Font-Underline="True"> Versicherung an Eides Statt</asp:Label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="5" height="34"></TD>
								<TD colSpan="4" rowSpan="5">
									<asp:label id="lblError" runat="server" Font-Bold="True" Width="223px" Visible="False" ForeColor="Red" Font-Names="Arial" Font-Size="XX-Large">Fehler beim Seitenaufbau.</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD height="32"></TD>
								<TD colSpan="2">
									<asp:Label id="Label2" runat="server" Font-Names="Arial" Font-Size="Small"> Firma</asp:Label></TD>
								<TD colSpan="2" rowSpan="4"></TD>
							</TR>
							<TR vAlign="top">
								<TD height="24"></TD>
								<TD colSpan="2">
									<asp:Label id="Label10" runat="server" Font-Size="Small" Font-Names="Arial"> Sixt Leasing AG</asp:Label></TD>
							</TR>
							<TR vAlign="top">
								<TD height="32"></TD>
								<TD colSpan="2">
									<asp:Label id="Label9" runat="server" Font-Size="Small" Font-Names="Arial">Zugspitzstraße 1</asp:Label></TD>
							</TR>
							<TR vAlign="top">
								<TD height="75"></TD>
								<TD colSpan="2">
									<asp:Label id="Label11" runat="server" Font-Size="Small" Font-Names="Arial">82049 Pullach im Isartal</asp:Label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="6" height="5"></TD>
								<TD colSpan="3" rowSpan="2">
									<asp:label id="lblFahrzeugkennzeichen" runat="server" Font-Bold="True" Font-Size="Large" Width="224px">__________________</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="2" height="36"></TD>
								<TD colSpan="4">
									<asp:Label id="Label4" runat="server" Font-Names="Arial" Font-Size="Small" Width="392px" Height="21px">Betreffend des Fahrzeuges mit dem amtl. Kennzeichen</asp:Label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="4" height="5"></TD>
								<TD colSpan="5" rowSpan="2">
									<asp:label id="lblFahrgestellnummer" runat="server" Font-Size="Large" Width="201px" Font-Bold="True">__________________</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD height="43"></TD>
								<TD colSpan="3">
									<asp:Label id="Label7" runat="server" Font-Size="Small" Font-Names="Arial" Width="209px" Height="24px">und der Fahrgestellnummer</asp:Label></TD>
							</TR>
							<TR vAlign="top">
								<TD height="84"></TD>
								<TD colSpan="8">
									<asp:Label id="Label5" runat="server" Width="662px" Height="66px">Mir ist die Bedeutung einer VERSICHERUNG AN EIDES STATT und die strafrechtlichen Folgen einer<br>vorsätzlich oder fahrlässig abgegebenen unrichtigen oder unvollständigen eidesstattlichen Versicherung bekannt<br>(§156 StGB und § 27 Abs. 4 VwVfG) und habe den folgenden Text zur Kenntnis genommen.</asp:Label></TD>
							</TR>
							<TR vAlign="top">
								<TD height="42"></TD>
								<TD colSpan="8">
									<asp:Label id="Label6" runat="server" Font-Size="Medium" Width="647px" Height="29px">§ 156 Strafgesetzbuch (StGB) „Falsche Versicherung an Eides Statt“</asp:Label></TD>
							</TR>
							<TR vAlign="top">
								<TD height="64"></TD>
								<TD colSpan="8">
									<asp:Label id="Label14" runat="server" Width="668px" Height="44px">Wer vor einer zur Abnahme einer Versicherung an Eides Statt zuständigen Behörde eine solche Versicherung<br>falsch abgibtoder unter Berufung auf eine solche Versicherung falsch aussagt, wird mit Freiheitsstrafe bis zu drei<br>Jahren oder mit Geldstrafe bestraft.</asp:Label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="9" height="48">
									<asp:Label id="Label15" runat="server" Font-Size="Medium" Width="665px" Height="35px">§27 (4) Verwaltungsverfahrensgesetz (VwVfG) „ Versicherung an Eides Statt“</asp:Label></TD>
							</TR>
							<TR vAlign="top">
								<TD height="73"></TD>
								<TD colSpan="8">
									<asp:Label id="Label16" runat="server" Width="596px" Height="70px">Vor der Aufnahme der Versicherung an Eides Statt ist der Versichernde über die Bedeutung der<br>eidesstattlichen Versicherung und die strafrechtlichen Folgen einer unrichtigen oder unvollständigen<br>eidesstattlichen Versicherung zu belehren. Die Belehrung ist in der Niederschrift zu vermerken.</asp:Label></TD>
							</TR>
							<TR vAlign="top">
								<TD height="32"></TD>
								<TD colSpan="8">
									<asp:Label id="Label18" runat="server" Font-Size="Medium" Width="598px" Height="24px">Erklärung des Versichernden:</asp:Label></TD>
							</TR>
							<TR vAlign="top">
								<TD height="127"></TD>
								<TD colSpan="8">
									<asp:Label id="Label17" runat="server" Width="660px" Height="118px">Hiermit erkläre ich gemäß § 5 Straßenverkehrsgesetz in Verbindung mit § 27 des<br>Verwaltungsverfahrensgesetzes an Eides Statt, dass mir der Fahrzeugschein bzw. die Zulassungsbescheinigung<br>Teil I offenkundig abhanden gekommen ist,  da ich diesen trotz intensiver Suche nicht mehr auffinden konnte.<br>Ich bestätige hiermit die Vollständigkeit der abgegebenen Erklärung und versichere an Eides Statt, dass ich<br>nach bestem Wissen die reine Wahrheit gesagt und nichts verschwiegen habe. Des weiteren versichere ich,<br>dass das o. g. Dokument bei keiner Dritten Person hinterlegt worden ist.</asp:Label></TD>
							</TR>
							<TR vAlign="top">
								<TD height="68"></TD>
								<TD colSpan="8">
									<asp:Label id="Label19" runat="server" Width="638px" Height="51px">Ferner ist mir bewusst, dass das Dokument ungültig und bei Wiederauffinden bei der Kfz-Zulassungsstelle<br> unverzüglich abzugeben ist.</asp:Label></TD>
							</TR>
							<TR vAlign="top">
								<TD height="29"></TD>
								<TD colSpan="3">
									<asp:label id="Label8" runat="server" Font-Bold="True">.......................................................</asp:label></TD>
								<TD colSpan="3"></TD>
								<TD colSpan="2">
									<asp:label id="Label12" runat="server" Font-Bold="True" Width="224px">.......................................................</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD height="17"></TD>
								<TD colSpan="3">
									<asp:Label id="Label3" runat="server" Font-Names="Arial" Font-Size="X-Small" Width="217px"> Ort, Datum</asp:Label></TD>
								<TD colSpan="4"></TD>
								<TD>
									<asp:Label id="Label13" runat="server" Font-Size="X-Small" Font-Names="Arial" Width="219px">Unterschrift des Versichernden</asp:Label></TD>
							</TR>
						</TABLE>
					</form>
				</TD>
			</TR>
		</TABLE>
	</body>
</HTML>
