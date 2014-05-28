<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change03_2.aspx.vb" Inherits="AppF2.Change03_2"
    MasterPageFile="../MasterPage/AppMaster.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
<%--                    <asp:HyperLink ID="lnkKreditlimit" runat="server" NavigateUrl="Change03.aspx">Händlersuche</asp:HyperLink>
--%>                    <asp:LinkButton ID="lbHaendlersuche" runat="server">Händlerauswahl</asp:LinkButton>
                    &nbsp;</div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                        </div>
                        <uc1:Kopfdaten ID="Kopfdaten1" runat="server"></uc1:Kopfdaten>
                        <div id="TableQuery">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
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
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="Result" runat="Server">
                            <div id="pagination">
                                <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                            </div>
                            <div id="data">
                                <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False"
                                    CellPadding="0" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                    AllowSorting="True" AllowPaging="True" CssClass="GridView" PageSize="20">
                                    <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                    <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                    <PagerSettings Visible="False" />
                                    <RowStyle CssClass="ItemStyle" />
                                    <EmptyDataRowStyle BackColor="#DFDFDF" />
                                    <Columns>
                                         <asp:TemplateField>
                                            <ItemTemplate>
                                                <p align="center">
                                                   <asp:ImageButton ID="lbStorno" runat="server" Height="20px" ImageUrl="../../../Images/Papierkorb_01.gif"
                                                        Width="20px" CommandName="Storno" 
                                                        CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>' ToolTip="Stornieren" />
                                                    <ajaxToolkit:ConfirmButtonExtender ID="lbStorno_ConfirmButtonExtender" 
                                                        runat="server" ConfirmText="" Enabled="True" TargetControlID="lbStorno">
                                                    </ajaxToolkit:ConfirmButtonExtender>
                                                </p>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="col_Haendlernummer">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Haendlernummer" runat="server" CommandName="Sort" CommandArgument="Haendlernummer">col_Haendlernummer</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Labelxaq1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Haendlernummer") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="col_Adresse">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Adresse" runat="server" CommandName="Sort" CommandArgument="Adresse">col_Adresse</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label21" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Adresse") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="col_Vertragsnummer">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="Vertragsnummer">col_Vertragsnummer</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Labelx1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Vertragsnummer") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="col_Anforderungsdatum">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Anforderungsdatum" runat="server" CommandName="Sort" CommandArgument="Anforderungsdatum">col_Anforderungsdatum</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Labely1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Anforderungsdatum", "{0:d}") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="col_Fahrgestellnummer">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Labelq1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="col_Briefnummer">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Briefnummer" runat="server" CommandName="Sort" CommandArgument="Briefnummer">col_Briefnummer</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Labelxc1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Briefnummer") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="col_Kontingentart">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Kontingentart" runat="server" CommandName="Sort" CommandArgument="Kontingentart">col_Kontingentart</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Labelc1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kontingentart") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="gesperrt" HeaderText="col_gesperrt" HeaderStyle-Width="100px">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_gesperrt" CommandArgument="gesperrt" CommandName="Sort" runat="server">col_gesperrt</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <p align="center">
                                                    <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.gesperrt") %>'>
                                                    </asp:Label>
                                                </p>
                                            </ItemTemplate>
                                            <HeaderStyle Width="50px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="False">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("VBELN") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("VBELN") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="0px" />
                                            <ItemStyle Width="0px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="False">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("EQUNR") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("EQUNR") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="0px" />
                                            <ItemStyle Width="0px" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <asp:Timer ID="timerHidePopup" runat="server" Interval="2000" Enabled="false"></asp:Timer>
                        <asp:Button ID="btnFake" runat="server" Text="Fake" Style="display: none" />
                        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnFake"
                            PopupControlID="mb" BackgroundCssClass="modalBackground" DropShadow="false">
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Panel ID="mb" runat="server" BackColor="White" Width="300" Height="36"
                            Style="display: none; border: solid 2px #000000">
                            <div style="padding-top: 10px; padding-bottom: 10px; text-align: center">
                                <%--Meldungstext kann per Feldübersetzung individuell angepasst werden--%>
                                <asp:Label ID="lbl_AuftragStornoErfolgreichMessage" runat="server" Text="Versandauftrag erfolgreich gelöscht" Font-Bold="True"></asp:Label>
                            </div>
                        </asp:Panel>
                        <div id="dataFooter">
                            &nbsp;
                        </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
