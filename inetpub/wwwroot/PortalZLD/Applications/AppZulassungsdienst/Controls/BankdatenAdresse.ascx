<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BankdatenAdresse.ascx.cs" Inherits="AppZulassungsdienst.Controls.BankdatenAdresse" %>

<table cellpadding="0" runat="server" id="TableBank" cellspacing="0">
    <tr>
        <td colspan="3" style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
        </td>
    </tr>
    <tr class="formquery" id="trError" runat="server">
        <td colspan="3" class="firstLeft active">
            <asp:Label ID="lblErrorBank" runat="server" CssClass="TextError" ForeColor="#BC2B2B"></asp:Label>
        </td>
    </tr>
    <tr class="formquery">
        <td class="firstLeft active">
            <asp:Label ID="lblZulDateBank" runat="server">Datum der Zulassung:</asp:Label>
        </td>
        <td class="firstLeft active" colspan="2">
            <asp:TextBox ID="txtZulDateBank" Enabled="false" runat="server" CssClass="TextBoxNormal"
                Width="65px"></asp:TextBox>
        </td>
    </tr>
    <tr class="formquery">
        <td class="firstLeft active">
            <asp:Label ID="Label3" runat="server">Kunde*:</asp:Label>
        </td>
        <td class="firstLeft active" style="height: 30px">
            <asp:TextBox ID="txtKundeBankSuche" onfocus="javascript:this.select();" Enabled="false"
                runat="server" CssClass="TextBoxNormal" MaxLength="8" Width="65px"></asp:TextBox>
        </td>
        <td class="firstLeft active" style="width: 100%; height: 30px;">
            <asp:TextBox ID="txtKundebank" runat="server" CssClass="TextBoxNormal" Width="400px"></asp:TextBox>
        </td>
    </tr>
    <tr class="formquery">
        <td class="firstLeft active">
            <asp:Label ID="lblReferenzen" runat="server">Referenzen:</asp:Label>
        </td>
        <td class="firstLeft active" colspan="2">
            <asp:TextBox ID="txtRef1Bank" Enabled="false" runat="server" CssClass="TextBoxNormal"
                MaxLength="20"></asp:TextBox>
            &nbsp;<asp:TextBox ID="txtRef2Bank" Enabled="false" runat="server" CssClass="TextBoxNormal"
                MaxLength="20" />
        </td>
    </tr>
    <tr>
        <td colspan="3" class="firstLeft active" style="background-color: #dfdfdf; height: 22px;
            padding-left: 15px">
            Abweichende Anschrift
        </td>
    </tr>
    <tr class="formquery">
        <td class="firstLeft active">
            <asp:Label ID="lblName" runat="server">Name:</asp:Label>
        </td>
        <td class="firstLeft active" colspan="2" style="width: 100%;">
            <asp:TextBox ID="txtName1" Width="400px" CssClass="TextBoxNormal" runat="server"
                MaxLength="40"></asp:TextBox>
        </td>
    </tr>
    <tr class="formquery">
        <td class="firstLeft active">
            &nbsp;
        </td>
        <td class="firstLeft active" colspan="2" style="width: 100%; height: 30px;">
            <asp:TextBox ID="txtName2" Width="400px" CssClass="TextBoxNormal" runat="server"
                MaxLength="40"></asp:TextBox>
        </td>
    </tr>
    <tr class="formquery">
        <td class="firstLeft active">
            <asp:Label ID="lblStrasse" runat="server">Straße:</asp:Label>
        </td>
        <td class="firstLeft active" colspan="2" style="width: 100%;">
            <asp:TextBox ID="txtStrasse" Width="400px" CssClass="TextBoxNormal" runat="server"
                MaxLength="60"></asp:TextBox>
        </td>
    </tr>
    <tr class="formquery">
        <td class="firstLeft active">
            <asp:Label ID="lblPLZBank" runat="server">PLZ/Ort*:</asp:Label>
        </td>
        <td class="firstLeft active">
            <asp:TextBox ID="txtPlz" runat="server"  onKeyPress="return numbersonly(event, false)" CssClass="TextBoxNormal" MaxLength="10" Width="65px"></asp:TextBox>
        </td>
        <td class="firstLeft active" style="width: 100%;">
            <asp:TextBox ID="txtOrt" Width="400px" CssClass="TextBoxNormal" runat="server" MaxLength="40"></asp:TextBox>
        </td>
    </tr>
    <tr class="formquery">
        <td class="firstLeft active">
        </td>
        <td class="firstLeft active" colspan="2" style="width: 100%;">
                <asp:CheckBox ID="chkEinzug" runat="server" Text="Einzugsermächtigung" />
                <asp:CheckBox ID="chkRechnung" runat="server" Text="Rechnung" />
        </td>
    </tr>
    <tr class="formquery">
        <td class="firstLeft active" colspan="3">
            &nbsp;
        </td>
    </tr>
    <tr>
        <td colspan="3" class="firstLeft active" style="background-color: #dfdfdf; height: 22px;
            padding-left: 15px">
            Bankverbindung
        </td>
    </tr>
    <tr class="formquery">
        <td class="firstLeft active">
            <asp:Label ID="lblKontoinhaber" runat="server">Kontoinhaber:</asp:Label>
        </td>
        <td class="firstLeft active" colspan="2" style="width: 100%;">
            <asp:TextBox ID="txtKontoinhaber" Width="400px" CssClass="TextBoxNormal" runat="server"
                MaxLength="40"></asp:TextBox>
        </td>
    </tr>
    <tr class="formquery">
        <td class="firstLeft active">
            <asp:Label ID="lblIBAN" runat="server">IBAN:</asp:Label>
        </td>
        <td class="firstLeft active" colspan="2" style="width: 100%;">
            <asp:TextBox ID="txtIBAN" Width="400px" CssClass="TextBoxNormal TextUpperCase" runat="server"
                MaxLength="34"></asp:TextBox>
        </td>
    </tr>
    <tr class="formquery">
        <td class="firstLeft active">
            <asp:Label ID="lblSWIFT" runat="server">SWIFT-BIC:</asp:Label>
        </td>
        <td class="firstLeft active" colspan="2" style="width: 100%;">
            <asp:TextBox ID="txtSWIFT" Width="400px" CssClass="TextBoxNormal" runat="server"
                MaxLength="11" Enabled="false"></asp:TextBox>
        </td>
    </tr>
    <tr class="formquery">
        <td class="firstLeft active">
            <asp:Label ID="lblGeldinstitut" runat="server">Geldinstitut:</asp:Label>
        </td>
        <td class="firstLeft active" colspan="2" style="width: 100%;">
            <asp:TextBox ID="txtGeldinstitut" Width="400px" CssClass="TextBoxNormal" runat="server"
                MaxLength="40" Text="Wird automatisch gefüllt!" Enabled="False"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:HiddenField runat="server" ID="ihBankleitzahl"/>
            <asp:HiddenField runat="server" ID="ihKontonummer"/>
            <asp:HiddenField runat="server" ID="ihLand"/>
        </td>
    </tr>
</table>
