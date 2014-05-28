<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change06.aspx.vb" Inherits="KBS.Change06" MasterPageFile="~/KBS.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" CausesValidation="false" runat="server" Visible="True">zurück</asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%;">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label">Bestellung Einzahlungsbelege</asp:Label>
                            <asp:Label ID="lblPageTitle" runat="server"/>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td class="firstLeft active">
                                        Bitte geben Sie hier die gewünschte Menge ein und klicken Sie auf 'Absenden'.
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="TableQuery">
                        <table cellpadding="0" cellspacing="0">
                            <tfoot>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </tfoot>
                            <tbody>
                                <tr class="formquery" style="width:100%">
                                    <td class="firstLeft active" >
                                        <asp:Label ID="lblError" CssClass="TextError" runat="server" /> &nbsp;
                                        </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        Menge
                                    </td>
                                          
                                </tr>                                        
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        <asp:TextBox ID="txtMenge" MaxLength="2" width="50px" runat="server"/>
                                        <cc1:FilteredTextBoxExtender Enabled="true" ID="fteMenge" runat="server" TargetControlID="txtMenge" FilterType="Numbers"/>
                                                
                                    </td>
                                         
                                </tr>
                                         
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        &nbsp;
                                    </td>
                                </tr>

                            </tbody>
                        </table>
                    </div>
                        <div id="dataFooter">
                            <asp:LinkButton ID="lbAbsenden" Text="Absenden" Height="16px" Width="78px" runat="server" CssClass="Tablebutton"/>
                        </div>
                        <asp:Button ID="MPEDummy1" Width="0" Height="0" runat="server" />
                        <cc1:ModalPopupExtender runat="server" ID="MPEBestellResultat" BackgroundCssClass="divProgress" 
                                            Enabled="true" PopupControlID="BestellResultat" TargetControlID="MPEDummy1"/>
                        
                        <asp:Panel ID="BestellResultat" HorizontalAlign="Center" runat="server" style="display:none">
                            <table cellspacing="0" id="Table1" runat="server" width="50%" bgcolor="white" cellpadding="0"
                                style="width:50%; border: solid 1px #646464">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>                               
                                <tr>
                                    <td class="firstLeft active">
                                        Bestellstatus:
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="firstLeft active">
                                        <asp:Label ID="lblBestellMeldung" runat="server"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lbBestellFinalize" Text="ok" Height="16px" Width="78px" runat="server"
                                                        CssClass="Tablebutton"/>
                                    </td>
                                </tr>
                                <tr >
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>                                   
                            </table>
                        </asp:Panel>
                   </div>
                </div>
            </div>
        </div>

</asp:Content>
