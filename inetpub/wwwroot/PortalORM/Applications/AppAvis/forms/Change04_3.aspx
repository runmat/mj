<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change04_3.aspx.vb" Inherits="AppAvis.Change04_3" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	    <style type="text/css">
            .style2
            {
                font-size: xx-small;
            }
            .style3
            {
                width: 100%;
            }
        </style>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table4" width="100%" align="center">
				<TR>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</TR>
				<TR>
					<TD>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;
									<asp:label id="lblPageTitle" runat="server"> (Freigeben/Beauftragen)</asp:label></TD>
							</TR>
							<TR>
								<TD class="TaskTitle" colSpan="2">&nbsp;</TD>
							</TR>
							<TR>
								<TD colSpan="2">
                                    <table cellpadding="0" cellspacing="0" class="style3">
                                        <tr>
                                            <td width="120">
                                                <asp:linkbutton id="cmdCreate" runat="server" CssClass="StandardButton"> •&nbsp;Suchen</asp:linkbutton>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblEinbau" runat="server" Text="Einbaufirma:" Width="80px" 
                                                    Visible="False"></asp:Label>
                                                <STRONG>
                                                            <asp:dropdownlist id="drpEinbau" 
                                                                runat="server" Width="300px" AppendDataBoundItems="True" 
                                                    Visible="False">
                                                            <asp:ListItem Selected="True">Auswahl</asp:ListItem>
                                                            </asp:dropdownlist>&nbsp;</STRONG></td>
                                        </tr>
                                        <tr>
                                            <td width="120">
                                                <asp:linkbutton id="cmdBack" runat="server" CssClass="StandardButton"> &#149;&nbsp;Zurück</asp:linkbutton>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblAusruest" runat="server" Text="Ausrüstung:" Width="80px" 
                                                    Visible="False"></asp:Label>
                                                <STRONG>
                                                            <asp:dropdownlist id="drpAusruest" 
                                                                runat="server" Width="300px" AppendDataBoundItems="True" 
                                                    Visible="False">
                                                            <asp:ListItem Selected="True">Auswahl</asp:ListItem>
                                                            </asp:dropdownlist>&nbsp;</STRONG></td>
                                        </tr>
                                        <tr>
                                            <td width="120">
                                                &nbsp;</td>
                                            <td>
									<asp:label id="lblErrMessage" runat="server" CssClass="TextError" EnableViewState="False"></asp:label>
								            </td>
                                        </tr>
                                    </table>
                                </TD>
							</TR>
							<TR>
								<TD colSpan="2">
								<asp:Panel ID="pnlAuswahl" runat="server">
                                    <table class="style3">
                                        <tr>
                                            <td width="120">
                                                &nbsp;</td>
                                            <td width="180">
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td width="120">
                                                &nbsp;</td>
                                            <td width="180">
                                                Carport:</td>
                                            <td>
                                                <strong>
                                                    <asp:DropDownList ID="drpCarport" runat="server" AppendDataBoundItems="True" 
                                                        Width="157px">
                                                        <asp:ListItem Selected="True">Auswahl</asp:ListItem>
                                                    </asp:DropDownList>
                                                </strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="120">
                                                &nbsp;</td>
                                            <td width="180">
                                                Hersteller-ID:</td>
                                            <td>
                                                <strong>
                                                    <asp:DropDownList ID="drpHersteller" runat="server" AppendDataBoundItems="True" 
                                                        AutoPostBack="True" Width="157px">
                                                        <asp:ListItem Selected="True">Auswahl</asp:ListItem>
                                                    </asp:DropDownList>
                                                </strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="120">
                                                &nbsp;</td>
                                            <td width="180">
                                                Modellgruppe:</td>
                                            <td>
                                                <strong>
                                                    <asp:DropDownList ID="drpModellgruppe" runat="server" 
                                                        AppendDataBoundItems="True" Enabled="False" Width="157px">
                                                        <asp:ListItem Selected="True">Auswahl</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <span class="style2">(Zum Aktivieren bitte Hersteller auswählen.)</span></strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="120">
                                                &nbsp;</td>
                                            <td width="180">
                                                Kraftstoff:</td>
                                            <td>
                                                <strong>
                                                    <asp:DropDownList ID="drpKraftstoff" runat="server" AppendDataBoundItems="True" 
                                                        Width="157px">
                                                        <asp:ListItem Selected="True">Auswahl</asp:ListItem>
                                                    </asp:DropDownList>
                                                </strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="120">
                                                &nbsp;</td>
                                            <td width="180">
                                                Navigation:</td>
                                            <td>
                                                <strong>
                                                    <asp:DropDownList ID="drpNavi" runat="server" AppendDataBoundItems="True" 
                                                        Width="157px">
                                                        <asp:ListItem Selected="True">Auswahl</asp:ListItem>
                                                    </asp:DropDownList>
                                                </strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="120">
                                                &nbsp;</td>
                                            <td width="180">
                                                Bereifung:</td>
                                            <td>
                                                <strong>
                                                    <asp:DropDownList ID="drpReifenart" runat="server" AppendDataBoundItems="True" 
                                                        Width="157px">
                                                        <asp:ListItem Selected="True">Auswahl</asp:ListItem>
                                                    </asp:DropDownList>
                                                </strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="120">
                                                &nbsp;</td>
                                            <td width="180">
                                                Aufbauart:</td>
                                            <td>
                                                <strong>
                                                    <asp:DropDownList ID="drpAufbauart" runat="server" AppendDataBoundItems="True" 
                                                        Width="157px">
                                                        <asp:ListItem Selected="True">Auswahl</asp:ListItem>
                                                    </asp:DropDownList>
                                                </strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="120">
                                                &nbsp;</td>
                                            <td width="180">
                                                Typ:</td>
                                            <td>
                                                <strong>
                                                    <asp:TextBox ID="txtTyp" runat="server"></asp:TextBox>
                                                </strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="120">
                                                &nbsp;</td>
                                            <td width="180">
                                                Händlernummer:</td>
                                            <td>
                                                 <asp:TextBox ID="txtHaendler" runat="server" MaxLength="10"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                                </asp:Panel>
								
								</TD>
							</TR>
							<TR>
								<TD class="PageNavigation" colSpan="2">
                                    <asp:GridView ID="grvFahrzeuge" runat="server" AutoGenerateColumns="False" 
                                        Visible="False" AllowSorting="True" CssClass="tableMain">
                                        <Columns>
                                            <asp:BoundField DataField="ID_BLOCK_RG" HeaderText="ID Regel" />
                                            <asp:BoundField DataField="CARPORT" HeaderText="Carport" />
                                            <asp:BoundField DataField="CHASSIS_NUM" HeaderText="Fahrgestellnummer" />
                                            <asp:BoundField DataField="ZZDAT_EIN" 
                                                HeaderText="Eingang Fahrzeug Carport" 
                                                DataFormatString="{0:dd.MM.yyyy}" />
                                            <asp:BoundField DataField="DAT_EING_ZBII" HeaderText="Eingang ZBII" 
                                                DataFormatString="{0:dd.MM.yyyy}" />
                                            <asp:BoundField DataField="EINBAUFIRMA" HeaderText="Ausrüsterfirma" />
                                            <asp:BoundField DataField="AUSRUESTUNG" HeaderText="Einbau" />
                                            <asp:BoundField DataField="DAT_BEAUFTR" DataFormatString="{0:dd.MM.yyyy}" 
                                                HeaderText="Datum der Beauftragung" />
                                            <asp:BoundField DataField="DAT_SPERR_ANL" HeaderText="Datum Sperre" 
                                                DataFormatString="{0:dd.MM.yyyy}" />
                                            <asp:BoundField DataField="SPERRVERMERK" HeaderText="Sperrvermerk" />
                                            <asp:TemplateField HeaderText="Freigeben">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkFreigabe" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">
									&nbsp;</TD>
								<TD vAlign="top">
									&nbsp;<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left">
												&nbsp;</TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD vAlign="top"></TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD><!--#include File="../../../PageElements/Footer.html" --> &nbsp;</TD>
							</TR>
							<TR>
								<TD vAlign="top"></TD>
								<TD></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>