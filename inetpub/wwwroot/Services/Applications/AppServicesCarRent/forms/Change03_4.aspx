<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change03_4.aspx.vb" Inherits="AppServicesCarRent.Change03_4"     MasterPageFile="../MasterPage/App.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:HyperLink ID="lnkFahrzeugsuche" runat="server" NavigateUrl="Change02.aspx">Fahrzeugsuche</asp:HyperLink>&nbsp;<asp:HyperLink
                    ID="lnkFahrzeugAuswahl" runat="server" NavigateUrl="Change02_2.aspx">| Fahrzeugauswahl</asp:HyperLink>
                <asp:HyperLink ID="lnkAdressAuswahl" runat="server" NavigateUrl="Change02_3.aspx"> | Adressauswahl</asp:HyperLink>
                <a class="active">|
                    <asp:Label ID="lblAddress" runat="server" Visible="False"></asp:Label><asp:Label
                        ID="lblMaterialNummer" runat="server" Visible="False"></asp:Label></a>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <div id="TableQuery">
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0" width="100%" style="width: 100%">
                            <tr id="tr_Message" runat="server" class="formquery">
                                <td colspan="2" class="firstLeft active">
                                    <p>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Black" Font-Bold="True"></asp:Label>
                                        <asp:Label ID="lblerror" runat="server" CssClass="TextError"></asp:Label></p>
                                </td>
                            </tr>
                            <tr id="tr_VersandAdresse" runat="server" class="formquery">
                                <td class="firstLeft active" style="vertical-align:top">
                                    <asp:Label ID="lbl_VersandAdresse" runat="server">lbl_VersandAdresse</asp:Label>
                                </td>
                                <td class="active" style="width: 100%">
                                    <asp:Label ID="lblXVersand" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="formquery" id="tr_Versandart" runat="server">
                                <td class="firstLeft active">
                                    <asp:Label ID="lbl_Versandart" runat="server">lbl_Versandart</asp:Label>
                                </td>
                                <td class="active">
                                    <asp:Label ID="lblXVersandartData" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="formquery" id="tr_Versandgrund" runat="server">
                                <td class="firstLeft active">
                                    <asp:Label ID="lbl_Versandgrund" runat="server">lbl_Versandgrund</asp:Label>
                                </td>
                                <td class="active">
                                    <asp:Label ID="lblXVersandGrundData" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td colspan="2" style="width: 100%">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="ExcelDiv" id="ExcelDiv" runat="server" visible="false">
                        <div align="right" class="rightPadding">
                            <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                            <span class="ExcelSpan">
                                <asp:LinkButton ID="lnkCreateExcel1" runat="server">Excel herunterladen</asp:LinkButton>
                            </span>
                        </div>
                    </div>
                    <div id="data">
                        <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False"
                            CellPadding="1" CellSpacing="1" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                            AllowSorting="True" AllowPaging="True" CssClass="GridView" PageSize="20">
                            <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                            <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                            <PagerSettings Visible="False" />
                            <RowStyle CssClass="ItemStyle" />
                            <Columns>
                                <asp:BoundField Visible="False" DataField="MANDT" SortExpression="MANDT" HeaderText="MANDT">
                                </asp:BoundField>
                                <asp:TemplateField SortExpression="ZZREFERENZ1" HeaderText="col_Ordernummer">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="col_Ordernummer" runat="server" CommandName="Sort" CommandArgument="ZZREFERENZ1">col_Ordernummer</asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label5" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZREFERENZ1") %>'>
                                        </asp:Label>
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

                                <asp:TemplateField SortExpression="STATUS" HeaderText="col_Status">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="col_Status" runat="server" CommandName="Sort" CommandArgument="STATUS">col_Status</asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label6" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.STATUS") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="False" HeaderText="Anfordern">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk0001" runat="server" Enabled="False"></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div id="dataFooter">
                        <asp:LinkButton ID="cmdSearch" runat="server" CssClass="TablebuttonLarge" Width="130px"
                            Height="16px">» Weiter</asp:LinkButton></div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
