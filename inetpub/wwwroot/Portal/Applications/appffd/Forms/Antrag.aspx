<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Antrag.aspx.vb" Inherits="AppFFD.Antrag" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Antrag</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
	</HEAD>
	<body bgColor="white" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="550" border="0">
				<TR>
					<TD width="64"><FONT face="Arial"><IMG height="52" alt="" src="/Portal/Customize/FFD/FordBank.jpg" width="94"></FONT></TD>
					<TD vAlign="bottom" noWrap width="100%">
						<P><FONT face="Arial"><FONT size="4"><STRONG>Darlehensantrag für</STRONG></FONT><BR>
								Ford Bank Niederlassung der FCE Bank plc (Bank)</FONT></P>
					</TD>
				</TR>
				<TR>
					<TD align="right" width="64"><FONT face="Arial"></FONT></TD>
					<TD noWrap>
						<TABLE id="Table2" cellSpacing="1" cellPadding="1" border="0" runat="server">
							<TR>
								<TD><FONT face="Arial">für:&nbsp;&nbsp;</FONT></TD>
								<TD><asp:checkbox id="CheckBox4" runat="server" Font-Bold="True" Text="Vorführfahrzeuge" Font-Names="Arial"></asp:checkbox><asp:checkbox id="CheckBox5" runat="server" Font-Bold="True" Text="Selbstfahrervermietfahrzeuge" Font-Names="Arial"></asp:checkbox><FONT face="Arial"></FONT></TD>
							</TR>
							<TR>
								<TD><FONT face="Arial"></FONT></TD>
								<TD><asp:checkbox id="CheckBox6" runat="server" Font-Bold="True" Text="händlereigene Aktion-/Tageszulassungen" Font-Names="Arial"></asp:checkbox><FONT face="Arial"></FONT></TD>
							</TR>
						</TABLE>
						<FONT face="Arial"></FONT>
					</TD>
				</TR>
				<TR>
					<TD colSpan="3">
						<HR width="100%" SIZE="1">
					</TD>
				</TR>
				<TR>
					<TD colSpan="3">
						<TABLE id="Table4" height="75" cellSpacing="1" cellPadding="1" width="100%" border="0">
							<TR>
								<TD vAlign="top"><FONT face="Arial"><FONT size="2">Wir beantragen die Gewährung eines 
											Darlehens gemäß den Bedingungen des mit der Bank vereinbarten Rahmenvertrages 
											für die Finanzierung von neuen Fahrzeugen, gebrauchten Fahrzeugen, 
											Vorführfahrzeugen und Selbstfahrervermietfahrzeugen für die nachstehend 
											aufgeführten Fahrzeuge. Im Übrigen gelten die Vereinbarungen gemäß Formular 
											306/2 "Eröffnung von Konten für Händler</FONT>".</FONT></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD colSpan="2"><asp:datagrid id="DataGrid1" runat="server" AutoGenerateColumns="False" bodyHeight="250" cssclass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" Width="100%">
							<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
							<HeaderStyle Font-Size="XX-Small" Font-Names="Arial" Font-Bold="True" Wrap="False" CssClass="GridTableHead"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="ZZFAHRG" HeaderText="Ident-Nr.">
									<ItemStyle Font-Size="XX-Small" Font-Names="Arial"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="LIZNR" SortExpression="LIZNR" HeaderText="EF-Nr.">
									<ItemStyle Font-Size="XX-Small" Font-Names="Arial"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Modell">
									<ItemTemplate>
										<asp:TextBox id="TextBox5" runat="server" Font-Names="Arial" Width="50px" Font-Size="XX-Small" BorderWidth="0px" MaxLength="10"></asp:TextBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="TIDNR" HeaderText="Brief-Nr.">
									<ItemStyle Font-Size="XX-Small" Font-Names="Arial"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="KW/TSN">
									<ItemTemplate>
										<asp:TextBox id="TextBox1" runat="server" Font-Names="Arial" Width="50px" Font-Size="XX-Small" BorderWidth="0px" MaxLength="15"></asp:TextBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="LICENSE_NUM" SortExpression="LICENSE_NUM" HeaderText="KFZ-Kennzeichen">
									<ItemStyle Font-Size="XX-Small" Font-Names="Arial"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Finanz.&lt;br&gt;Summe">
									<ItemTemplate>
										<asp:TextBox id="TextBox2" runat="server" Font-Names="Arial" Width="50px" Font-Size="XX-Small" BorderWidth="0px" MaxLength="10"></asp:TextBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn Visible="False" HeaderText="Herst.&lt;br&gt;Rechng.Nr">
									<ItemTemplate>
										<asp:TextBox id="TextBox3" runat="server" Font-Names="Arial" Width="50px" Font-Size="XX-Small" BorderWidth="0px" MaxLength="15"></asp:TextBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="MANDT" SortExpression="MANDT" HeaderText="MANDT"></asp:BoundColumn>
								<asp:BoundColumn DataField="ZZFINART" SortExpression="ZZFINART" HeaderText="FA">
									<ItemStyle Font-Size="XX-Small" Font-Names="Arial"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="S">
									<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:Label id=Label1 runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="XX-Small" Visible='<%# Ctype(DataBinder.Eval(Container, "DataItem.MANDT"),String)="2" %>'>x</asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="V">
									<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:Label id=Label2 runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="XX-Small" Visible='<%# Ctype(DataBinder.Eval(Container, "DataItem.MANDT"),String)="4" %>'>x</asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="HEZ">
									<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:Label id=Label3 runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="XX-Small" Visible='<%# Ctype(DataBinder.Eval(Container, "DataItem.MANDT"),String)="3" %>'>x</asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn Visible="False" HeaderText="Bezahlt">
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:CheckBox id=Bezahlt runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.ZZBEZAHLT") %>'>
										</asp:CheckBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn Visible="False" HeaderText="COC Besch.&lt;br&gt;vorhanden">
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:CheckBox id=ZZCOCKZ runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.ZZCOCKZ") %>'>
										</asp:CheckBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn Visible="False" HeaderText="Kontingentart"></asp:TemplateColumn>
							</Columns>
							<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Wrap="False"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD colSpan="2"><FONT face="Arial" size="2">Das Fahrzeug/die Fahrzeuge befindet/n sich 
							in unserem unmittelbaren Besitz und ist bei Ihnen als Lagerfahrzeug unter der 
							o.g. EF-Nr. einfinanziert.</FONT></TD>
				</TR>
				<TR>
					<TD vAlign="top" colSpan="2"><FONT face="Arial"><STRONG></STRONG></FONT><FONT face="Arial">
							<HR SIZE="1">
							<FONT size="2"><STRONG>Versicherung</STRONG></FONT></FONT></TD>
				</TR>
				<TR>
					<TD align="right"><FONT face="Arial"></FONT></TD>
					<TD>
						<P><FONT face="Arial" size="2"><asp:checkbox id="cbx3" runat="server" Text="Das Fahrzeug soll in folgendem Umfang über die Ford Bank versichert werden:" Font-Names="Arial" BackColor="Transparent" Font-Size="X-Small"></asp:checkbox></FONT></P>
					</TD>
				</TR>
				<TR>
					<TD height="68"><FONT face="Arial" size="2"></FONT></TD>
					<TD height="68"><asp:checkbox id="CheckBox1" runat="server" Text="Haftpflichtversicherung" Font-Names="Arial" Font-Size="X-Small" Font-Underline="True" Checked="True" Enabled="False"></asp:checkbox>
						<TABLE id="Table3" cellSpacing="1" cellPadding="1" width="550" border="0">
							<TR>
								<TD vAlign="top"><FONT face="Arial" size="1">Vorführfahrzeuge:<br>
										Selbstfahrervermietfahrzeuge:<br>
										Insassenunfallversicherung: Veruntreuungsversicherung </FONT>
								</TD>
								<TD vAlign="top" width="100%"><FONT face="Arial" size="1">50 Mio. EUR pauschal (pro 
										Person max. 8 Mio. EUR)<br>
										50 Mio. EUR pauschal (pro Person max. 7,5 Mio. EUR)<br>
										EUR 25.000,-- Tod / EUR 50.000,-- Invalidität<br>
									</FONT>
								</TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD vAlign="bottom" align="right" height="151"><FONT face="Arial" size="2"></FONT></TD>
					<TD height="151"><FONT face="Arial" size="2"><U>Kaskoversicherung</U></FONT><br>
						<FONT size="2"><FONT face="Arial"><FONT size="1">-mindestens Teilkasko EUR 150,- SB 
									(Code1)-</FONT> </FONT>
							<TABLE id="Table5" cellSpacing="1" cellPadding="1" width="550" border="0">
								<TR>
									<TD noWrap align="left" width="28"><FONT face="Arial"><FONT face="Arial" size="1"><U>Code</U></FONT></FONT></TD>
									<TD colSpan="2"><FONT face="Arial" size="2"><U></U></FONT></TD>
									<TD align="right"><FONT face="Arial" size="1"><U>EUR</U></FONT></TD>
									<TD><FONT face="Arial" size="2"></FONT></TD>
									<TD><FONT face="Arial" size="2"></FONT></TD>
									<TD align="right"><FONT face="Arial" size="1"><U>EUR</U></FONT></TD>
									<TD width="100%"><FONT face="Arial"></FONT></TD>
								</TR>
								<TR>
									<TD align="left" width="28"><asp:radiobutton id="RadioButton6" runat="server" Font-Names="Arial" Font-Size="XX-Small" GroupName="grpKasko" Checked="True"></asp:radiobutton></TD>
									<TD><FONT face="Arial" size="1">=</FONT></TD>
									<TD noWrap width="128"><FONT face="Arial" size="1">keine</FONT></TD>
									<TD align="right"></TD>
									<TD></TD>
									<TD></TD>
									<TD></TD>
									<TD></TD>
								</TR>
								<TR>
									<TD align="left" width="28"><FONT face="Arial" size="1"><asp:radiobutton id="RadioButton0" runat="server" Font-Names="Arial" Font-Size="XX-Small" GroupName="grpKasko"></asp:radiobutton></FONT></TD>
									<TD><FONT face="Arial" size="1">=</FONT></TD>
									<TD noWrap width="128"><FONT face="Arial" size="1">Teilkasko mit</FONT></TD>
									<TD align="right"><FONT face="Arial" size="1">150</FONT></TD>
									<TD><FONT face="Arial" size="1">SB</FONT></TD>
									<TD><FONT face="Arial" size="1"></FONT></TD>
									<TD><FONT face="Arial" size="1"></FONT></TD>
									<TD><FONT face="Arial" size="1"></FONT></TD>
								</TR>
								<TR>
									<TD align="left" width="28"><FONT face="Arial" size="1"><asp:radiobutton id="RadioButton1" runat="server" Font-Names="Arial" Font-Size="XX-Small" GroupName="grpKasko"></asp:radiobutton></FONT></TD>
									<TD><FONT face="Arial" size="1">=</FONT></TD>
									<TD width="128"><FONT face="Arial" size="1">Vollkasko mit</FONT></TD>
									<TD align="right"><FONT face="Arial" size="1">150</FONT></TD>
									<TD><FONT face="Arial" size="1">SB</FONT></TD>
									<TD noWrap><FONT face="Arial" size="1">inkl. Teilkasko mit</FONT></TD>
									<TD align="right"><FONT face="Arial" size="1">150</FONT></TD>
									<TD><FONT face="Arial" size="1">SB</FONT></TD>
								</TR>
								<TR>
									<TD align="left" width="28"><FONT face="Arial" size="1"><asp:radiobutton id="RadioButton2" runat="server" Font-Names="Arial" Font-Size="XX-Small" GroupName="grpKasko"></asp:radiobutton></FONT></TD>
									<TD><FONT face="Arial" size="1">=</FONT></TD>
									<TD width="128"><FONT face="Arial" size="1">Vollkasko mit</FONT></TD>
									<TD align="right"><FONT face="Arial" size="1">300</FONT></TD>
									<TD><FONT face="Arial" size="1">SB</FONT></TD>
									<TD><FONT face="Arial" size="1">inkl. Teilkasko mit</FONT></TD>
									<TD align="right"><FONT face="Arial" size="1">150</FONT></TD>
									<TD><FONT face="Arial" size="1">SB</FONT></TD>
								</TR>
								<TR>
									<TD align="left" width="31"><FONT face="Arial" size="1"><asp:radiobutton id="RadioButton3" runat="server" Font-Names="Arial" Font-Size="XX-Small" GroupName="grpKasko"></asp:radiobutton></FONT></TD>
									<TD><FONT face="Arial" size="1">=</FONT></TD>
									<TD width="128"><FONT face="Arial" size="1">Vollkasko mit</FONT></TD>
									<TD align="right"><FONT face="Arial" size="1">500</FONT></TD>
									<TD><FONT face="Arial" size="1">SB</FONT></TD>
									<TD><FONT face="Arial" size="1">inkl. Teilkasko mit</FONT></TD>
									<TD align="right"><FONT face="Arial" size="1">150</FONT></TD>
									<TD><FONT face="Arial" size="1">SB</FONT></TD>
								</TR>
								<TR>
									<TD align="left" width="31"><FONT face="Arial" size="1"><asp:radiobutton id="RadioButton4" runat="server" Font-Names="Arial" Font-Size="XX-Small" GroupName="grpKasko"></asp:radiobutton></FONT></TD>
									<TD><FONT face="Arial" size="1">=</FONT></TD>
									<TD width="128"><FONT face="Arial" size="1">Vollkasko mit</FONT></TD>
									<TD align="right"><FONT face="Arial" size="1">1.000</FONT></TD>
									<TD><FONT face="Arial" size="1">SB</FONT></TD>
									<TD><FONT face="Arial" size="1">inkl. Teilkasko mit</FONT></TD>
									<TD align="right"><FONT face="Arial" size="1">150</FONT></TD>
									<TD><FONT face="Arial" size="1">SB</FONT></TD>
								</TR>
								<TR>
									<TD align="left" width="31"><FONT face="Arial" size="1"><asp:radiobutton id="RadioButton5" runat="server" Font-Names="Arial" Font-Size="XX-Small" GroupName="grpKasko"></asp:radiobutton></FONT></TD>
									<TD><FONT face="Arial" size="1">=</FONT></TD>
									<TD width="128"><FONT face="Arial" size="1">Vollkasko mit</FONT></TD>
									<TD align="right"><FONT face="Arial" size="1">1.000</FONT></TD>
									<TD><FONT face="Arial" size="1">SB</FONT></TD>
									<TD><FONT face="Arial" size="1">inkl. Teilkasko mit</FONT></TD>
									<TD align="right"><FONT face="Arial" size="1">1.000</FONT></TD>
									<TD><FONT face="Arial" size="1">SB</FONT></TD>
								</TR>
							</TABLE>
						</FONT>
					</TD>
				</TR>
				<TR>
					<TD vAlign="bottom" align="left" colSpan="2">
						<TABLE id="Table6" cellSpacing="1" cellPadding="1" width="100%" border="0">
							<TR>
								<TD noWrap><asp:radiobutton id="RadioButton7" runat="server" Text="Das Fahrzeug ist im Rahmen von Delta Plus versichert." Font-Names="Arial" Font-Size="X-Small" GroupName="grF" Checked="True"></asp:radiobutton><FONT face="Arial"></FONT></TD>
								<TD><FONT face="Arial"></FONT></TD>
							</TR>
							<TR>
								<TD><FONT face="Arial"><asp:radiobutton id="RadioButton8" runat="server" Text="Das Fahrzeug ist versichert bei:" Font-Names="Arial" Font-Size="X-Small" GroupName="grF"></asp:radiobutton></FONT></TD>
								<TD><FONT face="Arial"><asp:textbox id="TextBox6" runat="server" Font-Names="Arial" Width="180px" Font-Size="XX-Small" MaxLength="30" BorderWidth="0px"></asp:textbox>/
										<asp:textbox id="TextBox7" runat="server" Font-Names="Arial" Width="100px" Font-Size="XX-Small" MaxLength="15" BorderWidth="0px"></asp:textbox></FONT></TD>
							</TR>
							<TR>
								<TD><FONT face="Arial"></FONT></TD>
								<TD noWrap width="100%"><FONT face="Arial" size="1">
										<HR width="100%" SIZE="1">
										(Versicherungsgesellschaft/Versich.schein-Nr.)</FONT></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD vAlign="bottom" align="left" colSpan="2"><FONT face="Arial" size="2"><STRONG>Aktions-/Tageszulassungen</STRONG></FONT></TD>
				</TR>
				<TR>
					<TD vAlign="bottom" align="left" colSpan="2">
						<TABLE id="Table7" height="75" cellSpacing="1" cellPadding="1" width="100%" border="0">
							<TR>
								<TD vAlign="top"><FONT face="Arial"><FONT size="2">Bei Fahrzeugen als händlereigene 
											Aktions-/Tageszulassung gilt: Das Fahrzeug wird nicht am Straßenverkehr 
											teilnehmen und die Kennzeichen werden nicht montiert. Die Bank wird das 
											Fahrzeug gegen Kaskoschäden (Kasko-Ruherisiko) mit einer Selbstbeteiligung auf 
											unsere Kosten versichern. Kopie der Fahrzeugrechnung liegt diesem Antrag bei.</FONT></FONT></TD>
							</TR>
						</TABLE>
						<FONT face="Arial" size="2">Die Finanzierunssumme/n bitten wir, mit der EF-Nr. zu 
							verrechnen.</FONT>
					</TD>
				</TR>
				<TR>
					<TD vAlign="bottom" align="left" colSpan="2">
						<TABLE id="Table8" cellSpacing="1" cellPadding="1" width="100%" border="0">
							<TR>
								<TD width="100%"><FONT face="Arial" size="2"></FONT></TD>
								<TD noWrap></TD>
							</TR>
							<TR>
								<TD width="100%"></TD>
								<TD noWrap></TD>
							</TR>
							<TR>
								<TD width="100%">
									<HR width="100%" SIZE="1">
									<FONT face="Arial" size="1">Ort/Datum</FONT></TD>
								<TD noWrap>
									<HR width="100%" SIZE="1">
									<FONT face="Arial" size="1">Firmenstempel / Unterschrift des Händlers</FONT></TD>
							</TR>
						</TABLE>
						<FONT face="Arial" size="1">*S=Selbstfahrervermietfahrzeug, V=Vorführfahrzeug, 
							HEZ=Händlereigene Zulassung</FONT>&nbsp;
					</TD>
				</TR>
			</TABLE>
			&nbsp;
		</form>
	</body>
</HTML>
