<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AutohausPortal.Start.Login"
    MasterPageFile="/AutohausPortal/MasterPage/Login.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:CheckBox ID="cbxLogin_TEST" runat="server" Visible="False"></asp:CheckBox>
        <asp:CheckBox ID="cbxLogin_PROD" runat="server" Visible="False"></asp:CheckBox>
        <asp:Literal ID="litAlert" runat="server"></asp:Literal>
    </div>
    <div id="login">
        <h1 class="login">
            Willkommen im Kundenportal</h1>
        <h3 class="login">
            Bitte melden Sie sich an.</h3>
        <!--Loginbereich-->
        <div class="loginbereich">
            <div>
                <h5>
                    <asp:Label ID="lblError" runat="server" Style="color: #B54D4D" Text=""></asp:Label>
                    <%--                                <asp:Label ID="lblMessage" runat="server" Style="color: #269700" Text=""></asp:Label>    --%>
                </h5>
            </div>
            <div class="loginbereich_top">
                &nbsp;
            </div>
            <div class="loginbereich_content">
                <!--formulardaten-->
                <div class="logindaten" id="StandardLogin" runat = "server">
                    <div class="loginpfeil">
                        <img width="42" height="42" alt="" src="/Autohausportal/images/loginpfeil.gif" />
                    </div>
                    <h3>
                        Kunden Login</h3>
                    <div class="trenner10">
                        &nbsp;
                    </div>
                    <div class="linie">
                        &nbsp;
                    </div>
                    <div class="trenner10">
                        &nbsp;
                    </div>
                    <!--formulardaten zeile1-->
                    <div class="blueformname">
                        Benutzername
                    </div>
                    <div class="blueformname" style="width: 42px;">
                        &nbsp;
                    </div>
                    <div class="blueformname">
                        Passwort
                    </div>
                    <div class="trenner5">
                        &nbsp;
                    </div>
                    <!--textinput-->
                    <div class="formfeld" id="formdiv1" style="width: 310px;">
                        <div class="formfeld_start"></div>
                        <asp:TextBox ID="txtUsername" CssClass="formtext" Style="width: 280px;" runat="server"></asp:TextBox>
                        <div class="formfeld_end"></div>
                    </div>
                    <div class="formname" style="width: 34px;">
                        &nbsp;
                    </div>
                    <div class="formfeld" id="formdiv2" style="width: 310px;">
                        <div class="formfeld_start"></div>
                        <asp:TextBox ID="txtPassword" TextMode="Password" Text="Passwort" CssClass="formtext"
                            Style="width: 280px;" runat="server"></asp:TextBox>
                        <div class="formfeld_end"></div>
                    </div>
                    <!--textinput-->
                    <!--formulardaten zeile1-->

                    <div class="trenner10">
                        &nbsp;
                    </div>
                    <div class="formname" style="margin-left: 6px;">
                        <asp:LinkButton ID="lbtnHelpCenter" runat="server" onclick="lbtnHelpCenter_Click"
                                               >Benötigen Sie Hilfe?<br /></asp:LinkButton>
                        <asp:LinkButton ID="lnkPasswortVergessen" Visible="false" runat="server" OnClick="lnkPasswortVergessen_Click">Passwort vergessen?</asp:LinkButton>

                    </div>
                    <asp:Button ID="cmdLogin" runat="server" Text="Login" CssClass="submitbutton" OnClick="btnLogin_Click" />
                    <div class="trenner10">
                        &nbsp;
                    </div>
                </div>
                <!--formulardaten-->

                <div class="loginbereich_bottom" id="divStandardLogin_bottom" runat = "server" style="padding-bottom: 15px">
                    &nbsp;
                </div>
                <div class="logindaten"  id="divKontakt" runat="server" visible="false">
                    <!--formularbereich2-->
                    <div class="formularbereich">
                        <div class="formlayer_plus_top" style="display:block">
                            </div>
                        <div class="formlayer_plus_content">
                            <!--formulardaten-->
                            <div class="formulardaten">
                                <div class="loginpfeil">
                                    <img width="42" height="42" alt="" src="/Autohausportal/images/loginpfeil.gif" />
                                </div>
                                <h3>
                                    Ihre Kontaktdaten</h3>

                                <div class="trenner10">
                                    &nbsp;
                                </div>
                                <div class="linie">
                                    &nbsp;
                                </div>
                                <asp:Label ID="MessageLabel" runat="server" Style="color: #B54D4D" Text=""></asp:Label> 
                                <div class="trenner">
                                    &nbsp;</div>
					            <div class="hinweispflicht" id="hinweispflicht1">
						            *Pflichtfelder</div>
                                <div class="formname">
                                    Anrede*</div>
                                <!--dropdown-->
                                <div class="formbereich" id="divAnrede" runat="server">
                                            <asp:DropDownList ID="ddlAnrede" runat="server" Width="290px">
                                                <asp:ListItem Value="-" Selected="True">Bitte auswählen</asp:ListItem>
                                                <asp:ListItem Value="Herr">Herr</asp:ListItem>
                                                <asp:ListItem Value="Frau">Frau</asp:ListItem>
                                            </asp:DropDownList>
                                </div>
                                <div class="trenner">
                                    &nbsp;</div>
                                <div class="formname">
                                    Benutzername*</div>
                                <!--textinput-->
                                <div class="formfeld" id="divKontoinhaber" runat="server" style="width: 300px;">
                                    <div class="formfeld_start"></div>
                                    <asp:TextBox CssClass="formtext" ID="txtWebUserName" MaxLength="40" runat="server"
                                        Style="width: 270px;"></asp:TextBox>
                                    <div class="formfeld_end"></div>
                                </div>
                                <!--formulardaten zeile1-->
                                <div class="trenner">
                                    &nbsp;</div>
                                <!--formulardaten zeile2-->
                                <div class="formname">
                                    Name*</div>
                                <!--textinput-->
                                <div class="formfeld" id="divBankkonto" runat="server" style="width: 300px;">
                                    <div class="formfeld_start"></div>
                                    <asp:TextBox CssClass="formtext" ID="txtName" MaxLength="18" runat="server"
                                        Style="width: 270px;"></asp:TextBox>
                                    <div class="formfeld_end"></div>
                                </div>
                                <!--textinput-->
                                <!--formulardaten zeile2-->
                                <div class="trenner">
                                    &nbsp;</div>
                                <!--formulardaten zeile3-->
                                <div class="formname">
                                    Vorname*</div>
                                <!--textinput-->
                                <div class="formfeld" id="divBankschluessel" runat="server" style="width: 300px;">
                                    <div class="formfeld_start"></div>
                                    <asp:TextBox CssClass="formtext" ID="txtVorname" MaxLength="15" runat="server"
                                        Style="width: 270px;"></asp:TextBox>
                                    <div class="formfeld_end"></div>
                                </div>
                                <!--textinput-->
                                <!--formulardaten zeile3-->
                                <div class="trenner">
                                    &nbsp;</div>
                                <!--formulardaten zeile3-->
                                <div class="formname">
                                    Firma*</div>
                                <!--textinput-->
                                <div class="formfeld fielddisabled" id="divGeldinstitut" runat="server" style="width: 300px;">
                                    <div class="formfeld_start"></div>
                                    <asp:TextBox CssClass="formtext" ID="txtFirma" MaxLength="40" ReadOnly="false"
                                        runat="server" Style="width: 270px;"></asp:TextBox>
                                    <div class="formfeld_end"></div>
                                </div>
                                <!--textinput-->
                                <!--formulardaten zeile3-->
                                <div class="trenner">
                                    &nbsp;</div>
                                <div class="formname">
                                    Telefon*</div>
                                <!--textinput-->
                                <div class="formfeld" id="div1" runat="server" style="width: 300px;">
                                    <div class="formfeld_start"></div>
                                    <asp:TextBox CssClass="formtext" ID="txtTelefon" MaxLength="15" runat="server" 
                                        Style="width: 270px;"></asp:TextBox>
                                    <div class="formfeld_end"></div>
                                </div>
                                <!--textinput-->
                                <!--formulardaten zeile3-->
                                <div class="trenner">
                                    &nbsp;</div>
                                <!--formulardaten zeile3-->
                                <div class="formname">
                                    Email-Adresse*</div>
                                <!--textinput-->
                                <div class="formfeld fielddisabled" id="div2" runat="server" style="width: 300px;">
                                    <div class="formfeld_start"></div>
                                    <asp:TextBox CssClass="formtext" ID="txtEmail" MaxLength="40" ReadOnly="false" runat="server"
                                        Style="width: 270px;"></asp:TextBox>
                                    <div class="formfeld_end"></div>
                                </div>
                                <!--textinput-->
                                <!--formulardaten zeile3-->
                                <div class="trenner"  id="divProblemTrenner" runat="server" visible="false">
                                    &nbsp;</div>
                                <div class="formname" id="divProblem" runat="server" visible="false">
                                    Frage/Problem:*<br />
                                    max. 250 Zeichen</div>
                                <!--textinput-->
                                <asp:TextBox ID="txtProblem" runat="server" Width="350px" MaxLength="250" TextMode="MultiLine"
                                    Height="100px" Visible="false"></asp:TextBox>
                                <!--textinput-->
                                <!--formulardaten zeile3-->
                                <div class="trenner10">
                                    &nbsp;</div>
                                <!--formulardaten zeile3-->
                                <div class="blueformname" style="width: 62px;">
                                    <img src="" alt="" id="imgCatcha1" runat="server" />&nbsp;
                                </div>
                                <div class="blueformname" style="width: 22px; margin-left: 12px; margin-right: 12px">
                                    <h3>+</h3>&nbsp;
                                </div>
                                <div class="blueformname" style="width: 62px;">
                                    <img src="" alt="" id="imgCatcha2" runat="server" />&nbsp;
                                </div>
                                <div class="blueformname">
                                    <asp:Button ID="cmdRefresh" runat="server" Text="Gleichung neu generieren" 
                                        CssClass="dynbutton" onclick="cmdRefresh_Click" />
                                </div>
                                <!--textinput-->
                                <!--formulardaten zeile3-->
                                <div class="trenner">
                                    &nbsp;</div>
                                <!--formulardaten zeile2-->
                                <div class="formname" style="width: auto;">
                                    Bitte geben Sie das Ergebnis der abgebildeten Gleichung ein!
                                </div>
                                <!--formulardaten zeile2-->
                                <div class="trenner">
                                    &nbsp;
                                </div>
                                <!--formulardaten zeile3 -->
                                <div class="formfeld fielddisabled" id="div3" runat="server" style="width: 200px;">
                                    <div class="formfeld_start"></div>
                                    <asp:TextBox CssClass="formtext" ID="CodeNumberTextBox" MaxLength="40" runat="server"
                                        Style="width: 170px;"></asp:TextBox>
                                    <div class="formfeld_end"></div>
                                </div>
                                <asp:Button ID="cmdSend" runat="server" Text="Senden" CssClass="dynbutton" 
                                    onclick="cmdSend_Click" />
                                <div class="trenner">
                                    &nbsp;
                                </div>
                            </div>
                            <!--formulardaten-->
                            <div class="formlayer_plus_bot">
                                &nbsp;</div>
                        </div>
                    </div>
                </div>
                <div class="loginbereich_bottom"  id="divKontakt_bottom" runat="server" visible="false">
                    &nbsp;
                </div>
                <div class="logindaten" id="divRepeater" runat="server">
                    <div class="formularbereich">
                        <div class="formlayer_plus_top">
                        </div>
                        <div class="formlayer_plus_content">
                            <asp:Repeater ID="Repeater1" runat="server">
                                <ItemTemplate>
                                   
                                    <div class="formbereich" style="margin-right: 5px;">
                                        <%# DataBinder.Eval(Container.DataItem, "titleText") %>
                                    </div>
									<div class="formbereich" style="margin-right: 10px;">
                                      <%# DataBinder.Eval(Container.DataItem, "messageText") %>
									</div>
                                                <div class="trenner">
                                                    &nbsp;
                                                </div>  
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
                <div class="loginbereich_bottom"  id="divRepeater_bottom" runat="server" visible="false">
                    &nbsp;
                </div>
                <div class="logindaten" id="DoubleLogin2" runat="server">
                    <div class="formularbereich">
                        <div class="formlayer_plus_top" style="display: block">
                        </div>
                        <div class="formlayer_plus_content">
                         Bitte verlassen Sie unser Portal immer über den Menüpunkt &quot;Abmelden&quot;.
                                <div class="trenner10">
                                    &nbsp;
                                </div>
                       <div class="formname" style="width : 250px">
                         Wollen Sie mit&nbsp; der&nbsp; Anmeldung fortfahren? </div><div class="formbereich" style="margin-right: 5px;">
                         <asp:Button ID="cmdContinue1" runat="server" Text="Senden" CssClass="dynbutton" 
                                    onclick="cmdContinue_Click" /></div>
                                <div class="trenner">
                                    &nbsp;
                                </div>
                          <div class="formname" style="width : 250px" > Zurück zur Anmeldeseite</div>
                          <div class="formbereich" style="margin-right: 5px;"><asp:Button ID="cmdBack1" runat="server" CssClass="dynbutton" 
                                Text="Zurück" OnClick="cmdBack_Click"></asp:Button></div>
                        </div>
                    </div>
                </div>
                <div class="loginbereich_bottom"  id="divDoubleLogin2_bottom" runat="server" visible="false">
                    &nbsp;
                </div>
        <!--Loginbereich-->
<%--        <div id="thawteseal" style="padding-top: 15px; float: right" title="Click to Verify - This site chose Thawte SSL for secure e-commerce and confidential communications.">
            <div>
                <script type="text/javascript" src="https://seal.thawte.com/getthawteseal?host_name=kundenportal.kroschke.de&amp;size=M&amp;lang=de"></script>
            </div>
            <div>
                <a href="http://www.thawte.de/ssl-certificates/" target="_blank" style="color: #000000;
                    text-decoration: none; font: bold 10px arial,sans-serif; margin: 0px; padding: 0px;">
                    &#220;ber SSL-Zertifikate</a></div>
        </div>--%>
    </div>
        </div>

    </div>
</asp:Content>
