<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EFA_IPVerwaltung.aspx.vb"
    Inherits="Admin.EFA_IPVerwaltung" MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
            </div>
            <div id="innerContent">
                <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                <asp:Label ID="lblMessage" runat="server" CssClass="TextExtraLarge" EnableViewState="False"></asp:Label>
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                    </div>
                    <div id="TableQuery">
                        <table id="tableSearch" runat="server" cellspacing="0" cellpadding="0">
                            <tbody>
                                <tr class="formquery">
                                    <td class="firstLeft active" colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr class="formquery" id="trInsertIP" runat="server">
                                    <td class="firstLeft active">
                                        KassenIP hinzufügen
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:TextBox runat="server" ID="txtNeuKasseIP" Width="120" MaxLength="15"></asp:TextBox>
                                    </td>
                                    <td class="firstLeft active" colspan="2" style="width: 100%;">
                                        <asp:LinkButton ID="lbHinzufuegen" runat="server" CssClass="StandardButton">&#8226;&nbsp;hinzufügen</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr class="formquery" id="trSuche" runat="server">
                                    <td class="firstLeft active" style="height: 36px">
                                        <asp:Label ID="lblSuch" runat="server">Suche:</asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="height: 36px">
                                        <asp:DropDownList ID="ddlSearch" runat="server" Style="width: auto">
                                            <asp:ListItem Text="IP" Value="IP"> </asp:ListItem>
                                            <asp:ListItem Text="Lagerort" Value="LGORT"> </asp:ListItem>
                                            <asp:ListItem Text="WERK" Value="WERKS"> </asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="firstLeft active" style="height: 36px">
                                        <asp:TextBox ID="txtSuche" runat="server" CssClass="TextBoxNormal" MaxLength="15"></asp:TextBox>
                                    </td>
                                    <td class="firstLeft active" style="width: 100%; height: 36px;">
                                        <asp:ImageButton ID="ibtnSearch" Style="padding-bottom: 2px; padding-right: 5px;"
                                            ImageUrl="/Services/Images/FilterEFA.gif" runat="server" OnClick="ibtnSearch_Click" />
                                        <asp:ImageButton ID="ibtnNoFilter" Visible="false" ImageUrl="/Services/Images/Unfilter.gif"
                                            runat="server" OnClick="ibtnNoFilter_Click" />
                                    </td>
                                </tr>
                                <tr style="background-color: #dfdfdf; height: 22px">
                                    <td colspan="4">
                                        &nbsp;
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <div id="QueryFooter" runat="server">
                        </div>
                    </div>
                    <div id="Result" runat="Server" visible="true">
                        <div id="pagination">
                            <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                        </div>
                        <div id="data">
                            <asp:GridView ID="gv" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0" AllowPaging="True" GridLines="None"
                                PageSize="20" CssClass="GridView" DataKeyNames="IP">
                                <PagerSettings Visible="false" />
                                <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                <AlternatingRowStyle CssClass="GridTableAlternate" />
                                <RowStyle Wrap="false" CssClass="ItemStyle" />
                                <EditRowStyle Wrap="False"></EditRowStyle>
                                <Columns>
                                    <asp:BoundField Visible="false" DataField="IP" ReadOnly="true" />
                                    <asp:BoundField Visible="false" DataField="CustomerID" ReadOnly="true" />
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="IP" CommandName="sort"
                                                Text="Kassen IP"></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" MaxLength="15" ID="txtIP" Text='<%# DataBinder.Eval(Container, "DataItem.IP") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="120" />
                                        <HeaderStyle  Width="120"/>

                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="LGORT" CommandName="sort"
                                                Text="Lagerort"></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" MaxLength="4" ID="txtLGORT" Text='<%# DataBinder.Eval(Container, "DataItem.LGORT") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="120" />
                                        <HeaderStyle  Width="120"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="WERKS" CommandName="sort"
                                                Text="WERK"></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" MaxLength="4" ID="txtWERKS" Text='<%# DataBinder.Eval(Container, "DataItem.WERKS") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="120" />
                                        <HeaderStyle  Width="120"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label runat="server" Text="Firma" ID="lblFirma"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DropDownList runat="server" ID="ddlFirma" Width="130px">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <ItemStyle Width="140" />
                                        <HeaderStyle  Width="140"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Superadmin">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkMaster" Checked='<%# DataBinder.Eval(Container, "DataItem.Master") %>'
                                                runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle Width="60" />
                                        <HeaderStyle  Width="60"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ToolTip="Diese Kasse speichern" ID="ImageButton1"
                                                Height="14" CommandName="save" Width="14" ImageUrl="/Services/Images/Save.gif"
                                                CommandArgument='<%# DataBinder.Eval(Container, "DataItem.IP") %>' />
                                            <asp:ImageButton runat="server" ToolTip="Diese Kasse entfernen" ID="imgDelete" Height="14"
                                                CommandName="entfernen" Width="14" ImageUrl="/Services/Images/Papierkorb_01.gif"
                                                CommandArgument='<%# DataBinder.Eval(Container, "DataItem.IP") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="65"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" Width="65"></ItemStyle>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
