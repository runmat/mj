<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="WarengruppeAnlegen.aspx.cs" Inherits="Kantine.Warengruppe.WarengruppeAnlegen" MasterPageFile="Kantine.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <div id="Main" style="text-align: center;">
        <br />
        <div style="float: left;">
            <asp:Label ID="lblError" runat="server" class="Error"></asp:Label>
        </div>
        <br />
        <br />
        <div class="Heading">
            <asp:label ID="lblÜberschrift" runat="server">Warengruppe anlegen</asp:label>
        </div>
        <div class="Rahmen">
            <div>&nbsp;</div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">                
                <ContentTemplate>
                    <table>
                        <tr>
                            <td class="Beschriftung" width="150">
                                Warengruppenbezeichnung:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtWarengruppenBezeichnung" runat="server" Width="200px" 
                                    AutoPostBack="false" ontextchanged="txtWarengruppenBezeichnung_TextChanged" TabIndex="1"></asp:TextBox>
                            </td>
                        </tr>                        
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div>
                &nbsp;
            </div>
        </div>
        <div class="Buttoncontainer">
            <table>
                <tr>
                    <td width="100%">
                    </td>
                    <td>
                        <asp:Button ID="btnSpeichern" runat="server" Text="Speichern" CssClass="Button" OnClick="btnSpeichern_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnBack" runat="server" Text="Zurück" CssClass="Button" OnClick="btnBack_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
