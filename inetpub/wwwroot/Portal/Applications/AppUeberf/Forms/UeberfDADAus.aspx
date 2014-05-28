<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UeberfDADAus.aspx.vb" Inherits="AppUeberf.UeberfDADAus" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
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
            .style14
            {
                width: 139px;
                font-weight: bold;
                color: #009900;
            }
            .style22
            {
                width: 100%;
            }
            
            .style29
            {
                width: 141px;
            }
            .style30
            {
                width: 123px;
            }
            .style32
            {
                width: 146px;
            }

            .style33
            {
                width: 149px;
            }
            .style34
            {
                width: 202px;
            }

            .style37
            {
                width: 79px;
            }

            .style39
            {
                width: 185px;
            }

            .style40
            {
                width: 141px;
                font-weight: bold;
                color: #009900;
            }
            .style41
            {
                width: 202px;
                color: #009900;
                font-weight: bold;
            }

            .style42
            {
                width: 452px;
                font-weight: bold;
                color: #009900;
            }
            .style43
            {
                width: 452px;
            }
            .style48
            {
                height: 69px;
                width: 96%;
            }

            .style51
            {
                width: 167px;
            }
            .style52
            {
                width: 130px;
            }

            .style53
            {
                width: 82px;
            }

            .style54
            {
                width: 75px;
            }
            .style55
            {
                width: 120px;
            }

            .style56
            {
                width: 177px;
            }

            .style57
            {
                width: 27px;
            }
            .style58
            {
                width: 140px;
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
									<asp:label id="lblPageTitle" runat="server">Auslieferung</asp:label>)</td>
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
									<asp:calendar id="calTerminhinweis" runat="server" Width="53px" CellPadding="0" BorderStyle="Solid" 
                                                    BorderColor="Black" Visible="False" Height="133px">
										<TodayDayStyle Font-Bold="True"></TodayDayStyle>
										<NextPrevStyle ForeColor="White"></NextPrevStyle>
										<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
										<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
										<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
										<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
										<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
									</asp:calendar>
									<asp:calendar id="calAuslieferung" runat="server" Width="53px" CellPadding="0" BorderStyle="Solid" 
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
												<table cellpadding="1" cellspacing="1" style="width: 190%">
                                                    <tr>
                                                        <td class="style42">
                                                        Leasingnehmer</td>
                                                        <td class="style51">
                                                            &nbsp;</td>
                                                        <td class="style40">
                                                        Fahrzeugnutzer</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style43">
                                                            <asp:Panel ID="pnl1" runat="server" BorderColor="Black" BorderStyle="Solid" 
                                                                BorderWidth="1px" Height="150px" Width="400px">
                                                                <table cellpadding="0" cellspacing="0" class="style48">
                                                                    <tr>
                                                                        <td class="style29">
                                                                            Leasingvertragsnr.</td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtReferenz" runat="server" TabIndex="1" MaxLength="20"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="style29">
                                                                            Leasingnehmernr.*</td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtLnNummer" runat="server" TabIndex="2" MaxLength="12"></asp:TextBox>
                                                                            &nbsp;<asp:ImageButton ID="ibtRefresh" runat="server" 
                                                                                ImageUrl="/Portal/images/refresh.gif" ToolTip="Aktualisieren" 
                                                                                Height="16px" Width="16px" />
                                                                            &nbsp;<asp:ImageButton ID="ibtSearchLN" runat="server" 
                                                                                ImageUrl="/Portal/images/lupe2.gif" style="width: 16px" ToolTip="Adresssuche" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="style29">
                                                                            Name*</td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtLnName" runat="server" Width="245px" TabIndex="3" 
                                                                                MaxLength="35"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="style29">
                                                                            Strasse, Nr.*</td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtLnStrasse" runat="server" Width="244px" TabIndex="5" 
                                                                                MaxLength="35"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="style29">
                                                                            PLZ*, Ort*</td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtLnPLZ" runat="server" Width="60px" TabIndex="6" 
                                                                                MaxLength="10"></asp:TextBox>
                                                                            <asp:TextBox ID="txtLnOrt" runat="server" Width="184px" TabIndex="7" 
                                                                                MaxLength="35"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="style29">
                                                                            Land*</td>
                                                                        <td>
                                                                            <asp:DropDownList ID="drpLnLand" runat="server" TabIndex="6">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                        <td class="style51">
                                                            &nbsp;</td>
                                                        <td valign="top">
         &nbsp;</td>
                                                        <td valign="top">
                                                            <asp:Panel ID="pnl2" runat="server" BorderStyle="Solid" BorderWidth="1px" 
                                                                Height="150px" Width="400px">
                                                                <table cellpadding="0" cellspacing="0" class="style48">
                                                                    <tr>
                                                                        <td class="style54">
                                                                            Name*</td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtFnName" runat="server" Width="245px" TabIndex="7" 
                                                                                MaxLength="35"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="style54">
                                                                            Telefon</td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtFnTelefon" runat="server" Width="179px" TabIndex="8" 
                                                                                MaxLength="16"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="style54">
                                                                            E-Mail</td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtFnMail" runat="server" Width="178px" TabIndex="9" 
                                                                                MaxLength="241"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="style54">
                                                                            &nbsp;&nbsp;</td>
                                                                        <td>
                                                                            &nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="style54">
                                                                            &nbsp;&nbsp;</td>
                                                                        <td>
                                                                            &nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="style54">
                                                                            &nbsp;&nbsp;</td>
                                                                        <td>
                                                                            &nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="style54">
                                                                            &nbsp;&nbsp;</td>
                                                                        <td>
                                                                            &nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="style54">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            &nbsp;</td>
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
                                                    <td class="style29">
                                                        &nbsp;</td>
                                                    <td class="style2">
                                                        &nbsp;</td>
                                                    <td class="style12">
                                                        &nbsp;</td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td class="style40">
                                                        Fahrzeugdaten</td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <asp:Panel ID="pnl3" runat="server" BorderColor="Black" BorderStyle="Solid" 
                                                            BorderWidth="1px" Width="825px">
                                                            <table cellpadding="0" cellspacing="0" class="style22">
<tr>
                                                    <td class="style56">
                                                        Fahrzeugtyp*</td>
                                                    <td class="style2">
                                                        <asp:TextBox ID="txtLnFahrzeugtyp" runat="server" TabIndex="10" MaxLength="25"></asp:TextBox>
                                                    </td>
                                                    <td class="style12">
                                                        Kennzeichen</td>
                                                    <td>
                                                        <asp:TextBox ID="txtKennzeichen" runat="server" TabIndex="14" MaxLength="20"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style56">
                                                        FIN*</td>
                                                    <td class="style2">
                                                        <asp:TextBox ID="txtFahrgestellnummer" runat="server" TabIndex="11" 
                                                            MaxLength="17"></asp:TextBox>
                                                    </td>
                                                    <td class="style12">
                                                        Bereifung*</td>
                                                    <td>
                                                        <asp:RadioButtonList ID="rdbBereifung" runat="server" Height="19px" 
                                                            RepeatDirection="Horizontal" TabIndex="15" TextAlign="Left" Width="137px">
                                                            <asp:ListItem Value="S">Sommer</asp:ListItem>
                                                            <asp:ListItem Value="W">Winter</asp:ListItem>
                                                            <asp:ListItem Value="G">Ganzjahresreifen</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style56">
                                                        &nbsp;</td>
                                                    <td class="style2">
                                                    &nbsp;</td>
                                                    <td class="style12">
                                                        Fahrzeugklasse in Tonnen*</td>
                                                    <td>
                                                        <asp:RadioButtonList ID="rdbFahrzeugklasse" runat="server" Height="22px" 
                                                            RepeatDirection="Horizontal" TabIndex="16" TextAlign="Left" Width="217px">
                                                            <asp:ListItem Selected="True" Value="PKW">&lt; 3,5</asp:ListItem>
                                                            <asp:ListItem Value="LKW">3,5 - 7,5</asp:ListItem>
                                                            <asp:ListItem Value="LKW">&gt; 7,5</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                
                                                <tr>
                                                    <td class="style40">
                                                        &nbsp;</td>
                                                    <td class="style2">
                                                        &nbsp;</td>
                                                    <td class="style12">
                                                        &nbsp;</td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                
                                                <tr>
                                                    <td class="style40">
                                                        Händler/Abholadresse</td>
                                                    <td class="style2">
                                                        &nbsp;</td>
                                                    <td class="style12">
                                                        &nbsp;</td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                
                                                <tr>
                                                    <td colspan="4">
                                                        <asp:Panel ID="pnl4" runat="server" Width="825px" BorderColor="Black" 
                                                            BorderStyle="Solid" BorderWidth="1px">
                                                            <table cellpadding="0" cellspacing="0" class="style22">
<tr>
                                                    <td class="style29">
                                                        Name*</td>
                                                    <td class="style2">
                                                        <asp:TextBox ID="txtHaendlerName" runat="server" Width="245px" TabIndex="17" 
                                                            MaxLength="35"></asp:TextBox>
                                                        <asp:ImageButton ID="ibtSearchHaendler" runat="server" 
                                                                    ImageUrl="/Portal/images/lupe2.gif" ToolTip="Adresssuche" />
                                                    </td>
                                                    <td class="style52">
                                                        Ansprechpartner*</td>
                                                    <td>
                                                        <asp:TextBox ID="txtHaendlerApName" runat="server" TabIndex="22" Width="200px" 
                                                            MaxLength="35"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style29">
                                                        Strasse, Nr.*</td>
                                                    <td class="style2">
                                                        <asp:TextBox ID="txtHaendlerStrasse" runat="server" Width="295px" TabIndex="18" 
                                                            MaxLength="35"></asp:TextBox>
                                                    </td>
                                                    <td class="style52">
                                                        Telefon1*</td>
                                                    <td>
                                                        <asp:TextBox ID="txtHaendlerTelefon" runat="server" TabIndex="24" 
                                                            MaxLength="16"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style29">
                                                        PLZ, Ort*</td>
                                                    <td class="style2">
                                                        <asp:TextBox ID="txtHaendlerPLZ" runat="server" Width="60px" TabIndex="19" 
                                                            MaxLength="10"></asp:TextBox>
&nbsp;<asp:TextBox ID="txtHaendlerOrt" runat="server" Width="231px" MaxLength="35"></asp:TextBox>
                                                    </td>
                                                    <td class="style52">
                                                        Telefon2</td>
                                                    <td>
                                                        <asp:TextBox ID="txtHaendlerTelefon2" runat="server" TabIndex="24" 
                                                            MaxLength="16"></asp:TextBox>
                                                        </td>
                                                </tr>
                                                <tr>
                                                    <td class="style29">
                                                        Land*</td>
                                                    <td class="style2">
                                                        <asp:DropDownList ID="drpHaendlerLand" runat="server" TabIndex="20">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="style52">
                                                        E-Mail</td>
                                                    <td>
                                                        <asp:TextBox ID="txtHaendlerMail" runat="server" Width="200px" MaxLength="241"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                
                                                
                                            </table>
                                            <table class="style3" cellpadding="1" cellspacing="0">
                                                <tr>
                                                    <td class="style30">
                                                        &nbsp;</td>
                                                    <td>
                                                        &nbsp;</td>                                                        
                                                    <td>
                                                        &nbsp;</td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                            </table>
                                            </td></tr>
										<tr><td class="style14">Fahrzeugempfänger</td></tr>
										<tr><td>
                                            <asp:Panel ID="pnl5" runat="server" BorderColor="Black" BorderStyle="Solid" 
                                                BorderWidth="1px" Width="825px">
                                                <table cellpadding="0" cellspacing="0" class="style22">
										<tr><td>
                                            <table class="style3">
                                                <tr>
                                                    <td class="style33">
                                                        Auswahl</td>
                                                    <td>
                                                        <asp:RadioButtonList ID="rdoFahrzeugempfaenger" runat="server" 
                                                            RepeatDirection="Horizontal" AutoPostBack="True">
                                                            <asp:ListItem Value="0">Leasingnehmer</asp:ListItem>
                                                            <asp:ListItem Value="1">Halter</asp:ListItem>
                                                            <asp:ListItem Value="2" Selected="True">Dritte</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                    
                                                </tr>
                                            </table>
                                           </td>
                                           </tr>
                                            <tr>
                                            <td>
                                              <table cellpadding="1" cellspacing="0" width="100%">
                                                <tr>
                                                    <td class="style33">
                                                        Name*</td>
                                                    <td class="style2">
                                                        <asp:TextBox ID="txtEmpfName" runat="server" Width="245px" MaxLength="35"></asp:TextBox>
                                                        <asp:ImageButton ID="ibtSearchAuslieferung" runat="server" 
                                                                    ImageUrl="/Portal/images/lupe2.gif" ToolTip="Adresssuche" />
                                                    </td>
                                                    <td class="style55">
                                                        Ansprechpartner*</td>
                                                    <td>
                                                        <asp:TextBox ID="txtEmpfAPName" runat="server" Width="200px" MaxLength="35"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style33">
                                                        Strasse, Nr.*</td>
                                                    <td class="style2">
                                                        <asp:TextBox ID="txtEmpfStrasse" runat="server" Width="244px" MaxLength="35"></asp:TextBox>
                                                    </td>
                                                    <td class="style55">
                                                        Telefon1*</td>
                                                    <td>
                                                        <asp:TextBox ID="txtEmpfAPTelefon" runat="server" MaxLength="16"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style33">
                                                        PLZ*, Ort*</td>
                                                    <td class="style2">
                                                        <asp:TextBox ID="txtEmpfPLZ" runat="server" Width="60px" MaxLength="10"></asp:TextBox>
&nbsp;<asp:TextBox ID="txtEmpfOrt" runat="server" Width="233px" MaxLength="35"></asp:TextBox>
                                                    </td>
                                                    <td class="style55">
                                                        Telefon2</td>
                                                    <td>
                                                        <asp:TextBox ID="txtEmpfAPTelefon2" runat="server" MaxLength="16"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style33">
                                                        Land*</td>
                                                    <td class="style2">
                                                        <asp:DropDownList ID="drpEmpfLand" runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="style55">
                                                        E-Mail</td>
                                                    <td>
                                                        <asp:TextBox ID="txtEmpfAPMail" runat="server" Width="200px" MaxLength="241"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                </table>
                                            </td>
                                            </tr>
										<tr><td>
                                            &nbsp;</td></tr>
                                            <tr><td>
                                                <table class="style22">
                                                    <tr>
                                                        <td class="style32">
                                                            Datum Auslieferung*</td>
                                                        <td>
                                                        <asp:TextBox ID="txtAuslieferungDatum" runat="server"></asp:TextBox>
                                                        &nbsp;<asp:ImageButton ID="ibtCalAuslieferung" runat="server" 
                                                                ImageUrl="/Portal/images/calendar.jpg" />
                                                        </td>
                                                        
                                                    </tr>
                                                    <tr>
                                                        <td class="style32">
                                                            Terminart</td>
                                                        <td>
                                                        <asp:RadioButtonList ID="rdoAuslieferung" runat="server" 
                                                            RepeatDirection="Horizontal">
                                                            <asp:ListItem Value="0">ab sofort</asp:ListItem>
                                                            <asp:ListItem Value="1">frühestens ab</asp:ListItem>
                                                            <asp:ListItem Value="2">spätestens am</asp:ListItem>
                                                            <asp:ListItem Value="3" Selected="True">Fixtermin</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                        </td>
                                                        
                                                    </tr>
                                                    </table>
                                                </td></tr>
                                                </table>
                                            </asp:Panel>
                                            </td></tr>
                                            <tr><td>&nbsp;&nbsp;</td></tr>
                                            <tr><td class="style41">Dienstleistungsdetails                 <asp:Panel ID="pnl6" runat="server" Width="825px" BorderColor="Black" 
                                                    BorderStyle="Solid" BorderWidth="1px">
                                                <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td class="style34">
                                                                Wagen vollgetankt übergeben</td>
                                                            <td>
                                                                <asp:checkbox id="chkWagenVolltanken" runat="server"></asp:checkbox>
                                                            </td>
                                                            <td class="style57">
                                                                &nbsp;</td>
                                                            <td class="style58">
                                                                <asp:Label ID="lblHandyadapter" runat="server" Text="Handy Adapter"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkHandyadapter" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style34">
                                                                Wagenwäsche</td>
                                                            <td>
                                                                <asp:checkbox id="chkWw" runat="server"></asp:checkbox>
                                                            </td>
                                                            <td class="style57">
                                                                &nbsp;</td>
                                                            <td class="style58">
                                                                <asp:Label ID="lblVerbandskasten" runat="server" Text="Verbandskasten"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkVerbandskasten" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style34">
                                                                Fahrzeugeinweisung</td>
                                                            <td>
                                                                <asp:checkbox id="chkEinweisung" runat="server"></asp:checkbox>
                                                            </td>
                                                            <td class="style57">
                                                                &nbsp;</td>
                                                            <td class="style58">
                                                                <asp:Label ID="lblNaviCD" runat="server" Text="Navi-CD"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkNaviCD" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style34">
                                                                Servicekarte</td>
                                                            <td>
                                                <asp:CheckBox ID="chkServicekarte" runat="server" />
                                                            </td>
                                                            <td class="style57">
                                                                &nbsp;</td>
                                                            <td class="style58">
                                                                <asp:Label ID="lblWarndreieck" runat="server" Text="Warndreieck"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkWarndreieck" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style34">
                                                                <asp:Label ID="lblWarnweste" runat="server" Text="Warnweste"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkWarnweste" runat="server" />
                                                            </td>
                                                            <td class="style57">
                                                                &nbsp;</td>
                                                            <td class="style58">
                                                                <asp:Label ID="lblFussmatten" runat="server" Text="Fußmatten"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkFussmatten" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style34">
                                                Tankkarten</td>
                                                            <td>
                                                <asp:TextBox ID="txtTankkarten" runat="server" Width="188px" MaxLength="132"></asp:TextBox>
                                                            </td>
                                                            <td class="style57">
                                                                &nbsp;</td>
                                                            <td class="style58">
                                                                <asp:Label ID="lblFKontrolle" runat="server" Text="Führerscheinkontrolle"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkFKontrolle" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                </td></tr>
                                                <tr><td>
                                                    
                                                    &nbsp;&nbsp;</td></tr>
                                               <TR>
								<TD style="WIDTH: 418px" vAlign="top">
									<P>
										<asp:CheckBox id="chkWinterreifenHandling" runat="server" Width="374px" 
                                            Font-Bold="True" Text="Winterreifenhandling durch DAD" AutoPostBack="True" 
                                            ForeColor="#009900"></asp:CheckBox><asp:panel id="pnlWinterreifen" 
                                        runat="server" Width="773px" Visible="False" Height="100%" BorderColor="Black" 
                                        BorderStyle="Solid" BorderWidth="1px">
											<TABLE id="Table3" style="WIDTH: 769px; HEIGHT: 166px" cellSpacing="1" cellPadding="1" width="769" border="0">
												<TR>
													<TD style="HEIGHT: 33px"><FONT color="#0000ff">Hinweis: Bei Vorgabe eines Fixtermins 
															kann es zu Problemen bei der Beschaffung von Rädern und Montageterminen kommen!</FONT></TD>
												</TR>
												<TR>
													<TD>
														<asp:RadioButton id="rdoWinterreifenAbWerk" runat="server" Text="Winterräder ab Werk geliefert - montieren lassen" AutoPostBack="True" GroupName="Winterräder" Enabled="False"></asp:RadioButton></TD>
												</TR>
												<TR>
													<TD>
														<asp:RadioButton id="rdoWinterreifenBesorgen" runat="server" Width="341px" Text="Winterräder besorgen + montieren lassen" AutoPostBack="True" GroupName="Winterräder" Enabled="False"></asp:RadioButton></TD>
												</TR>
												<TR>
													<TD noWrap>
														<asp:CheckBox id="chkWinterreifenGroesser" runat="server" Width="387px" Text="Größere Reifendimensionen gewünscht, wenn zugelassen:" Enabled="False"></asp:CheckBox>
														<asp:TextBox id="txtWinterreifenGroesse" runat="server" Width="166px" Enabled="False"></asp:TextBox></TD>
												</TR>
												<TR>
													<TD>
														<asp:Label id="Label1" runat="server" Height="25px">Felgen:</asp:Label>
														<asp:RadioButtonList id="rdlFelgen" runat="server" RepeatDirection="Horizontal" Enabled="False">
															<asp:ListItem Value="Standard" Selected="True">Standard</asp:ListItem>
															<asp:ListItem Value="Zubeh&#246;r-LM">Zubeh&#246;r-LM</asp:ListItem>
															<asp:ListItem Value="Herst.-LM">Herst.-LM</asp:ListItem>
														</asp:RadioButtonList></TD>
												</TR>
												<TR>
													<TD>
														<asp:Label id="Label3" runat="server" Height="25px">Radkappen:</asp:Label>
														<asp:RadioButtonList id="rdlRadkappen" runat="server" RepeatDirection="Horizontal" Enabled="False">
															<asp:ListItem Value="Zubeh&#246;r">Zubeh&#246;r</asp:ListItem>
															<asp:ListItem Value="Hersteller">Hersteller</asp:ListItem>
															<asp:ListItem Value="Keine" Selected="True">Keine</asp:ListItem>
														</asp:RadioButtonList></TD>
												</TR>
												<TR>
													<TD>
														<asp:Label id="Label4" runat="server" Height="25px">Zweiter Radsatz:</asp:Label>
														<asp:RadioButtonList id="rdlZweiterRadsatz" runat="server" RepeatDirection="Horizontal" Enabled="False">
															<asp:ListItem Value="ins Fzg. legen" Selected="True">ins Fzg. legen</asp:ListItem>
															<asp:ListItem Value="am Ziel einlagern">am Ziel einlagern</asp:ListItem>
															<asp:ListItem Value="einlagern - siehe Bemerkung">einlagern - siehe Bemerkung</asp:ListItem>
														</asp:RadioButtonList></TD>
												</TR>
											</TABLE>
										</asp:panel></P>
								</TD>
							</TR>
							<tr><td>
                                <table class="style22">
                                    <tr>
                                        <td colspan="2">
                                            <asp:Panel ID="pnl7" runat="server" BorderColor="Black" BorderStyle="Solid" 
                                                BorderWidth="1px" Width="825px">
                                                <table cellpadding="0" cellspacing="0" class="style22">
                                                    <tr>
                                                        <td class="style53">
                                                            Bemerkung</td>
                                                        <td>
                                                            <asp:TextBox ID="txtBemerkung" runat="server" Height="48px" Width="728px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style37">
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                </table>
                                </td></tr>
                                <tr><td>
                                    <table class="style22">
                                        <tr>
                                            <td class="style37">
                                                &nbsp;</td>
                                            <td class="style39">
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                    </table>
                                    </td></tr>
                                
									</table>
								
								</table>
							
									
									
                                </td>
									
							</tr>
						</table>
			</form>
	</body>
</HTML>
