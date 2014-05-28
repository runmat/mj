<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UeberfDADEinRueck.aspx.vb" Inherits="AppUeberf.UeberfDADEinRueck" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD runat="server">
	    
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	    <style type="text/css">
            .style2
            {
                width: 320px;
            }
            .style3
            {
                width: 100%;
                height: 17px;
            }
            .style12
            {
                width: 198px;
            }
                        
            .style35
            {
                width: 105px;
                font-weight: bold;
            }
            .style36
            {
                width: 198px;
                font-weight: bold;
                color: #009900;
            }

            .style37
            {
                width: 141px;
            }

            .style38
            {
                font-weight: normal;
            }

            .style40
            {
                width: 413px;
            }

            .style42
            {
                width: 105px;
                font-weight: bold;
                color: #009900;
            }
            .style43
            {
                width: 105px;
            }
            .style45
            {
                color: #009900;
                font-weight: bold;
            }
            .style46
            {
                color: #009900;
                font-weight: bold;
                width: 418px;
            }
            .style47
            {
                width: 418px;
            }
            .style48
            {
                width: 95%;
            }
            .style49
            {
                width: 157px;
            }
            .style51
            {
                width: 22px;
            }
            .style52
            {
                width: 100%;
            }
            .style53
            {
                width: 307px;
            }

            .style54
            {
                width: 85px;
            }
            .style55
            {
                width: 146px;
            }
            .style56
            {
                width: 330px;
            }

            </style>
	    </HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<table width="100%" align="center" cellSpacing="0" cellPadding="0">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td vAlign="top" align="left" width="100%" colSpan="3">
						<table id="Table10" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;(
									<asp:label id="lblPageTitle" runat="server">Rückführung/Anschlußfahrt</asp:label>)</td>
							</tr>
							<tr>
								<td style="WIDTH: 144px" vAlign="top" width="174">
									<table id="Table12" cellSpacing="0" cellPadding="0" border="0">
										<tr>
											<td width="150">
												<asp:linkbutton id="lbtWeiter" tabIndex="12" runat="server" 
                                                    CssClass="StandardButtonTable" Width="100px"> •&nbsp;Weiter</asp:linkbutton>
												</td>
										</tr>
										<tr>
											<td vAlign="middle" width="150"><asp:linkbutton id="lbtBack" tabIndex="12" 
                                                    runat="server" CssClass="StandardButtonTable" Width="100px"> •&nbsp;Zurück</asp:linkbutton></td>
										</tr>
										<tr>
											<td vAlign="middle" width="150">
									<asp:calendar id="calWunschtermin" runat="server" Width="53px" CellPadding="0" BorderStyle="Solid" 
                                                    BorderColor="Black" Visible="False" Height="133px">
										<TodayDayStyle Font-Bold="True"></TodayDayStyle>
										<NextPrevStyle ForeColor="White"></NextPrevStyle>
										<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
										<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
										<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
										<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
										<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
									</asp:calendar>
									        </td>
										</tr>
									</table>
								</td>
								<td style="WIDTH: 917px" vAlign="top">
									<table id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td style="WIDTH: 437px" width="437">
												<asp:Literal ID="ltAnzeige" runat="server"></asp:Literal>
                                            </td>
										</tr>
										<tr>
											<td style="WIDTH: 437px" width="437">
												<asp:label id="lblError" runat="server" Width="821px" EnableViewState="False" CssClass="TextError"></asp:label></td>
										</tr>
										<tr>
											<td style="WIDTH: 437px" width="437">
												<table cellpadding="1" cellspacing="1" style="width: 188%">
                                                    <tr>
                                                        <td class="style46">
                                                        Leasing</td>
                                                        <td class="style51">
                                                            &nbsp;</td>
                                                        <td class="style45">
                                                        Fahrzeugnutzer</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style47">
                                                            <asp:Panel ID="pnl1" runat="server" BorderColor="Black" BorderStyle="Solid" 
                                                                BorderWidth="1px" Height="80px">
                                                                <table cellpadding="0" cellspacing="0" class="style48">
                                                                    <tr>
                                                                        <td class="style49">
                                                                            Leasingnehmernr.</td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtLnNummer" runat="server" MaxLength="10" TabIndex="1"></asp:TextBox>
                                                                            <asp:ImageButton ID="ibtRefresh0" runat="server" Height="16px" 
                                                                                ImageUrl="/Portal/images/refresh.gif" ToolTip="Aktualisieren" Width="16px" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="style49">
                                                                            Leasingvertragsnr*</td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtLnVertragsnr" runat="server" MaxLength="20" TabIndex="2"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="style49">
                                                                            Ansprechpartner Leasing</td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtLnAnsprech" runat="server" MaxLength="35" TabIndex="3" 
                                                                                Width="245px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                        <td class="style51">
                                                            &nbsp;</td>
                                                        <td>
                                                            <asp:Panel ID="pnl2" runat="server" BorderStyle="Solid" BorderWidth="1px" 
                                                                Height="80px" Width="400px">
                                                                <table cellpadding="0" cellspacing="0" class="style48">
                                                                    <tr>
                                                                        <td class="style54">
                                                                            Name*</td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtFnName" runat="server" TabIndex="4" Width="221px" 
                                                                                MaxLength="35"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="style54">
                                                                            Telefon</td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtFnTelefon" runat="server" TabIndex="6" Width="179px" 
                                                                                MaxLength="16"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="style54">
                                                                            E-Mail<br />
                                                                            &nbsp;</td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtFnMail" runat="server" TabIndex="7" Width="178px" 
                                                                                MaxLength="241"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
										</tr>
										<tr><td>
                                            <table cellpadding="1" cellspacing="0" width="100%">
                                                <tr>
                                                    <td class="style42">
                                                        &nbsp;</td>
                                                    <td class="style53">
                                                        &nbsp;</td>
                                                    <td class="style36">
                                                        &nbsp;</td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td class="style42">
                                                        Fahrzeugdaten                                        &nbsp;</td>
                                                    <td class="style36">
                                                        &nbsp;</td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                             &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <asp:Panel ID="pnl3" runat="server" BorderColor="Black" BorderStyle="Solid" 
                                                            BorderWidth="1px" Width="818px">
                                                            <table cellspacing="0" class="style52" cellpadding="0">
                                                <tr>
                                                    <td class="style43">
                                                        Fahrzeugtyp*</td>
                                                    <td class="style53">
                                                        <asp:TextBox ID="txtLnFahrzeugtyp" runat="server" MaxLength="25" TabIndex="8"></asp:TextBox>
                                                    </td>
                                                    <td class="style12">
                                                        Kennzeichen*</td>
                                                    <td>
                                                        <asp:TextBox ID="txtKennzeichen" runat="server" TabIndex="12" MaxLength="11"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style43">
                                                        FIN*</td>
                                                    <td class="style53">
                                                        <asp:TextBox ID="txtFahrgestellnummer" runat="server" MaxLength="17" 
                                                            TabIndex="10"></asp:TextBox>
                                                    </td>
                                                    <td class="style12">
                                                        Bereifung*</td>
                                                    <td>
                                                        <asp:radiobuttonlist id="rdbBereifung" runat="server" Width="137px" 
                                                            Height="19px" RepeatDirection="Horizontal" TextAlign="Left" TabIndex="13">
													<asp:ListItem Value="S">Sommer</asp:ListItem>
													<asp:ListItem Value="W">Winter</asp:ListItem>
													<asp:ListItem Value="G">Ganzjahresreifen</asp:ListItem>
												</asp:radiobuttonlist>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style43">
                                                        <asp:Label ID="lblAbmeldung" runat="server" Text="Abmeldung"></asp:Label>
                                                    </td>
                                                    <td class="style53">
                                                        <asp:CheckBox ID="chkAbmeldung" runat="server" Checked="True" />
                                                    </td>
                                                    <td class="style12">
                                                        Fahrzeugklasse in Tonnen*</td>
                                                    <td>
                                                        <asp:radiobuttonlist id="rdbFahrzeugklasse" runat="server" Width="217px" 
                                                            Height="22px" RepeatDirection="Horizontal" TextAlign="Left" TabIndex="14">
													<asp:ListItem Value="PKW" Selected="True">&lt; 3,5</asp:ListItem>
													<asp:ListItem Value="LKW">3,5 - 7,5</asp:ListItem>
													<asp:ListItem Value="LKW">&gt; 7,5</asp:ListItem>
												</asp:radiobuttonlist></td>
                                                </tr>
                                                <tr>
                                                    <td class="style43">
                                                        &nbsp;</td>
                                                    <td class="style53">
                                                        &nbsp;</td>
                                                    <td class="style12">
                                                        &nbsp;</td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td class="style43">
                                                        <asp:Label ID="lblWunschtermin" runat="server" Text="Wunschtermin*" 
                                                            Visible="False"></asp:Label>
                                                    </td>
                                                    <td class="style53">
                                                        <asp:TextBox ID="txtWunschtermin" runat="server" MaxLength="10" TabIndex="15" 
                                                            Visible="False"></asp:TextBox>
&nbsp;<asp:ImageButton ID="ibtCalWunschtermin" runat="server" ImageUrl="/Portal/images/calendar.jpg" Visible="False" />
                                                    </td>
                                                    <td class="style12">
                                                        &nbsp;</td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                                <tr>
                                                                    <td class="style43">
                                                                        <asp:Label ID="lblTerminart" runat="server" Text="Terminart"></asp:Label>
                                                                    </td>
                                                                    <td class="style53">
                                                                        <asp:RadioButtonList ID="rdoAuslieferung" runat="server" 
                                                                            RepeatDirection="Horizontal" Visible="False">
                                                                            <asp:ListItem Selected="True" Value="1">frühestens ab</asp:ListItem>
                                                                            <asp:ListItem Value="2">spätestens am</asp:ListItem>
                                                                        </asp:RadioButtonList>
                                                                    </td>
                                                                    <td class="style12">
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                    
                                                </tr>
                                                <tr>
                                                    <td class="style42">
                                                        &nbsp;&nbsp;</td>
                                                    <td class="style53">
                                                        &nbsp;</td>
                                                    <td class="style12">
                                                        &nbsp;</td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>

                                                <tr>
                                                    <td class="style42">
                                                        Abholadresse                                        &nbsp;</td>
                                                    <td class="style12">
                                                        &nbsp;</td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                             &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <asp:Panel ID="pnl4" runat="server" BorderColor="Black" BorderStyle="Solid" 
                                                            BorderWidth="1px" Width="818px">
                                                            <table cellpadding="0" cellspacing="0" class="style52">
                                                 <tr>
                                                    <td class="style35">
                                                        <span class="style38">Name</span>*</td>
                                                    <td class="style56">
                                                        <asp:TextBox ID="txtAbName" runat="server" Width="245px" MaxLength="35" 
                                                            TabIndex="16"></asp:TextBox>
                                                        <asp:ImageButton ID="ibtSearchAbhol" runat="server" 
                                                                    ImageUrl="/Portal/images/lupe2.gif" ToolTip="Adresssuche" />
                                                    </td>
                                                    <td class="style55">
                                                        Ansprechpartner*</td>
                                                    <td>
                                                        <asp:TextBox ID="txtAbAnsprech" runat="server" TabIndex="21" MaxLength="35"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style43">
                                                        Strasse, Nr.*</td>
                                                    <td class="style56">
                                                        <asp:TextBox ID="txtAbStrasse" runat="server" Width="295px" MaxLength="35" 
                                                            TabIndex="17"></asp:TextBox>
                                                    </td>
                                                    <td class="style55">
                                                        Telefon*</td>
                                                    <td>
                                                        <asp:TextBox ID="txtAbTelefon" runat="server" TabIndex="22" MaxLength="16"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style43">
                                                        PLZ, Ort*</td>
                                                    <td class="style56">
                                                        <asp:TextBox ID="txtAbPLZ" runat="server" Width="60px" MaxLength="7" 
                                                            TabIndex="18"></asp:TextBox>
&nbsp;<asp:TextBox ID="txtAbOrt" runat="server" Width="230px" TabIndex="19" MaxLength="35"></asp:TextBox>
                                                    </td>
                                                    <td class="style55">
                                                        Telefon2</td>
                                                    <td>
                                                        <asp:TextBox ID="txtAbHandy" runat="server" TabIndex="24" MaxLength="16"></asp:TextBox>
                                                        </td>
                                                </tr>
                                                <tr>
                                                    <td class="style43">
                                                        Land*</td>
                                                    <td class="style56">
                                                        <asp:DropDownList ID="drpAbLand" runat="server" TabIndex="20">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="style55">
                                                        E-Mail</td>
                                                    <td>
                                                        <asp:TextBox ID="txtAbMail" runat="server" MaxLength="40" TabIndex="23" 
                                                            Width="200px"></asp:TextBox>
                                                        </td>
                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr><td colspan="4">&nbsp;&nbsp;</td></tr>
                                                <tr><td class="style42" colspan="4">Anlieferadresse</td></tr>
                                                <tr><td colspan="4">
                                                    <asp:Panel ID="pnl6" runat="server" BorderColor="Black" BorderStyle="Solid" 
                                                        BorderWidth="1px" Width="818px">
                                            <table class="style3" cellpadding="1" cellspacing="0">
                                                <tr>
                                                    <td class="style40">
                                                        <asp:Label ID="lblAnlieferungError" runat="server" ForeColor="#CC0000" 
                                                            Text="Autoland. Bitte wählen Sie eine Adresse aus der Liste:" 
                                                            Visible="False" Font-Bold="True"></asp:Label>
                                                    </td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td class="style40">
                                                        <asp:DropDownList ID="drpAutoland" runat="server" Visible="False" Width="500px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        &nbsp;<asp:ImageButton ID="ibtRefresh" runat="server" 
                                                                    ImageUrl="/Portal/images/select.gif" ToolTip="Übernehmen" 
                                                            Height="16px" Width="16px" Visible="False" />
                                                        &nbsp;</td>
                                                </tr>
                                            </table>
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td class="style43" colspan="0" rowspan="0">
                                                        Auswahl<td>
                                                        <asp:RadioButtonList ID="rdoAnlieferadresse" runat="server" 
                                                            RepeatDirection="Horizontal" AutoPostBack="True" TabIndex="25">
                                                            <asp:ListItem Value="Haendler">Händler</asp:ListItem>
                                                            <asp:ListItem Value="Autoland">Autoland</asp:ListItem>
                                                            <asp:ListItem Value="Dritte" Selected="True">Dritte</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                    
                                                    <td>
                                                        &nbsp;</td>
                                                    
                                                </tr>
                                            </table>
                                              <table cellpadding="1" cellspacing="0" width="100%">
                                                <tr>
                                                    <td class="style43">
                                                        Name*</td>
                                                    <td class="style2">
                                                        <asp:TextBox ID="txtAnName" runat="server" Width="245px" MaxLength="35" 
                                                            TabIndex="26"></asp:TextBox>
                                                        <asp:ImageButton ID="ibtSearchAnlieferung" runat="server" 
                                                                    ImageUrl="/Portal/images/lupe2.gif" ToolTip="Adresssuche" />
                                                    </td>
                                                    <td class="style55">
                                                        Ansprechpartner*</td>
                                                    <td>
                                                        <asp:TextBox ID="txtAnAnsprech" runat="server" MaxLength="35" TabIndex="30"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style43">
                                                        Strasse, Nr.*</td>
                                                    <td class="style2">
                                                        <asp:TextBox ID="txtAnStrasse" runat="server" Width="244px" TabIndex="27" 
                                                            MaxLength="35"></asp:TextBox>
                                                    </td>
                                                    <td class="style55">
                                                        Telefon1*</td>
                                                    <td>
                                                        <asp:TextBox ID="txtAnTelefon" runat="server" MaxLength="16" TabIndex="31"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style43">
                                                        PLZ*, Ort*</td>
                                                    <td class="style2">
                                                        <asp:TextBox ID="txtAnPLZ" runat="server" Width="60px" MaxLength="7" 
                                                            TabIndex="28"></asp:TextBox>
&nbsp;<asp:TextBox ID="txtAnOrt" runat="server" Width="233px" MaxLength="35"></asp:TextBox>
                                                    </td>
                                                    <td class="style55">
                                                                                                                Telefon2</td>
                                                    <td>
                                                        <asp:TextBox ID="txtAnHandy" runat="server" MaxLength="16"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                  <tr>
                                                      <td class="style43">
                                                          Land*</td>
                                                      <td class="style2">
                                                          <asp:DropDownList ID="drpAnLand" runat="server" TabIndex="29">
                                                          </asp:DropDownList>
                                                      </td>
                                                      <td class="style55">
                                                          E-Mail</td>
                                                      <td>
                                                          <asp:TextBox ID="txtAnMail" runat="server" MaxLength="40" TabIndex="33" 
                                                              Width="200px"></asp:TextBox>
                                                      </td>
                                                  </tr>
                                                </table>                                                        
                                                        
                                                    </asp:Panel>
                                                    </td></tr>
                                                <tr><td class="style42" colspan="4">&nbsp;</td></tr>
                                                </table>

                                            </td>
                                            </tr>
                                                <tr><td>
                                                    <asp:Panel ID="pnl7" runat="server" BorderColor="Black" BorderStyle="Solid" 
                                                        BorderWidth="1px" Width="818px">
                                                       <table>
                                                            <tr>
                                                                <td class="style37">
                                                                    Bemerkung</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtBemerkung" runat="server" Height="48px" Width="650px" 
                                                                        MaxLength="256" TabIndex="41"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>                                                        
                                                    </asp:Panel>
                                                    </td></tr>
							<tr><td>
 
                                </td></tr>
                                <tr><td>
                                    </td></tr>
                                
									</table>
								
								</table>
							
									
									
                                </td>
									
							</tr>
						</table>
			</form>
	</body>
</HTML>
