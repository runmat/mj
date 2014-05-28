<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change02_2.aspx.vb" Inherits="AppServicesCarRent.Change02_2"
    MasterPageFile="../MasterPage/App.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                    <asp:HyperLink ID="lnkKreditlimit" runat="server" NavigateUrl="Change01.aspx">Fahrzeugsuche</asp:HyperLink>
                    <a class="active">| Fahrzeugauswahl</a>
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                        </div>
                        <div id="TableQuery">
                            <table id="tab1" cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="ShowScript" runat="server" class="formquery">
                                        <td class="active" width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div id="Result" runat="Server">
                                    <div class="ExcelDiv">
                                        <div align="right" class="rightPadding">
                                            &nbsp;</div>
                                    </div>
                                    <div id="pagination">
                                        <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                    </div>
                                    <div id="data">
                                        <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView1"
                                            CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" AllowSorting="True">
                                            <PagerSettings Visible="False" />
                                            <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <EditRowStyle></EditRowStyle>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Equipment" SortExpression="Equnr" Visible="False">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("Equnr") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEqunr" runat="server" Text='<%# Bind("Equnr") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="MANDT" HeaderText="col_Anfordern">
                                                   <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Anfordern" runat="server" CommandName="Sort" CommandArgument="MANDT">col_Anfordern</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chk0000" runat="server"></asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="CHASSIS_NUM" HeaderText="col_Fahrgestellnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="CHASSIS_NUM">col_Fahrgestellnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CHASSIS_NUM") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="LICENSE_NUM" HeaderText="col_Kennzeichen">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="LICENSE_NUM">col_Kennzeichen</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LICENSE_NUM") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="LIZNR" HeaderText="col_Leasingvertragsnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Leasingvertragsnummer" runat="server" CommandName="Sort"
                                                            CommandArgument="LIZNR">col_Leasingvertragsnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LIZNR") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="TIDNR" HeaderText="col_NummerZB2">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_NummerZB2" runat="server" CommandName="Sort" CommandArgument="TIDNR">col_NummerZB2</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TIDNR") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="ZZREFERENZ1" HeaderText="col_Ordernummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Ordernummer" runat="server" CommandName="Sort" CommandArgument="ZZREFERENZ1">col_Ordernummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label5" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZREFERENZ1") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="STATUS" HeaderText="col_Status">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Status" runat="server" CommandName="Sort" CommandArgument="STATUS">col_Status</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label6" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.STATUS") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div id="dataFooter">
                            <asp:LinkButton ID="cmdSave" runat="server" CssClass="Tablebutton" Width="78px"> » Weiter</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
