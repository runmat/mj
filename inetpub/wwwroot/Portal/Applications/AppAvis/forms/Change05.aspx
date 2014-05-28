<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change05.aspx.vb" Inherits="AppAvis.Change05" %>
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
            .style1
            {
                width: 100%;
            }
            .style2
            {
                width: 196px;
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
							<TBODY>
								<TR>
									<TD class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:label></TD>
								</TR>
								<TR>
									<TD vAlign="top">
										<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" border="0">
											<TR>
												<TD class="TaskTitle" width="150">&nbsp;</TD>
											</TR>
											<TR>
												<TD vAlign="center" width="150"><asp:linkbutton id="cmdSearch" runat="server" 
                                                        CssClass="StandardButton"> •&nbsp;Suchen</asp:linkbutton></TD>
											</TR>
											<TR>
												<TD vAlign="center" width="150"><asp:linkbutton id="cmdFreigeben" runat="server" 
                                                        CssClass="StandardButton" Visible="False"> •&nbsp;Freigeben</asp:linkbutton></TD>
											</TR>
                                            <tr>
                                                <TD vAlign="center" width="150"><asp:linkbutton id="lbBack" runat="server" 
                                                        CssClass="StandardButton"> •&nbsp;Zurück</asp:linkbutton></TD>
                                            </tr>
										</TABLE>
										<P>&nbsp;</P>
										<P>&nbsp;</P>
									</TD>
									<TD vAlign="top">
										<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
											<TR>
												<TD class="TaskTitle" vAlign="top">&nbsp;</TD>
											</TR>
										</TABLE>
										<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
											<TR>
												<TD vAlign="top" align="left">
													<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
														<TR>
															<TD class="TextLarge" vAlign="center" width="150">&nbsp;</TD>
															<TD class="TextLarge" vAlign="center"></TD>
														</TR>
													</TABLE>
													<table id="tableMain" runat="server" class="BorderLeftBottom">
														<tr>
															<td><asp:label id="lbl_Fahrgestellnummer" runat="server">Fahrgestellnummer</asp:label>&nbsp;
															</td>
															<td><asp:textbox id="txtFahrgestellnummer" MaxLength="17" Runat="server"></asp:textbox></td>
														</tr>
														<tr>
															<td>
                                                                <asp:Label ID="lblMVANr" runat="server" Text="MVA-Nr."></asp:Label>
															</td>
															<td><asp:textbox id="txtOrdernummer" MaxLength="10" Runat="server"></asp:textbox></td>
														</tr>
														<tr>
															<td><asp:label id="lbl_Kennzeichen" runat="server">Kennzeichen</asp:label>
															</td>
															<td><asp:textbox id="txtKennzeichen" MaxLength="15" Runat="server"></asp:textbox></td>
														</tr>
														<tr>
															<td>&nbsp;
															</td>
															<td>&nbsp;</td>
														</tr>
													</table>
												</TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
								<tr>
									<td></td>
									<td><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
								</tr>
								<TR>
									<TD vAlign="top" width="150">&nbsp;</TD>
									<TD vAlign="top">
                                        <asp:Panel ID="pnlAusgabe" runat="server" Visible="False">
                                            <table class="style1">
                                                <tr>
                                                    <td class="style2">
                                                        <b>Kennzeichen:</b></td>
                                                    <td>
                                                        <asp:Label ID="lblPKennzeichen" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style2">
                                                        <b>Fahrgestellnummer:</b></td>
                                                    <td>
                                                        <asp:Label ID="lblPFahrgestellnummer" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style2">
                                                        <b>Herstellername:</b></td>
                                                    <td>
                                                        <asp:Label ID="lblPHersteller" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style2">
                                                        <b>Typ:</b></td>
                                                    <td>
                                                        <asp:Label ID="lblPTyp" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style2">
                                                        <b>Modellbezeichnung</b></td>
                                                    <td>
                                                        <asp:Label ID="lblPModellbezeichnung" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style2">
                                                        <b>Datum Zulassung:</b></td>
                                                    <td>
                                                        <asp:Label ID="lblPDatumZul" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style2">
                                                        <b>Datum geplante Abmeldung:</b></td>
                                                    <td>
                                                        <asp:Label ID="lblPDatumGeplAbmeldung" runat="server"></asp:Label>
                                                        <asp:Label ID="lblPEqui" runat="server" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </TD>
								</TR>
								<TR>
									<TD vAlign="top" width="150"></TD>
					</TD>
					<TD><!--#include File="../../../PageElements/Footer.html" --></TD>
				</TR>
			</TABLE>
			</TD></TR></TBODY></TABLE></form>
	</body>
</HTML>