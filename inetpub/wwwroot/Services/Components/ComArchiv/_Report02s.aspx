<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="_Report02s.aspx.vb" Inherits="CKG.Components.ComArchive._Report02s"
    MasterPageFile="../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../PageElements/GridNavigation.ascx" %>
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
                        <asp:Panel ID="DivSearch" runat="server">
                            <div id="TableQuery">
                                <table id="tab1" cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table id="tbAnwendungen" cellpadding="0" cellspacing="0" runat="server">
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div style="background-color: #dfdfdf; height: 22px; width: 100%">
                                    &nbsp;
                                </div>
                            </div>
                        </asp:Panel>
                        <div id="dataQueryFooter">
                            <asp:LinkButton ID="btnback" runat="server" CssClass="Tablebutton" Width="78px">» zurück </asp:LinkButton>
                            <span runat="server" id="Spani"></span>
                        </div>
                        <div id="dataFooter">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
