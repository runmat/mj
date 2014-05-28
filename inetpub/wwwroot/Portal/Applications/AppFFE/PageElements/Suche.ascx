<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Suche.ascx.vb" Inherits="AppFFE.Suche" %>
<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
	<tr>
		<td class="PageNavigation" colSpan="2"><asp:label id="lblHeadline" runat="server"></asp:label>&nbsp;
			<asp:label id="lblTask" runat="server"></asp:label>
            <asp:imagebutton id="ImageButton1" runat="server" 
                ImageUrl="../../../Images/empty.gif"></asp:imagebutton></td>
	</tr>
	<tr>
		<td vAlign="top" width="120">
			<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
				<TR>
					<TD class="TaskTitle" width="150">&nbsp;</TD>
				</TR>
				<TR>
					<TD vAlign="center" width="150"><asp:linkbutton id="cmdSelect" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Auswählen</asp:linkbutton></TD>
				</TR>
				<TR>
					<TD vAlign="center" width="150"><asp:linkbutton id="cmdSearch" runat="server" CssClass="StandardButton"> &#149;&nbsp;Suchen</asp:linkbutton></TD>
				</TR>
				<TR>
					<TD vAlign="center" width="150"><asp:linkbutton id="cmdReset" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Zurücksetzen</asp:linkbutton></TD>
				</TR>
			</TABLE>
		</td>
		<td vAlign="top">
			<TABLE id="Table4" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="TaskTitle">&nbsp;</TD>
				</TR>
			</TABLE>
			<TABLE id="Table3" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
				<TR id="tr_Haendlernummer" runat="server">
					<TD class="TextLarge" width="98" height="32">
						<asp:label id="lbl_HaendlerNummer" runat="server">lbl_Haendlernr</asp:label>&nbsp;</TD>
					<TD class="TextLarge" width="100%" height="32"><asp:textbox id="txtNummer" runat="server" MaxLength="10" Width="250px"></asp:textbox></TD>
				</TR>
				<TR id="tr_Name" runat="server">
					<TD class="StandardTableAlternate" width="98">
                        <asp:Label ID="lbl_Name" runat="server" Text="lbl_Name"></asp:Label>
                    </TD>
					<TD class="StandardTableAlternate"><asp:textbox id="txtName" runat="server" MaxLength="35" Width="250px"></asp:textbox></TD>
				</TR>
				<TR id="tr_Ort" runat="server">
					<TD class="TextLarge" width="98">
                        <asp:Label ID="lbl_Ort" runat="server" Text="lbl_Ort"></asp:Label>
                    </TD>
					<TD class="TextLarge"><asp:textbox id="txtCity" runat="server" MaxLength="35" Width="250px"></asp:textbox></TD>
				</TR>
				<TR id="tr_HdAuswahl" runat="server" Visible="False">
					<TD class="StandardTableAlternate" width="98"><asp:label id="lbl_Auswahl" 
                            runat="server" Visible="False">lbl_Auswahl</asp:label>&nbsp;&nbsp;&nbsp;</TD>
					<TD class="StandardTableAlternate"><asp:dropdownlist id="cmbHaendler" runat="server" Visible="False"></asp:dropdownlist>&nbsp;&nbsp;&nbsp;</TD>
				</TR>
				<TR id="tr_SubsidiaryRow" runat="server" Visible="False">
					<TD class="TextLarge" width="98"><asp:label id="lbl_FilialeShow" runat="server" 
                            Visible="False">lbl_Filiale</asp:label></TD>
					<TD class="TextLarge"><asp:dropdownlist id="cmbFilialen" runat="server"></asp:dropdownlist><asp:label id="lblFiliale" runat="server" Visible="False"></asp:label></TD>
				</TR>
				<TR id="tr_DistrictRow" runat="server">
					<TD class="TextLarge" width="98">
                        <asp:label id="lbl_Distrikt" 
                            runat="server" Visible="False">lbl_Distrikt</asp:label></TD>
					<TD class="TextLarge"><asp:dropdownlist id="DistrictDropDown" runat="server"></asp:dropdownlist><asp:label id="DistrictLabel" runat="server" Visible="False"></asp:label></TD>
				</TR>
				<TR id="tr_Report" runat="server">
					<TD class="TextLarge" width="98" height="25">
                        <asp:Label ID="lbl_Report" runat="server" Text="lbl_Report"></asp:Label>
                    </TD>
					<TD class="TextLarge" height="25"><asp:textbox id="txtDatumAb" runat="server" MaxLength="35" Width="250px">TTMMJJJJ</asp:textbox></TD>
				</TR>
				<TR>
					<TD class="TextLarge" width="98">&nbsp;</TD>
					<TD class="TextLarge"><asp:label id="lblMessage" runat="server"></asp:label>&nbsp;&nbsp;</TD>
				</TR>
			</TABLE>
		</td>
	</tr>
</TABLE>
