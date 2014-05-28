<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KontaktLogin.aspx.cs" Inherits="AutohausPortal.Info.KontaktLogin"  
     MasterPageFile="/AutohausPortal/MasterPage/Login.Master" %>
     
<%@ MasterType VirtualPath="/AutohausPortal/MasterPage/Login.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <div id="spacer" style="height: 10px;">
    </div>
    <div class="inhaltsseite">
        <div class="inhaltsseite_top">
            &nbsp;</div>
        <div class="innerwrap">
            <h1>
                Kontakt</h1>
            <table>
                <tr>
                    <td width="50%" valign="top">
                        <p>
                            Christoph Kroschke GmbH<br />
                            Ladestraße 1<br />
                            22926 Ahrensburg
                        </p>
                        <p>
                            <table>
                                <tr>
                                    <td>
                                        Telefon:
                                    </td>
                                    <td style="padding-left:3px;">
                                        +49 (0)4102 804-170
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Telefax:
                                    </td>
                                    <td style="padding-left:3px;">
                                        +49 (0)4102 804-300
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        E-Mail:
                                    </td>
                                    <td style="padding-left:3px;">
                                        <a href="mailto:service@kroschke.de">service@kroschke.de</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Homepage:
                                    </td>
                                    <td style="padding-left:3px;">
                                        <a href="http://www.kroschke.de" target="_blank">www.kroschke.de</a>
                                    </td>
                                </tr>
                            </table>
                        </p>
                    </td>
                    <td width="50%" valign="top">
                        <p>
                            <b>Rückrufservice:</b></p>
                        <p>
                            Fragen? Wir rufen Sie kostenlos zurück!<br />
                            Einfach Ihre Telefonnummer eingeben und "Absenden" anklicken!</p>
                        <div>
                            <table>
                                <tr>
                                    <td>
                                        Anrede:
                                    </td>
                                    <td>
                                        <div class="formfeld">
                                            <asp:DropDownList runat="server" ID="ddlAnrede" Width="80px">
                                                <asp:ListItem Text="Frau" Value="Frau"></asp:ListItem>
                                                <asp:ListItem Text="Herr" Value="Herr"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Ihr Name:
                                    </td>
                                    <td colspan="2">
                                        <div id="divName" runat="server" class="formfeld">
                                            <div class="formfeld_start"></div>
                                            <asp:TextBox ID="txtName" runat="server" Width="270px" CssClass="formtext"></asp:TextBox>
                                            <div class="formfeld_end"></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Telefon-Nr.:
                                    </td>
                                    <td colspan="2">
                                        <div id="divTelefon" runat="server" class="formfeld">
                                            <div class="formfeld_start"></div>
                                            <asp:TextBox ID="txtTelefon" runat="server" Width="270px" CssClass="formtext"></asp:TextBox>
                                            <div class="formfeld_end"></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Betreff:
                                    </td>
                                    <td colspan="2">
                                        <div id="divBetreff" runat="server" class="formfeld">
                                            <div class="formfeld_start"></div>
                                            <asp:TextBox ID="txtBetreff" runat="server" Width="270px" CssClass="formtext"></asp:TextBox>
                                            <div class="formfeld_end"></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Rückruftermin:
                                    </td>
                                    <td>
                                        <div id="divDatum" runat="server" class="formfeld" style="margin-left: 0px;">
                                            <div class="formfeld_start"></div>
                                            <asp:TextBox ID="txtDatum" runat="server" CssClass="formtext jqcalendar jqcalendarWerktage"
                                                Width="100px" MaxLength="10"></asp:TextBox>
                                            <div class="formfeld_end_wide">
                                                <img src="../images/icon_datepicker.gif" width="22" height="19" alt="Kalender" class="datepicker" /></div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="formfeld">
                                            <asp:DropDownList ID="ddlZeit" runat="server" Style="height: 36px;">
                                                <asp:ListItem>8:00 - 9:00 Uhr</asp:ListItem>
                                                <asp:ListItem>9:00 - 10:00 Uhr</asp:ListItem>
                                                <asp:ListItem>10:00 - 11:00 Uhr</asp:ListItem>
                                                <asp:ListItem>11:00 - 12:00 Uhr</asp:ListItem>
                                                <asp:ListItem>12:00 - 13:00 Uhr</asp:ListItem>
                                                <asp:ListItem>13:00 - 14:00 Uhr</asp:ListItem>
                                                <asp:ListItem>14:00 - 15:00 Uhr</asp:ListItem>
                                                <asp:ListItem>15:00 - 16:00 Uhr</asp:ListItem>
                                                <asp:ListItem>17:00 - 18:00 Uhr</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Label ID="lblError" runat="server" Style="color: #B54D4D" Text=""></asp:Label><br />
                                        <asp:Label ID="lblMessage" runat="server" Style="color: #269700" Visible="false">Ihre Anfrage wurde gesendet. Sie werden schnellstm&ouml;glich zur&uuml;ckgerufen.</asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
            <div class="formbuttons" style="padding-right:46px;">
                <asp:Button ID="cmdSave" runat="server" CssClass="submitbutton" Text="Absenden" OnClick="cmdSave_Click" style="margin-bottom:10px;" />
            </div>            
        </div>        
        <div class="inhaltsseite_bot">
            &nbsp;</div>
    </div>
</asp:Content>