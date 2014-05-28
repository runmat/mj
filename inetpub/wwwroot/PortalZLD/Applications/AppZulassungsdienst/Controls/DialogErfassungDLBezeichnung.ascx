<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DialogErfassungDLBezeichnung.ascx.cs" Inherits="AppZulassungsdienst.Controls.DialogErfassungDLBezeichnung" %>
<script language="javascript" type="text/javascript">
    function SetDLBezeichnung(wert) {
        $get('<%=txtDLBezeichnung.ClientID%>').value = wert;
    }
</script>
<div style="padding-left:10px;padding-top:15px;margin-bottom:10px;">
    Bitte geben Sie an, um welche Art der Dienstleistung es sich bei<br />
    der erfassten "Sonstigen Dienstleistung" handelt:                                                                                 
</div>
<div>
    &nbsp;
</div>
<table width="100%">
    <tr>
        <td style="padding-left:10px">
            Beschreibung:
        </td>
        <td style="padding-left:10px">
            <asp:TextBox ID="txtDLBezeichnung" runat="server" CssClass="TextBoxNormal" Width="255px" MaxLength="40"></asp:TextBox>
        </td> 
    </tr>
    <tr>
        <td colspan="2">
            &nbsp;
        </td>     
    </tr>
    <tr>
        <td colspan="2" style="padding-left: 150px">
            <asp:Button ID="btnClose" runat="server" Text="Schließen" CssClass="TablebuttonLarge"
                Font-Bold="true" Width="75px" OnClick="btnClose_Click" style="margin-bottom:10px" />                                                    
        </td>
    </tr>
</table>