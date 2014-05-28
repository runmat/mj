<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report41_02Print.aspx.vb" Inherits="AppFFE.Report41_02Print" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Händlerübersicht Kontingente/Fälligkeiten - Druckansicht</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
	</HEAD>
	<body bgColor="white" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" height="531" cellSpacing="1" cellPadding="1" width="616" border="0">
				<TR>
					<TD noWrap colSpan="2"><FONT face="Arial"></FONT>
						<P><FONT face="Arial"><STRONG><FONT size="4"><U>Händlerübersicht Kontingente/Fälligkeit</U></FONT></STRONG></FONT></P>
					</TD>
				</TR>
				<TR>
					<TD align="left" colSpan="2"><FONT face="Arial"></FONT><FONT face="Arial"><FONT size="2">Benutzer:</FONT>&nbsp;
							<asp:textbox id="txtUser" runat="server" BorderColor="Transparent" ReadOnly="True" BorderWidth="0px" BackColor="Transparent"></asp:textbox></FONT></TD>
				</TR>
				<TR>
					<TD vAlign="top" align="left" colSpan="2">
						<TABLE id="Table2" height="85" cellSpacing="1" cellPadding="1" width="614" border="0">
							<TR>
								<TD width="22" height="24"><FONT face="Arial" size="2"><STRONG>Händlernr:</STRONG></FONT></TD>
								<TD width="269" height="24"><asp:textbox id="txtNr" runat="server" BorderColor="Transparent" ReadOnly="True" BorderWidth="0px" BackColor="Transparent" Width="172px"></asp:textbox></TD>
								<TD width="897" height="24"><FONT face="Arial" size="2"><STRONG>Lastschrift:</STRONG></FONT></TD>
								<TD width="536" height="24">&nbsp;
									<asp:checkbox id="chkLastschrift" runat="server" Enabled="False"></asp:checkbox>&nbsp;
									<FONT face="Arial" size="2"><STRONG>seit:<asp:textbox id="datseit" runat="server" BorderColor="Transparent" ReadOnly="True" BorderWidth="0px" BackColor="Transparent" Width="68px"></asp:textbox></STRONG></FONT></TD>
							</TR>
							<TR>
								<TD width="22"><FONT face="Arial" size="2"><STRONG>Name:</STRONG></FONT></TD>
								<TD width="269"><asp:textbox id="txtName" runat="server" BorderColor="Transparent" ReadOnly="True" BorderWidth="0px" BackColor="Transparent" Width="271px" Height="19px"></asp:textbox></TD>
								<TD width="897"><FONT face="Arial" size="2"><STRONG>letzte Änderung:</STRONG></FONT></TD>
								<TD width="232"><asp:textbox id="datLastChange" runat="server" BorderColor="Transparent" ReadOnly="True" BorderWidth="0px" BackColor="Transparent" Width="109px"></asp:textbox></TD>
							</TR>
							<TR>
								<TD width="22"><FONT face="Arial" size="2"><STRONG>Adresse:</STRONG></FONT></TD>
								<TD width="269"><asp:textbox id="txtAdresse" runat="server" BorderColor="Transparent" ReadOnly="True" BorderWidth="0px" BackColor="Transparent" Width="287px" Height="19px"></asp:textbox></TD>
								<TD width="897"><FONT face="Arial" size="2"><STRONG>gehört zu:</STRONG></FONT></TD>
								<TD width="232"><asp:textbox id="txtDistrikt" runat="server" BorderColor="Transparent" ReadOnly="True" BorderWidth="0px" BackColor="Transparent" Width="112px"></asp:textbox></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD vAlign="top" align="left" colSpan="2">
						<TABLE id="Table3" height="56" cellSpacing="0" cellPadding="0" width="612" border="1">
							<TR>
								<TD width="260" bgColor="#cccccc"><FONT face="Arial" size="1"><STRONG>Kontingentart</STRONG></FONT></TD>
								<TD width="98" bgColor="#cccccc"><FONT face="Arial" size="1"><STRONG>Kontingent</STRONG></FONT></TD>
								<TD width="98" bgColor="#cccccc"><FONT face="Arial" size="1"><STRONG>Inanspruchnahme</STRONG></FONT></TD>
								<TD noWrap bgColor="#cccccc"><FONT face="Arial" size="1"><STRONG>Freies Kontingent</STRONG></FONT></TD>
								<TD noWrap bgColor="#cccccc"><FONT face="Arial" size="1">Gesperrt</FONT></TD>
							</TR>
							<TR>
								<TD width="260"><FONT face="Arial" size="2"><asp:textbox id="txtA001" runat="server" BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" Width="100%" Font-Size="XX-Small"></asp:textbox></FONT></TD>
								<TD width="98"><asp:textbox id="txtK001" runat="server" BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:textbox></TD>
								<TD width="98"><asp:textbox id="txtI001" runat="server" BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:textbox></TD>
								<TD><asp:textbox id="txtF001" runat="server" BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:textbox></TD>
								<TD><asp:textbox id="Textbox1" runat="server" BorderColor="Transparent" BorderWidth="0px" Width="50px" Font-Size="XX-Small"></asp:textbox></TD>
							</TR>
							<TR>
								<TD width="260" height="20"><FONT face="Arial" size="2"><asp:textbox id="txtA002" runat="server" BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" Width="100%" Font-Size="XX-Small"></asp:textbox></FONT></TD>
								<TD width="98" height="20"><asp:textbox id="txtK002" runat="server" BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:textbox></TD>
								<TD width="98" height="20"><asp:textbox id="txtI002" runat="server" BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:textbox></TD>
								<TD height="20"><asp:textbox id="txtF002" runat="server" BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:textbox></TD>
								<TD height="20"><asp:textbox id="Textbox2" runat="server" BorderColor="Transparent" BorderWidth="0px" Width="50px" Font-Size="XX-Small"></asp:textbox></TD>
							</TR>
						</TABLE>
						<TABLE id="Table4" height="58" cellSpacing="0" cellPadding="0" width="612" border="1">
							<TR>
								<TD width="445" bgColor="#cccccc"><FONT face="Arial" size="1"><STRONG>Kontingentart</STRONG></FONT></TD>
								<TD width="148" bgColor="#cccccc"><FONT face="Arial" size="1"><STRONG>Richtwert</STRONG></FONT></TD>
								<TD width="393" bgColor="#cccccc"><FONT face="Arial" size="1"><STRONG>Inanspruchnahme</STRONG></FONT></TD>
							</TR>
							<TR>
								<TD width="445" height="21"><FONT face="Arial" size="2"><asp:textbox id="A003" runat="server" BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" Width="100%" Font-Size="XX-Small"></asp:textbox></FONT></TD>
								<TD width="148" height="21"><asp:textbox id="R003" runat="server" BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:textbox></TD>
								<TD width="393" height="21"><asp:textbox id="I003" runat="server" BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:textbox></TD>
							</TR>
							<TR>
								<TD width="445" height="20"><FONT face="Arial" size="2"><asp:textbox id="A004" runat="server" BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" Width="100%" Font-Size="XX-Small"></asp:textbox></FONT></TD>
								<TD width="148" height="20"><asp:textbox id="R004" runat="server" BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:textbox></TD>
								<TD width="393" height="20"><asp:textbox id="I004" runat="server" BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:textbox></TD>
							</TR>
							<TR>
								<TD width="445" height="20"><FONT face="Arial" size="2"><asp:textbox id="A006" 
                                        runat="server" BorderColor="Transparent" BorderWidth="0px" 
                                        BackColor="Transparent" Width="100%" Font-Size="XX-Small"></asp:textbox></FONT></TD>
								<TD width="148" height="20"><asp:textbox id="R006" runat="server" 
                                        BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" 
                                        Width="30px" Font-Size="XX-Small"></asp:textbox></TD>
								<TD width="393" height="20"><asp:textbox id="I006" runat="server" 
                                        BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" 
                                        Width="30px" Font-Size="XX-Small"></asp:textbox></TD>
							</TR>
						</TABLE>
						<TABLE id="Table5" height="36" cellSpacing="0" cellPadding="0" width="612" border="1">
							<TR>
								<TD width="260" bgColor="#cccccc" height="13"><FONT face="Arial" size="1"><STRONG>Gesamt</STRONG></FONT></TD>
								<TD width="92" bgColor="#cccccc" height="13"><FONT face="Arial" size="1"><STRONG>Kontingente</STRONG></FONT></TD>
								<TD bgColor="#cccccc" height="13"><FONT face="Arial" size="1"><STRONG>Inanspruchnahme</STRONG></FONT></TD>
							</TR>
							<TR>
								<TD width="260"><FONT face="Arial" size="2"><asp:textbox id="Textbox10" runat="server" BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" Width="100%" Font-Size="XX-Small"></asp:textbox></FONT></TD>
								<TD width="99" height="20"><asp:textbox id="Textbox14" runat="server" BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:textbox></TD>
								<TD height="20"><asp:textbox id="Textbox15" runat="server" BorderColor="Transparent" BorderWidth="0px" BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:textbox></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD colSpan="3"><asp:datagrid id="DataGrid1" runat="server" Width="611px" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="250" AutoGenerateColumns="False">
							<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
							<ItemStyle Font-Size="XX-Small" Font-Names="Arial"></ItemStyle>
							<HeaderStyle Font-Size="XX-Small" Font-Names="Arial" Font-Bold="True" Wrap="False" CssClass="GridTableHead" BackColor="Silver"></HeaderStyle>
							<Columns>
								<asp:BoundColumn Visible="False" DataField="KontingentID" HeaderText="KontingentID"></asp:BoundColumn>
								<asp:BoundColumn DataField="Kontingentart" HeaderText="Kontingentart">
									<HeaderStyle Width="285"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Alte Zahlungsfrist" ReadOnly="True" HeaderText="F&#228;lligkeit ">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn>
									<HeaderStyle Width="164px"></HeaderStyle>
								</asp:TemplateColumn>
								<asp:TemplateColumn>
									<HeaderStyle Width="164px"></HeaderStyle>
								</asp:TemplateColumn>
								<asp:TemplateColumn Visible="False" HeaderText="Neue F&#228;lligkeit in Tagen"></asp:TemplateColumn>
								<asp:BoundColumn Visible="False" HeaderText="ROW"></asp:BoundColumn>
							</Columns>
							<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Wrap="False"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD colSpan="2">
						<P><FONT face="Arial" size="2"><br>
								<br>
								<br>
								<STRONG>Tagesdatum, Unterschrift</STRONG>:</FONT>________________________________________</P>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
