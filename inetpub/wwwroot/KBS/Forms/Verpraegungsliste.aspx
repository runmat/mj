<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Verpraegungsliste.aspx.vb"
    Inherits="KBS.Verpraegungsliste" MasterPageFile="../KBS.Master" %>

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
                            <asp:Label ID="lblHead" runat="server" Text="Verprägungsliste"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                        &nbsp;
                    </div>
                    <div id="TableQuery" style="margin-bottom: 0px">
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0" width="100%">
                            <tbody>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label runat="server">Belegnummer:</asp:Label>
                                    </td>
                                    <td colspan="2" class="firstLeft">
                                        <asp:TextBox ID="txtBelegnummer" runat="server"></asp:TextBox>                                 
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label runat="server">Datum von:</asp:Label>
                                    </td>
                                    <td class="firstLeft" style="width:60px">
                                        <asp:TextBox ID="txtDatumVon" runat="server" MaxLength="6" Width="55px"></asp:TextBox>                                 
                                    </td>
                                    <td>
                                        &nbsp;Format: TTMMJJ
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label runat="server">Datum bis:</asp:Label>
                                    </td>
                                    <td class="firstLeft" style="width:60px">
                                        <asp:TextBox ID="txtDatumBis" runat="server" MaxLength="6" Width="55px"></asp:TextBox>                                 
                                    </td>
                                    <td>
                                        &nbsp;Format: TTMMJJ
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3"> 
                                    </td>
                                    <td align="right" style="padding-right:15px;">
                                        <asp:LinkButton ID="cmdCreate" runat="server" CssClass="Tablebutton" Width="78px">» Suchen</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" colspan="4" style="padding-bottom:5px;">
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="dataQueryFooter" class="dataQueryFooter">
                        &nbsp;
                    </div>
                    <div id="data">
                    </div>
                    <div id="dataFooter" class="dataQueryFooter">
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
