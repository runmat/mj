<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report201_4.aspx.vb" Inherits="AppECAN.Report201_4" %>
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
	<body leftMargin="0" topMargin="0" MS_POSITIONING="GridLayout">
		<TABLE height="710" cellSpacing="0" cellPadding="0" width="571" border="0" ms_2d_layout="TRUE">
			<TR vAlign="top">
				<TD width="571" height="710">
					<form id="Form1" method="post" runat="server">
						<TABLE height="569" cellSpacing="0" cellPadding="0" width="708" border="0" ms_2d_layout="TRUE">
							<TR vAlign="top">
								<TD width="3" height="16"></TD>
								<TD width="2"></TD>
								<TD width="3"></TD>
								<TD width="118"></TD>
								<TD width="266"></TD>
								<TD width="316"></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="3" height="54"></TD>
								<TD colSpan="3">
									<asp:Label id="Label1" runat="server" Font-Names="Arial" Font-Size="Large">Erklärung über den Verbleib eines Fahrzeugscheines</asp:Label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="5" height="34"></TD>
								<TD rowSpan="5">
									<asp:label id="lblError" runat="server" Font-Bold="True" Width="223px" Visible="False" ForeColor="Red" Font-Names="Arial" Font-Size="XX-Large">Fehler beim Seitenaufbau.</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="3" height="32"></TD>
								<TD colSpan="2">
									<asp:Label id="Label2" runat="server" Font-Names="Arial" Font-Size="Small">Die Firma</asp:Label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="3" height="24"></TD>
								<TD colSpan="2">
									<asp:Label id="Label10" runat="server" Font-Size="Small" Font-Names="Arial"> CC-Bank AG</asp:Label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="3" height="32"></TD>
								<TD colSpan="2">
									<asp:Label id="Label9" runat="server" Font-Size="Small" Font-Names="Arial">Kaiserstraße 74</asp:Label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="3" height="80"></TD>
								<TD colSpan="2">
									<asp:Label id="Label11" runat="server" Font-Size="Small" Font-Names="Arial">D-41061 Mönchengladbach</asp:Label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="3" height="45"></TD>
								<TD colSpan="3">
									<asp:Label id="Label4" runat="server" Font-Names="Arial" Font-Size="Small">erklärt als Eigentümerin des Fahrzeuges mit dem amtlichen Kennzeichen</asp:Label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="4" height="51"></TD>
								<TD colSpan="2">
									<asp:label id="lblFahrzeugkennzeichen" runat="server" Font-Bold="True" Font-Size="Large">________________________________,</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="3" height="48"></TD>
								<TD colSpan="3">
									<asp:Label id="Label5" runat="server" Font-Names="Arial" Font-Size="Small">dass der Fahrzeugschein zur Stillegung nicht vorgelegt werden kann,<br>da der Halter die Herausgabe an uns als Eigentümerin verweigert.</asp:Label></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="3" height="111"></TD>
								<TD colSpan="3">
									<asp:Label id="Label6" runat="server" Width="628px" Font-Names="Arial" Font-Size="Small" Height="41px">Wir verpflichten uns, bei Fund der ersten Ausfertigung des Fahrzeugscheines, diesen unaufgefordert bei der Zulassungsstelle abzugeben.</asp:Label></TD>
							</TR>
							<TR vAlign="top">
								<TD height="23"></TD>
								<TD colSpan="4">
									<asp:label id="Label8" runat="server" Font-Bold="True">.......................................................</asp:label></TD>
								<TD rowSpan="2"></TD>
							</TR>
							<TR vAlign="top">
								<TD colSpan="2" height="19"></TD>
								<TD colSpan="3">
									<asp:Label id="Label3" runat="server" Font-Names="Arial" Font-Size="Small">Stempel und Unterschrift</asp:Label></TD>
							</TR>
						</TABLE>
					</form>
				</TD>
			</TR>
		</TABLE>
	</body>
</HTML>
