<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="ArtikelAnlegen.aspx.cs" Inherits="Kantine.ArtikelAnlegen" MasterPageFile="Kantine.Master" %>

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
            <asp:label ID="lblÜberschrift" runat="server">Artikel anlegen</asp:label>
        </div>
        <div class="Rahmen">
            <div>&nbsp;</div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">                
                <ContentTemplate>
                    <table>
                        <tr>
                            <td class="Beschriftung" width="150">
                                Artikelbezeichnung:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtArtikelbezeichnung" runat="server" Width="200px" OnTextChanged="txtArtikelbezeichnung_TextChanged" AutoPostBack="false" TabIndex="1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="Beschriftung" width="150">
                                Preis in &#8364;:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtPreis" runat="server" Width="200px" OnTextChanged="txtPreis_TextChanged" AutoPostBack="false" TabIndex="2"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="txtPreis_FilteredTextBoxExtender" 
                                    runat="server" Enabled="True" TargetControlID="txtPreis" FilterMode="ValidChars" ValidChars="0123456789,-+">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="Beschriftung" width="150">
                                Warengruppe:
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlWarengruppe" runat="server" AutoPostBack="false" CssClass="Dropdown" 
                                    OnTextChanged="ddlWarengruppe_TextChanged" DataTextField="BezeichnungWarengruppe" DataValueField="WarengruppeID" TabIndex="3"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="Beschriftung" width="150">
                                EAN:
                            </td>
                            <td align="left">
                               <asp:TextBox ID="txtEAN" runat="server" Width="200px" OnTextChanged="txtEAN_TextChanged"
                                    AutoPostBack="false" MaxLength="12" TabIndex="4"></asp:TextBox>
                               <cc1:FilteredTextBoxExtender ID="ftbeEAN" runat="server"  TargetControlID="txtEAN" FilterMode="ValidChars" ValidChars="0123456789"></cc1:FilteredTextBoxExtender>
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
                        <asp:Button ID="btnSpeichern" runat="server" Text="Speichern" CssClass="Button" OnClick="btnSpeichern_Click" TabIndex="5"/>
                    </td>
                    <td>
                        <asp:Button ID="btnBack" runat="server" Text="Zurück" CssClass="Button" OnClick="btnBack_Click" TabIndex="6"/>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
