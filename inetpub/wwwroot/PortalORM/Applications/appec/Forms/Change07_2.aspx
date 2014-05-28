<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change07_2.aspx.vb" Inherits="AppEC.Change07_2" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:Styles id="ucStyles" runat="server"></uc1:Styles>
	    <style type="text/css">
            .style4
            {
                font-weight: bold;
                text-decoration: underline;
            }
        </style>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table4" width="100%" align="center">
				<TR>
					<td><uc1:Header id="ucHeader" runat="server"></uc1:Header></td>
				</TR>
				<TR>
					<TD>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="PageNavigation" colSpan="2">
									<asp:Label id="lblHead" runat="server"></asp:Label>
								</TD>
							</TR>
							<TR>
								<TD class="TaskTitle" colSpan="2">Bitte geben Sie hier die Daten für die Unfallmeldung ein.</TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">
								    <asp:linkbutton id="cmdBack" runat="server" CssClass="StandardButton" > &#149;&nbsp;Zurück</asp:linkbutton>
								</TD>
								<TD vAlign="top">
								
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="500px" border="0">
										<TR>
											<TD class="" vAlign="top">
												<TABLE class="BorderLeftBottom" id="Table5" cellSpacing="1" cellPadding="1" width="100%" border="0">
																										
													<TR>
														<TD>&nbsp;</TD>
														<TD  colspan="3">
								        <asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label>
								        <asp:label id="lblMessage" runat="server" EnableViewState="False" 
                                                ForeColor="#000099"></asp:label>
															</TD>
													</TR>
																										
													<TR>
														<TD>&nbsp;</TD>
														<TD class="style4" >
															Fahrzeuginfo</TD>
														<TD colspan="2">
															&nbsp;</TD>
													</TR>
																										
													<TR>
														<TD>&nbsp;</TD>
														<TD>
															Fahrgestellnummer:</TD>
														<TD colspan="2">
                                            <asp:Label ID="lblFahrgestellnummerShow" runat="server" ForeColor="#666666"></asp:Label>
                                                        </TD>
													</TR>
																										
													<TR>
														<TD>&nbsp;</TD>
														<TD>
															Kennzeichen:</TD>
														<TD>
                                            <asp:Label ID="lblKennzeichenShow" runat="server" ForeColor="#666666"></asp:Label>
                                                        </TD>
														<TD>
															&nbsp;</TD>
													</TR>
																										
													<TR>
														<TD>&nbsp;</TD>
														<TD>
															Briefnummer:</TD>
														<TD>
                                            <asp:Label ID="lblBriefnummerShow" runat="server" ForeColor="#666666"></asp:Label>
                                                        </TD>
														<TD>
															&nbsp;</TD>
													</TR>
																										
													<TR>
														<TD>&nbsp;</TD>
														<TD>
															Unit-Nr.</TD>
														<TD>
                                            <asp:Label ID="lblUnitnrShow" runat="server" ForeColor="#666666"></asp:Label>
                                                        </TD>
														<TD>
															&nbsp;</TD>
													</TR>
																										
													<TR>
														<TD>&nbsp;</TD>
														<TD>
															&nbsp;</TD>
														<TD>
															&nbsp;</TD>
														<TD>
															&nbsp;</TD>
													</TR>
																										
													<TR>
														<TD>&nbsp;</TD>
														<TD class="style4" >
															Station</TD>
														<TD>
															&nbsp;</TD>
														<TD>
															&nbsp;</TD>
													</TR>
																										
													<TR>
														<TD></TD>
														<TD>
															<P>Stationscode:</P>
														</TD>
														<TD>
															<asp:TextBox id="txtStationscode" runat="server"></asp:TextBox></TD>
														<TD>
															<asp:LinkButton id="cmdCreate" runat="server" CssClass="StandardButton"> •&nbsp;Existenz prüfen</asp:LinkButton></TD>
													</TR>
													<TR>
														<TD>&nbsp;</TD>
														<TD valign="top" >&nbsp;</TD>
														<TD>
															&nbsp;</TD>
														<TD>&nbsp;</TD>
													</TR>
													<TR>
														<TD></TD>
														<TD valign="top" >Adresse:</TD>
														<TD colspan="2">
															<asp:Label ID="lblStation" runat="server" ForeColor="#666666" 
                                                                style="font-weight: 700"></asp:Label>
                                                        </TD>
													</TR>
													<TR>
														<TD>&nbsp;</TD>
														<TD valign="top" >&nbsp;</TD>
														<TD colspan="2">
															&nbsp;</TD>
													</TR>
													<TR id="tr0" runat="server" visible="false">
														<TD>&nbsp;</TD>
														<TD valign="top" class="style4" >Meldungsdaten</TD>
														<TD colspan="2">
															&nbsp;</TD>
													</TR>
													<TR id="tr1" runat="server" visible="false">
														<TD>&nbsp;</TD>
														<TD valign="top" >Standort:</TD>
														<TD colspan="2">
                                                             <asp:TextBox ID="txtStandort" runat="server" MaxLength="60" Width="340px" ></asp:TextBox>
                                                        </TD>
													</TR>
													<TR id="tr2" runat="server" visible="false">
														<TD>&nbsp;</TD>
														<TD valign="top"> 
                                                            &nbsp;</TD>
														<TD>
                                                             &nbsp;</TD>
														<TD>
															<asp:LinkButton id="cmdSave" runat="server" CssClass="StandardButton"> •&nbsp;Sichern</asp:LinkButton></TD>
													</TR>
													<TR>
														<TD>&nbsp;</TD>
														<TD valign="top"> 
                                                            &nbsp;</TD>
														<TD>
                                                             &nbsp;</TD>
														<TD>
                                                             &nbsp;</TD>
													</TR>
													</TABLE>
													
											</TD>
										</TR>
									</TABLE>
									
									
								</TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD vAlign="top">&nbsp;</TD>
							</TR>
							
							<tr>
							<TD vAlign="top">&nbsp;</TD>
								<TD><!--#include File="../../../PageElements/Footer.html" -->
							</tr>
							
							
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>