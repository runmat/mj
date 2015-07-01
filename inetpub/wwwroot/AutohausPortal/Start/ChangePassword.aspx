<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs"
    Inherits="AutohausPortal.Start.ChangePassword" MasterPageFile="/AutohausPortal/MasterPage/Selection.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../Scripts/PasswordStrength.js"></script>
    <div>
        <asp:CheckBox ID="cbxLogin_TEST" runat="server" Visible="False"></asp:CheckBox>
        <asp:CheckBox ID="cbxLogin_PROD" runat="server" Visible="False"></asp:CheckBox>
        <asp:Literal ID="litAlert" runat="server"></asp:Literal>
    </div>
    <div class="formulare">
        <!-- WICHTIG:
			Für den IE6 ist es leider notwendig, sowohl die Breite der Formfelder als auch die der umgebenden Layer zu definieren.
			Dabei muß der umgebende Layer immer 30 Pixel breiter sein, als das umschließende Formfeld.
			Wenn ein Input-Feld mit einem eingelagerten Button benötigt wird (z.B. ein Datepicker), muß dieser Wert nochmals um 
			25 erhöht werden.
			-->
        <!-- FORMULARLAYER1 -->
        <div style="margin-left: 65px">
            <h3>
                <asp:Label ID="lblError" runat="server" Style="color: #B54D4D" Text=""></asp:Label>
                <asp:Label ID="lblMessage" runat="server" Style="color: #269700" Text=""></asp:Label>
            </h3>
        </div>
        <div class="formlayer" id="StandardLogin" runat="server">
            <div class="formlayer_top">
                <div class="formopener" id="formopener1" onclick="openforms(1);">
                    <img src="../images/loginpfeil.gif" width="42" height="42" alt="" /></div>
                <h2>
                    <asp:Label ID="lblPwdExp" runat="server" Text="Ihr Passwort ist abgelaufen. Bitte ändern Sie Ihr Passwort !"></asp:Label>
                    <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h2>
            </div>
            <div class="formlayer_plus" style="display: block">
                <!--formularbereich1-->
                <div class="formularbereich">
                    <div class="formlayer_plus_top">
                        &nbsp;</div>
                    <div class="formlayer_plus_content">
                        <!--formulardaten-->
                        <div class="formulardaten">
                            <div class="formbereich">
                                Hier erhalten Sie eine Hilfestellung, wie Sie Ihr Kennwort korrekt ändern!<br />
                                <br />
                                Folgende Regeln gelten für Ihren Zugang:<br />
                                <br />
                                <asp:Label ID="lblLength" runat="server" Text="Label"></asp:Label><br />
                                <asp:Label ID="lblSpecial" runat="server" Text="Label"></asp:Label><br />
                                <asp:Label ID="lblUpperCase" runat="server" Text="Label"></asp:Label><br />
                                <asp:Label ID="lblNumeric" runat="server" Text="Label"></asp:Label><br />
                            </div>
                        </div>
                        <div class="formlayer_plus_bot">
                            &nbsp;</div>
                    </div>
                    <div class="trenner10">
                        &nbsp;</div>
                </div>
            </div>
        </div>
        <div class="formlayer" id="StandardLogin2" runat="server">
            <div class="formlayer_plus" id="form3" style="display: block">
                <!--formularbereich1-->
                <div class="formularbereich">
                    <div class="formlayer_plus_top">
                        &nbsp;</div>
                    <div class="formlayer_plus_content">
                        <!--formulardaten-->
                        <div class="formulardaten">
                            <!--formulardaten zeile1-->
                            <div class="formname">
                                altes Passwort:</div>
                            <!--textinput-->
                            <div class="formfeld" id="divName1" runat="server" style="width: 300px;">
                                <div class="formfeld_start"></div>
                                <asp:TextBox CssClass="formtext" ID="txtOldPwd" MaxLength="40" runat="server" Style="width: 270px;"
                                    TextMode="Password"></asp:TextBox>
                                <div class="formfeld_end"></div>
                            </div>
                            <!--textinput-->
                            <div class="formname" id="spanUCase" style="color: #B54D4D; padding-top: 0px">
                                Großbuchstaben:</div>
                            <div class="formname" id="divUCase" style="color: #B54D4D; padding-top: 0px">
                                nicht erfüllt</div>
                            <!--formulardaten zeile1-->
                            <div class="trenner">
                                &nbsp;</div>
                            <!--formulardaten zeile2-->
                            <div class="formname">
                                &nbsp;neues Passwort</div>
                            <!--textinput-->
                            <div class="formfeld" id="divName2" runat="server" style="width: 300px;">
                                <div class="formfeld_start"></div>
                                <asp:TextBox CssClass="formtext" ID="txtNewPwd" MaxLength="40" runat="server" Style="width: 270px;"
                                    TextMode="Password"></asp:TextBox>
                                <div class="formfeld_end"></div>
                            </div>
                            <!--textinput-->
                            <div class="formname" id="spanNumeric" style="color: #B54D4D; padding-top: 0px">
                                Zahl:</div>
                            <div class="formname" id="divNumeric" style="color: #B54D4D; padding-top: 0px">
                                nicht erfüllt</div>
                            <!--formulardaten zeile2-->
                            <div class="trenner">
                                &nbsp;</div>
                            <!--formulardaten zeile3-->
                            <div class="formname">
                                Passwortbestätigung</div>
                            <!--textinput-->
                            <div class="formfeld" id="divStrasse" runat="server" style="width: 300px;">
                                <div class="formfeld_start"></div>
                                <asp:TextBox CssClass="formtext" ID="txtNewPwdConfirm" MaxLength="40" runat="server"
                                    Style="width: 270px;" TextMode="Password"></asp:TextBox>
                                <div class="formfeld_end"></div>
                            </div>
                            <!--textinput-->
                            <div class="formname" id="spanSpecial" style="color: #B54D4D; padding-top: 0px">
                                Sonderzeichen:</div>
                            <div class="formname" id="divSpecial" style="color: #B54D4D; padding-top: 0px">
                                nicht erfüllt</div>
                            <div id="spanLength" class="formname" style="color: #B54D4D; padding-top: 12px">
                                Länge:</div>
                            <div class="formname" id="divLength" style="color: #B54D4D; padding-top: 12px">
                                nicht erfüllt</div>
                            <!--formulardaten zeile3-->
                            <!--formulardaten zeile2-->
                            <div class="trenner">
                                &nbsp;</div>
                        </div>
                        <!--formulardaten-->
                        <div class="formlayer_plus_bot">
                            &nbsp;</div>
                    </div>
                </div>
                <!--formularbereich1-->
                <div class="trenner10">
                    &nbsp;
                </div>
            </div>
            <!-- FORMULARLAYER3 -->
            <!--formbuttons-->
            <div class="formbuttons">
                <asp:Button ID="cmdShowQuestion" runat="server" CssClass="submitbutton" Text="geheime Frage"
                    OnClick="lnkShowQuestion_Click" />
                <asp:Button ID="cmdSave" runat="server" CssClass="submitbutton" Text="Speichern"
                    OnClick="btnChange_Click" />
                <asp:Button ID="cmdCancel" runat="server" CssClass="button" Text="Abbrechen" OnClick="cmdCancel_Click" />
            </div>
            <!--formbuttons-->
        </div>
        <div class="formlayer" id="RequestPassword" runat="server">
            <div class="formlayer_top">
                <div class="formopener" id="Div2">
                    <img src="../images/loginpfeil.gif" width="42" height="42" alt="" /></div>
                <h2>
                    <asp:Label ID="Label4" runat="server" Text="Label"></asp:Label></h2>
            </div>
            <div class="formlayer_plus" style="display: block">
                <!--formularbereich1-->
                <!--formularbereich1-->
                <div class="formularbereich">
                    <div class="formlayer_plus_top">
                        &nbsp;</div>
                    <div class="formlayer_plus_content">
                        <!--formulardaten-->
                        <div class="formulardaten">
                            <div class="formbereich">
                                An dieser Stelle können Sie ein neues Passwort anfordern bzw. Ihren Benutzer entsperren.
                                <br />
                                Beantworten Sie zunächst die von Ihnen gewählte Frage.
                            </div>
                            <div class="trenner10">
                                &nbsp;</div>
                            <div class="formname">
                                Frage:
                            </div>
                            <!--textinput-->
                            <div class="formname" style="width : auto">
                                <strong>
                                    <asp:Label ID="lblFrage" runat="server"></asp:Label></strong>
                            </div>
                            <div class="trenner">
                                &nbsp;</div>
                            <div class="formname">
                                Antwort:
                            </div>
                            <!--textinput-->
                            <div class="formfeld" id="div1" runat="server" style="width: 300px;">
                                <div class="formfeld_start"></div>
                                <asp:TextBox CssClass="formtext" ID="txtAntwortAnforderung" MaxLength="150" runat="server"
                                    Style="width: 270px;"></asp:TextBox>
                                <div class="formfeld_end"></div>
                            </div>
                            <div class="trenner">
                                &nbsp;</div>
                        </div>
                        <div class="formlayer_plus_bot">
                            &nbsp;</div>
                    </div>
                    <div class="trenner10">
                        &nbsp;</div>
                </div>
            </div>
            <!-- FORMULARLAYER3 -->
            <!--formbuttons-->
            <div class="formbuttons">
                <asp:Button ID="cmdLogout" runat="server" CssClass="submitbutton" Text="Erneut anmelden"
                   visible ="false" onclick="cmdLogout_Click"  />
                <asp:Button ID="cmdRequest" runat="server" CssClass="submitbutton" Text="Anfordern"
                    OnClick="lnkRequest_Click" />
            </div>
        </div>
        <div class="formlayer" id="RequestQuestion" runat="server">
            <div class="formlayer_top">
                <div class="formopener" id="Div4">
                    <img src="../images/loginpfeil.gif" width="42" height="42" alt="" /></div>
                <h2>
                    <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label></h2>
            </div>
            <div class="formlayer_plus" style="display: block">
                <!--formularbereich1-->
                <!--formularbereich1-->
                <div class="formularbereich">
                    <div class="formlayer_plus_top">
                        &nbsp;</div>
                    <div class="formlayer_plus_content">
                        <!--formulardaten-->
                        <div class="formulardaten">
                            <div class="formbereich">
                                An dieser Stelle können Sie ein neues Passwort anfordern bzw. Ihren Benutzer entsperren.
                                <br />
                                Beantworten Sie zunächst die von Ihnen gewählte Frage.
                            </div>
                            <div class="trenner10">
                                &nbsp;</div>
                            <div class="formname">
                                Frage:
                            </div>
                            <!--textinput-->
                            <div class="formbereich">

                                     <asp:DropDownList ID="ddlFrage" runat="server" Width="300px" >
                                    </asp:DropDownList>
                            </div>
                            <div class="trenner">
                                &nbsp;</div>
                            <div class="formname">
                                Antwort:
                            </div>
                            <!--textinput-->
                            <div class="formfeld" id="div5" runat="server" style="width: 300px;">
                                <div class="formfeld_start"></div>
                                <asp:TextBox CssClass="formtext" ID="txtAnfordernSpeichern" MaxLength="150" runat="server"
                                    Style="width: 270px;"></asp:TextBox>
                                <div class="formfeld_end"></div>
                            </div>
                            <div class="trenner">
                                &nbsp;</div>
                        </div>
                        <div class="formlayer_plus_bot">
                            &nbsp;</div>
                    </div>
                    <div class="trenner10">
                        &nbsp;</div>
                </div>
            </div>
            <!-- FORMULARLAYER3 -->
            <!--formbuttons-->
            <div class="formbuttons">
                <asp:Button ID="cmdSetzeFrageAntwort" runat="server" CssClass="submitbutton" Text="Speichern"
                    OnClick="cmdSetzeFrageAntwort_Click" />
                <asp:Button ID="cmdShowPassword" runat="server" CssClass="button" Text="Abbrechen" OnClick="lnkShowPassword_Click" />
            </div>
        </div>
    </div>
    <div id="innerContent">
        <div id="innerContentRight" style="width: 100%">
            <div id="innerContentRightHeading">
                <h1>
                    &nbsp;
                </h1>
            </div>
            <div id="pagination">
                <table cellpadding="0" cellspacing="0" style="height: 24px">
                    <tbody>
                        <tr>
                            <td class="login">
                                &nbsp;
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
