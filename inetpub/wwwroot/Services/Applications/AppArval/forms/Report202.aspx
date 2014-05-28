<%@ Page Language="vb" EnableEventValidation="false" AutoEventWireup="false" CodeBehind="Report202.aspx.vb"
    Inherits="AppArval.Report202" MasterPageFile="../../../MasterPage/Services.Master" %>
    
<%@ Register Assembly="BusyBoxDotNet" Namespace="BusyBoxDotNet" TagPrefix="busyboxdotnet" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                
                                    <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>&nbsp;
                                    <asp:Label ID="lblPageTitle" runat="server">(Zusammenstellung von Abfragekriterien)</asp:Label>
                                
                            </h1>
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
                                        <td colspan="3" class="firstLeft active">
                                            <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Zulassungdatum von:
                                        </td>
                                        <td>
                                         <div class="NeutralCalendar" >
                                           <asp:TextBox CssClass="TextBoxNormal" ID="txtAbmeldedatumVon" runat="server"></asp:TextBox>&nbsp;(TT.MM.JJJJ)&nbsp;
                                         </div>
                                          
                                         <cc1:CalendarExtender ID="txtAbmeldedatumVon_CalendarExtender" runat="server" Format="dd.MM.yyyy"
                                                    PopupPosition="BottomLeft" Animated="true" Enabled="True" TargetControlID="txtAbmeldedatumVon">
                                                </cc1:CalendarExtender>
                                                <cc1:MaskedEditExtender ID="meetxtAbmeldedatumVon" runat="server" TargetControlID="txtAbmeldedatumVon"
                                                    Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                </cc1:MaskedEditExtender>
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Zulassungdatum bis:
                                        </td>
                                        <td>
                                         <div class="NeutralCalendar" >
                                            <asp:TextBox CssClass="TextBoxNormal" ID="txtAbmeldedatumBis" runat="server"></asp:TextBox>&nbsp;(TT.MM.JJJJ)&nbsp;
                                          </div>
                                          <cc1:CalendarExtender ID="txtAbmeldedatumBis_CalendarExtender" runat="server" Format="dd.MM.yyyy"
                                                    PopupPosition="BottomLeft" Animated="true" Enabled="True" TargetControlID="txtAbmeldedatumbis">
                                                </cc1:CalendarExtender>
                                                <cc1:MaskedEditExtender ID="meetxtAbmeldedatumbis" runat="server" TargetControlID="txtAbmeldedatumbis"
                                                    Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                </cc1:MaskedEditExtender>
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Leasingvertrags-Nr.
                                        </td>
                                        <td>
                                            <asp:TextBox CssClass="TextBoxNormal" ID="txtLVNr" runat="server"></asp:TextBox>&nbsp;(12345678)
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Kundennr. Händler
                                        </td>
                                        <td>
                                            <asp:TextBox CssClass="TextBoxNormal" ID="txtHalter" runat="server"></asp:TextBox>&nbsp;(12345678)
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div id="dataFooter">
                            <asp:LinkButton ID="cmdCreate" Text="Weiter" Height="16px" Width="78px" runat="server"
                                CssClass="Tablebutton" ></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
