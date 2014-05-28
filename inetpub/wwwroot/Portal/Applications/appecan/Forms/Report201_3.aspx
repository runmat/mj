<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report201_3.aspx.vb" Inherits="AppECAN.Report201_3" %>
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
		<TABLE height="735" cellSpacing="0" cellPadding="0" width="965" border="0" ms_2d_layout="TRUE">
			<TR vAlign="top">
				<TD width="965" height="735">
					<form id="Form1" method="post" runat="server">
						<TABLE height="963" cellSpacing="0" cellPadding="0" width="733" border="0" ms_2d_layout="TRUE">
							<TR>
								<TD width="1" height="0"></TD>
								<TD width="1" height="0"></TD>
								<TD width="1" height="0"></TD>
								<TD width="1" height="0"></TD>
								<TD width="1" height="0"></TD>
								<TD width="1" height="0"></TD>
								<TD width="4" height="0"></TD>
								<TD width="4" height="0"></TD>
								<TD width="26" height="0"></TD>
								<TD width="40" height="0"></TD>
								<TD width="77" height="0"></TD>
								<TD width="1" height="0"></TD>
								<TD width="1" height="0"></TD>
								<TD width="1" height="0"></TD>
								<TD width="1" height="0"></TD>
								<TD width="1" height="0"></TD>
								<TD width="1" height="0"></TD>
								<TD width="218" height="0"></TD>
								<TD width="8" height="0"></TD>
								<TD width="26" height="0"></TD>
								<TD width="1" height="0"></TD>
								<TD width="1" height="0"></TD>
								<TD width="5" height="0"></TD>
								<TD width="111" height="0"></TD>
								<TD width="200" height="0"></TD>
							</TR>
							<TR vAlign="top">
								<TD height="28"></TD>
								<TD colSpan="10"><asp:label id="Label17" runat="server" Font-Size="Smaller" Font-Names="Arial">Fahrzeug- und Aufbauart</asp:label></TD>
								<TD colSpan="10"></TD>
								<TD colSpan="3"><asp:label id="Label1" runat="server" Font-Size="Smaller" Font-Names="Arial">Den</asp:label></TD>
								<TD rowSpan="3"></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="20" height="24"></TD>
								<TD colSpan="4"><asp:label id="Label2" runat="server" Font-Size="Smaller" Font-Names="Arial">Telefon</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="21" height="28"></TD>
								<TD colSpan="3"><asp:label id="Label3" runat="server" Font-Size="Smaller" Font-Names="Arial">BN</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="21" height="24"></TD>
								<TD colSpan="4"><asp:label id="Label4" runat="server" Font-Size="Smaller" Font-Names="Arial">Uhrzeit: __________________________</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="18" height="24"><asp:label id="Label5" runat="server" Font-Size="XX-Small" Font-Names="Arial">(Bei Antwortschreiben bitte auch das amtl. Kennzeichen angeben)</asp:label></TD>
								<TD colSpan="7" rowSpan="2"></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="9" height="4"></TD>
								<TD colSpan="9" rowSpan="2">
									<asp:label id="Label45" runat="server" Font-Names="Arial" Font-Size="Small">Stadt Flensburg<br>FB 1/Kfz-Zulassung und Führerscheinstelle<br>Rudolf-Diesel-Str. 3a<br>24941 Flensburg</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="9" height="96"></TD>
								<TD colSpan="4"></TD>
								<TD colSpan="3" rowSpan="6"><asp:label id="lblError" runat="server" Font-Size="XX-Large" Font-Names="Arial" ForeColor="Red" Visible="False" Width="223px" Font-Bold="True">Fehler beim Seitenaufbau.</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="2" height="2"></TD>
								<TD colSpan="8" rowSpan="2"><asp:label id="Label6" runat="server" Font-Size="Small" Font-Names="Arial" Font-Bold="True">Anzeige</asp:label></TD>
								<TD colSpan="12"></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="2" height="29"></TD>
								<TD colSpan="8"><asp:label id="Label7" runat="server" Font-Size="Smaller" Font-Names="Arial">(Verdacht einer Straftat liegt nicht vor)</asp:label></TD>
								<TD colSpan="4"></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="3" height="15"></TD>
								<TD colSpan="6"><asp:label id="Label10" runat="server" Font-Size="XX-Small" Font-Names="Arial">Betreff</asp:label></TD>
								<TD colSpan="13"></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="3" height="23"></TD>
								<TD colSpan="15"><asp:label id="Label9" runat="server" Font-Size="Smaller" Font-Names="Arial">Verlust von Fahrzeug-Kennzeichen</asp:label></TD>
								<TD colSpan="4"></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="4" height="16"></TD>
								<TD colSpan="5"><asp:label id="Label12" runat="server" Font-Size="XX-Small" Font-Names="Arial">Bezug</asp:label></TD>
								<TD colSpan="13"></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="5" height="39"></TD>
								<TD colSpan="6"><asp:label id="Label11" runat="server" Font-Size="Smaller" Font-Names="Arial">PDV 350 - 21.3050.2</asp:label></TD>
								<TD colSpan="14"></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="24" height="1"></TD>
								<TD rowSpan="3"><asp:checkbox id="CheckBox2" runat="server" Text="   hinteres" Enabled="False" Checked="True"></asp:checkbox></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="17" height="4"></TD>
								<TD colSpan="6" rowSpan="2"><asp:label id="lblFahrzeugkennzeichen" runat="server" Font-Bold="True">________________________________</asp:label></TD>
								<TD rowSpan="2"><asp:checkbox id="CheckBox1" runat="server" Text="   vorderes" Enabled="False" Checked="True"></asp:checkbox></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="4" height="20"></TD>
								<TD colSpan="7"><asp:label id="Label13" runat="server" Font-Size="Smaller" Font-Names="Arial">Fahrzeugkennzeichen</asp:label></TD>
								<TD colSpan="6"></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="16" height="6"></TD>
								<TD colSpan="7" rowSpan="2"><asp:label id="Label15" runat="server" Font-Bold="True">________________________________</asp:label></TD>
								<TD colSpan="2" rowSpan="2"></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="3" height="30"></TD>
								<TD colSpan="7"><asp:label id="Label14" runat="server" Font-Size="Smaller" Font-Names="Arial">Verlustort</asp:label></TD>
								<TD colSpan="6"></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="4" height="25"></TD>
								<TD colSpan="7"><asp:label id="Label16" runat="server" Font-Size="Smaller" Font-Names="Arial" Font-Bold="True">Fahrzeugdaten</asp:label></TD>
								<TD colSpan="14"></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="16" height="1"></TD>
								<TD colSpan="9" rowSpan="2"><asp:label id="lblFahrzeugUndAufbauart" runat="server" Font-Bold="True">___________________________________________________________________</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="4" height="22"></TD>
								<TD colSpan="7"><asp:label id="Label19" runat="server" Font-Size="Smaller" Font-Names="Arial">Fahrzeug- und Aufbauart</asp:label></TD>
								<TD colSpan="5"></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="15" height="3"></TD>
								<TD colSpan="10" rowSpan="2"><asp:label id="lblHersteller" runat="server" Font-Bold="True">___________________________________________________________________</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="4" height="20"></TD>
								<TD colSpan="6"><asp:label id="Label23" runat="server" Font-Size="Smaller" Font-Names="Arial">Hersteller</asp:label></TD>
								<TD colSpan="5"></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="15" height="3"></TD>
								<TD colSpan="10" rowSpan="2"><asp:label id="lblTypUndAusfuehrung" runat="server" Font-Bold="True">___________________________________________________________________</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="4" height="21"></TD>
								<TD colSpan="7"><asp:label id="Label25" runat="server" Font-Size="Smaller" Font-Names="Arial">Typ und Ausführung</asp:label></TD>
								<TD colSpan="4"></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="14" height="3"></TD>
								<TD colSpan="11" rowSpan="2"><asp:label id="lblFIN" runat="server" Font-Bold="True">___________________________________________________________________</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="4" height="30"></TD>
								<TD colSpan="7"><asp:label id="Label24" runat="server" Font-Size="Smaller" Font-Names="Arial">Fahrzeug-Ident.-Nr.</asp:label></TD>
								<TD colSpan="3"></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="6" height="23"></TD>
								<TD colSpan="5"><asp:label id="Label20" runat="server" Font-Size="Smaller" Font-Names="Arial" Font-Bold="True">Anzeigender</asp:label></TD>
								<TD colSpan="14"></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="13" height="2"></TD>
								<TD colSpan="12" rowSpan="2"><asp:label id="Label29" runat="server" Font-Bold="True">___________________________________________________________________</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="4" height="19"></TD>
								<TD colSpan="7"><asp:label id="Label21" runat="server" Font-Size="Smaller" Font-Names="Arial">Name / Geburtsname</asp:label></TD>
								<TD colSpan="2"></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="13" height="5"></TD>
								<TD colSpan="12" rowSpan="2"><asp:label id="Label30" runat="server" Font-Bold="True">___________________________________________________________________</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="6" height="19"></TD>
								<TD colSpan="4"><asp:label id="Label22" runat="server" Font-Size="Smaller" Font-Names="Arial">Vornamen</asp:label></TD>
								<TD colSpan="3"></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="12" height="5"></TD>
								<TD colSpan="13" rowSpan="2"><asp:label id="Label32" runat="server" Font-Bold="True">___________________________________________________________________</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="5" height="20"></TD>
								<TD colSpan="5"><asp:label id="Label26" runat="server" Font-Size="Smaller" Font-Names="Arial">geb. am / in</asp:label></TD>
								<TD colSpan="2"></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="13" height="4"></TD>
								<TD colSpan="12" rowSpan="2"><asp:label id="Label33" runat="server" Font-Bold="True">___________________________________________________________________</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="6" height="22"></TD>
								<TD colSpan="4"><asp:label id="Label27" runat="server" Font-Size="Smaller" Font-Names="Arial">wohnhaft</asp:label></TD>
								<TD colSpan="3"></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="12" height="4"></TD>
								<TD colSpan="13" rowSpan="2"><asp:label id="Label31" runat="server" Font-Bold="True">___________________________________________________________________</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="5" height="31"></TD>
								<TD colSpan="6"><asp:label id="Label28" runat="server" Font-Size="Smaller" Font-Names="Arial">Telefon privat / dienstlich</asp:label></TD>
								<TD></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="5" height="20"></TD>
								<TD colSpan="6"><asp:label id="Label34" runat="server" Font-Size="Smaller" Font-Names="Arial" Font-Bold="True">Fahrzeughalter</asp:label></TD>
								<TD colSpan="14"></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="14" height="3"></TD>
								<TD colSpan="11" rowSpan="2"><asp:label id="lblName" runat="server" Font-Bold="True">___________________________________________________________________</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="5" height="22"></TD>
								<TD colSpan="6"><asp:label id="Label35" runat="server" Font-Size="Smaller" Font-Names="Arial">Name / Geburtsname</asp:label></TD>
								<TD colSpan="3"></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="12" height="4"></TD>
								<TD colSpan="13" rowSpan="2"><asp:label id="Label37" runat="server" Font-Bold="True">___________________________________________________________________</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="6" height="21"></TD>
								<TD colSpan="4"><asp:label id="Label36" runat="server" Font-Size="Smaller" Font-Names="Arial">geb. am / in</asp:label></TD>
								<TD colSpan="2"></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="11" height="2"></TD>
								<TD colSpan="14" rowSpan="2"><asp:label id="lblWohnhaft" runat="server" Font-Bold="True">___________________________________________________________________</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="6" height="22"></TD>
								<TD colSpan="4"><asp:label id="Label38" runat="server" Font-Size="Smaller" Font-Names="Arial">wohnhaft</asp:label></TD>
								<TD></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="13" height="3"></TD>
								<TD colSpan="12" rowSpan="2"><asp:label id="Label40" runat="server" Font-Bold="True">___________________________________________________________________</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="5" height="31"></TD>
								<TD colSpan="6"><asp:label id="Label39" runat="server" Font-Size="Smaller" Font-Names="Arial">Telefon privat / dienstlich</asp:label></TD>
								<TD colSpan="2"></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="5" height="24"></TD>
								<TD colSpan="13"><asp:label id="Label41" runat="server" Font-Size="Smaller" Font-Names="Arial" Font-Bold="True">Erklärung des Anzeigenden</asp:label></TD>
								<TD colSpan="7"></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="5" height="48"></TD>
								<TD colSpan="20"><asp:label id="Label42" runat="server" Font-Size="Smaller" Font-Names="Arial" Width="609px" Height="39px">Das / die Kennzeichen wurden vom Halter bei Rückgabe des Leasingfahrzeuges, das die Firma ECAN im Auftrag der Eigentümerin des Fahrzeuges, der CC Bank, verwertet, trotz Aufforderung nicht zurück gegeben. Der Halter ist nicht bereit, eine Verlusterklärung abzugeben.</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="5" height="33"></TD>
								<TD colSpan="20">
									<asp:label id="Label46" runat="server" Font-Names="Arial" Font-Size="Smaller" Width="606px" Height="28px">Ich werde die Zulassungsstelle für Kraftfahrzeuge umgehend verständigen, wenn ich das / die Kennzeichen ohne deren Mitwirkung zurückerhalten sollte.</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="18" height="1"></TD>
								<TD colSpan="7" rowSpan="2"><asp:label id="Label43" runat="server" Font-Bold="True">______________________________</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="7" height="19"></TD>
								<TD colSpan="11"><asp:label id="Label44" runat="server" Font-Bold="True">______________________________</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="19" height="1"></TD>
								<TD colSpan="5" rowSpan="2"><asp:label id="Label8" runat="server" Font-Size="XX-Small" Font-Names="Arial">Unterschrift des Anzeigenden</asp:label></TD>
								<TD rowSpan="2"></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="8" height="14"></TD>
								<TD colSpan="10"><asp:label id="Label18" runat="server" Font-Size="XX-Small" Font-Names="Arial">Unterschrift des Sachbearbeiters</asp:label></TD>
								<TD></TD>
							</TR>
						</TABLE>
					</form>
				</TD>
			</TR>
		</TABLE>
	</body>
</HTML>
