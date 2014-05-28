<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BankdatenAdresse.ascx.cs" Inherits="AutohausPortal.Controls.BankdatenAdresse" %>

<!--formularbereich1-->
<div class="formularbereich">
    <div class="formlayer_plus_top">&nbsp;</div>
    <div class="formlayer_plus_content">
        <!--formulardaten-->
        <div class="formulardaten">
            <h3>Abweichende Anschrift</h3>
            <!--formulardaten zeile1-->
            <div class="formname">Name</div>
            <!--textinput-->
            <div class="formfeld" id="divName1" runat="server" style="width: 500px;">
                <div class="formfeld_start"></div>
                <asp:TextBox CssClass="formtext" ID="txtName1" MaxLength="40" runat="server" Style="width: 470px;"></asp:TextBox>
                <div class="formfeld_end"></div>
            </div>
            <!--textinput-->
            <div class="helpbutton">
                <div class="helplayer" id="helplayer10">
                    <p>
                        Sollen die Kennzeichen & Zulassungs- unterlagen an eine abweichende Adresse versendet werden, können Sie diese hier eintragen. 
                        Sie können hier auch wählen, ob eine Rechnung erstellt oder per Einzugsermächtigung gezahlt werden soll.
                    </p>
                </div>
                <img src="../images/button_help.gif" width="27" height="26" alt="Hilfe" class="helpicon" />
            </div>
            <!--formulardaten zeile1-->
            <div class="trenner">&nbsp;</div>
            <!--formulardaten zeile2-->
            <div class="formname">&nbsp;</div>
            <!--textinput-->
            <div class="formfeld" id="divName2" runat="server" style="width: 500px;">
                <div class="formfeld_start"></div>
                <asp:TextBox CssClass="formtext" ID="txtName2" MaxLength="40" runat="server" Style="width: 470px;"></asp:TextBox>
                <div class="formfeld_end"></div>
            </div>
            <!--textinput-->
            <!--formulardaten zeile2-->
            <div class="trenner">&nbsp;</div>
            <!--formulardaten zeile3-->
            <div class="formname">Straße</div>
            <!--textinput-->
            <div class="formfeld" id="divStrasse" runat="server" style="width: 500px;">
                <div class="formfeld_start"></div>
                <asp:TextBox CssClass="formtext" ID="txtStrasse" MaxLength="60" runat="server" Style="width: 470px;"></asp:TextBox>
                <div class="formfeld_end"></div>
            </div>
            <!--textinput-->
            <!--formulardaten zeile3-->
            <div class="trenner">&nbsp;</div>
            <!--formulardaten zeile4-->
            <div class="formname">PLZ/Ort</div>
            <!--textinput-->
            <div class="formfeld" id="divPLZ" runat="server" style="width: 100px;">
                <div class="formfeld_start"></div>
                <asp:TextBox CssClass="formtext" onKeyPress="return numbersonly(event, false)" ID="txtPlz" MaxLength="5" runat="server" Style="width: 70px;"></asp:TextBox>
                <div class="formfeld_end"></div>
            </div>
            <!--textinput-->
            <!--textinput-->
            <div class="formfeld" id="divOrt" runat="server" style="width: 400px;">
                <div class="formfeld_start"></div>
                <asp:TextBox CssClass="formtext" ID="txtOrt" MaxLength="40" runat="server" Style="width: 370px;"></asp:TextBox>
                <div class="formfeld_end"></div>
            </div>
            <!--textinput-->
            <!--formulardaten zeile4-->
            <div class="trenner">&nbsp;</div>
            <!--formulardaten zeile5-->
            <div class="formname">&nbsp;</div>
            <div class="formselects" id="divZahlungsart" runat="server" style="float: none">
                <asp:RadioButton ID="rbEinzug" GroupName="Zahlungsart" runat="server" /><div class="radiolabel">
                    Einzugsermächtigung</div>
                <asp:RadioButton ID="rbRechnung" GroupName="Zahlungsart" runat="server" /><div class="radiolabel">
                    Rechnung</div>
                <asp:RadioButton ID="rbBar" GroupName="Zahlungsart" runat="server" /><div class="radiolabel">
                    Bar</div>
            </div>
            <!--formulardaten zeile2-->
            <div class="trenner">&nbsp;</div>
        </div>
        <!--formulardaten-->
        <div class="formlayer_plus_bot">&nbsp;</div>
    </div>
</div>
<!--formularbereich1-->
<!--formularbereich2-->
<div class="formularbereich">
    <div class="formlayer_plus_top">&nbsp;</div>
    <div class="formlayer_plus_content">
        <!--formulardaten-->
        <div class="formulardaten">
            <h3>Bankdaten</h3>
            <!--formulardaten zeile1-->
            <div class="formname">Kontoinhaber</div>
            <!--textinput-->
            <div class="formfeld" id="divKontoinhaber" runat="server" style="width: 500px;">
                <div class="formfeld_start"></div>
                <asp:TextBox CssClass="formtext" ID="txtKontoinhaber" MaxLength="40" runat="server"
                    Style="width: 470px;"></asp:TextBox>
                <div class="formfeld_end"></div>
            </div>
            <!--textinput-->
            <div class="helpbutton">
                <div class="helplayer" id="helplayer11">
                    <p>
                        Tragen Sie hier die Bankdaten des Kunden ein, wenn wir die Rechnungsstellung an den Kunden übernehmen sollen und der Kunde mit Einzugsermächtigung zahlt.
                    </p>
                </div>
                <img src="../images/button_help.gif" width="27" height="26" alt="Hilfe" class="helpicon" />
            </div>
            <!--formulardaten zeile1-->
            <div class="trenner">&nbsp;</div>
            <!--formulardaten zeile2-->
            <div class="formname">IBAN</div>
            <!--textinput-->
            <div class="formfeld" id="divIBAN" runat="server" style="width: 500px;">
                <div class="formfeld_start"></div>
                <asp:TextBox CssClass="formtext" ID="txtIBAN" MaxLength="34" runat="server"
                    Style="width: 470px;"></asp:TextBox>
                <div class="formfeld_end"></div>
            </div>
            <!--textinput-->
            <!--formulardaten zeile2-->
            <div class="trenner">&nbsp;</div>
            <!--formulardaten zeile3-->
            <div class="formname">SWIFT-BIC</div>
            <!--textinput-->
            <div class="formfeld fielddisabled" id="divSWIFT" runat="server" style="width: 500px;">
                <div class="formfeld_start"></div>
                <%--Die SWIFT ist zwar nur 11-stellig, aber der Text "Wird automatisch gefüllt!" muss im Feld angezeigt werden können--%>
                <asp:TextBox CssClass="formtext" ID="txtSWIFT" MaxLength="40" Text="Wird automatisch gefüllt!" 
                    runat="server" Style="width: 470px;" Enabled="False"></asp:TextBox>
                <div class="formfeld_end"></div>
            </div>
            <!--textinput-->
            <!--formulardaten zeile3-->
            <div class="trenner">&nbsp;</div>
            <!--formulardaten zeile3-->
            <div class="formname">Geldinstitut</div>
            <!--textinput-->
            <div class="formfeld fielddisabled" id="divGeldinstitut" runat="server" style="width: 500px;">
                <div class="formfeld_start"></div>
                <asp:TextBox CssClass="formtext" ID="txtGeldinstitut" MaxLength="40" Text="Wird automatisch gefüllt!"
                    runat="server" Style="width: 470px;" Enabled="False"></asp:TextBox>
                <div class="formfeld_end"></div>
            </div>
            <!--textinput-->
            <!--formulardaten zeile3-->
            <div class="trenner">&nbsp;</div>
        </div>
        <!--formulardaten-->
        <div class="formlayer_plus_bot">&nbsp;</div>
    </div>
</div>
<!--formularbereich2-->
<div class="trenner10">&nbsp;</div>