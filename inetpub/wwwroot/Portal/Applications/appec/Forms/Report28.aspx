<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report28.aspx.vb" Inherits="AppEC.Report28"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" encType="multipart/form-data" runat="server" DefaultButton="cmdCreate">
			<table width="100%" align="center">
				<tr>
					<td>
					    <uc1:header id="ucHeader" runat="server"></uc1:header>
					</td>
				</tr>
				<tr>
					<td>
						<table id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td class="PageNavigation" colSpan="2">
									<asp:label id="lblHead" runat="server"></asp:label>
								</td>
							</tr>
							<tr>
								<td vAlign="top" width="120">
								    <asp:calendar id="calKalender" runat="server" BorderStyle="Solid" BorderColor="Black" CellPadding="0" Width="120px" Visible="False">
										<TodayDayStyle Font-Bold="True"></TodayDayStyle>
										<NextPrevStyle ForeColor="White"></NextPrevStyle>
										<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
										<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
										<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
										<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
										<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
									</asp:calendar>
                                    <input type="hidden" id="ihSelectedCalendar" runat="server" value=""/>
								</td>
								<td vAlign="top">
									<div align="top">
										<table id="tblSelection" cellSpacing="0" cellPadding="0" border="0" runat="server">
											<tr>
												<td vAlign="top">
													<table id="Table1" class="BorderLeftBottom" cellSpacing="1" cellPadding="1" border="0">
														<tr>
														    <td noWrap colspan="3">
														        <asp:RadioButtonList ID="rblFahrzeugauswahl" RepeatDirection="Horizontal" runat="server" AutoPostBack="True">
														            <asp:ListItem Value="manuell" Selected="True">manuelle Fahrzeugdateneingabe</asp:ListItem>
                                                                    <asp:ListItem Value="upload">Upload Fahrzeugdaten</asp:ListItem>
														        </asp:RadioButtonList>
														    </td>
														</tr>
                                                        <tr>
                                                            <td noWrap colspan="3">&nbsp;</td>
                                                        </tr>
                                                        <tr id="trFahrgestellnummer" runat="server">
														    <td noWrap>
														        Fahrgestellnummer:
														    </td>
														    <td noWrap>
															    <asp:TextBox id="txtFahrgestellnummer" runat="server" MaxLength="30"></asp:TextBox>
                                                            </td>
														    <td noWrap></td>
													    </tr>
                                                        <tr id="trKennzeichen" runat="server">
														    <td noWrap>
														        Kennzeichen:
														    </td>
														    <td noWrap>
															    <asp:TextBox id="txtKennzeichen" runat="server" MaxLength="15"></asp:TextBox>
                                                            </td>
														    <td noWrap></td>
													    </tr>
                                                        <tr id="trZB2Nummer" runat="server">
														    <td noWrap>
														        ZB2-Nummer:
														    </td>
														    <td noWrap>
															    <asp:TextBox id="txtZB2Nummer" runat="server" MaxLength="20"></asp:TextBox>
                                                            </td>
														    <td noWrap></td>
													    </tr>
                                                        <tr id="trModelId" runat="server">
														    <td noWrap>
														        Model ID:
														    </td>
														    <td noWrap>
															    <asp:TextBox id="txtModelId" runat="server" MaxLength="20"></asp:TextBox>
                                                            </td>
														    <td noWrap></td>
													    </tr>
                                                        <tr id="trUnitnummer" runat="server">
														    <td noWrap>
														        Unit Nr.:
														    </td>
														    <td noWrap>
															    <asp:TextBox id="txtUnitnummer" runat="server" MaxLength="8"></asp:TextBox>
                                                            </td>
														    <td noWrap></td>
													    </tr>
                                                        <tr id="trAuftragsnummer" runat="server">
														    <td noWrap>
														        Auftragsnummer:
														    </td>
														    <td noWrap>
															    <asp:TextBox id="txtAuftragsnummer" runat="server" MaxLength="25"></asp:TextBox>
                                                            </td>
														    <td noWrap></td>
													    </tr>
                                                        <tr id="trBatchId" runat="server">
														    <td noWrap>
														        Batch ID:
														    </td>
														    <td noWrap>
															    <asp:TextBox id="txtBatchId" runat="server"></asp:TextBox>
                                                            </td>
														    <td noWrap></td>
													    </tr>
                                                        <tr id="trSippCode" runat="server">
														    <td noWrap>
														        SIPP Code:
														    </td>
														    <td noWrap>
															    <asp:TextBox id="txtSippCode" runat="server" MaxLength="4"></asp:TextBox>
                                                            </td>
														    <td noWrap></td>
													    </tr>
                                                        <tr id="trUploadHinweis" runat="server" Visible="false">
                                                            <td colspan="3">
                                                                Bitte wählen Sie eine lokal gespeicherte 
									                            Excel-Datei zur Übertragung aus.
                                                            </td>
                                                        </tr>
                                                        <tr id="trUpload" runat="server" Visible="false">
															<td class="" vAlign="top" align="left" colspan="3">
																<input class="InfoBoxFlat" id="upFile" type="file" size="40" name="File1" runat="server"/>
                                                                <a href="javascript:openinfo('Info03.htm');">
                                                                    <img src="/Portal/Images/fragezeichen.gif" border="0"/>
                                                                </a>
															</td>
														</tr>
													    <tr>
														    <td noWrap>
														        Eingang ZB2 von:
														    </td>
														    <td noWrap>
															    <asp:TextBox id="txtEingangZB2_von" runat="server"></asp:TextBox>
                                                            </td>
														    <td noWrap>
															    <asp:LinkButton id="btnCalEingangZB2_von" runat="server" CssClass="StandardButtonTable" Width="100px">Kalender...</asp:LinkButton>
                                                            </td>
													    </tr>
													    <tr>
														    <td noWrap>
														        Eingang ZB2 bis:
														    </td>
														    <td noWrap>
															    <asp:TextBox id="txtEingangZB2_bis" runat="server"></asp:TextBox>
														    <td noWrap>
															    <asp:LinkButton id="btnCalEingangZB2_bis" runat="server" CssClass="StandardButtonTable" Width="100px">Kalender...</asp:LinkButton>
                                                            </td>
													    </tr>
                                                        <tr>
														    <td noWrap>
														        Eingang Fahrzeug von:
														    </td>
														    <td noWrap>
															    <asp:TextBox id="txtEingangFzg_von" runat="server"></asp:TextBox>
                                                            </td>
														    <td noWrap>
															    <asp:LinkButton id="btnCalEingangFzg_von" runat="server" CssClass="StandardButtonTable" Width="100px">Kalender...</asp:LinkButton>
                                                            </td>
													    </tr>
													    <tr>
														    <td noWrap>
														        Eingang Fahrzeug bis:
														    </td>
														    <td noWrap>
															    <asp:TextBox id="txtEingangFzg_bis" runat="server"></asp:TextBox>
														    <td noWrap>
															    <asp:LinkButton id="btnCalEingangFzg_bis" runat="server" CssClass="StandardButtonTable" Width="100px">Kalender...</asp:LinkButton></td>
													    </tr>
                                                        <tr>
														    <td noWrap>
														        Bereitmeldung Fahrzeug von:
														    </td>
														    <td noWrap>
															    <asp:TextBox id="txtBereitmFzg_von" runat="server"></asp:TextBox>
                                                            </td>
														    <td noWrap>
															    <asp:LinkButton id="btnCalBereitmFzg_von" runat="server" CssClass="StandardButtonTable" Width="100px">Kalender...</asp:LinkButton>
                                                            </td>
													    </tr>
													    <tr>
														    <td noWrap>
														        Bereitmeldung Fahrzeug bis:
														    </td>
														    <td noWrap>
															    <asp:TextBox id="txtBereitmFzg_bis" runat="server"></asp:TextBox>
                                                            </td>
														    <td noWrap>
															    <asp:LinkButton id="btnCalBereitmFzg_bis" runat="server" CssClass="StandardButtonTable" Width="100px">Kalender...</asp:LinkButton>
                                                            </td>
													    </tr>
                                                        <tr>
														    <td noWrap>
														        Zulassungsdatum von:
														    </td>
														    <td noWrap>
															    <asp:TextBox id="txtZulassungsdatum_von" runat="server"></asp:TextBox>
                                                            </td>
														    <td noWrap>
															    <asp:LinkButton id="btnCalZulassungsdatum_von" runat="server" CssClass="StandardButtonTable" Width="100px">Kalender...</asp:LinkButton>
                                                            </td>
													    </tr>
													    <tr>
														    <td noWrap>
														        Zulassungsdatum bis:
														    </td>
														    <td noWrap>
															    <asp:TextBox id="txtZulassungsdatum_bis" runat="server"></asp:TextBox>
                                                            </td>
														    <td noWrap>
															    <asp:LinkButton id="btnCalZulassungsdatum_bis" runat="server" CssClass="StandardButtonTable" Width="100px">Kalender...</asp:LinkButton>
                                                            </td>
													    </tr>
                                                        <tr>
														    <td noWrap>
														        Hersteller:
														    </td>
														    <td noWrap>
                                                                <asp:DropDownList ID="ddlHersteller" runat="server">
															    </asp:DropDownList>
                                                            </td>  
														    <td noWrap></td>
													    </tr>
                                                        <%--<tr>
														    <td noWrap>
														        Fahrzeugart:
														    </td>
														    <td noWrap>
															    <asp:DropDownList ID="ddlFahrzeugart" runat="server">
															        <asp:ListItem Value="PKW">PKW</asp:ListItem>
                                                                    <asp:ListItem Value="LKW">LKW</asp:ListItem>
                                                                    <asp:ListItem Value="PKWLKW">PKW + LKW</asp:ListItem>
															    </asp:DropDownList>
                                                            </td>
														    <td noWrap></td>
													    </tr>--%>
                                                        <tr>
														    <td noWrap>
														        PDI Standort:
														    </td>
														    <td noWrap>
                                                                <asp:DropDownList ID="ddlPdiStandort" runat="server">
															    </asp:DropDownList>
                                                            </td>  
														    <td noWrap></td>
													    </tr>
                                                        <tr>
														    <td noWrap>
														        Status:
														    </td>
														    <td noWrap>
                                                                <asp:DropDownList ID="ddlStatus" runat="server">
															    </asp:DropDownList>
                                                            </td>  
														    <td noWrap></td>
													    </tr>
													    <tr>
														    <td noWrap colspan="2">&nbsp;</td>
														    <td noWrap>
															    <asp:LinkButton id="cmdCreate" runat="server" CssClass="StandardButton"> &#149;&nbsp;Erstellen</asp:LinkButton>
                                                            </td>
													    </tr>
													</table>
													<p align="center">&nbsp;</p>
												</td>
											</tr>
										</table>
									</div>
								</td>
				            </tr>
				            <tr>
					            <td vAlign="top" width="50">
					                &nbsp;
                                </td>
					            <td vAlign="top">
					                <asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label>
						            <asp:label id="lblExcelfile" runat="server"></asp:label>
					            </td>
				            </tr>
				            <tr>
					            <td vAlign="top" width="50">
					                &nbsp;
                                </td>
					            <td align="right">
					                <!--#include File="../../../PageElements/Footer.html" -->
                                </td>
				            </tr>
				            <script language="JavaScript">										
                				<!--
				                function openinfo(url) {
				                    fenster = window.open(url, "Fahrzeuge", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0,width=650,height=250");
				                    fenster.focus();
				                }
                			-->
				            </script>
			            </table>
			        </td>
                </tr>
            </table>
	    </form>
	    <asp:literal id="Literal1" runat="server"></asp:literal>
	</body>
</html>