<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report28s.aspx.vb" Inherits="CKG.Components.ComCommon.Report28s" MasterPageFile="../../MasterPage/Services.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>

                            <div id="paginationQuery" style="width: 100%; display: block;">
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div id="TableQuery" style="margin-bottom: 10px">
                                <table id="tab1" cellpadding="0" cellspacing="0">
                                    <tfoot>
                                        <tr>
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tfoot>
                                    <tbody>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td nowrap="nowrap" width="100%">
                                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:LinkButton ID="cmdCreate" runat="server" CssClass="Tablebutton" Height="16px"
                                                    Width="78px">» Erstellen</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div id="DivPlaceholder" runat="server" style="height: 550px;">
                            </div>
                     </div>
                <asp:Literal ID="Literal1" runat="server"></asp:Literal></div>
        </div>
    </div>
</asp:Content>
