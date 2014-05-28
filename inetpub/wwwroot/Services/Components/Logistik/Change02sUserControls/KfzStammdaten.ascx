<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="KfzStammdaten.ascx.vb"
    Inherits="CKG.Components.Logistik.KfzStammdaten" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Panel runat="server" DefaultButton="dummySearch">
    <asp:Button runat="server" ID="dummySearch" OnClick="OnErgänzenClick" Style="display: none;" />
    <table cellspacing="0" cellpadding="0" width="100%">
        <tr>
            <td class="First" style="padding-left: 7px">
                <asp:Label ID="lbl_Fahrgestellnummer" runat="server">lbl_Fahrgestellnummer</asp:Label>
            </td>
            <td width="40%">
                <asp:TextBox ID="txtFahrgestellnummer" Width="300px" runat="server" MaxLength="20" />
                <asp:RequiredFieldValidator ID="rfvFahrgestellnummer" runat="server" EnableClientScript="false"
                    ControlToValidate="txtFahrgestellnummer" Display="None" SetFocusOnError="true"
                    ErrorMessage="Bitte geben Sie eine Fahrgestellnummer an." />
            </td>
            <td class="First" style="width: 130px">
                <asp:Label ID="lbl_Zugelassen" runat="server">Fahrzeug zugelassen<br />und betriebsbereit?</asp:Label>
            </td>
            <td>
                <span>
                    <asp:RadioButtonList ID="rblZugelassen" runat="server" RepeatDirection="Horizontal"
                        RepeatLayout="Flow">
                        <asp:ListItem Value="J">Ja</asp:ListItem>
                        <asp:ListItem Value="N" style="padding-left: 36px">Nein</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="rfvZugelassen" runat="server" EnableClientScript="false"
                        ControlToValidate="rblZugelassen" Display="None" SetFocusOnError="true" ErrorMessage="Bitte geben Sie an, ob Fahrzeug zugelassen ist." />
                </span>
            </td>
        </tr>
        <tr>
            <td class="First" style="padding-left: 7px">
                <asp:Label ID="lbl_Kennzeichen" runat="server">lbl_Kennzeichen</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtKennzeichen1" Width="300px" runat="server" MaxLength="20" />
                <cc1:TextBoxWatermarkExtender ID="txtKennzeichen1_TextBoxWatermarkExtender" runat="server"
                    Enabled="True" TargetControlID="txtKennzeichen1" WatermarkCssClass="Watermarked"
                    WatermarkText="z.B. HH-PT664" />
            </td>
            <td class="First" style="width: 120px">
                <asp:Label ID="Label7" runat="server">Zulassung an DAD beauftragt?</asp:Label>
            </td>
            <td>
                <span>
                    <asp:RadioButtonList ID="rblBeauftragt" runat="server" RepeatDirection="Horizontal"
                        RepeatLayout="Flow">
                        <asp:ListItem Value="J">Ja</asp:ListItem>
                        <asp:ListItem Value="N" style="padding-left: 36px">Nein</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="rfvBeauftragt" runat="server" EnableClientScript="false"
                        ControlToValidate="rblBeauftragt" Display="None" SetFocusOnError="true" ErrorMessage="Bitte geben Sie an, ob Zulassung an DAD beauftragt ist." />
                </span>
            </td>
        </tr>
        <tr>
            <td class="First" style="padding-left: 7px; height: 32px;">
                <asp:Label ID="lbl_Typ" runat="server">lbl_Typ</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtTyp" runat="server" Width="300px" MaxLength="25" />
                <cc1:TextBoxWatermarkExtender ID="txtTyp_TextBoxWatermarkExtender" runat="server"
                    Enabled="True" TargetControlID="txtTyp" WatermarkCssClass="Watermarked" WatermarkText="z.B. Audi A4" />
                <asp:RequiredFieldValidator ID="rfvTyp" runat="server" EnableClientScript="false"
                    ControlToValidate="txtTyp" Display="None" SetFocusOnError="true" ErrorMessage="Bitte geben Sie einen Typ an." />
            </td>
            <td>
                <asp:Label ID="lbl_Bereifung" runat="server" Style="padding-right: 33px" Font-Bold="True">Bereifung: </asp:Label>
            </td>
            <td>
                <asp:RadioButtonList ID="rblBereifung" runat="server" RepeatDirection="Horizontal"
                    RepeatLayout="Flow">
                    <asp:ListItem Value="S">Sommer</asp:ListItem>
                    <asp:ListItem Value="W" style="padding-left: 2px">Winter</asp:ListItem>
                    <asp:ListItem Value="G" style="padding-left: 15px">GJ</asp:ListItem>
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="rfvBereifung" runat="server" EnableClientScript="false"
                    ControlToValidate="rblBereifung" Display="None" SetFocusOnError="true" ErrorMessage="Bitte geben Sie die Bereifung an." />
            </td>
        </tr>
        <tr>
            <td class="First" style="padding-left: 7px">
                <asp:Label ID="lbl_Referenznummer" runat="server">lbl_Referenznummer</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtReferenznummer" runat="server" Width="300px" MaxLength="20" />&nbsp;
                <asp:ImageButton ID="ImageButton10" Style="padding-right: 0px; padding-bottom: 4px"
                    ToolTip="Hier können Sie Ihre interne Nummer für diesen Auftrag / Vorgang eingeben."
                    runat="server" ImageUrl="/Services/Images/fragezeichen.gif" />
            </td>
            <td>
                <asp:Label ID="lbl_Fahrzeugklasse" runat="server" Font-Bold="True" Style="padding-right: 17px">Fahrzeugklasse: </asp:Label>
            </td>
            <td>
                <asp:RadioButtonList ID="rblFahrzeugklasse" runat="server" RepeatDirection="Horizontal"
                    RepeatLayout="Flow">
                    <asp:ListItem Value="PKW">&lt; 3,5</asp:ListItem>
                    <asp:ListItem Value="PK1" style="padding-left: 20px">3,5 - 7,5</asp:ListItem>
                    <asp:ListItem Value="LKW" style="padding-left: 2px">&gt; 7,5</asp:ListItem>
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="rfvFahrzeugklasse" runat="server" EnableClientScript="false"
                    ControlToValidate="rblFahrzeugklasse" Display="None" SetFocusOnError="true" ErrorMessage="Bitte geben Sie die Fahrzeugklasse an." />
            </td>
        </tr>
        <tr>
            <td class="First" style="padding-left: 7px; height: 30px;">
                <asp:Label ID="lbl_Fahrzeugwert" runat="server">lbl_Fahrzeugwert </asp:Label>
            </td>
            <td>
                <div id="divFahrzeugwert" runat="server" style="width: 303px">
                    <asp:DropDownList ID="drpFahrzeugwert" runat="server" Width="303px">
                        <asp:ListItem Value="" Text="Bitte auswählen" />
                        <asp:ListItem Value="Z00" Text="&hellip;bis  50  Tsd. €" Selected="True" />
                        <asp:ListItem Value="Z50" Text="&hellip;bis 150  Tsd. €" />
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvFahrzeugwert" runat="server" EnableClientScript="false"
                        ControlToValidate="drpFahrzeugwert" Display="None" SetFocusOnError="true" ErrorMessage="Bitte wählen Sie den Fahrzeugwert." />
                </div>
            </td>
            <td class="First" style="height: 30px">
                <asp:LinkButton ID="lbErgänzen" runat="server" CssClass="greyButton fillin" Text="Daten ergänzen"
                    OnClick="OnErgänzenClick" />
            </td>
            <td>
            </td>
        </tr>
    </table>
</asp:Panel>
