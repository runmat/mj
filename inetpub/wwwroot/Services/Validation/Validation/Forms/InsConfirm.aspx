<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InsConfirm.aspx.vb" Inherits="Validation.InsConfirm" MasterPageFile="~/Master/Design.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   

    <style type="text/css">
        .style1
        {
            font-weight: bold;
            width: 52px;
        }
        .style2
        {
            width: 52px;
        }
    </style>
   

</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div id="site">
         <asp:ScriptManager ID="Scriptmanager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true" AsyncPostBackTimeout="36000" ></asp:ScriptManager>

        <div id="content">
            <div id="navigationSubmenu">
                &nbsp;</div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 78%">
                    <div id="innerContentRightHeading">
                        <h1>
                            Willkommen auf unserem Internet-Server!</h1>
                    </div>
                    <div id="pagination">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td class="login">
                                        &nbsp;</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>

                    <div id="login">
                        <div id="StandardLogin" runat="server" style="margin-bottom: 45px">
                            <table cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr>
                                        <td colspan="3" class="paddingTop">
                                            &nbsp;</td>
                                    </tr>
                                    <tr><td colspan="3">
                                            <asp:Label ID="lblError" runat="server" CssClass="TextError" 
                                                EnableViewState="False"></asp:Label>
                                        </td></tr>
                                    <tr><td colspan="3">
                                            <asp:Label ID="lblAusgabe" runat="server" Font-Bold="True" ForeColor="Blue"></asp:Label>
                                        </td></tr>
                                    <tr><td colspan="3">
                                            &nbsp;</td></tr>
                                    <tr>
                                        <td class="bold" colspan="3">
                                           <asp:Label ID="lblInfo" runat="server" Visible="False">Bitte geben Sie Ihre 9-stellige Agenturnummer ein, und klicken Sie auf 'Bestätigen'.</asp:Label> </td>
                                        
                                    </tr>
                                    <tr>
                                        <td class="style1">
                                            &nbsp;</td>
                                        <td class="input">
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblVermittlernummer" runat="server" Visible="False">Agenturnummer: </asp:Label>
                                            </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtVermittlernummer" runat="server" MaxLength="9" 
                                                Width="100px"></asp:TextBox>
                                                            <ajaxToolkit:MaskedEditExtender ID="txtVermittlernummer_MaskedEditExtender" runat="server"
                                                                AutoComplete="False" ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                CultureTimePlaceholder="" Enabled="True" Mask="9999-9999-9" MaskType="Number"
                                                                TargetControlID="txtVermittlernummer">
                                                            </ajaxToolkit:MaskedEditExtender>
                                            </td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="style1">
                                            &nbsp;</td>
                                        <td  class="rightPadding" align="right">
                                           <asp:LinkButton ID="cmdConfirm" runat="server" CssClass="Tablebutton" Height="16px"
                                                Text="&amp;nbsp;&amp;#187; Bestätigen" Width="78px"></asp:LinkButton>
                                           </td>
                                        <td class="login">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="style2">
                                        </td>
                                        <td class="login">
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                </tbody>
                            </table>
                            <div style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #dfdfdf">
                            </div>
                        </div>
  
                    </div>
                    <div>
                    </div>

                </div>
            </div>
        </div>
    </div>
</asp:Content>