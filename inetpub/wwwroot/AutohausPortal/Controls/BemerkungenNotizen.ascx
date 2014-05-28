<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BemerkungenNotizen.ascx.cs" Inherits="AutohausPortal.Controls.BemerkungenNotizen" %>

<!--formularbereich1-->
<div class="formularbereich">
    <div class="formlayer_plus_top">&nbsp;</div>
    <div class="formlayer_plus_content">
        <!--formulardaten-->
        <div class="formulardaten">
            <h3>
                Bemerkungen und Notizen
            </h3>
            <!--formulardaten zeile1-->
            <div class="formname">Bemerkung für Auftrag</div>
            <!--textinput-->
            <div class="formfeld" id="formfeldkunde2" style="width: 500px;">
                <div class="formfeld_start"></div>
                <asp:TextBox CssClass="formtext" ID="txtBemerk" MaxLength="120" runat="server" Style="width: 470px;"></asp:TextBox>
                <div class="formfeld_end"></div>
            </div>
            <!--textinput-->
            <div class="helpbutton">
                <div class="helplayer" id="helplayer9">
                    <p>Hier können Sie weitergehende Informationen für den ausführenden Zulassungsdienst eintragen.</p>
                </div>
                <img src="../images/button_help.gif" width="27" height="26" alt="Hilfe" class="helpicon" />
            </div>
            <!--formulardaten zeile1-->
            <div class="trenner">&nbsp;</div>
            <!--formulardaten zeile2-->
            <div class="formname">Verkäuferkürzel für Auftrag und intern</div>
            <!--textinput-->
            <div class="formfeld" style="width: 100px;">
                <div class="formfeld_start"></div>
                <asp:TextBox CssClass="formtext" ID="txtVKkurz" MaxLength="10" runat="server" Style="width: 70px;"></asp:TextBox>
                <div class="formfeld_end"></div>
            </div>
            <!--textinput-->
            <!--textinput-->
            <div class="formfeld" style="width: 400px;">
                <div class="formfeld_start"></div>
                <asp:TextBox CssClass="formtext" ID="txtKunRef" MaxLength="100" runat="server" Style="width: 370px;"
                    Text="Kundeninterne Referenz"></asp:TextBox>
                <div class="formfeld_end"></div>
            </div>
            <!--textinput-->
            <!--formulardaten zeile2-->
            <div class="trenner">&nbsp;</div>
            <!--formulardaten zeile3-->
            <div class="formname">Notiz für intern</div>
            <!--textinput-->
            <div class="formfeld" style="width: 500px;">
                <div class="formfeld_start"></div>
                <asp:TextBox CssClass="formtext" ID="txtNotiz" MaxLength="100" runat="server" Style="width: 470px;"></asp:TextBox>
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
<!--formularbereich1-->
<div class="trenner10">&nbsp;</div>
