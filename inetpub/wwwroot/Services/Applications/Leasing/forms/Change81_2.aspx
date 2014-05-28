<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Change81_2.aspx.cs" Inherits="Leasing.forms.Change81_2"
    MasterPageFile="../Master/App.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:HyperLink ID="step1" runat="server">Fahrzeugsuche</asp:HyperLink>
                <a class="active">| Fahrzeugauswahl</a>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <div id="TableQuery">
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%;
                            border: none">
                            <tr class="formquery">
                                <td class="firstLeft active" style="width: 100%; height: 19px;">
                                    <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red"></asp:Label>
                                    <asp:Label ID="lblNoData" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="Result" runat="Server">
                        <div id="pagination">
                            <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                        </div>
                        <div id="data">
                            <table cellspacing="0" cellpadding="0" bgcolor="white" border="0">
                                <tr>
                                    <td>
                                        <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView1"
                                            CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" AllowSorting="True"
                                            Style="width: auto;" OnSorting="GridView1_Sorting">
                                            <PagerSettings Visible="False" />
                                            <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Equipment" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblEqunr" Text='<%# DataBinder.Eval(Container, "DataItem.Equnr") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Anfordern">
                                                    <HeaderStyle Width="50px" />
                                                    <ItemStyle Width="50px" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkAuswahl" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.MANDT").ToString() != "11" %>'
                                                            Checked='<%# DataBinder.Eval(Container, "DataItem.MANDT").ToString() == "99" %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer"
                                                    HeaderStyle-Width="100px">
                                                    <HeaderStyle Wrap="False" />
                                                    <ItemStyle Wrap="False" />
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="lnkHistorie" Target="_blank" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
                                                        </asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Leasingnummer" HeaderText="col_Leasingnummer"
                                                    HeaderStyle-Width="100px">
                                                    <HeaderStyle Wrap="False" />
                                                    <ItemStyle Wrap="False" />
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Leasingnummer" runat="server" CommandName="Sort" CommandArgument="Modell">col_Leasingnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblLeasingnummer" Text='<%# DataBinder.Eval(Container, "DataItem.Leasingnummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="NummerZB2" HeaderText="col_NummerZB2">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_NummerZB2" runat="server" CommandName="Sort" CommandArgument="NummerZB2">col_NummerZB2</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblNummerZB2" Text='<%# DataBinder.Eval(Container, "DataItem.NummerZB2") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="Kennzeichen">col_Kennzeichen</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblKennzeichen" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Ordernummer" HeaderText="col_Ordernummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Ordernummer" runat="server" CommandName="Sort" CommandArgument="Ordernummer">col_Ordernummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblOrdernummer" Text='<%# DataBinder.Eval(Container, "DataItem.Ordernummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Abmeldedatum" HeaderText="col_Abmeldedatum">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Abmeldedatum" runat="server" CommandName="Sort" CommandArgument="Abmeldedatum">col_Abmeldedatum</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNoDate" runat="server" Font-Bold="True" Visible='<%# DataBinder.Eval(Container, "DataItem.Abmeldedatum") == System.DBNull.Value %>'
                                                            ForeColor="Red">XX.XX.XXXX</asp:Label>
                                                        <asp:Label ID="lblYesDate" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Abmeldedatum") != System.DBNull.Value %>'
                                                            Text='<%# DataBinder.Eval(Container, "DataItem.Abmeldedatum", "{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="CoC" HeaderText="col_CoC">
                                                    <HeaderStyle Width="50px" />
                                                    <ItemStyle Width="50px" />
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_CoC" runat="server" CommandName="Sort" CommandArgument="CoC">col_CoC</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkCoC" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.CoC").ToString()=="X" %>'
                                                            Enabled="False" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Status" HeaderText="col_Status">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Status" runat="server" CommandName="Sort" CommandArgument="Status">col_Status</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblStatus" Text='<%# DataBinder.Eval(Container, "DataItem.Status") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="dataQueryFooter">
                            <asp:LinkButton ID="cmdSend" runat="server" CssClass="Tablebutton" Width="78px" Height="16px"
                                CausesValidation="False" Font-Underline="False" OnClick="cmdSend_Click">» 
                            Weiter</asp:LinkButton>
                            &nbsp;
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
