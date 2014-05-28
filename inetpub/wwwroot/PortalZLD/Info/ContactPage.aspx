<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ContactPage.aspx.vb" Inherits="CKG.PortalZLD.ContactPage"
    MasterPageFile="../MasterPage/Services.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="/PortalZLD/JScript/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="/PortalZLD/JScript/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
        <script src="/PortalZLD/JScript/jquery.ui.datepicker-de.js" type="text/javascript"></script>
        <script type="text/javascript">
            function pageLoad() {
                $("#ctl00_ContentPlaceHolder1_txtDate").unbind();
                $("#ctl00_ContentPlaceHolder1_txtDate").datepicker();
            }
        </script>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
            </div>
            <div id="innerContent">
                <div id="innerContentRight">
                    <div id="Result" runat="Server" style="margin-top: 25px;">
                        <div class="DivKontakt" runat="server">
                            <div class="ImpressumHead" style="width: 100%">
                                <h1>
                                    <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                            </div>
                            <div id="pnl1ndAddress" class="pnl1ndAddress" runat="server">
                                <table cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCName2" runat="server" CssClass="lblKontaktFirma">Christoph Kroschke GmbH</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCAddress2" runat="server">Ladestraße 1 </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCOrt" runat="server">22926 Ahrensburg</asp:Label><br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCTel" runat="server">Telefon: +49 4102 804-222</asp:Label><br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCfax" runat="server">Telefax: +49 4102 804-300</asp:Label><br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="pnlLinks2" runat="server">
                                                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="mailto:support@kroschke.de">support@kroschke.de</asp:HyperLink>
                                                <br />
                                                <asp:HyperLink ID="lnkWeb2" runat="server" NavigateUrl="http://www.kroschke.de">www.kroschke.de</asp:HyperLink>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            
                        </div>
                    </div>
                </div>
            </div>
            <div id="innerContentLeft2">
                <div class="ImpressumHead" style="width: 100%">
                    <h1>
                        <asp:Label ID="Label1" runat="server" Text="Rückrufservice"></asp:Label></h1>
                </div>
                <div id="innerContentLeft2Callback"  style="width: 100%">
                    <table cellspacing="0" cellpadding="0" width="100%">
                        <tr>
                            <td>
                                <asp:Panel ID="Panel1" runat="server" Visible="true" Width="100%">
                                    <table cellspacing="0" cellpadding="0" width="100%">
                                        <tr>
                                            <td class="innerCallback" colspan="3">
                                                <asp:Label ID="Label2" runat="server" CssClass="lblKontaktFirma">Fragen? Wir rufen Sie kostenlos zurück!</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="innerCallback" colspan="3">
                                                <asp:Label ID="Label3" runat="server">Einfach Ihre Telefonnummer eingeben und "Absenden" anklicken!</asp:Label><br />
                                            </td>
                                        </tr>
         
                                        <tr>
                                            <td class="innerCallback" style="width: 112px">
                                                Anrede:</td>
                                            <td align="left" class="innerCallback" colspan="2" width="80%">
                                                <asp:DropDownList ID="ddlAnrede" runat="server" Width="75px" CssClass="field">
                                                    <asp:ListItem>Frau</asp:ListItem>
                                                    <asp:ListItem>Herr</asp:ListItem>
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td class="innerCallback" style="width: 112px" >
                                                Ihr Name:&nbsp;<br />
                                            </td>
                                            <td class="innerCallback">
                                                <asp:TextBox ID="txtName" runat="server" Width="250px" CssClass="field"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="innerCallback" style="width: 112px">
                                                Telefon-Nr.:&nbsp;
                                            </td>
                                            <td align="left" class="innerCallback" colspan="2">
                                                <asp:TextBox ID="txtTel" runat="server" CssClass="field" 
                                                    Width="250px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="innerCallback" style="width: 112px">
                                                Betreff:&nbsp;
                                            </td>
                                            <td align="left" class="innerCallback" colspan="2">
                                                <asp:TextBox ID="txtBetreff" runat="server" Width="250px" CssClass="field"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="innerCallback" style="width: 112px">
                                                Rückruftermin:&nbsp;</td>
                                            <td align="left" class="innerCallback" colspan="2">
                                                <asp:TextBox ID="txtDate" runat="server" Width="100px" CssClass="field"></asp:TextBox>
                                                <asp:DropDownList ID="ddlTime"  runat="server" CssClass="field">
                                                    <asp:ListItem>8:00 - 9:00 Uhr</asp:ListItem>
                                                    <asp:ListItem>9:00 - 10:00 Uhr</asp:ListItem>
                                                    <asp:ListItem>10:00 - 11:00 Uhr</asp:ListItem>
                                                    <asp:ListItem>11:00 - 12:00 Uhr</asp:ListItem>
                                                    <asp:ListItem>12:00 - 13:00 Uhr</asp:ListItem>
                                                    <asp:ListItem>13:00 - 14:00 Uhr</asp:ListItem>
                                                    <asp:ListItem>14:00 - 15:00 Uhr</asp:ListItem>
                                                    <asp:ListItem>15:00 - 16:00 Uhr</asp:ListItem>
                                                    <asp:ListItem>17:00 - 18:00 Uhr</asp:ListItem>
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td class="innerCallback" colspan="3">
                                                <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label><br />
                                                <asp:Label ID="lblSuccess" runat="server" CssClass="TextSuccess" Visible="false">
                                                    Ihre Anfrage wurde gesendet. Sie werden schnellstm&ouml;glich zur&uuml;ckgerufen.
                                                </asp:Label>
                                            </td>
                                        </tr>                                        
                                        
                                        <tr>
                                            <td class="innerCallback" style="width: 112px">
                                                &nbsp;
                                            </td>
                                            <td align="right" class="innerCallback" colspan="2" style="padding-right: 15px">
                                                <asp:LinkButton ID="cmdSend" runat="server" CssClass="Tablebutton" Height="16px"
                                                    Width="78px">» Absenden </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
