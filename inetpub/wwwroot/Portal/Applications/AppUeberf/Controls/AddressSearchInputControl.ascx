<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AddressSearchInputControl.ascx.vb"
    Inherits="AppUeberf.Controls.AddressSearchInputControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<p>
    <table id="Table1" cellspacing="0" cellpadding="2" width="474" border="0" style="width: 474px;
        height: 118px" bgcolor="#eeeeef">
        <tr>
            <td colspan="3" style="width: 454px">
                <asp:Label ID="Label4" runat="server">Addresssuche</asp:Label>&nbsp;
                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 137px">
                <asp:Label ID="Label3" runat="server">Name:</asp:Label>
            </td>
            <td style="width: 93px">
                <asp:Label ID="Label1" runat="server">PLZ</asp:Label>
            </td>
            <td style="width: 197px">
                <asp:Label ID="Label2" runat="server">Ort</asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 137px">
                <asp:TextBox ID="txtName" runat="server" MaxLength="40"></asp:TextBox>
            </td>
            <td style="width: 93px">
                <asp:TextBox ID="txtPLZ" runat="server" Width="90px" MaxLength="5"></asp:TextBox>
            </td>
            <td style="width: 197px">
                <asp:TextBox ID="txtOrt" runat="server" MaxLength="40"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="3" style="width: 454px">
                <asp:Button ID="btnSuche" runat="server" Text="Suchen"></asp:Button>
                <asp:Label ID="Label5" runat="server">Platzhaltersuche durch Verwendung des Zeichens * möglich.</asp:Label>
            </td>
        </tr>
    </table>
</p>
