<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change09.aspx.vb" Inherits="KBS.Change09" MasterPageFile="~/KBS.Master" %>

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
                            <asp:Label ID="lblHead" runat="server" Text="Label">Inventur</asp:Label>
                            <asp:Label ID="lblPageTitle" runat="server"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td class="firstLeft active">
                                        &nbsp;
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="TableQuery"> 
                        <asp:UpdatePanel runat="server" ID="upGrid">
                            <ContentTemplate>
                                <table cellspacing="0" width="100%" cellpadding="0" bgcolor="white" border="0">
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        <asp:Repeater ID="Repeater1" runat="server" >
                                            <ItemTemplate>
                                                <table class="" style="border: none" border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr class="formquery">
                                                        <td class="firstLeft active">
                                                            <asp:LinkButton ID="lbtNext" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "PRODH")%>' 
                                                             runat="server" OnClick="lbtNext_Click" ForeColor="#4C4C4C"
                                                             
                                                             Font-Underline="True"
                                                             Font-Size="12px"><%#DataBinder.Eval(Container.DataItem, "VTEXT")%>
                                                             </asp:LinkButton>
    
                                                        </td>
                                                        <td style=" width:75%">
                                                             <img runat="server" alt="" src="/KBS/Images/haken_gruen.gif" 
                                                             visible='<%#DataBinder.Eval(Container.DataItem, "ZAEHLVH") isnot System.String.Empty %>' border="0" />
                                                        </td>
    
                                                    </tr>

                                                </table>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                         </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                    </tr>                                    
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div id="dataFooter">
                            <asp:LinkButton ID="lbAbsenden" 
                                Text="Inventur Abschließen" Height="16px" Width="155px" runat="server" CssClass="TablebuttonXLarge"></asp:LinkButton>
                                <asp:LinkButton ID="lbNachdruck" 
                                Text="Nachdruck" Height="16px" Width="78px" Visible="false" runat="server" CssClass="Tablebutton"></asp:LinkButton>
                        </div>
                        <asp:Panel ID="PLCheck" runat="server" Width="350px" style="display:none">
                            <table cellspacing="0" id="tblCheck" runat="server" width="100%" bgcolor="#FFFFFF"
                                cellpadding="0" border="0" style="border: solid 1px #000000" > 
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>                              
                               <tr>
                                    <td align="center" class="firstLeft active" >
                                       Wollen Sie die Inventur wirklich abschließen?
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:LinkButton ID="lbOk" Text="Abschließen" Height="16px" Width="78px" runat="server"
                                            CssClass="Tablebutton"></asp:LinkButton>
                                        &nbsp; &nbsp;
                                        <asp:LinkButton ID="lbCancel" Text="Abbrechen" Height="16px" Width="78px"
                                            runat="server" CssClass="Tablebutton"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Button ID="MPEDummy" style="display: none" runat="server" />
                        <cc1:ModalPopupExtender runat="server" ID="mpeCheck" BackgroundCssClass="divProgress"
                            Enabled="true" PopupControlID="PLCheck" TargetControlID="MPEDummy" CancelControlID="lbCancel">
                        </cc1:ModalPopupExtender>
                   </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
