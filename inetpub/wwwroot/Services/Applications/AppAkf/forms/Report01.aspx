<%@ Page Language="vb"   EnableEventValidation="false" AutoEventWireup="false" CodeBehind="Report01.aspx.vb" Inherits="AppAkf.Report01"
     MasterPageFile="../../../MasterPage/Services.Master" %>

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
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                        </div>
                       
                        <div id="TableQuery">
                            <table cellpadding="0" cellspacing="0">
                               <tbody>
                                    <tr class="formquery">
                                        <td colspan="3"  class="firstLeft active">
                                            <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                                                   </tbody>
                            </table>
                        </div>
                        
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>