<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Start.aspx.vb" Inherits="Validation.Start"  MasterPageFile="~/Master/Design.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   

</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div id="site">
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
                                    <tr><td colspan="3">&nbsp;</td></tr>
                                    <tr>
                                        <td class="bold">
                                            &nbsp;</td>
                                        <td class="input">
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="bold">
                                            &nbsp;</td>
                                        <td class="input">
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="bold">
                                            &nbsp;</td>
                                        <td class="input">
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="bold">
                                            &nbsp;</td>
                                        <td class="input">
                                            &nbsp;</td>
                                        <td class="lock">
                                            <asp:ImageButton ID="btnEmpty" runat="server" ImageUrl="../Images/empty.gif" Height="16px"
                                                Width="1px" />&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
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