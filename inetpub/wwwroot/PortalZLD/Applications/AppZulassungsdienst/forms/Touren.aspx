<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Touren.aspx.cs" Inherits="AppZulassungsdienst.forms.Touren"
    MasterPageFile="../MasterPage/App.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="JavaScript" type="text/javascript" src="/PortalZLD/Applications/AppZulassungsdienst/JavaScript/helper.js?22042016"></script>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück" OnClick="lb_zurueck_Click"></asp:LinkButton>
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
                                <asp:Panel ID="pnlQuery" runat="server">
                                    <table cellpadding="0" runat="server" id="TableBank" cellspacing="0">
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="4">
                                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="trSearchResult" runat="server" class="formquery">
                                            <td class="firstLeft active">
                                                <asp:GridView ID="GridView1" Border="0" runat="server" AutoGenerateColumns="False"
                                                    ShowHeader="False" OnRowCommand="GridView1_RowCommand">
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                    <Columns>
                                                        <asp:TemplateField ItemStyle-Width="5%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTour" runat="server" Text='<%# Eval("Gruppe") %>'/> 
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="GruppenName" SortExpression="GruppenName"></asp:BoundField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ibtnedt" ImageUrl="/PortalZLD/images/Edit.gif" CommandArgument='<%# Eval("Gruppe") %>'
                                                                    runat="server" CommandName="Edt" ToolTip="Bearbeiten" Width="16px" Height="16px" />
                                                                <asp:ImageButton ID="ibtnInsert" ImageUrl="/PortalZLD/images/Rein01_04.jpg" CommandArgument='<%# Eval("Gruppe") %>'
                                                                    runat="server" CommandName="Insert" ToolTip="Kunden hinzufügen/bearbeiten" Width="16px"
                                                                    Height="16px" />
                                                                <asp:ImageButton ID="ibtnDel" ImageUrl="/PortalZLD/images/del.png" CommandArgument='<%# Eval("Gruppe") %>'
                                                                    runat="server" CommandName="Del" ToolTip="Löschen" />
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="TablePadding" Width="60px" />
                                                            <ItemStyle CssClass="TablePadding" Width="60px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                            <td style="width: 45%">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="dataQueryFooter">
                                                    <asp:LinkButton ID="lbtnNew" Text="» Neue Tour" Height="16px" Width="100px" runat="server"
                                                        CssClass="TablebuttonMiddle" TabIndex="27" OnClick="lbtnNew_Click"></asp:LinkButton>
                                                </div>
                                            </td>
                                            <td style="width: 45%">
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="Panel2" runat="server" Visible="false">
                                    <table id="Table1" runat="server" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr>
                                                <td colspan="2" style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="2">
                                                    <asp:Label ID="lblErrorInsert" runat="server" CssClass="TextError"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" style="height: 32px">
                                                    <asp:Label ID="Label3" runat="server" Height="16px">Tourbezeichnung</asp:Label>
                                                </td>
                                                <td class="firstLeft active" style="width: 100%">
                                                    <asp:TextBox ID="txtGruppe" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                                    &nbsp;* max. 30 Zeichen
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td colspan="2">
                                                    <div class="dataQueryFooter" style="width: 525px">
                                                        <asp:LinkButton ID="lbtnInsertTour" runat="server" CssClass="TablebuttonMiddle"
                                                            Height="16px" TabIndex="27" Text="» Speichern" Width="100px" OnClick="lbtnInsertTour_Click"></asp:LinkButton>
                                                        <asp:LinkButton ID="lbtnCancelTour" Text="» Abbrechen" Height="16px" Width="100px"
                                                            runat="server" CssClass="TablebuttonMiddle" TabIndex="27" OnClick="lbtnCancelTour_Click"></asp:LinkButton>
                                                        <asp:Label ID="lblTourIDEdit" runat="server" Visible="False"></asp:Label>
                                                    </div>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div id="Div1" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="Panel1" runat="server" Visible="false">
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr>
                                                <td colspan="3" style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="3">
                                                    <asp:Label ID="lblErrorTour" runat="server" CssClass="TextError"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" style="height: 32px">
                                                    <asp:Label ID="lblTour" runat="server" Height="16px">Tourbezeichnung</asp:Label>
                                                </td>
                                                <td class="firstLeft active" colspan="2" style="height: 32px">
                                                    <asp:Label ID="lblTourShow" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblKunde" runat="server">Kunde*:</asp:Label>
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtKunnr" AutoPostBack="false" runat="server" CssClass="TextBoxNormal"
                                                        MaxLength="8" Width="75px"></asp:TextBox>
                                                </td>
                                                <td class="firstLeft active" style="width: 100%; vertical-align: top; margin-top: 3px">
                                                    <asp:DropDownList ID="ddlKunnr" runat="server" Style="width: auto; position: absolute;" EnableViewState="False">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td colspan="3">
                                                    <div class="dataQueryFooter" style="margin-bottom: 5px;">
                                                        <asp:Label ID="lblTourID" runat="server" Visible="False"></asp:Label>
                                                        <asp:LinkButton ID="lbtnInsert" Text="» hinzufügen" Height="16px" Width="100px" runat="server"
                                                            CssClass="TablebuttonMiddle" TabIndex="27" OnClick="lbtnInsert_Click"></asp:LinkButton>
                                                        <asp:LinkButton ID="lblCancelCustomer" Text="» Abbrechen" Height="16px" Width="100px"
                                                            runat="server" CssClass="TablebuttonMiddle" TabIndex="27" OnClick="lbtnCancelTour_Click"></asp:LinkButton>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="3">
                                                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                                                        Border="0" Width="500px" OnRowCommand="GridView2_RowCommand">
                                                        <RowStyle CssClass="ItemStyle" />
                                                        <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                        <Columns>
                                                            <asp:TemplateField Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTempID" runat="server" Text='<%# Eval("KundenNr") %>'> </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Name1" ItemStyle-Width="100%" SortExpression="Name1" HeaderText="Kunde">
                                                            </asp:BoundField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="ibtnDel" ImageUrl="/PortalZLD/images/del.png" CommandArgument='<%# Eval("KundenNr") %>'
                                                                        runat="server" CommandName="Del" ToolTip="Löschen" />
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="TablePadding" Width="60px" />
                                                                <ItemStyle CssClass="TablePadding" Width="60px" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td colspan="3">
                                                    <div class="dataQueryFooter" style="margin-bottom: 15px;">
                                                        &nbsp;
                                                    </div>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                    </div>
                                </asp:Panel>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
