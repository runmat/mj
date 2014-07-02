<%@ Page Language="vb"   EnableEventValidation="false" AutoEventWireup="false" CodeBehind="Change02.aspx.vb" Inherits="AppGenerali.Change02"
     MasterPageFile="../../../MasterPage/Services.Master" %>
<%@ Register Assembly="BusyBoxDotNet" Namespace="BusyBoxDotNet" TagPrefix="busyboxdotnet" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <busyboxdotnet:BusyBox ID="BusyBox1" runat="server" Image="Custom" ImageUrl="../../../ApplicationImages/Indicator.gif"
        Text="Ihr Vorgang wird bearbeitet. Dies kann einige Zeit in Anspruch nehmen."
        Title="Bitte warten.." ShowBusyBox="Custom" GZipCompression="False"></busyboxdotnet:BusyBox>
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">&nbsp;
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                        </div>
                        <div id="paginationQuery">
                            <table cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr>
                                        <td class="firstLeft active">
                                            bitte geben Sie ihre Suchkriterien ein
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div id="TableQuery">
                            <table cellpadding="0" cellspacing="0">
                                <tfoot>
                                    <tr>
                                        <td colspan="3">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </tfoot>
                                <tbody>
                                    <tr class="formquery">
                                        <td colspan="3"  class="firstLeft active">
                                            <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="tr_Leasingvertragsnummer" class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_Leasingvertragsnummer" Text="Leasingvertragsnummer:" runat="server"></asp:Label>
                                        </td>
                                        <td class="firstLeft active" >
                                            <asp:TextBox ID="txtLeasingvertragsnummer" runat="server" 
                                                CssClass="TextBoxNormal"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active" width="100%"  >
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr id="tr_Kennzeichen" class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_Kennzeichen" Text="Kennzeichen:" runat="server"></asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtKennzeichen" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr id="tr_Suchname" class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_Suchname" Text="Suchname:" runat="server"></asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtSuchname" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr id="tr_Fahrgestellnummer" class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_Fahrgestellnummer" Text="Fahrgestellnummer:" runat="server"></asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtFahrgestellnummer" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td colspan="3" class="firstLeft active">
                                            <asp:Label ID="lbl_Info" Text="Alle Eingaben mit mehrfacher Platzhalter-Suche (*) möglich (z.B. 'F*23Z*1*')"
                                                runat="server">Alle Eingaben mit mehrfacher Platzhalter-Suche (*) möglich 
                                            (z.B. 'F*23Z*1*')</asp:Label>
                                        </td>
                                        
                                    </tr>
                                    <tr class="formquery">
                                        <td colspan="2" class="firstLeft active">
                                            &nbsp;&nbsp;</td>
                                        <td class="firstLeft active">
                                            &nbsp;</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div id="dataFooter">
                            <asp:LinkButton ID="lb_Weiter" Text="Weiter" Height="16px" Width="78px" runat="server"
                                CssClass="Tablebutton" OnClientClick="Show_ctl00_ContentPlaceHolder1_BusyBox1();"></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
