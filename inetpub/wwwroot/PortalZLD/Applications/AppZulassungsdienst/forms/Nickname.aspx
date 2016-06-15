<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Nickname.aspx.cs" Inherits="AppZulassungsdienst.forms.Nickname" MasterPageFile="../MasterPage/App.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="JavaScript" type="text/javascript" src="/PortalZLD/Applications/AppZulassungsdienst/JavaScript/helper.js?22042016"></script>

    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div id="TableQuery">
                                <table cellpadding="0" cellspacing="0">
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="paginationQuery">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tbody>
                                                        <tr>
                                                            <td class="firstLeft active" style="font-size: 12px">
                                                                Suche
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="0" cellspacing="0" style="border: none">
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="width: 10%;font-size: 12px;">
                                                        Kunden-Nr.:
                                                    </td>
                                                    <td class="firstLeft active" style="width: 10%">
                                                        <asp:TextBox ID="txtKundeSearch" runat="server" CssClass="TextBoxNormal" 
                                                           MaxLength="10" onfocus="javascript:this.select();" Width="75px"></asp:TextBox>
                                                    </td>
                                                    <td class="firstLeft active" style="width: 50%; vertical-align:top; margin-top:3px">
                                                        <asp:DropDownList ID="ddlKunnr" runat="server"  
                                                            Style="width: auto; position:absolute;" EnableViewState="False" >
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="active" style="width: 10%">
                                                        <asp:LinkButton ID="lbtnSearch" runat="server" CssClass="Tablebutton" Height="16px" 
                                                           Text="» Suchen" Width="78px" onclick="lbtnSearch_Click"></asp:LinkButton>

                                                    </td>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" style="height: 19px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="paginationQuery">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tbody>
                                                        <tr>
                                                            <td class="firstLeft active" style="font-size: 12px">
                                                                Suchbegriff pflegen
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="0" style="border: none" cellspacing="0">
                                        <tr class="formquery">
                                            <td class="firstLeft active" style="width: 10%;font-size: 12px;">
                                                Kunde:
                                            </td>
                                            <td class="firstLeft active" style="width: 10%">
                                                <asp:TextBox ID="txtKundeNr" Enabled="false"
                                                    runat="server" CssClass="TextBoxNormal" MaxLength="8" Width="95px"></asp:TextBox>
                                            </td>
                                            <td class="active" style="width: 80%">
                                                <asp:TextBox ID="txtKundeName" Enabled="false" runat="server" CssClass="TextBoxNormal"
                                                   Style="width:650px" ></asp:TextBox>
                                            </td>
                                        </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" style="font-size: 12px;">
                                                        Suchbegriff:
                                                    </td>
                                                    <td class="firstLeft active" colspan ="2"  >
                                                      <asp:TextBox ID="txtNickname" MaxLength="30"  runat="server" CssClass="TextBoxNormal"
                                                        Width="400px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" >
                                                       
                                                    </td>
                                                    <td class="firstLeft active" colspan ="2"  >
                                                         max. 30 Zeichen
                                                    </td>
                                                </tr>
                                            </table>
                                            <div id="dataFooter">
                                                <asp:LinkButton ID="lbAbsenden" Visible="false" Text="Absenden" Height="16px" 
                                                    Width="78px" runat="server"
                                                    CssClass="Tablebutton" TabIndex="27" onclick="lbAbsenden_Click" ></asp:LinkButton>
                                                <asp:LinkButton ID="lbtnDelete" Visible="false" Text="Löschen" Height="16px" 
                                                    Width="78px" runat="server"
                                                    CssClass="Tablebutton" TabIndex="27" onclick="lbtnDelete_Click"></asp:LinkButton>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                    </tr>
                                   
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
