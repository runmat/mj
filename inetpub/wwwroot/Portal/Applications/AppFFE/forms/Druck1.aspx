<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Druck1.aspx.vb" Inherits="AppFFE.Druck1" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Zu autorisierende Vorgänge - Druckansicht</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
	    <style type="text/css">
            .style1
            {
                width: 262px;
            }
            .style2
            {
                width: 59px;
            }
            .style3
            {
                width: 646px;
            }
        </style>
	</HEAD>
	<body bgColor="white" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="550" border="0">
				<TR>
					<TD noWrap colSpan="2" class="style3"><FONT face="Arial"></FONT>
						<P><FONT face="Arial"><STRONG><FONT size="4"><U>Freizugebende&nbsp;Vorgänge</U></FONT></STRONG></FONT></P>
					</TD>
				</TR>
				<TR>
					<TD align="left" colSpan="2" class="style3"><FONT face="Arial"></FONT><FONT face="Arial"><FONT size="2">Benutzer:</FONT>&nbsp;
							<asp:textbox id="txtUser" runat="server" BorderColor="Transparent" ReadOnly="True" BorderWidth="0px" BackColor="Transparent"></asp:textbox></FONT></TD>
				</TR>
				<TR>
					<TD vAlign="top" align="left" colSpan="2" class="style3">
						<TABLE id="Table2" cellSpacing="1" cellPadding="1" width="100%" border="0">
							<TR>
								<TD width="22"><FONT face="Arial" size="2"><STRONG>Händlernr:</STRONG></FONT></TD>
								<TD width="100%"><asp:textbox id="txtNr" runat="server" BorderColor="Transparent" ReadOnly="True" BorderWidth="0px" BackColor="Transparent" Width="100%"></asp:textbox></TD>
							</TR>
							<TR>
								<TD width="22"><FONT face="Arial" size="2"><STRONG>Name:</STRONG></FONT></TD>
								<TD><asp:textbox id="txtName" runat="server" BorderColor="Transparent" ReadOnly="True" BorderWidth="0px" BackColor="Transparent" Width="100%"></asp:textbox></TD>
							</TR>
							<TR>
								<TD width="22"><FONT face="Arial" size="2"><STRONG>Adresse:</STRONG></FONT></TD>
								<TD><asp:textbox id="txtAdresse" runat="server" BorderColor="Transparent" ReadOnly="True" BorderWidth="0px" BackColor="Transparent" Width="100%"></asp:textbox></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD vAlign="top" align="left" colSpan="2" class="style3">
						<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="1">
							<TR>
								<TD class="style1"><FONT face="Arial" size="1"><STRONG>Kontingentart</STRONG></FONT></TD>
								<TD class="style2"><FONT face="Arial" size="1"><STRONG>Kontingent</STRONG></FONT></TD>
								<TD><FONT face="Arial" size="1"><STRONG>Inanspruchnahme</STRONG></FONT></TD>
								<TD noWrap><FONT face="Arial" size="1"><STRONG>Freies Kontingent</STRONG></FONT></TD>
								<TD noWrap><FONT face="Arial" size="1">ZE Eingang</FONT></TD>
								<TD noWrap><FONT face="Arial" size="1">Neues freies<br>
										Kontingent</FONT></TD>
							</TR>
							<TR>
								<TD noWrap class="style1"><FONT face="Arial" size="2"><asp:textbox id="txtA001" runat="server" BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" Width="100%" Font-Size="XX-Small"></asp:textbox></FONT></TD>
								<TD class="style2"><asp:textbox id="txtK001" runat="server" 
                                        BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" 
                                        Width="30px" Font-Size="XX-Small" style="margin-left: 0px"></asp:textbox></TD>
								<TD><asp:textbox id="txtI001" runat="server" BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:textbox></TD>
								<TD><asp:textbox id="txtF001" runat="server" BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:textbox></TD>
								<TD><asp:textbox id="Textbox1" runat="server" BorderColor="Transparent" BorderWidth="0px" Width="50px" Font-Size="XX-Small"></asp:textbox></TD>
								<TD><asp:textbox id="Textbox5" runat="server" BorderColor="Transparent" BorderWidth="0px" Width="50px" Font-Size="XX-Small"></asp:textbox></TD>
							</TR>
							<TR>
								<TD height="20" class="style1"><FONT face="Arial" size="2"><asp:textbox id="txtA002" runat="server" BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" Width="100%" Font-Size="XX-Small"></asp:textbox></FONT></TD>
								<TD height="20" class="style2"><asp:textbox id="txtK002" runat="server" BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:textbox></TD>
								<TD height="20"><asp:textbox id="txtI002" runat="server" BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:textbox></TD>
								<TD height="20"><asp:textbox id="txtF002" runat="server" BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:textbox></TD>
								<TD height="20"><asp:textbox id="Textbox2" runat="server" BorderColor="Transparent" BorderWidth="0px" Width="50px" Font-Size="XX-Small"></asp:textbox></TD>
								<TD height="20"><asp:textbox id="Textbox6" runat="server" BorderColor="Transparent" BorderWidth="0px" Width="50px" Font-Size="XX-Small"></asp:textbox></TD>
							</TR>
							<TR id="trDP" runat="server">
								<TD class="style1"><FONT face="Arial" size="2"><asp:textbox id="txtA004" runat="server" BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" Width="100%" Font-Size="XX-Small"></asp:textbox></FONT></TD>
								<TD class="style2"><asp:textbox id="txtK004" runat="server" BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:textbox></TD>
								<TD><asp:textbox id="txtI004" runat="server" BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:textbox></TD>
								<TD><asp:textbox id="txtF004" runat="server" BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" Width="30px" Font-Size="XX-Small" Visible="False"></asp:textbox></TD>
								<TD><asp:textbox id="Textbox3" runat="server" BorderColor="Transparent" BorderWidth="0px" Width="50px" Font-Size="XX-Small"></asp:textbox></TD>
								<TD><asp:textbox id="Textbox7" runat="server" BorderColor="Transparent" BorderWidth="0px" Width="50px" Font-Size="XX-Small"></asp:textbox></TD>
							</TR>
							<TR id="trHEZ" runat="server">
								<TD noWrap class="style1"><FONT face="Arial" size="2"><asp:textbox id="txtA005" runat="server" BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" Width="100%" Font-Size="XX-Small"></asp:textbox></FONT></TD>
								<TD class="style2"><asp:textbox id="txtK005" runat="server" BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:textbox></TD>
								<TD><asp:textbox id="txtI005" runat="server" BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:textbox></TD>
								<TD><asp:textbox id="txtF005" runat="server" BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" Width="30px" Font-Size="XX-Small" Visible="False"></asp:textbox></TD>
								<TD><asp:textbox id="Textbox4" runat="server" BorderColor="Transparent" BorderWidth="0px" Width="50px" Font-Size="XX-Small"></asp:textbox></TD>
								<TD><asp:textbox id="Textbox8" runat="server" BorderColor="Transparent" BorderWidth="0px" Width="50px" Font-Size="XX-Small"></asp:textbox></TD>
							</TR>
						</TABLE>
						<P><FONT face="Arial"><U><STRONG>Vorgänge</STRONG></U></FONT></P>
					</TD>
				</TR>
				<TR>
					<TD colSpan="3" class="style3">
						<P><asp:datagrid id="DataGrid1" runat="server" Width="100%" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="250" AutoGenerateColumns="False">
								<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
								<HeaderStyle Font-Size="XX-Small" Font-Names="Arial" Font-Bold="True" Wrap="False" CssClass="GridTableHead"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="VBELN" SortExpression="VBELN" HeaderText="Auftrags-Nr.">
										<ItemStyle Font-Size="XX-Small" Font-Names="Arial"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ZZREFNR" SortExpression="ZZREFNR" HeaderText="Vertrags-Nr.">
										<ItemStyle Font-Size="XX-Small" Font-Names="Arial"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ZZANFDT" SortExpression="ZZANFDT HeaderText="Angefordert am:">
										<ItemStyle Font-Size="XX-Small" Font-Names="Arial"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="ZZFAHRG" SortExpression="ZZFAHRG" HeaderText="Fahrgestell-Nr."></asp:BoundColumn>
									<asp:BoundColumn DataField="ZZBRIEF" SortExpression="ZZBRIEF" HeaderText="ZBII-Nummer">
										<ItemStyle Font-Size="XX-Small" Font-Names="Arial"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="BSTZD" SortExpression="BSTZD" HeaderText="Kontingentart">
										<ItemStyle Font-Size="XX-Small" Font-Names="Arial"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ZZFINART" SortExpression="ZZFINART" HeaderText="FA">
										<ItemStyle Font-Size="XX-Small" Font-Names="Arial"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="InAutorisierung" SortExpression="InAutorisierung" HeaderText="InAutorisierung"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="Initiator" SortExpression="Initiator" HeaderText="Initiator"></asp:BoundColumn>
									<asp:TemplateColumn HeaderText="Freigabe">
										<ItemStyle Font-Size="XX-Small" Font-Names="Arial"></ItemStyle>
										<ItemTemplate>
											<asp:Label id=lblAut runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.InAutorisierung")=TRUE %>' Text='<%# DataBinder.Eval(Container, "DataItem.Initiator")%>'>
											</asp:Label>
										</ItemTemplate>
									</asp:TemplateColumn>
								</Columns>
								<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Wrap="False"></PagerStyle>
							</asp:datagrid></P>
					</TD>
				</TR>
				<TR>
					<TD colSpan="2" class="style3">
						<P><FONT face="Arial" size="2"><br>
								<br>
								<br>
								<STRONG>Tagesdatum, Unterschrift</STRONG>:</FONT>________________________________________</P>
					</TD>
				</TR>
			</TABLE>
			<P>&nbsp;</P>
			<P></P>
		</form>
	</body>
</HTML>
