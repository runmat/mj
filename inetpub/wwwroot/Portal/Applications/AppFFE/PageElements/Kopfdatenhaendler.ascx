<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Kopfdatenhaendler.ascx.vb" Inherits="AppFFE.Kopfdatenhaendler" %>
<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" align="center" border="0" bgColor="white" class="TableKontingent">
	<TR>
		<TD vAlign="top" class="">
			<TABLE id="Table1" cellSpacing="0" cellPadding="3" width="100%" align="center" border="0">
				<TR>
					<TD class="TextLarge" vAlign="top">
						<asp:label id="lbl_HaendlerNummer" runat="server">Händlernummer:</asp:label></TD>
					<TD class="TextLarge"><asp:label id="lblHaendlerNummer" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="TextLarge" vAlign="top">Name:&nbsp;&nbsp;
					</TD>
					<TD class="TextLarge" width="100%"><asp:label id="lblHaendlerName" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="TextLarge" vAlign="top">Adresse:</TD>
					<TD class="TextLarge"><asp:label id="lblAdresse" runat="server"></asp:label></TD>
				</TR>
			</TABLE>
		</TD>
		<TD></TD>
		<TD vAlign="top"></TD>
	</TR>
</TABLE>
<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
	<TR>
		<TD class="TextLarge" vAlign="top"><asp:label id="lblMessage" runat="server"></asp:label>
		</TD>
	</TR>
</TABLE>