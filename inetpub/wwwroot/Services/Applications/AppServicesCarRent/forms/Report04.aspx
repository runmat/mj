<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report04.aspx.vb" Inherits="AppServicesCarRent.Report04"
    MasterPageFile="../MasterPage/App.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkCreateExcel1" />
                        </Triggers>
                        <ContentTemplate>
                            <div id="paginationQuery">
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td class="active" style="width: 25px;">
                                                <asp:ImageButton ID="NewSearch" runat="server" Width="17px" OnClick="NewSearch_Click" />
                                            </td>
                                            <td align="left" class="active" style="vertical-align: middle;">
                                                Abfrageoptionen
                                            </td>
                                            <td class="active" style="width: 25px;" align="right">
                                                <asp:ImageButton ID="NewSearch2" runat="server" Width="17px" OnClick="NewSearch_Click" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <asp:Panel runat="server" ID="Panel1">
                                <div id="TableQuery">
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%">
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="2" style="width: 100%">
                                                <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td nowrap="nowrap" class="firstLeft active" style="vertical-align: top">
                                                <asp:Label ID="lblEqui" runat="server" Text="Equipment:"></asp:Label>
                                            </td>
                                            <td class="active" width="100%" style="vertical-align: top; padding-left: 15px">
                                                <asp:RadioButtonList ID="rblEqui" runat="server" RepeatLayout="Flow">
                                                    <asp:ListItem Selected="True" Value="B">ZB II</asp:ListItem>
                                                    <asp:ListItem Value="T">Fahrzeugschlüssel</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" nowrap="nowrap" style="vertical-align: top">
                                                <asp:Label ID="lblMahnstufe" runat="server" Text="Mahnstufe:"></asp:Label>
                                            </td>
                                            <td class="active" width="100%" style="vertical-align: top; padding-left: 15px">
                                                <asp:RadioButtonList ID="rblMahnstufe" runat="server" RepeatLayout="Flow">
                                                    <asp:ListItem Selected="True" Value="1">1x angemahnt</asp:ListItem>
                                                    <asp:ListItem Value="2">2x angemahnt</asp:ListItem>
                                                    <asp:ListItem Value="3">3x angemahnt</asp:ListItem>
                                                    <asp:ListItem Value="A">Alle</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="2" style="width: 100%">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                    <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                        &nbsp;
                                    </div>
                                </div>
                                <div id="dataQueryFooter">
                                    <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px">» Erstellen</asp:LinkButton>
                                </div>
                            </asp:Panel>
                            <div id="Result" runat="Server" visible="false">
                                <div class="ExcelDiv">
                                    <div align="right" class="rightPadding">
                                        <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreateExcel1" runat="server">Excel herunterladen</asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server">
                                    </uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                    CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0" AllowPaging="True" GridLines="None"
                                                    PageSize="20" CssClass="GridView">
                                                    <PagerSettings Visible="false" />
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle Wrap="false" CssClass="ItemStyle" />
                                                    <EditRowStyle Wrap="False"></EditRowStyle>
                                                    <Columns>
                                                        <asp:TemplateField SortExpression="Versanddatum" HeaderText="col_Versanddatum">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Versanddatum" runat="server" CommandArgument="Versanddatum"
                                                                    CommandName="sort">col_Versanddatum</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Versanddatum","{0:d}") %>'
                                                                    ID="lblVersanddatum" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Vertragsnummer" HeaderText="col_Vertragsnummer">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="Vertragsnummer">col_Vertragsnummer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblVertragsnummer" Text='<%# DataBinder.Eval(Container, "DataItem.Vertragsnummer") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblFahrgestellnummer" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandArgument="Kennzeichen"
                                                                    CommandName="sort">col_Kennzeichen</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'
                                                                    ID="lblKennzeichen" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Mahnstufe" HeaderText="col_Mahnstufe">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Mahnstufe" runat="server" CommandArgument="Mahnstufe" CommandName="sort">col_Mahnstufe</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Mahnstufe") %>'
                                                                    ID="lblMahnstufe" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Versandgrund" HeaderText="col_Versandgrund">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Versandgrund" runat="server" CommandArgument="Versandgrund"
                                                                    CommandName="sort">col_Versandgrund</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Versandgrund") %>'
                                                                    ID="lblVersandgrund" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Adresse" HeaderText="col_Adresse">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Adresse" runat="server" CommandArgument="Adresse" CommandName="sort">col_Adresse</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Adresse") %>'
                                                                    ID="lblAdresse" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Materialnummer" HeaderText="col_Materialnummer">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Materialnummer" runat="server" CommandArgument="Materialnummer"
                                                                    CommandName="sort">col_Materialnummer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Materialnummer") %>'
                                                                    ID="lblMaterialnummer" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
