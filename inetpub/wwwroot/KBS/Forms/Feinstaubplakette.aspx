<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Feinstaubplakette.aspx.vb"
    Inherits="KBS.Feinstaubplakette" MasterPageFile="../KBS.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zur&uuml;ck"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Druck Feinstaubplakette"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                        &nbsp;
                    </div>
                    <div id="TableQuery" style="margin-bottom: 0px">
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0" width="100%">
                            <tbody>
                                <tr class="formquery">
                                    <td class="firstLeft active" style="width: 120px">
                                        <asp:Label runat="server">Kennzeichen:</asp:Label>
                                    </td>
                                    <td class="firstLeft">
                                        <asp:TextBox ID="txtKennzeichen" style="text-transform: uppercase" runat="server" MaxLength="20"></asp:TextBox>                                 
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td> 
                                    </td>
                                    <td class="firstLeft">
                                        <asp:LinkButton ID="cmdCreate" runat="server" CssClass="Tablebutton" Width="78px">» Drucken</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" colspan="2" style="padding-bottom:5px;">
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="dataQueryFooter">
                        &nbsp;
                    </div>
                    <div id="data">
                    </div>
                    <div id="dataFooter">
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
