<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ProgressControl.ascx.vb" Inherits="AppUeberf.Controls.ProgressControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="300" border="0" style="BACKGROUND-COLOR: lemonchiffon">
	<TR>
		<TD></TD>
		<TD style="WIDTH: 257px"></TD>
		<TD colSpan="2">
			<asp:Label id="lblText" Font-Bold="True" runat="server">Schritt x von y</asp:Label>
		</TD>
	</TR>
	<TR>
		<TD>
			<asp:label id="lblRefLabel" Font-Bold="True" runat="server" Font-Italic="True" Width="96px">Referenz:</asp:label></TD>
		<TD style="WIDTH: 257px">
			<asp:label id="lblReferenz" Font-Bold="True" runat="server" Font-Italic="True" Width="225px"></asp:label></TD>
		<TD>
			<asp:label id="lblNameLabel" Font-Bold="True" runat="server" Font-Italic="True" Width="96px">Leasingnehmer:</asp:label></TD>
		<TD>
			<asp:label id="lblLeasingnehmerName" Font-Bold="True" runat="server" Font-Italic="True" Width="242px"></asp:label></TD>
	</TR>
	<TR>
		<TD>
			<asp:label id="lblTypLabel" Font-Bold="True" runat="server" Font-Italic="True" Width="96px">Fahrzeugtyp:</asp:label></TD>
		<TD style="WIDTH: 257px">
			<asp:label id="lblFahrzeugtyp" Font-Bold="True" runat="server" Font-Italic="True" Width="266px"></asp:label></TD>
		<TD>
			<asp:label id="lblOrtLabel" Font-Bold="True" runat="server" Font-Italic="True" Width="96px">Ort:</asp:label></TD>
		<TD>
			<asp:label id="lblLeasingnehmerOrt" Font-Bold="True" runat="server" Font-Italic="True" Width="245px"></asp:label></TD>
	</TR>
</TABLE>
