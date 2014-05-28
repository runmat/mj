<%@ Page Language="vb"   EnableEventValidation="false" AutoEventWireup="false" CodeBehind="Change03_5.aspx.vb" Inherits="AppArval.Change03_5"
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
                        <div id="paginationQuery">
                            <table cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr>
                                        <td class="firstLeft active">
                                      <STRONG><U>Sie haben 
								folgenden&nbsp;Zulassungsauftag erstellt</U></STRONG>
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
                                       <tr class="formquery">
                                        <td colspan="3"  class="firstLeft active" >
                                         <asp:Label id="lblContent" runat="server" Font-Names="Courier New"  ></asp:Label>
                                        </td>
                                    </tr>
                                      
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                          &nbsp;
                                        </td>
                                        <td class="firstLeft active" >
                                            &nbsp;
                                        </td>
                                        <td class="firstLeft active" width="100%"  >
                                            &nbsp;
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