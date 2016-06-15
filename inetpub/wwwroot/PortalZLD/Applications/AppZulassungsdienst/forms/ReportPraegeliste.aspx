<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportPraegeliste.aspx.cs"
    Inherits="AppZulassungsdienst.forms.ReportPraegeliste" MasterPageFile="../MasterPage/App.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="JavaScript" type="text/javascript" src="/PortalZLD/Applications/AppZulassungsdienst/JavaScript/helper.js?22042016"></script>
    <script language="javascript" type="text/javascript">
        function checkZulassungsdatum() {
            var tb = document.getElementById('<%= txtZulDate.ClientID %>');
            document.getElementById('<%= ihDatumIstWerktag.ClientID %>').value = nurWerktage(tb.value)[0];
            return true;
        }
    </script>
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
                    <div id="paginationQuery">
                    </div>
                    <div id="TableQuery" style="margin-bottom: 10px">
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr class="formquery">
                                    <td class="firstLeft active" colspan="4" width="100%">
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                        <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lblStva" runat="server">StVA von:</asp:Label>
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:TextBox ID="txtStVavon" runat="server" CssClass="TextBoxNormal" MaxLength="8"
                                            Width="75px"></asp:TextBox>
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:Label ID="Label1" runat="server">bis:</asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="width: 100%;">
                                        <asp:TextBox ID="txtStVaBis" runat="server" CssClass="TextBoxNormal" MaxLength="8"
                                            Width="75px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lblDatum" runat="server">Datum der Zulassung:</asp:Label>
                                    </td>
                                    <td class="firstLeft active" colspan="3">
                                        <asp:TextBox ID="txtZulDate" runat="server" CssClass="TextBoxNormal" Width="75px"
                                            MaxLength="6"></asp:TextBox>
                                        <asp:Label ID="txtZulDateFormate" Style="padding-left: 2px; font-weight: normal"
                                            Height="15px" runat="server">(ttmmjj)</asp:Label>
                                        <asp:LinkButton runat="server" Style="padding-left: 10px; font-weight: normal" Height="15px"
                                            ID="lbtnGestern" Text="Gestern |" Width="60px" />
                                        <asp:LinkButton runat="server" Style="font-weight: normal" Height="15px" ID="lbtnHeute"
                                            Width="50px" Text="Heute |" />
                                        <asp:LinkButton runat="server" Style="font-weight: normal" Height="15px" ID="lbtnMorgen"
                                            Width="60px" Text="Morgen" />
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" style="padding-top: 0px">
                                        <asp:Label ID="lblDarstellung" runat="server">Darstellung:</asp:Label>
                                    </td>
                                    <td class="firstLeft active" colspan="3" style="padding-top: 0px">
                                        <asp:RadioButtonList ID="rbAnsicht" Width="250px" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="0" Selected="True" Text="Delta-Liste"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Gesamt-Liste"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" style="padding-top: 0px">
                                       <asp:Label ID="Label2" runat="server">Sortierung nach:</asp:Label>
                                    </td>
                                    <td class="firstLeft active" colspan="3" style="padding-top: 0px">
                                     <asp:RadioButtonList ID="rblSort" Width="250px" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="1" Selected="True" Text="ID"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Kennzeichengröße"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Kennzeichen"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td colspan="4">
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                            &nbsp;
                        </div>
                    </div>
                    <div id="dataQueryFooter">
                        <asp:LinkButton ID="cmdCreate" runat="server" CssClass="Tablebutton" Width="78px"
                            OnClick="cmdCreate_Click" OnClientClick="checkZulassungsdatum();">» Weiter </asp:LinkButton>
                    </div>
                    <div id="dataFooter">
                        &nbsp;
                    </div>
                    <input type="hidden" runat="server" id="ihDatumIstWerktag" value="false" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
